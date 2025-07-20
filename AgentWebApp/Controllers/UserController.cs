using AutoMapper;
using Ioc.Core;
using Ioc.Core.DbModel.Models;
using Ioc.ObjModels.Model;
using Ioc.ObjModels.Model.DataTableModel;
using Ioc.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AgentWebApp.Controllers
{
    public class UserController : Controller
    {
        private IUserService userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;


        public UserController(IUserService userService, IMapper mapper, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.userService = userService;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;

        }

        public ActionResult Index()
        {
            /*UserModel users = userService.GetUsers().Select(u => new UserModel
            {
                FirstName = u.UserProfile.FirstName,
                LastName = u.UserProfile.LastName,
                Email = u.Email,
                Address = u.UserProfile.Address,
                ID = u.ID
            });*/
            return View(new UserModel());
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] DataTableParameters parameters)
        {
            /*try
            {
                // Total record count
                var totalRecords = await userService.GetCount();

                // Filter, sort and paginate data
                var pagedData = userService.GetAllWithInclude("UserProfile").Skip(start).Take(length).ToList();
                var mappedData = _mapper.Map<List<UserModelDt>>(pagedData);
                // Return the data in DataTables format
                return Json(new
                {
                    draw = draw,
                    recordsTotal = totalRecords,
                    recordsFiltered = totalRecords,
                    data = mappedData
                });
            }
            catch (Exception ex)
            {
            }
            return Json(new
            {
                message = "error"
            });*/

           
            var users = GetFilteredUsers(parameters);
            var response = new DataTableResponse
            {
                Draw = parameters.Draw,
                RecordsTotal = users.TotalCount,
                RecordsFiltered = users.FilteredCount,
                Data = users.Data.ToList(),
            };

            return Ok(response);
        }




        private FilteredUsers GetFilteredUsers(DataTableParameters parameters)
        {
            // Implement your filtering logic here, for example using Entity Framework
            // Example:

            var searchValue = parameters.Search;



            var query = userService.GetAllWithInclude("UserProfile").AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(u =>
                    u.UserProfile.FirstName.Contains(searchValue) ||
                    u.UserProfile.LastName.Contains(searchValue) ||
                    u.UserProfile.Address.Contains(searchValue) ||
                    u.UserName.Contains(searchValue) ||
                    u.Email.Contains(searchValue));
            }

            var totalRecords = query.Count();
            var filteredRecords = query.Count();
            var data = query.Skip(parameters.Start).Take(parameters.Length).ToList();
            var mappedData = _mapper.Map<List<UserModelDt>>(data);

            return new FilteredUsers
            {
                TotalCount = totalRecords,
                FilteredCount = filteredRecords,
                Data = mappedData
            };
        }



        public ActionResult Create()
        {
            /*UserModel users = userService.GetUsers().Select(u => new UserModel
            {
                FirstName = u.UserProfile.FirstName,
                LastName = u.UserProfile.LastName,
                Email = u.Email,
                Address = u.UserProfile.Address,
                ID = u.ID
            });*/
            return View(new UserModel());
        }

        [HttpGet]
        public async Task<ActionResult> CreateEditUser(Guid? id)
        {
            UserModel model = new UserModel();
            if (id.HasValue && id != null)
            {
                User userEntity = await userService.GetByIdWithInclude((Guid)id, "UserProfile");
                model.ID = userEntity.ID;
                model.FirstName = userEntity.UserProfile?.FirstName;
                model.LastName = userEntity.UserProfile?.LastName;
                model.Address = userEntity.UserProfile?.Address;
                model.Email = userEntity.Email;
                model.UserName = userEntity.UserName;
                model.Password = userEntity.Password;
            }
            return Json(new {data = model, success = true, message = "User fetched" });
        }

        [HttpPost]
        public async Task<IActionResult> CreateEditUserAPI([FromBody] UserModel model)
        {
            string returnMsg = string.Empty;

            try
            {
                if (model.ID == Guid.Empty)
                {
                    User userEntity = new User
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        Password = model.Password,
                        CreatedAt = DateTime.UtcNow,
                        ModifiedDate = DateTime.UtcNow,
                        UserProfile = new UserProfile
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Address = model.Address,
                            CreatedAt = DateTime.UtcNow,
                            ModifiedDate = DateTime.UtcNow,
                        }
                    };
                    await userService.InsertUser(userEntity);

                    returnMsg =  await SaveAndUpdateAspNetUser(model, 0);

                    return Json(new { success = true, message = "User created" , ApiMessage = returnMsg });
                }
                else
                {
                    User userEntity = userService.GetByIdWithInclude((Guid)model.ID, "UserProfile").Result;
                    userEntity.UserName = model.UserName;
                    userEntity.Email = model.Email;
                    userEntity.Password = model.Password;
                    userEntity.ModifiedDate = DateTime.UtcNow;
                    userEntity.UserProfile.FirstName = model.FirstName;
                    userEntity.UserProfile.LastName = model.LastName;
                    userEntity.UserProfile.Address = model.Address;
                    userEntity.UserProfile.ModifiedDate = DateTime.UtcNow;
                    await userService.UpdateUser(userEntity);

                    #region aspnet user creation 

                    returnMsg = await SaveAndUpdateAspNetUser(model, 1);


                    #endregion
                    return Json(new { success = true, message = "User updated", ApiMessage = returnMsg });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        private async Task<string> SaveAndUpdateAspNetUser(UserModel model, int typeOfOpe)
        {
            string returnMsg = string.Empty;

            try
            {
                var role = new IdentityRole();
                role.Name = "User";

                if (!await _roleManager.RoleExistsAsync(role.Name))
                {
                    await _roleManager.CreateAsync(role);
                }
                if (typeOfOpe == 0)
                {
                    var userExists = await _userManager.FindByNameAsync(model.UserName);
                    if (userExists != null)
                        model.UserName = model.UserName + new Random().NextInt64();

                    ApplicationUser user = new()
                    {
                        Email = model.Email,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = model.UserName
                    };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    await _userManager.AddToRoleAsync(user, role.Name);
                    if (!result.Succeeded)
                        returnMsg = "User creation failed! Please check user details and try again.";
                    else
                        returnMsg = "Login user created for this user";
                }
                else
                {
                    var userExists = await _userManager.FindByNameAsync(model.UserName);
                    if (userExists == null)
                    {
                        ApplicationUser user = new()
                        {
                            Email = model.Email,
                            SecurityStamp = Guid.NewGuid().ToString(),
                            UserName = model.UserName
                        };
                        var result = await _userManager.CreateAsync(user, model.Password);
                        await _userManager.AddToRoleAsync(user, role.Name);
                        if (!result.Succeeded)
                            returnMsg = "User creation failed! Please check user details and try again.";
                        else
                            returnMsg = "Login user created for this user";
                    }

                }
               

            }
            catch (Exception ex)
            {
                returnMsg = ex.Message;
            }
            return returnMsg;
        }
        public async Task<ActionResult> DetailUser(Guid? id)
        {
            UserModel model = new UserModel();
            if (id.HasValue)
            {
                User userEntity = await userService.GetUser(id.Value);
                // model.ID = userEntity.ID;  
                model.FirstName = userEntity.UserProfile.FirstName;
                model.LastName = userEntity.UserProfile.LastName;
                model.Address = userEntity.UserProfile.Address;
                model.Email = userEntity.Email;
                model.UserName = userEntity.UserName;
            }
            return View(model);
        }

        public async Task<ActionResult> DeleteUser(Guid id)
        {
            UserModel model = new UserModel();
            if (id != null)
            {
                User userEntity = await userService.GetUser(id);
                model.FirstName = userEntity.UserProfile.FirstName;
                model.LastName = userEntity.UserProfile.LastName;
                model.Address = userEntity.UserProfile.Address;
                model.Email = userEntity.Email;
                model.UserName = userEntity.UserName;
            }
            return Json(new {success = true, message = "User deleted" });
        }


        [HttpDelete]
        public async Task<ActionResult> DeleteUserAPI(Guid id)
        {
            try
            {
                if (id != null)
                {
                    await userService.DeleteUser(id);
                    return Json(new { success = true, message = "User deleted" });
                }
                return Json(new { success = true, message = "User not fapund" });
            }
            catch
            {
                return Json(new { success = true, message = "Error" });
            }
        }

        public ActionResult UserProfile()
        {
            /*UserModel users = userService.GetUsers().Select(u => new UserModel
            {
                FirstName = u.UserProfile.FirstName,
                LastName = u.UserProfile.LastName,
                Email = u.Email,
                Address = u.UserProfile.Address,
                ID = u.ID
            });*/
            return View(new UserModel());
        }
    }
}

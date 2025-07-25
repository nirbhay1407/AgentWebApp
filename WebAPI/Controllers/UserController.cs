using Ioc.Core.DbModel.Models;
using Ioc.ObjModels.Model;
using Ioc.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebAPI.FakerData;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private IUserService userService;
     

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        [HttpGet]
        public ActionResult Get()
        {

            return Ok(userService.GetAll());
        }

        /*[HttpGet]
        public async Task<ActionResult> CreateEditUser(Guid? id)
        {
            UserModel model = new UserModel();
            if (id.HasValue && id != null)
            {
                User userEntity = await userService.GetUser((Guid)id);
                model.FirstName = userEntity.UserProfile.FirstName;
                model.LastName = userEntity.UserProfile.LastName;
                model.Address = userEntity.UserProfile.Address;
                model.Email = userEntity.Email;
                model.UserName = userEntity.UserName;
                model.Password = userEntity.Password;
            }
            return Ok(model);
        }*/

        [HttpPost]
        public async Task<ActionResult> CreateEditUser(UserModel? model)
        {
            User? userEntity = await userService.GetUser((Guid)model.ID);

            if (userEntity == null)
            {
                UserProfile UserProfile = new UserProfile
                {
                    FirstName = model?.FirstName,
                    LastName = model?.LastName,
                    Address = model?.Address,
                    //AddedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                };
                User userEntityD = new User
                {
                    UserName = model?.UserName,
                    Email = model?.Email,
                    Password = model?.Password,
                    //AddedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    // UserProfileId = UserProfile.ID
                };
                await userService.InsertUser(userEntityD);
                return RedirectToAction("index");
            }
            else
            {
                userEntity.UserName = model.UserName;
                userEntity.Email = model.Email;
                userEntity.Password = model.Password;
                userEntity.ModifiedDate = DateTime.UtcNow;
                userEntity.UserProfile.FirstName = model.FirstName;
                userEntity.UserProfile.LastName = model.LastName;
                userEntity.UserProfile.Address = model.Address;
                userEntity.UserProfile.ModifiedDate = DateTime.UtcNow;
                await userService.UpdateUser(userEntity);
                return RedirectToAction("index");

            }
            return Ok(model);
        }
        [HttpGet("DetailUser")]
        public async Task<ActionResult> DetailUser(Guid? id)
        {
            UserModel model = new UserModel();
            if (id.HasValue)
            {
                User userEntity = await userService.GetUser(id.Value);
                // model.ID = userEntity.ID;  
                model.FirstName = userEntity?.UserProfile?.FirstName;
                model.LastName = userEntity?.UserProfile?.LastName;
                model.Address = userEntity?.UserProfile?.Address;
                model.Email = userEntity?.Email;
                model.UserName = userEntity?.UserName;
            }
            return Ok(model);
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteUser(Guid? id)
        {
            UserModel model = new UserModel();
            if (id != null)
            {
                User userEntity = await userService.GetUser((Guid)id);
                model.FirstName = userEntity?.UserProfile?.FirstName;
                model.LastName = userEntity?.UserProfile?.LastName;
                model.Address = userEntity?.UserProfile?.Address;
                model.Email = userEntity?.Email;
                model.UserName = userEntity?.UserName;
            }
            return Ok(model);
        }


        [HttpDelete("DeleteUser")]
        public async Task<ActionResult> DeleteUser(Guid? id, FormCollection collection)
        {
            try
            {
                if (id != null)
                {
                    await userService.DeleteUser((Guid)id);
                    return RedirectToAction("Index");
                }
                return Ok();
            }
            catch
            {
                return Ok();
            }
        }

        [HttpGet("SaveFakeDataUser/{howMuch}")]
        public async Task<IActionResult> SaveFakeDataUser(int howMuch)
        {
            var dataToCreate = FakerDataClass.GetFakeUser(howMuch);
            await userService.CreateInRange(dataToCreate);
            return Ok($"{howMuch} TIMES DATA IS CREATED");
        }

        
    }
}

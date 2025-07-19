using AutoMapper;
using Ioc.Core.DbModel.Models;
using Ioc.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Xml;
using WebAPI.Models;
using Newtonsoft.Json;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoryService _repository;
        private readonly IMapper _mapper;

        public SubCategoryController(ISubCategoryService repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = _repository.GetAllWithInclude("Category");
            var result = _mapper.Map<IEnumerable<SubCategoryModel>>(data);

            // To convert an XML node contained in string xml into a JSON string   
           /* XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            string jsonText = JsonConvert.SerializeXmlNode(doc);*/

            // To convert JSON text contained in string json into an XML node
            XmlDocument doc = JsonConvert.DeserializeXmlNode(JsonConvert.SerializeObject(result));


            return Ok(result);
        }

        [HttpGet("getwi")]
        public async Task<IActionResult> GetWi()
        {
            var data = _repository.GetAll();
            var result = _mapper.Map<IEnumerable<SubCategoryModel>>(data);
            return Ok(result);
        }

        [HttpGet("GetSubCategory/{id}")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var data = await _repository.GetByIdWithInclude(id, "Category");
            var result = _mapper.Map<SubCategoryModel>(data);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Save(SubCategoryModel model)
        {

            var data = _mapper.Map<SubCategory>(model);
            await _repository.Create(data);
            var returnD = _mapper.Map<SubCategoryModel>(data);
            return Ok(returnD);
        }

        [HttpDelete]
        public async Task<IActionResult> delete(Guid id)
        {
            await _repository.Delete(id);
            return NoContent();
        }
    }
}

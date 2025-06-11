using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prj2Services.Services;
using Project2Database.Models;

namespace Project2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Prj2Api : ControllerBase
    {
        private readonly BlogServices _blogServices;
        public Prj2Api(BlogServices blogServices)
        {
            _blogServices = blogServices;

        }

        [HttpGet]
        public IActionResult GetDream()
        {
            var result = _blogServices.GetDream();
            return Ok(result);
        }

        [HttpGet("{id}")]

        public IActionResult GetDream(int id)
        {
            var result = _blogServices.GetDream(id);
            return Ok(result);
        }



        //[HttpPost]
        //public IActionResult Post([FromBody] TblBlogDetail detail)
        //{
        //    var res = _blogServices.PostBlogDetail(detail);
        //    return Ok(res);
        //}
    }
}

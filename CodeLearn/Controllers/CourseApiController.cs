using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CodeLearn.DataLayer.Context;

namespace CodeLearn.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseApiController : ControllerBase
    {
        private CodeLearnContext _context;

        public CourseApiController(CodeLearnContext context)
        {
            _context = context;
        }

        [Produces("application/json")]
        [HttpGet("search")]
        public async Task<IActionResult> Search()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var courseTitle = _context.Courses
                    .Where(c => c.CourseTitle.Contains(term))
                    .Select(c => c.CourseTitle)
                    .ToList();
                return Ok(courseTitle);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}

using CodeLearn.Core.Services.Interfaces;
using CodeLearn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeLearn.Web.Pages.Admin.Courses
{
    public class CreateEpisodeModel : PageModel
    {
        private ICourseService _courseService;
        public CreateEpisodeModel(ICourseService courseService)
        {
            _courseService = courseService;
        }


        [BindProperty] //tavane estefade az in property dar view ra midahad
        public CourseEpisode CourseEpisode { get; set; }
        public void OnGet(int id)
        {
            CourseEpisode = new CourseEpisode();
            CourseEpisode.CourseId = id;
        }

        public IActionResult OnPost(IFormFile fileEpisode)
        {
            ModelState.Clear();
            if(!ModelState.IsValid || fileEpisode==null)
                return Page();
            if(_courseService.ChecExistFile(fileEpisode.FileName))
            {
                ViewData["IsExsistFile"] = true;
                return Page();
            }

            _courseService.AddEpisode(CourseEpisode, fileEpisode);
            return Redirect("/Admin/Courses/IndexEpisode/" + CourseEpisode.CourseId);
        }
    }
}

using CodeLearn.Core.Services.Interfaces;
using CodeLearn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeLearn.Web.Pages.Admin.Courses
{
    public class EditEpisodeModel : PageModel
    {
        private ICourseService _courseService;
        public EditEpisodeModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
        public CourseEpisode CourseEpisode { get; set; }
        public void OnGet(int id)
        {
            CourseEpisode = _courseService.GetEpisodeById(id);

        }

        public IActionResult OnPost(IFormFile fileEpisode)
        {
            ModelState.Clear();
            if (!ModelState.IsValid || fileEpisode == null)
                return Page();
            if(fileEpisode != null)
            {
                if (_courseService.ChecExistFile(fileEpisode.FileName))
                {
                    ViewData["IsExsistFile"] = true;
                    return Page();
                }
            }

            _courseService.EditEpisode(CourseEpisode, fileEpisode);
            return Redirect("/Admin/Courses/IndexEpisode/" + CourseEpisode.CourseId);
        }
    }
}

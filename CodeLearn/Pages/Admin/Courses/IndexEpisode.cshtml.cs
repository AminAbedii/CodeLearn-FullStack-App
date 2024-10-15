using CodeLearn.Core.Services.Interfaces;
using CodeLearn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeLearn.Web.Pages.Admin.Courses
{
    public class IndexEpisodeModel : PageModel
    {
        private ICourseService _courseService;
        public IndexEpisodeModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public List<CourseEpisode> ListEpisode { get; set; }
        public void OnGet(int id)
        {
            ViewData["CourseId"] = id;

            ListEpisode = _courseService.GetCourseEpisodes(id);
        }
    }
}

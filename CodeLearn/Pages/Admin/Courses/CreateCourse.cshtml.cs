using CodeLearn.Core.Services.Interfaces;
using CodeLearn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CodeLearn.Web.Pages.Admin.Courses
{
    public class CreateCourseModel : PageModel
    {
        private ICourseService _courseService;
        public CreateCourseModel(ICourseService courseService)
        {
            _courseService = courseService;
        }


        [BindProperty]
        public Course course { get; set; } //chon aksare etelaate course ro mixaym ps niyazi be ViewModel nist

        public void OnGet()
        {
            var groups = _courseService.GetGroupForManageCourse();
            ViewData["Groups"] = new SelectList(groups, "Value", "Text");

            var subGroups = _courseService.GetSubGroupForManageCourse(int.Parse(groups.First().Value));
            ViewData["SubGroups"] = new SelectList(subGroups, "Value", "Text");

            var teachers = _courseService.GetTeachers();
            ViewData["Teachers"] = new SelectList(teachers, "Value", "Text");

            var levels = _courseService.GetLevels();
            ViewData["Levels"] = new SelectList(levels, "Value", "Text");

            var statues = _courseService.GetStatues();
            ViewData["Statues"] = new SelectList(statues, "Value", "Text");
        }

        public IActionResult OnPost(IFormFile imgCourseUp,IFormFile demoUp)     //IFormFile baraye upload kardane file(Aks ya Video) kaarbord darad
        {
            ModelState.Clear();
            if(!ModelState.IsValid)
                return Page();
            _courseService.AddCourse(course, imgCourseUp, demoUp);

            return RedirectToPage("Index");
        }
    }
}

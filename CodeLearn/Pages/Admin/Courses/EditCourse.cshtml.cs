using CodeLearn.Core.Services.Interfaces;
using CodeLearn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CodeLearn.Web.Pages.Admin.Courses
{
    public class EditCourseModel : PageModel
    {
        private ICourseService _courseService;
        public EditCourseModel(ICourseService courseService)
        {
            _courseService = courseService;
        }


        [BindProperty]
        public Course course { get; set; } //chon aksare etelaate course ro mixaym ps niyazi be ViewModel nist
        public void OnGet(int id)
        {
            course = _courseService.GetCourseById(id);

            var groups = _courseService.GetGroupForManageCourse();
            ViewData["Groups"] = new SelectList(groups, "Value", "Text", course.GroupId);

            var subGroups = _courseService.GetSubGroupForManageCourse(int.Parse(groups.First().Value));
            ViewData["SubGroups"] = new SelectList(subGroups, "Value", "Text",course.SubGroup??0);

            var teachers = _courseService.GetTeachers();
            ViewData["Teachers"] = new SelectList(teachers, "Value", "Text",course.TeacherId);

            var levels = _courseService.GetLevels();
            ViewData["Levels"] = new SelectList(levels, "Value", "Text",course.LevelId);

            var statues = _courseService.GetStatues();
            ViewData["Statues"] = new SelectList(statues, "Value", "Text",course.StatusId);
        }

        public IActionResult OnPost(IFormFile imgCourseUp, IFormFile demoUp)
        {
            if (!ModelState.IsValid)
                return Page();

            _courseService.UpdateCourse(course, imgCourseUp, demoUp);

            return RedirectToPage("Index");
        }
    }
}

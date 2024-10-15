using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLearn.Core.DTOs.Course
{
    public class ShowCourseForAdminViewModel
    {
        public int CourseID { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public int EpisoteCounte { get; set; }
    }
}

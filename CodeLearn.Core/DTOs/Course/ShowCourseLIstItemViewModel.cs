using CodeLearn.DataLayer.Entities.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLearn.Core.DTOs.Course
{
    public class ShowCourseLIstItemViewModel
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public int Price { get; set; }
        public TimeSpan TotalTime { get; set; }
        public List<CourseEpisode> CourseEpisodes { get; set; }
    }
}

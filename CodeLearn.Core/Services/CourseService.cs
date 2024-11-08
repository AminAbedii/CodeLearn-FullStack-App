using CodeLearn.Core.Convertors;
using CodeLearn.Core.DTOs.Course;
using CodeLearn.Core.Generator;
using CodeLearn.Core.Security;
using CodeLearn.Core.Services.Interfaces;
using CodeLearn.DataLayer.Context;
using CodeLearn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CodeLearn.Core.Services
{
    public class CourseService : ICourseService
    {
        private CodeLearnContext _context;
        public CourseService(CodeLearnContext context)
        {
            _context = context;
        }

        public void AddComment(CourseComment comment)
        {
            _context.CourseComments.Add(comment);
            _context.SaveChanges();
        }

        public int AddCourse(Course course,IFormFile imgCourse, IFormFile courseDemo)
        {
            course.CreateDate = DateTime.Now;
            course.CourseImageName = "no-photo.jpg";
            //TODO Image Check
            if (imgCourse != null && imgCourse.IsImage())
            {
                course.CourseImageName= NameGenerator.GeneratorUniqCode() + Path.GetExtension(imgCourse.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Course/image", course.CourseImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imgCourse.CopyTo(stream);
                }

                //TODO Image ReSize
                ImageConvertor imgResize = new ImageConvertor();
                string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Course/thumb", course.CourseImageName);
                imgResize.Image_resize(imagePath,thumbPath,150);
            }
            //TODO Upload Demo
            if(courseDemo != null)
            {
                course.DemoFileName = NameGenerator.GeneratorUniqCode() + Path.GetExtension(courseDemo.FileName);
                string demoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Course/demoes", course.DemoFileName);
                using (var stream = new FileStream(demoPath, FileMode.Create))
                {
                    courseDemo.CopyTo(stream);
                }
            }

            _context.Add(course);
            _context.SaveChanges();

            return course.CourseId;
        }

        public int AddEpisode(CourseEpisode episode ,IFormFile episodeFile)
        {
            episode.EpisodeFileName = episodeFile.FileName;
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseFiles", episode.EpisodeFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                episodeFile.CopyTo(stream);
            }


            _context.CourseEpisodes.Add(episode);
            _context.SaveChanges();
            return episode.EpisodeId;
        }

        public void AddGroup(CourseGroup group)
        {
            _context.CourseGroups.Add(group);
            _context.SaveChanges();
        }

        public void AddsVote(int userId, int courseId, bool vote)
        {
            var UserVote = _context.CourseVotes.FirstOrDefault(c => c.UserId == userId && c.CourseId == courseId);
            if (UserVote != null)
            {
                UserVote.Vote = vote;
            }
            else
            {
                UserVote = new CourseVote()
                {
                    CourseId = courseId,
                    UserId = userId,
                    Vote = vote
                };
                _context.Add(UserVote);
            }

            _context.SaveChanges();
        }

        public bool ChecExistFile(string fileName)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseFiles", fileName);
            return File.Exists(path);
        }

        public void EditEpisode(CourseEpisode episode, IFormFile episodeFile)
        {
            if(episodeFile !=null)
            {
                string DeleteFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseFiles", episode.EpisodeFileName);
                File.Delete(DeleteFilePath);


                episode.EpisodeFileName = episodeFile.FileName;
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseFiles", episode.EpisodeFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    episodeFile.CopyTo(stream);
                }
            }
            _context.CourseEpisodes.Update(episode);
            _context.SaveChanges();
        }

        public List<CourseGroup> GetAllGroup()
        {
            return _context.CourseGroups.ToList();
        }

        public CourseGroup GetById(int groupId)
        {
            return _context.CourseGroups.Find(groupId);
        }

        public List<ShowCourseLIstItemViewModel> GetCourse(int pageId = 1, string filter = "", string getType = "all", string orderByType = "date",
            int startPrice = 0, int endPrice = 0, List<int> selectedGroups = null, int take = 0)
        {
            if (take == 0)
                take = 8;

            IQueryable<Course> result = _context.Courses;

            if (!string.IsNullOrEmpty(filter))
            {
                result = result.Where(c => c.CourseTitle.Contains(filter)||c.Tags.Contains(filter));
            }

            switch(getType)
            {
                case "all":
                    break;
                case "buy":
                    {
                        result = result.Where(c => c.CoursePrice != 0);
                        break;
                    }
                case "free":
                    {
                        result = result.Where(c => c.CoursePrice == 0);
                        break;
                    }
            }

            switch(orderByType)
            {
                case "date":
                    {
                        result=result.OrderByDescending(c=>c.CreateDate); 
                        break;
                    }
                case "updateDate":
                    {
                        result=result.OrderByDescending(c=>c.UpdateDate); 
                        break;
                    }
            }

            if (startPrice > 0)
            {
                result = result.Where(c => c.CoursePrice > startPrice);
            }
            if (endPrice > 0)
            {
                result = result.Where(c => c.CoursePrice < endPrice);
            }

            if(selectedGroups!=null && selectedGroups.Any())
            {
                foreach(int groupId in selectedGroups)
                {
                    result = result.Where(c => c.GroupId == groupId || c.SubGroup == groupId);
                }
            }

            int skip = (pageId - 1) * take;
            //int pageCount = result.Include(c => c.CourseEpisodes)
            //    .ToList() // Retrieve the data from the database
            //    .Select(c => new ShowCourseLIstItemViewModel()
            //    {
            //        CourseId = c.CourseId,
            //        ImageName = c.CourseImageName,
            //        Price = c.CoursePrice,
            //        Title = c.CourseTitle,
            //        TotalTime = new TimeSpan(c.CourseEpisodes.Sum(e => e.EpisodeTime.Ticks))
            //    }).Count() / take;
            var courses = result.Include(c => c.CourseEpisodes)
                .ToList() // Retrieve the data from the database
                .Select(c => new ShowCourseLIstItemViewModel()
                {
                    CourseId = c.CourseId,
                    ImageName = c.CourseImageName,
                    Price = c.CoursePrice,
                    Title = c.CourseTitle,
                    TotalTime = new TimeSpan(c.CourseEpisodes.Sum(e => e.EpisodeTime.Ticks))
                })
                .ToList();

            var paginatedCourses = courses.Skip(skip).Take(take).ToList();
            return paginatedCourses;

        }

        public Course GetCourseById(int courseId)
        {
            return _context.Courses.Find(courseId);
        }

        public Tuple<List<CourseComment>, int> GetCourseComment(int courseId, int pageId = 1)
        {
            int take = 5;
            int skip = (pageId - 1) * take;
            int pageCount = _context.CourseComments.Where(c => !c.IsDelete && c.CourseId == courseId).Count() / take;

            if ((pageCount % 2) != 0)
            {
                pageCount += 1;
            }

            return Tuple.Create(
                _context.CourseComments.Include(c => c.User).Where(c => !c.IsDelete && c.CourseId == courseId).Skip(skip).Take(take)
                    .OrderByDescending(c => c.CreateDate).ToList(), pageCount);
        }

        public List<CourseEpisode> GetCourseEpisodes(int courseId)
        {
            return _context.CourseEpisodes.Where(e=>e.CourseId == courseId).ToList();
        }

        public Course GetCourseForShow(int courseId)
        {
            return _context.Courses.Include(c => c.CourseEpisodes)
                .Include(c => c.CourseLevel)
                .Include(c => c.User)
                .Include(c => c.CourseStatus)
                .FirstOrDefault(c => c.CourseId == courseId);
        }

        public List<ShowCourseForAdminViewModel> GetCoursesForAdmin()
        {
            return _context.Courses.Select(c=> new ShowCourseForAdminViewModel()
            {
                CourseID=c.CourseId,
                EpisoteCounte=c.CourseEpisodes.Count,
                ImageName=c.CourseImageName,
                Title=c.CourseTitle,

            }).ToList();
            
        }

        public Tuple<int, int> GetCourseVotes(int courseId)
        {
            var votes = _context.CourseVotes.Where(v => v.CourseId == courseId).Select(v => v.Vote).ToList();

            return Tuple.Create(votes.Count(c => c), votes.Count(c => !c));

        }

        public CourseEpisode GetEpisodeById(int episodeId)
        {
            return _context.CourseEpisodes.Find(episodeId);
        }

        public List<SelectListItem> GetGroupForManageCourse()
        {
            return _context.CourseGroups.Where(g => g.ParentId == null)
                .Select(g => new SelectListItem()
                {
                    Text = g.GroupTitle,  //vurudi
                    Value = g.GroupId.ToString()  //xuruji
                }).ToList();
        }

        public List<SelectListItem> GetLevels()
        {
            return _context.CourseLevels.Select(s => new SelectListItem()
            {
                Value = s.LevelId.ToString(),
                Text = s.LevelTitle
            }).ToList();
        }

        public List<ShowCourseLIstItemViewModel> GetPopularCourse()
        {
            return _context.Courses.Include(c => c.OrderDetails)
                .Include(c => c.CourseEpisodes)
                .Where(c => c.OrderDetails.Any())
                .OrderByDescending(d => d.OrderDetails.Count)
                .Take(8)
                .Select(c => new ShowCourseLIstItemViewModel()
                {
                    CourseId = c.CourseId,
                    ImageName = c.CourseImageName,
                    Price = c.CoursePrice,
                    Title = c.CourseTitle,
                    CourseEpisodes = c.CourseEpisodes
                })
                .ToList();
        }

        public List<SelectListItem> GetStatues()
        {
            return _context.CourseStatuses.Select(l => new SelectListItem()
            {
                Value = l.StatusId.ToString(),
                Text = l.StatusTitle
            }).ToList();
        }

        public List<SelectListItem> GetSubGroupForManageCourse(int groupId)
        {
            return _context.CourseGroups.Where(g => g.ParentId == groupId)
                .Select(g => new SelectListItem()
                {
                    Text = g.GroupTitle,  //vurudi
                    Value = g.GroupId.ToString()  //xuruji
                }).ToList();
        }

        public List<SelectListItem> GetTeachers()
        {
            return _context.UserRoles.Where(r => r.RoleId == 2).Include(r=>r.User)
                .Select(u => new SelectListItem()
                {
                    Value = u.UserId.ToString(),
                    Text = u.User.UserName
                }).ToList();
        }

        public bool IsFree(int courseId)
        {
            return _context.Courses.Where(c => c.CourseId == courseId).Select(c => c.CoursePrice).First() == 0;
        }

        public void UpdateCourse(Course course, IFormFile imgCourse, IFormFile courseDemo)
        {
            course.UpdateDate = DateTime.Now;
            if (imgCourse != null && imgCourse.IsImage())
            {
                if (course.CourseImageName != "no-photo.jpg")
                {
                    string deleteimagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/image", course.CourseImageName);
                    if (File.Exists(deleteimagePath))
                    {
                        File.Delete(deleteimagePath);
                    }

                    string deletethumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/thumb", course.CourseImageName);
                    if (File.Exists(deletethumbPath))
                    {
                        File.Delete(deletethumbPath);
                    }
                }
                course.CourseImageName = NameGenerator.GeneratorUniqCode() + Path.GetExtension(imgCourse.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/image", course.CourseImageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imgCourse.CopyTo(stream);
                }

                ImageConvertor imgResizer = new ImageConvertor();
                string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/thumb", course.CourseImageName);

                imgResizer.Image_resize(imagePath, thumbPath, 150);
            }

            if (courseDemo != null)
            {
                if (course.DemoFileName != null)
                {
                    string deleteDemoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/demoes", course.DemoFileName);
                    if (File.Exists(deleteDemoPath))
                    {
                        File.Delete(deleteDemoPath);
                    }
                }
                course.DemoFileName = NameGenerator.GeneratorUniqCode() + Path.GetExtension(courseDemo.FileName);
                string demoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/demoes", course.DemoFileName);
                using (var stream = new FileStream(demoPath, FileMode.Create))
                {
                    courseDemo.CopyTo(stream);
                }
            }

            _context.Courses.Update(course);
            _context.SaveChanges();
        }

        public void UpdateGroup(CourseGroup @group)
        {
            _context.CourseGroups.Update(group);
            _context.SaveChanges();
        }
    }
}


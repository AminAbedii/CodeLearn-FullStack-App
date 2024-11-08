﻿using CodeLearn.Core.DTOs.Course;
using CodeLearn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLearn.Core.Services.Interfaces
{
    public interface ICourseService
    {
        #region Group
        List<CourseGroup> GetAllGroup();
        List<SelectListItem> GetGroupForManageCourse();
        List<SelectListItem> GetSubGroupForManageCourse(int groupId);
        List<SelectListItem> GetTeachers();
        List<SelectListItem> GetLevels();
        List<SelectListItem> GetStatues();



        CourseGroup GetById(int groupId);
        void AddGroup(CourseGroup group);
        void UpdateGroup(CourseGroup group);
        #endregion

        #region Course
        List<ShowCourseForAdminViewModel> GetCoursesForAdmin();
        int AddCourse(Course course,IFormFile imgCourse,IFormFile courseDemo);
        Course GetCourseById(int courseId);
        void UpdateCourse(Course course, IFormFile imgCourse, IFormFile courseDemo);

        List<ShowCourseLIstItemViewModel> GetCourse(int pageId = 1, string filter = "", string getType = "all",
            string orderByType = "date", int startPrice = 0, int endPrice = 0, List<int> selectedGroups = null, int take=0);

        Course GetCourseForShow(int courseId);


        List<ShowCourseLIstItemViewModel> GetPopularCourse();
        bool IsFree(int courseId);
        #endregion

        #region Episode
        List<CourseEpisode> GetCourseEpisodes(int courseId);
        bool ChecExistFile(string fileName);
        int AddEpisode(CourseEpisode episode, IFormFile episodeFile);
        CourseEpisode GetEpisodeById(int episodeId);
        void EditEpisode(CourseEpisode episode, IFormFile episodeFile);
        #endregion

        #region Comments
        void AddComment(CourseComment comment);
        Tuple<List<CourseComment>, int> GetCourseComment(int courseId, int pageId = 1);

        #endregion

        #region Course Vote

        void AddsVote(int userId, int courseId, bool vote);
        Tuple<int, int> GetCourseVotes(int courseId);

        #endregion
    }
}

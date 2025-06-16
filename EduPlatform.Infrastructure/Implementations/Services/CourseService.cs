using AutoMapper;
using EduPlatform.Application.DTOs.Course;
using EduPlatform.Application.Interfaces.Repositories;
using EduPlatform.Application.Interfaces.Services;
using EduPlatform.Domain.Entities;

namespace EduPlatform.Infrastructure.Implementations.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork  _unitOfWork;
        private readonly IMapper _mapper;
        public CourseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
       
        public async Task<IReadOnlyList<CourseDTO>> GetAllCoursesAsync(int? categoryId = null)
        {
            var courses =  await _unitOfWork.CourseRepository.GetAllCoursesAsync(categoryId);

            if (!courses.Any())
            {
                return new List<CourseDTO>();
            }

            return courses;
        }

        public async Task<CourseDetailDTO> GetCourseDetailsByIdAsync(int courseId)
        {
            if (courseId <= 0)
            {
                throw new ArgumentException("Invalid course ID", nameof(courseId));
            }

            var course = await _unitOfWork.CourseRepository.GetCourseDetailsByIdAsync(courseId);

            if (course is null)
            {
                throw new KeyNotFoundException($"Course with ID {courseId} not found.");
            }

            return course;
        }

        public async Task AddCourseAsync(CourseDetailDTO courseDTO)
        {
            if (courseDTO == null)
            {
                throw new ArgumentNullException(nameof(courseDTO), "Course cannot be null");
            }

            // Map CourseDetailDTO to Course Entity (Manual mapping)
            var course = new Course
            {
                Title = courseDTO.Title,
                Description = courseDTO.Description,
                Price = courseDTO.Price,
                CourseType = courseDTO.CourseType,
                SeatsAvailable = courseDTO.SeatsAvailable,
                Duration = courseDTO.Duration,
                CategoryId = courseDTO.CategoryId,
                InstructorId = courseDTO.InstructorId,
                StartDate = courseDTO.StartDate,
                EndDate = courseDTO.EndDate,
                Thumbnail = courseDTO.Thumbnail,
                SessionDetails = courseDTO.SessionDetails.Select(sd => new SessionDetail
                {
                    Title = sd.Title,
                    Description = sd.Description,
                    VideoUrl = sd.VideoUrl,
                    VideoOrder = sd.VideoOrder
                }).ToList()
            };

             await _unitOfWork.CourseRepository.AddCourseAsync(course);
        }

        public async Task UpdateCourseAsync(CourseDetailDTO courseDTO)
        {
            if (courseDTO == null)
            {
                throw new ArgumentNullException(nameof(courseDTO), "Course cannot be null");
            }

            var course = await _unitOfWork.CourseRepository.GetCourseByIdAsync(courseDTO.CourseId);
            
            if (course is null)
            {
                throw new KeyNotFoundException($"Course with ID {courseDTO.CourseId} not found.");
            }

            //update course properties
            course.Title = courseDTO.Title;
            course.Description = courseDTO.Description;
            course.Price = courseDTO.Price;
            course.CourseType = courseDTO.CourseType;
            course.SeatsAvailable = courseDTO.SeatsAvailable;
            course.Duration = courseDTO.Duration;
            course.CategoryId = courseDTO.CategoryId;
            course.InstructorId = courseDTO.InstructorId;
            course.StartDate = courseDTO.StartDate;
            course.EndDate = courseDTO.EndDate;

            // Handle session details update
            // Remove any session details that were removed in the updated model
            var existingSessionIds = course.SessionDetails.Select(s => s.SessionId).ToList();
            
            var updatedSessionIds = courseDTO.SessionDetails.Select(s => s.SessionId).ToList();

            // Remove sessions that are not in the updated list
            var sessionsToRemove = course.SessionDetails.Where(s => !updatedSessionIds.Contains(s.SessionId)).ToList();
            
            foreach (var session in sessionsToRemove)
            {
                course.SessionDetails.Remove(session);
                
                _unitOfWork.CourseRepository.RemoveSessionDetail(session); // This removes the session from the database
            }

            // Update or add session details
            foreach (var sessionDTO in courseDTO.SessionDetails)
            {
                var existingSession = course.SessionDetails.FirstOrDefault(s => s.SessionId == sessionDTO.SessionId);
                
                if (existingSession != null)
                {
                    // Update existing session details
                    existingSession.Title = sessionDTO.Title;
                    existingSession.Description = sessionDTO.Description;
                    existingSession.VideoUrl = sessionDTO.VideoUrl;
                    existingSession.VideoOrder = sessionDTO.VideoOrder;
                }
                else
                {
                    // Add new session details
                    var newSession = new SessionDetail
                    {
                        Title = sessionDTO.Title,
                        Description = sessionDTO.Description,
                        VideoUrl = sessionDTO.VideoUrl,
                        VideoOrder = sessionDTO.VideoOrder,
                        CourseId = course.CourseId
                    };

                    course.SessionDetails.Add(newSession);
                }
            }

            // Call repository to update the course along with its session details
            await _unitOfWork.CourseRepository.UpdateCourseAsync(course);
        }

        public async Task DeleteCourseAsync(int courseId)
        {
            if (courseId <= 0)
            {
                throw new ArgumentException("Invalid course ID", nameof(courseId));
            }

            var course = await _unitOfWork.CourseRepository.GetCourseByIdAsync(courseId);

            if (course == null)
            {
                throw new KeyNotFoundException($"Course with ID {courseId} not found.");
            }

            await _unitOfWork.CourseRepository.DeleteCourseAsync(courseId);
        }

        public async Task<IReadOnlyList<InstructorDTO>> GetAllInstructorsAsync()
        {
            var instructors = await _unitOfWork.CourseRepository.GetAllInstructorsAsync();

            if (!instructors.Any())
            {
                return new List<InstructorDTO>();
            }

            #region Manual mapping
            //var instructorsDTO = instructors.Select(i => new InstructorDTO
            //{
            //    InstructorId = i.InstructorId,
            //    FirstName = i.FirstName,
            //    LastName = i.LastName,
            //    Email = i.Email,
            //    Bio = i.Bio,
            //    UserId = i.UserId
            //}).ToList(); 
            #endregion

            // Map Instructor entities to InstructorDTOs (Auto mapping)
            var instructorsDTO = _mapper.Map<IReadOnlyList<InstructorDTO>>(instructors);

            return instructorsDTO;
        }
        public async Task<bool> UpdateCourseThumbnail(string courseThumbnailUrl, int courseId)
        {
            if (courseId <= 0)
            {
                throw new ArgumentException("Invalid course ID", nameof(courseId));
            }

            if (string.IsNullOrEmpty(courseThumbnailUrl))
            {
                throw new ArgumentException("Course thumbnail URL cannot be null or empty", nameof(courseThumbnailUrl));
            }

            return await _unitOfWork.CourseRepository.UpdateCourseThumbnail(courseThumbnailUrl, courseId);
        }
    }
}

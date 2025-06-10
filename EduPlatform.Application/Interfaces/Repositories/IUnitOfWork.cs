
namespace EduPlatform.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        public ICategoryRepository CategoryRepository { get; }
        public ICourseRepository CourseRepository { get; }
        public IVideoRequestRepository VideoRequestRepository { get; }
        public IReviewRepository ReviewRepository { get; }
        Task SaveChangesAsync();
    }
}

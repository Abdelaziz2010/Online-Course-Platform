
namespace EduPlatform.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        public ICategoryRepository CategoryRepository { get; }
        public ICourseRepository CourseRepository { get; }
        Task SaveChangesAsync();
    }
}

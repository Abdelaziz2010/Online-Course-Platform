
namespace EduPlatform.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        public ICategoryRepository CategoryRepository { get; }
        Task SaveChangesAsync();
    }
}

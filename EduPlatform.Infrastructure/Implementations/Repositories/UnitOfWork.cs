
using EduPlatform.Application.Interfaces.Repositories;
using EduPlatform.Infrastructure.Data;

namespace EduPlatform.Infrastructure.Implementations.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EduPlatformDbContext _context;
        private ICategoryRepository _categoryRepository;
        private ICourseRepository _courseRepository;
        private IVideoRequestRepository _videoRequestRepository;


        public UnitOfWork(EduPlatformDbContext context)
        {
            _context = context;
        }


        //lazy intialization
        public ICategoryRepository CategoryRepository => _categoryRepository ??= new CategoryRepository(_context);
        public ICourseRepository CourseRepository => _courseRepository ??= new CourseRepository(_context);
        public IVideoRequestRepository VideoRequestRepository => _videoRequestRepository ??= new VideoRequestRepository(_context);

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

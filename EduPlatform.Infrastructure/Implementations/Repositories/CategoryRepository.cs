
using EduPlatform.Application.Interfaces.Repositories;
using EduPlatform.Domain.Entities;
using EduPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EduPlatform.Infrastructure.Implementations.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly EduPlatformDbContext _context;

        public CategoryRepository(EduPlatformDbContext context)
        {
            _context = context;
        }

        public async Task<CourseCategory?> GetCategoryByIdAsync(int id)
        {
           var category  = await _context.CourseCategories.FindAsync(id);

           return category;
        } 

        public async Task<List<CourseCategory>> GetAllCategoriesAsync()
        {
            var categories = await _context.CourseCategories.AsNoTracking().ToListAsync();

            return categories;
        }
    }
}

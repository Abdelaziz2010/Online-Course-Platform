
using EduPlatform.Domain.Entities;

namespace EduPlatform.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<CourseCategory?> GetCategoryByIdAsync(int id);
        Task<List<CourseCategory>> GetAllCategoriesAsync();
    }
}

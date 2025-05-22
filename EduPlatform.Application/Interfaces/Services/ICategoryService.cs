

using EduPlatform.Application.DTOs;
using EduPlatform.Domain.Entities;

namespace EduPlatform.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<CategoryDTO?> GetCategoryByIdAsync(int id);
        Task<List<CategoryDTO>> GetAllCategoriesAsync();
    }
}

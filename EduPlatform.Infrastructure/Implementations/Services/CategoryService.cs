
using AutoMapper;
using EduPlatform.Application.DTOs;
using EduPlatform.Application.Interfaces.Repositories;
using EduPlatform.Application.Interfaces.Services;

namespace EduPlatform.Infrastructure.Implementations.Services
{
    internal class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        //mapper
        private readonly IMapper _mapper;
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<CategoryDTO?> GetCategoryByIdAsync(int id)
        {
            var category =  await _unitOfWork.CategoryRepository.GetCategoryByIdAsync(id);

            if (category is not null)
            {
                #region Manual Mapping
                //return new CategoryDTO
                //{
                //    CategoryId = category.CategoryId,
                //    CategoryName = category.CategoryName,
                //    Description = category.Description
                //}; 
                #endregion

                var categoryDTO = _mapper.Map<CategoryDTO>(category);
                return categoryDTO;
            }

            return null;
        }

        public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllCategoriesAsync();

            var categoryDTOs = _mapper.Map<List<CategoryDTO>>(categories);

            #region Manual Mapping
            //var categoryDTOs = categories.Select(c => new CategoryDTO
            //{
            //    CategoryId = c.CategoryId,
            //    CategoryName = c.CategoryName,
            //    Description = c.Description
            //}).ToList(); 
            #endregion

            return categoryDTOs;
        }
    }
}

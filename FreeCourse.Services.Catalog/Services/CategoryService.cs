using AutoMapper;
using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Services
{
    public class CategoryService : ICategoryService
    {

        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public CategoryService(IMongoInstanceHelper _mongoDatabase, IDatabaseSettings databaseSettings, IMapper mapper)
        {
            _categoryCollection = _mongoDatabase.GetMongoCollectionByName<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }


        public async Task<Response<List<CategoryDto>>> GetAllAsync()
        {
            var categories = await _categoryCollection.Find(x => true).ToListAsync();
            var categoryDtoList = _mapper.Map<List<CategoryDto>>(categories);
            var response = Response<List<CategoryDto>>.Success(categoryDtoList, 200);

            return response;
        }

        public async Task<Response<CategoryDto>> CreateAsync(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _categoryCollection.InsertOneAsync(category);
            var dto = _mapper.Map<CategoryDto>(category);
            var result = Response<CategoryDto>.Success(dto, 200);

            return result;
        }

        public async Task<Response<CategoryDto>> GetById(string id)
        {
            var data = await _categoryCollection.Find<Category>(x => x.Id == id).FirstOrDefaultAsync();

            if (data == null)
            {
                return Response<CategoryDto>.Fail("Category Not Found", 404);
            }

            var dto = _mapper.Map<CategoryDto>(data);
            var result = Response<CategoryDto>.Success(dto, 200);

            return result;

        }
    }
}

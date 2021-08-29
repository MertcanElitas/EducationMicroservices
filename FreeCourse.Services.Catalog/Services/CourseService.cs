using AutoMapper;
using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using Mass = MassTransit;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreeCourse.Shared.Messages;

namespace FreeCourse.Services.Catalog.Services
{
    public class CourseService : ICourseService
    {
        private readonly IMongoCollection<Course> _mongoCourseCollection;
        private readonly IMongoCollection<Category> _mongoCategoryCollection;
        private readonly IMapper _mapper;
        private readonly Mass.IPublishEndpoint _publishEndpoint;



        public CourseService(IMongoInstanceHelper _mongoDatabase, IDatabaseSettings databaseSettings, IMapper mapper, Mass.IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
            _mongoCourseCollection = _mongoDatabase.GetMongoCollectionByName<Course>(databaseSettings.CourseCollectionName);
            _mongoCategoryCollection = _mongoDatabase.GetMongoCollectionByName<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }

        public async Task<Response<List<CourseDto>>> GetAllAsync()
        {
            var cources = await _mongoCourseCollection.Find(x => true).ToListAsync();

            if (cources.Any())
            {
                foreach (var course in cources)
                {
                    course.Category = await _mongoCategoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstOrDefaultAsync();
                }
            }
            else
            {
                cources = new List<Course>();
            }

            var dto = _mapper.Map<List<CourseDto>>(cources);
            var response = Response<List<CourseDto>>.Success(dto, 200);

            return response;
        }

        public async Task<Response<CourseDto>> GetCourseByIdAsync(string id)
        {
            var data = await _mongoCourseCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (data == null)
            {
                return Response<CourseDto>.Fail("Course not found", 404);
            }

            data.Category = await _mongoCategoryCollection.Find<Category>(x => x.Id == data.CategoryId).FirstOrDefaultAsync();

            var course = _mapper.Map<CourseDto>(data);

            return Response<CourseDto>.Success(course, 200);
        }

        public async Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
        {
            var cources = await _mongoCourseCollection.Find(x => x.UserId == userId).ToListAsync();

            if (cources.Any())
            {
                foreach (var course in cources)
                {
                    course.Category = await _mongoCategoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
                }
            }
            else
            {
                cources = new List<Course>();
            }

            var dto = _mapper.Map<List<CourseDto>>(cources);
            var response = Response<List<CourseDto>>.Success(dto, 200);

            return response;
        }

        public async Task<Response<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto)
        {
            var courseEntity = _mapper.Map<Course>(courseCreateDto);

            courseEntity.CreatedTime = DateTime.Now;
            await _mongoCourseCollection.InsertOneAsync(courseEntity);

            var courseDto = _mapper.Map<CourseDto>(courseEntity);

            return Response<CourseDto>.Success(courseDto, 200);
        }

        public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courseCreateDto)
        {
            var courseEntity = _mapper.Map<Course>(courseCreateDto);

            var result = await _mongoCourseCollection.FindOneAndReplaceAsync(x => x.Id == courseEntity.Id, courseEntity);

            if (result == null)
                return Response<NoContent>.Fail("Course not found", 404);

            var courseNameChangedEvent = new CourseNameChangedEvent()
            {
                CourseId = courseCreateDto.Id,
                UpdatedName = courseCreateDto.Name
            };

            await _publishEndpoint.Publish(courseNameChangedEvent);

            return Response<NoContent>.Success(204);
        }

        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
            var deleteResult = await _mongoCourseCollection.DeleteOneAsync(x => x.Id == id);

            if (deleteResult.DeletedCount > 0)
                return Response<NoContent>.Success(204);
            else
                return Response<NoContent>.Fail("Delete operation failed", 404);
        }
    }
}

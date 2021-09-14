using Dapper;
using FreeCourse.Shared.Dtos;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Discount.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConntection;

        public DiscountService(IConfiguration configuration)
        {
            _configuration = configuration;

            var connectionString = _configuration.GetConnectionString("PostgreSql");
            _dbConntection = new NpgsqlConnection(connectionString);
        }


        public async Task<Response<NoContent>> Delete(int id)
        {
            var status = await _dbConntection.ExecuteAsync("Delete from discount where id=@Id", new
            {
                Id = id
            });

            if (status > default(int))
                return Response<NoContent>.Success(204);

            return Response<NoContent>.Fail("An error accured while adding", 500);
        }

        public async Task<Response<List<Models.Discount>>> GetAll()
        {
            var discounts = await _dbConntection.QueryAsync<Models.Discount>("Select * From discount");

            return Response<List<Models.Discount>>.Success(discounts.ToList(), 200);
        }

        public async Task<Response<Models.Discount>> GetByCodeAndUserId(string code, string userId)
        {
            var discounts = await _dbConntection.QueryAsync<Models.Discount>("Select * From discount Where code=@Code and userid=@UserId", new { Code = code, UserId = userId });

            var model = discounts.FirstOrDefault();

            if (model == null)
                return Response<Models.Discount>.Fail("Discount not found", 404);

            return Response<Models.Discount>.Success(model, 200);
        }

        public async Task<Response<Models.Discount>> GetById(int id)
        {
            var discounts = await _dbConntection.QueryAsync<Models.Discount>("Select * From discount Where Id=@Id", new { Id = id });
            var model = discounts.FirstOrDefault();

            if (model == null)
                return Response<Models.Discount>.Fail("Discount not found", 404);

            return Response<Models.Discount>.Success(model, 200);
        }

        public async Task<Response<NoContent>> Save(Models.Discount discount)
        {
            var status = await _dbConntection.ExecuteAsync("Insert Into discount (userid,rate,code) Values(@UserId,@Rate,@Code)", discount);

            if (status > default(int))
                return Response<NoContent>.Success(204);

            return Response<NoContent>.Fail("An error accured while adding", 500);
        }

        public async Task<Response<NoContent>> Update(Models.Discount discount)
        {
            var status = await _dbConntection.ExecuteAsync("Update discount set userid=@UserId,code=@Code,rate=@Rate where id=@Id", new
            {
                Id = discount.Id,
                UserId = discount.UserId,
                Rate = discount.Rate
            });

            if (status > default(int))
                return Response<NoContent>.Success(204);

            return Response<NoContent>.Fail("An error accured while adding", 500);
        }
    }
}

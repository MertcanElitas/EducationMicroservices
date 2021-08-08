using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Discount.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Discount.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : CustomBaseController
    {
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IDiscountService _discountService;

        public DiscountsController(ISharedIdentityService sharedIdentityService, IDiscountService discountService)
        {
            _sharedIdentityService = sharedIdentityService;
            _discountService = discountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _discountService.GetAll();

            return CreateActionResultInstance(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _discountService.GetById(id);

            return CreateActionResultInstance(data);
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var userId = _sharedIdentityService.GetUserId;
            var data = await _discountService.GetByCodeAndUserId(code, userId);

            return CreateActionResultInstance(data);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Models.Discount model)
        {
            var result = await _discountService.Save(model);

            return CreateActionResultInstance(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Models.Discount model)
        {
            var result = await _discountService.Update(model);

            return CreateActionResultInstance(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _discountService.Delete(id);

            return CreateActionResultInstance(result);
        }
    }
}

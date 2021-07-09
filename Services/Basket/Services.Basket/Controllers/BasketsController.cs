using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Basket.Dtos;
using Services.Basket.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Basket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : CustomBaseController
    {
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _identityService;

        public BasketsController(IBasketService basketService, ISharedIdentityService identityService)
        {
            _basketService = basketService;
            _identityService = identityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            var userId = _identityService.GetUserId;
            var basket = await _basketService.GetBasket(userId);

            return CreateActionResultInstance(basket);
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrUpdateBasket(BasketDto basketDto)
        {
            var basket = await _basketService.SaveOrUpdate(basketDto);

            return CreateActionResultInstance(basket);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasket()
        {
            var userId = _identityService.GetUserId;
            var basket = await _basketService.Delete(userId);

            return CreateActionResultInstance(basket);
        }
    }
}

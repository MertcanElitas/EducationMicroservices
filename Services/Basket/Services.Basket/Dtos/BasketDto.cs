using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Basket.Dtos
{
    public class BasketDto
    {
        public BasketDto()
        {
            BasketItems = new List<BasketItemDto>();
        }

        public string UserId { get; set; }
        public string DiscountCode { get; set; }
        public decimal TotalPrice
        {
            get
            {
                var total = BasketItems.Sum(x => x.Price * x.Quatity);

                return total;
            }
        }

        public List<BasketItemDto> BasketItems { get; set; }
    }
}

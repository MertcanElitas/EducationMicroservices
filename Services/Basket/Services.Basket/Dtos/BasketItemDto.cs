using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Basket.Dtos
{
    public class BasketItemDto
    {
        public int Quatity { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public decimal Price { get; set; }
    }
}

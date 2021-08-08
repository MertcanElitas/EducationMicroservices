using FreeCourse.Shared.Dtos;
using MediatR;
using Services.Order.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Order.Application.Queries
{
    public class GetOrdersByUserIdQuery : IRequest<Response<List<OrderDto>>>
    {
        public string UserId { get; set; }
    }
}

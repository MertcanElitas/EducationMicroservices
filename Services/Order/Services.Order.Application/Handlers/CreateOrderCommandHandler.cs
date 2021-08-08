using FreeCourse.Shared.Dtos;
using MediatR;
using Services.Order.Application.Command;
using Services.Order.Application.Dtos;
using Services.Order.Domain.OrderAggregate;
using Services.Order.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Order.Application.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<CreatedOrderDto>>
    {
        private readonly OrderDbContext _orderDbContext;

        public CreateOrderCommandHandler(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task<Response<CreatedOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var adressDto = request.Address;

            var adress = new Adress(adressDto.Province, adressDto.ZipCode, adressDto.District, adressDto.Line, adressDto.Street);
            var order = new Order.Domain.OrderAggregate.Order(request.BuyerId, adress);

            request.OrderItems.ForEach(x =>
            {

                order.AddOrderItem(x.ProductId, x.ProductName, x.Price, x.PictureUrl);

            });

            _orderDbContext.Orders.Add(order);

            var result = await _orderDbContext.SaveChangesAsync();

            return Response<CreatedOrderDto>.Success(new CreatedOrderDto() { OrderId = order.Id }, 200);
        }
    }
}

using Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Order.Domain.OrderAggregate
{
    //Ef core features
    // -- Owned Types
    // --Shadow Property -> Databasede kolon olarak bulunan fakat kod tarafında bir karşılığı olmayan propertylere denir.
    // --Backing Fields  -> Okuma ve yazma işlemini ef core içinde bir field üzerinden yaparsak buna backing field denir.
    public class Order : Entity, IAggregateRoot
    {
        private readonly List<OrderItem> _orderItems;

        public DateTime CreatedDate { get; private set; }
        public Adress Address { get; private set; }
        public string BuyerId { get; private set; }
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        public Order()
        {

        }

        public Order(string buyerId, Adress address)
        {
            _orderItems = new List<OrderItem>();
            CreatedDate = DateTime.Now;
            BuyerId = buyerId;
            Address = address;
        }

        public void AddOrderItem(string productId, string productName, decimal price, string picturUrl)
        {
            var existProduct = _orderItems.Any(x => x.ProductId == productId);

            if (!existProduct)
            {
                var newOrderItem = new OrderItem(productId, productName, picturUrl, price);

                _orderItems.Add(newOrderItem);
            }
        }

        public decimal GetTotalPrice => _orderItems.Sum(x => x.Price);
    }
}

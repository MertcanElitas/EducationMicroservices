using Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Order.Domain.OrderAggregate
{
    public class OrderItem : Entity
    {
        public string ProductId { get; private set; }
        public string ProductName { get; private set; }
        public string PictureUrl { get; private set; }
        public decimal Price { get; private set; }

        //public decimal OrderId { get; private set; }
        //DDD metodolojisine göre bu şekilde bi navigation property tanımlanmamalı.
        //Nedeni OrderItem tek başına eklenebilen bir entity değildir.
        //Bu entitynin eklenebilmesi için agrregateroot üzerinden  bu işlemin yapılması gerekir.
        //Ancak ef core db ye ilişkiyi kurabilmek için OrderId adında bi kolon atar.
        //Db de olan ancak kod tarafında karşılığı olmayan propertylere "SHADOW PROPERTY" denir.

        public OrderItem(string productId, string productName, string pictureUrl, decimal price)
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
            Price = price;
        }

        public void UpdateOrderItem(string productName, string pictureUrl, decimal price)
        {
            ProductName = productName;
            PictureUrl = pictureUrl;
            Price = price;
        }
    }
}

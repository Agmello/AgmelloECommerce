using Modules.Orders.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Orders.Domain
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pending";
        public decimal TotalAmount { get; set; } = 0.0m;

        public List<OrderItems> OrderItems { get; set; } = new List<OrderItems>();

        public Order(AddOrderDto addOrderDto)
        {
            UserId = addOrderDto.UserId;
            Status = addOrderDto.Status;
            TotalAmount = OrderItems.Sum(x => x.UnitPrice * x.Quantity);
            OrderItems.AddRange(
                    addOrderDto.OrderItems.Select(oi => new OrderItems(oi)).ToList()
                );
        }
        public Order() { }
    }
}

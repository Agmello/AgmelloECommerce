using Modules.Orders.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Orders.Domain
{
    public class OrderItems
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid CatalogItemId { get; set; }
        public string CatalogItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public OrderItems(AddOrderItemDto dto)
        {
            CatalogItemId = dto.CatalogItemId;
            CatalogItemName = dto.CatalogItemName;
            Quantity = dto.Quantity;
            UnitPrice = dto.UnitPrice;
        }
        public OrderItems() { }
    }
}

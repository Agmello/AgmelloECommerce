using Modules.Orders.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Orders.Application.Dtos
{
    public class AddOrderDto
    {
        public Guid UserId { get; set; }
        public string Status { get; set; } = "Pending";
        public ICollection<AddOrderItemDto> OrderItems { get; set; } = new List<AddOrderItemDto>();
    }

    public class  AddOrderItemDto
    {
        public Guid CatalogItemId { get; set; }
        public string CatalogItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}

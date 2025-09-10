using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Modules.Common.Authorization;
using Modules.Orders.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Orders.Application.Authorization
{
    internal record CanManageOrderRequirment : CanModifyRequirement;

    internal class CanManageOrderRequirmentHandler(OrdersDbContext context, IHttpContextAccessor accessor) : CanModifyRequirmentHandler<CanManageOrderRequirment, OrdersDbContext>(context, accessor)
    {
        protected override async Task<bool> SpecialRequirmentFullied(Guid userId, Guid id)
        {
            var order = await m_dbContext.Orders.FirstOrDefaultAsync(order =>  order.Id == id);

            return (order != null && order.UserId == userId);

        }
    }
}

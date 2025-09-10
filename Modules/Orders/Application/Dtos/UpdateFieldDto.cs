using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Orders.Application.Dtos
{
    internal abstract class UpdateFieldDto
    {
        public string FieldName { get; set; } = string.Empty;
        public object? NewValue { get; set; }
    }
}

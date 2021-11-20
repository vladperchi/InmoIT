using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InmoIT.Shared.Dtos.Flow.Property
{
    public record GetPropertyByIdResponse(Guid Id, string InternalName);
}
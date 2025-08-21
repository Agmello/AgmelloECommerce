using MediatR;
using Modules.Catalog.Domain;
using Modules.Users.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Users.Application
{
    internal abstract class UserHandlerBase<TAction, TItem> : IRequestHandler<TAction, TItem> where TAction : IRequest<TItem>
    {
        protected readonly IUsersRepository m_userRepository;
        protected UserHandlerBase(IUsersRepository catalogRepository)
        {
            m_userRepository = catalogRepository;
        }
        public abstract Task<TItem> Handle(TAction request, CancellationToken cancellationToken);
    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Commands
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public DeleteProductCommand(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}

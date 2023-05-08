using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers
{
    public class GetAllTypesHandler : IRequestHandler<GetAllTypesQuery, IList<TypeResponse>>
    {
        private readonly ITypesRepository typesRepository;

        public GetAllTypesHandler(ITypesRepository typesRepository)
        {
            this.typesRepository = typesRepository;
        }
        public async Task<IList<TypeResponse>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
        {
            var typesList = await typesRepository.GetAllTypes();
            IList<TypeResponse> response = ProductMapper.Mapper.Map<IList<TypeResponse>>(typesList);
            return response;
        }
    }
}

using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetAllBrandsHandler : IRequestHandler<GetAllBrandsQuery, IList<BrandResponse>>
    {
        private readonly IBrandRepository brandRepository;

        public GetAllBrandsHandler(IBrandRepository brandRepository)
        {
            this.brandRepository = brandRepository;
        }


        public async Task<IList<BrandResponse>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var brandList = await brandRepository.GetAllBrands();
            var brandResponseList = ProductMapper.Mapper.Map<IList<BrandResponse>>(brandList);
            return brandResponseList;
        }
    }
}

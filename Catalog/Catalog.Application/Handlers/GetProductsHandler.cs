using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetProductsHandler : IRequestHandler<GetProductsQuery, Pagination<ProductResponse>>
    {
        private readonly IProductRepository productRepository;

        public GetProductsHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<Pagination<ProductResponse>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var productsList = await productRepository.GetProducts(request.CatalogSpecParams);
            Pagination<ProductResponse> products = ProductMapper.Mapper.Map<Pagination<ProductResponse>>(productsList);
            return products;
        }
    }
}

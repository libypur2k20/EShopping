﻿using Amazon.Runtime.Internal;
using Catalog.Application.Commands;
using Catalog.Application.Mappers;
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
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductResponse>
    {
        private readonly IProductRepository productRepository;

        public CreateProductHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {

            var productEntity = ProductMapper.Mapper.Map<Product>(request);
            
            if(productEntity == null)
            {
                throw new ApplicationException("There is an issue with mapping while creating new product.");
            }


            var productCreated = await productRepository.CreateProduct(productEntity);
            var productResponse = ProductMapper.Mapper.Map<ProductResponse>(productCreated);
            return productResponse;
        }
    }
}

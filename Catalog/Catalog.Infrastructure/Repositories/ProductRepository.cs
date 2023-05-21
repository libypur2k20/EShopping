using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository, IBrandRepository, ITypesRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context) {
            this._context = context;
        }


        public async Task<Product> CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
            return product;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            bool result = false;

            Product p = await GetProduct(id);
            if (p != null)
            {
                DeleteResult deleteResult = await _context.Products.DeleteOneAsync(p => true);
                result = (deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0);
            }
            return result;
        }

        public async Task<IEnumerable<ProductBrand>> GetAllBrands()
        {
            return await _context.Brands.Find(b =>true).ToListAsync();
        }

        public async Task<Pagination<Product>> GetProducts(CatalogSpecParams catalogSpecParams)
        {
            var builder = Builders<Product>.Filter;
            var filter = builder.Empty;

            if (!string.IsNullOrEmpty(catalogSpecParams.Search))
            {
                var searchFilter = builder.Regex(x => x.Name, new MongoDB.Bson.BsonRegularExpression(catalogSpecParams.Search));
                filter &= searchFilter;
            }

            if (!string.IsNullOrEmpty(catalogSpecParams.BrandId))
            {
                var brandFilter = builder.Eq(x => x.Brands.Id, catalogSpecParams.BrandId);
                filter &= brandFilter;
            }

            if (!string.IsNullOrEmpty(catalogSpecParams.TypeId))
            {
                var typeFilter = builder.Eq(x => x.Types.Id,catalogSpecParams.TypeId);
                filter &= typeFilter;
            }

            if (!string.IsNullOrEmpty(catalogSpecParams.Sort))
            {
                return new Pagination<Product>()
                {
                    PageIndex = catalogSpecParams.PageIndex,
                    PageSize = catalogSpecParams.PageSize,
                    Data = await DataFilter(catalogSpecParams,filter),
                    Count = await _context.Products.CountDocumentsAsync(p => true)

                };
            }

            return new Pagination<Product>()
            {
                PageIndex = catalogSpecParams.PageIndex,
                PageSize = catalogSpecParams.PageSize,
                Data = await _context
                .Products
                .Find(filter)
                .Sort(Builders<Product>.Sort.Ascending("Name"))
                .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
                .Limit(catalogSpecParams.PageSize)
                .ToListAsync(),
                Count = await _context.Products.CountDocumentsAsync(p => true)

            };
        }

        private async Task<IReadOnlyList<Product>> DataFilter(CatalogSpecParams catalogSpecParams, FilterDefinition<Product> filter)
        {
            switch (catalogSpecParams.Sort)
            {
                case "priceAsc":
                    return await _context.Products
                        .Find(filter)
                        .SortBy(p => p.Price)
                        .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
                        .Limit(catalogSpecParams.PageSize)
                        .ToListAsync();
                    
                case "priceDesc":
                    return await _context.Products
                        .Find(filter)
                        .SortByDescending(p => p.Price)
                        .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
                        .Limit(catalogSpecParams.PageSize)
                        .ToListAsync();
                default:
                    return await _context.Products
                        .Find(filter)
                        .SortBy(p => p.Name)
                        .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
                        .Limit(catalogSpecParams.PageSize)
                        .ToListAsync();
            }
        }

        public async Task<IEnumerable<ProductType>> GetAllTypes()
        {
            return await _context.Types.Find(t =>true).ToListAsync();
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByBrand(string name)
        {
            //MongoDB specific.
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Brands.Name, name);

            return await _context.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            //MongoDB specific.
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);

            return await _context.Products.Find(filter).ToListAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            ReplaceOneResult updateResult = await _context.Products.ReplaceOneAsync(p=>p.Id == product.Id, product);

            return (updateResult.IsAcknowledged && updateResult.ModifiedCount > 0);

        }
    }
}

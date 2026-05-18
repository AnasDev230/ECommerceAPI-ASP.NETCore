using AutoMapper;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Models.DTO.Product;
using ECommerceAPI_ASP.NETCore.Repositories.Interface;
using ECommerceAPI_ASP.NETCore.Services.Interface;

namespace ECommerceAPI_ASP.NETCore.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        public async Task<ProductDto> CreateAsync(CreateProductRequestDto request, string vendorId)
        {
            var category = await categoryRepository.GetByID(request.CategoryId);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {request.CategoryId} not found.");

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                DescriptionPlainText = request.DescriptionPlainText,
                Brand = request.Brand,
                IsActive = request.IsActive,
                Weight = request.Weight,
                ImageID = request.ImageID,
                VendorId = vendorId,
                CategoryId = request.CategoryId,
            };

            await productRepository.CreateAsync(product);
            return mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await productRepository.GetAllAsync();
            return mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<IEnumerable<ProductDto>> GetAllActiveAsync()
        {
            var products = await productRepository.GetAllAsync();
            var activeProducts = products.Where(p => p.IsActive);
            return mapper.Map<IEnumerable<ProductDto>>(activeProducts);
        }

        public async Task<ProductDto?> GetByIdAsync(Guid id)
        {
            var product = await productRepository.GetByID(id);
            if (product == null)
                return null;

            return mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetByCategoryIdAsync(Guid categoryId)
        {
            var category = await categoryRepository.GetByID(categoryId);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {categoryId} not found.");

            var products = await productRepository.GetAllProductsByCategoryID(categoryId);
            return mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<IEnumerable<ProductDto>> GetByVendorIdAsync(string vendorId)
        {
            var products = await productRepository.GetAllAsync();
            var vendorProducts = products.Where(p => p.VendorId == vendorId);
            return mapper.Map<IEnumerable<ProductDto>>(vendorProducts);
        }

        public async Task<ProductDto?> UpdateAsync(Guid id, CreateProductRequestDto request, string vendorId, bool isAdmin)
        {
            var product = await productRepository.GetByID(id);
            if (product == null)
                return null;

            if (!isAdmin && product.VendorId != vendorId)
                throw new UnauthorizedAccessException("You do not have permission to update this product.");

            if (request.CategoryId != product.CategoryId)
            {
                var category = await categoryRepository.GetByID(request.CategoryId);
                if (category == null)
                    throw new KeyNotFoundException($"Category with ID {request.CategoryId} not found.");
            }

            mapper.Map(request, product);
            var updated = await productRepository.UpdateAsync(product);
            if (!updated)
                return null;

            var updatedProduct = await productRepository.GetByID(id);
            return mapper.Map<ProductDto>(updatedProduct);
        }

        public async Task<ProductDto?> DeleteAsync(Guid id, string vendorId, bool isAdmin)
        {
            var product = await productRepository.GetByID(id);
            if (product == null)
                return null;

            if (!isAdmin && product.VendorId != vendorId)
                throw new UnauthorizedAccessException("You do not have permission to delete this product.");

            var deletedProduct = await productRepository.DeleteAsync(id);
            return mapper.Map<ProductDto>(deletedProduct);
        }
    }
}

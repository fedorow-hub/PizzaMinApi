
using System.Collections.Immutable;
using System.Diagnostics;

public class PizzaRepository : IPizzaRepository
{
    private readonly PizzaDb _context;
    public PizzaRepository(PizzaDb context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<List<Product>> GetProductsBySearchParamAsync(string searchParam)
    {
        var products = await _context.Products.ToListAsync();
        return products.Where(p => p.Name.Contains(searchParam, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public async Task<Product> GetProductAsync(int productId)
    {
        return await _context.Products.FindAsync(new object[] { productId });
    }

    public async Task InsertProductAsync(Product product)
    {
        await _context.Products.AddAsync(product);
    }

    public async Task UpdateProductAsync(Product product)
    {
        var productFromDb = await _context.Products.FindAsync(new object[] { product.Id });

        if (productFromDb == null) return;

        productFromDb.Category = product.Category;
        productFromDb.CategoryId = product.CategoryId;
        productFromDb.UpdatedAt = DateTime.Now;
        productFromDb.ImageUrl = product.ImageUrl;
        productFromDb.Name = product.Name;
        productFromDb.Ingredients = product.Ingredients;
        productFromDb.Items = product.Items;
    }

    public async Task DeleteProductAsync(int productId)
    {
        var productFromDb = await _context.Products.FindAsync(new object[] { productId });

        if (productFromDb == null) return;

        _context.Products.Remove(productFromDb);
    }

    public async Task<List<Ingredient>> GetIngredientsAsync()
    {
        return await _context.Ingredients.ToListAsync();
    }

    public async Task<List<CategoryDto>> GetCategoresAsync()
    {
        var categories = await _context.Categories
                                   .Include(c => c.Products).ThenInclude(p => p.Ingredients)
                                   .Include(c => c.Products).ThenInclude(p => p.Items)
                                   .Where(c => c.Products.Any())
                                   .ToListAsync();

        var categoryDtos = categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Products = c.Products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                ImageUrl = p.ImageUrl,
                ProductItems = p.Items.Select(i => new ProductItemDto
                {
                    Id = i.Id,
                    Price = i.Price,
                    Size = i.Size,
                    PizzaType = i.PizzaType
                }).ToList(),
                Ingredients = p.Ingredients.Select(i => new IngredientDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Price = i.Price,
                    ImageUrl = i.ImageUrl
                }).ToList()
            }).ToList()
        }).ToList();

        return categoryDtos;
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }


}

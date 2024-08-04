
using System.Collections.Immutable;

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

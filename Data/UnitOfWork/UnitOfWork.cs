using Inventory.Data.Abstractions.UnitOfWork;
using Inventory.Data.Abstractions.Repository;
using Inventory.Data.Repository;
using Inventory.Models;

namespace Inventory.Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly InventoryDbContext _context;

    private IRepository<Brand> _brands;
    private IRepository<Printer> _printers;
    private IRepository<Toner> _toners;

    public IRepository<Brand> Brands
    {
        get
        {
            if (_brands == null)
                _brands = new InventoryRepository<Brand>(_context);
            return _brands;
        }
    }

    public IRepository<Printer> Printers
    {
        get
        {
            if (_printers == null)
                _printers = new InventoryRepository<Printer>(_context);
            return _printers;
        }
    }

    public IRepository<Toner> Toners
    {
        get
        {
            if (_toners == null)
                _toners = new InventoryRepository<Toner>(_context);
            return _toners;
        }
    }

    public UnitOfWork(InventoryDbContext context)
    {
        _context = context;
    }

    private async Task<Result> SaveAsync()
    {
        if (await _context.SaveChangesAsync() > 0)
        {
            return Result.Success();
        }
        return Result.Failure("As alterações não foram salvas.");
    }

    public async Task<Result> CommitAsync()
    {
        return await SaveAsync();
    }

    private bool disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        this.disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
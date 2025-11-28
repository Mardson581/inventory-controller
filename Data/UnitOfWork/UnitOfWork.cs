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
    private IRepository<UserTonerRequest> _userTonerRequests;

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

    public IRepository<UserTonerRequest> TonerRequests
    {
        get
        {
            if (_userTonerRequests == null)
                _userTonerRequests = new InventoryRepository<UserTonerRequest>(_context);
            return _userTonerRequests;
        }
    }

    public UnitOfWork(InventoryDbContext context)
    {
        _context = context;
    }

    private async Task<Result<int>> SaveAsync()
    {
        var changes = await _context.SaveChangesAsync();
        if (changes > 0)
        {
            return Result<int>.Success(changes);
        }
        return Result<int>.Failure("As alterações não foram salvas.", 0);
    }

    public async Task<Result<int>> CommitAsync()
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
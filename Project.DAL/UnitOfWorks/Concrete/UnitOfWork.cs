using Project.DAL.Abstract;
using Project.DAL.DatabaseContext;
using Project.DAL.UnitOfWorks.Abstract;

namespace Project.DAL.UnitOfWorks.Concrete;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _dataContext;

    private bool isDisposed;

    public UnitOfWork(
        DataContext dataContext,
        IUserRepository userRepository,
        IAuthRepository authRepository,
        ILoggingRepository loggingRepository,
        ICategoryRepository categoryRepository,
        IAuthorRepository authorRepository,
        IBookRepository bookRepository)
    {
        _dataContext = dataContext;
        UserRepository = userRepository;
        AuthRepository = authRepository;
        LoggingRepository = loggingRepository;
        CategoryRepository = categoryRepository;
        AuthorRepository = authorRepository;
        BookRepository = bookRepository;
    }

    public IUserRepository UserRepository { get; set; }

    public IAuthRepository AuthRepository { get; set; }

    public ILoggingRepository LoggingRepository { get; set; }

    public ICategoryRepository CategoryRepository { get; set; }

    public IAuthorRepository AuthorRepository { get; set; }

    public IBookRepository BookRepository { get; set; }

    public async Task CommitAsync()
    {
        await _dataContext.SaveChangesAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (!isDisposed)
        {
            isDisposed = true;
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }
    }

    public void Dispose()
    {
        if (!isDisposed)
        {
            isDisposed = true;
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing) _dataContext.Dispose();
    }

    protected async ValueTask DisposeAsync(bool disposing)
    {
        if (disposing) await _dataContext.DisposeAsync();
    }
}
using Project.DAL.Abstract;

namespace Project.DAL.UnitOfWorks.Abstract;

public interface IUnitOfWork : IAsyncDisposable
{
    public IUserRepository UserRepository { get; set; }

    public IAuthRepository AuthRepository { get; set; }

    public ILoggingRepository LoggingRepository { get; set; }

    public ICategoryRepository CategoryRepository { get; set; }

    public IAuthorRepository AuthorRepository { get; set; }

    public IBookRepository BookRepository { get; set; }

    public Task CommitAsync();
}
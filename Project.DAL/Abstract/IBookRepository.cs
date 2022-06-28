using Project.DAL.GenericRepositories.Abstract;
using Project.Entity.Entities;

namespace Project.DAL.Abstract;

public interface IBookRepository : IGenericRepository<Book>
{
    Task AddBookAsync(Book book);

    Task<List<Book>> Search(Dictionary<string, string> filters);
}
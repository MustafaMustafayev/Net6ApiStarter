using Microsoft.EntityFrameworkCore;
using Project.Core.Enums;
using Project.DAL.Abstract;
using Project.DAL.DatabaseContext;
using Project.DAL.GenericRepositories.Concrete;
using Project.Entity.Entities;

namespace Project.DAL.Concrete;

public class BookRepository : GenericRepository<Book>, IBookRepository
{
    private readonly DataContext _dataContext;

    public BookRepository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task AddBookAsync(Book book)
    {
        await _dataContext.Books.AddAsync(book);
    }

    public async Task<List<Book>> Search(Dictionary<string, string> filters)
    {
        string name, isbn, categoryId, authorId;

        filters.TryGetValue(EBookSearchFilterKey.Name.ToString(), out name);
        filters.TryGetValue(EBookSearchFilterKey.ISBN.ToString(), out isbn);
        filters.TryGetValue(EBookSearchFilterKey.CategoryId.ToString(), out categoryId);
        filters.TryGetValue(EBookSearchFilterKey.AuthorId.ToString(), out authorId);

        var books = await _dataContext.Books.Where(m =>
                (!string.IsNullOrEmpty(name) ? m.Name == name : true) &&
                (!string.IsNullOrEmpty(isbn) ? m.ISBN == isbn : true) &&
                (!string.IsNullOrEmpty(categoryId) ? m.CategoryId == int.Parse(categoryId) : true) &&
                (!string.IsNullOrEmpty(authorId) ? m.Authors.Any(p => p.AuthorId == int.Parse(authorId)) : true))
            .ToListAsync();

        return books;
    }
}
using Microsoft.EntityFrameworkCore;
using Project.DAL.Abstract;
using Project.DAL.DatabaseContext;
using Project.DAL.GenericRepositories.Concrete;
using Project.Entity.Entities;

namespace Project.DAL.Concrete;

public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
{
    private readonly DataContext _dataContext;

    public AuthorRepository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<List<Author>> GetListAsync(List<int> authorIds)
    {
        return await _dataContext.Authors.Where(m => authorIds.Contains(m.AuthorId)).ToListAsync();
    }
}
using Project.DAL.GenericRepositories.Abstract;
using Project.Entity.Entities;

namespace Project.DAL.Abstract;

public interface IAuthorRepository : IGenericRepository<Author>
{
    Task<List<Author>> GetListAsync(List<int> authorIds);
}
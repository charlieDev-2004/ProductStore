using Core.Models;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<PagedResult<T>> GetAll(ISpecification<T> spec);
        Task<T> GetById(ISpecification<T> spec);
        Task<bool> Create(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
        Task<bool> Exist(int id);
        Task<int> Count();
        Task<bool> Save();
    }
}
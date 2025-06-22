using Core.Specifications;

namespace Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<ICollection<T>> GetAll(ISpecification<T> spec);
        Task<T> GetById(int id);
        Task<bool> Create(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
        Task<bool> Exist(int id);
        Task<bool> Save();
    }
}
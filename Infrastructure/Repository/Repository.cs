using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        private DbSet<T> Entity => _context.Set<T>();

        public async Task<bool> Create(T entity)
        {
            await Entity.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(T entity)
        {
            Entity.Remove(entity);
            return await Save();
        }

        public async Task<bool> Exist(int id)
        {
            return await Entity.AnyAsync(e => EF.Property<int>(e,"Id") == id);
        }

        public async Task<ICollection<T>> GetAll(ISpecification<T> spec)
        {
            return await SpecificationEvaluator<T>.GetQuery(Entity, spec).AsNoTracking().ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await Entity.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(T entity)
        {
             Entity.Update(entity);
            return await Save();
        }
    }
}
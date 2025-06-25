using Core.Interfaces;
using Core.Models.Pagination;
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

        public async Task<PagedResult<T>> GetAll(ISpecification<T> spec)
        {
            var pagedResult = new PagedResult<T>
            {
                CurrentPage = spec.PageNumber,
                PageSize = spec.PageSize,
                TotalPages = (int)Math.Ceiling((double)await Count() / (double)spec.PageSize),
                Items = await SpecificationEvaluator<T>.GetQuery(Entity, spec).Skip((spec.PageNumber - 1) * spec.PageSize).Take(spec.PageSize).AsNoTracking().ToListAsync(),
            };

            return pagedResult;
        }

        public async Task<T> GetById(ISpecification<T> spec)
        {
            return await SpecificationEvaluator<T>.GetQuery(Entity, spec).FirstOrDefaultAsync();
        }

        public async Task<int> Count()
        {
            return await Entity.CountAsync();
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
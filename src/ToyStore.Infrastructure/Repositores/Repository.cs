using Microsoft.EntityFrameworkCore;
using ToyStore.Domain.Interfaces;
using ToyStore.Infrastructure.Context;

namespace ToyStore.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext Context;

    public Repository(ApplicationDbContext context)
    {
        Context = context;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await Context.Set<T>().FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        await Context.Set<T>().AddAsync(entity);
    }

    public void Update(T entity)
    {
        Context.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        Context.Set<T>().Remove(entity);
    }

    public async Task SaveChangesAsync()
    {
        await Context.SaveChangesAsync();
    }
}
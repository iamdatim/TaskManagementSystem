using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem_DataSource.Context;
using TaskManagementSystem_DataSource.Entities;
using TaskManagementSystem_DataSource.Repository.Interface;

namespace TaskManagementSystem_DataSource.Repository.Implementation
{
    public class TodoRepo : ITodoRepo
    {
        private DbSet<Todo> _dbSet;
        private ApplicationDbContext _dbContext { get; set; }
        public TodoRepo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<Todo>();
        }

        public async Task<IEnumerable<Todo>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Todo> GetByIdAsync(Guid Id)
        {
            return await _dbSet.FindAsync(Id);
        }

        public async Task<bool> InsertAsync(Todo todo)
        {
            await _dbSet.AddAsync(todo);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task UpdateAsync(Todo todo)
        {
            _dbSet.Update(todo);
            await _dbContext.SaveChangesAsync();

        }

        public async Task DeleteAsync(Todo todo)
        {
            _dbSet.Remove(todo);
            await _dbContext.SaveChangesAsync();
        }
    }
}


using Blog.Core.Interfaces;
using Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Services
{
    public class GenaricRepository<TableModel> : IGenaricRepository<TableModel>
        where TableModel : class
    {
        // DI
        private readonly AppDbContext _context; // Whole DataBase
        private readonly DbSet<TableModel> _table; // Table

        public GenaricRepository(AppDbContext context)
        {
            _context = context;
            _table = _context.Set<TableModel>();
        }

        public async Task<IEnumerable<TableModel>> GetAllAsync()
        {
            return await _table.ToListAsync();
        }


        public async Task<TableModel?> GetByIdAsync(int id)
        {
            return await _table.FindAsync(id);
        }


        public async Task CreateAsync(TableModel model)
        {
            await _table.AddAsync(model);
        }

        public void Update(TableModel model)
        {
            _table.Update(model);
        }
        public void Delete(TableModel model)
        {
            _table.Remove(model);
        }

        //public async Task<int> SaveAsync()
        //{
        //    return await _context.SaveChangesAsync();
        //}


    }
}

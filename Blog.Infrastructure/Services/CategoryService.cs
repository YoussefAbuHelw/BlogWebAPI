using Blog.Core.Interfaces;
using Blog.Core.Models;
using Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        // DI
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
            // More Methods
            //return await _context.Categories.FirstAsync(c => c.Id == id);
            //return await _context.Categories.LastAsync(c => c.Id == id);
            //return await _context.Categories.SingleAsync(c => c.Id == id);
            //return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            //return await _context.Categories.LastOrDefaultAsync(c => c.Id == id);
            //return await _context.Categories.SingleOrDefaultAsync(c => c.Id == id);
        }



        public async Task CreateAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            // get old Category 
            var newCatogory = await GetByIdAsync(category.Id);
            if (newCatogory is null) return false;
            // set new data
            newCatogory.Name = category.Name;
            // Update (not async)
            _context.Categories.Update(newCatogory);
            // save 
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(int id, Category category)
        {
            var newCatogory = await GetByIdAsync(id);
            if (newCatogory is null)
                return false;
            newCatogory.Name = category.Name;
            _context.Categories.Update(newCatogory);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
           var Category = await GetByIdAsync(id);
            if (Category is null) return false;
            _context.Categories.Remove(Category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

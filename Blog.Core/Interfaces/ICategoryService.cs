using Blog.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Interfaces
{
    public interface ICategoryService
    {
        // Get All Categories
        Task<IEnumerable<Category>> GetAllAsync();
        // Get By Id
        Task<Category?> GetByIdAsync(int id);
        // Create 
        Task CreateAsync(Category category);
        // Update
        Task<bool> UpdateAsync(Category category);
        Task<bool> UpdateAsync(int id, Category category);
        // Delete
        Task<bool> DeleteAsync(int id);
    }
}

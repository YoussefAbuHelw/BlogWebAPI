using Blog.Core.Interfaces;
using Blog.Core.Models;
using Blog.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Blog.Infrastructure.Services
{
    public class UnitOfWork : IUnitOfWork
    {

        // DI For DB
        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            CategoryService = new CategoryService(_context);
            Categories = new GenaricRepository<Category>(_context);
            Posts = new GenaricRepository<Post>(_context);
            Comments = new GenaricRepository<Comment>(_context);
            Users = new GenaricRepository<User>(_context);
        }


        // Keys
        public IGenaricRepository<Category> Categories { get; private set; }

        public IGenaricRepository<Post> Posts { get; private set; }

        public IGenaricRepository<Comment> Comments { get; private set; }

        public IGenaricRepository<User> Users { get; private set; }

        public ICategoryService CategoryService { get; private set; }

        // Method
        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}

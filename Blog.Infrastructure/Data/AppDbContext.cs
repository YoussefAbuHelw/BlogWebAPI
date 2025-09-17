﻿using Blog.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Data
{
    // class to control database
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


        // Set Database Tables
        public DbSet<Post> Posts { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Category> Categories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Set Pk
            //modelBuilder.Entity<Post>(entity =>
            //{
            //    entity.HasKey(p => p.Id);
            //});
            //modelBuilder.Entity<Post>().HasKey(p => p.Id);
            //modelBuilder.Entity<Post>()
            //    .Property(p => p.Title)
            //    .IsRequired()
            //    .HasMaxLength(50);
            //;

            // To make a Column unique
            modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();

            // To Rename column
            //modelBuilder.Entity<Category>().Property(c => c.Name).HasColumnName("CategoryName");
            // To Rename Table
            //modelBuilder.Entity<Category>().ToTable("NewTable");

            // Set Default Value
            modelBuilder.Entity<Post>().Property(p => p.CreatedAt)
                .HasDefaultValue(DateTime.Now);



            // Relations
            //modelBuilder.Entity<Post>()
            //    .HasOne(p => p.User)
            //    .WithMany(u => u.Posts)
            //    .HasForeignKey(p => p.UserID);





        }
    }
}

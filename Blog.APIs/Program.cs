using Blog.Core.Interfaces;
using Blog.Infrastructure.Data;
using Blog.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory; // Add this using directive

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Database Connection
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(
//            builder.Configuration.GetConnectionString("BlogDB")
//        )
//);


//if (builder.Environment.IsDevelopment())
//{
//    // For development/testing → use in-memory database
//    builder.Services.AddDbContext<AppDbContext>(options =>
//        options.UseInMemoryDatabase("BlogDB"));
//}
//else
//{
    // For production → use SQL Server
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("BlogDB")
        ));
//}



// Request Life Time

//builder.Services.AddScoped<ICategoryService, CategoryService>();
//builder.Services.AddScoped(typeof(IGenaricRepository<>), typeof(GenaricRepository<>));

//builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

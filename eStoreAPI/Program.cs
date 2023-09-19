using AutoMapper;
using BusinessObject.Models;
using DataAccessLayer.Repositories;
using eStoreAPI;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
var config = builder.Configuration;

services.AddControllers();
var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<MapperProfile>(); // Add your AutoMapper profiles here
                                           // Add more profiles if needed
});
services.AddSingleton(mapperConfig.CreateMapper());
services.AddDbContext<DatabaseContext>(options =>
    { options.UseSqlServer(config.GetConnectionString("Conn"));});
services.AddScoped<CategoryRepository>();
services.AddScoped<MemberRepository>();
services.AddScoped<OrderRepository>();
services.AddScoped<OrderDetailRepository>();
services.AddScoped<ProductRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

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

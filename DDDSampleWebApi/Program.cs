using AutoMapper;
using DDDSampleWebApi.Persistence;
using DDDSampleWebApi.Persistence.Repositories;
using DDDSampleWebApi.Services.Implementation;
using DDDSampleWebApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IHikerService, HikerService>();
builder.Services.AddScoped<IHikerRepository, HikerRepository>();
builder.Services.AddScoped<IItemsRepository, ItemsRepository>();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase(databaseName:"HikersDB"));

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
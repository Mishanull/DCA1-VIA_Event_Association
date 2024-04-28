using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.QueryContracts.Contract;
using ViaEventAssociation.Core.QueryContracts.ServiceRegistration;
using ViaEventAssociation.Infrastructure.EfcQueries.DTOs;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.ServiceRegistration;
using ViaEventsAssociation.Core.Application.AppEntry.ServiceRegistration;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=>{
 c.CustomSchemaIds(i => i.FullName);
});
builder.Services.AddControllers();

// database contexts registration
var baseDir = AppDomain.CurrentDomain.BaseDirectory;

var relativePath = Path.Combine(baseDir, "../../DbFile/VeaDb.db");

var absolutePath = Path.GetFullPath(relativePath);

var connectionString = $"Data Source={absolutePath};";

builder.Services.AddDbContext<WriteDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDbContext<VeaDbContext>(options => options.UseSqlite(connectionString));

builder.Services.RegisterRepositories();
builder.Services.AddCommandHandlers(Assembly.GetAssembly(typeof(ICommandHandler<>))!);
builder.Services.AddQueryHandlers(Assembly.GetAssembly(typeof(IQueryHandler<,>))!);
builder.Services.RegisterCommandDispatcher();
builder.Services.RegisterQueryDispatcher();

var app = builder.Build();
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.Domain.Services;
using ViaEventAssociation.Core.QueryContracts.Contract;
using ViaEventAssociation.Core.QueryContracts.ServiceRegistration;
using ViaEventAssociation.Infrastructure.EfcQueries.DTOs;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.ServiceRegistration;
using ViaEventAssociation.Presentation.WebAPI.ServiceRegistration;
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
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<WriteDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDbContext<VeaDbContext>(options => options.UseSqlite(connectionString));

builder.Services.RegisterRepositories();
builder.Services.AddCommandHandlers(Assembly.GetAssembly(typeof(ICommandHandler<>))!);
builder.Services.AddQueryHandlers(Assembly.GetAssembly(typeof(IQueryHandler<,>))!);
builder.Services.RegisterCommandDispatcher();
builder.Services.RegisterQueryDispatcher();
builder.Services.RegisterMappingConfigs();
builder.Services.AddSingleton<ICurrentTime, CurrentTime>();

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

public partial class Program{ }
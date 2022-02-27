using MediatR;
using Microsoft.EntityFrameworkCore;
using sample.tektonlabs.core.Handlers;
using sample.tektonlabs.core.Interfaces;
using sample.tektonlabs.core.Models;
using sample.tektonlabs.core.Profiles;
using sample.tektonlabs.infrastructure;
using sample.tektonlabs.infrastructure.Implementations;
using sample.tektonlabs.webapi;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
        .ReadFrom.Configuration(hostingContext.Configuration)
                );
// Add services to the container.
builder.Services.AddDbContext<MyContext>(o =>
{
    o.UseInMemoryDatabase("MyContext");
});
builder.Services.AddHttpClient();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(CommonProfiles));
builder.Services.AddMediatR(typeof(GetProductByKeyHandler));
builder.Services.AddLazyCache();
builder.Services.AddScoped<IExternalPriceProvider, ExternalPriceProvider>();
builder.Services.AddScoped<IRepository<Product>, Repository>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler(app => app.Run(httpContext => httpContext.HandleException(app.ApplicationServices.GetRequiredService<ILogger<object>>())));
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using Newtonsoft.Json.Converters;
using ShoppingCart.DataAccess.Repositories;
using ShoppingCart.Domain.Repositories;
using ShoppingCart.Web.Middleware;
using ShoppingCart.Web.Services;
using ShoppingCart.Web.Services.Abstraction;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options =>
        options.SerializerSettings.Converters.Add(new StringEnumConverter())); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddScoped<IServiceManager, ServiceManager>();

builder.Services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                                                       .AllowAnyMethod()
                                                        .AllowAnyHeader())).
                                                        AddMvc().AddJsonOptions(options =>
                                                        {
                                                            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                                                        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();

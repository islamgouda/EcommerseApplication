using EcommerseApplication.Models;
using EcommerseApplication.Respository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IProductCategory, ProductCategoryRespository>();
builder.Services.AddScoped<IDiscount, DiscountRepository>();
//
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
string con = builder.Configuration.GetConnectionString("cs");
builder.Services.AddDbContext<Context>(optionBuider =>
{
    optionBuider.UseSqlServer(con);
});
builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

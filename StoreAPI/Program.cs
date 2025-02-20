using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using StoreAPI.AutoMapper;
using StoreAPI.Models;
using StoreAPI.Services.CollectionProducts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StoreDbContext>();

//Add services to the container


builder.Services.AddAutoMapper(typeof(BannerProfile), typeof(BannerSlideProfile), typeof(CollectionProfile), typeof(ProductImageProfile),typeof(ProductProfile),typeof(ProductVariantProfile),typeof(StoreOptionProfile),typeof(UserProfile),typeof(VariantProfile),typeof(VariantTypeProfile),typeof(CollectionProductProfile));


// Register the FarmDBContext using the connection string from appsettings.json

builder.Services.AddDbContext<StoreDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<ICollectionProductService, CollectionProductService>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Set to Ignore to remove the $id fields
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});



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

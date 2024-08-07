using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using NomuBackend.Model;
using NomuBackend.Settings;
using Microsoft.Extensions.Options;
using NomuBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// MongoDB ayarlarını yapılandır
var mongoDbConfig = builder.Configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>();
if (mongoDbConfig == null || string.IsNullOrEmpty(mongoDbConfig.DatabaseName))
{
    throw new ApplicationException("MongoDbConfig or DatabaseName is not configured correctly");
}

// Identity ve MongoDB yapılandırması
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>(
        mongoDbConfig.ConnectionString, mongoDbConfig.DatabaseName
    )
    .AddDefaultTokenProviders();

// Authorization politikalarını ekle
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
});

// MongoDB konfigürasyonlarını ekle
builder.Services.Configure<MongoDbConfig>(
    builder.Configuration.GetSection(nameof(MongoDbConfig)));

builder.Services.AddSingleton<IMongoDbSettings>(sp =>
    sp.GetRequiredService<IOptions<MongoDbConfig>>().Value);

builder.Services.AddSingleton<IMongoClient>(s =>
{
    var config = s.GetRequiredService<IOptions<MongoDbConfig>>().Value;
    var connectionString = config.ConnectionString;

    if (string.IsNullOrEmpty(connectionString))
    {
        throw new ApplicationException("MongoDB connection string is not configured.");
    }

    return new MongoClient(connectionString);
});

// ProductService'i DI konteynerine ekleyin
builder.Services.AddScoped<IProductService, ProductService>(); // IProductService'i eklediğinizden emin olun

// MVC hizmetlerini ekle
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// HTTP istek boru hattı yapılandırması
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();

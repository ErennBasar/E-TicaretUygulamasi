using ECommerceAPI.Application;
using ECommerceAPI.Application.Validators.Products;
using ECommerceAPI.Infrastructure;
using ECommerceAPI.Infrastructure.Enums;
using ECommerceAPI.Infrastructure.Filters;
using ECommerceAPI.Infrastructure.Services.Storage.Azure;
using ECommerceAPI.Infrastructure.Services.Storage.Local;
using ECommerceAPI.Persistence;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Hata veriyorsa Presentation'da Persistence referans eklenmemiştir.
builder.Services.AddPersistenceServices(); 

builder.Services.AddInfrastructureServices();

builder.Services.AddApplicationServices();

//builder.Services.AddStorage(StorageType.Azure)
builder.Services.AddStorage<AzureStorage>();
//builder.Services.AddStorage<LocalStorage>(StorageType.Azure);

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod()
    ));

// Services üstüne geldiğinde IServiceCollection olduğu gözüküyor. Yani IoC konteynırına eklemem gerekenleri IServiceCollection üzerinden ekliyoruz.
builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
        // CreateProductValidator ekledik çünkü; ekleyeceğim başka validator'lar olursa otomatik olarak eklenecek.
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
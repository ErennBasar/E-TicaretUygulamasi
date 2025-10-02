using ECommerceAPI.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistenceServices(); // Hata veriyorsa Presentation'da Persistence referans eklenmemiştir.

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod()
    ));

// Services üstüne geldiğinde IServiceCollection olduğu gözüküyor. Yani IoC konteynırına eklemem gerekenleri IServiceCollection üzerinden ekliyoruz.
builder.Services.AddControllers(); 

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
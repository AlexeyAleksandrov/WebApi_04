var builder = WebApplication.CreateBuilder(args);

// добавляем контроллеры в контейнер сервисов
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// app.UseAuthorization();     // задел на будущее, пока авторизация не работает

// регистрируем маршрутизацию контроллеров
app.MapControllers();

app.Run();


// dotnet ef dbcontext scaffold "Host=localhost;Port=5432;Database=WebApi04;Username=postgres;Password=1111" Npgsql.EntityFrameworkCore.PostgreSQL
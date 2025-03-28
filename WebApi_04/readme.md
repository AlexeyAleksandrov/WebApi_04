# REST API
## ASP.NET, версия .Net 8.0
### Подготовка к запуску
1. В файле program.cs удалить демо-код.
2. Добавить в program.cs:
3. Добавляем контроллеры в контейнер сервисов
<br><code>builder.Services.AddControllers();</code><br>
5. Ниже:
<br><code>app.UseAuthorization();</code> – поддержка авторизации (будем использовать позже)
7. Регистрируем маршрутизацию контроллеров:
<br><code>app.MapControllers();</code>

<details>
    <summary>Полный код Program.cs:</summary>
    <code>
    Program.cs:

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
</code>
</details>

### Создание контроллеров
Для начала создадим папки Controllers, Database, DTO и Models
<br>В папке Controllers создадим класс UserController.

#### Создание класса контроллера
Для того, чтобы ASP.NET подхватил класс как контроллер, необходимо над классом добавить аннотацию <code>[ApiController]</code>, а также необходимо унаследовать класс от <code>ControllerBase</code>:
<code><br>
[ApiController]<br>
public class UsersController : ControllerBase<br>
{
}
</code>

А также, чтобы у контроллера появился путь, необхоимо написать аннотацию <code>[Route("api/v1/users")]</code><br>:
<code><br>
[ApiController]
[Route("api/v1/users")]
public class UsersController : ControllerBase
{
}
</code>

### Обработка запросов
Обаботка запросов производится при помощи аннотаций: <code>[HttpGet], [HttpPost], [HttpPut], [HttpPatch] и [HttpDelete] </code>

#### Обработка GET запросов
Для обработки GET запроса необходимо создать <code>public</code> метод, который должен возвращать объект <code>IACtionResult</code>. 
Название метода может быть любым, но реккомендуется начинать название со слова Get, например, <code>GetUsers()</code>
Над методом необходимо поставить аннотацию [HttpGet] с указанием маршрута, например, [HttpGet("all")]:
<code>
    [ApiController]
    [Route("api/v1/users")]
    public class UsersController : ControllerBase
    {
        // Обработка GET-запроса для получения всех пользователей
        [HttpGet("all")]
        public IActionResult GetUsers()
        {
            // Логика получения данных о пользователях
            return Ok(new List<string> { "User1", "User2" });
        }
    }
</code>
### Обработка POST-запросов
Для обработки POST-запросов необходимо создать метод, аннотированный [HttpPost], который будет принимать данные из тела запроса.

<code>
[HttpPost]
public IActionResult CreateUser([FromBody] UserDTO userDto)
{
// Логика создания нового пользователя
return CreatedAtAction(nameof(GetUserById), new { id = userDto.Id }, userDto);
}
</code>

### Обработка PUT-запросов
PUT-запросы используются для полного обновления ресурса.

<code>
[HttpPut("{id}")]
public IActionResult UpdateUser(int id, [FromBody] UserDTO userDto)
{
// Логика обновления пользователя
return NoContent();
}
</code>

### Обработка DELETE-запросов
DELETE-запросы позволяют удалять ресурс.

<code>
[HttpDelete("{id}")]
public IActionResult DeleteUser(int id)
{
    // Логика удаления пользователя
    return NoContent();
}
</code>

## Подключение базы данных
Для подключения к существующей БД необходимо установить следующие пакеты:
### Установка пакетов
* Microsoft.EntityFrameworkCore
* Microsoft.EntityFrameworkCore.Tools
* Npgsql.EntityFrameworkCore.PostgreSQL

### Маппинг таблиц
После установки пакетов необходимо выполнить маппинг таблиц в БД в класс в C#, командой:
<code>dotnet ef dbcontext scaffold "Host=localhost;Port=5432;Database=WebApi09;Username=postgres;Password=1111" Npgsql.EntityFrameworkCore.PostgreSQL</code>
Данную команду необходимо выполнять в папке с проектом. Перейти в папку с проектом можно командой `cd`.

Где:
* Host - IP адрес БД
* Port - порт БД
* Database - название базы данных
* Username - имя пользователя в СУБД
* Password - пароль пользователя
* Npgsql.EntityFrameworkCore.PostgreSQL - драйвер СУБД

### Подготовка проекта
После выполнения команды в папке с проектом создайте папку Models и перенесите в неё классы таблиц из БД. Создайте папку DataBase и перенесите в неё класс *Context.cs

## Работа с Базой Данных

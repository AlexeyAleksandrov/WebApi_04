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

Полный код Program.cs:

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


### Создание контроллеров
Для начала создадим папки Controllers, Database, DTO и Models
<br>В папке Controllers создадим класс UserController.

#### Создание класса контроллера
Для того, чтобы ASP.NET подхватил класс как контроллер, необходимо над классом добавить аннотацию <code>[ApiController]</code>, а также необходимо унаследовать класс от <code>ControllerBase</code>:

    [ApiController]
    public class UsersController : ControllerBase
    {
    }



А также, чтобы у контроллера появился путь, необхоимо написать аннотацию <code>[Route("api/v1/users")]</code><br>:

    [ApiController]
    [Route("api/v1/users")]
    public class UsersController : ControllerBase
    {
    }


### Обработка запросов
Обаботка запросов производится при помощи аннотаций: <code>[HttpGet], [HttpPost], [HttpPut], [HttpPatch] и [HttpDelete] </code>

#### Обработка GET запросов
Для обработки GET запроса необходимо создать <code>public</code> метод, который должен возвращать объект <code>IACtionResult</code>. 
Название метода может быть любым, но реккомендуется начинать название со слова Get, например, <code>GetUsers()</code>
Над методом необходимо поставить аннотацию `[HttpGet]` с указанием маршрута, например, [HttpGet("all")]:

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

### Обработка POST-запросов
Для обработки POST-запросов необходимо создать метод, аннотированный [HttpPost], который будет принимать данные из тела запроса.


    [HttpPost]
    public IActionResult CreateUser([FromBody] UserDTO userDto)
    {
    // Логика создания нового пользователя
    return CreatedAtAction(nameof(GetUserById), new { id = userDto.Id }, userDto);
    }


### Обработка PUT-запросов
PUT-запросы используются для полного обновления ресурса.

    [HttpPut("{id}")]
    public IActionResult UpdateUser(int id, [FromBody] UserDTO userDto)
    {
    // Логика обновления пользователя
    return NoContent();
    }

### Обработка DELETE-запросов
DELETE-запросы позволяют удалять ресурс.

    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        // Логика удаления пользователя
        return NoContent();
    }


## Подключение базы данных
Для подключения к существующей БД необходимо установить следующие пакеты:
### Установка пакетов
* Microsoft.EntityFrameworkCore
* Microsoft.EntityFrameworkCore.Tools
* Npgsql.EntityFrameworkCore.PostgreSQL

### Маппинг таблиц
После установки пакетов необходимо выполнить маппинг таблиц в БД в класс в C#, командой:

    dotnet ef dbcontext scaffold "Host=localhost;Port=5432;Database=WebApi09;Username=postgres;Password=1111" Npgsql.EntityFrameworkCore.PostgreSQL

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

## Работа с Базой Данных в C# в Entity Framework

### Использование using

В C#, `using` используется для автоматического освобождения ресурсов. Конструкция using гарантирует, что объекты, которые используют внешние ресурсы (сети, файлы, соединение с базой данных и т.д.), будут корректно освобождены, когда они больше не нужны. Это помогает избежать утечек ресурсов. В контексте базы данных это особенно важно, поскольку открытое соединение с базой может удерживать ресурсы и повлиять на производительность приложения.

#### Пример использования

    using (WebApi04Context dbContext = new WebApi04Context())
    {
    // Взаимодействие с базой данных
    }


Здесь создается объект `dbContext`, который управляет соединением с базой данных, и он будет автоматически закрыт по завершении блока using.

### LINQ

LINQ (Language Integrated Query) - мощный инструмент для запросов к коллекциям данных в C#. Он позволяет использовать знакомые SQL-подобные конструкции для получения, фильтрации и манипуляции данными прямо в памяти.

#### Формирование запросов в LINQ

LINQ предоставляет несколько подходов для создания запросов:

1. LINQ-методы: Более функциональный стиль, использующий методы расширения.
2. LINQ-запросы: Более SQL-подобный подход, использующий ключевые слова для создания запросов.

#### Примеры работы с пользователями
Рассмотрим стандартные операции работы с пользователем:
* Добавление;
* Получение;
* Редактирвоание;
* Удаление.
##### Добавление пользователя

Чтобы добавить пользователя в базу данных напишем следующий код:

Создадим пользователя и заполним его данными:

    WebApi_04.User user = new User();
    user.Login = AddingUser.Login;
    user.Password = AddingUser.Password;
    user.Age = AddingUser.Age;

Записываем созданного пользователя в БД:

    using (WebApi04Context dbContext = new WebApi04Context())
    {
        dbContext.Users.Add(user);  // Добавляет пользователя в DbSet
        dbContext.SaveChanges();    // Сохраняет изменения в базе данных
    }


##### Получение всех пользователей
Получения списка всех пользователей:

    using (WebApi04Context dbContext = new WebApi04Context())
    {
        var logins = (
                from user in dbContext.Users
                select user.Login
            ).ToList();   // Формирует список всех логинов
        return Ok(logins);
    }


##### Получение пользователя по ID

Выборка пользователя по ID:

    using (WebApi04Context dbContext = new WebApi04Context())
    {
        var UsersList = (
            from user in dbContext.Users
            where user.Id.Equals(Id)
            select user
        ).ToList();
    
        if (UsersList.Count == 0)
        {
            return StatusCode(404, "Пользователь не найден!");
        }
    
        var FindedUser = UsersList[0];
        // Дальнейшая работа с найденным пользователем
        return Ok(FindedUser);
    }

##### Редактирование пользователя

Обновление данных пользователя:

    using (WebApi04Context dbContext = new WebApi04Context())
    {
        var UsersList = (
            from user in dbContext.Users
            where user.Id.Equals(Id)
            select user
        ).ToList();
    
        if (UsersList.Count == 0)
        {
            return StatusCode(404, $"Пользователь {Id} не найден!");
        }
    
        var FindedUser = UsersList[0];
        FindedUser.Password = EditingUser.Password;
        FindedUser.Age = EditingUser.Age;
    
        dbContext.Users.Update(FindedUser);  // Обновляет данные пользователя
        dbContext.SaveChanges();             // Сохраняет изменения в базе данных
    
        return Ok(EditingUser);
    }

##### Удаление пользователя
Удаление пользователя из базы данных:

    using (WebApi04Context dbContext = new WebApi04Context())
    {
        var UsersList = (
            from user in dbContext.Users
            where user.Id.Equals(Id)
            select user
        ).ToList();
    
        if (UsersList.Count == 0)
        {
            return StatusCode(404, $"Пользователь {Id} не найден!");
        }
    
        var FindedUser = UsersList[0];
    
        dbContext.Users.Remove(FindedUser);  // Удаляет пользователя из DbSet
        dbContext.SaveChanges();             // Сохраняет изменения в базе данных
    
        return Ok(FindedUser);
    }

### Использование LINQ в C#

LINQ (Language Integrated Query) - это набор технологий на платформе .NET, которые позволяют писать запросы для доступа к данным легко, в понятной и простой форме. Используя LINQ, можно работать с данными из массивов, коллекций, баз данных и других источников.

#### Основные компоненты LINQ

#### 1. from

Ключевое слово `from` указывает источник данных, который будет использоваться в запросе. Оно похоже на оператор SELECT в SQL.

    var query = 
        from user in dbContext.Users 
        select user;


#### 2. where

`where` - это фильтр, который позволяет возвращать только те элементы, которые соответствуют заданным условиям.

    var query = 
        from user in dbContext.Users
        where user.Age > 18
        select user;


#### 3. select

`select` указывает, какие данные вы хотите получить от каждого элемента запроса. Вы можете выбрать весь объект, отдельные поля или даже создать новый тип данных.

    var query = 
        from user in dbContext.Users
        select new
        {
            user.Id,
            user.Login
        };


#### 4. orderby

`orderby` позволяет упорядочивать результаты в возрастающем или убывающем порядке. Комбинируется с ключевым словом ascending (по умолчанию) или descending.

    var query = 
        from user in dbContext.Users
        orderby user.Age descending
        select user;


#### 5. Использование join

`join` позволяет соединять данные из двух источников на основе общего ключа.

    var query = 
        from user in dbContext.Users
        join order in dbContext.Orders
            on user.Id equals order.UserId
        select new
        {
            user.Login,
            order.OrderId
        };


### Возврат кортежа

Вы можете использовать `select` для возврата анонимных типов, которые ведут себя подобно кортежам. 
Например:

    var query = 
        from user in dbContext.Users
        where user.Age > 18
        select (
            user.Login, 
            user.Age
        );


### Сбор данных из результата запроса

#### ToList()

`ToList()` создает список на основе результатов запроса. Это полезно, когда нужна коллекция, с которой можно работать позже.

    var userList = (
        from user in dbContext.Users
        select user
    ).ToList();


#### Count()

`Count()` возвращает количество элементов, удовлетворяющих запросу.

    int NumberOfAdults = (
        from user in dbContext.Users
        where user.Age >= 18
        select user
    ).Count();


#### Дополнительные методы

- `First() / FirstOrDefault()`: Возвращает первый элемент из запроса или значение по умолчанию, если результат пуст (например, `null` для ссылочных типов).

        var firstUser = (
                from user in dbContext.Users
                select user
            ).FirstOrDefault();


- `Any()`: Проверяет, содержится ли хотя бы один элемент в коллекции с заданным условием.

        bool hasAdults = (
                from user in dbContext.Users
                where user.Age >= 18
                select user
            ).Any();


- `Max() / Min()`: Получают максимальное или минимальное значение из коллекции.

        var maxAge = (
            from user in dbContext.Users
            select user.Age
        ).Max();
  
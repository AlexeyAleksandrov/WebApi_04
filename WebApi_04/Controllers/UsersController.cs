using Microsoft.AspNetCore.Mvc;
using WebApi_04.DTO;


namespace WebApi_04.Controllers;

[ApiController]
[Route("api/v1/users")]
public class UsersController : ControllerBase
{
    private static List<User> Users = new List<User>();

    [HttpPost("add")]
    public IActionResult AddUser([FromBody] UserDTO AddingUser)
    {
        WebApi_04.User user = new User();
        user.Login = AddingUser.Login;
        user.Password = AddingUser.Password;
        user.Age = AddingUser.Age;
        
        using (WebApi04Context dbContext = new WebApi04Context())
        {
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
        }
        
        return Ok(AddingUser);
    }

    [HttpGet("all")]
    public IActionResult GetAllUsers()
    {
        using (WebApi04Context dbContext = new WebApi04Context())
        {
            var logins = (
                from user in dbContext.Users
                select (user.Login))
                .ToList();
            return Ok(logins);
        }
    }

    [HttpGet("{Id}")]
    public IActionResult GetUserById(long Id)
    {
        using (WebApi04Context dbContext = new WebApi04Context())
        {
            var UsersList = (
                from user in dbContext.Users
                where user.Id.Equals(Id)
                select user).ToList();  // получаем список пользователей по ID
            
            if (UsersList.Count == 0)   // если не найдено никого
            {
                return StatusCode(404, "Пользователь не найден!");
            }

            var FindedUser = UsersList[0];  // достаём пользователя
            
            // готовим данные для ответа
            UserDTO userDto = new UserDTO();
            userDto.Id = FindedUser.Id;
            userDto.Login = FindedUser.Login;
            userDto.Age = FindedUser.Age;
            
            return Ok(userDto);
        }
    }

    [HttpPatch("edit/{Id}")]
    public IActionResult EditUser(long Id, [FromBody] UserDTO EditingUser)
    {
        using (WebApi04Context dbContext = new WebApi04Context())
        {
            var UsersList = (
                from user in dbContext.Users
                where user.Id.Equals(Id)
                select user).ToList();  // получаем список пользователей по ID
            
            if (UsersList.Count == 0)   // если не найдено никого
            {
                return StatusCode(404, $"Пользователь {Id} не найден!");
            }

            var FindedUser = UsersList[0];  // достаём пользователя
            
            FindedUser.Password = EditingUser.Password;
            FindedUser.Age = EditingUser.Age;

            dbContext.Users.Update(FindedUser);
            dbContext.SaveChanges();

            return Ok(EditingUser);
        }
    }

    [HttpDelete("del/{Id}")]
    public IActionResult DeleteUser(long Id)
    {
        using (WebApi04Context dbContext = new WebApi04Context())
        {
            var UsersList = (
                from user in dbContext.Users
                where user.Id.Equals(Id)
                select user).ToList();  // получаем список пользователей по ID
            
            if (UsersList.Count == 0)   // если не найдено никого
            {
                return StatusCode(404, $"Пользователь {Id} не найден!");
            }

            var FindedUser = UsersList[0];  // достаём пользователя

            dbContext.Users.Remove(FindedUser);
            dbContext.SaveChanges();

            return Ok(FindedUser);
        }
    }
}
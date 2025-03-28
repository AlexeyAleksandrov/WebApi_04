namespace WebApi_04.DTO;

public class UserDTO
{
    public long Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public int? Age { get; set; }
}
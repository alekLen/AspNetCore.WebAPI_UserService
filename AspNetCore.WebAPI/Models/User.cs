using Microsoft.EntityFrameworkCore;

namespace AspNetCore.WebAPI.Models
{
    
    public class User
    {
        public User(string login, string name,string surname, string email, string gender, string password)
        {
            Login = login;
            Name = name;
            Surname = surname;
            Email = email;
            Gender = gender;
            Password = password;
                   
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string? Salt { get; set; }
    }
}

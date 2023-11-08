using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspNetCore.WebAPI.Models;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace AspNetCore.WebAPI.Controllers
{
    [ApiController]
    [Route("api/Users")]
    public class UserController : ControllerBase
    {
        private readonly UserContext _context;

        public UserController(UserContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/3
        [HttpGet("{login},{password}")]
        public async Task<ActionResult<User>> GetUser(string login,string password)
        {
            User u = await _context.Users.SingleOrDefaultAsync(m => m.Login == login);
            if (u == null)
                return NotFound();
            if (CheckPassword(u.Salt,password,u.Password))
            {
                return new ObjectResult(u);
            }
            else
                return NotFound();           
        }
        [HttpGet("{login}")]
        public async Task<ActionResult<User>> GetUser(string login)
        {
            var User = await _context.Users.SingleOrDefaultAsync(m => m.Login == login);
            if (User == null)
            {
                return NotFound();
            }
            return new ObjectResult(User);
        }    
        // PUT: api/Users
        [HttpPut]
        public async Task<ActionResult<User>> PutUsers(User p)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var User = await _context.Users.SingleOrDefaultAsync(e => e.Id == p.Id);
            if (User==null)
            {
                return NotFound();
            }
            else
            {
                if(User.Password != p.Password)
                {
                    User.Salt = Salt();
                    User.Password = HashedPassword(User.Salt, p.Password);
                }else
                    User.Password =  p.Password;
                User.Login = p.Login;
                User.Email = p.Email;
                User.Surname = p.Surname;
                User.Name=p.Name;
                User.Gender= p.Gender;
                
            }

            _context.Update(User);
            await _context.SaveChangesAsync();
            return Ok(User);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User p)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User p1 = new(p.Login, p.Name, p.Surname, p.Email, p.Gender, p.Password);
            p1.Salt = Salt();
            p1.Password=HashedPassword(p1.Salt, p.Password);
            _context.Users.Add(p1);
            await _context.SaveChangesAsync();

            return Ok(p1);
        }

        // DELETE: api/Users/3
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var p = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
            if (p == null)
            {
                return NotFound();
            }

            _context.Users.Remove(p);
            await _context.SaveChangesAsync();

            return Ok(p);
        }
        string Salt()
        {
            byte[] saltbuf = new byte[16];
            RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(saltbuf);
            StringBuilder sb = new StringBuilder(16);
            for (int i = 0; i < 16; i++)
                sb.Append(string.Format("{0:X2}", saltbuf[i]));
            return sb.ToString();           
        }
        string HashedPassword(string salt,string pass)
        {
            string password = salt + pass;
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return hashedPassword ;
        }
        bool CheckPassword(string salt,string password,string userPassword)
        {
            string conf = salt + password;
            if (BCrypt.Net.BCrypt.Verify(conf, userPassword))
                return true;
            else
                return false;
        }

    }
}

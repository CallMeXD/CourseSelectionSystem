using CourseSelectionSystem.Data;
using CourseSelectionSystem.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CourseSelectionSystem.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;
        public UserService(AppDbContext context)
        {
            _context = context;
        }

        // --- 密码哈希方法 ---
        public string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // --- 登录验证 ---
        public User Authenticate(string userId, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserID == userId);

            if (user == null)
            {
                return null;
            }

            string hashedPassword = HashPassword(password);

            if (hashedPassword == user.PasswordHash)
            {
                return user;
            }

            return null;
        }

        // --- 注册新用户 ---
        public bool Register(string userId, string userName, string password, string role)
        {
            try
            {
                if (_context.Users.Any(u => u.UserID == userId))
                {
                    return false; // 账号已存在
                }

                string hashedPassword = HashPassword(password);

                var newUser = new User
                {
                    UserID = userId,
                    UserName = userName,
                    PasswordHash = hashedPassword,
                    Role = role
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
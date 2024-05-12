using System.Security.Cryptography;
using System.Text;

namespace HarmonyAPI.Model
{
    /// <summary>
    /// Создаем пользователя для хранения в бд
    /// Шифруем пароль солью
    /// Храним соль и пароль в базе
    /// При авторизации достаем данные из базы и сверяем с теми что нам пришли
    /// </summary>
    public class User
    {
        public Guid Id { get; set; }
        public Guid StockDetailId { get; set; }
        public string Login { get; private set; }
        public string EncryptedPassword { get; private set; }
        public string Salt { get; private set; }

        public User() { }

        public User(string login, string password)
        {
            Login = login;
            Salt = GenerateSalt();
            EncryptedPassword = EncryptPassword(password, Salt);
        }

        private string GenerateSalt()
        {
            byte[] bytes = new byte[16];
            using (var keyGenerator = RandomNumberGenerator.Create())
            {
                keyGenerator.GetBytes(bytes);
                return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
            }
        }

        private string EncryptPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = string.Concat(salt, password);
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLowerInvariant();
            }
        }

        public bool CheckPassword(string password)
        {
            return EncryptPassword(password, Salt) == EncryptedPassword;
        }
    }
}

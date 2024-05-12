
namespace WorkerClient.Model
{
    public class AuthData
    {
        public Guid Id { get; private set; }
        public string Token { get; private set; }

        private static AuthData instance;
        private static readonly object _lock = new object();
        private AuthData() { }
        
        private  AuthData(Guid id,string token)
        {
            Id = id;
            Token = token;
        }
        public static AuthData getInstance(Guid id = new Guid(), string token = "")
        {
            if (instance == null)
                instance = new AuthData(id, token);
                
            return instance;
        }


    }
}

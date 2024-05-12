using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using WorkerClient.Model;

namespace WorkerClient
{
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
        }

        public async Task<bool> Authenticate(User user)
        {
            var jsonContent = JsonContent.Create(user);
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage? response=null;
                try
                {
                    response = client.PostAsync("https://localhost:7229/user", jsonContent).Result;
                }
                catch(Exception ex) 
                {
                    MessageBox.Show("Ошибка соединения: "+ex.Message);
                }
                if (response.IsSuccessStatusCode)
                {
                    var token = response.Content.ReadFromJsonAsync<AuthDTO>().Result;
                    if (token != null)
                    {
                        var o1=AuthData.getInstance(token.Id, token.Token);
                        return true;
                        
                    }
                    
                }
            }
            return false;
        }
        void Login(string login, string password)
        {
            bool isAuthenticated = Authenticate(new User { Login = login, Password = password }).Result;

            if (isAuthenticated)
            {
                // Если аутентификация успешна, открываем основное окно с таблицей
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.");
            }
        }


        private void Login_Btn_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordBox.Password;

            
            Login(login, password);

        }

        private void User1_Data_Btn_Click(object sender, RoutedEventArgs e)
        {
            string login = "Mechta1";
            string password = "testpass1";
            Login(login, password);

        }
        private void User2_Data_Btn_Click(object sender, RoutedEventArgs e)
        {
            string login = "Mechta2";
            string password = "testpass2";
            Login(login, password);

        }
        private void User3_Data_Btn_Click(object sender, RoutedEventArgs e)
        {
            string login = "Mechta3";
            string password = "testpass3";
            Login(login, password);

        }


    }
}

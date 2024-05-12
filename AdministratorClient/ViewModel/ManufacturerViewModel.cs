using AdministratorClient.Command;
using AdministratorClient.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Windows;


namespace AdministratorClient.ViewModel
{
    public class ManufacturerViewModel : INotifyPropertyChanged
    {
        public ManufacturerViewModel()
        {
            GetManufacturers();
            
        }
        private const string path = "http://localhost:5125";
        private ObservableCollection<Manufacturer> manufacturers;
        public ObservableCollection<Manufacturer> Manufacturers
        {
            get { return manufacturers; }
            set
            {
                if (manufacturers != value)
                {
                    manufacturers = value;
                    OnPropertyChanged("Manufacturers");
                }
            }
        }


        private string addName="";
        public string AddName
        {
            get { return addName; }
            set
            {
                addName = value;
                OnPropertyChanged("AddName");
            }
        }

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      int code = 0;
                      code=AddManufacturer(AddName);
                      if (code == 200)
                          GetManufacturers();
                      else if(code==490)
                          MessageBox.Show("Такой производитель уже есть в базе.\n Данные не добалены");
                      else
                          MessageBox.Show("Произошла ошибка\n Данные не добалены");



                  }));
            }
        }

        private RelayCommand editCommand;
        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ??
                    (editCommand = new RelayCommand(obj =>
                    {

                        if (obj is Manufacturer manuf)
                        {
                            EditManufacturers(manuf);
                        }

                    }));
            }
        }

        private RelayCommand removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                    (removeCommand = new RelayCommand(obj =>
                    {
                        int code = 0;
                        if (obj is Manufacturer manuf)
                        {
                            code = DeleteManufacturers(manuf);
                            if (code == 200)
                                Manufacturers.Remove(manuf);
                            else
                                MessageBox.Show("Произошла ошибка\n Данные не удалены");
                        }

                        


                    },
                    (obj) => Manufacturers.Count > 0));
            }
        }

        private Manufacturer selectedManufacturer;
        public Manufacturer SelectedManufacturer
        {
            get { return selectedManufacturer; }
            set
            {
                selectedManufacturer = value;
                OnPropertyChanged("SelectedManufacturer");
            }
        }

        private void GetManufacturers()
        {
            using(HttpClient client = new HttpClient())
            {
                HttpResponseMessage? resp=null;
                try
                {
                    resp = client.GetAsync($"{path}/manufacturer").Result;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось подключиться к серверу");
                }
                if (resp != null)
                Manufacturers = resp.Content.ReadFromJsonAsync<ObservableCollection<Manufacturer>>().Result;
            } 
        }

        private int AddManufacturer(string name)
        {
            //Сперва проверяем есть ли в нашей модели такой производитель
            //Если нет то отправляем запрос на сервер
            if (Manufacturers.Any(item => item.Name == name))
                return 490;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage? resp = null;
                try
                {
                    resp = client.PostAsJsonAsync($"{path}/manufacturer", name).Result;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось подключиться к серверу");
                }
                if (resp != null && resp.IsSuccessStatusCode)
                {

                    Manufacturers = resp.Content.ReadFromJsonAsync<ObservableCollection<Manufacturer>>().Result;
                    return 200;
                }
                else if ((int)resp.StatusCode == 490)
                {

                    return 490;
                }
                else
                    return 0;

            }

        }

        private void EditManufacturers(Manufacturer manufacturer)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage? resp = null;
                try
                {
                    resp = client.PutAsJsonAsync($"{path}/manufacturer", manufacturer).Result;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось подключиться к серверу");
                }
                
            }
        }
        private int DeleteManufacturers(Manufacturer manufacturer)
        {

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage? resp = null;
                try
                {
                    resp = client.DeleteAsync($"{path}/manufacturer/{manufacturer.Id}").Result;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось подключиться к серверу");
                }
                if (resp.IsSuccessStatusCode)
                    return 200;
                else if ((int)resp.StatusCode == 490)
                {

                    return 490;
                }
                else
                    return 0;

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

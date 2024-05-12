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
    public class ProductViewModel : INotifyPropertyChanged
    {
        public ProductViewModel()
        {
            GetProducts();
            GetManufacturers();
            GetCategories();

        }
        private const string path = "http://localhost:5125";
        private ObservableCollection<Product> products;
        public ObservableCollection<Product> Products
        {
            get { return products; }
            set
            {
                if (products != value)
                {
                    products = value;
                    OnPropertyChanged("Products");
                }
            }
        }

        private ObservableCollection<Manufacturer> manufacturers;
        public ObservableCollection<Manufacturer> Manufacturers
        {
            get { return manufacturers; }
            set
            {
                if (manufacturers != value)
                {
                    manufacturers = value;
                    OnPropertyChanged(nameof(Manufacturers));
                }
            }
        }
        private Manufacturer manufacturer;
        public Manufacturer Manufacturer
        {
          get { return manufacturer; }
          set
            {
                
                manufacturer = value;
                OnPropertyChanged(nameof(Manufacturer));

            }

        }
        private ObservableCollection<Category> categories;
        public ObservableCollection<Category> Categories
        {
            get { return categories; }
            set
            {
                if (categories != value)
                {
                    categories = value;
                    OnPropertyChanged(nameof(Categories));
                }
            }
        }
        private Category category;
        public Category Category
        {
            get { return category; }
            set
            {

                category = value;
                OnPropertyChanged(nameof(Category));

            }

        }
        private Product product = new();
        public Product Product
        {
            get { return product; }
            set
            {
                product = value;
                OnPropertyChanged(nameof(Product));
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
                      code = AddProduct(Product);

                      if (code == 200)
                          GetProducts();
                      else
                          MessageBox.Show("Произошла ошибка\n Данные не добалены");

                  }));
            }
        }


        private void GetProducts()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage? resp = null;
                try
                {
                    resp = client.GetAsync($"{path}/product").Result;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось подключиться к серверу");
                }
                if (resp != null)
                    Products = resp.Content.ReadFromJsonAsync<ObservableCollection<Product>>().Result;
                
            }
        }
        private void GetCategories()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage? resp = null;
                try
                {
                    resp = client.GetAsync($"{path}/category").Result;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось подключиться к серверу");
                }
                if (resp != null)
                    Categories = resp.Content.ReadFromJsonAsync<ObservableCollection<Category>>().Result;

            }
        }
        /// <summary>
        /// Получаем список производителей для выпадающего списка
        /// </summary>
        private void GetManufacturers()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage? resp = null;
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

        private int AddProduct(Product product)
        {
            //Добавляем Id производителя и категории которые выбраны из списка
            product.ManufacturerId=Manufacturer.Id;
            product.CategoryId = Category.Id;


            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage? resp = null;
                try
                {
                    resp = client.PostAsJsonAsync($"{path}/product", product).Result;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось подключиться к серверу");
                }
                if (resp != null && resp.IsSuccessStatusCode)
                {

                    Products = resp.Content.ReadFromJsonAsync<ObservableCollection<Product>>().Result;
                    return 200;
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

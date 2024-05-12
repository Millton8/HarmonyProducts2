using AdministratorClient.Model;
using AdministratorClient.ViewModel;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;

namespace AdministratorClient.API
{
    internal class ProductAPIASPv2: IProductAPI
    {
        private const string path = "https://localhost:7229";
        public ObservableCollection<Product> GetProducts()
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
                    return resp.Content.ReadFromJsonAsync<ObservableCollection<Product>>().Result;
                return null;
                
            }
        }

        public ObservableCollection<Manufacturer> GetManufacturers()
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
                    return resp.Content.ReadFromJsonAsync<ObservableCollection<Manufacturer>>().Result;
                return null;
            }
        }

        public ObservableCollection<Category> GetCategories()
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
                    return resp.Content.ReadFromJsonAsync<ObservableCollection<Category>>().Result;
                return null;

            }
        }

        public ObservableCollection<Product> AddProduct(Product product,Manufacturer? manufacturer, Category? category, out int status)
        {
            status=0;
            //Добавляем Id производителя и категории которые выбраны из списка
            if (manufacturer != null)
            {
                product.ManufacturerId = manufacturer.Id;
            }
            if (category != null)
            {
                product.CategoryId = category.Id;
            }

            //Если сперва меняли продукт
            //Проверяем id

            if (product.Id != Guid.Empty)
            {
                product.Id = Guid.Empty;
                //product.Category = null;
                product.Manufacturer = null;
            }

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
                    status = 200;
                    return resp.Content.ReadFromJsonAsync<ObservableCollection<Product>>().Result;



                }
                else
                    return null;

            }

        }
        public int EditProduct(Product product, Manufacturer? manufacturer, Category? category)
        {
            //Проверяем были ли изменены значения в списках
            //Если да то изменяем значение в продукте
            //Если нет то оставляем текущее
            if (manufacturer != null)
            {
                product.ManufacturerId = manufacturer.Id;
            }
            if (category != null)
            {
                product.CategoryId = category.Id;
            }
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage? resp = null;
                try
                {
                    resp = client.PutAsJsonAsync($"{path}/product", product).Result;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось подключиться к серверу");
                }
                if (resp.IsSuccessStatusCode)
                    return 200;
                else
                    return 0;

            }
        }

        public int DeleteProduct(Product product)
        {

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage? resp = null;
                try
                {
                    resp = client.DeleteAsync($"{path}/product/{product.Id}").Result;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось подключиться к серверу");
                }
                if (resp.IsSuccessStatusCode)
                    return 200;
                else
                    return 0;

            }
        }
        
    }
}

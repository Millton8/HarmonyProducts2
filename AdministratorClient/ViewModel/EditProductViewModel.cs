using AdministratorClient.Command;
using AdministratorClient.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace AdministratorClient.ViewModel
{
    public class EditProductViewModel : INotifyPropertyChanged
    {
        public EditProductViewModel(IProductAPI api)
        {
            Products=api.GetProducts();
            Manufacturers=api.GetManufacturers();
            Categories=api.GetCategories();
            this.api= api;
            //Создаем хэш имен продуктов для быстрой проверки перед добавлением
            //Нам нет смысла делать задачу через await
            //Также не важно успеет ли полность сформироваться список имен
            //В бд все продукты должны иметь уникальные имена
            //Это просто защита от лишних обращений к базе.
            Task.Run(() => GetProductNames());


        }
        private const string path = "http://localhost:5125";
        //Хэш имен продуктов для быстрой проверки перед добавлением
        private static HashSet<string> productNames = new HashSet<string>();
        //API для взаимодействия с сервером
        private IProductAPI api;



        private ObservableCollection<Product> products;
        public ObservableCollection<Product> Products
        {
            get { return products; }
            set
            {
                if (products != value)
                {
                    products = value;
                    OnPropertyChanged(nameof(Products));
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


        private Product selectedProduct = new();
        public Product SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
                //Если продукты не выбран то кнопки Изменить Удалить - недоступны
                OnPropertyChanged(nameof(IsUpdateEnabled));
            }
        }
        //Проверка на то был ли выбран продукт
        public bool IsUpdateEnabled
        {
            get
            {
                if (SelectedProduct != null)
                    return SelectedProduct.Id != Guid.Empty;
                return false;
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
                        int code = 0;
                        if (obj is Product prod)
                        {
                            code=api.EditProduct(prod,Manufacturer,Category);
                            if (code == 200)
                            {
                                MessageBox.Show("Данные обновлены");
                            }
                            else
                                MessageBox.Show("Произошла ошибка\n Данные не обновлены");
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
                        if (obj is Product prod)
                        {
                            code = api.DeleteProduct(prod);
                            if (code == 200)
                            {
                                Products.Remove(prod);
                                MessageBox.Show("Данные удалены");
                            }
                            else
                                MessageBox.Show("Произошла ошибка\n Данные не удалены");
                        }




                    }));
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
                      if (obj is Product prod)
                      {

                          int code = 0;
                          //Сперва проверяем есть ли у нас такой продукт
                          if (isExist())
                          {
                              MessageBox.Show("Продукт с таким названием уже есть\n" +
                                              "Данные не добавлены");
                              return;
                          }
                          var productsAfterAdd = api.AddProduct(prod, Manufacturer, Category, out code);
                          //Проверяем ответ от сервера
                          //Если ответ нас устраивает то меняем данные о продуктах
                          //Если нет то данные в VM не меняются и пользователь может с ними работать
                          if (code == 200)
                          {
                              Products = productsAfterAdd;
                              SelectedProduct = new();

                          }
                          else
                              MessageBox.Show("Произошла ошибка\n Данные не добалены");
                      }



                  }));
            }
        }


        private bool isExist()
        {
            return productNames.Contains(selectedProduct.Name);

        }
        private async Task GetProductNames()
        {
            foreach (var product in products)
            {
                productNames.Add(product.Name);

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

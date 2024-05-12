using AdministratorClient.Model;
using System.Collections.ObjectModel;


namespace AdministratorClient.ViewModel
{
    public interface IProductAPI
    {
        ObservableCollection<Product> GetProducts();
        ObservableCollection<Manufacturer> GetManufacturers();
        ObservableCollection<Category> GetCategories();
        ObservableCollection<Product> AddProduct(Product product, Manufacturer? manufacturer, Category? category, out int status);
        int EditProduct(Product product, Manufacturer? manufacturer, Category? category);
        int DeleteProduct(Product product);



    }
}
using AdministratorClient.API;

namespace AdministratorClient.ViewModel
{
    public class MainViewModel
    {
        public ManufacturerViewModel ManufacturerTab { get; set; }
        public ProductViewModel ProductTab { get; set; }
        public EditProductViewModel EditProductTab { get; set; }

        public MainViewModel() 
        {
            ManufacturerTab = new ManufacturerViewModel();
            ProductTab = new ProductViewModel();
            //Типизируем VM нужным нам классом реализующим интерфейс IProductAPI
            EditProductTab = new EditProductViewModel(new ProductAPIASPv2());
        }
    }
}

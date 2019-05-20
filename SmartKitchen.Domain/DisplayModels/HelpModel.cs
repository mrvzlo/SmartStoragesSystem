namespace SmartKitchen.Domain.DisplayModels
{
    public class HelpModel
    {
        public int StorageTypesCount { get; set; }
        public int ProductCount { get; set; }

        public HelpModel(int types, int products)
        {
            StorageTypesCount = types;
            ProductCount = products;
        }
    }
}

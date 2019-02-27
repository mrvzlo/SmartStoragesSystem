using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.DisplayModels
{
	public class ProductDisplayModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Usages { get; set; }
    }
}
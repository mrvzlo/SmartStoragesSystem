namespace SmartKitchen.Domain.DisplayModels
{
    public class CellAmountChange
    {
        public decimal Decrease { get; set; }
        public int TotalHours { get; set; }

        public decimal DecreasePerHour() => TotalHours == 0 ? 0 : Decrease / TotalHours;
    }
}
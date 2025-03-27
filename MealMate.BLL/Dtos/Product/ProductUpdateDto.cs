namespace MealMate.BLL.Dtos.Product
{
    public class ProductUpdateDto
    {
        public string? Image { get; init; }
        public string? Name { get; init; }
        public int? Amount { get; init; }
        public double? Price { get; init; }
        public string? Description { get; init; }

        public void Deconstruct(out string? image, out string? name, out int? amount, out double? price, out string? description)
        {
            image = Image;
            name = Name;
            amount = Amount;
            price = Price;
            description = Description;
        }
    }
}

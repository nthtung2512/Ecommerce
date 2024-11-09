namespace MealMate.BLL.Dtos.Product
{
    public class ProductUpdateDto
    {
        public string? Category { get; init; }
        public string? Description { get; init; }
        public double? Price { get; set; }
        public int? Weight { get; init; }
        public string? ImageURL { get; init; }

        public void Deconstruct(out string? category, out string? description, out double? price, out int? weight, out string? imageURL)
        {
            category = Category;
            description = Description;
            price = Price;
            weight = Weight;
            imageURL = ImageURL;
        }
    }
}

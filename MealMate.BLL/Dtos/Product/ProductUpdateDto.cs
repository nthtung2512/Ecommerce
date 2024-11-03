namespace MealMate.BLL.Dtos.Product
{
    public class ProductUpdateDto
    {
        public string? Category { get; init; }
        public string? Description { get; init; }
        public string? PName { get; init; }
        public double? Price { get; set; }
        public double? Weight { get; init; }
        public string? ImageURL { get; init; }

        public void Deconstruct(out string? category, out string? description, out string? pName, out double? price, out double? weight, out string? imageURL)
        {
            category = Category;
            description = Description;
            pName = PName;
            price = Price;
            weight = Weight;
            imageURL = ImageURL;
        }
    }
}

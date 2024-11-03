namespace MealMate.BLL.Dtos.Stores
{
    public class StoreDto
    {
        public required Guid StoreID { get; init; }
        public required string Name { get; init; }
        public required DateTime OpeningDate { get; init; }
        public required string ContactInfo { get; init; }
        public required string Location { get; init; }
    }
}

namespace MealMate.BLL.Dtos.Stores
{
    public class ATDto
    {
        public required Guid ProductID { get; init; }
        public required Guid StoreID { get; init; }
        public required int NumberAtStore { get; init; }
    }
}

namespace MealMate.DAL.Entities.Transactions
{
    public class TempTop5Product
    {
        public Guid ProductID { get; init; }
        public required string Name { get; init; }
        public required double Revenue { get; init; }
    }
}

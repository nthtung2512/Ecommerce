namespace MealMate.DAL.Entities.Transactions
{
    public class CartItem
    {
        public Guid ProductID { get; set; }
        public int Quantity { get; set; }
        public Guid StoreID { get; set; }
    }
}

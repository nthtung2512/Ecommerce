namespace MealMate.BLL.Dtos.Delivery
{
    public class RouteRequest
    {
        public string ShopAddress { get; set; }
        public List<string> DeliveryAddresses { get; set; }
    }
}

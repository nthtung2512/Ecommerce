namespace MealMate.BLL.Dtos.Delivery
{
    public class RouteResult
    {
        public List<string> OptimalRoute { get; set; }
        public double TotalDistance { get; set; } // in meters
    }
}

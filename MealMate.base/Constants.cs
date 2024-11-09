namespace MealMate.Base
{
    public static class Constants
    {
        public static DateTime Now => DateTime.UtcNow.AddHours(7).ToUniversalTime();
    }
}

namespace MealMate.Base.Extensions
{
    public static class StringExtensions
    {
        public static string RemovePostFix(this string url, string postFix)
        {
            if (url.EndsWith(postFix))
            {
                return url[..^postFix.Length];
            }
            return url;
        }
    }
}

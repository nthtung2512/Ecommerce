namespace MealMate.Base.Hub
{
    public interface IHubContextWrapper<T>
        where T : IHubClient
    {
        T Client(string connId);
        T Group(string groupName);
    }
}
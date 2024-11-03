using System.Net;

namespace MealMate.DAL.Utils.Exceptions
{
    public class EntityNotFoundException(string message) : DomainException(message)
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
    }
}

using System.Net;

namespace MealMate.DAL.Utils.Exceptions
{
    public class EntityBadRequestException(string message) : DomainException(message)
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
    }
}

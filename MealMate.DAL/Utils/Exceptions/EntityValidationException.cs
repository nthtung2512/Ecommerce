using System.Net;

namespace MealMate.DAL.Utils.Exceptions
{
    public class EntityValidationException(string message) : DomainException(message)
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.UnprocessableEntity;
    }
}

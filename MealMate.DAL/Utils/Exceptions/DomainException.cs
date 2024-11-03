using System.Net;

namespace MealMate.DAL.Utils.Exceptions
{
    public abstract class DomainException : Exception
    {
        protected DomainException(string? message)
            : base(message) { }

        public abstract HttpStatusCode StatusCode { get; }
    }

}

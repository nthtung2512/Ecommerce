using MealMate.DAL.Entities.Payment.VNPay;
using Microsoft.AspNetCore.Http;

namespace MealMate.BLL.IServices.Payment
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);

    }
}

using MealMate.DAL.Entities.Payment;
using Microsoft.AspNetCore.Http;

namespace MealMate.BLL.IServices.Payment
{
    public interface IMomoService
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model);
        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
    }
}

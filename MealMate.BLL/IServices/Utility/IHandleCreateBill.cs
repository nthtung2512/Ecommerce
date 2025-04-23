namespace MealMate.BLL.IServices.Utility
{
    public interface IHandleCreateBill
    {
        Task CreateBulkBillPrevDay(Guid storeId);
    }
}

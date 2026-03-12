namespace WebAPI.RequestModels.OrderRequestModels;

public class GetOrderRequestModel
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 100;
}

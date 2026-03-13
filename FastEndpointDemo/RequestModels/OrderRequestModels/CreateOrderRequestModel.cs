namespace WebAPI.RequestModels.OrderRequestModels;

public class CreateOrderRequestModel
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

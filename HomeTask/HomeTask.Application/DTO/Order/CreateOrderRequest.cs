namespace HomeTask.Application.DTO.Order
{
    public class CreateOrderRequest
    {
        public string Description { get; set; }
        public int ClientId { get; set; }

        public CreateOrderRequest(string description, int clientId)
        {
            Description = description;
            ClientId = clientId;
        }
    }
}
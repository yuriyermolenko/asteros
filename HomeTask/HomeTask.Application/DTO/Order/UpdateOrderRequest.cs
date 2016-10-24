namespace HomeTask.Application.DTO.Order
{
    public class UpdateOrderRequest
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int ClientId { get; set; }

        public UpdateOrderRequest(int id, string description, int clientId)
        {
            Id = id;
            Description = description;
            ClientId = clientId;

        }
    }
}
namespace HomeTask.Application.DTO.Client
{
    public class CreateClientRequest
    {
        public string Address { get; }
        public bool VIP { get; }

        public CreateClientRequest(string address, bool vip)
        {
            Address = address;
            VIP = vip;
        }
    }
}
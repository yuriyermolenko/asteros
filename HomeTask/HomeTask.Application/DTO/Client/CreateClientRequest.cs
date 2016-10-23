namespace HomeTask.Application.DTO.Client
{
    public class CreateClientRequest
    {
        public string Address { get; private set; }
        public bool VIP { get; private set; }

        public CreateClientRequest(string address, bool vip)
        {
            Address = address;
            VIP = vip;
        }
    }
}
using PacketParser.Entities;

namespace PacketParser.EventArgs.Customer;

public class CustomerAddressSaveRequestEventArgs : BaseEventArgs
{
	public WebCustomerAddress CustomerAddress { get; set; }
}

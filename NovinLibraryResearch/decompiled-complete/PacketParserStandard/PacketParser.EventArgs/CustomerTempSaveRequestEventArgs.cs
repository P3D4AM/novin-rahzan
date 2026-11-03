using Newtonsoft.Json;
using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class CustomerTempSaveRequestEventArgs : BaseEventArgs
{
	[JsonIgnore]
	public WebCus TempCustomer { get; set; }

	public string ClientIp { get; set; }

	public static CustomerTempSaveRequestEventArgs FromString(Packet pack, string data)
	{
		CustomerTempSaveRequestEventArgs customerTempSaveRequestEventArgs = data.FromJson<CustomerTempSaveRequestEventArgs>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\Customer\\CustomerTempSaveRequestEventArgs.cs", 14);
		customerTempSaveRequestEventArgs.Part = pack.Part;
		customerTempSaveRequestEventArgs.TotalParts = pack.TotalParts;
		customerTempSaveRequestEventArgs.PacketReferenceId = pack.PacketReferenceId;
		return customerTempSaveRequestEventArgs;
	}
}

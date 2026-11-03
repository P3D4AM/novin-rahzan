using PacketParser.Entities;

namespace FormCustom.ServiceManagement;

public class ServiceFailureConfig
{
	public string ServiceName { get; set; }

	public ReturnedState State { get; set; }

	public string ErrorMessage { get; set; }
}

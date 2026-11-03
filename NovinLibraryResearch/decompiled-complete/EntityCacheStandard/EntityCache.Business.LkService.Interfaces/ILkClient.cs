namespace EntityCache.Business.LkService.Interfaces;

public interface ILkClient
{
	int Port { get; set; }

	string Ip { get; set; }

	long LkSerial { get; set; }

	bool IsClient { get; set; }
}

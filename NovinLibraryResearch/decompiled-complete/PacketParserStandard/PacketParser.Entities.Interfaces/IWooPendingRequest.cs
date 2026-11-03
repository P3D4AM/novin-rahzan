namespace PacketParser.Entities.Interfaces;

internal interface IWooPendingRequest
{
	string UserName { get; set; }

	string Password { get; set; }

	int Id { get; set; }

	WooRequestType RequestType { get; set; }
}

namespace PacketParser.Entities;

public enum BioMetricActions
{
	Unknown = 0,
	SaveRequest = 1,
	SaveResponse = 2,
	ReadRequest = 5,
	ReadResponse = 6,
	Reset = 7,
	DeleteRequest = 8,
	DisconnectRequest = 9,
	DisconnectResponse = 10
}

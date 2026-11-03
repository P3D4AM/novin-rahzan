using PacketParser.Entities;
using PacketParser.PosToPc.Sadad.Classes;

namespace PacketParser.PosToPc.Sadad.Interface;

internal interface IModem
{
	EnSadadDeviceType DeviceType { get; set; }

	bool HasTimeoutError { get; set; }

	EnSadadPcPosConnectionType MediaType { get; set; }

	int Timeout { get; }

	void CancelReceive();

	void Connect();

	void Disconnect();

	byte[] Receive();

	byte[] Receive(int timeout);

	void Send(PcPosBaseMessage message);

	void SetTimeout(int? timeout);
}

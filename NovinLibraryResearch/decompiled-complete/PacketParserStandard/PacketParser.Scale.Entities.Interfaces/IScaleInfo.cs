using System;
using PacketParser.Entities;

namespace PacketParser.Scale.Entities.Interfaces;

public interface IScaleInfo
{
	bool AccDate { get; set; }

	bool AccDivision { get; set; }

	bool AccID { get; set; }

	bool AccIP { get; set; }

	bool AccMemory { get; set; }

	ConnectionType ConnetcionType { get; set; }

	bool DeleteFlag { get; set; }

	string Description { get; set; }

	string EndMessage { get; set; }

	short FactorNo { get; set; }

	int Id { get; set; }

	int IpPortSerialSpeed { get; set; }

	string IpSerialPortName { get; set; }

	bool IsChecked { get; set; }

	bool MngLock { get; set; }

	string Model { get; set; }

	string Name { get; set; }

	short ScaleId { get; set; }

	bool SellJamandeh { get; set; }

	string Serial { get; set; }

	string ShopName { get; set; }

	int TimerInterval { get; set; }

	string PcIp { get; set; }

	int TxPort { get; set; }

	int RxPort { get; set; }

	EnReadWeightType ReadWeightType { get; set; }

	string ReadWeightRequest { get; set; }

	string ReadWeightRegex { get; set; }

	decimal WeightZarib { get; set; }

	event Action OnItemChanges;

	MicroScaleInfo ToMicroScaleInfo();
}

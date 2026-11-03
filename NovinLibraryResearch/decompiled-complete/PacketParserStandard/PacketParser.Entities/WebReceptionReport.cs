using System;

namespace PacketParser.Entities;

public class WebReceptionReport
{
	public Guid Guid { get; set; }

	public DateTime DateM { get; set; }

	public string Time { get; set; }

	public int Id2 { get; set; }

	public Guid TafsilGuid { get; set; }

	public Guid TafsilGroupGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	public Guid CashierGuid { get; set; }

	public string TafsilName { get; set; }

	public string Description { get; set; }

	public decimal SumNaghd { get; set; }

	public decimal SumHavale { get; set; }

	public decimal SumChequeSh { get; set; }

	public decimal SumChequeM { get; set; }

	public decimal SumEnteghal { get; set; }

	public decimal Sum { get; set; }

	public bool IsChecked { get; set; }

	public int StatusId { get; set; }

	public ENSource Source { get; set; }

	public short SanadSt { get; set; }

	public byte[] SourceImage { get; set; }

	public byte[] ServerStatusImage { get; set; }

	public byte[] SanadStatusImage { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public string HostRefrenced { get; set; }

	public int SanadNo { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public string ContextGuid { get; set; }

	public bool IsTemp { get; set; }

	public int DeliveryId { get; set; }

	public string SendToServerStatus { get; set; }

	public EnKhazaneMaster Type { get; private set; }

	public string AndroidIme { get; set; }

	public DateTime? TransferDate { get; set; }

	public string CpuSerial { get; set; }

	public DateTime DateSabt { get; set; }

	public string HddSerial { get; set; }

	public short IdUser { get; set; }

	public string Ip { get; set; }

	public string LkSerial { get; set; }

	public decimal AfterAccount { get; set; }

	public decimal BeforeAccount { get; set; }

	public DependencyType DependencyType { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public bool? DeliveryStatus { get; set; }

	public decimal SumBankPos { get; set; }
}

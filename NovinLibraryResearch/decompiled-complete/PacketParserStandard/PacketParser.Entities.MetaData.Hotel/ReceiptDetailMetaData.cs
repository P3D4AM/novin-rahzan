namespace PacketParser.Entities.MetaData.Hotel;

public class ReceiptDetailMetaData
{
	public EnDeviceTravel DeviceTeravel { get; set; }

	public EnReasonTravel ReasonTeravel { get; set; }

	public string BiginingCity { get; set; }

	public decimal FlighNumber { get; set; }

	public int AdultCount { get; set; }

	public int ChildCount { get; set; }

	public int BabyCount { get; set; }
}

namespace PacketParser.DefaultsCoding;

public class TransferDefault
{
	public bool Hesabs { get; set; }

	public bool PrdGroup { get; set; }

	public bool PrdVahed { get; set; }

	public bool CustGroup { get; set; }

	public bool CustTitle { get; set; }

	public bool Sandogh { get; set; }

	public bool FareOrderCredit { get; set; }

	public bool Bank { get; set; }

	public bool Depot { get; set; }

	public bool LocalsSettings { get; set; }

	public bool Bardasht { get; set; }

	public bool Customers { get; set; }

	public bool User { get; set; }

	public bool Hazine { get; set; }

	public bool PriceLevel { get; set; }

	public TransferDefault(bool defaultValues)
	{
		Hesabs = defaultValues;
		PrdGroup = defaultValues;
		PrdVahed = defaultValues;
		CustGroup = defaultValues;
		CustTitle = defaultValues;
		Sandogh = defaultValues;
		FareOrderCredit = defaultValues;
		Bank = defaultValues;
		Depot = defaultValues;
		LocalsSettings = defaultValues;
		Bardasht = defaultValues;
		Customers = defaultValues;
		User = defaultValues;
		Hazine = defaultValues;
		PriceLevel = defaultValues;
	}
}

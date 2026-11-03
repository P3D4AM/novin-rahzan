namespace PacketParser.Access.SanadStatusAccess.DefautAccess;

public class OrderSellDefaultSanadStatusAccess : BaseSyncDefaultSanadStatusAccess
{
	public int TaxStatusAfterSend { get; set; }

	public int TaxStatusAfterFailed { get; set; }

	public int TaxStatusAfterSuccess { get; set; }
}

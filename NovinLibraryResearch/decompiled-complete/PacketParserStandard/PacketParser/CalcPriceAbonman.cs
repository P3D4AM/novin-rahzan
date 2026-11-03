using System;
using PacketParser.Entities;
using PacketParser.Settings.Accounting;

namespace PacketParser;

[Obsolete("Transferd to sharedActivationBusiness", true)]
public class CalcPriceAbonman
{
	public decimal Tamdid5YearPrice { get; private set; }

	public decimal Tamdid10YearPrice { get; private set; }

	public decimal TamdidPrice { get; private set; }

	public decimal TamdidPrice_Total { get; private set; }

	public decimal EzafatPercent { get; private set; }

	public decimal EzafatPrice { get; private set; }

	public decimal KosoratPercent { get; private set; }

	public decimal KosoratPrice { get; private set; }

	public decimal TamdidPrice_Total_5year_Net { get; private set; }

	public decimal TamdidPrice_Total_10year_Net { get; private set; }

	public decimal TamdidPrice_Total_1year_Net { get; private set; }

	public WebOrder Order { get; private set; }

	public ActivationAbonmanSettings Abonman { get; private set; }

	public long LkSerial { get; private set; }

	public Guid TafsilGuid { get; private set; }

	public Guid UserAccGuid { get; private set; }

	public Guid DepotGuid { get; private set; }

	private WebOrderDetail GenerateOrderDet(decimal price, Guid productGuid)
	{
		try
		{
			return new WebOrderDetail
			{
				Quantity = 1m,
				Price = price,
				Description = $"سریال قفل:{LkSerial}",
				DepotGuid = DepotGuid,
				Discount = 0m,
				ProductGuid = productGuid
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateOrderDet", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\CalcPriceAbonman.cs", 215);
			return null;
		}
	}
}

using System;
using System.ComponentModel;
using PacketParser.Entities;

namespace PacketParser.Settings.Accounting;

public class TransferSettings
{
	public const string KeyName = "Acc_TranferSettings";

	[DefaultValue(false)]
	public bool CusTitle { get; set; }

	[DefaultValue(false)]
	public bool CusGroup { get; set; }

	[DefaultValue(false)]
	public bool Customers { get; set; }

	[DefaultValue(false)]
	public bool PrdVahed { get; set; }

	[DefaultValue(false)]
	public bool PrdGroup { get; set; }

	[DefaultValue(false)]
	public bool Product { get; set; }

	[DefaultValue(false)]
	public bool AnbarGardani { get; set; }

	[DefaultValue(false)]
	public bool HesabType { get; set; }

	[DefaultValue(false)]
	public bool HesabGroup { get; set; }

	[DefaultValue(false)]
	public bool Kol { get; set; }

	[DefaultValue(false)]
	public bool TafsilGroup { get; set; }

	[DefaultValue(false)]
	public bool Moein { get; set; }

	[DefaultValue(false)]
	public bool Tafsil { get; set; }

	[DefaultValue(false)]
	public bool ServiceGroup { get; set; }

	[DefaultValue(false)]
	public bool ServiceVahed { get; set; }

	[DefaultValue(false)]
	public bool Service { get; set; }

	[DefaultValue(false)]
	public bool Depot { get; set; }

	[DefaultValue(false)]
	public bool User { get; set; }

	[DefaultValue(false)]
	public bool Decimal { get; set; }

	[DefaultValue(false)]
	public bool DecimalUpdate { get; set; }

	[DefaultValue(false)]
	public bool Lottery { get; set; }

	[DefaultValue(false)]
	public bool LotteryScore { get; set; }

	[DefaultValue(false)]
	public bool CustomerMotefareghe { get; set; }

	[DefaultValue(false)]
	public bool Vam { get; set; }

	[DefaultValue(false)]
	public bool CustomerCard { get; set; }

	[DefaultValue(false)]
	public bool ProductList { get; set; }

	[DefaultValue(false)]
	public bool TafsilList { get; set; }

	[DefaultValue(false)]
	public bool DasteCheck { get; set; }

	[DefaultValue(false)]
	public bool MoeinTafsliGroupRelation { get; set; }

	[DefaultValue(false)]
	public bool Desc { get; set; }

	[DefaultValue(false)]
	public bool AghsatM { get; set; }

	[DefaultValue(false)]
	public bool AghsatMGroup { get; set; }

	[DefaultValue(false)]
	public bool AghsatSh { get; set; }

	[DefaultValue(false)]
	public bool CheckedSave { get; set; }

	[DefaultValue(false)]
	public bool Resid { get; set; }

	[DefaultValue(false)]
	public bool DepotTransfer { get; set; }

	[DefaultValue(false)]
	public bool Log { get; set; }

	[DefaultValue(false)]
	public bool Settings { get; set; }

	[DefaultValue(false)]
	public bool OrderService { get; set; }

	[DefaultValue(false)]
	public bool TableRelationShip { get; set; }

	[DefaultValue(false)]
	public bool VisitorArea { get; set; }

	[DefaultValue(false)]
	public bool Sanad { get; set; }

	[DefaultValue(false)]
	public bool OrderSell { get; set; }

	[DefaultValue(false)]
	public bool OrderBuy { get; set; }

	[DefaultValue(false)]
	public bool OrderBackSell { get; set; }

	[DefaultValue(false)]
	public bool OrderBackBuy { get; set; }

	[DefaultValue(false)]
	public bool OrderZayeat { get; set; }

	[DefaultValue(false)]
	public bool PishFactor { get; set; }

	[DefaultValue(false)]
	public bool Reception { get; set; }

	[DefaultValue(false)]
	public bool Pardakht { get; set; }

	[DefaultValue(false)]
	public bool ChequeMOperation { get; set; }

	[DefaultValue(false)]
	public bool ChequeShOperation { get; set; }

	[DefaultValue(false)]
	public bool Sanads { get; set; }

	[DefaultValue(false)]
	public bool PrdSerials { get; set; }

	[DefaultValue(false)]
	public bool Tolid { get; set; }

	[DefaultValue(false)]
	public bool TolidFormula { get; set; }

	public static TransferSettings FromString(string data)
	{
		using (new JaegerMonitor("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\TransferSettings.cs"))
		{
			try
			{
				return data.FromJson<TransferSettings>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\TransferSettings.cs", 73);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\TransferSettings.cs", 77);
				throw;
			}
		}
	}

	public override string ToString()
	{
		using (new JaegerMonitor("ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\TransferSettings.cs"))
		{
			try
			{
				return Json.ToStringJson(this);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\TransferSettings.cs", 90);
				return "";
			}
		}
	}
}

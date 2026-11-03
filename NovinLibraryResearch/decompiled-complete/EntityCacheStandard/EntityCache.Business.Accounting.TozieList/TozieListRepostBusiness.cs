using System;
using PacketParser;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.Accounting.TozieList;

public class TozieListRepostBusiness : IHasContext
{
	public Guid Guid { get; set; }

	public int Number { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public string Time => DateM.ToShortTimeString();

	public string DriverName { get; set; }

	public string DriverMobile1 { get; set; }

	public string DriverMobile2 { get; set; }

	public string CarNumber { get; set; }

	public string Description { get; set; }

	public Guid CashierGuid { get; set; }

	public string CashierName { get; set; }

	public string ContextGuid { get; private set; }

	public int SellCount { get; set; }

	public decimal SellSum { get; set; }

	public int BackSellCount { get; set; }

	public decimal BackSellSum { get; set; }

	public int ReceptionCount { get; set; }

	public decimal ReceptionSum { get; set; }

	public TozieListRepostBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}

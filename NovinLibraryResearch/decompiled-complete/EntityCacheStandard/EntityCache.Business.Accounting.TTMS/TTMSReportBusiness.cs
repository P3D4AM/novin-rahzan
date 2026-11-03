using System;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.Accounting.TTMS;

public class TTMSReportBusiness : IHasContext
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public string FilePath { get; set; } = "";


	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public EnSeason Priod { get; set; }

	public string PriodName => Priod.GetDisplay().GetTranslate();

	public short Year { get; set; }

	public short State { get; set; }

	public string StateName { get; set; } = "";


	public int SellCount { get; set; }

	public decimal SellSum { get; set; }

	public int BackSellCount { get; set; }

	public decimal BackSellSum { get; set; }

	public int BuyCount { get; set; }

	public decimal BuySum { get; set; }

	public int BackBuyCount { get; set; }

	public decimal BackBuySum { get; set; }

	public string ContextGuid { get; private set; }

	public TTMSReportBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}

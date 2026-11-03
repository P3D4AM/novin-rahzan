using System;

namespace PacketParser.BusinessLayer.Models;

public class KardexCustomerModel
{
	public Guid TafsilGuid { get; set; }

	public long TafsilCode { get; set; }

	public string TafsilName { get; set; }

	public Guid GroupGuid { get; set; }

	public decimal PreDebit { get; set; }

	public decimal PreCredit { get; set; }

	public decimal PreSood { get; set; }

	public double PreVorodQt { get; set; }

	public double PreKhorojQt { get; set; }

	public double PreBuyQt { get; set; }

	public decimal PreBuySum { get; set; }

	public double PreSellQt { get; set; }

	public decimal PreSellSum { get; set; }

	public double PreBackBuyQt { get; set; }

	public decimal PreBackBuySum { get; set; }

	public double PreBackSellQt { get; set; }

	public decimal PreBackSellSum { get; set; }

	public decimal BetweenDebit { get; set; }

	public decimal BetweenCredit { get; set; }

	public decimal BetweenSood { get; set; }

	public double BetweenVorodQt { get; set; }

	public double BetweenKhorojQt { get; set; }

	public double BetweenBuyQt { get; set; }

	public decimal BetweenBuySum { get; set; }

	public double BetweenSellQt { get; set; }

	public decimal BetweenSellSum { get; set; }

	public double BetweenBackBuyQt { get; set; }

	public decimal BetweenBackBuySum { get; set; }

	public double BetweenBackSellQt { get; set; }

	public decimal BetweenBackSellSum { get; set; }

	public decimal AfterDebit { get; set; }

	public decimal AfterCredit { get; set; }

	public decimal AfterSood { get; set; }

	public double AfterVorodQt { get; set; }

	public double AfterKhorojQt { get; set; }

	public double AfterBuyQt { get; set; }

	public decimal AfterBuySum { get; set; }

	public double AfterSellQt { get; set; }

	public decimal AfterSellSum { get; set; }

	public double AfterBackBuyQt { get; set; }

	public decimal AfterBackBuySum { get; set; }

	public double AfterBackSellQt { get; set; }

	public decimal AfterBackSellSum { get; set; }

	public decimal Debit { get; set; }

	public decimal Credit { get; set; }

	public decimal Sood { get; set; }

	public double VorodQt { get; set; }

	public double KhorojQt { get; set; }

	public double RemQt { get; set; }

	public double BuyQt { get; set; }

	public decimal BuySum { get; set; }

	public double SellQt { get; set; }

	public decimal SellSum { get; set; }

	public double BackBuyQt { get; set; }

	public decimal BackBuySum { get; set; }

	public double BackSellQt { get; set; }

	public decimal BackSellSum { get; set; }
}

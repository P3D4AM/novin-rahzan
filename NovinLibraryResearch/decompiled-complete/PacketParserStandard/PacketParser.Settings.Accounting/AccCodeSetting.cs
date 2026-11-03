using PacketParser.Settings.Accounting.AccCoding;

namespace PacketParser.Settings.Accounting;

public class AccCodeSetting
{
	public SellCodingSettings SellCoding { get; set; } = new SellCodingSettings();


	public BuyCodingSettings BuyCoding { get; set; } = new BuyCodingSettings();


	public BackSellCodingSettings BackSellCoding { get; set; } = new BackSellCodingSettings();


	public ZayeatCodingSettings ZayeatCoding { get; set; } = new ZayeatCodingSettings();


	public AmaniVorodCodingSettings AmaniVorodCoding { get; set; } = new AmaniVorodCodingSettings();


	public AmaniKhorojCodingSettings AmaniKhorojCoding { get; set; } = new AmaniKhorojCodingSettings();


	public BackBuyCodingSettings BackBuyCoding { get; set; } = new BackBuyCodingSettings();


	public CheckMCodingSettings CheckMCoding { get; set; } = new CheckMCodingSettings();


	public CheckSHCodingSettings CheckSHCoding { get; set; } = new CheckSHCodingSettings();


	public BankActivityCodingSetting BankActivityCoding { get; set; } = new BankActivityCodingSetting();


	public TolidCodingSetting TolidCoding { get; set; } = new TolidCodingSetting();


	public PermanentCodingSettings Permanent { get; set; } = new PermanentCodingSettings();

}

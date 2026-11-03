using PacketParser.Entities;

namespace PacketParser.Settings.Accounting;

public class PrintSettings
{
	private const string Sign = "\\r\\n\\r\\n<div style=\"float: right; margin-right: 10%;\">امضاء خریدار</div><div style=\"float: left; margin-left: 10%;\">امضاء فروشنده</div>\\r\\n\\r\\n\\r\\n\\r\\n";

	public string SellFooterString { get; set; } = "اجناس فوق صحیح و سالم تحویل خریدار گردید\\r\\nاجناس فروخته شده به هیچ عنوان پس گرفته نمیشود.\\r\\n\\r\\n<div style=\"float: right; margin-right: 10%;\">امضاء خریدار</div><div style=\"float: left; margin-left: 10%;\">امضاء فروشنده</div>\\r\\n\\r\\n\\r\\n\\r\\n";


	public string BuyFooterString { get; set; } = "اجناس فوق صحیح و سالم تحویل خریدار گردید\\r\\nاجناس فروخته شده به هیچ عنوان پس گرفته نمیشود.\\r\\n\\r\\n<div style=\"float: right; margin-right: 10%;\">امضاء خریدار</div><div style=\"float: left; margin-left: 10%;\">امضاء فروشنده</div>\\r\\n\\r\\n\\r\\n\\r\\n";


	public string BackSellFooterString { get; set; } = "اجناس فوق صحیح و سالم تحویل خریدار گردید\\r\\nاجناس فروخته شده به هیچ عنوان پس گرفته نمیشود.\\r\\n\\r\\n<div style=\"float: right; margin-right: 10%;\">امضاء خریدار</div><div style=\"float: left; margin-left: 10%;\">امضاء فروشنده</div>\\r\\n\\r\\n\\r\\n\\r\\n";


	public string BackBuyFooterString { get; set; } = "اجناس فوق صحیح و سالم تحویل خریدار گردید\\r\\nاجناس فروخته شده به هیچ عنوان پس گرفته نمیشود.\\r\\n\\r\\n<div style=\"float: right; margin-right: 10%;\">امضاء خریدار</div><div style=\"float: left; margin-left: 10%;\">امضاء فروشنده</div>\\r\\n\\r\\n\\r\\n\\r\\n";


	public string ReceptionFooterString { get; set; } = "اجناس فوق صحیح و سالم تحویل خریدار گردید\\r\\nاجناس فروخته شده به هیچ عنوان پس گرفته نمیشود.\\r\\n\\r\\n<div style=\"float: right; margin-right: 10%;\">امضاء خریدار</div><div style=\"float: left; margin-left: 10%;\">امضاء فروشنده</div>\\r\\n\\r\\n\\r\\n\\r\\n";


	public string PaymentFooterString { get; set; } = "اجناس فوق صحیح و سالم تحویل خریدار گردید\\r\\nاجناس فروخته شده به هیچ عنوان پس گرفته نمیشود.\\r\\n\\r\\n<div style=\"float: right; margin-right: 10%;\">امضاء خریدار</div><div style=\"float: left; margin-left: 10%;\">امضاء فروشنده</div>\\r\\n\\r\\n\\r\\n\\r\\n";


	public string PishFactorFooterString { get; set; } = "اجناس فوق صحیح و سالم تحویل خریدار گردید\\r\\nاجناس فروخته شده به هیچ عنوان پس گرفته نمیشود.\\r\\n\\r\\n<div style=\"float: right; margin-right: 10%;\">امضاء خریدار</div><div style=\"float: left; margin-left: 10%;\">امضاء فروشنده</div>\\r\\n\\r\\n\\r\\n\\r\\n";


	public string GetFooterString(PrintType printType)
	{
		return printType switch
		{
			PrintType.Sell => SellFooterString, 
			PrintType.Buy => BuyFooterString, 
			PrintType.BackSell => BackSellFooterString, 
			PrintType.BackBuy => BuyFooterString, 
			PrintType.Reception => ReceptionFooterString, 
			PrintType.Payement => PaymentFooterString, 
			PrintType.GardeshTafsili => "", 
			PrintType.GardeshTajmi => "", 
			PrintType.none => "", 
			PrintType.PishFactor => PishFactorFooterString, 
			_ => "", 
		};
	}
}

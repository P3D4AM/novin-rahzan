namespace PacketParser.Payamak.Setting;

public class SellManage
{
	public static string DateNow => "[تاریخ روز]".FixString().Trim();

	public static string TimeNow => "[زمان ارسال]".FixString().Trim();

	public static string SumSell => "[جمع فروش]".FixString().Trim();

	public static string SumDiscount => "[جمع تخفیفات]".FixString().Trim();

	public static string SumNaghdi => "[جمع دریافتی نقدی]".FixString().Trim();

	public static string SumBank => "[جمع دریافتی بانک]".FixString().Trim();

	public static string ContCheck => "[تعداد چک های دریافتی]".FixString().Trim();

	public static string SumCheck => "[جمع چک های دریافتی]".FixString().Trim();
}

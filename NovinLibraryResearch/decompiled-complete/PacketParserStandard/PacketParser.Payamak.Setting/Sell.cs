namespace PacketParser.Payamak.Setting;

public class Sell
{
	public static string FullName => "[نام مشتری]".FixString().Trim();

	public static string Number => "[شماره]".FixString().Trim();

	public static string Date => "[تاریخ]".FixString().Trim();

	public static string Account => "[حساب فعلی]".FixString().Trim();

	public static string AghlamSum => "[جمع اقلام]".FixString().Trim();

	public static string Discount => "[تخفیف]".FixString().Trim();

	public static string Pardakht => "[قابل پرداخت]".FixString().Trim();

	public static string Serial => "[سریال]".FixString().Trim();

	public static string Garanti => "[کد گارانتی]".FixString().Trim();
}

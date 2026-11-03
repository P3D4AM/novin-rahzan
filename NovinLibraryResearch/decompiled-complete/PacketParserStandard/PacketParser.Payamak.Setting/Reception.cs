namespace PacketParser.Payamak.Setting;

public class Reception
{
	public static string FullName => "[نام مشتری]".FixString().Trim();

	public static string Date => "[تاریخ]".FixString().Trim();

	public static string Account => "[حساب فعلی]".FixString().Trim();

	public static string Price => "[مبلغ]".FixString().Trim();
}

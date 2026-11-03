namespace PacketParser.Payamak.Setting;

public class CheckM
{
	public static string FullName => "[نام طرف حساب]".FixString().Trim();

	public static string CheckNO => "[شماره چک]".FixString().Trim();

	public static string CheckDate => "[تاریخ چک]".FixString().Trim();

	public static string CheckPrice => "[مبلغ چک]".FixString().Trim();
}

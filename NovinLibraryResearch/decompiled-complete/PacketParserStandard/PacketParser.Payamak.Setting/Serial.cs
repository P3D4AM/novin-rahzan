namespace PacketParser.Payamak.Setting;

public class Serial
{
	public static string Pardazande => "[طرف حساب]".FixString().Trim();

	public static string GNCode => "[کد گارانتی]".FixString().Trim();

	public static string GNDate => "[تاریخ اتمام گارانتی]".FixString().Trim();

	public static string SerialNumber => "[سریال]".FixString().Trim();
}

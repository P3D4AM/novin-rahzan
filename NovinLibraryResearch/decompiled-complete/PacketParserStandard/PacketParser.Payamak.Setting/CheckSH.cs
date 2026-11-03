namespace PacketParser.Payamak.Setting;

public class CheckSH
{
	public static string FullName => "[نام گیرنده]".FixString().Trim();

	public static string CheckNO => "[شماره چک]".FixString().Trim();

	public static string CheckDate => "[تاریخ چک]".FixString().Trim();

	public static string CheckPrice => "[مبلغ چک]".FixString().Trim();

	public static string BankName => "[نام بانک]".FixString().Trim();
}

using System.Collections;

namespace Stimulsoft.Editor;

public class HyperText
{
	private static Hashtable identsTable;

	private static void InitIdentsTable()
	{
		if (identsTable == null)
		{
			identsTable = new Hashtable();
			for (char c = 'A'; c <= 'Z'; c = (char)(c + 1))
			{
				identsTable.Add(c, c);
			}
			for (char c2 = 'a'; c2 <= 'z'; c2 = (char)(c2 + 1))
			{
				identsTable.Add(c2, c2);
			}
			for (char c3 = '0'; c3 <= '9'; c3 = (char)(c3 + 1))
			{
				identsTable.Add(c3, c3);
			}
			identsTable.Add('_', '_');
		}
	}

	public static bool IsEmailString(string text)
	{
		bool flag = string.Compare(text, 0, EditConsts.MailTo, 0, EditConsts.MailTo.Length, ignoreCase: true) == 0;
		if (!flag)
		{
			int length = text.Length;
			int num = text.IndexOf("@");
			int num2 = text.LastIndexOf("@");
			int num3 = text.LastIndexOf(".");
			flag = num >= 0 && num == num2 && num3 > num && num != length - 1;
			if (flag)
			{
				InitIdentsTable();
				flag = identsTable.ContainsKey(text[num]);
			}
		}
		return flag;
	}

	public static bool IsWWWString(string text)
	{
		return string.Compare(text, 0, EditConsts.WWW, 0, EditConsts.WWW.Length, ignoreCase: true) == 0;
	}

	public static bool IsHttpString(string text)
	{
		return string.Compare(text, 0, EditConsts.HTTPProtocol, 0, EditConsts.HTTPProtocol.Length, ignoreCase: true) == 0;
	}

	public static bool IsFtpString(string text)
	{
		return string.Compare(text, 0, EditConsts.FTPProtocol, 0, EditConsts.FTPProtocol.Length, ignoreCase: true) == 0;
	}

	public static bool IsGopherString(string text)
	{
		return string.Compare(text, 0, EditConsts.GopherProtocol, 0, EditConsts.GopherProtocol.Length, ignoreCase: true) == 0;
	}

	public static bool IsFileString(string text)
	{
		return string.Compare(text, 0, EditConsts.FileProtocol, 0, EditConsts.FileProtocol.Length, ignoreCase: true) == 0;
	}

	public static bool IsHyperText(string text)
	{
		if (!IsEmailString(text) && !IsWWWString(text) && !IsHttpString(text) && !IsFtpString(text) && !IsGopherString(text))
		{
			return IsFileString(text);
		}
		return true;
	}
}

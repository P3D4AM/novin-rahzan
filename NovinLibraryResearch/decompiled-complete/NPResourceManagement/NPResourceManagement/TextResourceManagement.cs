using System;
using System.Text;
using NPResourceManagement.Properties;
using PacketParser;
using PacketParser.Entities;

namespace NPResourceManagement;

public static class TextResourceManagement
{
	private static TextBrandResourceManagement _textResource;

	public static TextBrandResourceManagement TextResource
	{
		get
		{
			try
			{
				if (_textResource == null)
				{
					_textResource = Resources.TextBrandResourceManagement.FromJson<TextBrandResourceManagement>("TextResource", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\NPResourceManagement\\TextResourceManagement.cs", 17);
				}
				return _textResource;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TextResource", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\NPResourceManagement\\TextResourceManagement.cs", 22);
				return new TextBrandResourceManagement();
			}
		}
	}

	public static string TextLanguageFarsiTimeAttendance => Resources.LanguageFarsi.Replace("[CompanyName]", TextResource.CompanyName).Replace("[Address]", TextResource.Address);

	public static string TextLanguageArabicTimeAttendance => Resources.LanguageArabic.Replace("[CompanyName]", TextResource.CompanyName).Replace("[Address]", TextResource.Address);

	public static string TextLanguagePersianAccounting => Resources.Accounting_Persian;

	public static string TextLanguageEnglishAccounting => Resources.Accounting_English;

	public static string TextLanguageRussianAccounting => Encoding.UTF8.GetString(Resources.Accounting_Russian);

	public static string TextLanguageGeorgianAccounting => Encoding.UTF8.GetString(Resources.Accounting_Georgian);

	public static string TextLanguageEnglishNewAccounting => Encoding.UTF8.GetString(Resources.Accounting_EnglishNew);
}

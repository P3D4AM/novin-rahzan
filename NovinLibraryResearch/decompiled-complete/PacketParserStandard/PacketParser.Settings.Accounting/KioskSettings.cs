using System;
using System.ComponentModel;
using PacketParser.Entities;

namespace PacketParser.Settings.Accounting;

public class KioskSettings
{
	public const string KeyName = "Acc_KioskSettings";

	private string _title = "";

	public EnTheme Theme { get; set; }

	public Guid ProductGroupSelectedGuid { get; set; }

	public string MenuTitle
	{
		get
		{
			if (string.IsNullOrEmpty(_title))
			{
				_title = "منو";
			}
			return _title;
		}
		set
		{
			_title = value;
		}
	}

	public bool ShowPopularProducts { get; set; }

	public bool ShowServeType { get; set; }

	public EnCurreny CurrenyType { get; set; }

	[DefaultValue(true)]
	public bool AuthorizeByCode { get; set; } = true;


	[DefaultValue(true)]
	public bool AuthorizeByUserName { get; set; } = true;


	[DefaultValue(true)]
	public bool AuthorizeByMobile { get; set; } = true;


	[DefaultValue(true)]
	public bool AuthorizeByCard { get; set; } = true;


	public string CompanyName { get; set; } = "نوین پرداز";


	public string EnglishSloganBanner { get; set; }

	public string PersianSloganBanner { get; set; }

	public static KioskSettings FromString(string data)
	{
		using (new JaegerMonitor("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\KioskSettings.cs"))
		{
			try
			{
				return data.FromJson<KioskSettings>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\KioskSettings.cs", 39);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\KioskSettings.cs", 43);
				throw;
			}
		}
	}

	public override string ToString()
	{
		using (new JaegerMonitor("ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\KioskSettings.cs"))
		{
			try
			{
				return Json.ToStringJson(this);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\KioskSettings.cs", 56);
				return "";
			}
		}
	}
}

using System.ComponentModel;

namespace PacketParser.Entities.WordPress;

public class WooBaseSettings
{
	[PersianNameAttribute.PersianName("نام کاربری پنل")]
	public string CPanelUserName { get; set; } = "";


	[PersianNameAttribute.PersianName("پسورد پنل")]
	public string CPanelPassword { get; set; } = "";


	[PersianNameAttribute.PersianName("نام کاربری ادمین")]
	public string AdminUserName { get; set; } = "";


	[PersianNameAttribute.PersianName("پسورد ادمین")]
	public string AdminPassword { get; set; } = "";


	public string Ck { get; set; } = "";


	public string Cs { get; set; } = "";


	[PersianNameAttribute.PersianName("ادرس هاست")]
	public string HostUrl { get; set; } = "";


	[PersianNameAttribute.PersianName("ادرس وردپرس ادمین")]
	public string WPAdminAddress { get; set; } = "";


	[DefaultValue(false)]
	[PersianNameAttribute.PersianName("مبالغ در سایت بر حسب تومان است")]
	public bool WebSiteDecimalIsToman { get; set; }

	public WooBaseSettings Clone()
	{
		return (WooBaseSettings)MemberwiseClone();
	}
}

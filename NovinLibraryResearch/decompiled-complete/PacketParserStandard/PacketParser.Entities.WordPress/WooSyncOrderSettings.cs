using System.ComponentModel;

namespace PacketParser.Entities.WordPress;

public class WooSyncOrderSettings
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("انتقال فاکتور از سایت ورد پرس به وب سرویس فعال باشد")]
	public bool SyncAll { get; set; } = true;


	public WooSyncOrderSettings Clone()
	{
		return (WooSyncOrderSettings)MemberwiseClone();
	}
}

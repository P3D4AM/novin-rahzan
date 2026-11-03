using System.ComponentModel;

namespace PacketParser.Access;

public class TafsilLevelAcess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده گروه تفصیلی")]
	public bool TafsilLevel_See { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("افزودن  گروه تفصیلی")]
	public bool TafsilLevel_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف  گروه تفصیلی")]
	public bool TafsilLevel_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش  گروه تفصیلی")]
	public bool TafsilLevel_Upadte { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده گروه های تفصیلی حذف شده")]
	public bool Role_ShowDeleted { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("فعال کردن گروه های تفصیلی حذف شده")]
	public bool Role_Active { get; set; } = true;

}

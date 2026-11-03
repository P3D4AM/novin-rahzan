using System.ComponentModel;

namespace PacketParser.Access;

public class PlacementAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("افزودن")]
	public bool Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف")]
	public bool Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش")]
	public bool Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری")]
	public bool Report { get; set; } = true;

}

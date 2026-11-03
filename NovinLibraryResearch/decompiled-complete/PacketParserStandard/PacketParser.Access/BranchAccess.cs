using System.ComponentModel;

namespace PacketParser.Access;

public class BranchAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("افزودن شعبه")]
	public bool Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش لیست شعبه ها")]
	public bool Report { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف شعبه")]
	public bool Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش شعبه")]
	public bool Edit { get; set; } = true;

}

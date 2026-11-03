using System.ComponentModel;

namespace PacketParser.Access;

public class DasteCheckAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده دسته چک")]
	public bool DasteCheck_See { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تعریف دسته چک")]
	public bool DasteCheck_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف دسته چک")]
	public bool DasteCheck_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش دسته چک")]
	public bool DasteCheck_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ دسته چک")]
	public bool DasteCheck_Print { get; set; } = true;

}

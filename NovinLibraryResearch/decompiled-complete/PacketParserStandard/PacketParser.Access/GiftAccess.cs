using System.ComponentModel;

namespace PacketParser.Access;

public class GiftAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تعریف اشانتیون")]
	public bool Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف اشانتیون")]
	public bool Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش اشانتیون")]
	public bool Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری اشانتیون ها")]
	public bool Report { get; set; } = true;

}

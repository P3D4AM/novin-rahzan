using System.ComponentModel;
using PacketParser.Entities.Attendance;

namespace PacketParser.Attendance.Access;

public class Taradod
{
	[AttHelpURL("New_Item51.htm")]
	[PersianNameAttribute.PersianName("اضافه، حذف، ویرایش تردد پرسنل")]
	[DefaultValue(true)]
	public bool TaradodMain { get; set; } = true;


	[PersianNameAttribute.PersianName("ایجاد یک تردد جدید")]
	[DefaultValue(true)]
	public bool TaradodInsert { get; set; } = true;


	[PersianNameAttribute.PersianName("حذف یک تردد")]
	[DefaultValue(true)]
	public bool TaradodDelete { get; set; } = true;


	[PersianNameAttribute.PersianName("ویرایش یک تردد")]
	[DefaultValue(true)]
	public bool TaradodUpdate { get; set; } = true;


	[PersianNameAttribute.PersianName("نمایش یک تردد")]
	[DefaultValue(true)]
	public bool TaradodDisplay { get; set; } = true;


	[PersianNameAttribute.PersianName("گزارش تجمیع تردد")]
	[AttHelpURL("New_Item57.htm")]
	[DefaultValue(true)]
	public bool TaradodReportAmarPersonnel { get; set; } = true;


	[PersianNameAttribute.PersianName("گزارش تردد یک شخص در طول بازه تاریخ")]
	[AttHelpURL("New_Item50.htm")]
	[DefaultValue(true)]
	public bool TaradodReportTaradodPersonnel { get; set; } = true;


	[PersianNameAttribute.PersianName("گزارش تردد کلیه کارکنان در یک روز خاص")]
	[AttHelpURL("New_Item56.htm")]
	[DefaultValue(true)]
	public bool TaradodReportTaradodPersonnelOnADay { get; set; } = true;


	[PersianNameAttribute.PersianName("گزارش تردد کل کارکنان در طول بازه تاریخ")]
	[DefaultValue(true)]
	public bool RptTaradodAllPersonnel { get; set; } = true;


	[PersianNameAttribute.PersianName("دریافت آنلاین تردد")]
	[DefaultValue(true)]
	public bool TaradodReciveData { get; set; } = true;


	[PersianNameAttribute.PersianName("دریافت آفلاین تردد")]
	[AttHelpURL("New_Item69.htm")]
	[DefaultValue(true)]
	public bool TaradodReciveDataOffLine { get; set; } = true;


	[PersianNameAttribute.PersianName("ویرایش مدیریتی تردد")]
	[AttHelpURL("New_Item55.htm")]
	[DefaultValue(true)]
	public bool TaradodEditManagement { get; set; } = true;


	[PersianNameAttribute.PersianName("ویرایش نوع تردد")]
	[AttHelpURL("New_Item54.htm")]
	[DefaultValue(true)]
	public bool TaradodChangeTypeTaradod { get; set; } = true;


	[PersianNameAttribute.PersianName("ثبت تردد جمعه کاری")]
	[DefaultValue(true)]
	public bool TaradodWorkHoliday { get; set; } = true;


	[PersianNameAttribute.PersianName("ثبت تردد شب کاری")]
	[DefaultValue(true)]
	public bool TaradodWorkNight { get; set; } = true;


	[PersianNameAttribute.PersianName("ثبت تردد نوبت کاری")]
	[DefaultValue(true)]
	public bool TaradodWorkShift { get; set; } = true;


	[PersianNameAttribute.PersianName("ثبت ماموریت روزانه")]
	[DefaultValue(true)]
	public bool TaradodMissionDaily { get; set; } = true;


	[PersianNameAttribute.PersianName("ویرایش و استفاده از کلید های تابعی")]
	[DefaultValue(true)]
	public bool TaradodFunctionKeys { get; set; } = true;


	[PersianNameAttribute.PersianName("ذخیره یک تردد")]
	[DefaultValue(true)]
	public bool TaradodSaveNewTaradod { get; set; } = true;


	[PersianNameAttribute.PersianName("تعریف انواع تردد")]
	[AttHelpURL("New_Item59.htm")]
	[DefaultValue(true)]
	public bool TaradodDefine { get; set; } = true;


	[PersianNameAttribute.PersianName("اصلاح گروهی ترددها")]
	[DefaultValue(true)]
	public bool GroupTaradodEdit { get; set; } = true;

}

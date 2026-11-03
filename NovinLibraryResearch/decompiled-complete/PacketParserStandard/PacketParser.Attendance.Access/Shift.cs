using System;
using System.ComponentModel;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;

namespace PacketParser.Attendance.Access;

public class Shift
{
	[AttHelpURL("New_Item8.htm")]
	[PersianNameAttribute.PersianName("شیفت ها")]
	[DefaultValue(true)]
	public bool ShiftMain { get; set; } = true;


	[PersianNameAttribute.PersianName("تعریف شیفت پیشرفته")]
	[DefaultValue(true)]
	public bool ShiftAdvance { get; set; } = true;


	[AttHelpURL("rtretewtwe.htm")]
	[PersianNameAttribute.PersianName("ایجاد شیفت جدید")]
	[DefaultValue(true)]
	public bool ShiftInsert { get; set; } = true;


	[PersianNameAttribute.PersianName("حذف یک شیفت")]
	[DefaultValue(true)]
	public bool ShiftDelete { get; set; } = true;


	[AttHelpURL("New_Item97.htm")]
	[PersianNameAttribute.PersianName("ویرایش یک شیفت")]
	[DefaultValue(true)]
	public bool ShiftUpdate { get; set; } = true;


	[PersianNameAttribute.PersianName("شیفت عادی")]
	[AttHelpURL("New_Item9.htm")]
	[DefaultValue(true)]
	public bool ShiftNormal { get; set; } = true;


	[PersianNameAttribute.PersianName("شیفت شناور")]
	[AttHelpURL("New_Item13.htm")]
	[DefaultValue(true)]
	public bool ShiftShenavar { get; set; } = true;


	[PersianNameAttribute.PersianName("شیفت چرخشی")]
	[AttHelpURL("New_Item27.htm")]
	[DefaultValue(true)]
	public bool ShiftCircle { get; set; } = true;


	[PersianNameAttribute.PersianName("شیفت قابل برنامه ریزی")]
	[AttHelpURL("New_Item29.htm")]
	[DefaultValue(true)]
	public bool ShiftProgrammable { get; set; } = true;


	[PersianNameAttribute.PersianName("شیفت چرخشی صفحه دوم")]
	[AttHelpURL("New_Item28.htm")]
	[DefaultValue(true)]
	public bool ShiftCirclePage2 { get; set; } = true;


	[PersianNameAttribute.PersianName("پکیج ورزشی")]
	[AttHelpURL("New_Item8.htm")]
	[DefaultValue(true)]
	public bool ShiftPackage { get; set; } = true;


	public bool CanInsert(AccessLevelClass pAccessLevel, int count)
	{
		bool result = true;
		try
		{
			if (pAccessLevel.LevelHozor <= Level_Hozor.Level1 && count > 1)
			{
				result = false;
			}
			else
			{
				switch (pAccessLevel.LevelHozor)
				{
				default:
					goto end_IL_0002;
				case Level_Hozor.Level2:
					if (count > 3)
					{
						break;
					}
					goto end_IL_0002;
				case Level_Hozor.Level3:
					if (count > 5)
					{
						break;
					}
					goto end_IL_0002;
				case Level_Hozor.Level4:
					if (count > 10)
					{
						break;
					}
					goto end_IL_0002;
				}
				result = false;
			}
			end_IL_0002:;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CanInsert", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Attendance\\Access\\Shift.cs", 67);
		}
		return result;
	}

	public bool CanInsertPackage(AccessLevelClass pAccessLevel)
	{
		return pAccessLevel.LevelHozor > Level_Hozor.Level2;
	}
}

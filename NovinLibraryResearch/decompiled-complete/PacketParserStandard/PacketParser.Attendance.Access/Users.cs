using System;
using System.ComponentModel;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;

namespace PacketParser.Attendance.Access;

public class Users
{
	[AttHelpURL("New_Item.htm")]
	[PersianNameAttribute.PersianName("کاربران")]
	[DefaultValue(true)]
	public bool UsersMain { get; set; } = true;


	[PersianNameAttribute.PersianName("ایجاد کاربران")]
	[DefaultValue(true)]
	public bool UsersInsert { get; set; } = true;


	[PersianNameAttribute.PersianName("حذف کاربران")]
	[DefaultValue(true)]
	public bool UsersDelete { get; set; } = true;


	[PersianNameAttribute.PersianName("ویرایش کاربران")]
	[DefaultValue(true)]
	public bool UsersUpdate { get; set; } = true;


	[PersianNameAttribute.PersianName("گزارش عملکرد کاربران")]
	[AttHelpURL("New_Item67.htm")]
	[DefaultValue(true)]
	public bool UsersReportLogs { get; set; } = true;


	[PersianNameAttribute.PersianName("ویرایش سطح دسترسی کاربران")]
	[DefaultValue(true)]
	public bool UsersAccessLevel { get; set; } = true;


	public bool CanInsert(AccessLevelClass pAccessLevel, int count)
	{
		try
		{
			bool result = true;
			if (pAccessLevel.LevelHozor <= Level_Hozor.Level2 && count > 1)
			{
				result = false;
			}
			else if (pAccessLevel.LevelHozor == Level_Hozor.Level3 && count > 2)
			{
				result = false;
			}
			else if (pAccessLevel.LevelHozor == Level_Hozor.Level4 && count > 5)
			{
				result = false;
			}
			return result;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CanInsert", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Attendance\\Access\\Users.cs", 50);
			return false;
		}
	}
}

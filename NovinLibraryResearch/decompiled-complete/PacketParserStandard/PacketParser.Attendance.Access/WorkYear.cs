using System;
using System.ComponentModel;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;

namespace PacketParser.Attendance.Access;

public class WorkYear
{
	[AttHelpURL("New_Item4.htm")]
	[PersianNameAttribute.PersianName("تعداد سال های کاری و شرکت ها")]
	[DefaultValue(true)]
	public bool WorkYearMain { get; set; } = true;


	[PersianNameAttribute.PersianName("ایجاد شرکت")]
	[DefaultValue(true)]
	public bool WorkYearInsert { get; set; } = true;


	[PersianNameAttribute.PersianName("حذف شرکت")]
	[DefaultValue(true)]
	public bool WorkYearDelete { get; set; } = true;


	[PersianNameAttribute.PersianName("ویرایش شرکت")]
	[DefaultValue(true)]
	public bool WorkYearUpdate { get; set; } = true;


	public ReturnedSaveFuncInfoWithValue<bool> CanInsert(int count)
	{
		ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<bool>();
		try
		{
			bool value = true;
			ReturnedSaveFuncInfo saveFuncInfo = AccessLevelClass.Instance.SaveFuncInfo;
			Level_Hozor levelHozor = AccessLevelClass.Instance.LevelHozor;
			if (levelHozor > Level_Hozor.Level2)
			{
				if (levelHozor != Level_Hozor.Level3)
				{
					if (levelHozor == Level_Hozor.Level4 && count > 5)
					{
						goto IL_003c;
					}
				}
				else if (count > 2)
				{
					goto IL_003c;
				}
			}
			else if (count > 1)
			{
				goto IL_003c;
			}
			goto IL_003e;
			IL_003e:
			returnedSaveFuncInfoWithValue.value = value;
			returnedSaveFuncInfoWithValue.AddReturnedValue(saveFuncInfo, ReturnedState.None, "CanInsert", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Attendance\\Access\\WorkYear.cs", 47);
			goto end_IL_0006;
			IL_003c:
			value = false;
			goto IL_003e;
			end_IL_0006:;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CanInsert", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Attendance\\Access\\WorkYear.cs", 49);
		}
		return returnedSaveFuncInfoWithValue;
	}
}

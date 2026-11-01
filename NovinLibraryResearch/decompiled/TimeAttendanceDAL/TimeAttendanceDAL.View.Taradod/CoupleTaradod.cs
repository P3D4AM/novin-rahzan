using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FormCustom;
using PacketParser;
using PacketParser.Attendance;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Attendance;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceDAL.Dal.Taradod;

namespace TimeAttendanceDAL.View.Taradod;

public class CoupleTaradod : IHasContext, ICoupleTaradod
{
	public static ColorDet ClsColor = new ColorDet();

	public string ContextGuid { get; set; }

	public DateTime DateM { get; set; }

	public TypeTaradodEnumType TypeTaradod { get; set; }

	public List<StatuseTaradodEnumType> StatuseTaradodLst { get; set; } = new List<StatuseTaradodEnumType>();


	public bool IsTaradodVirtual
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Invalid comparison between Unknown and I4
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Invalid comparison between Unknown and I4
			try
			{
				if ((int)Vtaradod.IsVirtual == 4 && (int)Ktaradod.IsVirtual == 4)
				{
					return true;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "IsTaradodVirtual", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Taradod\\CoupleTaradod.cs", 37);
			}
			return false;
		}
	}

	public bool IsEnteghaliTaradod
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Invalid comparison between Unknown and I4
			if (Vtaradod != null)
			{
				return (int)Vtaradod.IsVirtual == 6;
			}
			return false;
		}
	}

	public Color ColorV { get; set; }

	public Color ColorKh { get; set; }

	public Color BackColorOfCell { get; set; }

	public LogOnPersonel Ktaradod { get; set; }

	public LogOnPersonel Vtaradod { get; set; }

	public int PersonId { get; set; }

	public int PersonNo { get; set; }

	public int DeviceId { get; set; }

	public string VTime
	{
		get
		{
			string result = "00:00";
			try
			{
				if (Vtaradod != null)
				{
					result = Calendar.DateToFormatTime(Vtaradod.DateTimeLogM);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "VTime", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Taradod\\CoupleTaradod.cs", 62);
			}
			return result;
		}
	}

	public string KTime
	{
		get
		{
			string result = "00:00";
			try
			{
				if (Ktaradod != null)
				{
					result = Calendar.DateToFormatTime(Ktaradod.DateTimeLogM);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "KTime", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Taradod\\CoupleTaradod.cs", 76);
			}
			return result;
		}
	}

	public CoupleTaradod(string contextGuid, DateTime dateM)
	{
		ContextGuid = contextGuid;
		DateM = dateM;
		TypeTaradod = (TypeTaradodEnumType)3;
		Vtaradod = new LogOnPersonel(ContextGuid, DateM.Date);
		Ktaradod = new LogOnPersonel(ContextGuid, DateM.Date);
	}

	public async Task SetVorodAsync(StringBuilder sb, string pval, CancellationToken token)
	{
		sb.AppendLine($"line 91 in method satVorod in {DateTime.Now}");
		try
		{
			pval = Text.FixString(pval);
			string text = Calendar.DateToFormatTime(Vtaradod.DateTimeLogM);
			if (pval == text)
			{
				return;
			}
			if (pval != "")
			{
				if (pval == "24:00")
				{
					pval = "23:59:59";
				}
				if (!TimeSpan.TryParse(pval, out var result))
				{
					return;
				}
				DateTime dateTimeLogM = DateM.Date + result;
				Vtaradod.DateTimeLogM = dateTimeLogM;
				if ((int)Vtaradod.IsVirtual != 5)
				{
					Vtaradod.IsVirtual = (SavedTaradodEnumType)2;
				}
				if (Vtaradod.Id <= 0)
				{
					Vtaradod.PersonNo = PersonNo;
					Vtaradod.DeviceId_ = DeviceId;
					Vtaradod.TypeTaradod = (TypeTaradodEnumType)3;
					Vtaradod.TypeTaradodVacationOrMission = (TypeTaradodEnumType)3;
					Vtaradod.PersonId = PersonId;
					Vtaradod.IsVirtual = (SavedTaradodEnumType)5;
				}
				if ((int)Vtaradod.TypeTaradod == 1)
				{
					Vtaradod.TypeTaradod = (TypeTaradodEnumType)3;
					Vtaradod.TypeTaradodVacationOrMission = (TypeTaradodEnumType)3;
					await Vtaradod.SetIDtrafficFromTypeTaradodAsync(token);
				}
				sb.AppendLine($"line 125 in {DateTime.Now} =>Vtaradod:{Vtaradod.DateTimeLogM}");
				await Vtaradod.SaveAsync("CoupleTaradod.SetVorodAsync", token, "SetVorodAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Taradod\\CoupleTaradod.cs");
			}
			else if (Vtaradod.Id > 0 && pval == "")
			{
				await Vtaradod.DeleteAsync(token);
			}
			CheckValidVk();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetVorodAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Taradod\\CoupleTaradod.cs", 136);
		}
	}

	public async Task SetKhorojAsync(StringBuilder sb, string pVal, CancellationToken token)
	{
		sb.AppendLine($"line 142 in {DateTime.Now} method setKhoroj");
		try
		{
			pVal = Text.FixString(pVal);
			string text = Calendar.DateToFormatTime(Ktaradod.DateTimeLogM);
			if (pVal == text)
			{
				return;
			}
			if (pVal != "")
			{
				DateTime dateTimeLogM = ((!(pVal == "24:00")) ? (DateM.Date + TimeSpan.Parse(pVal)) : new DateTime(DateM.Year, DateM.Month, DateM.Day, 23, 59, 59));
				Ktaradod.DateTimeLogM = dateTimeLogM;
				if ((int)Ktaradod.IsVirtual != 5)
				{
					Ktaradod.IsVirtual = (SavedTaradodEnumType)2;
				}
				if (Ktaradod.Id <= 0)
				{
					Ktaradod.PersonNo = PersonNo;
					Ktaradod.DeviceId_ = DeviceId;
					Ktaradod.TypeTaradod = (TypeTaradodEnumType)4;
					Ktaradod.TypeTaradodVacationOrMission = (TypeTaradodEnumType)4;
					Ktaradod.PersonId = PersonId;
					Ktaradod.IsVirtual = (SavedTaradodEnumType)5;
				}
				if ((int)Ktaradod.TypeTaradod == 1)
				{
					Ktaradod.TypeTaradod = (TypeTaradodEnumType)4;
					Ktaradod.TypeTaradodVacationOrMission = (TypeTaradodEnumType)4;
					await Ktaradod.SetIDtrafficFromTypeTaradodAsync(token);
				}
				sb.AppendLine($"line 181 in {DateTime.Now}=> ktaradod:{Ktaradod.DateTimeLogM}");
				await Ktaradod.SaveAsync("CoupleTaradod.SetKhorojAsync", token, "SetKhorojAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Taradod\\CoupleTaradod.cs");
			}
			else if (Ktaradod.Id > 0 && pVal == "")
			{
				await Ktaradod.DeleteAsync(token);
			}
			string text2 = pVal;
			if (text2 == "24:00")
			{
				text2 = "23:59:59";
			}
			if (text2 == "")
			{
				text2 = "00:00";
			}
			Ktaradod.DateTimeLogM = Ktaradod.DateTimeLogM.Date + TimeSpan.Parse(text2);
			CheckValidVk();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "the value of pval = " + pVal, "SetKhorojAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Taradod\\CoupleTaradod.cs", 195);
		}
	}

	public void CheckValidVk()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected I4, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Invalid comparison between Unknown and I4
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected I4, but got Unknown
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Invalid comparison between Unknown and I4
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Invalid comparison between Unknown and I4
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Invalid comparison between Unknown and I4
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Invalid comparison between Unknown and I4
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Invalid comparison between Unknown and I4
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Invalid comparison between Unknown and I4
		try
		{
			if (Ktaradod == null || Vtaradod == null)
			{
				return;
			}
			TypeTaradodEnumType typeTaradod = TypeTaradod;
			switch (typeTaradod - 5)
			{
			default:
				if ((int)typeTaradod != 28)
				{
					switch (typeTaradod - 91)
					{
					case 0:
						break;
					case 1:
						goto IL_010a;
					case 2:
						goto IL_012a;
					default:
						goto IL_016a;
					}
					BackColorOfCell = StringExtensions.ToColor(ClsColor.BackColorShiftWork1, default(Color));
				}
				else
				{
					BackColorOfCell = StringExtensions.ToColor(ClsColor.BackColorTatilWork, default(Color));
				}
				break;
			case 0:
				BackColorOfCell = StringExtensions.ToColor(ClsColor.BackColorVacation, default(Color));
				break;
			case 2:
				BackColorOfCell = StringExtensions.ToColor(ClsColor.BackColorMission, default(Color));
				break;
			case 3:
				BackColorOfCell = StringExtensions.ToColor(ClsColor.BackColorFriDayWork, default(Color));
				break;
			case 5:
				BackColorOfCell = StringExtensions.ToColor(ClsColor.BackColorNightWork, default(Color));
				break;
			case 1:
			case 4:
				goto IL_016a;
				IL_010a:
				BackColorOfCell = StringExtensions.ToColor(ClsColor.BackColorShiftWork2, default(Color));
				break;
				IL_016a:
				BackColorOfCell = StringExtensions.ToColor(ClsColor.BackColorNormal, default(Color));
				break;
				IL_012a:
				BackColorOfCell = StringExtensions.ToColor(ClsColor.BackColorShiftWork3, default(Color));
				break;
			}
			if (Ktaradod.DateTimeLogM.Date == Vtaradod.DateTimeLogM.Date && Ktaradod.DateTimeLogM >= Vtaradod.DateTimeLogM)
			{
				if ((int)Ktaradod.IsVirtual == 2)
				{
					ColorKh = StringExtensions.ToColor(ClsColor.EditTaradodColor, default(Color));
				}
				else if ((int)Ktaradod.IsVirtual == 5)
				{
					ColorKh = StringExtensions.ToColor(ClsColor.AddTaradodColor, default(Color));
				}
				else if ((int)Ktaradod.IsVirtual == 4)
				{
					ColorKh = StringExtensions.ToColor(ClsColor.VirtualTaradodColor, default(Color));
				}
				else
				{
					ColorKh = StringExtensions.ToColor(ClsColor.RealTaradodColor, default(Color));
				}
				if ((int)Vtaradod.IsVirtual == 2)
				{
					ColorV = StringExtensions.ToColor(ClsColor.EditTaradodColor, default(Color));
				}
				else if ((int)Vtaradod.IsVirtual == 5)
				{
					ColorV = StringExtensions.ToColor(ClsColor.AddTaradodColor, default(Color));
				}
				else if ((int)Vtaradod.IsVirtual == 4)
				{
					ColorV = StringExtensions.ToColor(ClsColor.VirtualTaradodColor, default(Color));
				}
				else
				{
					ColorV = StringExtensions.ToColor(ClsColor.RealTaradodColor, default(Color));
				}
			}
			else
			{
				Color colorV = (ColorKh = Color.Red);
				ColorV = colorV;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CheckValidVk", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Taradod\\CoupleTaradod.cs", 263);
		}
	}

	public async Task<int> KarkardAsync(IEnumerable<ShiftDetView> shiftDetViews, CancellationToken token)
	{
		_ = 1;
		try
		{
			if (Vtaradod.Id <= 0 || Ktaradod.Id <= 0)
			{
				return 0;
			}
			DateTime dStart = Vtaradod.DateTimeLogM;
			DateTime dEnd = Ktaradod.DateTimeLogM;
			if (shiftDetViews != null)
			{
				List<ShiftDetView> detViews = shiftDetViews.ToList();
				ShiftDetView sh1 = await Shifts.GetShiftAsync(detViews, dStart, token);
				ShiftCodeing sh1Shiftcode = sh1?.ClsShiftCoding;
				if (sh1 != null && sh1Shiftcode.HurryDelayPerform && dStart >= sh1Shiftcode.VorodStart && dStart <= sh1Shiftcode.VorodEnd)
				{
					dStart = sh1Shiftcode.Vorod;
				}
				ShiftDetView shiftDetView = await Shifts.GetShiftAsync(detViews, dEnd, token);
				ShiftCodeing shiftCodeing = shiftDetView?.ClsShiftCoding;
				if (sh1 != null && shiftDetView != null && sh1Shiftcode.HurryDelayPerform && dEnd >= shiftCodeing.KhorojStart && dEnd <= shiftCodeing.KhorojEnd)
				{
					dEnd = shiftCodeing.Khoroj;
				}
			}
			return TimeDiff(dStart, dEnd);
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "KarkardAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Taradod\\CoupleTaradod.cs", 299);
			throw;
		}
	}

	public int TimeWorkOnTypeTaradod(TypeTaradodEnumType ptype)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (KTime != "00:00" && TypeTaradod == ptype)
			{
				TimeSpan timeSpan = Ktaradod.DateTimeLogM.Subtract(Vtaradod.DateTimeLogM);
				int num = timeSpan.Hours * 60 + timeSpan.Minutes;
				if (timeSpan.Seconds > 30)
				{
					num++;
				}
				return num;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TimeWorkOnTypeTaradod", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Taradod\\CoupleTaradod.cs", 318);
		}
		return 0;
	}

	public string GetToolTip(CancellationToken token = default(CancellationToken))
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected I4, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected I4, but got Unknown
		string text = "";
		try
		{
			TypeTaradodEnumType typeTaradod = TypeTaradod;
			switch (typeTaradod - 5)
			{
			default:
				if ((int)typeTaradod != 28)
				{
					switch (typeTaradod - 91)
					{
					case 0:
						break;
					case 1:
						goto IL_0077;
					case 2:
						goto IL_007f;
					default:
						goto IL_008f;
					}
					text = "[نوبت کاری صبح]";
				}
				else
				{
					text = "[تعطیل کاری]";
				}
				break;
			case 2:
				text = "[ماموریت]";
				break;
			case 0:
				text = "[مرخصی]";
				break;
			case 3:
				text = "[جمعه کاری]";
				break;
			case 5:
				text = "[شب کاری]";
				break;
			case 1:
			case 4:
				goto IL_008f;
				IL_0077:
				text = "[نوبت کاری عصر]";
				break;
				IL_008f:
				if (VTime != "00:00" && KTime == "00:00")
				{
					text = "[ورود و خروج ناقص]";
				}
				else if (VTime != "00:00" && KTime != "00:00")
				{
					text = "[ورود و خروج کامل]";
				}
				break;
				IL_007f:
				text = "[نوبت کاری شب]";
				break;
			}
			return Utilities.GetTranslate(text);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetToolTip", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Taradod\\CoupleTaradod.cs", 368);
			return "";
		}
	}

	public static int TimeDiff(DateTime d1, DateTime d2)
	{
		try
		{
			int num = (int)d2.Subtract(d1).TotalSeconds;
			int num2 = (Math.Abs(num) + 29) / 60;
			if (num < 0)
			{
				num2 = -num2;
			}
			return num2;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TimeDiff", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Taradod\\CoupleTaradod.cs", 384);
			return 0;
		}
	}

	public async Task<bool> SetDurationAsync(StringBuilder sb, bool isSetOnVorod, CancellationToken token)
	{
		sb.AppendLine($"line 392 in setDuration in {DateTime.Now}");
		try
		{
			int num = TimeDiff(Vtaradod.DateTimeLogM, Ktaradod.DateTimeLogM);
			if (isSetOnVorod)
			{
				if (Vtaradod.Duration == num)
				{
					return true;
				}
				Vtaradod.Duration = num;
				sb.AppendLine($"line 404 in {DateTime.Now} => Vtaradod:{Vtaradod.DateTimeLogM}");
				await Vtaradod.SaveAsync("CoupleTaradod.SetDurationAsync1", token, "SetDurationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Taradod\\CoupleTaradod.cs");
			}
			else
			{
				if (Ktaradod.Duration == num)
				{
					return true;
				}
				Ktaradod.Duration = num;
				sb.AppendLine($"line 415 in {DateTime.Now} => Ktaradod:{Ktaradod.DateTimeLogM}");
				await Ktaradod.SaveAsync("CoupleTaradod.SetDurationAsync2", token, "SetDurationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Taradod\\CoupleTaradod.cs");
			}
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetDurationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Taradod\\CoupleTaradod.cs", 420);
			return false;
		}
	}

	public int GetDuration()
	{
		try
		{
			return TimeDiff(Vtaradod.DateTimeLogM, Ktaradod.DateTimeLogM);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetDuration", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Taradod\\CoupleTaradod.cs", 432);
		}
		return 0;
	}

	public async Task<bool> AddDurationAsync(StringBuilder sb, bool isSetOnVorod, int duration, CancellationToken token)
	{
		sb.AppendLine($"in {DateTime.Now} method AddDuration");
		try
		{
			if (isSetOnVorod)
			{
				Vtaradod.Duration = duration;
				sb.AppendLine($"in {DateTime.Now}=> Vtaradod:{Vtaradod.DateTimeLogM}");
				await Vtaradod.SaveAsync("CoupleTaradod.AddDurationAsync1", token, "AddDurationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Taradod\\CoupleTaradod.cs");
			}
			else
			{
				Ktaradod.Duration = duration;
				sb.AppendLine($"in {DateTime.Now}=> Ktaradod:{Ktaradod.DateTimeLogM}");
				await Ktaradod.SaveAsync("CoupleTaradod.AddDurationAsync2", token, "AddDurationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Taradod\\CoupleTaradod.cs");
			}
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "AddDurationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Taradod\\CoupleTaradod.cs", 457);
			return false;
		}
	}

	public int GetLunchMinutes(short lunchTimeStart, short lunchTimeEnd)
	{
		try
		{
			LogOnPersonel vtaradod = Vtaradod;
			if (vtaradod == null || vtaradod.Id == 0)
			{
				goto IL_0028;
			}
			LogOnPersonel ktaradod = Ktaradod;
			if (ktaradod == null || ktaradod.Id == 0)
			{
				goto IL_0028;
			}
			int num = Vtaradod.DateTimeLogM.Hour * 60 + Vtaradod.DateTimeLogM.Minute;
			int num2 = Ktaradod.DateTimeLogM.Hour * 60 + Ktaradod.DateTimeLogM.Minute;
			if (num2 <= num || Vtaradod.DateTimeLogM.Day != Ktaradod.DateTimeLogM.Day)
			{
				return 0;
			}
			if (Vtaradod.DateTimeLogM.Day < Ktaradod.DateTimeLogM.Day)
			{
				num2 = lunchTimeEnd;
			}
			if (num <= lunchTimeStart && num2 >= lunchTimeStart && num2 <= lunchTimeEnd)
			{
				return num2 - lunchTimeStart;
			}
			if (num >= lunchTimeStart && num2 <= lunchTimeEnd)
			{
				return num2 - num;
			}
			if (num <= lunchTimeStart && num2 >= lunchTimeEnd)
			{
				return lunchTimeEnd - lunchTimeStart;
			}
			if (num >= lunchTimeStart && num <= lunchTimeEnd && num2 >= lunchTimeEnd)
			{
				return lunchTimeEnd - num;
			}
			goto end_IL_0000;
			IL_0028:
			return 0;
			end_IL_0000:;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetLunchMinutes", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Taradod\\CoupleTaradod.cs", 486);
		}
		return 0;
	}
}

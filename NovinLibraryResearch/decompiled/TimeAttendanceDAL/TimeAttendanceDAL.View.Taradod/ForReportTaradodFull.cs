using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Attendance.Entities;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Dal.User;

namespace TimeAttendanceDAL.View.Taradod;

[Serializable]
[DoNotPruneType]
public class ForReportTaradodFull
{
	public string V1DeviceName { get; set; }

	public string V2DeviceName { get; set; }

	public string V3DeviceName { get; set; }

	public string V4DeviceName { get; set; }

	public string V5DeviceName { get; set; }

	public string V6DeviceName { get; set; }

	public string V7DeviceName { get; set; }

	public string V8DeviceName { get; set; }

	public string K1DeviceName { get; set; }

	public string K2DeviceName { get; set; }

	public string K3DeviceName { get; set; }

	public string K4DeviceName { get; set; }

	public string K5DeviceName { get; set; }

	public string K6DeviceName { get; set; }

	public string K7DeviceName { get; set; }

	public string K8DeviceName { get; set; }

	public string V1 { get; set; }

	public string V2 { get; set; }

	public string V3 { get; set; }

	public string V4 { get; set; }

	public string V5 { get; set; }

	public string V6 { get; set; }

	public string V7 { get; set; }

	public string V8 { get; set; }

	public string K1 { get; set; }

	public string K2 { get; set; }

	public string K3 { get; set; }

	public string K4 { get; set; }

	public string K5 { get; set; }

	public string K6 { get; set; }

	public string K7 { get; set; }

	public string K8 { get; set; }

	public int OnHalioDayWorkKarkard { get; set; }

	public int OnTatilWorkKarkard { get; set; }

	public int ShiftWorkKarkard1 { get; set; }

	public int ShiftWorkKarkard2 { get; set; }

	public int ShiftWorkKarkard3 { get; set; }

	public int NightWorkKarkard { get; set; }

	public string Hozor_Time { get; set; }

	public int DayOfMounth { get; set; }

	public string DayOfWeek { get; set; }

	public string DateSH { get; set; }

	public string StatusName { get; set; }

	public string ShiftTime { get; set; }

	public string EKTime { get; set; }

	public string KKTime { get; set; }

	public string MorSaatiTime { get; set; }

	public string Mission { get; set; }

	public string Year { get; set; }

	public string month { get; set; }

	public int Sum_Karkerd { get; set; }

	public int Sum_Gheybat { get; set; }

	public int Sum_Kasrkar { get; set; }

	public int Sum_Ezafekar { get; set; }

	public int Sum_Vacation { get; set; }

	public int Sum_Mission { get; set; }

	public int Sum_Night { get; set; }

	public int Sum_Friday { get; set; }

	public int Sum_Tatil { get; set; }

	public int Sum_NobatKari1 { get; set; }

	public int Sum_NobatKari2 { get; set; }

	public int Sum_NobatKari3 { get; set; }

	public int CountKarkerd { get; set; }

	public int CountWorkdayHoghogh { get; set; }

	public int CountMorakhasiAll { get; set; }

	public int CountMorakhasiWithSalary { get; set; }

	public int CountMorakhasi { get; set; }

	public int CountGheybat { get; set; }

	public int CountTatil { get; set; }

	public int CountNaghes { get; set; }

	public int Countshift { get; set; }

	public int PersonID { get; set; }

	public long CodeTafsil { get; set; }

	public string FullName { get; set; }

	public string GroupName { get; set; }

	public int MissionDaily { get; set; }

	public int SumMoazafiMin { get; set; }

	public string SumMoazafiTime { get; set; }

	public int MoazafiMin { get; set; }

	public string MoazafiTime { get; set; }

	public static async Task<List<ForReportTaradodFull>> FillListAsync(string contextGuid, TaradodFull pcls, CancellationToken token = default(CancellationToken))
	{
		List<ForReportTaradodFull> lstdet = new List<ForReportTaradodFull>();
		int lineno = 1;
		List<DeviceInfo> devices = await DeviceInfo.GetAllAsync(contextGuid, token);
		try
		{
			if (pcls?.ListTaradodDets != null)
			{
				foreach (TaradodFullDetView item in pcls.ListTaradodDets)
				{
					CoupleTaradod[] tardodCurDey = item.CoupleTaradodCurrentDay;
					lineno = 2;
					ForReportTaradodFull xcls = new ForReportTaradodFull();
					lineno = 3;
					xcls.V1 = tardodCurDey[0].VTime + GetChar(tardodCurDey[0].Vtaradod.IsVirtual);
					xcls.V2 = tardodCurDey[1].VTime + GetChar(tardodCurDey[1].Vtaradod.IsVirtual);
					xcls.V3 = tardodCurDey[2].VTime + GetChar(tardodCurDey[2].Vtaradod.IsVirtual);
					xcls.V4 = tardodCurDey[3].VTime + GetChar(tardodCurDey[3].Vtaradod.IsVirtual);
					xcls.V5 = tardodCurDey[4].VTime + GetChar(tardodCurDey[4].Vtaradod.IsVirtual);
					xcls.V6 = tardodCurDey[5].VTime + GetChar(tardodCurDey[5].Vtaradod.IsVirtual);
					xcls.V7 = tardodCurDey[6].VTime + GetChar(tardodCurDey[6].Vtaradod.IsVirtual);
					xcls.V8 = tardodCurDey[7].VTime + GetChar(tardodCurDey[7].Vtaradod.IsVirtual);
					xcls.K1 = tardodCurDey[0].KTime + GetChar(tardodCurDey[0].Ktaradod.IsVirtual);
					xcls.K2 = tardodCurDey[1].KTime + GetChar(tardodCurDey[1].Ktaradod.IsVirtual);
					xcls.K3 = tardodCurDey[2].KTime + GetChar(tardodCurDey[2].Ktaradod.IsVirtual);
					xcls.K4 = tardodCurDey[3].KTime + GetChar(tardodCurDey[3].Ktaradod.IsVirtual);
					xcls.K5 = tardodCurDey[4].KTime + GetChar(tardodCurDey[4].Ktaradod.IsVirtual);
					xcls.K6 = tardodCurDey[5].KTime + GetChar(tardodCurDey[5].Ktaradod.IsVirtual);
					xcls.K7 = tardodCurDey[6].KTime + GetChar(tardodCurDey[6].Ktaradod.IsVirtual);
					xcls.K8 = tardodCurDey[7].KTime + GetChar(tardodCurDey[7].Ktaradod.IsVirtual);
					xcls.V1DeviceName = devices.FirstOrDefault((DeviceInfo p) => p.Id == tardodCurDey[0].Vtaradod.DeviceId_)?.DeviceName ?? "";
					xcls.V2DeviceName = devices.FirstOrDefault((DeviceInfo p) => p.Id == tardodCurDey[1].Vtaradod.DeviceId_)?.DeviceName ?? "";
					xcls.V3DeviceName = devices.FirstOrDefault((DeviceInfo p) => p.Id == tardodCurDey[2].Vtaradod.DeviceId_)?.DeviceName ?? "";
					xcls.V4DeviceName = devices.FirstOrDefault((DeviceInfo p) => p.Id == tardodCurDey[3].Vtaradod.DeviceId_)?.DeviceName ?? "";
					xcls.V5DeviceName = devices.FirstOrDefault((DeviceInfo p) => p.Id == tardodCurDey[4].Vtaradod.DeviceId_)?.DeviceName ?? "";
					xcls.V6DeviceName = devices.FirstOrDefault((DeviceInfo p) => p.Id == tardodCurDey[5].Vtaradod.DeviceId_)?.DeviceName ?? "";
					xcls.V7DeviceName = devices.FirstOrDefault((DeviceInfo p) => p.Id == tardodCurDey[6].Vtaradod.DeviceId_)?.DeviceName ?? "";
					xcls.V8DeviceName = devices.FirstOrDefault((DeviceInfo p) => p.Id == tardodCurDey[7].Vtaradod.DeviceId_)?.DeviceName ?? "";
					xcls.K1DeviceName = devices.FirstOrDefault((DeviceInfo p) => p.Id == tardodCurDey[0].Ktaradod.DeviceId_)?.DeviceName ?? "";
					xcls.K2DeviceName = devices.FirstOrDefault((DeviceInfo p) => p.Id == tardodCurDey[1].Ktaradod.DeviceId_)?.DeviceName ?? "";
					xcls.K3DeviceName = devices.FirstOrDefault((DeviceInfo p) => p.Id == tardodCurDey[2].Ktaradod.DeviceId_)?.DeviceName ?? "";
					xcls.K4DeviceName = devices.FirstOrDefault((DeviceInfo p) => p.Id == tardodCurDey[3].Ktaradod.DeviceId_)?.DeviceName ?? "";
					xcls.K5DeviceName = devices.FirstOrDefault((DeviceInfo p) => p.Id == tardodCurDey[4].Ktaradod.DeviceId_)?.DeviceName ?? "";
					xcls.K6DeviceName = devices.FirstOrDefault((DeviceInfo p) => p.Id == tardodCurDey[5].Ktaradod.DeviceId_)?.DeviceName ?? "";
					xcls.K7DeviceName = devices.FirstOrDefault((DeviceInfo p) => p.Id == tardodCurDey[6].Ktaradod.DeviceId_)?.DeviceName ?? "";
					xcls.K8DeviceName = devices.FirstOrDefault((DeviceInfo p) => p.Id == tardodCurDey[7].Ktaradod.DeviceId_)?.DeviceName ?? "";
					lineno = 19;
					ForReportTaradodFull forReportTaradodFull = xcls;
					forReportTaradodFull.OnHalioDayWorkKarkard = await item.GetOnFriDayWorkKarkardAsync(token);
					lineno = 20;
					forReportTaradodFull = xcls;
					forReportTaradodFull.OnTatilWorkKarkard = await item.GetOnTatilWorkKarkardAsync(token);
					lineno = 100;
					forReportTaradodFull = xcls;
					forReportTaradodFull.ShiftWorkKarkard1 = await item.GetShiftWorkKarkard1Aaync(token);
					forReportTaradodFull = xcls;
					forReportTaradodFull.ShiftWorkKarkard2 = await item.GetShiftWorkKarkard2Async(token);
					forReportTaradodFull = xcls;
					forReportTaradodFull.ShiftWorkKarkard3 = await item.GetShiftWorkKarkard3Async(token);
					lineno = 21;
					forReportTaradodFull = xcls;
					forReportTaradodFull.NightWorkKarkard = await item.GetNightWorkKarkardAsync(token);
					lineno = 22;
					xcls.Year = pcls.Year;
					lineno = 23;
					xcls.month = pcls.Month;
					lineno = 24;
					xcls.DateSH = item.DateSh;
					lineno = 25;
					xcls.Sum_Karkerd = pcls.SumKarkerd;
					lineno = 26;
					xcls.Sum_Gheybat = pcls.CountGheybat;
					lineno = 27;
					xcls.Sum_Kasrkar = pcls.SumKasrkar;
					lineno = 28;
					xcls.Sum_Ezafekar = pcls.SumEzafekar;
					lineno = 29;
					xcls.Sum_Vacation = pcls.SumWorkVacation;
					lineno = 30;
					xcls.Sum_Mission = pcls.SumWorkMission;
					lineno = 31;
					xcls.Sum_Night = pcls.SumWorkNightWork;
					lineno = 32;
					xcls.Sum_NobatKari1 = pcls.SumNobatKari1;
					xcls.Sum_NobatKari2 = pcls.SumNobatKari2;
					xcls.Sum_NobatKari3 = pcls.SumNobatKari3;
					lineno = 33;
					forReportTaradodFull = xcls;
					forReportTaradodFull.Hozor_Time = await item.GetHozorTimeAsync(token);
					lineno = 34;
					xcls.DayOfMounth = item.DayofShMonth;
					lineno = 35;
					xcls.DayOfWeek = item.Dayofweek;
					lineno = 36;
					forReportTaradodFull = xcls;
					forReportTaradodFull.StatusName = await item.GetNameStatuseTaradodAsync(token);
					lineno = 37;
					xcls.ShiftTime = item.ShiftTime;
					lineno = 38;
					forReportTaradodFull = xcls;
					forReportTaradodFull.EKTime = await item.GetEkTimeAsync(token);
					lineno = 39;
					forReportTaradodFull = xcls;
					forReportTaradodFull.KKTime = await item.GetKkTimeAsync(token);
					lineno = 40;
					xcls.MorSaatiTime = item.MorSaatiTime;
					lineno = 41;
					xcls.Mission = item.MissionTime;
					lineno = 42;
					xcls.CountKarkerd = pcls.CountKarkerd;
					xcls.CountWorkdayHoghogh = pcls.CountWorkdayHoghogh;
					lineno = 43;
					xcls.CountMorakhasiAll = pcls.CountMorakhasiWithSalary + pcls.CountMorakhasiNotSalary;
					lineno = 44;
					xcls.CountMorakhasiWithSalary = pcls.CountMorakhasiWithSalary;
					xcls.CountMorakhasi = pcls.CountMorakhasi;
					lineno = 45;
					xcls.Sum_Friday = pcls.SumFriDayWork;
					lineno = 46;
					xcls.Sum_Tatil = pcls.SumTatilDayWork;
					lineno = 146;
					xcls.CountGheybat = pcls.CountGheybat;
					lineno = 47;
					xcls.CountTatil = pcls.CountTatil;
					lineno = 48;
					xcls.CountNaghes = pcls.CountNaghes;
					lineno = 49;
					xcls.Countshift = pcls.Countshift;
					lineno = 50;
					xcls.PersonID = ((PersonBase)item.PersonInfo).Id;
					lineno = 51;
					xcls.CodeTafsil = item.CodeTafsil;
					lineno = 52;
					xcls.FullName = item.FullName;
					lineno = 53;
					forReportTaradodFull = xcls;
					forReportTaradodFull.GroupName = await item.PersonInfo.GetGroupNameAsync(token);
					lineno = 54;
					xcls.MissionDaily = pcls.MissionDaily;
					lineno = 55;
					if (await item.GetKarkardAsync(token) > 0 && item.SumShiftMinuts > 0)
					{
						xcls.MoazafiMin = item.GetSumMoazafiMinutes();
						xcls.MoazafiTime = Time.IntToTime(xcls.MoazafiMin);
					}
					xcls.SumMoazafiMin = pcls.SumMoazzafi;
					xcls.SumMoazafiTime = Time.IntToTime(pcls.SumMoazzafi);
					lstdet.Add(xcls);
				}
			}
		}
		catch (Exception ex)
		{
			StackTrace stackTrace = new StackTrace(ex, fNeedFileInfo: true);
			int num = 0;
			StackFrame[] frames = stackTrace.GetFrames();
			foreach (StackFrame stackFrame in frames)
			{
				if (!string.IsNullOrEmpty(stackFrame.GetFileName()))
				{
					num = stackFrame.GetFileLineNumber();
					break;
				}
			}
			if (pcls?.ListTaradodDets != null)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, $"line: {lineno} CoupleTaradodReadOnly: {pcls.ListTaradodDets[0].CoupleTaradodReadOnly}  StackTraceLine: {num}", "FillListAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Taradod\\ForReportTaradodFull.cs", 257);
			}
		}
		return lstdet;
	}

	private static string GetChar(SavedTaradodEnumType vtaradodIsVirtual)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Invalid comparison between Unknown and I4
		try
		{
			if ((int)vtaradodIsVirtual != 2)
			{
				if ((int)vtaradodIsVirtual == 5)
				{
					return UserAttendance.SelectedUser.UserSetting.ManualAddTaradodCharacter;
				}
				return "";
			}
			return UserAttendance.SelectedUser.UserSetting.ManualEditTaradodCharacter;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetChar", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Taradod\\ForReportTaradodFull.cs", 278);
		}
		return "";
	}
}

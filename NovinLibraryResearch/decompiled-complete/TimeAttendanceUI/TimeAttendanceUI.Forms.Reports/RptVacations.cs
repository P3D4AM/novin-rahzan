using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom.Cache;
using PacketParser;
using PacketParser.Attendance.Entities;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Dal.Vacation;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendancePrint.UI;

namespace TimeAttendanceUI.Forms.Reports;

public class RptVacations : IHasContext
{
	private List<Morakhasidaily> _lstMorakhasidaily = new List<Morakhasidaily>();

	private DateTime _startDateM = DateTime.Now;

	private DateTime _endDateM = DateTime.Now;

	public string ContextGuid { get; set; }

	public RptVacations(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public async Task ReportAsync(IWin32Window owner, DateTime startDateM, DateTime endDateM, List<int> vacations, CancellationToken token = default(CancellationToken))
	{
		_ = 6;
		try
		{
			List<RemindVacation> remindVacationList = new List<RemindVacation>();
			_startDateM = startDateM;
			_endDateM = endDateM;
			List<Morakhasidaily> list = await Morakhasidaily.GetSpecificDataAsync(ContextGuid, new List<int>(), startDateM, endDateM, "", token);
			_lstMorakhasidaily = ((vacations.Count > 0) ? (from gl in list
				join fi in vacations on gl.TypeId equals fi
				select gl).ToList() : list.ToList());
			_lstMorakhasidaily = _lstMorakhasidaily.OrderBy((Morakhasidaily p) => p.PersonFullName).ToList();
			List<object> lst = new List<object>();
			foreach (Morakhasidaily item2 in _lstMorakhasidaily)
			{
				GharardadNew clsGharardad2 = await GharardadNew.GetAsync(ContextGuid, item2.PersonId, item2.DateFrom, token);
				double item3 = (await Morakhasidaily.CalcLeaveBalanceAsync(ContextGuid, clsGharardad2, token)).Item3;
				if (remindVacationList.All((RemindVacation p) => p.personId != item2.PersonId))
				{
					remindVacationList.Add(new RemindVacation
					{
						personId = item2.PersonId,
						remindVacation = Math.Round(item3, 2),
						VacationTime1Day = Time.IntToTime((int)(clsGharardad2?.LeaveMinutes ?? 440))
					});
				}
				lst.Add(new
				{
					Date = Calendar.MiladiToCalture(item2.DateFrom),
					PersonId = item2.PersonId,
					FullName = item2.PersonFullName,
					Dow = item2.Dow,
					TypeMorakhasyName = item2.TypeMorakhasiName,
					DurationMin = (clsGharardad2?.LeaveMinutes ?? 440),
					DurationTime = Time.IntToTime((int)(clsGharardad2?.LeaveMinutes ?? 440)),
					Desc = item2.Desc,
					HasHoghogh = item2.HasHoghogh
				});
			}
			List<int> list2 = (await TimeAttendanceDAL.Dal.FLRPerson.Person.GetAllAsync(ContextGuid, new List<int>(), personStatus: true, token))?.Select((TimeAttendanceDAL.Dal.FLRPerson.Person p) => ((PersonBase)p).Id).ToList();
			if ((vacations.Count == 0 || vacations.Contains(999)) && list2 != null)
			{
				foreach (int item5 in list2)
				{
					foreach (MorakhasiSaati item1 in await MorakhasiSaati.GetSpecificDataAsync(ContextGuid, item5, _startDateM, _endDateM, (TypeTaradodEnumType)5, token))
					{
						string personFullName = item1.PersonFullName;
						lst.Add(new
						{
							Date = Calendar.MiladiToCalture(item1.DateFrom),
							PersonID = item1.PersonID,
							fullName = personFullName,
							Dow = item1.DayOfWeek,
							TypeMorakhasyName = Utilities.GetTranslate("[مرخصی ساعتی]"),
							DurationMin = item1.Duration,
							DurationTime = Time.IntToTime(item1.Duration),
							Desc = "",
							Hoghogh = true
						});
						if (!remindVacationList.Any((RemindVacation p) => p.personId == item1.PersonID))
						{
							GharardadNew clsGharardad2 = await GharardadNew.GetAsync(ContextGuid, item1.PersonID, item1.DateFrom, token);
							double item4 = (await Morakhasidaily.CalcLeaveBalanceAsync(ContextGuid, clsGharardad2, token)).Item3;
							remindVacationList.Add(new RemindVacation
							{
								personId = item1.PersonID,
								remindVacation = Math.Round(item4, 2),
								VacationTime1Day = Time.IntToTime((int)(clsGharardad2?.LeaveMinutes ?? 440))
							});
						}
					}
				}
			}
			if (lst.Count <= 0 || remindVacationList.Count <= 0)
			{
				FMessageBox.Show(Utilities.GetTranslate("[موردی برای نمایش یافت نشد. لطفا بازه تاریخ یا نوع مرخصی را بررسی نمائید]"));
				return;
			}
			List<object> lst2 = remindVacationList.Cast<object>().ToList();
			PrintRpt(owner, lst, lst2);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\RptVacations.cs", 106);
		}
	}

	private async void PrintRpt(IWin32Window owner, List<object> lstMorakhasi, List<object> lst2)
	{
		try
		{
			FrmPrint frm = new FrmPrint(ContextGuid, (EnumPrintShow)15, lstMorakhasi, null, 0, "", lst2);
			await ((CacheBaseForm)frm).ShowAsync(owner);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "PrintRpt", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\RptVacations.cs", 119);
		}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom.Cache;
using PacketParser.Attendance.Entities;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.View.Taradod;
using TimeAttendancePrint.UI;

namespace TimeAttendanceUI.Forms.Reports;

public class RptAllTaradod : IHasContext
{
	private readonly List<TaradodFull> _lsTaradodFulls = new List<TaradodFull>();

	public string ContextGuid { get; set; }

	public RptAllTaradod(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public async Task ReportAsync(IWin32Window owner, DateTime startDateTime, DateTime endDateTime, List<int> personIDs, CancellationToken token)
	{
		try
		{
			_lsTaradodFulls.Clear();
			List<TimeAttendanceDAL.Dal.FLRPerson.Person> list = await TimeAttendanceDAL.Dal.FLRPerson.Person.GetAllAsync(ContextGuid, personIDs, personStatus: true, token);
			if (personIDs.Count > 0)
			{
				list = list.Where((TimeAttendanceDAL.Dal.FLRPerson.Person p) => personIDs.Contains(((PersonBase)p).Id)).ToList();
			}
			foreach (TimeAttendanceDAL.Dal.FLRPerson.Person item in list)
			{
				if (token.IsCancellationRequested)
				{
					return;
				}
				TaradodFull taradodFull = await TaradodFull.CreateAsync(ContextGuid, startDateTime, endDateTime, item, null, checkValidationVorodAndKHoroj: false, default(CancellationToken));
				await taradodFull.FillTaradodFullPropertysAsync(token);
				_lsTaradodFulls.Add(taradodFull);
			}
			if (!token.IsCancellationRequested)
			{
				await PrintRptAsync(owner, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\RptAllTaradod.cs", 44);
		}
	}

	private async Task PrintRptAsync(IWin32Window owner, CancellationToken token)
	{
		_ = 1;
		try
		{
			List<object> lst = new List<object>();
			foreach (TaradodFull lsTaradodFull in _lsTaradodFulls)
			{
				if (token.IsCancellationRequested)
				{
					return;
				}
				List<ForReportTaradodFull> collection = await ForReportTaradodFull.FillListAsync(ContextGuid, lsTaradodFull);
				if (token.IsCancellationRequested)
				{
					return;
				}
				lst.AddRange(collection);
			}
			if (!token.IsCancellationRequested)
			{
				FrmPrint frm = new FrmPrint(ContextGuid, (EnumPrintShow)13, lst, null, 0, "");
				await ((CacheBaseForm)frm).ShowAsync(owner);
				((Component)(object)frm).Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "PrintRptAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\RptAllTaradod.cs", 66);
		}
	}
}

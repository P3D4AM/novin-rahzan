using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using TimeAttendanceDAL.Settings;
using TimeAttendancePrint.UI;

namespace TimeAttendancePrint;

public class ClsPrint : IHasContext
{
	public string ContextGuid { get; set; }

	public ClsPrint(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public async Task Print2PrinterAsync(StiReport st, DataSet ds, List<object> list, int sanadId, string extraVar, List<object> list2 = null, CancellationToken token = default(CancellationToken))
	{
		try
		{
			((CollectionBase)(object)st.Dictionary.Databases).Clear();
			StiSqlDatabase val = new StiSqlDatabase("Database", Extensions.GetSqlConnectionString(ContextGuid));
			st.Dictionary.Databases.Add((StiDatabase)(object)val);
			st.Dictionary.Variables.Add("DateM", (object)DateTime.Now);
			st.Dictionary.Variables.Add("DateCalture", (object)Calendar.MiladiToCalture(DateTime.Now));
			st.Dictionary.Variables.Add("SanadID", (object)sanadId);
			st.Dictionary.Variables.Add("ExtraVar", (object)extraVar);
			st.Dictionary.Variables.Add("Time", (object)Time.IntToTime(DateTime.Now.Hour * 60 + DateTime.Now.Minute));
			st.RegData("لیست داده ها", (object)list);
			st.RegData("DataList", (object)list2);
			st.RegData("منبع داده ها", (object)(await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(ContextGuid, token)).SaleKari);
			if (Print.ShowDesign)
			{
				st.Design();
			}
			else
			{
				st.Print(false, (short)1);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Print2PrinterAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendancePrint\\_ClsPrint.cs", 51);
		}
	}

	public async Task PrintPreview(StiReport st, DataSet ds, List<object> list, int sanadId, string extraVar, List<object> list2 = null, CancellationToken token = default(CancellationToken))
	{
		try
		{
			((CollectionBase)(object)st.Dictionary.Databases).Clear();
			StiSqlDatabase val = new StiSqlDatabase("Database", Extensions.GetSqlConnectionString(ContextGuid));
			st.Dictionary.Databases.Add((StiDatabase)(object)val);
			st.Dictionary.Variables.Add("DateM", (object)DateTime.Now);
			st.Dictionary.Variables.Add("DateCalture", (object)Calendar.MiladiToCalture(DateTime.Now));
			st.Dictionary.Variables.Add("SanadID", (object)sanadId);
			st.Dictionary.Variables.Add("ExtraVar", (object)extraVar);
			st.Dictionary.Variables.Add("Time", (object)Time.IntToTime(DateTime.Now.Hour * 60 + DateTime.Now.Minute));
			st.RegData("لیست داده ها", (object)list);
			if (list2 != null)
			{
				st.RegData("DataList", (object)list2);
			}
			st.RegData("منبع داده ها", (object)(await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(ContextGuid, token)).SaleKari);
			if (Print.ShowDesign)
			{
				st.Design();
				return;
			}
			FrmPrintPreview frmPrintPreview = new FrmPrintPreview(st, ContextGuid);
			((Form)frmPrintPreview).ShowDialog();
			((Component)(object)frmPrintPreview).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "PrintPreview", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendancePrint\\_ClsPrint.cs", 90);
		}
	}

	public async Task Design(StiReport st, DataSet ds, List<object> list, int sanadId, string extraVar, List<object> list2 = null, CancellationToken token = default(CancellationToken))
	{
		try
		{
			((CollectionBase)(object)st.Dictionary.Databases).Clear();
			StiSqlDatabase val = new StiSqlDatabase("Database", Extensions.GetSqlConnectionString(ContextGuid));
			st.Dictionary.Databases.Add((StiDatabase)(object)val);
			st.Dictionary.Variables.Add("DateM", (object)DateTime.Now);
			st.Dictionary.Variables.Add("DateCalture", (object)Calendar.MiladiToCalture(DateTime.Now));
			st.Dictionary.Variables.Add("SanadID", (object)sanadId);
			st.Dictionary.Variables.Add("ExtraVar", (object)extraVar);
			st.Dictionary.Variables.Add("Time", (object)Time.IntToTime(DateTime.Now.Hour * 60 + DateTime.Now.Minute));
			st.RegData("لیست داده ها", (object)list);
			st.RegData("DataList", (object)list2);
			st.RegData("منبع داده ها", (object)(await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(ContextGuid, token)).SaleKari);
			try
			{
				st.Design();
			}
			catch
			{
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Design", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendancePrint\\_ClsPrint.cs", 128);
		}
	}
}

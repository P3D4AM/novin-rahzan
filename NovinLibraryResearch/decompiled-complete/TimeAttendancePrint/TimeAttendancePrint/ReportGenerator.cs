using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using Serilog;
using Stimulsoft.Report;
using TimeAttendancePrint.SetSTI;

namespace TimeAttendancePrint;

public class ReportGenerator : IHasContext
{
	private EnumPrintShow _reportType;

	private DataSet _ds = new DataSet();

	private DataView _dv = new DataView();

	private StiReport _sti;

	private List<object> _list;

	private List<object> _list2;

	private int _sanadId;

	private string _extraVar;

	private ClsGeneral _clsGeneral;

	public string ContextGuid { get; set; }

	public ReportGenerator(string contextGuid, EnumPrintShow reportType, Print.EnPrinterType paper, List<object> list, DataView dv, int sanadId, string extraVar, List<object> list2 = null)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		ContextGuid = contextGuid;
		_reportType = reportType;
		_dv = dv;
		_list = list;
		if (list2 != null)
		{
			_list2 = list2;
		}
		_sanadId = sanadId;
		_extraVar = extraVar;
		_clsGeneral = new ClsGeneral();
		SeTsti(paper);
		AddView(_dv);
	}

	private bool AddTable(DataTable tbl)
	{
		DataView dataView = new DataView();
		dataView.Table = tbl;
		AddView(dataView);
		return true;
	}

	private dynamic AddView(DataView v)
	{
		if (v != null && v.Count > 0)
		{
			_ds.Tables.Add(v.ToTable());
			return true;
		}
		return false;
	}

	private bool AddDs(DataSet ds)
	{
		_ds = ds;
		return true;
	}

	public async Task Print2PrinterAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			await new ClsPrint(ContextGuid).Print2PrinterAsync(_sti, _ds, _list, _sanadId, _extraVar, _list2, token);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task PrintAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			await new ClsPrint(ContextGuid).Print2PrinterAsync(_sti, _ds, _list, _sanadId, _extraVar, _list2, token);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public void PrintPreview()
	{
		new ClsPrint(ContextGuid).PrintPreview(_sti, _ds, _list, _sanadId, _extraVar, _list2);
	}

	public void Design()
	{
		new ClsPrint(ContextGuid).Design(_sti, _ds, _list, _sanadId, _extraVar, _list2);
	}

	private void SeTsti(Print.EnPrinterType paPer)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		_sti = new StiReport();
		_sti = _clsGeneral.GetSti(_sti, paPer, _reportType);
	}
}

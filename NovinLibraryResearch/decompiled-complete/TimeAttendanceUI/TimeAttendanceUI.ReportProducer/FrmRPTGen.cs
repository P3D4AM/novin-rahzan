using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.Other;
using TimeAttendanceDAL.Dal.Reports;
using TimeAttendanceDAL.Utility;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.ReportProducer;

public class FrmRPTGen : BaseForm
{
	private bool _isDisposed;

	private CancellationTokenSource _initializeToken = new CancellationTokenSource();

	private RPTGen _cls;

	private Font _font;

	private IContainer components;

	internal PanelEx PanelEx1;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	internal PanelEx PanelEx3;

	private ButtonX BtnDown;

	internal LabelX LabelX4;

	internal LabelX LabelX3;

	private ButtonX BtnUP;

	private ButtonX BtnRemove;

	private ButtonX BtnAdd;

	internal DataGridViewX DGridSelection;

	internal DataGridViewTextBoxColumn DGIndex;

	internal DataGridViewTextBoxColumn DGNameS;

	internal DataGridViewTextBoxColumn DGFNameS;

	internal DataGridViewX DGrid;

	internal PanelEx PanelEx2;

	internal LabelX LabelX5;

	internal TextBox Txtdesc;

	internal TextBox TxtQuery;

	private ButtonX BtnStim;

	internal TextBox TxtTitle;

	internal LabelX LabelX2;

	internal LabelX LabelX1;

	internal DataGridViewX DGridSqlP;

	private BindingSource controlsBindingSource;

	private BindingSource sQLParametersBindingSource;

	private DataGridViewTextBoxColumn DGName;

	private DataGridViewTextBoxColumn DGFANAME;

	private DataGridViewTextBoxColumn DGNameSQL;

	private DataGridViewTextBoxColumn FanameSql;

	private bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((BaseForm)this).IsDisposed;
			}
			return true;
		}
	}

	public FrmRPTGen(string contextGuid, RPTGen report, Font font)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		FrmRPTGen frmRPTGen = this;
		try
		{
			_font = font;
			InitializeComponent();
			_cls = report;
			((Control)TxtTitle).Focus();
			CancellationToken initToken = _initializeToken.Token;
			new Waiter("بارگذاری داده...", (Form)(object)this, Task.Run(() => frmRPTGen.InitAsync(initToken)), true, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGen.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGen.cs", 38);
		}
	}

	private async Task InitAsync(CancellationToken token)
	{
		try
		{
			string formTitle = Utilities.GetTranslate("[افزودن طراحی گزارش]");
			if (_cls.ID > 0)
			{
				formTitle = Utilities.GetTranslate("[ویرایش طراحی گزارش]");
			}
			List<SQLParameters> parameters = SQLParameters.GetAll();
			List<Controls> controls = await Controls.GetAllAsync();
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)async delegate
			{
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				try
				{
					((Control)TxtTitle).Focus();
					((Control)this).Text = formTitle;
					controlsBindingSource.DataSource = controls;
					sQLParametersBindingSource.DataSource = parameters;
					((Control)TxtQuery).Text = _cls.SQLQuery;
					((Control)TxtTitle).Text = _cls.Title;
					((Control)Txtdesc).Text = _cls.Description;
					foreach (RPTGenDet item in (await _cls.GetLstRPTGenDetAsync()).Where((RPTGenDet item) => item.Visible))
					{
						int i = item.Order;
						string name = item.ParameterName;
						string text = await FarsiNameAsync(name);
						object[] array = new object[3] { i, name, text };
						((DataGridView)DGridSelection).Rows.Add(array);
					}
					((DataGridView)DGridSelection).Sort((DataGridViewColumn)(object)DGIndex, ListSortDirection.Ascending);
				}
				catch (Exception ex2)
				{
					WebErrorLog.ErrorLogInstance.StartLog(ex2, "FrmRptGen.InitAsync", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGen.cs", 84);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGen.cs", 90);
		}
	}

	private async Task<string> FarsiNameAsync(string ENName)
	{
		try
		{
			if (!(ENName == "ucDate1"))
			{
				if (ENName == "ucPerson1")
				{
					return Utilities.GetTranslate("[پرسنل]");
				}
				return "";
			}
			return Utilities.GetTranslate("[تاریخ]");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FarsiNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGen.cs", 112);
		}
		return "";
	}

	private async Task GetDataAsync()
	{
		try
		{
			_cls.Description = Text.FixString(((Control)Txtdesc).Text);
			_cls.SQLQuery = ((Control)TxtQuery).Text;
			_cls.Title = Text.FixString(((Control)TxtTitle).Text);
			_cls.SetLstRPTGenDet(new List<RPTGenDet>());
			for (int i = 0; i <= ((DataGridView)DGridSelection).Rows.Count - 1; i++)
			{
				RPTGenDet cls = new RPTGenDet(((CacheBaseForm)this).ContextGuid)
				{
					Order = short.Parse(((DataGridView)DGridSelection)[((DataGridViewBand)DGIndex).Index, i].Value.ToString()),
					ParameterName = ((DataGridView)DGridSelection)[((DataGridViewBand)DGNameS).Index, i].Value.ToString(),
					Visible = true
				};
				(await _cls.GetLstRPTGenDetAsync()).Add(cls);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGen.cs", 138);
		}
	}

	private void MoveLeft()
	{
		try
		{
			((Control)DGrid).Focus();
			if (((DataGridView)DGrid).RowCount == 0)
			{
				return;
			}
			if (((DataGridView)DGrid).CurrentRow != null)
			{
				string text = ((DataGridView)DGrid)[((DataGridViewBand)DGName).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString();
				string text2 = ((DataGridView)DGrid)[((DataGridViewBand)DGFANAME).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString();
				for (int i = 0; i <= ((DataGridView)DGridSelection).RowCount - 1; i++)
				{
					if (!(((DataGridView)DGridSelection)[((DataGridViewBand)DGNameS).Index, i].Value.ToString() != text))
					{
						Utility.MessageBoxShowError("[این پارامتر قبلا انتخاب شده است]", ((CacheBaseForm)this).ContextGuid);
						return;
					}
				}
				int num = ((DataGridView)DGridSelection).Rows.Count + 1;
				object[] array = new object[3] { num, text, text2 };
				((DataGridView)DGridSelection).Rows.Add(array);
			}
			((DataGridView)DGridSelection).Sort((DataGridViewColumn)(object)DGIndex, ListSortDirection.Ascending);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "MoveLeft", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGen.cs", 170);
		}
	}

	private void BtnAdd_Click(object sender, EventArgs e)
	{
		MoveLeft();
	}

	private void Delete()
	{
		try
		{
			((Control)DGridSelection).Focus();
			if (((DataGridView)DGridSelection).RowCount != 0)
			{
				if (((DataGridView)DGridSelection).CurrentRow != null)
				{
					((DataGridView)DGridSelection).Rows.RemoveAt(((DataGridViewBand)((DataGridView)DGridSelection).CurrentRow).Index);
				}
				((DataGridView)DGridSelection).Sort((DataGridViewColumn)(object)DGIndex, ListSortDirection.Ascending);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Delete", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGen.cs", 190);
		}
	}

	private void BtnRemove_Click(object sender, EventArgs e)
	{
		Delete();
	}

	private void MoveUp()
	{
		try
		{
			((Control)DGridSelection).Focus();
			if (((DataGridView)DGridSelection).CurrentRow == null || ((DataGridViewBand)((DataGridView)DGridSelection).CurrentRow).Index != 0)
			{
				if (((DataGridView)DGridSelection).CurrentRow != null)
				{
					int num = Convert.ToInt32(((DataGridView)DGridSelection)[((DataGridViewBand)DGIndex).Index, ((DataGridViewBand)((DataGridView)DGridSelection).CurrentRow).Index].Value.ToString());
					((DataGridView)DGridSelection)[((DataGridViewBand)DGIndex).Index, ((DataGridViewBand)((DataGridView)DGridSelection).CurrentRow).Index].Value = num - 1;
					((DataGridView)DGridSelection)[((DataGridViewBand)DGIndex).Index, ((DataGridViewBand)((DataGridView)DGridSelection).CurrentRow).Index - 1].Value = num;
				}
				((DataGridView)DGridSelection).Sort((DataGridViewColumn)(object)DGIndex, ListSortDirection.Ascending);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "MoveUp", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGen.cs", 215);
		}
	}

	private void BtnUP_Click(object sender, EventArgs e)
	{
		MoveUp();
	}

	private void MoveDown()
	{
		try
		{
			((Control)DGridSelection).Focus();
			if (((DataGridView)DGridSelection).CurrentRow == null || ((DataGridViewBand)((DataGridView)DGridSelection).CurrentRow).Index != ((DataGridView)DGridSelection).Rows.Count - 1)
			{
				if (((DataGridView)DGridSelection).CurrentRow != null)
				{
					int num = Convert.ToInt32(((DataGridView)DGridSelection)[((DataGridViewBand)DGIndex).Index, ((DataGridViewBand)((DataGridView)DGridSelection).CurrentRow).Index].Value.ToString());
					((DataGridView)DGridSelection)[((DataGridViewBand)DGIndex).Index, ((DataGridViewBand)((DataGridView)DGridSelection).CurrentRow).Index].Value = num + 1;
					((DataGridView)DGridSelection)[((DataGridViewBand)DGIndex).Index, ((DataGridViewBand)((DataGridView)DGridSelection).CurrentRow).Index + 1].Value = num;
				}
				((DataGridView)DGridSelection).Sort((DataGridViewColumn)(object)DGIndex, ListSortDirection.Ascending);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "MoveDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGen.cs", 240);
		}
	}

	private void BtnDown_Click(object sender, EventArgs e)
	{
		MoveDown();
	}

	private void BtnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnCancel_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGen.cs", 258);
		}
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		if (!((Control)BtnAccept).Enabled || !((Control)BtnAccept).Visible)
		{
			return;
		}
		try
		{
			await GetDataAsync();
			if (await _cls.SaveAsync())
			{
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)1, "[اطلاعات شما با موفقیت ثبت شد]", "", 3);
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
				await RPTGen.SerializeALLAsync(((CacheBaseForm)this).ContextGuid);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGen.cs", 278);
		}
	}

	private void DGridSqlP_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtQuery).Focus();
			if (((DataGridView)DGridSqlP).CurrentRow != null)
			{
				string text = ((DataGridView)DGridSqlP)[((DataGridViewBand)DGNameSQL).Index, ((DataGridViewBand)((DataGridView)DGridSqlP).CurrentRow).Index].Value.ToString();
				text = text.Replace("@", "");
				text = "@" + text;
				TxtQuery.Paste(text);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGridSqlP_DoubleClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGen.cs", 295);
		}
	}

	private void FRMRPTGen_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 27)
			{
				if ((int)keyCode == 116)
				{
					((PopupItemControl)BtnAccept).PerformClick();
				}
			}
			else
			{
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMRPTGen_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGen.cs", 316);
		}
	}

	private void TxtTitle_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 13)
			{
				((Control)TxtQuery).Focus();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtTitle_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGen.cs", 329);
		}
	}

	private void DGrid_DoubleClick(object sender, EventArgs e)
	{
		MoveLeft();
	}

	private async void BtnStim_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)DGridSqlP).Focus();
			FrmFilter frm = new FrmFilter(((CacheBaseForm)this).ContextGuid, _cls, _font);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				_cls.RPT = frm.RPT.RPT;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnStim_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGen.cs", 349);
		}
	}

	private void TxtTitle_Enter(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Control)(TextBox)sender).BackColor = Color.Yellow;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtTitle_Enter", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGen.cs", 361);
		}
	}

	private void TxtTitle_Leave(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Control)(TextBox)sender).BackColor = Color.White;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtTitle_Leave", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGen.cs", 373);
		}
	}

	private void TxtQuery_Enter(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtQuery).BackColor = Color.Yellow;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtQuery_Enter", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGen.cs", 385);
		}
	}

	private void TxtQuery_Leave(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtQuery).BackColor = Color.White;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtQuery_Leave", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGen.cs", 397);
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_initializeToken?.Cancel();
				_initializeToken?.Dispose();
				_initializeToken = null;
				components?.Dispose();
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMRPTGen.cs", 419);
		}
	}

	private void FrmRPTGen_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private void InitializeComponent()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Expected O, but got Unknown
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Expected O, but got Unknown
		//IL_0746: Unknown result type (might be due to invalid IL or missing references)
		//IL_0750: Expected O, but got Unknown
		//IL_0827: Unknown result type (might be due to invalid IL or missing references)
		//IL_0831: Expected O, but got Unknown
		//IL_08cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d9: Expected O, but got Unknown
		//IL_097b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0985: Expected O, but got Unknown
		//IL_0a5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a67: Expected O, but got Unknown
		//IL_0b3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b49: Expected O, but got Unknown
		//IL_0ca1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cab: Expected O, but got Unknown
		//IL_0f2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f38: Expected O, but got Unknown
		//IL_14c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_14d3: Expected O, but got Unknown
		//IL_15ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f7: Expected O, but got Unknown
		//IL_17e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ec: Expected O, but got Unknown
		//IL_19cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d9: Expected O, but got Unknown
		//IL_1a0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a16: Expected O, but got Unknown
		//IL_1a1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a28: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DataGridViewCellStyle val3 = new DataGridViewCellStyle();
		DataGridViewCellStyle val4 = new DataGridViewCellStyle();
		DataGridViewCellStyle val5 = new DataGridViewCellStyle();
		DataGridViewCellStyle val6 = new DataGridViewCellStyle();
		PanelEx1 = new PanelEx();
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		PanelEx3 = new PanelEx();
		BtnDown = new ButtonX();
		LabelX4 = new LabelX();
		LabelX3 = new LabelX();
		BtnUP = new ButtonX();
		BtnRemove = new ButtonX();
		BtnAdd = new ButtonX();
		DGridSelection = new DataGridViewX();
		DGIndex = new DataGridViewTextBoxColumn();
		DGNameS = new DataGridViewTextBoxColumn();
		DGFNameS = new DataGridViewTextBoxColumn();
		DGrid = new DataGridViewX();
		DGName = new DataGridViewTextBoxColumn();
		DGFANAME = new DataGridViewTextBoxColumn();
		controlsBindingSource = new BindingSource(components);
		PanelEx2 = new PanelEx();
		LabelX5 = new LabelX();
		Txtdesc = new TextBox();
		TxtQuery = new TextBox();
		BtnStim = new ButtonX();
		TxtTitle = new TextBox();
		LabelX2 = new LabelX();
		LabelX1 = new LabelX();
		DGridSqlP = new DataGridViewX();
		DGNameSQL = new DataGridViewTextBoxColumn();
		FanameSql = new DataGridViewTextBoxColumn();
		sQLParametersBindingSource = new BindingSource(components);
		((Control)PanelEx1).SuspendLayout();
		((Control)PanelEx3).SuspendLayout();
		((ISupportInitialize)DGridSelection).BeginInit();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)controlsBindingSource).BeginInit();
		((Control)PanelEx2).SuspendLayout();
		((ISupportInitialize)DGridSqlP).BeginInit();
		((ISupportInitialize)sQLParametersBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)PanelEx1).Controls.Add((Control)(object)PanelEx3);
		((Control)PanelEx1).Controls.Add((Control)(object)PanelEx2);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(913, 629);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		PanelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 0;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnCancel.Image = (Image)(object)Resources.stop1;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(12, 583);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(94, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 3;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو]   (Esc)";
		((Control)BtnCancel).Click += BtnCancel_Click;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(112, 583);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(94, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 2;
		((Control)BtnAccept).Text = "[ثبت]  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((Control)PanelEx3).Anchor = (AnchorStyles)15;
		PanelEx3.CanvasColor = SystemColors.Control;
		PanelEx3.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx3).Controls.Add((Control)(object)BtnDown);
		((Control)PanelEx3).Controls.Add((Control)(object)LabelX4);
		((Control)PanelEx3).Controls.Add((Control)(object)LabelX3);
		((Control)PanelEx3).Controls.Add((Control)(object)BtnUP);
		((Control)PanelEx3).Controls.Add((Control)(object)BtnRemove);
		((Control)PanelEx3).Controls.Add((Control)(object)BtnAdd);
		((Control)PanelEx3).Controls.Add((Control)(object)DGridSelection);
		((Control)PanelEx3).Controls.Add((Control)(object)DGrid);
		PanelEx3.DisabledBackColor = Color.Empty;
		((Control)PanelEx3).Location = new Point(12, 344);
		((Control)PanelEx3).Name = "PanelEx3";
		((Control)PanelEx3).Size = new Size(889, 233);
		PanelEx3.Style.Alignment = (StringAlignment)1;
		PanelEx3.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		PanelEx3.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		PanelEx3.Style.Border = (eBorderType)1;
		PanelEx3.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx3.Style.BorderWidth = 3;
		PanelEx3.Style.CornerType = (eCornerType)2;
		PanelEx3.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx3.Style.GradientAngle = 90;
		((Control)PanelEx3).TabIndex = 1;
		((Control)BtnDown).AccessibleRole = (AccessibleRole)43;
		((Control)BtnDown).Anchor = (AnchorStyles)9;
		BtnDown.ColorTable = (eButtonColor)7;
		((Control)BtnDown).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnDown.Image = (Image)(object)Resources.arrow_double_down;
		BtnDown.ImagePosition = (eImagePosition)1;
		((Control)BtnDown).Location = new Point(470, 154);
		((Control)BtnDown).Name = "BtnDown";
		((Control)BtnDown).RightToLeft = (RightToLeft)1;
		((Control)BtnDown).Size = new Size(32, 34);
		((PopupItemControl)BtnDown).Style = (eDotNetBarStyle)9;
		((Control)BtnDown).TabIndex = 5;
		((Control)BtnDown).TabStop = false;
		((Control)BtnDown).Click += BtnDown_Click;
		((Control)LabelX4).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX4).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LabelX4).ForeColor = Color.DarkRed;
		((Control)LabelX4).Location = new Point(300, 7);
		((Control)LabelX4).Name = "LabelX4";
		((Control)LabelX4).Size = new Size(155, 23);
		((Control)LabelX4).TabIndex = 6;
		((Control)LabelX4).Text = "[پارامترهای انتخابی]";
		((Control)LabelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX3).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LabelX3).ForeColor = Color.DarkRed;
		((Control)LabelX3).Location = new Point(714, 7);
		((Control)LabelX3).Name = "LabelX3";
		((Control)LabelX3).Size = new Size(155, 23);
		((Control)LabelX3).TabIndex = 0;
		((Control)LabelX3).Text = "[کلیه پارامتر ها]";
		((Control)BtnUP).AccessibleRole = (AccessibleRole)43;
		((Control)BtnUP).Anchor = (AnchorStyles)9;
		BtnUP.ColorTable = (eButtonColor)7;
		((Control)BtnUP).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnUP.Image = (Image)(object)Resources.arrow_double_up;
		BtnUP.ImagePosition = (eImagePosition)1;
		((Control)BtnUP).Location = new Point(470, 114);
		((Control)BtnUP).Name = "BtnUP";
		((Control)BtnUP).RightToLeft = (RightToLeft)1;
		((Control)BtnUP).Size = new Size(32, 34);
		((PopupItemControl)BtnUP).Style = (eDotNetBarStyle)9;
		((Control)BtnUP).TabIndex = 4;
		((Control)BtnUP).TabStop = false;
		((Control)BtnUP).Click += BtnUP_Click;
		((Control)BtnRemove).AccessibleRole = (AccessibleRole)43;
		((Control)BtnRemove).Anchor = (AnchorStyles)9;
		BtnRemove.ColorTable = (eButtonColor)7;
		((Control)BtnRemove).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnRemove.Image = (Image)(object)Resources.edit_delete;
		BtnRemove.ImagePosition = (eImagePosition)1;
		((Control)BtnRemove).Location = new Point(470, 74);
		((Control)BtnRemove).Name = "BtnRemove";
		((Control)BtnRemove).RightToLeft = (RightToLeft)1;
		((Control)BtnRemove).Size = new Size(32, 34);
		((PopupItemControl)BtnRemove).Style = (eDotNetBarStyle)9;
		((Control)BtnRemove).TabIndex = 3;
		((Control)BtnRemove).TabStop = false;
		((Control)BtnRemove).Click += BtnRemove_Click;
		((Control)BtnAdd).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAdd).Anchor = (AnchorStyles)9;
		BtnAdd.ColorTable = (eButtonColor)7;
		((Control)BtnAdd).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAdd.Image = (Image)(object)Resources.arrow_double_left;
		BtnAdd.ImagePosition = (eImagePosition)1;
		((Control)BtnAdd).Location = new Point(470, 34);
		((Control)BtnAdd).Name = "BtnAdd";
		((Control)BtnAdd).RightToLeft = (RightToLeft)1;
		((Control)BtnAdd).Size = new Size(32, 34);
		((PopupItemControl)BtnAdd).Style = (eDotNetBarStyle)9;
		((Control)BtnAdd).TabIndex = 2;
		((Control)BtnAdd).TabStop = false;
		((Control)BtnAdd).Click += BtnAdd_Click;
		((DataGridView)DGridSelection).AllowUserToAddRows = false;
		((DataGridView)DGridSelection).AllowUserToDeleteRows = false;
		((DataGridView)DGridSelection).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGridSelection).AlternatingRowsDefaultCellStyle = val;
		((Control)DGridSelection).Anchor = (AnchorStyles)15;
		((DataGridView)DGridSelection).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGridSelection).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[3]
		{
			(DataGridViewColumn)DGIndex,
			(DataGridViewColumn)DGNameS,
			(DataGridViewColumn)DGFNameS
		});
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGridSelection).DefaultCellStyle = val2;
		((DataGridView)DGridSelection).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGridSelection).Location = new Point(24, 34);
		((DataGridView)DGridSelection).MultiSelect = false;
		((Control)DGridSelection).Name = "DGridSelection";
		((DataGridView)DGridSelection).ReadOnly = true;
		((DataGridView)DGridSelection).RowHeadersVisible = false;
		((DataGridView)DGridSelection).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGridSelection).Size = new Size(431, 187);
		((Control)DGridSelection).TabIndex = 7;
		((DataGridViewColumn)DGIndex).HeaderText = "Index";
		((DataGridViewColumn)DGIndex).Name = "DGIndex";
		((DataGridViewBand)DGIndex).ReadOnly = true;
		DGIndex.SortMode = (DataGridViewColumnSortMode)2;
		((DataGridViewBand)DGIndex).Visible = false;
		((DataGridViewColumn)DGNameS).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGNameS).HeaderText = "[پارامترها]";
		((DataGridViewColumn)DGNameS).MinimumWidth = 150;
		((DataGridViewColumn)DGNameS).Name = "DGNameS";
		((DataGridViewBand)DGNameS).ReadOnly = true;
		DGNameS.SortMode = (DataGridViewColumnSortMode)2;
		((DataGridViewBand)DGNameS).Visible = false;
		((DataGridViewColumn)DGFNameS).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DGFNameS).HeaderText = "[پارامترهای انتخابی]";
		((DataGridViewColumn)DGFNameS).Name = "DGFNameS";
		((DataGridViewBand)DGFNameS).ReadOnly = true;
		DGFNameS.SortMode = (DataGridViewColumnSortMode)2;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val3.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val3;
		((Control)DGrid).Anchor = (AnchorStyles)11;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[2]
		{
			(DataGridViewColumn)DGName,
			(DataGridViewColumn)DGFANAME
		});
		((DataGridView)DGrid).DataSource = controlsBindingSource;
		val4.Alignment = (DataGridViewContentAlignment)16;
		val4.BackColor = SystemColors.Window;
		val4.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		val4.ForeColor = SystemColors.ControlText;
		val4.SelectionBackColor = SystemColors.Highlight;
		val4.SelectionForeColor = SystemColors.ControlText;
		val4.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val4;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(517, 34);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(352, 187);
		((Control)DGrid).TabIndex = 1;
		((Control)DGrid).DoubleClick += DGrid_DoubleClick;
		((DataGridViewColumn)DGName).DataPropertyName = "Name";
		((DataGridViewColumn)DGName).HeaderText = "[نام]";
		((DataGridViewColumn)DGName).Name = "DGName";
		((DataGridViewBand)DGName).ReadOnly = true;
		((DataGridViewBand)DGName).Visible = false;
		((DataGridViewColumn)DGFANAME).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DGFANAME).DataPropertyName = "FaName";
		((DataGridViewColumn)DGFANAME).HeaderText = "[کلیه پارامتر ها]";
		((DataGridViewColumn)DGFANAME).Name = "DGFANAME";
		((DataGridViewBand)DGFANAME).ReadOnly = true;
		controlsBindingSource.DataSource = typeof(Controls);
		((Control)PanelEx2).Anchor = (AnchorStyles)13;
		PanelEx2.CanvasColor = SystemColors.Control;
		PanelEx2.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx2).Controls.Add((Control)(object)LabelX5);
		((Control)PanelEx2).Controls.Add((Control)(object)Txtdesc);
		((Control)PanelEx2).Controls.Add((Control)(object)TxtQuery);
		((Control)PanelEx2).Controls.Add((Control)(object)BtnStim);
		((Control)PanelEx2).Controls.Add((Control)(object)TxtTitle);
		((Control)PanelEx2).Controls.Add((Control)(object)LabelX2);
		((Control)PanelEx2).Controls.Add((Control)(object)LabelX1);
		((Control)PanelEx2).Controls.Add((Control)(object)DGridSqlP);
		PanelEx2.DisabledBackColor = Color.Empty;
		((Control)PanelEx2).Location = new Point(12, 12);
		((Control)PanelEx2).Name = "PanelEx2";
		((Control)PanelEx2).Size = new Size(889, 326);
		PanelEx2.Style.Alignment = (StringAlignment)1;
		PanelEx2.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		PanelEx2.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		PanelEx2.Style.Border = (eBorderType)1;
		PanelEx2.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx2.Style.BorderWidth = 3;
		PanelEx2.Style.CornerType = (eCornerType)2;
		PanelEx2.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx2.Style.GradientAngle = 90;
		((Control)PanelEx2).TabIndex = 0;
		((Control)LabelX5).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX5).Location = new Point(821, 250);
		((Control)LabelX5).Name = "LabelX5";
		((Control)LabelX5).Size = new Size(59, 23);
		((Control)LabelX5).TabIndex = 4;
		((Control)LabelX5).Text = "[شرح] :";
		LabelX5.TextAlignment = (StringAlignment)2;
		((Control)Txtdesc).Anchor = (AnchorStyles)14;
		((Control)Txtdesc).Location = new Point(24, 252);
		((TextBoxBase)Txtdesc).Multiline = true;
		((Control)Txtdesc).Name = "Txtdesc";
		Txtdesc.ScrollBars = (ScrollBars)3;
		((Control)Txtdesc).Size = new Size(791, 59);
		((Control)Txtdesc).TabIndex = 5;
		((Control)Txtdesc).TabStop = false;
		((Control)Txtdesc).Enter += TxtTitle_Enter;
		((Control)Txtdesc).Leave += TxtTitle_Leave;
		((Control)TxtQuery).Anchor = (AnchorStyles)11;
		((Control)TxtQuery).Location = new Point(470, 46);
		((TextBoxBase)TxtQuery).Multiline = true;
		((Control)TxtQuery).Name = "TxtQuery";
		((Control)TxtQuery).RightToLeft = (RightToLeft)0;
		TxtQuery.ScrollBars = (ScrollBars)3;
		((Control)TxtQuery).Size = new Size(345, 200);
		((Control)TxtQuery).TabIndex = 3;
		((Control)TxtQuery).TabStop = false;
		((Control)TxtQuery).Enter += TxtQuery_Enter;
		((Control)TxtQuery).Leave += TxtQuery_Leave;
		((Control)BtnStim).AccessibleRole = (AccessibleRole)43;
		((Control)BtnStim).Anchor = (AnchorStyles)15;
		BtnStim.ColorTable = (eButtonColor)6;
		((Control)BtnStim).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnStim.ImagePosition = (eImagePosition)1;
		((Control)BtnStim).Location = new Point(24, 19);
		((Control)BtnStim).Name = "BtnStim";
		((Control)BtnStim).RightToLeft = (RightToLeft)1;
		((Control)BtnStim).Size = new Size(431, 21);
		((PopupItemControl)BtnStim).Style = (eDotNetBarStyle)9;
		((Control)BtnStim).TabIndex = 6;
		((Control)BtnStim).TabStop = false;
		((Control)BtnStim).Text = "[طراحی گزارش]";
		((Control)BtnStim).Click += BtnStim_Click;
		((Control)TxtTitle).Anchor = (AnchorStyles)9;
		((Control)TxtTitle).Location = new Point(470, 19);
		((Control)TxtTitle).Name = "TxtTitle";
		((Control)TxtTitle).Size = new Size(345, 21);
		((Control)TxtTitle).TabIndex = 1;
		((Control)TxtTitle).Enter += TxtTitle_Enter;
		((Control)TxtTitle).KeyDown += new KeyEventHandler(TxtTitle_KeyDown);
		((Control)TxtTitle).Leave += TxtTitle_Leave;
		((Control)LabelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX2).Location = new Point(821, 46);
		((Control)LabelX2).Name = "LabelX2";
		((Control)LabelX2).Size = new Size(52, 23);
		((Control)LabelX2).TabIndex = 2;
		((Control)LabelX2).Text = "[کوئری] :";
		LabelX2.TextAlignment = (StringAlignment)2;
		((Control)LabelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX1).Location = new Point(821, 17);
		((Control)LabelX1).Name = "LabelX1";
		((Control)LabelX1).Size = new Size(52, 23);
		((Control)LabelX1).TabIndex = 0;
		((Control)LabelX1).Text = "[عنوان] :";
		LabelX1.TextAlignment = (StringAlignment)2;
		((DataGridView)DGridSqlP).AllowUserToAddRows = false;
		((DataGridView)DGridSqlP).AllowUserToDeleteRows = false;
		((DataGridView)DGridSqlP).AllowUserToResizeRows = false;
		val5.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGridSqlP).AlternatingRowsDefaultCellStyle = val5;
		((Control)DGridSqlP).Anchor = (AnchorStyles)15;
		((DataGridView)DGridSqlP).AutoGenerateColumns = false;
		((DataGridView)DGridSqlP).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGridSqlP).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[2]
		{
			(DataGridViewColumn)DGNameSQL,
			(DataGridViewColumn)FanameSql
		});
		((DataGridView)DGridSqlP).DataSource = sQLParametersBindingSource;
		val6.Alignment = (DataGridViewContentAlignment)16;
		val6.BackColor = SystemColors.Window;
		val6.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		val6.ForeColor = SystemColors.ControlText;
		val6.SelectionBackColor = SystemColors.Highlight;
		val6.SelectionForeColor = SystemColors.ControlText;
		val6.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGridSqlP).DefaultCellStyle = val6;
		((DataGridView)DGridSqlP).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGridSqlP).Location = new Point(24, 46);
		((DataGridView)DGridSqlP).MultiSelect = false;
		((Control)DGridSqlP).Name = "DGridSqlP";
		((DataGridView)DGridSqlP).ReadOnly = true;
		((DataGridView)DGridSqlP).RowHeadersVisible = false;
		((DataGridView)DGridSqlP).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGridSqlP).Size = new Size(431, 200);
		((Control)DGridSqlP).TabIndex = 7;
		((Control)DGridSqlP).DoubleClick += DGridSqlP_DoubleClick;
		((DataGridViewColumn)DGNameSQL).DataPropertyName = "Name";
		((DataGridViewColumn)DGNameSQL).HeaderText = "Name";
		((DataGridViewColumn)DGNameSQL).Name = "DGNameSQL";
		((DataGridViewBand)DGNameSQL).ReadOnly = true;
		((DataGridViewBand)DGNameSQL).Visible = false;
		((DataGridViewColumn)FanameSql).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)FanameSql).DataPropertyName = "FaName";
		((DataGridViewColumn)FanameSql).HeaderText = "[متغیرهای SQL]";
		((DataGridViewColumn)FanameSql).Name = "FanameSql";
		((DataGridViewBand)FanameSql).ReadOnly = true;
		sQLParametersBindingSource.DataSource = typeof(SQLParameters);
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(913, 629);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Form)this).KeyPreview = true;
		((Control)this).Name = "FrmRPTGen";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[فرم طراحی گزارش]";
		((Form)this).FormClosed += new FormClosedEventHandler(FrmRPTGen_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(FRMRPTGen_KeyDown);
		((Control)PanelEx1).ResumeLayout(false);
		((Control)PanelEx3).ResumeLayout(false);
		((ISupportInitialize)DGridSelection).EndInit();
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)controlsBindingSource).EndInit();
		((Control)PanelEx2).ResumeLayout(false);
		((Control)PanelEx2).PerformLayout();
		((ISupportInitialize)DGridSqlP).EndInit();
		((ISupportInitialize)sQLParametersBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}

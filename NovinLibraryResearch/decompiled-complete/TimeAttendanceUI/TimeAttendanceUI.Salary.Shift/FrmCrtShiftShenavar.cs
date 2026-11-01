using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using FormCustom.CurrencyTextBox;
using FormCustom.Forms;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using Serilog;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Salary.Shift;

public class FrmCrtShiftShenavar : BaseForm
{
	public ShiftDet ClsShiftDet;

	private string frm_Text = "شیفت شناور";

	private Font _font;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private bool _isDisposed;

	private IContainer components;

	public PanelEx PanelEx1;

	private GroupPanel groupPanel7;

	internal DateBoxFa TxtFinishM;

	internal DateBoxFa TXTStartDate;

	private LabelX labelX1;

	private TextBoxX TxtDesc;

	private GroupPanel groupPanelTaradod;

	private LabelX LblValStartTime;

	private LabelX LblStartTime;

	private GroupPanel groupPanelSelTaradod;

	private LabelX labelX39;

	internal ButtonX BtnMorePlace;

	private ComboBoxEx TxtSelectTaradod;

	private GroupPanel groupPanel1;

	private CheckBoxX ChbSat;

	private CheckBoxX ChbMon;

	private CheckBoxX ChbSun;

	private CheckBoxX ChbThr;

	private CheckBoxX ChbTue;

	private CheckBoxX ChbWed;

	private LabelX LblDescPeakOverTime;

	private LabelX LblPeakOvertime;

	private LabelX labelX4;

	private LabelX LblNextDay;

	private CheckBoxX ChbSTTatili;

	private LabelX labelX3;

	private LabelX labelX2;

	private LabelX labelX5;

	private RadioButton RbMustWork;

	private RadioButton RbOptionalNotKarkerd;

	private RadioButton RbOptionalKarkerd;

	internal ButtonX BtnHelp;

	public bool WorkOnHaliday => ChbSTTatili.Checked;

	public bool IsDisposed
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

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_loadDataToken?.Cancel();
				_loadDataToken?.Dispose();
				_loadDataToken = null;
				_searchToken?.Cancel();
				_searchToken?.Dispose();
				_searchToken = null;
				components?.Dispose();
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public FrmCrtShiftShenavar(string contextGuid, Font font)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_font = font;
			InitializeComponent();
			ClsShiftDet = new ShiftDet(((CacheBaseForm)this).ContextGuid);
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftShenavar.cs");
			EnableEvents(enable: true);
			((Control)TxtDesc).Text = "شیفت شناور";
			frm_Text = "[شیفت شناور]";
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftShenavar.cs", 63);
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			await CmbShiftCodingTypes_load(token);
			await InitPanelTaradodAsync(token);
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	private async Task InitPanelTaradodAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			await ClsShiftDet.SetShiftCodingIdAsync(ClsShiftDet.ShiftCodingId);
			string text_TaradodShenavar = Utilities.GetTranslate("[تردد شناور]");
			string text_hozorTime = Utilities.GetTranslate("[مدت زمان حضور] : ");
			ShiftCodeing coding = ClsShiftDet.ShiftCodeing;
			if (coding != null)
			{
				InitPeakLables(coding);
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
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						((Control)groupPanelTaradod).Text = text_TaradodShenavar;
						((Control)LblStartTime).Text = text_hozorTime;
						((Control)TxtDesc).Text = ClsShiftDet.Description;
						ChbSat.Checked = ClsShiftDet.Sat;
						ChbSun.Checked = ClsShiftDet.Sun;
						ChbMon.Checked = ClsShiftDet.Mon;
						ChbTue.Checked = ClsShiftDet.Tue;
						ChbWed.Checked = ClsShiftDet.Wed;
						ChbThr.Checked = ClsShiftDet.Tur;
						((Control)TXTStartDate).Text = Calendar.MiladiToCalture(ClsShiftDet.DateStartM);
						((Control)TxtFinishM).Text = Calendar.MiladiToCalture(ClsShiftDet.DateFinishM);
						if (coding != null)
						{
							((Control)LblValStartTime).Text = $"{coding.H2:00} : {coding.M2:00}";
						}
					}
				}
				catch (OperationCanceledException)
				{
				}
				catch (Exception ex4)
				{
					Log.Error(ex4, "");
				}
			});
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "InitPanelTaradodAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftShenavar.cs", 128);
		}
	}

	private async Task CmbShiftCodingTypes_load(CancellationToken token = default(CancellationToken))
	{
		try
		{
			List<ShiftCodeing> data = await ShiftCodeing.GetAllAsync(((CacheBaseForm)this).ContextGuid, (TypeShiftEnumType)2, token);
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
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						if (!UserAttendance.SelectedUser.AccessLevelMerg.Shift.ShiftAdvance)
						{
							((Control)groupPanelSelTaradod).Visible = false;
						}
						((ListControl)TxtSelectTaradod).DisplayMember = "NameShiftCodeing";
						((ListControl)TxtSelectTaradod).ValueMember = "Id";
						((ComboBox)TxtSelectTaradod).DataSource = Utilities.ToSortableBindingList<ShiftCodeing>((IEnumerable<ShiftCodeing>)data, default(CancellationToken));
					}
				}
				catch (OperationCanceledException)
				{
				}
				catch (Exception ex4)
				{
					Log.Error(ex4, "");
				}
			});
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "CmbShiftCodingTypes_load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftShenavar.cs", 157);
		}
	}

	public ShiftDet GetData()
	{
		try
		{
			ClsShiftDet.Sat = ChbSat.Checked;
			ClsShiftDet.Sun = ChbSun.Checked;
			ClsShiftDet.Mon = ChbMon.Checked;
			ClsShiftDet.Tue = ChbTue.Checked;
			ClsShiftDet.Wed = ChbWed.Checked;
			ClsShiftDet.Tur = ChbThr.Checked;
			if (RbOptionalKarkerd.Checked || RbMustWork.Checked)
			{
				ClsShiftDet.Fri = true;
			}
			ClsShiftDet.Description = Text.FixString(((Control)TxtDesc).Text);
			ClsShiftDet.DateStartM = TXTStartDate.MiladiDate.Date;
			ClsShiftDet.DateFinishM = TxtFinishM.MiladiDate.Date.AddDays(1.0).AddSeconds(-1.0);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftShenavar.cs", 176);
		}
		return ClsShiftDet;
	}

	private void FRMCrtShiftShenavar_Load(object sender, EventArgs e)
	{
		try
		{
			frm_Text = Utilities.GetTranslate(frm_Text);
			((Control)this).Text = frm_Text;
			((Control)TXTStartDate).Focus();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMCrtShiftShenavar_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftShenavar.cs", 188);
		}
	}

	private void FRMCrtShiftShenavar_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 13)
			{
				if ((int)keyCode != 27)
				{
					if ((int)keyCode == 123)
					{
					}
				}
				else
				{
					e.Handled = true;
					((Form)this).DialogResult = (DialogResult)2;
					((Form)this).Close();
				}
			}
			else
			{
				e.Handled = true;
				((Control)this).SelectNextControl(((ContainerControl)this).ActiveControl, true, true, true, true);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMCrtShiftShenavar_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftShenavar.cs", 209);
		}
	}

	private void TxtDesc_Enter(object sender, EventArgs e)
	{
		try
		{
			((TextBoxBase)TxtDesc).SelectAll();
			((Control)TxtDesc).BackColor = Color.Yellow;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtDesc_Enter", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftShenavar.cs", 218);
		}
	}

	private void TxtDesc_Leave(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtDesc).BackColor = Color.White;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtDesc_Leave", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftShenavar.cs", 226);
		}
	}

	private async Task FillPanelTaradodAsync(short pid, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (ClsShiftDet.ShiftCodingId == pid)
			{
				return;
			}
			await ClsShiftDet.SetShiftCodingIdAsync(pid);
			string text_shiftShenavar = Utilities.GetTranslate("[شیفت شناور]");
			string text_ShenavarStr3 = Utilities.GetTranslate("[مدت حضور] :");
			InitPeakLables(ClsShiftDet.ShiftCodeing);
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
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						((Control)LblStartTime).Text = text_ShenavarStr3;
						((Control)groupPanelTaradod).Text = text_shiftShenavar;
						((Control)LblValStartTime).Text = ClsShiftDet.ShiftCodeing.Khoroj.ToString("HH:mm");
					}
				}
				catch (OperationCanceledException)
				{
				}
				catch (Exception ex3)
				{
					Log.Error(ex3, "");
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillPanelTaradodAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftShenavar.cs", 254);
		}
	}

	private async void BtnMorePlace_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			FrmShiftCodeingList frm = new FrmShiftCodeingList(((CacheBaseForm)this).ContextGuid, _font, isSelect: true, (TypeShiftEnumType)2);
			if ((int)((Form)frm).ShowDialog() == 1)
			{
				await CmbShiftCodingTypes_load();
				((ListControl)TxtSelectTaradod).SelectedValue = frm.SelectedRow.Id;
				await FillPanelTaradodAsync(frm.SelectedRow.Id);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnMorePlace_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftShenavar.cs", 267);
		}
	}

	private async void TxtSelectTaradod_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (((ListControl)TxtSelectTaradod).SelectedValue != null)
			{
				await FillPanelTaradodAsync(Convert.ToInt16(((ListControl)TxtSelectTaradod).SelectedValue.ToString()));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtSelectTaradod_SelectedIndexChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftShenavar.cs", 276);
		}
	}

	private void InitPeakLables(ShiftCodeing pShiftCodeing)
	{
		try
		{
			string translate = Utilities.GetTranslate("[انتقال از روز قبل دارد و حداکثر زمان خروج] " + Time.IntToTime(pShiftCodeing.MaxTimeCanExit) + " [میباشد و از این ساعت به بعد بعنوان ورود در نظر گرفته میشود.]");
			((Control)LblPeakOvertime).Text = Time.IntToTime((int)pShiftCodeing.PeakOvertime);
			((Control)LblDescPeakOverTime).Visible = pShiftCodeing.PeakOvertime > 0;
			((Control)LblNextDay).Visible = false;
			if (pShiftCodeing.NextDay)
			{
				((Control)LblNextDay).Visible = true;
				((Control)LblNextDay).Text = translate;
			}
			else
			{
				((Control)LblNextDay).Visible = false;
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "InitPeakLables", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftShenavar.cs", 298);
		}
	}

	private void BtnHelp_Click(object sender, EventArgs e)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string text = "[1- حضور اختیاری در جمعه ها، حتی در صورت عدم حضور جزء کارکرد میباشد و در صورت حضور جمعه کاری نیز لحاظ میشود]\r\n";
			text += "[2- حضور اختیاری در جمعه ها، فقط در صورت حضور جزء کارکرد میباشد و در صورت حضور جمعه کاری لحاظ میشود]\r\n";
			text += "[3- حضور در روز جمعه الزامی است و مشابه روزهای عادی محاسبه می شود و در صورت عدم حضور غیبت محسوب میشود]\r\n";
			((Form)new FrmHelpNextDay(((CacheBaseForm)this).ContextGuid, Utilities.GetTranslate(text), Utilities.GetTranslate("[توضیحات روز جمعه]"))).ShowDialog();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnHelp_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftShenavar.cs", 315);
		}
	}

	private void RbOptionalKarkerd_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			EnableEvents(enable: false);
			if (RbOptionalKarkerd.Checked)
			{
				ChbSTTatili.Checked = false;
			}
			EnableEvents(enable: true);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RbOptionalKarkerd_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftShenavar.cs", 326);
		}
	}

	private void ChbSTTatili_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			EnableEvents(enable: false);
			if (ChbSTTatili.Checked && RbOptionalKarkerd.Checked)
			{
				RbMustWork.Checked = true;
			}
			EnableEvents(enable: true);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ChbSTTatili_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftShenavar.cs", 341);
		}
	}

	private void RbMustWork_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			EnableEvents(enable: false);
			if (RbMustWork.Checked)
			{
				ChbSTTatili.Checked = true;
			}
			EnableEvents(enable: true);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RbMustWork_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftShenavar.cs", 353);
		}
	}

	private void EnableEvents(bool enable)
	{
		try
		{
			if (enable)
			{
				RbOptionalKarkerd.CheckedChanged += RbOptionalKarkerd_CheckedChanged;
				ChbSTTatili.CheckedChanged += ChbSTTatili_CheckedChanged;
				RbMustWork.CheckedChanged += RbMustWork_CheckedChanged;
			}
			else
			{
				RbOptionalKarkerd.CheckedChanged -= RbOptionalKarkerd_CheckedChanged;
				ChbSTTatili.CheckedChanged -= ChbSTTatili_CheckedChanged;
				RbMustWork.CheckedChanged -= RbMustWork_CheckedChanged;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "EnableEvents", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftShenavar.cs", 373);
		}
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
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
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Expected O, but got Unknown
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0faa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb4: Expected O, but got Unknown
		//IL_0fe0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fea: Expected O, but got Unknown
		//IL_1131: Unknown result type (might be due to invalid IL or missing references)
		//IL_113b: Expected O, but got Unknown
		//IL_1167: Unknown result type (might be due to invalid IL or missing references)
		//IL_1171: Expected O, but got Unknown
		//IL_1e23: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e2d: Expected O, but got Unknown
		PanelEx1 = new PanelEx();
		groupPanel7 = new GroupPanel();
		labelX1 = new LabelX();
		TxtDesc = new TextBoxX();
		groupPanelTaradod = new GroupPanel();
		LblNextDay = new LabelX();
		LblDescPeakOverTime = new LabelX();
		LblPeakOvertime = new LabelX();
		labelX4 = new LabelX();
		LblValStartTime = new LabelX();
		LblStartTime = new LabelX();
		groupPanelSelTaradod = new GroupPanel();
		labelX3 = new LabelX();
		labelX2 = new LabelX();
		TXTStartDate = new DateBoxFa();
		TxtFinishM = new DateBoxFa();
		labelX39 = new LabelX();
		BtnMorePlace = new ButtonX();
		TxtSelectTaradod = new ComboBoxEx();
		groupPanel1 = new GroupPanel();
		labelX5 = new LabelX();
		RbMustWork = new RadioButton();
		RbOptionalNotKarkerd = new RadioButton();
		RbOptionalKarkerd = new RadioButton();
		BtnHelp = new ButtonX();
		ChbSTTatili = new CheckBoxX();
		ChbSat = new CheckBoxX();
		ChbMon = new CheckBoxX();
		ChbSun = new CheckBoxX();
		ChbThr = new CheckBoxX();
		ChbTue = new CheckBoxX();
		ChbWed = new CheckBoxX();
		((Control)PanelEx1).SuspendLayout();
		((Control)groupPanel7).SuspendLayout();
		((Control)groupPanelTaradod).SuspendLayout();
		((Control)groupPanelSelTaradod).SuspendLayout();
		((Control)groupPanel1).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)groupPanel7);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Font = new Font("B Homa", 9.75f);
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Margin = new Padding(3, 4, 3, 4);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).RightToLeft = (RightToLeft)1;
		((Control)PanelEx1).Size = new Size(499, 500);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 0;
		((Control)groupPanel7).Anchor = (AnchorStyles)15;
		((Control)groupPanel7).BackColor = Color.Transparent;
		((PanelControl)groupPanel7).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel7).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel7).Controls.Add((Control)(object)labelX1);
		((Control)groupPanel7).Controls.Add((Control)(object)TxtDesc);
		((Control)groupPanel7).Controls.Add((Control)(object)groupPanelTaradod);
		((Control)groupPanel7).Controls.Add((Control)(object)groupPanelSelTaradod);
		((Control)groupPanel7).Controls.Add((Control)(object)groupPanel1);
		groupPanel7.DisabledBackColor = Color.Empty;
		((Control)groupPanel7).Location = new Point(12, 12);
		((Control)groupPanel7).Name = "groupPanel7";
		((Control)groupPanel7).Size = new Size(475, 476);
		((PanelControl)groupPanel7).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel7).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel7).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel7).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel7).Style.BorderBottomWidth = 3;
		((PanelControl)groupPanel7).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel7).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel7).Style.BorderLeftWidth = 3;
		((PanelControl)groupPanel7).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel7).Style.BorderRightWidth = 3;
		((PanelControl)groupPanel7).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel7).Style.BorderTopWidth = 3;
		((PanelControl)groupPanel7).Style.CornerDiameter = 4;
		((PanelControl)groupPanel7).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel7).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel7).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel7).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel7).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel7).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel7).TabIndex = 1;
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(391, 399);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(62, 24);
		((Control)labelX1).TabIndex = 5;
		((Control)labelX1).Text = "[شرح] :";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)TxtDesc).Anchor = (AnchorStyles)15;
		TxtDesc.Border.Class = "TextBoxBorder";
		TxtDesc.Border.CornerType = (eCornerType)1;
		((Control)TxtDesc).Location = new Point(20, 399);
		((TextBoxBase)TxtDesc).Multiline = true;
		((Control)TxtDesc).Name = "TxtDesc";
		((Control)TxtDesc).Size = new Size(367, 68);
		((Control)TxtDesc).TabIndex = 6;
		((Control)TxtDesc).Enter += TxtDesc_Enter;
		((Control)TxtDesc).Leave += TxtDesc_Leave;
		((PanelControl)groupPanelTaradod).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanelTaradod).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanelTaradod).Controls.Add((Control)(object)LblNextDay);
		((Control)groupPanelTaradod).Controls.Add((Control)(object)LblDescPeakOverTime);
		((Control)groupPanelTaradod).Controls.Add((Control)(object)LblPeakOvertime);
		((Control)groupPanelTaradod).Controls.Add((Control)(object)labelX4);
		((Control)groupPanelTaradod).Controls.Add((Control)(object)LblValStartTime);
		((Control)groupPanelTaradod).Controls.Add((Control)(object)LblStartTime);
		groupPanelTaradod.DisabledBackColor = Color.Empty;
		((Control)groupPanelTaradod).Location = new Point(20, 91);
		((Control)groupPanelTaradod).Name = "groupPanelTaradod";
		((Control)groupPanelTaradod).Size = new Size(435, 106);
		((PanelControl)groupPanelTaradod).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanelTaradod).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanelTaradod).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanelTaradod).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanelTaradod).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanelTaradod).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanelTaradod).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanelTaradod).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanelTaradod).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanelTaradod).Style.BorderRightWidth = 1;
		((PanelControl)groupPanelTaradod).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanelTaradod).Style.BorderTopWidth = 1;
		((PanelControl)groupPanelTaradod).Style.CornerDiameter = 4;
		((PanelControl)groupPanelTaradod).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanelTaradod).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanelTaradod).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanelTaradod).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanelTaradod).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanelTaradod).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanelTaradod).TabIndex = 3;
		((Control)groupPanelTaradod).Text = "[مشخصات تردد شناور]";
		((Control)LblNextDay).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblNextDay).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblNextDay).ForeColor = Color.DarkRed;
		((Control)LblNextDay).Location = new Point(14, 33);
		((Control)LblNextDay).Name = "LblNextDay";
		((Control)LblNextDay).Size = new Size(410, 26);
		((Control)LblNextDay).TabIndex = 5;
		((Control)LblNextDay).Text = "[انتقال از روز قبل دارد]";
		((Control)LblDescPeakOverTime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblDescPeakOverTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblDescPeakOverTime).ForeColor = Color.DarkRed;
		((Control)LblDescPeakOverTime).Location = new Point(3, 3);
		((Control)LblDescPeakOverTime).Name = "LblDescPeakOverTime";
		((Control)LblDescPeakOverTime).Size = new Size(213, 24);
		((Control)LblDescPeakOverTime).TabIndex = 4;
		((Control)LblDescPeakOverTime).Text = "[شخص حداکثر میتواند این مقدار ساعت اضافه کاری کند]";
		((Control)LblPeakOvertime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblPeakOvertime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblPeakOvertime).ForeColor = Color.DarkRed;
		((Control)LblPeakOvertime).Location = new Point(225, 3);
		((Control)LblPeakOvertime).Name = "LblPeakOvertime";
		((Control)LblPeakOvertime).Size = new Size(31, 24);
		((Control)LblPeakOvertime).TabIndex = 3;
		((Control)LblPeakOvertime).Text = "-----";
		((Control)labelX4).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Location = new Point(261, 3);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(74, 24);
		((Control)labelX4).TabIndex = 2;
		((Control)labelX4).Text = "[سقف اضافه کاری] :";
		labelX4.TextAlignment = (StringAlignment)2;
		((Control)LblValStartTime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblValStartTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblValStartTime).ForeColor = Color.DarkRed;
		((Control)LblValStartTime).Location = new Point(334, 3);
		((Control)LblValStartTime).Name = "LblValStartTime";
		((Control)LblValStartTime).Size = new Size(30, 24);
		((Control)LblValStartTime).TabIndex = 1;
		((Control)LblValStartTime).Text = "-----";
		((Control)LblStartTime).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblStartTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblStartTime).Location = new Point(369, 3);
		((Control)LblStartTime).Name = "LblStartTime";
		((Control)LblStartTime).Size = new Size(54, 24);
		((Control)LblStartTime).TabIndex = 0;
		((Control)LblStartTime).Text = "[مقدار کارکرد] :";
		LblStartTime.TextAlignment = (StringAlignment)2;
		((PanelControl)groupPanelSelTaradod).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanelSelTaradod).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanelSelTaradod).Controls.Add((Control)(object)labelX3);
		((Control)groupPanelSelTaradod).Controls.Add((Control)(object)labelX2);
		((Control)groupPanelSelTaradod).Controls.Add((Control)(object)TXTStartDate);
		((Control)groupPanelSelTaradod).Controls.Add((Control)(object)TxtFinishM);
		((Control)groupPanelSelTaradod).Controls.Add((Control)(object)labelX39);
		((Control)groupPanelSelTaradod).Controls.Add((Control)(object)BtnMorePlace);
		((Control)groupPanelSelTaradod).Controls.Add((Control)(object)TxtSelectTaradod);
		groupPanelSelTaradod.DisabledBackColor = Color.Empty;
		((Control)groupPanelSelTaradod).Location = new Point(20, 3);
		((Control)groupPanelSelTaradod).Name = "groupPanelSelTaradod";
		((Control)groupPanelSelTaradod).Size = new Size(435, 82);
		((PanelControl)groupPanelSelTaradod).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanelSelTaradod).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanelSelTaradod).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanelSelTaradod).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanelSelTaradod).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanelSelTaradod).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanelSelTaradod).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanelSelTaradod).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanelSelTaradod).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanelSelTaradod).Style.BorderRightWidth = 1;
		((PanelControl)groupPanelSelTaradod).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanelSelTaradod).Style.BorderTopWidth = 1;
		((PanelControl)groupPanelSelTaradod).Style.CornerDiameter = 4;
		((PanelControl)groupPanelSelTaradod).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanelSelTaradod).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanelSelTaradod).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanelSelTaradod).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanelSelTaradod).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanelSelTaradod).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanelSelTaradod).TabIndex = 2;
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(104, 3);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(62, 24);
		((Control)labelX3).TabIndex = 6;
		((Control)labelX3).Text = "[تاریخ پایان]:";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(373, 3);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(51, 24);
		((Control)labelX2).TabIndex = 6;
		((Control)labelX2).Text = "[تاریخ شروع] :";
		labelX2.TextAlignment = (StringAlignment)2;
		TXTStartDate.AllowNull = false;
		TXTStartDate.BackcolorTooltip = Color.Red;
		((TextBoxX)TXTStartDate).Border.CornerType = (eCornerType)1;
		TXTStartDate.EnterToTab = true;
		TXTStartDate.EscToClose = true;
		TXTStartDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		TXTStartDate.FocusBackColor = Color.Yellow;
		TXTStartDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TXTStartDate.FocusForeColor = Color.Blue;
		TXTStartDate.FocusTextSelect = true;
		((Control)TXTStartDate).Font = new Font("Times New Roman", 12f);
		((Control)TXTStartDate).Location = new Point(280, 3);
		TXTStartDate.MessageEmptyShowDialog = false;
		TXTStartDate.MessegeEmpty = "";
		TXTStartDate.MessegeEmptyInFormRight = true;
		TXTStartDate.MessegeEmptyShowInForm = false;
		TXTStartDate.MiladiDate = new DateTime(2021, 12, 11, 15, 30, 3, 515);
		((Control)TXTStartDate).Name = "TXTStartDate";
		((Control)TXTStartDate).RightToLeft = (RightToLeft)0;
		((Control)TXTStartDate).Size = new Size(87, 20);
		((Control)TXTStartDate).TabIndex = 0;
		((Control)TXTStartDate).Text = "11/12/2021";
		((TextBox)TXTStartDate).TextAlign = (HorizontalAlignment)2;
		TXTStartDate.TootTipDelay = 500;
		TxtFinishM.AllowNull = false;
		TxtFinishM.BackcolorTooltip = Color.Red;
		((TextBoxX)TxtFinishM).Border.CornerType = (eCornerType)1;
		TxtFinishM.EnterToTab = true;
		TxtFinishM.EscToClose = true;
		TxtFinishM.FarsyDateStyle = (ENFarsyDateStyle)1;
		TxtFinishM.FocusBackColor = Color.Yellow;
		TxtFinishM.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TxtFinishM.FocusForeColor = Color.Blue;
		TxtFinishM.FocusTextSelect = true;
		((Control)TxtFinishM).Font = new Font("Times New Roman", 12f);
		((Control)TxtFinishM).Location = new Point(11, 2);
		TxtFinishM.MessageEmptyShowDialog = false;
		TxtFinishM.MessegeEmpty = "";
		TxtFinishM.MessegeEmptyInFormRight = true;
		TxtFinishM.MessegeEmptyShowInForm = false;
		TxtFinishM.MiladiDate = new DateTime(2021, 12, 11, 15, 30, 3, 520);
		((Control)TxtFinishM).Name = "TxtFinishM";
		((Control)TxtFinishM).RightToLeft = (RightToLeft)0;
		((Control)TxtFinishM).Size = new Size(87, 20);
		((Control)TxtFinishM).TabIndex = 0;
		((Control)TxtFinishM).Text = "11/12/2021";
		((TextBox)TxtFinishM).TextAlign = (HorizontalAlignment)2;
		TxtFinishM.TootTipDelay = 500;
		((Control)labelX39).Anchor = (AnchorStyles)9;
		((Control)labelX39).BackColor = Color.Transparent;
		((BaseItemControl)labelX39).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX39).Location = new Point(381, 40);
		((Control)labelX39).Name = "labelX39";
		((Control)labelX39).Size = new Size(43, 31);
		((Control)labelX39).TabIndex = 0;
		((Control)labelX39).Text = "[نوع تردد] :";
		labelX39.TextAlignment = (StringAlignment)2;
		((Control)BtnMorePlace).AccessibleRole = (AccessibleRole)43;
		BtnMorePlace.ColorTable = (eButtonColor)6;
		((Control)BtnMorePlace).Location = new Point(11, 40);
		((Control)BtnMorePlace).Name = "BtnMorePlace";
		((Control)BtnMorePlace).Size = new Size(34, 31);
		((PopupItemControl)BtnMorePlace).Style = (eDotNetBarStyle)9;
		((Control)BtnMorePlace).TabIndex = 2;
		((Control)BtnMorePlace).TabStop = false;
		((Control)BtnMorePlace).Text = "...";
		((Control)BtnMorePlace).Click += BtnMorePlace_Click;
		((Control)TxtSelectTaradod).Anchor = (AnchorStyles)13;
		((ComboBox)TxtSelectTaradod).DrawMode = (DrawMode)1;
		((ComboBox)TxtSelectTaradod).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)TxtSelectTaradod).FormattingEnabled = true;
		((ComboBox)TxtSelectTaradod).ItemHeight = 26;
		((Control)TxtSelectTaradod).Location = new Point(49, 40);
		((Control)TxtSelectTaradod).Name = "TxtSelectTaradod";
		((Control)TxtSelectTaradod).Size = new Size(325, 32);
		TxtSelectTaradod.Style = (eDotNetBarStyle)9;
		((Control)TxtSelectTaradod).TabIndex = 1;
		((ComboBox)TxtSelectTaradod).SelectedIndexChanged += TxtSelectTaradod_SelectedIndexChanged;
		((PanelControl)groupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel1).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel1).Controls.Add((Control)(object)labelX5);
		((Control)groupPanel1).Controls.Add((Control)(object)RbMustWork);
		((Control)groupPanel1).Controls.Add((Control)(object)RbOptionalNotKarkerd);
		((Control)groupPanel1).Controls.Add((Control)(object)RbOptionalKarkerd);
		((Control)groupPanel1).Controls.Add((Control)(object)BtnHelp);
		((Control)groupPanel1).Controls.Add((Control)(object)ChbSTTatili);
		((Control)groupPanel1).Controls.Add((Control)(object)ChbSat);
		((Control)groupPanel1).Controls.Add((Control)(object)ChbMon);
		((Control)groupPanel1).Controls.Add((Control)(object)ChbSun);
		((Control)groupPanel1).Controls.Add((Control)(object)ChbThr);
		((Control)groupPanel1).Controls.Add((Control)(object)ChbTue);
		((Control)groupPanel1).Controls.Add((Control)(object)ChbWed);
		groupPanel1.DisabledBackColor = Color.Empty;
		((Control)groupPanel1).Location = new Point(20, 203);
		((Control)groupPanel1).Name = "groupPanel1";
		((Control)groupPanel1).Size = new Size(435, 190);
		((PanelControl)groupPanel1).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel1).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel1).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel1).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel1).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel1).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel1).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel1).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel1).Style.CornerDiameter = 4;
		((PanelControl)groupPanel1).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel1).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel1).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel1).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel1).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel1).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel1).TabIndex = 4;
		((Control)labelX5).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).ForeColor = Color.DarkRed;
		((Control)labelX5).Location = new Point(367, 80);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(58, 24);
		((Control)labelX5).TabIndex = 19;
		((Control)labelX5).Text = "[[روزهای جمعه]] :";
		((Control)RbMustWork).ForeColor = SystemColors.HotTrack;
		((Control)RbMustWork).Location = new Point(67, 110);
		((Control)RbMustWork).Name = "RbMustWork";
		((Control)RbMustWork).Size = new Size(295, 24);
		((Control)RbMustWork).TabIndex = 22;
		((Control)RbMustWork).Text = "[حضور الزامی است و در صورت عدم حضور غیبت محسوب میشود]";
		((ButtonBase)RbMustWork).UseVisualStyleBackColor = true;
		((Control)RbOptionalNotKarkerd).ForeColor = SystemColors.HotTrack;
		((Control)RbOptionalNotKarkerd).Location = new Point(67, 79);
		((Control)RbOptionalNotKarkerd).Name = "RbOptionalNotKarkerd";
		((Control)RbOptionalNotKarkerd).Size = new Size(295, 24);
		((Control)RbOptionalNotKarkerd).TabIndex = 21;
		((Control)RbOptionalNotKarkerd).Text = "[حضور اختیاری در جمعه ها و در صورت عدم حضور کارکرد محاسبه نمیشود]";
		((ButtonBase)RbOptionalNotKarkerd).UseVisualStyleBackColor = true;
		RbOptionalKarkerd.Checked = true;
		((Control)RbOptionalKarkerd).ForeColor = SystemColors.HotTrack;
		((Control)RbOptionalKarkerd).Location = new Point(67, 50);
		((Control)RbOptionalKarkerd).Name = "RbOptionalKarkerd";
		((Control)RbOptionalKarkerd).Size = new Size(295, 24);
		((Control)RbOptionalKarkerd).TabIndex = 20;
		RbOptionalKarkerd.TabStop = true;
		((Control)RbOptionalKarkerd).Text = "[حضور اختیاری در جمعه ها و در صورت عدم حضور کارکرد محاسبه میشود]";
		((ButtonBase)RbOptionalKarkerd).UseVisualStyleBackColor = true;
		((Control)BtnHelp).AccessibleRole = (AccessibleRole)43;
		BtnHelp.ColorTable = (eButtonColor)6;
		BtnHelp.Image = (Image)(object)Resources.helpinfo32;
		BtnHelp.ImagePosition = (eImagePosition)2;
		((Control)BtnHelp).Location = new Point(8, 50);
		((Control)BtnHelp).Name = "BtnHelp";
		((Control)BtnHelp).Size = new Size(54, 84);
		((PopupItemControl)BtnHelp).Style = (eDotNetBarStyle)9;
		((Control)BtnHelp).TabIndex = 23;
		((Control)BtnHelp).TabStop = false;
		((Control)BtnHelp).Text = "[شرح]";
		((Control)BtnHelp).Click += BtnHelp_Click;
		((Control)ChbSTTatili).Anchor = (AnchorStyles)9;
		((BaseItemControl)ChbSTTatili).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbSTTatili).Location = new Point(231, 140);
		((Control)ChbSTTatili).Name = "ChbSTTatili";
		((Control)ChbSTTatili).Size = new Size(193, 35);
		((BaseItemControl)ChbSTTatili).Style = (eDotNetBarStyle)9;
		((Control)ChbSTTatili).TabIndex = 9;
		((Control)ChbSTTatili).Text = "[در نظر گرفتن تعطیلات رسمی بعنوان روزکاری]";
		((BaseItemControl)ChbSat).BackgroundStyle.CornerType = (eCornerType)1;
		ChbSat.Checked = true;
		ChbSat.CheckState = (CheckState)1;
		ChbSat.CheckValue = "Y";
		((Control)ChbSat).Location = new Point(387, 3);
		((Control)ChbSat).Name = "ChbSat";
		((Control)ChbSat).Size = new Size(36, 33);
		((BaseItemControl)ChbSat).Style = (eDotNetBarStyle)9;
		((Control)ChbSat).TabIndex = 0;
		((Control)ChbSat).Text = "[شنبه]";
		((BaseItemControl)ChbMon).BackgroundStyle.CornerType = (eCornerType)1;
		ChbMon.Checked = true;
		ChbMon.CheckState = (CheckState)1;
		ChbMon.CheckValue = "Y";
		((Control)ChbMon).Location = new Point(242, 3);
		((Control)ChbMon).Name = "ChbMon";
		((Control)ChbMon).Size = new Size(45, 33);
		((BaseItemControl)ChbMon).Style = (eDotNetBarStyle)9;
		((Control)ChbMon).TabIndex = 2;
		((Control)ChbMon).Text = "[دوشنبه]";
		((BaseItemControl)ChbSun).BackgroundStyle.CornerType = (eCornerType)1;
		ChbSun.Checked = true;
		ChbSun.CheckState = (CheckState)1;
		ChbSun.CheckValue = "Y";
		((Control)ChbSun).Location = new Point(314, 3);
		((Control)ChbSun).Name = "ChbSun";
		((Control)ChbSun).Size = new Size(46, 33);
		((BaseItemControl)ChbSun).Style = (eDotNetBarStyle)9;
		((Control)ChbSun).TabIndex = 1;
		((Control)ChbSun).Text = "[یکشنبه]";
		((BaseItemControl)ChbThr).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbThr).Location = new Point(8, 3);
		((Control)ChbThr).Name = "ChbThr";
		((Control)ChbThr).Size = new Size(50, 33);
		((BaseItemControl)ChbThr).Style = (eDotNetBarStyle)9;
		((Control)ChbThr).TabIndex = 5;
		((Control)ChbThr).Text = "[پنجشنبه]";
		((BaseItemControl)ChbTue).BackgroundStyle.CornerType = (eCornerType)1;
		ChbTue.Checked = true;
		ChbTue.CheckState = (CheckState)1;
		ChbTue.CheckValue = "Y";
		((Control)ChbTue).Location = new Point(164, 3);
		((Control)ChbTue).Name = "ChbTue";
		((Control)ChbTue).Size = new Size(51, 33);
		((BaseItemControl)ChbTue).Style = (eDotNetBarStyle)9;
		((Control)ChbTue).TabIndex = 3;
		((Control)ChbTue).Text = "[سه شنبه]";
		((BaseItemControl)ChbWed).BackgroundStyle.CornerType = (eCornerType)1;
		ChbWed.Checked = true;
		ChbWed.CheckState = (CheckState)1;
		ChbWed.CheckValue = "Y";
		((Control)ChbWed).Location = new Point(85, 3);
		((Control)ChbWed).Name = "ChbWed";
		((Control)ChbWed).Size = new Size(51, 33);
		((BaseItemControl)ChbWed).Style = (eDotNetBarStyle)9;
		((Control)ChbWed).TabIndex = 4;
		((Control)ChbWed).Text = "[چهارشنبه]";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((ScrollableControl)this).AutoScroll = true;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(499, 500);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "FrmCrtShiftShenavar";
		((Control)this).Text = "FRMCrtShift1";
		((Form)this).Load += FRMCrtShiftShenavar_Load;
		((Control)this).KeyDown += new KeyEventHandler(FRMCrtShiftShenavar_KeyDown);
		((Control)PanelEx1).ResumeLayout(false);
		((Control)groupPanel7).ResumeLayout(false);
		((Control)groupPanelTaradod).ResumeLayout(false);
		((Control)groupPanelSelTaradod).ResumeLayout(false);
		((Control)groupPanel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}

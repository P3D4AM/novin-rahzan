using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevComponents.Editors;
using FormCustom;
using FormCustom.Cache;
using PacketParser;
using PacketParser.Attendance.Access;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using Serilog;
using TimeAttendance.ValidatorAttribute.Salary.FLRShift;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceUI.Properties;
using TimeAttendanceUI.Salary.Forms.Codeing;

namespace TimeAttendanceUI.Salary.Shift;

public class FrmShiftCodeing : FormExtention
{
	private CancellationTokenSource _changeToken = new CancellationTokenSource();

	private Dictionary<string, string> trDictionary = new Dictionary<string, string>();

	private readonly ShiftCodeing _clsShiftCodeing;

	private IContainer components;

	private PanelEx panelEx1;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private GroupPanel groupPanel7;

	private NumericUpDown MinShenavar;

	private NumericUpDown HourShenavar;

	internal LabelX labelX6;

	private RadioButton RBShenavarShift;

	private RadioButton RBNormalShift;

	private GroupPanel grpPanelNormal2;

	private LabelX lblTakhirKh;

	private LabelX TimeKhoroj2;

	private LabelX labelX16;

	private LabelX lblTajilKh;

	private NumericUpDown TakhirKh;

	private NumericUpDown TajilKh;

	private LabelX TimeKhoroj1;

	private NumericUpDown Min2;

	internal LabelX labelX2;

	private NumericUpDown Hour2;

	private LabelX labelX13;

	private GroupPanel grpPanelNormal1;

	private LabelX labelX15;

	private NumericUpDown Hour1;

	private LabelX lblTajilV;

	private LabelX lblTakhirV;

	private NumericUpDown Min1;

	private LabelX timeVorod2;

	private LabelX TimeVorod1;

	internal LabelX labelX7;

	private NumericUpDown TakhirV;

	private LabelX labelX12;

	private NumericUpDown TajilV;

	private GroupPanel groupPanel4;

	private GroupPanel grpPanelShenavar;

	private LabelX labelX8;

	private TextBoxX TxtDesc;

	internal LabelX labelX5;

	internal LabelX labelX10;

	internal LabelX labelX9;

	internal LabelX labelX1;

	internal LabelX labelX3;

	internal LabelX labelX4;

	private GroupPanel groupPanel5;

	private LabelX labelX11;

	private LabelX labelX14;

	private ColorDialog colorDialog1;

	private PanelEx pnlColor;

	private RadioButton RBSurcease;

	private ToolTip toolTip1;

	private NumericUpDown MlBLMaxTimeCanExit;

	internal LabelX labelX17;

	private NumericUpDown HlBLMaxTimeCanExit;

	private GroupPanel grpPanelPeakOvertime;

	private NumericUpDown MinutePeakOvertime;

	private NumericUpDown HourPeakOvertime;

	internal LabelX labelX18;

	private GroupPanel grpPanelSurcease;

	private LabelX labelX22;

	private LabelX labelX25;

	private LabelX labelX24;

	private LabelX labelX23;

	private Panel PnlNormal1;

	private LabelX labelX27;

	internal ButtonX BtnJarimeInfo;

	private NumericUpDown JarimeV;

	private CheckBoxX ChkJarime;

	internal ButtonX btnHurryDelayPerform;

	private CheckBoxX chbxHurryDelayPerform;

	private CheckBoxX ChbNextDay;

	private Panel PnlNextDay;

	internal ButtonX BtnHelp;

	private LabelX labelX26;

	private LabelX labelX29;

	private CheckBoxX ChkCalcEnteghalInSrcDate;

	private LabelX labelX28;

	private CheckBoxX ChkVacationInLunch;

	internal ButtonX btnHelpLunchTime;

	private LabelX labelX21;

	private LabelX labelX19;

	private NumericUpDown txtLunchTime;

	private NumericUpDown txtMinStartLunch;

	private NumericUpDown txtHurStartLunch;

	internal LabelX labelX20;

	private ComboBoxEx CmbTypeTaradodInShift;

	private ComboItem Normal;

	private ComboItem ShiftWork1;

	private ComboItem ShiftWork2;

	private ComboItem ShiftWork3;

	private ComboItem NightWork;

	private LabelX labelX30;

	private CheckBoxX ChkTashvighShift;

	internal ButtonX BtnSelectMazaya;

	private LabelX labelX31;

	private LabelX LblTashvighType;

	private CheckBoxX ChkAdamKasreKar;

	private CheckBoxX ChkCalcEK;

	public DateTime Vorod => DateTime.Now.Date.AddHours((int)Hour1.Value).AddMinutes((int)Min1.Value);

	public DateTime Khoroj => DateTime.Now.Date.AddHours((int)Hour2.Value).AddMinutes((int)Min2.Value);

	public DateTime VorodStart => Vorod.AddMinutes((double)(-TajilV.Value));

	public DateTime VorodEnd => Vorod.AddMinutes((double)TakhirV.Value);

	public DateTime KhorojStart => Khoroj.AddMinutes((double)(-TajilKh.Value));

	public DateTime KhorojEnd => Khoroj.AddMinutes((double)TakhirKh.Value);

	private async Task SetTrDictionaryAsync()
	{
		try
		{
			if (trDictionary == null)
			{
				trDictionary = new Dictionary<string, string>();
			}
			if (!trDictionary.TryGetValue("[افزودن یک نوع تردد جدید]", out var value))
			{
				trDictionary.Add("[افزودن یک نوع تردد جدید]", Utilities.GetTranslate("[افزودن یک نوع تردد جدید]"));
			}
			if (!trDictionary.TryGetValue("[ویرایش یک نوع تردد]", out value))
			{
				trDictionary.Add("[ویرایش یک نوع تردد]", Utilities.GetTranslate("[ویرایش یک نوع تردد]"));
			}
			if (!trDictionary.TryGetValue("[سقف اضافه کاری تا ساعت]", out value))
			{
				trDictionary.Add("[سقف اضافه کاری تا ساعت]", Utilities.GetTranslate("[سقف اضافه کاری تا ساعت]"));
			}
			if (!trDictionary.TryGetValue("[سقف اضافه کاری به مدت ]", out value))
			{
				trDictionary.Add("[سقف اضافه کاری به مدت ]", Utilities.GetTranslate("[سقف اضافه کاری به مدت ]"));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetTrDictionaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeing.cs", 46);
		}
	}

	public FrmShiftCodeing(string contextGuid)
		: base(contextGuid, true, true)
	{
		Task.Run((Func<Task?>)SetTrDictionaryAsync);
		InitializeComponent();
		try
		{
			_clsShiftCodeing = new ShiftCodeing();
			LoadData();
			((Control)this).Text = "افزودن یک نوع تردد جدید";
			if (trDictionary.TryGetValue("[افزودن یک نوع تردد جدید]", out var value))
			{
				((Control)this).Text = value;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeing.cs", 64);
		}
	}

	public FrmShiftCodeing(string contextGuid, TypeShiftEnumType ptyp)
		: base(contextGuid, true, true)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Task.Run((Func<Task?>)SetTrDictionaryAsync);
			InitializeComponent();
			_clsShiftCodeing = new ShiftCodeing
			{
				TypeShift = ptyp
			};
			LoadData();
			((Control)this).Text = "افزودن یک نوع تردد جدید";
			if (trDictionary.TryGetValue("[افزودن یک نوع تردد جدید]", out var value))
			{
				((Control)this).Text = value;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeing.cs", 82);
		}
	}

	public FrmShiftCodeing(string contextGuid, ShiftCodeing shiftCoding)
		: base(contextGuid, true, true)
	{
		try
		{
			Task.Run((Func<Task?>)SetTrDictionaryAsync);
			InitializeComponent();
			_clsShiftCodeing = shiftCoding;
			LoadData();
			((Control)this).Text = "ویرایش یک نوع تردد";
			if (trDictionary.TryGetValue("[ویرایش یک نوع تردد]", out var value))
			{
				((Control)this).Text = value;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeing.cs", 97);
		}
	}

	private void LoadData()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Expected I4, but got Unknown
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a0: Invalid comparison between Unknown and I4
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Expected I4, but got Unknown
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Invalid comparison between Unknown and I4
		try
		{
			if ((int)_clsShiftCodeing.TypeShift != 11)
			{
				Hour1.Value = _clsShiftCodeing.H1;
				Hour2.Value = _clsShiftCodeing.H2;
				Min1.Value = _clsShiftCodeing.M1;
				Min2.Value = _clsShiftCodeing.M2;
				string text = Time.IntToTime(_clsShiftCodeing.MaxTimeCanExit);
				if (!string.IsNullOrEmpty(text))
				{
					HlBLMaxTimeCanExit.Value = int.Parse(text.Substring(0, 2));
					MlBLMaxTimeCanExit.Value = int.Parse(text.Substring(3, 2));
				}
				ChbNextDay.Checked = _clsShiftCodeing.NextDay;
				ChkCalcEnteghalInSrcDate.Checked = _clsShiftCodeing.CalcEnteghalInSrcDate;
				ChkCalcEK.Checked = _clsShiftCodeing.NotCalcEKForShift;
				ChkTashvighShift.Checked = _clsShiftCodeing.HasTashvighiInShift;
				ChkAdamKasreKar.Checked = _clsShiftCodeing.HasTashvighIfNotKKInBeforeShift;
				SetTashvighTypeAsync(_clsShiftCodeing.MazayaIdForTashvigh);
				((ListControl)CmbTypeTaradodInShift).SelectedIndex = (int)_clsShiftCodeing.TypeTaradodInShift;
				((Control)ChkCalcEnteghalInSrcDate).Enabled = ChbNextDay.Checked;
				TakhirV.Value = _clsShiftCodeing.TakhirV;
				JarimeV.Value = _clsShiftCodeing.JarimeV;
				TakhirKh.Value = _clsShiftCodeing.TakhirKh;
				TajilV.Value = _clsShiftCodeing.TajilV;
				TajilKh.Value = _clsShiftCodeing.TajilKh;
				text = Time.IntToTime((int)_clsShiftCodeing.PeakOvertime);
				if (!string.IsNullOrEmpty(text))
				{
					int num = int.Parse(text.Substring(0, 2));
					int num2 = int.Parse(text.Substring(3, 2));
					if (num >= 24)
					{
						num = 23;
						num2 = 59;
					}
					HourPeakOvertime.Value = num;
					MinutePeakOvertime.Value = num2;
				}
				chbxHurryDelayPerform.Checked = _clsShiftCodeing.HurryDelayPerform;
				ChkJarime.Checked = _clsShiftCodeing.ChkJarime;
				((Control)txtLunchTime).Text = _clsShiftCodeing.LunchTime.ToString();
				((Control)txtMinStartLunch).Text = (_clsShiftCodeing.StartLunchTime % 60).ToString();
				((Control)txtHurStartLunch).Text = (_clsShiftCodeing.StartLunchTime / 60).ToString();
				if ((int)_clsShiftCodeing.TypeShift == 2)
				{
					MinShenavar.Value = _clsShiftCodeing.M2;
					HourShenavar.Value = _clsShiftCodeing.H2;
				}
				ChkVacationInLunch.Checked = _clsShiftCodeing.CalcVacationInLunch;
			}
			TypeShiftEnumType typeShift = _clsShiftCodeing.TypeShift;
			switch ((int)typeShift)
			{
			default:
				if ((int)typeShift != 100)
				{
					break;
				}
				goto case 3;
			case 1:
			{
				RBNormalShift.Checked = true;
				((Control)grpPanelPeakOvertime).Text = "سقف اضافه کاری تا ساعت";
				if (trDictionary.TryGetValue("[سقف اضافه کاری تا ساعت]", out var value2))
				{
					((Control)grpPanelPeakOvertime).Text = value2;
				}
				break;
			}
			case 2:
			{
				RBShenavarShift.Checked = true;
				((Control)grpPanelPeakOvertime).Text = "سقف اضافه کاری به مدت";
				if (trDictionary.TryGetValue("[سقف اضافه کاری به مدت ]", out var value))
				{
					((Control)grpPanelPeakOvertime).Text = value;
				}
				break;
			}
			case 11:
				RBSurcease.Checked = true;
				break;
			case 3:
			case 4:
			case 5:
				RBNormalShift.Checked = true;
				RBNormalShift_CheckedChanged(null, null);
				break;
			case 0:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
				break;
			}
			((Control)TxtDesc).Text = _clsShiftCodeing.Description;
			ChangeTime();
			pnlColor.Style.BackColor1.Color = ColorTranslator.FromHtml(_clsShiftCodeing.ShiftColor);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeing.cs", 197);
		}
	}

	private async Task SetTashvighTypeAsync(int mazayaIdForTashvigh, CancellationToken token = default(CancellationToken))
	{
		try
		{
			FishHesab clsFishHesab = await FishHesab.GetAsync(((CacheBaseForm)this).ContextGuid, mazayaIdForTashvigh, token);
			if (clsFishHesab == null)
			{
				return;
			}
			_clsShiftCodeing.MazayaIdForTashvigh = mazayaIdForTashvigh;
			do
			{
				await Task.Delay(100);
				if (((FormExtention)this).IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			while (!((Control)this).IsHandleCreated);
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!token.IsCancellationRequested && !((FormExtention)this).IsDisposed)
					{
						((Control)LblTashvighType).Text = clsFishHesab?.Name ?? "----";
					}
				}
				catch (TaskCanceledException)
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
			Log.Error(ex, "");
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		try
		{
			((FormExtention)this).OnLoad(e);
			SetTooltip();
			((FormExtention)this).OK_Click += new CrmEventHandler(AcceptEvent);
			((Control)BtnCancel).Click += base.BtnCancel_Click;
			((ToolStrip)base.statusStrip1).BackColor = Color.FromArgb(175, 210, 255);
			((Control)RBNormalShift).Focus();
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Shift.ShiftShenavar)
			{
				((Control)RBShenavarShift).Enabled = false;
			}
			InitAccItems();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeing.cs", 246);
		}
	}

	private void InitAccItems()
	{
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Invalid comparison between Unknown and I4
		try
		{
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Shift.ShiftAdvance)
			{
				((Control)lblTajilV).Enabled = false;
				((Control)TajilV).Enabled = false;
				((Control)lblTakhirV).Enabled = false;
				((Control)TakhirV).Enabled = false;
				((Control)lblTajilKh).Enabled = false;
				((Control)TajilKh).Enabled = false;
				((Control)lblTakhirKh).Enabled = false;
				((Control)TakhirKh).Enabled = false;
				ChbNextDay.Checked = false;
				ChkCalcEnteghalInSrcDate.Checked = false;
				ChkCalcEK.Checked = false;
				ChkAdamKasreKar.Checked = false;
				ChkTashvighShift.Checked = false;
				if ((int)AccessLevelClass.Instance.LevelHozor != 5)
				{
					((Control)ChkJarime).Enabled = false;
					ChkJarime.Checked = false;
					((Control)JarimeV).Enabled = false;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "InitAccItems", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeing.cs", 278);
		}
	}

	private async void RBNormalShift_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			GroupPanel obj = grpPanelNormal1;
			GroupPanel obj2 = grpPanelNormal2;
			Panel pnlNormal = PnlNormal1;
			Panel pnlNextDay = PnlNextDay;
			GroupPanel obj3 = grpPanelPeakOvertime;
			GroupPanel obj4 = grpPanelShenavar;
			bool flag2 = (((Control)grpPanelSurcease).Visible = false);
			bool flag4 = (((Control)obj4).Visible = flag2);
			bool flag6 = (((Control)obj3).Visible = flag4);
			bool flag8 = (((Control)pnlNextDay).Visible = flag6);
			bool flag10 = (((Control)pnlNormal).Visible = flag8);
			bool visible = (((Control)obj2).Visible = flag10);
			((Control)obj).Visible = visible;
			if (RBNormalShift.Checked)
			{
				((Control)grpPanelNormal1).Visible = true;
				((Control)grpPanelNormal2).Visible = true;
				((Control)PnlNormal1).Visible = true;
				((Control)PnlNextDay).Visible = true;
				((Control)grpPanelPeakOvertime).Visible = true;
				((Control)grpPanelPeakOvertime).Text = Utilities.GetTranslate("[سقف اضافه کاری تا ساعت]");
				((Control)ChbNextDay).Text = Utilities.GetTranslate("[انتقال به روز بعد اعمال شود در صورتیکه تردد روز بعد قبل از ساعت ]");
			}
			else if (RBShenavarShift.Checked)
			{
				((Control)PnlNextDay).Visible = true;
				((Control)grpPanelShenavar).Visible = true;
				((Control)grpPanelPeakOvertime).Visible = true;
				((Control)grpPanelPeakOvertime).Text = Utilities.GetTranslate("[سقف اضافه کاری به مدت ]");
				((Control)ChbNextDay).Text = Utilities.GetTranslate("[انتقال به روز بعد در صورتیکه زمان بین ترددهای ورود و روز بعد کمتر از]");
			}
			else if (RBSurcease.Checked)
			{
				((Control)grpPanelSurcease).Visible = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RBNormalShift_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeing.cs", 322);
		}
	}

	private void Hour1_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			ChangeTime();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Hour1_ValueChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeing.cs", 335);
		}
	}

	private void ChangeTime()
	{
		try
		{
			((Control)TimeVorod1).Text = VorodStart.Hour + ":" + VorodStart.Minute;
			((Control)timeVorod2).Text = VorodEnd.Hour + ":" + VorodEnd.Minute;
			((Control)TimeKhoroj1).Text = KhorojStart.Hour + ":" + KhorojStart.Minute;
			((Control)TimeKhoroj2).Text = KhorojEnd.Hour + ":" + KhorojEnd.Minute;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ChangeTime", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeing.cs", 349);
		}
	}

	private void BtnHelp_Click(object sender, EventArgs e)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Form)new FrmHelpNextDay(((CacheBaseForm)this).ContextGuid, "", "[توضیحات انتقال از روز قبل]")).ShowDialog();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnHelp_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeing.cs", 363);
		}
	}

	private void SetTooltip()
	{
		try
		{
			string text = "[ترددهایی که کارمند بعد از این زمان ثبت میکند، بعنوان ورود محاسبه میشود.]";
			toolTip1.SetToolTip((Control)(object)HlBLMaxTimeCanExit, text);
			toolTip1.SetToolTip((Control)(object)MlBLMaxTimeCanExit, text);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetTooltip", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeing.cs", 378);
		}
	}

	private void ChbNextDay_CheckedChanged(object sender, EventArgs e)
	{
		NumericUpDown mlBLMaxTimeCanExit = MlBLMaxTimeCanExit;
		NumericUpDown hlBLMaxTimeCanExit = HlBLMaxTimeCanExit;
		bool flag = (((Control)ChkCalcEnteghalInSrcDate).Enabled = ChbNextDay.Checked);
		bool enabled = (((Control)hlBLMaxTimeCanExit).Enabled = flag);
		((Control)mlBLMaxTimeCanExit).Enabled = enabled;
	}

	private void pnlColor_Click(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		try
		{
			if ((int)((CommonDialog)colorDialog1).ShowDialog() == 1)
			{
				pnlColor.Style.BackColor1.Color = colorDialog1.Color;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "pnlColor_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeing.cs", 397);
		}
	}

	private void GetData()
	{
		try
		{
			_clsShiftCodeing.H1 = short.Parse(Hour1.Value.ToString());
			_clsShiftCodeing.H2 = short.Parse(Hour2.Value.ToString());
			_clsShiftCodeing.M1 = short.Parse(Min1.Value.ToString());
			_clsShiftCodeing.M2 = short.Parse(Min2.Value.ToString());
			_clsShiftCodeing.TakhirV = short.Parse(TakhirV.Value.ToString());
			_clsShiftCodeing.JarimeV = short.Parse(JarimeV.Value.ToString());
			_clsShiftCodeing.TakhirKh = short.Parse(TakhirKh.Value.ToString());
			_clsShiftCodeing.TajilV = short.Parse(TajilV.Value.ToString());
			_clsShiftCodeing.TajilKh = short.Parse(TajilKh.Value.ToString());
			_clsShiftCodeing.Description = Text.FixString(((Control)TxtDesc).Text);
			_clsShiftCodeing.NextDay = ChbNextDay.Checked;
			_clsShiftCodeing.CalcEnteghalInSrcDate = ChkCalcEnteghalInSrcDate.Checked;
			_clsShiftCodeing.NotCalcEKForShift = ChkCalcEK.Checked;
			_clsShiftCodeing.HasTashvighiInShift = ChkTashvighShift.Checked;
			_clsShiftCodeing.HasTashvighIfNotKKInBeforeShift = ChkAdamKasreKar.Checked;
			_clsShiftCodeing.TypeTaradodInShift = (TypeTaradodInShiftEnum)(short)((ListControl)CmbTypeTaradodInShift).SelectedIndex;
			_clsShiftCodeing.HurryDelayPerform = chbxHurryDelayPerform.Checked;
			_clsShiftCodeing.ChkJarime = ChkJarime.Checked;
			_clsShiftCodeing.LunchTime = Utilities.ParseToShort(((Control)txtLunchTime).Text, (short)0);
			_clsShiftCodeing.StartLunchTime = (short)(Utilities.ParseToShort(((Control)txtHurStartLunch).Text, (short)0) * 60 + Utilities.ParseToShort(((Control)txtMinStartLunch).Text, (short)0));
			if (_clsShiftCodeing.H2 < 12 && _clsShiftCodeing.NextDay && !RBShenavarShift.Checked && HlBLMaxTimeCanExit.Value < (decimal)_clsShiftCodeing.H2)
			{
				HlBLMaxTimeCanExit.Value = _clsShiftCodeing.H2 + 1;
			}
			_clsShiftCodeing.MaxTimeCanExit = (int)(HlBLMaxTimeCanExit.Value * 60m + MlBLMaxTimeCanExit.Value);
			_clsShiftCodeing.PeakOvertime = (short)(HourPeakOvertime.Value * 60m + MinutePeakOvertime.Value);
			if (_clsShiftCodeing.PeakOvertime >= 1440)
			{
				_clsShiftCodeing.PeakOvertime = 1439;
			}
			if (RBShenavarShift.Checked)
			{
				_clsShiftCodeing.TypeShift = (TypeShiftEnumType)2;
				_clsShiftCodeing.H1 = 0;
				_clsShiftCodeing.H2 = short.Parse(HourShenavar.Value.ToString());
				_clsShiftCodeing.M1 = 0;
				_clsShiftCodeing.M2 = short.Parse(MinShenavar.Value.ToString());
			}
			else if (RBSurcease.Checked)
			{
				_clsShiftCodeing.TypeShift = (TypeShiftEnumType)11;
			}
			else if (RBNormalShift.Checked)
			{
				_clsShiftCodeing.TypeShift = (TypeShiftEnumType)100;
			}
			_clsShiftCodeing.ShiftColor = pnlColor.Style.BackColor1.Color.Name;
			if (_clsShiftCodeing.ShiftColor.Any(char.IsDigit))
			{
				_clsShiftCodeing.ShiftColor = "#" + _clsShiftCodeing.ShiftColor;
			}
			_clsShiftCodeing.CalcVacationInLunch = ChkVacationInLunch.Checked;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeing.cs", 458);
		}
	}

	private async void AcceptEvent(object sender, EventArgs e)
	{
		if (!((Control)BtnAccept).Enabled || !((Control)BtnAccept).Visible)
		{
			return;
		}
		try
		{
			GetData();
			DialogResult val = ShiftCodeingCheckValidation.CheckValidation(((CacheBaseForm)this).ContextGuid, _clsShiftCodeing, isShowMessage: true);
			if ((int)val == 6 || (int)val == 5)
			{
				((FormExtention)this).ResetControlChanged();
				((Form)this).DialogResult = (DialogResult)((await _clsShiftCodeing.SaveAsync()) ? 1 : 2);
				((Form)this).Close();
			}
			else if ((int)val == 2)
			{
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "AcceptEvent", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeing.cs", 486);
		}
	}

	private void btnHelpLunchTime_Click(object sender, EventArgs e)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("[تایم استراحت مدت زمانی است که پرسنل می توانند از زمان تعیین شده به مدت مشخص شده در قسمت تایم استراحت، به امورات شخصی از قبیل ناهار ]");
			stringBuilder.AppendLine("[نماز و یا استراحت بپردازند ]");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("[بدیهی است این مدت زمان در کارکرد شخص لحاظ نمی گردد]");
			stringBuilder.AppendLine("[به عبارتی در محاسبه کارکرد، اضافه کار، کسرکار و ... بی تاثیر است]");
			((Form)new FrmHelpNextDay(((CacheBaseForm)this).ContextGuid, stringBuilder.ToString(), "[توضیحات تایم استراحت]")).ShowDialog();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "btnHelpLunchTime_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeing.cs", 508);
		}
	}

	private void ChkJarime_CheckedChanged(object sender, EventArgs e)
	{
		((Control)JarimeV).Enabled = ((Control)ChkJarime).Enabled;
	}

	private void BtnJarimeInfo_Click(object sender, EventArgs e)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string text = "[در صورتیکه تیک گزینه محاسبه جریمه فعال باشد] \r\n";
			text += "[و شخصی بیش از مدت زمان مشخص شده در ورود تاخیر داشته باشد برای وی آن روز جریمه محسوب خواهد شد]\r\n ";
			text += "[و بر اساس مبلغ جریمه تعیین شده در قراردادش، به ازاء هر روز مبلغ ذکر شده از حقوق وی کسر خواهد گردید]";
			((Form)new FrmHelpNextDay(((CacheBaseForm)this).ContextGuid, text, "[محاسبه جریمه]")).ShowDialog();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnJarimeInfo_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeing.cs", 531);
		}
	}

	private void btnHurryDelayPerform_Click(object sender, EventArgs e)
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("[در حالت عادی کارکرد براساس زمان ورود و خروج واقعی محاسبه میشود]");
			stringBuilder.AppendLine("[و اگر این گزینه انتخاب شود تعجیل و تاخیر در کارکرد اعمال میشود]");
			stringBuilder.AppendLine("[فرض کنید شروع شیفت ساعت 7 باشد و تعجیل در ورود را 10 دقیقه تنظیم کرده باشید و شخص ساعت 06:52 تردد بزند]");
			stringBuilder.AppendLine("");
			stringBuilder.AppendLine("[حال اگر این گزینه فعال باشد کارکرد از ساعت 07:00 محاسبه میشود]");
			stringBuilder.AppendLine("[در غیر اینصورت از ساعت 06:52 محاسبه میشود]");
			stringBuilder.AppendLine("[دقت داشته باشید که این تنظیم فقط بر روی کارکرد اعمال می گردد]");
			((Form)new FrmHelpNextDay(((CacheBaseForm)this).ContextGuid, stringBuilder.ToString(), "[محاسبه کارکرد (تعجیل و تاخیر)]")).ShowDialog();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "btnHurryDelayPerform_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeing.cs", 557);
		}
	}

	private async void BtnSelectMazaya_Click(object sender, EventArgs e)
	{
		try
		{
			if (!((Control)BtnSelectMazaya).Enabled || !((Control)BtnSelectMazaya).Visible)
			{
				return;
			}
			try
			{
				FrmHesabList frmHesabList = new FrmHesabList(((CacheBaseForm)this).ContextGuid, ((Control)this).Font, 1);
				((Form)frmHesabList).WindowState = (FormWindowState)0;
				FrmHesabList frmHesabList2 = frmHesabList;
				_changeToken?.Cancel();
				_changeToken = new CancellationTokenSource();
				_ = _changeToken.Token;
				if ((int)((Form)frmHesabList2).ShowDialog() == 1)
				{
					string selectedItems = frmHesabList2.SelectedItems;
					if (!(selectedItems == ""))
					{
						int mazayaIdForTashvigh = int.Parse(selectedItems.Split(new char[1] { char.Parse(";") }).FirstOrDefault());
						await SetTashvighTypeAsync(mazayaIdForTashvigh);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnSelectMazaya_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FrmShiftCodeing.cs", 582);
			}
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((FormExtention)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
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
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Expected O, but got Unknown
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Expected O, but got Unknown
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Expected O, but got Unknown
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Expected O, but got Unknown
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Expected O, but got Unknown
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Expected O, but got Unknown
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Expected O, but got Unknown
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Expected O, but got Unknown
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Expected O, but got Unknown
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Expected O, but got Unknown
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Expected O, but got Unknown
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Expected O, but got Unknown
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Expected O, but got Unknown
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Expected O, but got Unknown
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Expected O, but got Unknown
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Expected O, but got Unknown
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Expected O, but got Unknown
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Expected O, but got Unknown
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Expected O, but got Unknown
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Expected O, but got Unknown
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Expected O, but got Unknown
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Expected O, but got Unknown
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Expected O, but got Unknown
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Expected O, but got Unknown
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Expected O, but got Unknown
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Expected O, but got Unknown
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Expected O, but got Unknown
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Expected O, but got Unknown
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Expected O, but got Unknown
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Expected O, but got Unknown
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Expected O, but got Unknown
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Expected O, but got Unknown
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Expected O, but got Unknown
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Expected O, but got Unknown
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Expected O, but got Unknown
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Expected O, but got Unknown
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Expected O, but got Unknown
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Expected O, but got Unknown
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Expected O, but got Unknown
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Expected O, but got Unknown
		//IL_0718: Unknown result type (might be due to invalid IL or missing references)
		//IL_0722: Expected O, but got Unknown
		//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fc: Expected O, but got Unknown
		//IL_0a19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a23: Expected O, but got Unknown
		//IL_0e34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e3e: Expected O, but got Unknown
		//IL_0f44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4e: Expected O, but got Unknown
		//IL_1086: Unknown result type (might be due to invalid IL or missing references)
		//IL_1090: Expected O, but got Unknown
		//IL_11b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11bd: Expected O, but got Unknown
		//IL_13cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d9: Expected O, but got Unknown
		//IL_145e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1468: Expected O, but got Unknown
		//IL_175b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1765: Expected O, but got Unknown
		//IL_17ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_17f4: Expected O, but got Unknown
		//IL_187e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1888: Expected O, but got Unknown
		//IL_199c: Unknown result type (might be due to invalid IL or missing references)
		//IL_19a6: Expected O, but got Unknown
		//IL_1a61: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a6b: Expected O, but got Unknown
		//IL_1e7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e88: Expected O, but got Unknown
		//IL_2287: Unknown result type (might be due to invalid IL or missing references)
		//IL_2291: Expected O, but got Unknown
		//IL_26e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_26ea: Expected O, but got Unknown
		//IL_2781: Unknown result type (might be due to invalid IL or missing references)
		//IL_278b: Expected O, but got Unknown
		//IL_29d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_29e3: Expected O, but got Unknown
		//IL_2d7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d86: Expected O, but got Unknown
		//IL_2e1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e28: Expected O, but got Unknown
		//IL_2ec0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eca: Expected O, but got Unknown
		//IL_33ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_33b8: Expected O, but got Unknown
		//IL_3627: Unknown result type (might be due to invalid IL or missing references)
		//IL_3631: Expected O, but got Unknown
		//IL_37dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_37e6: Expected O, but got Unknown
		//IL_42df: Unknown result type (might be due to invalid IL or missing references)
		//IL_42e9: Expected O, but got Unknown
		//IL_45f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_45fa: Expected O, but got Unknown
		//IL_48e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_48f3: Expected O, but got Unknown
		//IL_499f: Unknown result type (might be due to invalid IL or missing references)
		//IL_49a9: Expected O, but got Unknown
		//IL_4a4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a59: Expected O, but got Unknown
		//IL_4afc: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b06: Expected O, but got Unknown
		//IL_4ba9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bb3: Expected O, but got Unknown
		//IL_4c49: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c53: Expected O, but got Unknown
		components = new Container();
		panelEx1 = new PanelEx();
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		groupPanel7 = new GroupPanel();
		PnlNextDay = new Panel();
		ChkAdamKasreKar = new CheckBoxX();
		LblTashvighType = new LabelX();
		BtnSelectMazaya = new ButtonX();
		labelX31 = new LabelX();
		ChkTashvighShift = new CheckBoxX();
		labelX30 = new LabelX();
		CmbTypeTaradodInShift = new ComboBoxEx();
		Normal = new ComboItem();
		ShiftWork1 = new ComboItem();
		ShiftWork2 = new ComboItem();
		ShiftWork3 = new ComboItem();
		NightWork = new ComboItem();
		labelX28 = new LabelX();
		ChkVacationInLunch = new CheckBoxX();
		btnHelpLunchTime = new ButtonX();
		labelX21 = new LabelX();
		labelX19 = new LabelX();
		txtLunchTime = new NumericUpDown();
		txtMinStartLunch = new NumericUpDown();
		txtHurStartLunch = new NumericUpDown();
		labelX20 = new LabelX();
		ChkCalcEnteghalInSrcDate = new CheckBoxX();
		ChbNextDay = new CheckBoxX();
		HlBLMaxTimeCanExit = new NumericUpDown();
		BtnHelp = new ButtonX();
		labelX17 = new LabelX();
		MlBLMaxTimeCanExit = new NumericUpDown();
		grpPanelPeakOvertime = new GroupPanel();
		MinutePeakOvertime = new NumericUpDown();
		HourPeakOvertime = new NumericUpDown();
		labelX18 = new LabelX();
		labelX26 = new LabelX();
		grpPanelNormal1 = new GroupPanel();
		labelX1 = new LabelX();
		labelX15 = new LabelX();
		Hour1 = new NumericUpDown();
		lblTajilV = new LabelX();
		lblTakhirV = new LabelX();
		Min1 = new NumericUpDown();
		timeVorod2 = new LabelX();
		TimeVorod1 = new LabelX();
		labelX7 = new LabelX();
		labelX3 = new LabelX();
		TakhirV = new NumericUpDown();
		labelX12 = new LabelX();
		TajilV = new NumericUpDown();
		labelX4 = new LabelX();
		grpPanelNormal2 = new GroupPanel();
		labelX5 = new LabelX();
		labelX10 = new LabelX();
		labelX9 = new LabelX();
		lblTakhirKh = new LabelX();
		TimeKhoroj2 = new LabelX();
		labelX16 = new LabelX();
		lblTajilKh = new LabelX();
		TakhirKh = new NumericUpDown();
		TajilKh = new NumericUpDown();
		TimeKhoroj1 = new LabelX();
		Min2 = new NumericUpDown();
		labelX2 = new LabelX();
		Hour2 = new NumericUpDown();
		labelX13 = new LabelX();
		PnlNormal1 = new Panel();
		btnHurryDelayPerform = new ButtonX();
		chbxHurryDelayPerform = new CheckBoxX();
		labelX27 = new LabelX();
		BtnJarimeInfo = new ButtonX();
		JarimeV = new NumericUpDown();
		ChkJarime = new CheckBoxX();
		groupPanel4 = new GroupPanel();
		RBSurcease = new RadioButton();
		RBNormalShift = new RadioButton();
		RBShenavarShift = new RadioButton();
		pnlColor = new PanelEx();
		labelX14 = new LabelX();
		labelX8 = new LabelX();
		grpPanelShenavar = new GroupPanel();
		MinShenavar = new NumericUpDown();
		HourShenavar = new NumericUpDown();
		labelX6 = new LabelX();
		TxtDesc = new TextBoxX();
		groupPanel5 = new GroupPanel();
		labelX11 = new LabelX();
		grpPanelSurcease = new GroupPanel();
		labelX29 = new LabelX();
		labelX25 = new LabelX();
		labelX24 = new LabelX();
		labelX23 = new LabelX();
		labelX22 = new LabelX();
		colorDialog1 = new ColorDialog();
		toolTip1 = new ToolTip(components);
		ChkCalcEK = new CheckBoxX();
		((Control)panelEx1).SuspendLayout();
		((Control)groupPanel7).SuspendLayout();
		((Control)PnlNextDay).SuspendLayout();
		((ISupportInitialize)txtLunchTime).BeginInit();
		((ISupportInitialize)txtMinStartLunch).BeginInit();
		((ISupportInitialize)txtHurStartLunch).BeginInit();
		((ISupportInitialize)HlBLMaxTimeCanExit).BeginInit();
		((ISupportInitialize)MlBLMaxTimeCanExit).BeginInit();
		((Control)grpPanelPeakOvertime).SuspendLayout();
		((ISupportInitialize)MinutePeakOvertime).BeginInit();
		((ISupportInitialize)HourPeakOvertime).BeginInit();
		((Control)grpPanelNormal1).SuspendLayout();
		((ISupportInitialize)Hour1).BeginInit();
		((ISupportInitialize)Min1).BeginInit();
		((ISupportInitialize)TakhirV).BeginInit();
		((ISupportInitialize)TajilV).BeginInit();
		((Control)grpPanelNormal2).SuspendLayout();
		((ISupportInitialize)TakhirKh).BeginInit();
		((ISupportInitialize)TajilKh).BeginInit();
		((ISupportInitialize)Min2).BeginInit();
		((ISupportInitialize)Hour2).BeginInit();
		((Control)PnlNormal1).SuspendLayout();
		((ISupportInitialize)JarimeV).BeginInit();
		((Control)groupPanel4).SuspendLayout();
		((Control)grpPanelShenavar).SuspendLayout();
		((ISupportInitialize)MinShenavar).BeginInit();
		((ISupportInitialize)HourShenavar).BeginInit();
		((Control)groupPanel5).SuspendLayout();
		((Control)grpPanelSurcease).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Controls.Add((Control)(object)groupPanel7);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(574, 713);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		BtnCancel.DialogResult = (DialogResult)2;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(15, 653);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(100, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 2;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو]   (Esc)";
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(121, 653);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(95, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 1;
		((Control)BtnAccept).Text = "[ثبت]  (F5)";
		((Control)BtnAccept).Click += AcceptEvent;
		((Control)groupPanel7).Anchor = (AnchorStyles)15;
		((Control)groupPanel7).BackColor = Color.Transparent;
		((PanelControl)groupPanel7).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel7).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel7).Controls.Add((Control)(object)ChkCalcEK);
		((Control)groupPanel7).Controls.Add((Control)(object)PnlNextDay);
		((Control)groupPanel7).Controls.Add((Control)(object)grpPanelNormal1);
		((Control)groupPanel7).Controls.Add((Control)(object)grpPanelNormal2);
		((Control)groupPanel7).Controls.Add((Control)(object)PnlNormal1);
		((Control)groupPanel7).Controls.Add((Control)(object)groupPanel4);
		((Control)groupPanel7).Controls.Add((Control)(object)pnlColor);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX14);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX8);
		((Control)groupPanel7).Controls.Add((Control)(object)grpPanelShenavar);
		((Control)groupPanel7).Controls.Add((Control)(object)TxtDesc);
		((Control)groupPanel7).Controls.Add((Control)(object)groupPanel5);
		((Control)groupPanel7).Controls.Add((Control)(object)grpPanelSurcease);
		groupPanel7.DisabledBackColor = Color.Empty;
		((Control)groupPanel7).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)groupPanel7).Location = new Point(12, 12);
		((Control)groupPanel7).Name = "groupPanel7";
		((Control)groupPanel7).Size = new Size(550, 635);
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
		((Control)groupPanel7).TabIndex = 0;
		((Control)PnlNextDay).Controls.Add((Control)(object)ChkAdamKasreKar);
		((Control)PnlNextDay).Controls.Add((Control)(object)LblTashvighType);
		((Control)PnlNextDay).Controls.Add((Control)(object)BtnSelectMazaya);
		((Control)PnlNextDay).Controls.Add((Control)(object)labelX31);
		((Control)PnlNextDay).Controls.Add((Control)(object)ChkTashvighShift);
		((Control)PnlNextDay).Controls.Add((Control)(object)labelX30);
		((Control)PnlNextDay).Controls.Add((Control)(object)CmbTypeTaradodInShift);
		((Control)PnlNextDay).Controls.Add((Control)(object)labelX28);
		((Control)PnlNextDay).Controls.Add((Control)(object)ChkVacationInLunch);
		((Control)PnlNextDay).Controls.Add((Control)(object)btnHelpLunchTime);
		((Control)PnlNextDay).Controls.Add((Control)(object)labelX21);
		((Control)PnlNextDay).Controls.Add((Control)(object)labelX19);
		((Control)PnlNextDay).Controls.Add((Control)(object)txtLunchTime);
		((Control)PnlNextDay).Controls.Add((Control)(object)txtMinStartLunch);
		((Control)PnlNextDay).Controls.Add((Control)(object)txtHurStartLunch);
		((Control)PnlNextDay).Controls.Add((Control)(object)labelX20);
		((Control)PnlNextDay).Controls.Add((Control)(object)ChkCalcEnteghalInSrcDate);
		((Control)PnlNextDay).Controls.Add((Control)(object)ChbNextDay);
		((Control)PnlNextDay).Controls.Add((Control)(object)HlBLMaxTimeCanExit);
		((Control)PnlNextDay).Controls.Add((Control)(object)BtnHelp);
		((Control)PnlNextDay).Controls.Add((Control)(object)labelX17);
		((Control)PnlNextDay).Controls.Add((Control)(object)MlBLMaxTimeCanExit);
		((Control)PnlNextDay).Controls.Add((Control)(object)grpPanelPeakOvertime);
		((Control)PnlNextDay).Controls.Add((Control)(object)labelX26);
		((Control)PnlNextDay).Location = new Point(4, 269);
		((Control)PnlNextDay).Name = "PnlNextDay";
		((Control)PnlNextDay).Size = new Size(536, 190);
		((Control)PnlNextDay).TabIndex = 25;
		((BaseItemControl)ChkAdamKasreKar).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChkAdamKasreKar).Font = new Font("Tahoma", 8f, (FontStyle)1);
		((Control)ChkAdamKasreKar).Location = new Point(9, 140);
		((Control)ChkAdamKasreKar).Name = "ChkAdamKasreKar";
		((Control)ChkAdamKasreKar).Size = new Size(194, 21);
		((BaseItemControl)ChkAdamKasreKar).Style = (eDotNetBarStyle)9;
		((Control)ChkAdamKasreKar).TabIndex = 40;
		((Control)ChkAdamKasreKar).Text = "نداشتن کسرکار شیفت قبل";
		((Control)LblTashvighType).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblTashvighType).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblTashvighType).Location = new Point(44, 119);
		((Control)LblTashvighType).Name = "LblTashvighType";
		((Control)LblTashvighType).Size = new Size(107, 21);
		((Control)LblTashvighType).TabIndex = 39;
		((Control)LblTashvighType).Text = "انتخاب نوع تشویقی";
		LblTashvighType.TextAlignment = (StringAlignment)2;
		((Control)BtnSelectMazaya).AccessibleRole = (AccessibleRole)43;
		((Control)BtnSelectMazaya).Font = new Font("Tahoma", 15f, (FontStyle)1, (GraphicsUnit)2);
		((Control)BtnSelectMazaya).Location = new Point(6, 119);
		((Control)BtnSelectMazaya).Name = "BtnSelectMazaya";
		((Control)BtnSelectMazaya).Size = new Size(20, 21);
		((PopupItemControl)BtnSelectMazaya).Style = (eDotNetBarStyle)9;
		((Control)BtnSelectMazaya).TabIndex = 38;
		((Control)BtnSelectMazaya).TabStop = false;
		((Control)BtnSelectMazaya).Text = "...";
		BtnSelectMazaya.TextColor = Color.Red;
		((Control)BtnSelectMazaya).Click += BtnSelectMazaya_Click;
		((Control)labelX31).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX31).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX31).Location = new Point(153, 119);
		((Control)labelX31).Name = "labelX31";
		((Control)labelX31).Size = new Size(75, 21);
		((Control)labelX31).TabIndex = 37;
		((Control)labelX31).Text = "نوع تشویق:";
		labelX31.TextAlignment = (StringAlignment)2;
		((BaseItemControl)ChkTashvighShift).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChkTashvighShift).Font = new Font("Tahoma", 8f, (FontStyle)1);
		((Control)ChkTashvighShift).Location = new Point(11, 97);
		((Control)ChkTashvighShift).Name = "ChkTashvighShift";
		((Control)ChkTashvighShift).Size = new Size(220, 28);
		((BaseItemControl)ChkTashvighShift).Style = (eDotNetBarStyle)9;
		((Control)ChkTashvighShift).TabIndex = 36;
		((Control)ChkTashvighShift).Text = "مشوق حضور در شیفت";
		((BaseItemControl)labelX30).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX30).Location = new Point(202, 163);
		((Control)labelX30).Name = "labelX30";
		((Control)labelX30).Size = new Size(90, 21);
		((Control)labelX30).TabIndex = 35;
		((Control)labelX30).Text = "نوع تردد شیفت:";
		labelX30.TextAlignment = (StringAlignment)2;
		((Control)CmbTypeTaradodInShift).Anchor = (AnchorStyles)13;
		((ListControl)CmbTypeTaradodInShift).DisplayMember = "Name";
		((ComboBox)CmbTypeTaradodInShift).DrawMode = (DrawMode)1;
		((ComboBox)CmbTypeTaradodInShift).DropDownStyle = (ComboBoxStyle)2;
		((Control)CmbTypeTaradodInShift).Font = new Font("Tahoma", 9f, (FontStyle)1);
		((ListControl)CmbTypeTaradodInShift).FormattingEnabled = true;
		((ComboBox)CmbTypeTaradodInShift).ItemHeight = 17;
		CmbTypeTaradodInShift.Items.AddRange(new object[5] { Normal, ShiftWork1, ShiftWork2, ShiftWork3, NightWork });
		((Control)CmbTypeTaradodInShift).Location = new Point(6, 163);
		((Control)CmbTypeTaradodInShift).Name = "CmbTypeTaradodInShift";
		((Control)CmbTypeTaradodInShift).Size = new Size(191, 23);
		CmbTypeTaradodInShift.Style = (eDotNetBarStyle)9;
		((Control)CmbTypeTaradodInShift).TabIndex = 34;
		((ListControl)CmbTypeTaradodInShift).ValueMember = "Id";
		Normal.Text = "عادی";
		Normal.Value = "0";
		ShiftWork1.Text = "نوبت کاری1";
		ShiftWork1.Value = "1";
		ShiftWork2.Text = "نوبت کاری2";
		ShiftWork2.Value = "2";
		ShiftWork3.Text = "نوبت کاری3";
		ShiftWork3.Value = "3";
		NightWork.Text = "شبکاری";
		NightWork.Value = "4";
		((BaseItemControl)labelX28).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX28).Location = new Point(75, 5);
		((Control)labelX28).Name = "labelX28";
		((Control)labelX28).Size = new Size(91, 21);
		((Control)labelX28).TabIndex = 32;
		((Control)labelX28).Text = "[دقیقه می باشد]";
		((BaseItemControl)ChkVacationInLunch).BackgroundStyle.CornerType = (eCornerType)1;
		ChkVacationInLunch.Checked = true;
		ChkVacationInLunch.CheckState = (CheckState)1;
		ChkVacationInLunch.CheckValue = "Y";
		((Control)ChkVacationInLunch).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)ChkVacationInLunch).Location = new Point(28, 32);
		((Control)ChkVacationInLunch).Name = "ChkVacationInLunch";
		((Control)ChkVacationInLunch).Size = new Size(502, 21);
		((BaseItemControl)ChkVacationInLunch).Style = (eDotNetBarStyle)9;
		((Control)ChkVacationInLunch).TabIndex = 31;
		((Control)ChkVacationInLunch).Text = "[بازه استراحت بین مرخصی/ماموریت جزء مرخصی/ماموریت محاسبه گردد]";
		((Control)btnHelpLunchTime).AccessibleRole = (AccessibleRole)43;
		((Control)btnHelpLunchTime).Font = new Font("Tahoma", 15f, (FontStyle)1, (GraphicsUnit)2, (byte)0);
		((Control)btnHelpLunchTime).Location = new Point(3, 5);
		((Control)btnHelpLunchTime).Name = "btnHelpLunchTime";
		((Control)btnHelpLunchTime).Size = new Size(20, 21);
		((PopupItemControl)btnHelpLunchTime).Style = (eDotNetBarStyle)9;
		((Control)btnHelpLunchTime).TabIndex = 30;
		((Control)btnHelpLunchTime).TabStop = false;
		((Control)btnHelpLunchTime).Text = "?";
		btnHelpLunchTime.TextColor = Color.Red;
		((Control)btnHelpLunchTime).Click += btnHelpLunchTime_Click;
		((BaseItemControl)labelX21).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX21).Location = new Point(359, 5);
		((Control)labelX21).Name = "labelX21";
		((Control)labelX21).Size = new Size(171, 21);
		((Control)labelX21).TabIndex = 26;
		((Control)labelX21).Text = "[شروع بازه استراحت از ساعت ]";
		labelX21.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX19).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX19).Location = new Point(221, 5);
		((Control)labelX19).Name = "labelX19";
		((Control)labelX19).Size = new Size(50, 21);
		((Control)labelX19).TabIndex = 24;
		((Control)labelX19).Text = "[به مدت]";
		labelX19.TextAlignment = (StringAlignment)2;
		((Control)txtLunchTime).Location = new Point(169, 5);
		txtLunchTime.Maximum = new decimal(new int[4] { 240, 0, 0, 0 });
		((Control)txtLunchTime).Name = "txtLunchTime";
		((Control)txtLunchTime).Size = new Size(44, 21);
		((Control)txtLunchTime).TabIndex = 25;
		((UpDownBase)txtLunchTime).TextAlign = (HorizontalAlignment)2;
		((Control)txtMinStartLunch).Location = new Point(320, 5);
		txtMinStartLunch.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)txtMinStartLunch).Name = "txtMinStartLunch";
		((Control)txtMinStartLunch).Size = new Size(33, 21);
		((Control)txtMinStartLunch).TabIndex = 27;
		((UpDownBase)txtMinStartLunch).TextAlign = (HorizontalAlignment)2;
		((Control)txtHurStartLunch).Location = new Point(278, 5);
		txtHurStartLunch.Maximum = new decimal(new int[4] { 23, 0, 0, 0 });
		((Control)txtHurStartLunch).Name = "txtHurStartLunch";
		((Control)txtHurStartLunch).Size = new Size(33, 21);
		((Control)txtHurStartLunch).TabIndex = 29;
		((UpDownBase)txtHurStartLunch).TextAlign = (HorizontalAlignment)2;
		((BaseItemControl)labelX20).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX20).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX20).Location = new Point(310, 5);
		((Control)labelX20).Name = "labelX20";
		((Control)labelX20).Size = new Size(10, 21);
		((Control)labelX20).TabIndex = 28;
		((Control)labelX20).Text = ":";
		labelX20.TextAlignment = (StringAlignment)1;
		((BaseItemControl)ChkCalcEnteghalInSrcDate).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChkCalcEnteghalInSrcDate).Font = new Font("Tahoma", 8f, (FontStyle)1);
		((Control)ChkCalcEnteghalInSrcDate).Location = new Point(310, 97);
		((Control)ChkCalcEnteghalInSrcDate).Name = "ChkCalcEnteghalInSrcDate";
		((Control)ChkCalcEnteghalInSrcDate).Size = new Size(220, 28);
		((BaseItemControl)ChkCalcEnteghalInSrcDate).Style = (eDotNetBarStyle)9;
		((Control)ChkCalcEnteghalInSrcDate).TabIndex = 23;
		((Control)ChkCalcEnteghalInSrcDate).Text = "[محاسبه تردد انتقال یافته در روز مبدا]";
		((BaseItemControl)ChbNextDay).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbNextDay).Font = new Font("Tahoma", 8f, (FontStyle)1);
		((Control)ChbNextDay).Location = new Point(168, 63);
		((Control)ChbNextDay).Name = "ChbNextDay";
		((Control)ChbNextDay).Size = new Size(362, 28);
		((BaseItemControl)ChbNextDay).Style = (eDotNetBarStyle)9;
		((Control)ChbNextDay).TabIndex = 8;
		((Control)ChbNextDay).Text = "[انتقال به روز بعد اعمال شود در صورتیکه تردد روز بعد قبل از ساعت ]";
		ChbNextDay.CheckedChanged += ChbNextDay_CheckedChanged;
		((Control)HlBLMaxTimeCanExit).Enabled = false;
		((Control)HlBLMaxTimeCanExit).Location = new Point(86, 67);
		HlBLMaxTimeCanExit.Maximum = new decimal(new int[4] { 23, 0, 0, 0 });
		((Control)HlBLMaxTimeCanExit).Name = "HlBLMaxTimeCanExit";
		((Control)HlBLMaxTimeCanExit).Size = new Size(33, 21);
		((Control)HlBLMaxTimeCanExit).TabIndex = 3;
		((UpDownBase)HlBLMaxTimeCanExit).TextAlign = (HorizontalAlignment)2;
		((Control)BtnHelp).AccessibleRole = (AccessibleRole)43;
		((Control)BtnHelp).Font = new Font("Tahoma", 15f, (FontStyle)1, (GraphicsUnit)2);
		((Control)BtnHelp).Location = new Point(3, 67);
		((Control)BtnHelp).Name = "BtnHelp";
		((Control)BtnHelp).Size = new Size(20, 21);
		((PopupItemControl)BtnHelp).Style = (eDotNetBarStyle)9;
		((Control)BtnHelp).TabIndex = 5;
		((Control)BtnHelp).TabStop = false;
		((Control)BtnHelp).Text = "?";
		BtnHelp.TextColor = Color.Red;
		((Control)BtnHelp).Click += BtnHelp_Click;
		((BaseItemControl)labelX17).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX17).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX17).Location = new Point(120, 67);
		((Control)labelX17).Name = "labelX17";
		((Control)labelX17).Size = new Size(9, 21);
		((Control)labelX17).TabIndex = 2;
		((Control)labelX17).Text = ":";
		labelX17.TextAlignment = (StringAlignment)1;
		((Control)MlBLMaxTimeCanExit).Enabled = false;
		((Control)MlBLMaxTimeCanExit).Location = new Point(128, 67);
		MlBLMaxTimeCanExit.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)MlBLMaxTimeCanExit).Name = "MlBLMaxTimeCanExit";
		((Control)MlBLMaxTimeCanExit).Size = new Size(34, 21);
		((Control)MlBLMaxTimeCanExit).TabIndex = 1;
		((UpDownBase)MlBLMaxTimeCanExit).TextAlign = (HorizontalAlignment)2;
		((PanelControl)grpPanelPeakOvertime).CanvasColor = SystemColors.Control;
		((PanelControl)grpPanelPeakOvertime).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)grpPanelPeakOvertime).Controls.Add((Control)(object)MinutePeakOvertime);
		((Control)grpPanelPeakOvertime).Controls.Add((Control)(object)HourPeakOvertime);
		((Control)grpPanelPeakOvertime).Controls.Add((Control)(object)labelX18);
		grpPanelPeakOvertime.DisabledBackColor = Color.Empty;
		((Control)grpPanelPeakOvertime).Location = new Point(318, 132);
		((Control)grpPanelPeakOvertime).Name = "grpPanelPeakOvertime";
		((Control)grpPanelPeakOvertime).Size = new Size(212, 54);
		((PanelControl)grpPanelPeakOvertime).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)grpPanelPeakOvertime).Style.BackColorGradientAngle = 90;
		((PanelControl)grpPanelPeakOvertime).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)grpPanelPeakOvertime).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)grpPanelPeakOvertime).Style.BorderBottomWidth = 1;
		((PanelControl)grpPanelPeakOvertime).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)grpPanelPeakOvertime).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)grpPanelPeakOvertime).Style.BorderLeftWidth = 1;
		((PanelControl)grpPanelPeakOvertime).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)grpPanelPeakOvertime).Style.BorderRightWidth = 1;
		((PanelControl)grpPanelPeakOvertime).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)grpPanelPeakOvertime).Style.BorderTopWidth = 1;
		((PanelControl)grpPanelPeakOvertime).Style.CornerDiameter = 4;
		((PanelControl)grpPanelPeakOvertime).Style.CornerType = (eCornerType)2;
		((PanelControl)grpPanelPeakOvertime).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)grpPanelPeakOvertime).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)grpPanelPeakOvertime).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)grpPanelPeakOvertime).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)grpPanelPeakOvertime).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)grpPanelPeakOvertime).TabIndex = 4;
		((Control)grpPanelPeakOvertime).Text = "[سقف اضافه کاری پایان شیفت تا ساعت]";
		((Control)MinutePeakOvertime).Anchor = (AnchorStyles)9;
		((Control)MinutePeakOvertime).Location = new Point(128, 3);
		MinutePeakOvertime.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)MinutePeakOvertime).Name = "MinutePeakOvertime";
		((Control)MinutePeakOvertime).Size = new Size(71, 21);
		((Control)MinutePeakOvertime).TabIndex = 0;
		((UpDownBase)MinutePeakOvertime).TextAlign = (HorizontalAlignment)2;
		((Control)HourPeakOvertime).Anchor = (AnchorStyles)9;
		((Control)HourPeakOvertime).Location = new Point(34, 3);
		HourPeakOvertime.Maximum = new decimal(new int[4] { 23, 0, 0, 0 });
		((Control)HourPeakOvertime).Name = "HourPeakOvertime";
		((Control)HourPeakOvertime).Size = new Size(71, 21);
		((Control)HourPeakOvertime).TabIndex = 2;
		((UpDownBase)HourPeakOvertime).TextAlign = (HorizontalAlignment)2;
		((Control)labelX18).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX18).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX18).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX18).Location = new Point(111, 3);
		((Control)labelX18).Name = "labelX18";
		((Control)labelX18).Size = new Size(21, 21);
		((Control)labelX18).TabIndex = 1;
		((Control)labelX18).Text = ":";
		labelX18.TextAlignment = (StringAlignment)1;
		((BaseItemControl)labelX26).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX26).Location = new Point(28, 68);
		((Control)labelX26).Name = "labelX26";
		((Control)labelX26).Size = new Size(52, 18);
		((Control)labelX26).TabIndex = 22;
		((Control)labelX26).Text = "[باشد]";
		((PanelControl)grpPanelNormal1).CanvasColor = SystemColors.Control;
		((PanelControl)grpPanelNormal1).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)grpPanelNormal1).Controls.Add((Control)(object)labelX1);
		((Control)grpPanelNormal1).Controls.Add((Control)(object)labelX15);
		((Control)grpPanelNormal1).Controls.Add((Control)(object)Hour1);
		((Control)grpPanelNormal1).Controls.Add((Control)(object)lblTajilV);
		((Control)grpPanelNormal1).Controls.Add((Control)(object)lblTakhirV);
		((Control)grpPanelNormal1).Controls.Add((Control)(object)Min1);
		((Control)grpPanelNormal1).Controls.Add((Control)(object)timeVorod2);
		((Control)grpPanelNormal1).Controls.Add((Control)(object)TimeVorod1);
		((Control)grpPanelNormal1).Controls.Add((Control)(object)labelX7);
		((Control)grpPanelNormal1).Controls.Add((Control)(object)labelX3);
		((Control)grpPanelNormal1).Controls.Add((Control)(object)TakhirV);
		((Control)grpPanelNormal1).Controls.Add((Control)(object)labelX12);
		((Control)grpPanelNormal1).Controls.Add((Control)(object)TajilV);
		((Control)grpPanelNormal1).Controls.Add((Control)(object)labelX4);
		grpPanelNormal1.DisabledBackColor = Color.Empty;
		((Control)grpPanelNormal1).Location = new Point(274, 62);
		((Control)grpPanelNormal1).Name = "grpPanelNormal1";
		((Control)grpPanelNormal1).Size = new Size(266, 131);
		((PanelControl)grpPanelNormal1).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)grpPanelNormal1).Style.BackColorGradientAngle = 90;
		((PanelControl)grpPanelNormal1).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)grpPanelNormal1).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)grpPanelNormal1).Style.BorderBottomWidth = 1;
		((PanelControl)grpPanelNormal1).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)grpPanelNormal1).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)grpPanelNormal1).Style.BorderLeftWidth = 1;
		((PanelControl)grpPanelNormal1).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)grpPanelNormal1).Style.BorderRightWidth = 1;
		((PanelControl)grpPanelNormal1).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)grpPanelNormal1).Style.BorderTopWidth = 1;
		((PanelControl)grpPanelNormal1).Style.CornerDiameter = 4;
		((PanelControl)grpPanelNormal1).Style.CornerType = (eCornerType)2;
		((PanelControl)grpPanelNormal1).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)grpPanelNormal1).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)grpPanelNormal1).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)grpPanelNormal1).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)grpPanelNormal1).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)grpPanelNormal1).TabIndex = 1;
		((Control)grpPanelNormal1).Text = "[ساعت شروع]";
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX1).Location = new Point(132, 84);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(10, 21);
		((Control)labelX1).TabIndex = 13;
		((Control)labelX1).Text = ":";
		labelX1.TextAlignment = (StringAlignment)1;
		((Control)labelX15).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX15).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX15).ForeColor = SystemColors.ControlText;
		((Control)labelX15).Location = new Point(56, 84);
		((Control)labelX15).Name = "labelX15";
		((Control)labelX15).Size = new Size(15, 23);
		((Control)labelX15).TabIndex = 11;
		((Control)labelX15).Text = "[تا] :";
		labelX15.TextAlignment = (StringAlignment)2;
		((Control)Hour1).Anchor = (AnchorStyles)9;
		((Control)Hour1).Location = new Point(55, 3);
		Hour1.Maximum = new decimal(new int[4] { 23, 0, 0, 0 });
		((Control)Hour1).Name = "Hour1";
		((Control)Hour1).Size = new Size(71, 21);
		((Control)Hour1).TabIndex = 2;
		((UpDownBase)Hour1).TextAlign = (HorizontalAlignment)2;
		Hour1.ValueChanged += Hour1_ValueChanged;
		((Control)lblTajilV).Anchor = (AnchorStyles)9;
		((BaseItemControl)lblTajilV).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblTajilV).Location = new Point(148, 57);
		((Control)lblTajilV).Name = "lblTajilV";
		((Control)lblTajilV).Size = new Size(100, 21);
		((Control)lblTajilV).TabIndex = 6;
		((Control)lblTajilV).Text = "[تعجیل در ورود ]";
		lblTajilV.TextAlignment = (StringAlignment)2;
		((Control)lblTakhirV).Anchor = (AnchorStyles)9;
		((BaseItemControl)lblTakhirV).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblTakhirV).Location = new Point(148, 30);
		((Control)lblTakhirV).Name = "lblTakhirV";
		((Control)lblTakhirV).Size = new Size(97, 21);
		((Control)lblTakhirV).TabIndex = 3;
		((Control)lblTakhirV).Text = " [تاخیر در ورود]";
		lblTakhirV.TextAlignment = (StringAlignment)2;
		((Control)Min1).Anchor = (AnchorStyles)9;
		((Control)Min1).Location = new Point(150, 3);
		Min1.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)Min1).Name = "Min1";
		((Control)Min1).Size = new Size(71, 21);
		((Control)Min1).TabIndex = 0;
		((UpDownBase)Min1).TextAlign = (HorizontalAlignment)2;
		Min1.ValueChanged += Hour1_ValueChanged;
		((Control)timeVorod2).Anchor = (AnchorStyles)9;
		((BaseItemControl)timeVorod2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)timeVorod2).ForeColor = Color.DarkRed;
		((Control)timeVorod2).Location = new Point(-34, 84);
		((Control)timeVorod2).Name = "timeVorod2";
		((Control)timeVorod2).Size = new Size(65, 23);
		((Control)timeVorod2).TabIndex = 12;
		((Control)timeVorod2).Text = "------";
		((Control)TimeVorod1).Anchor = (AnchorStyles)9;
		((BaseItemControl)TimeVorod1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)TimeVorod1).ForeColor = Color.DarkRed;
		((Control)TimeVorod1).Location = new Point(55, 84);
		((Control)TimeVorod1).Name = "TimeVorod1";
		((Control)TimeVorod1).Size = new Size(68, 23);
		((Control)TimeVorod1).TabIndex = 10;
		((Control)TimeVorod1).Text = "-----";
		((Control)labelX7).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX7).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX7).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX7).Location = new Point(132, 3);
		((Control)labelX7).Name = "labelX7";
		((Control)labelX7).Size = new Size(10, 21);
		((Control)labelX7).TabIndex = 1;
		((Control)labelX7).Text = ":";
		labelX7.TextAlignment = (StringAlignment)1;
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX3).Location = new Point(132, 57);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(10, 21);
		((Control)labelX3).TabIndex = 7;
		((Control)labelX3).Text = ":";
		labelX3.TextAlignment = (StringAlignment)1;
		((Control)TakhirV).Anchor = (AnchorStyles)9;
		((Control)TakhirV).Location = new Point(55, 30);
		TakhirV.Maximum = new decimal(new int[4] { 1000, 0, 0, 0 });
		((Control)TakhirV).Name = "TakhirV";
		((Control)TakhirV).Size = new Size(71, 21);
		((Control)TakhirV).TabIndex = 5;
		((UpDownBase)TakhirV).TextAlign = (HorizontalAlignment)2;
		TakhirV.ValueChanged += Hour1_ValueChanged;
		((Control)labelX12).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX12).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX12).ForeColor = SystemColors.ControlText;
		((Control)labelX12).Location = new Point(148, 84);
		((Control)labelX12).Name = "labelX12";
		((Control)labelX12).Size = new Size(86, 23);
		((Control)labelX12).TabIndex = 9;
		((Control)labelX12).Text = "[بازه ورود از]";
		labelX12.TextAlignment = (StringAlignment)2;
		((Control)TajilV).Anchor = (AnchorStyles)9;
		((Control)TajilV).Location = new Point(55, 57);
		TajilV.Maximum = new decimal(new int[4] { 1000, 0, 0, 0 });
		((Control)TajilV).Name = "TajilV";
		((Control)TajilV).Size = new Size(71, 21);
		((Control)TajilV).TabIndex = 8;
		((UpDownBase)TajilV).TextAlign = (HorizontalAlignment)2;
		TajilV.ValueChanged += Hour1_ValueChanged;
		((Control)labelX4).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX4).Location = new Point(132, 30);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(10, 21);
		((Control)labelX4).TabIndex = 4;
		((Control)labelX4).Text = ":";
		labelX4.TextAlignment = (StringAlignment)1;
		((PanelControl)grpPanelNormal2).CanvasColor = SystemColors.Control;
		((PanelControl)grpPanelNormal2).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)grpPanelNormal2).Controls.Add((Control)(object)labelX5);
		((Control)grpPanelNormal2).Controls.Add((Control)(object)labelX10);
		((Control)grpPanelNormal2).Controls.Add((Control)(object)labelX9);
		((Control)grpPanelNormal2).Controls.Add((Control)(object)lblTakhirKh);
		((Control)grpPanelNormal2).Controls.Add((Control)(object)TimeKhoroj2);
		((Control)grpPanelNormal2).Controls.Add((Control)(object)labelX16);
		((Control)grpPanelNormal2).Controls.Add((Control)(object)lblTajilKh);
		((Control)grpPanelNormal2).Controls.Add((Control)(object)TakhirKh);
		((Control)grpPanelNormal2).Controls.Add((Control)(object)TajilKh);
		((Control)grpPanelNormal2).Controls.Add((Control)(object)TimeKhoroj1);
		((Control)grpPanelNormal2).Controls.Add((Control)(object)Min2);
		((Control)grpPanelNormal2).Controls.Add((Control)(object)labelX2);
		((Control)grpPanelNormal2).Controls.Add((Control)(object)Hour2);
		((Control)grpPanelNormal2).Controls.Add((Control)(object)labelX13);
		grpPanelNormal2.DisabledBackColor = Color.Empty;
		((Control)grpPanelNormal2).Location = new Point(4, 62);
		((Control)grpPanelNormal2).Name = "grpPanelNormal2";
		((Control)grpPanelNormal2).Size = new Size(264, 131);
		((PanelControl)grpPanelNormal2).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)grpPanelNormal2).Style.BackColorGradientAngle = 90;
		((PanelControl)grpPanelNormal2).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)grpPanelNormal2).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)grpPanelNormal2).Style.BorderBottomWidth = 1;
		((PanelControl)grpPanelNormal2).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)grpPanelNormal2).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)grpPanelNormal2).Style.BorderLeftWidth = 1;
		((PanelControl)grpPanelNormal2).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)grpPanelNormal2).Style.BorderRightWidth = 1;
		((PanelControl)grpPanelNormal2).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)grpPanelNormal2).Style.BorderTopWidth = 1;
		((PanelControl)grpPanelNormal2).Style.CornerDiameter = 4;
		((PanelControl)grpPanelNormal2).Style.CornerType = (eCornerType)2;
		((PanelControl)grpPanelNormal2).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)grpPanelNormal2).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)grpPanelNormal2).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)grpPanelNormal2).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)grpPanelNormal2).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)grpPanelNormal2).TabIndex = 2;
		((Control)grpPanelNormal2).Text = "[ساعت پایان]";
		((Control)labelX5).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX5).Location = new Point(135, 30);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(12, 21);
		((Control)labelX5).TabIndex = 4;
		((Control)labelX5).Text = ":";
		labelX5.TextAlignment = (StringAlignment)1;
		((Control)labelX10).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX10).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX10).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX10).Location = new Point(135, 84);
		((Control)labelX10).Name = "labelX10";
		((Control)labelX10).Size = new Size(12, 21);
		((Control)labelX10).TabIndex = 7;
		((Control)labelX10).Text = ":";
		labelX10.TextAlignment = (StringAlignment)1;
		((Control)labelX9).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX9).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX9).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX9).Location = new Point(135, 57);
		((Control)labelX9).Name = "labelX9";
		((Control)labelX9).Size = new Size(12, 21);
		((Control)labelX9).TabIndex = 7;
		((Control)labelX9).Text = ":";
		labelX9.TextAlignment = (StringAlignment)1;
		((Control)lblTakhirKh).Anchor = (AnchorStyles)9;
		((BaseItemControl)lblTakhirKh).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblTakhirKh).Location = new Point(152, 30);
		((Control)lblTakhirKh).Name = "lblTakhirKh";
		((Control)lblTakhirKh).Size = new Size(96, 21);
		((Control)lblTakhirKh).TabIndex = 3;
		((Control)lblTakhirKh).Text = "[تاخیر در خروج ]";
		lblTakhirKh.TextAlignment = (StringAlignment)2;
		((Control)TimeKhoroj2).Anchor = (AnchorStyles)9;
		((BaseItemControl)TimeKhoroj2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)TimeKhoroj2).ForeColor = Color.DarkRed;
		((Control)TimeKhoroj2).Location = new Point(-6, 84);
		((Control)TimeKhoroj2).Name = "TimeKhoroj2";
		((Control)TimeKhoroj2).Size = new Size(52, 21);
		((Control)TimeKhoroj2).TabIndex = 12;
		((Control)TimeKhoroj2).Text = "-------";
		((Control)labelX16).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX16).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX16).ForeColor = SystemColors.ControlText;
		((Control)labelX16).Location = new Point(68, 84);
		((Control)labelX16).Name = "labelX16";
		((Control)labelX16).Size = new Size(15, 21);
		((Control)labelX16).TabIndex = 11;
		((Control)labelX16).Text = "[تا] :";
		labelX16.TextAlignment = (StringAlignment)2;
		((Control)lblTajilKh).Anchor = (AnchorStyles)9;
		((BaseItemControl)lblTajilKh).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblTajilKh).Location = new Point(152, 57);
		((Control)lblTajilKh).Name = "lblTajilKh";
		((Control)lblTajilKh).Size = new Size(96, 21);
		((Control)lblTajilKh).TabIndex = 6;
		((Control)lblTajilKh).Text = "[تعجیل درخروج]";
		lblTajilKh.TextAlignment = (StringAlignment)2;
		((Control)TakhirKh).Anchor = (AnchorStyles)9;
		((Control)TakhirKh).Location = new Point(59, 30);
		TakhirKh.Maximum = new decimal(new int[4] { 1000, 0, 0, 0 });
		((Control)TakhirKh).Name = "TakhirKh";
		((Control)TakhirKh).Size = new Size(71, 21);
		((Control)TakhirKh).TabIndex = 5;
		((UpDownBase)TakhirKh).TextAlign = (HorizontalAlignment)2;
		TakhirKh.ValueChanged += Hour1_ValueChanged;
		((Control)TajilKh).Anchor = (AnchorStyles)9;
		((Control)TajilKh).Location = new Point(59, 57);
		TajilKh.Maximum = new decimal(new int[4] { 1000, 0, 0, 0 });
		((Control)TajilKh).Name = "TajilKh";
		((Control)TajilKh).Size = new Size(71, 21);
		((Control)TajilKh).TabIndex = 8;
		((UpDownBase)TajilKh).TextAlign = (HorizontalAlignment)2;
		TajilKh.ValueChanged += Hour1_ValueChanged;
		((Control)TimeKhoroj1).Anchor = (AnchorStyles)9;
		((BaseItemControl)TimeKhoroj1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)TimeKhoroj1).ForeColor = Color.DarkRed;
		((Control)TimeKhoroj1).Location = new Point(74, 84);
		((Control)TimeKhoroj1).Name = "TimeKhoroj1";
		((Control)TimeKhoroj1).Size = new Size(56, 21);
		((Control)TimeKhoroj1).TabIndex = 10;
		((Control)TimeKhoroj1).Text = "------";
		((Control)Min2).Anchor = (AnchorStyles)9;
		((Control)Min2).Location = new Point(152, 3);
		Min2.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)Min2).Name = "Min2";
		((Control)Min2).Size = new Size(71, 21);
		((Control)Min2).TabIndex = 0;
		((UpDownBase)Min2).TextAlign = (HorizontalAlignment)2;
		Min2.ValueChanged += Hour1_ValueChanged;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX2).Location = new Point(135, 3);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(12, 21);
		((Control)labelX2).TabIndex = 1;
		((Control)labelX2).Text = ":";
		labelX2.TextAlignment = (StringAlignment)1;
		((Control)Hour2).Anchor = (AnchorStyles)9;
		((Control)Hour2).Location = new Point(59, 3);
		Hour2.Maximum = new decimal(new int[4] { 23, 0, 0, 0 });
		((Control)Hour2).Name = "Hour2";
		((Control)Hour2).Size = new Size(71, 21);
		((Control)Hour2).TabIndex = 2;
		((UpDownBase)Hour2).TextAlign = (HorizontalAlignment)2;
		Hour2.ValueChanged += Hour1_ValueChanged;
		((Control)labelX13).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX13).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX13).ForeColor = SystemColors.ControlText;
		((Control)labelX13).Location = new Point(152, 84);
		((Control)labelX13).Name = "labelX13";
		((Control)labelX13).Size = new Size(96, 21);
		((Control)labelX13).TabIndex = 9;
		((Control)labelX13).Text = "[بازه خروج از]";
		labelX13.TextAlignment = (StringAlignment)2;
		((Control)PnlNormal1).Controls.Add((Control)(object)btnHurryDelayPerform);
		((Control)PnlNormal1).Controls.Add((Control)(object)chbxHurryDelayPerform);
		((Control)PnlNormal1).Controls.Add((Control)(object)labelX27);
		((Control)PnlNormal1).Controls.Add((Control)(object)BtnJarimeInfo);
		((Control)PnlNormal1).Controls.Add((Control)(object)JarimeV);
		((Control)PnlNormal1).Controls.Add((Control)(object)ChkJarime);
		((Control)PnlNormal1).Location = new Point(4, 200);
		((Control)PnlNormal1).Name = "PnlNormal1";
		((Control)PnlNormal1).Size = new Size(536, 63);
		((Control)PnlNormal1).TabIndex = 20;
		((Control)btnHurryDelayPerform).AccessibleRole = (AccessibleRole)43;
		((Control)btnHurryDelayPerform).Font = new Font("Tahoma", 15f, (FontStyle)1, (GraphicsUnit)2);
		((Control)btnHurryDelayPerform).Location = new Point(3, 30);
		((Control)btnHurryDelayPerform).Name = "btnHurryDelayPerform";
		((Control)btnHurryDelayPerform).Size = new Size(20, 21);
		((PopupItemControl)btnHurryDelayPerform).Style = (eDotNetBarStyle)9;
		((Control)btnHurryDelayPerform).TabIndex = 23;
		((Control)btnHurryDelayPerform).TabStop = false;
		((Control)btnHurryDelayPerform).Text = "?";
		btnHurryDelayPerform.TextColor = Color.Red;
		((Control)btnHurryDelayPerform).Click += btnHurryDelayPerform_Click;
		((Control)chbxHurryDelayPerform).Anchor = (AnchorStyles)9;
		((BaseItemControl)chbxHurryDelayPerform).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chbxHurryDelayPerform).Location = new Point(28, 28);
		((Control)chbxHurryDelayPerform).Name = "chbxHurryDelayPerform";
		((Control)chbxHurryDelayPerform).Size = new Size(503, 23);
		((BaseItemControl)chbxHurryDelayPerform).Style = (eDotNetBarStyle)9;
		((Control)chbxHurryDelayPerform).TabIndex = 22;
		((Control)chbxHurryDelayPerform).Text = "[کارکرد بر اساس تعجیل و تاخیر در زمان ورود و خروج محاسبه شود]";
		((Control)labelX27).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX27).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX27).Location = new Point(43, 4);
		((Control)labelX27).Name = "labelX27";
		((Control)labelX27).Size = new Size(250, 21);
		((Control)labelX27).TabIndex = 14;
		((Control)labelX27).Text = "[دقیقه،  جریمه لحاظ گردد]";
		((Control)BtnJarimeInfo).AccessibleRole = (AccessibleRole)43;
		((Control)BtnJarimeInfo).Font = new Font("Tahoma", 15f, (FontStyle)1, (GraphicsUnit)2);
		((Control)BtnJarimeInfo).Location = new Point(3, 3);
		((Control)BtnJarimeInfo).Name = "BtnJarimeInfo";
		((Control)BtnJarimeInfo).Size = new Size(20, 21);
		((PopupItemControl)BtnJarimeInfo).Style = (eDotNetBarStyle)9;
		((Control)BtnJarimeInfo).TabIndex = 1;
		((Control)BtnJarimeInfo).TabStop = false;
		((Control)BtnJarimeInfo).Text = "?";
		BtnJarimeInfo.TextColor = Color.Red;
		((Control)BtnJarimeInfo).Click += BtnJarimeInfo_Click;
		((Control)JarimeV).Anchor = (AnchorStyles)9;
		((Control)JarimeV).Enabled = false;
		((Control)JarimeV).Location = new Point(298, 4);
		JarimeV.Maximum = new decimal(new int[4] { 300, 0, 0, 0 });
		((Control)JarimeV).Name = "JarimeV";
		((Control)JarimeV).Size = new Size(49, 21);
		((Control)JarimeV).TabIndex = 21;
		((UpDownBase)JarimeV).TextAlign = (HorizontalAlignment)2;
		JarimeV.Value = new decimal(new int[4] { 30, 0, 0, 0 });
		((Control)ChkJarime).Anchor = (AnchorStyles)9;
		((BaseItemControl)ChkJarime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChkJarime).Location = new Point(353, 1);
		((Control)ChkJarime).Name = "ChkJarime";
		((Control)ChkJarime).Size = new Size(178, 23);
		((Control)ChkJarime).TabIndex = 0;
		((Control)ChkJarime).Text = "[در صورت تاخیر ورود بیش از  ]";
		ChkJarime.CheckedChanged += ChkJarime_CheckedChanged;
		((PanelControl)groupPanel4).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel4).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel4).Controls.Add((Control)(object)RBSurcease);
		((Control)groupPanel4).Controls.Add((Control)(object)RBNormalShift);
		((Control)groupPanel4).Controls.Add((Control)(object)RBShenavarShift);
		groupPanel4.DisabledBackColor = Color.Empty;
		((Control)groupPanel4).Location = new Point(3, 2);
		((Control)groupPanel4).Name = "groupPanel4";
		((Control)groupPanel4).Size = new Size(537, 53);
		((PanelControl)groupPanel4).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel4).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel4).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel4).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel4).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel4).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel4).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel4).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel4).Style.CornerDiameter = 4;
		((PanelControl)groupPanel4).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel4).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel4).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel4).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel4).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel4).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel4).TabIndex = 0;
		((Control)groupPanel4).Text = "[نوع تردد]";
		((Control)RBSurcease).AutoSize = true;
		((Control)RBSurcease).Location = new Point(27, 3);
		((Control)RBSurcease).Name = "RBSurcease";
		((Control)RBSurcease).Size = new Size(120, 17);
		((Control)RBSurcease).TabIndex = 0;
		((Control)RBSurcease).Text = "[شیفت استراحت]";
		((ButtonBase)RBSurcease).UseVisualStyleBackColor = true;
		RBSurcease.CheckedChanged += RBNormalShift_CheckedChanged;
		((Control)RBNormalShift).AutoSize = true;
		RBNormalShift.Checked = true;
		((Control)RBNormalShift).Location = new Point(412, 5);
		((Control)RBNormalShift).Name = "RBNormalShift";
		((Control)RBNormalShift).Size = new Size(89, 17);
		((Control)RBNormalShift).TabIndex = 0;
		RBNormalShift.TabStop = true;
		((Control)RBNormalShift).Text = "شیفت عادی";
		((ButtonBase)RBNormalShift).UseVisualStyleBackColor = true;
		RBNormalShift.CheckedChanged += RBNormalShift_CheckedChanged;
		((Control)RBShenavarShift).AutoSize = true;
		((Control)RBShenavarShift).Location = new Point(203, 5);
		((Control)RBShenavarShift).Name = "RBShenavarShift";
		((Control)RBShenavarShift).Size = new Size(102, 17);
		((Control)RBShenavarShift).TabIndex = 1;
		((Control)RBShenavarShift).Text = "[شیفت شناور]";
		((ButtonBase)RBShenavarShift).UseVisualStyleBackColor = true;
		RBShenavarShift.CheckedChanged += RBNormalShift_CheckedChanged;
		((Control)pnlColor).Anchor = (AnchorStyles)14;
		pnlColor.CanvasColor = SystemColors.Control;
		pnlColor.ColorSchemeStyle = (eDotNetBarStyle)9;
		pnlColor.DisabledBackColor = Color.Empty;
		((Control)pnlColor).Location = new Point(4, 593);
		((Control)pnlColor).Name = "pnlColor";
		((Control)pnlColor).Size = new Size(536, 29);
		pnlColor.Style.Alignment = (StringAlignment)1;
		pnlColor.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		pnlColor.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		pnlColor.Style.Border = (eBorderType)1;
		pnlColor.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		pnlColor.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		pnlColor.Style.GradientAngle = 90;
		((Control)pnlColor).TabIndex = 15;
		((Control)pnlColor).Text = "[انتخاب رنگ شیفت]";
		((Control)pnlColor).Click += pnlColor_Click;
		((Control)labelX14).Anchor = (AnchorStyles)6;
		((BaseItemControl)labelX14).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX14).Location = new Point(396, 593);
		((Control)labelX14).Name = "labelX14";
		((Control)labelX14).Size = new Size(57, 23);
		((Control)labelX14).TabIndex = 14;
		((Control)labelX14).Text = "[رنگ شیفت] :";
		labelX14.TextAlignment = (StringAlignment)2;
		((Control)labelX8).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).Location = new Point(450, 552);
		((Control)labelX8).Name = "labelX8";
		((Control)labelX8).Size = new Size(85, 23);
		((Control)labelX8).TabIndex = 12;
		((Control)labelX8).Text = "[شرح] :";
		labelX8.TextAlignment = (StringAlignment)2;
		((PanelControl)grpPanelShenavar).CanvasColor = SystemColors.Control;
		((PanelControl)grpPanelShenavar).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)grpPanelShenavar).Controls.Add((Control)(object)MinShenavar);
		((Control)grpPanelShenavar).Controls.Add((Control)(object)HourShenavar);
		((Control)grpPanelShenavar).Controls.Add((Control)(object)labelX6);
		grpPanelShenavar.DisabledBackColor = Color.Empty;
		((Control)grpPanelShenavar).Location = new Point(7, 72);
		((Control)grpPanelShenavar).Name = "grpPanelShenavar";
		((Control)grpPanelShenavar).Size = new Size(533, 53);
		((PanelControl)grpPanelShenavar).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)grpPanelShenavar).Style.BackColorGradientAngle = 90;
		((PanelControl)grpPanelShenavar).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)grpPanelShenavar).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)grpPanelShenavar).Style.BorderBottomWidth = 1;
		((PanelControl)grpPanelShenavar).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)grpPanelShenavar).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)grpPanelShenavar).Style.BorderLeftWidth = 1;
		((PanelControl)grpPanelShenavar).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)grpPanelShenavar).Style.BorderRightWidth = 1;
		((PanelControl)grpPanelShenavar).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)grpPanelShenavar).Style.BorderTopWidth = 1;
		((PanelControl)grpPanelShenavar).Style.CornerDiameter = 4;
		((PanelControl)grpPanelShenavar).Style.CornerType = (eCornerType)2;
		((PanelControl)grpPanelShenavar).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)grpPanelShenavar).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)grpPanelShenavar).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)grpPanelShenavar).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)grpPanelShenavar).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)grpPanelShenavar).TabIndex = 1;
		((Control)grpPanelShenavar).Text = "[مدت حضور در شیفت شناور]";
		((Control)grpPanelShenavar).Visible = false;
		((Control)MinShenavar).Anchor = (AnchorStyles)9;
		((Control)MinShenavar).Location = new Point(451, 7);
		MinShenavar.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)MinShenavar).Name = "MinShenavar";
		((Control)MinShenavar).Size = new Size(71, 21);
		((Control)MinShenavar).TabIndex = 0;
		((UpDownBase)MinShenavar).TextAlign = (HorizontalAlignment)2;
		((Control)HourShenavar).Anchor = (AnchorStyles)9;
		((Control)HourShenavar).Location = new Point(358, 7);
		HourShenavar.Maximum = new decimal(new int[4] { 24, 0, 0, 0 });
		((Control)HourShenavar).Name = "HourShenavar";
		((Control)HourShenavar).Size = new Size(71, 21);
		((Control)HourShenavar).TabIndex = 2;
		((UpDownBase)HourShenavar).TextAlign = (HorizontalAlignment)2;
		((Control)labelX6).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX6).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX6).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX6).Location = new Point(434, 7);
		((Control)labelX6).Name = "labelX6";
		((Control)labelX6).Size = new Size(21, 21);
		((Control)labelX6).TabIndex = 1;
		((Control)labelX6).Text = ":";
		labelX6.TextAlignment = (StringAlignment)1;
		((Control)TxtDesc).Anchor = (AnchorStyles)15;
		TxtDesc.Border.Class = "TextBoxBorder";
		TxtDesc.Border.CornerType = (eCornerType)1;
		((Control)TxtDesc).Location = new Point(4, 555);
		((TextBoxBase)TxtDesc).Multiline = true;
		((Control)TxtDesc).Name = "TxtDesc";
		((Control)TxtDesc).Size = new Size(447, 32);
		((Control)TxtDesc).TabIndex = 13;
		((PanelControl)groupPanel5).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel5).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel5).Controls.Add((Control)(object)labelX11);
		groupPanel5.DisabledBackColor = Color.Empty;
		((Control)groupPanel5).Location = new Point(3, 3);
		((Control)groupPanel5).Name = "groupPanel5";
		((Control)groupPanel5).Size = new Size(445, 53);
		((PanelControl)groupPanel5).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel5).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel5).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel5).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel5).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel5).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel5).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel5).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel5).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel5).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel5).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel5).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel5).Style.CornerDiameter = 4;
		((PanelControl)groupPanel5).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel5).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel5).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel5).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel5).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel5).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel5).TabIndex = 6;
		((Control)groupPanel5).Text = "[نوع تردد]";
		((Control)groupPanel5).Visible = false;
		((Control)labelX11).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX11).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX11).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX11).ForeColor = Color.DarkRed;
		((Control)labelX11).Location = new Point(9, 1);
		((Control)labelX11).Name = "labelX11";
		((Control)labelX11).Size = new Size(423, 21);
		((Control)labelX11).TabIndex = 6;
		((Control)labelX11).Text = "[تردد چرخشی]";
		labelX11.TextAlignment = (StringAlignment)1;
		((PanelControl)grpPanelSurcease).CanvasColor = SystemColors.Control;
		((PanelControl)grpPanelSurcease).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)grpPanelSurcease).Controls.Add((Control)(object)labelX29);
		((Control)grpPanelSurcease).Controls.Add((Control)(object)labelX25);
		((Control)grpPanelSurcease).Controls.Add((Control)(object)labelX24);
		((Control)grpPanelSurcease).Controls.Add((Control)(object)labelX23);
		((Control)grpPanelSurcease).Controls.Add((Control)(object)labelX22);
		grpPanelSurcease.DisabledBackColor = Color.Empty;
		((Control)grpPanelSurcease).Location = new Point(0, 64);
		((Control)grpPanelSurcease).Name = "grpPanelSurcease";
		((Control)grpPanelSurcease).Size = new Size(540, 274);
		((PanelControl)grpPanelSurcease).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)grpPanelSurcease).Style.BackColorGradientAngle = 90;
		((PanelControl)grpPanelSurcease).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)grpPanelSurcease).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)grpPanelSurcease).Style.BorderBottomWidth = 1;
		((PanelControl)grpPanelSurcease).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)grpPanelSurcease).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)grpPanelSurcease).Style.BorderLeftWidth = 1;
		((PanelControl)grpPanelSurcease).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)grpPanelSurcease).Style.BorderRightWidth = 1;
		((PanelControl)grpPanelSurcease).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)grpPanelSurcease).Style.BorderTopWidth = 1;
		((PanelControl)grpPanelSurcease).Style.CornerDiameter = 4;
		((PanelControl)grpPanelSurcease).Style.CornerType = (eCornerType)2;
		((PanelControl)grpPanelSurcease).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)grpPanelSurcease).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)grpPanelSurcease).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)grpPanelSurcease).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)grpPanelSurcease).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)grpPanelSurcease).TabIndex = 18;
		((Control)grpPanelSurcease).Text = "[توضیحات شیفت استراحت]";
		((Control)grpPanelSurcease).Visible = false;
		((Control)labelX29).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX29).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX29).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX29).ForeColor = Color.DarkRed;
		((Control)labelX29).Location = new Point(92, 179);
		((Control)labelX29).Name = "labelX29";
		((Control)labelX29).Size = new Size(432, 44);
		((Control)labelX29).TabIndex = 17;
		((Control)labelX29).Text = "[محسوب می شود]";
		labelX29.WordWrap = true;
		((Control)labelX25).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX25).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX25).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX25).ForeColor = Color.DarkRed;
		((Control)labelX25).Location = new Point(94, 136);
		((Control)labelX25).Name = "labelX25";
		((Control)labelX25).Size = new Size(432, 44);
		((Control)labelX25).TabIndex = 16;
		((Control)labelX25).Text = "[و همچنین اگر در این روز کارگر حضور داشته باشد بعنوان اضافه کار ]";
		labelX25.WordWrap = true;
		((Control)labelX24).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX24).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX24).Font = new Font("Tahoma", 10f, (FontStyle)1);
		((Control)labelX24).ForeColor = Color.DarkRed;
		((Control)labelX24).Location = new Point(12, 99);
		((Control)labelX24).Name = "labelX24";
		((Control)labelX24).Size = new Size(519, 33);
		((Control)labelX24).TabIndex = 16;
		((Control)labelX24).Text = " [بدیهی است این روز همانند روز کاری در کارکرد کارگر محاسبه شده ]";
		labelX24.WordWrap = true;
		((Control)labelX23).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX23).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX23).Font = new Font("Tahoma", 10f, (FontStyle)1);
		((Control)labelX23).ForeColor = Color.DarkRed;
		((Control)labelX23).Location = new Point(12, 59);
		((Control)labelX23).Name = "labelX23";
		((Control)labelX23).Size = new Size(519, 35);
		((Control)labelX23).TabIndex = 16;
		((Control)labelX23).Text = "[بعنوان روز استراحت تعیین نموده و کارگر تعطیل می باشد.]";
		labelX23.WordWrap = true;
		((Control)labelX22).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX22).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX22).Font = new Font("Tahoma", 10f, (FontStyle)1);
		((Control)labelX22).ForeColor = Color.DarkRed;
		((Control)labelX22).Location = new Point(12, 21);
		((Control)labelX22).Name = "labelX22";
		((Control)labelX22).Size = new Size(519, 38);
		((Control)labelX22).TabIndex = 15;
		((Control)labelX22).Text = "[شیفت استراحت برای روزهایی است که کارفرما در قرارداد کارگر]";
		labelX22.WordWrap = true;
		((BaseItemControl)ChkCalcEK).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChkCalcEK).Font = new Font("Tahoma", 8f, (FontStyle)1);
		((Control)ChkCalcEK).Location = new Point(312, 465);
		((Control)ChkCalcEK).Name = "ChkCalcEK";
		((Control)ChkCalcEK).Size = new Size(220, 28);
		((BaseItemControl)ChkCalcEK).Style = (eDotNetBarStyle)9;
		((Control)ChkCalcEK).TabIndex = 29;
		((Control)ChkCalcEK).Text = "در این شیفت اضافه کار محاسبه نشود";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)BtnCancel;
		((Form)this).ClientSize = new Size(574, 713);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmShiftCodeing";
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "[فرم کدینگ شیفت]*";
		((Control)panelEx1).ResumeLayout(false);
		((Control)groupPanel7).ResumeLayout(false);
		((Control)PnlNextDay).ResumeLayout(false);
		((ISupportInitialize)txtLunchTime).EndInit();
		((ISupportInitialize)txtMinStartLunch).EndInit();
		((ISupportInitialize)txtHurStartLunch).EndInit();
		((ISupportInitialize)HlBLMaxTimeCanExit).EndInit();
		((ISupportInitialize)MlBLMaxTimeCanExit).EndInit();
		((Control)grpPanelPeakOvertime).ResumeLayout(false);
		((ISupportInitialize)MinutePeakOvertime).EndInit();
		((ISupportInitialize)HourPeakOvertime).EndInit();
		((Control)grpPanelNormal1).ResumeLayout(false);
		((ISupportInitialize)Hour1).EndInit();
		((ISupportInitialize)Min1).EndInit();
		((ISupportInitialize)TakhirV).EndInit();
		((ISupportInitialize)TajilV).EndInit();
		((Control)grpPanelNormal2).ResumeLayout(false);
		((ISupportInitialize)TakhirKh).EndInit();
		((ISupportInitialize)TajilKh).EndInit();
		((ISupportInitialize)Min2).EndInit();
		((ISupportInitialize)Hour2).EndInit();
		((Control)PnlNormal1).ResumeLayout(false);
		((ISupportInitialize)JarimeV).EndInit();
		((Control)groupPanel4).ResumeLayout(false);
		((Control)groupPanel4).PerformLayout();
		((Control)grpPanelShenavar).ResumeLayout(false);
		((ISupportInitialize)MinShenavar).EndInit();
		((ISupportInitialize)HourShenavar).EndInit();
		((Control)groupPanel5).ResumeLayout(false);
		((Control)grpPanelSurcease).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}

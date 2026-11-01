using System;
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
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using Serilog;
using TimeAttendance.ValidatorAttribute.Package;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceDAL.View.Shift;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.SportClub;

public class FrmPackage : FormExtention
{
	private ShiftViewModel _clsShifts;

	private CancellationTokenSource TokenFrm = new CancellationTokenSource();

	private bool _isDisposed;

	private CancellationTokenSource _loadDataToken;

	private short _packageId;

	private IContainer components;

	internal PanelEx PanelEx1;

	internal ButtonX BtnAccept;

	internal ButtonX BtnCancel;

	internal PanelEx PanelEx2;

	internal TextBoxX txtDesc;

	internal LabelX labelX2;

	internal TextBoxX txtName;

	internal LabelX LabelX1;

	internal LabelX labelX3;

	private RadioButton RbtnPeriodic;

	private RadioButton RbtnMonth;

	internal LabelX labelX4;

	internal DateBoxFa txtStartDate;

	internal DateBoxFa txtFinishDate;

	private LabelX labelX6;

	private LabelX labelX7;

	internal LabelX labelX8;

	internal LabelX labelX5;

	private MaskedTextBoxAdv mtxEndFri;

	private MaskedTextBoxAdv mtxStartFri;

	private MaskedTextBoxAdv mtxEndThu;

	private MaskedTextBoxAdv mtxStartThu;

	private MaskedTextBoxAdv mtxEndWed;

	private MaskedTextBoxAdv mtxStartWed;

	private MaskedTextBoxAdv mtxEndTue;

	private MaskedTextBoxAdv mtxStartTue;

	private MaskedTextBoxAdv mtxEndMon;

	private MaskedTextBoxAdv mtxStartMon;

	private MaskedTextBoxAdv mtxEndSun;

	private MaskedTextBoxAdv mtxEndSat;

	private MaskedTextBoxAdv mtxStartSun;

	private MaskedTextBoxAdv mtxStartSat;

	private NumericUpDown numPeriodic;

	private CheckBoxX ChbSat;

	private CheckBoxX ChbMon;

	private CheckBoxX ChbSun;

	private CheckBoxX ChbFri;

	private CheckBoxX ChbThu;

	private CheckBoxX ChbTue;

	private CheckBoxX ChbWed;

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((FormExtention)this).IsDisposed;
			}
			return true;
		}
	}

	public FrmPackage(string contextGuid)
		: base(contextGuid, true, true)
	{
		InitializeComponent();
		_clsShifts = new ShiftViewModel(((CacheBaseForm)this).ContextGuid);
		FillForm();
		((Control)this).Text = "افزودن پکیج جدید";
	}

	public FrmPackage(string contextGuid, short ID)
		: base(contextGuid, true, true)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		_packageId = ID;
		InitializeComponent();
		_loadDataToken?.Cancel();
		_loadDataToken = new CancellationTokenSource();
		CancellationToken token = _loadDataToken.Token;
		new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\FrmPackage.cs");
		((Control)this).Text = "ویرایش پکیج";
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			_clsShifts = await ShiftViewModel.GetAsync(((CacheBaseForm)this).ContextGuid, _packageId, token);
			FillForm();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		((FormExtention)this).OnLoad(e);
		((FormExtention)this).OK_Click += new CrmEventHandler(BtnAccept_Click);
		((Control)BtnCancel).Click += base.BtnCancel_Click;
	}

	private void FillForm()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Invalid comparison between Unknown and I4
		((Control)txtName).Text = _clsShifts.Name;
		((Control)txtDesc).Text = _clsShifts.Description;
		if ((int)_clsShifts.PackageType == 51)
		{
			RbtnMonth.Checked = true;
		}
		else
		{
			RbtnPeriodic.Checked = true;
			((Control)numPeriodic).Text = _clsShifts.Periodic.ToString();
		}
		((Control)txtStartDate).Text = _clsShifts.DateStartSh;
		((Control)txtFinishDate).Text = _clsShifts.DateFinishSh;
		((Control)mtxStartSat).Text = _clsShifts.StartTimeSat;
		((Control)mtxEndSat).Text = _clsShifts.EndTimeSat;
		((Control)mtxStartSun).Text = _clsShifts.StartTimeSun;
		((Control)mtxEndSun).Text = _clsShifts.EndTimeSun;
		((Control)mtxStartMon).Text = _clsShifts.StartTimeMon;
		((Control)mtxEndMon).Text = _clsShifts.EndTimeMon;
		((Control)mtxStartTue).Text = _clsShifts.StartTimeTue;
		((Control)mtxEndTue).Text = _clsShifts.EndTimeTue;
		((Control)mtxStartWed).Text = _clsShifts.StartTimeWed;
		((Control)mtxEndWed).Text = _clsShifts.EndTimeWed;
		((Control)mtxStartThu).Text = _clsShifts.StartTimeThu;
		((Control)mtxEndThu).Text = _clsShifts.EndTimeThu;
		((Control)mtxStartFri).Text = _clsShifts.StartTimeFri;
		((Control)mtxEndFri).Text = _clsShifts.EndTimeFri;
		ChbSat.Checked = _clsShifts.Sat;
		ChbSun.Checked = _clsShifts.Sun;
		ChbMon.Checked = _clsShifts.Mon;
		ChbTue.Checked = _clsShifts.Tue;
		ChbWed.Checked = _clsShifts.Wed;
		ChbThu.Checked = _clsShifts.Thu;
		ChbFri.Checked = _clsShifts.Fri;
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		if (!((Control)BtnAccept).Enabled || !((Control)BtnAccept).Visible)
		{
			return;
		}
		try
		{
			TokenFrm?.Cancel();
			TokenFrm = new CancellationTokenSource();
			CancellationToken token = TokenFrm.Token;
			Getdata();
			DialogResult val = PackageCheckValidation.CheckValidation(((CacheBaseForm)this).ContextGuid, _clsShifts, isShowMessage: true);
			if ((int)val == 6)
			{
				await ShiftDet.DeleteAsync(((CacheBaseForm)this).ContextGuid, _clsShifts.ShiftId, token);
				if (await (await _clsShifts.ConvertToShiftsAsync(token)).SaveAsync(token))
				{
					((FormExtention)this).ResetControlChanged();
					((Form)this).DialogResult = (DialogResult)1;
					((Form)this).Close();
				}
				else
				{
					((Form)this).DialogResult = (DialogResult)2;
					((Form)this).Close();
				}
			}
			else if ((int)val == 2)
			{
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\FrmPackage.cs", 144);
		}
	}

	private void Getdata()
	{
		_clsShifts.Name = ((Control)txtName).Text;
		_clsShifts.Description = ((Control)txtDesc).Text;
		if (RbtnMonth.Checked)
		{
			_clsShifts.PackageType = (PackageEnumType)51;
			_clsShifts.Periodic = 0;
		}
		else
		{
			_clsShifts.PackageType = (PackageEnumType)52;
			_clsShifts.Periodic = Utilities.ParseToShort(((Control)numPeriodic).Text, (short)0);
		}
		_clsShifts.DateStartM = Utilities.CalendarToMiladi(((Control)txtStartDate).Text);
		_clsShifts.DateFinishM = Utilities.CalendarToMiladi(((Control)txtFinishDate).Text);
		_clsShifts.DateFinishM = _clsShifts.DateFinishM.AddDays(1.0).AddSeconds(-1.0);
		_clsShifts.StartTimeSat = ((Control)mtxStartSat).Text;
		_clsShifts.StartTimeSun = ((Control)mtxStartSun).Text;
		_clsShifts.StartTimeMon = ((Control)mtxStartMon).Text;
		_clsShifts.StartTimeTue = ((Control)mtxStartTue).Text;
		_clsShifts.StartTimeWed = ((Control)mtxStartWed).Text;
		_clsShifts.StartTimeThu = ((Control)mtxStartThu).Text;
		_clsShifts.StartTimeFri = ((Control)mtxStartFri).Text;
		_clsShifts.EndTimeSat = ((Control)mtxEndSat).Text;
		_clsShifts.EndTimeSun = ((Control)mtxEndSun).Text;
		_clsShifts.EndTimeMon = ((Control)mtxEndMon).Text;
		_clsShifts.EndTimeTue = ((Control)mtxEndTue).Text;
		_clsShifts.EndTimeWed = ((Control)mtxEndWed).Text;
		_clsShifts.EndTimeThu = ((Control)mtxEndThu).Text;
		_clsShifts.EndTimeFri = ((Control)mtxEndFri).Text;
		_clsShifts.Sat = ChbSat.Checked;
		_clsShifts.Sun = ChbSun.Checked;
		_clsShifts.Mon = ChbMon.Checked;
		_clsShifts.Tue = ChbTue.Checked;
		_clsShifts.Wed = ChbWed.Checked;
		_clsShifts.Thu = ChbThu.Checked;
		_clsShifts.Fri = ChbFri.Checked;
	}

	private void RbtnPeriodic_CheckedChanged(object sender, EventArgs e)
	{
		((Control)numPeriodic).Enabled = RbtnPeriodic.Checked;
	}

	private void ChbSat_CheckedChanged(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		string name = ((Control)(CheckBoxX)sender).Name;
		if (name == null)
		{
			return;
		}
		int length = name.Length;
		if (length != 6)
		{
			return;
		}
		switch (name[4])
		{
		case 'a':
			if (name == "ChbSat")
			{
				MaskedTextBoxAdv obj4 = mtxStartSat;
				bool enabled = (((Control)mtxEndSat).Enabled = ChbSat.Checked);
				((Control)obj4).Enabled = enabled;
			}
			break;
		case 'u':
			if (!(name == "ChbSun"))
			{
				if (name == "ChbTue")
				{
					MaskedTextBoxAdv obj6 = mtxStartTue;
					bool enabled = (((Control)mtxEndTue).Enabled = ChbTue.Checked);
					((Control)obj6).Enabled = enabled;
				}
			}
			else
			{
				MaskedTextBoxAdv obj7 = mtxStartSun;
				bool enabled = (((Control)mtxEndSun).Enabled = ChbSun.Checked);
				((Control)obj7).Enabled = enabled;
			}
			break;
		case 'o':
			if (name == "ChbMon")
			{
				MaskedTextBoxAdv obj2 = mtxStartMon;
				bool enabled = (((Control)mtxEndMon).Enabled = ChbMon.Checked);
				((Control)obj2).Enabled = enabled;
			}
			break;
		case 'e':
			if (name == "ChbWed")
			{
				MaskedTextBoxAdv obj5 = mtxStartWed;
				bool enabled = (((Control)mtxEndWed).Enabled = ChbWed.Checked);
				((Control)obj5).Enabled = enabled;
			}
			break;
		case 'h':
			if (name == "ChbThu")
			{
				MaskedTextBoxAdv obj3 = mtxStartThu;
				bool enabled = (((Control)mtxEndThu).Enabled = ChbThu.Checked);
				((Control)obj3).Enabled = enabled;
			}
			break;
		case 'r':
			if (name == "ChbFri")
			{
				MaskedTextBoxAdv obj = mtxStartFri;
				bool enabled = (((Control)mtxEndFri).Enabled = ChbFri.Checked);
				((Control)obj).Enabled = enabled;
			}
			break;
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
				components?.Dispose();
				((FormExtention)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
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
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Expected O, but got Unknown
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Expected O, but got Unknown
		//IL_1a9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aa9: Expected O, but got Unknown
		//IL_1ad5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1adf: Expected O, but got Unknown
		//IL_1c4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c59: Expected O, but got Unknown
		//IL_1c85: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c8f: Expected O, but got Unknown
		//IL_2090: Unknown result type (might be due to invalid IL or missing references)
		//IL_209a: Expected O, but got Unknown
		//IL_2136: Unknown result type (might be due to invalid IL or missing references)
		//IL_2140: Expected O, but got Unknown
		//IL_21dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_21e6: Expected O, but got Unknown
		//IL_2282: Unknown result type (might be due to invalid IL or missing references)
		//IL_228c: Expected O, but got Unknown
		//IL_2324: Unknown result type (might be due to invalid IL or missing references)
		//IL_232e: Expected O, but got Unknown
		//IL_2440: Unknown result type (might be due to invalid IL or missing references)
		//IL_244a: Expected O, but got Unknown
		PanelEx1 = new PanelEx();
		BtnAccept = new ButtonX();
		BtnCancel = new ButtonX();
		PanelEx2 = new PanelEx();
		ChbSat = new CheckBoxX();
		ChbMon = new CheckBoxX();
		ChbSun = new CheckBoxX();
		ChbFri = new CheckBoxX();
		ChbThu = new CheckBoxX();
		ChbTue = new CheckBoxX();
		ChbWed = new CheckBoxX();
		numPeriodic = new NumericUpDown();
		mtxEndFri = new MaskedTextBoxAdv();
		mtxStartFri = new MaskedTextBoxAdv();
		mtxEndThu = new MaskedTextBoxAdv();
		mtxStartThu = new MaskedTextBoxAdv();
		mtxEndWed = new MaskedTextBoxAdv();
		mtxStartWed = new MaskedTextBoxAdv();
		mtxEndTue = new MaskedTextBoxAdv();
		mtxStartTue = new MaskedTextBoxAdv();
		mtxEndMon = new MaskedTextBoxAdv();
		mtxStartMon = new MaskedTextBoxAdv();
		mtxEndSun = new MaskedTextBoxAdv();
		mtxEndSat = new MaskedTextBoxAdv();
		mtxStartSun = new MaskedTextBoxAdv();
		mtxStartSat = new MaskedTextBoxAdv();
		txtStartDate = new DateBoxFa();
		txtFinishDate = new DateBoxFa();
		labelX6 = new LabelX();
		labelX7 = new LabelX();
		RbtnPeriodic = new RadioButton();
		RbtnMonth = new RadioButton();
		txtDesc = new TextBoxX();
		labelX8 = new LabelX();
		labelX5 = new LabelX();
		labelX4 = new LabelX();
		labelX3 = new LabelX();
		labelX2 = new LabelX();
		txtName = new TextBoxX();
		LabelX1 = new LabelX();
		((Control)PanelEx1).SuspendLayout();
		((Control)PanelEx2).SuspendLayout();
		((ISupportInitialize)numPeriodic).BeginInit();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)PanelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)PanelEx1).Controls.Add((Control)(object)PanelEx2);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(650, 376);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.SteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 0;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)3;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnAccept.Image = (Image)(object)Resources.accept;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(118, 319);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).Size = new Size(106, 33);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 1;
		((Control)BtnAccept).Text = "ثبت(F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)3;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnCancel.Image = (Image)(object)Resources.stop1;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(3, 319);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).Size = new Size(106, 33);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 2;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "لغـو(Esc)";
		((Control)PanelEx2).Anchor = (AnchorStyles)15;
		PanelEx2.CanvasColor = SystemColors.Control;
		PanelEx2.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx2).Controls.Add((Control)(object)ChbSat);
		((Control)PanelEx2).Controls.Add((Control)(object)ChbMon);
		((Control)PanelEx2).Controls.Add((Control)(object)ChbSun);
		((Control)PanelEx2).Controls.Add((Control)(object)ChbFri);
		((Control)PanelEx2).Controls.Add((Control)(object)ChbThu);
		((Control)PanelEx2).Controls.Add((Control)(object)ChbTue);
		((Control)PanelEx2).Controls.Add((Control)(object)ChbWed);
		((Control)PanelEx2).Controls.Add((Control)(object)numPeriodic);
		((Control)PanelEx2).Controls.Add((Control)(object)mtxEndFri);
		((Control)PanelEx2).Controls.Add((Control)(object)mtxStartFri);
		((Control)PanelEx2).Controls.Add((Control)(object)mtxEndThu);
		((Control)PanelEx2).Controls.Add((Control)(object)mtxStartThu);
		((Control)PanelEx2).Controls.Add((Control)(object)mtxEndWed);
		((Control)PanelEx2).Controls.Add((Control)(object)mtxStartWed);
		((Control)PanelEx2).Controls.Add((Control)(object)mtxEndTue);
		((Control)PanelEx2).Controls.Add((Control)(object)mtxStartTue);
		((Control)PanelEx2).Controls.Add((Control)(object)mtxEndMon);
		((Control)PanelEx2).Controls.Add((Control)(object)mtxStartMon);
		((Control)PanelEx2).Controls.Add((Control)(object)mtxEndSun);
		((Control)PanelEx2).Controls.Add((Control)(object)mtxEndSat);
		((Control)PanelEx2).Controls.Add((Control)(object)mtxStartSun);
		((Control)PanelEx2).Controls.Add((Control)(object)mtxStartSat);
		((Control)PanelEx2).Controls.Add((Control)(object)txtStartDate);
		((Control)PanelEx2).Controls.Add((Control)(object)txtFinishDate);
		((Control)PanelEx2).Controls.Add((Control)(object)labelX6);
		((Control)PanelEx2).Controls.Add((Control)(object)labelX7);
		((Control)PanelEx2).Controls.Add((Control)(object)RbtnPeriodic);
		((Control)PanelEx2).Controls.Add((Control)(object)RbtnMonth);
		((Control)PanelEx2).Controls.Add((Control)(object)txtDesc);
		((Control)PanelEx2).Controls.Add((Control)(object)labelX8);
		((Control)PanelEx2).Controls.Add((Control)(object)labelX5);
		((Control)PanelEx2).Controls.Add((Control)(object)labelX4);
		((Control)PanelEx2).Controls.Add((Control)(object)labelX3);
		((Control)PanelEx2).Controls.Add((Control)(object)labelX2);
		((Control)PanelEx2).Controls.Add((Control)(object)txtName);
		((Control)PanelEx2).Controls.Add((Control)(object)LabelX1);
		PanelEx2.DisabledBackColor = Color.Empty;
		((Control)PanelEx2).Location = new Point(4, 5);
		((Control)PanelEx2).Name = "PanelEx2";
		((Control)PanelEx2).Size = new Size(641, 307);
		PanelEx2.Style.Alignment = (StringAlignment)1;
		PanelEx2.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		PanelEx2.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		PanelEx2.Style.Border = (eBorderType)1;
		PanelEx2.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx2.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx2.Style.GradientAngle = 90;
		((Control)PanelEx2).TabIndex = 0;
		((Control)ChbSat).Anchor = (AnchorStyles)9;
		((BaseItemControl)ChbSat).BackgroundStyle.CornerType = (eCornerType)1;
		ChbSat.Checked = true;
		ChbSat.CheckState = (CheckState)1;
		ChbSat.CheckValue = "Y";
		((Control)ChbSat).Location = new Point(488, 205);
		((Control)ChbSat).Name = "ChbSat";
		((Control)ChbSat).Size = new Size(57, 21);
		((BaseItemControl)ChbSat).Style = (eDotNetBarStyle)9;
		((Control)ChbSat).TabIndex = 36;
		((Control)ChbSat).Text = "شنبه";
		ChbSat.CheckedChanged += ChbSat_CheckedChanged;
		((Control)ChbMon).Anchor = (AnchorStyles)9;
		((BaseItemControl)ChbMon).BackgroundStyle.CornerType = (eCornerType)1;
		ChbMon.Checked = true;
		ChbMon.CheckState = (CheckState)1;
		ChbMon.CheckValue = "Y";
		((Control)ChbMon).Location = new Point(335, 205);
		((Control)ChbMon).Name = "ChbMon";
		((Control)ChbMon).Size = new Size(57, 21);
		((BaseItemControl)ChbMon).Style = (eDotNetBarStyle)9;
		((Control)ChbMon).TabIndex = 38;
		((Control)ChbMon).Text = "دوشنبه";
		ChbMon.CheckedChanged += ChbSat_CheckedChanged;
		((Control)ChbSun).Anchor = (AnchorStyles)9;
		((BaseItemControl)ChbSun).BackgroundStyle.CornerType = (eCornerType)1;
		ChbSun.Checked = true;
		ChbSun.CheckState = (CheckState)1;
		ChbSun.CheckValue = "Y";
		((Control)ChbSun).Location = new Point(408, 205);
		((Control)ChbSun).Name = "ChbSun";
		((Control)ChbSun).Size = new Size(60, 21);
		((BaseItemControl)ChbSun).Style = (eDotNetBarStyle)9;
		((Control)ChbSun).TabIndex = 37;
		((Control)ChbSun).Text = "یکشنبه";
		ChbSun.CheckedChanged += ChbSat_CheckedChanged;
		((Control)ChbFri).Anchor = (AnchorStyles)9;
		((BaseItemControl)ChbFri).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbFri).Location = new Point(28, 205);
		((Control)ChbFri).Name = "ChbFri";
		((Control)ChbFri).Size = new Size(55, 21);
		((BaseItemControl)ChbFri).Style = (eDotNetBarStyle)9;
		((Control)ChbFri).TabIndex = 42;
		((Control)ChbFri).Text = "جمعه";
		ChbFri.CheckedChanged += ChbSat_CheckedChanged;
		((Control)ChbThu).Anchor = (AnchorStyles)9;
		((BaseItemControl)ChbThu).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbThu).Location = new Point(95, 205);
		((Control)ChbThu).Name = "ChbThu";
		((Control)ChbThu).Size = new Size(65, 21);
		((BaseItemControl)ChbThu).Style = (eDotNetBarStyle)9;
		((Control)ChbThu).TabIndex = 41;
		((Control)ChbThu).Text = "پنجشنبه";
		ChbThu.CheckedChanged += ChbSat_CheckedChanged;
		((Control)ChbTue).Anchor = (AnchorStyles)9;
		((BaseItemControl)ChbTue).BackgroundStyle.CornerType = (eCornerType)1;
		ChbTue.Checked = true;
		ChbTue.CheckState = (CheckState)1;
		ChbTue.CheckValue = "Y";
		((Control)ChbTue).Location = new Point(248, 205);
		((Control)ChbTue).Name = "ChbTue";
		((Control)ChbTue).Size = new Size(68, 21);
		((BaseItemControl)ChbTue).Style = (eDotNetBarStyle)9;
		((Control)ChbTue).TabIndex = 39;
		((Control)ChbTue).Text = "سه شنبه";
		ChbTue.CheckedChanged += ChbSat_CheckedChanged;
		((Control)ChbWed).Anchor = (AnchorStyles)9;
		((BaseItemControl)ChbWed).BackgroundStyle.CornerType = (eCornerType)1;
		ChbWed.Checked = true;
		ChbWed.CheckState = (CheckState)1;
		ChbWed.CheckValue = "Y";
		((Control)ChbWed).Location = new Point(167, 205);
		((Control)ChbWed).Name = "ChbWed";
		((Control)ChbWed).Size = new Size(69, 21);
		((BaseItemControl)ChbWed).Style = (eDotNetBarStyle)9;
		((Control)ChbWed).TabIndex = 40;
		((Control)ChbWed).Text = "چهارشنبه";
		ChbWed.CheckedChanged += ChbSat_CheckedChanged;
		((Control)numPeriodic).Anchor = (AnchorStyles)9;
		((Control)numPeriodic).Enabled = false;
		((Control)numPeriodic).Location = new Point(377, 122);
		numPeriodic.Maximum = new decimal(new int[4] { 365, 0, 0, 0 });
		((Control)numPeriodic).Name = "numPeriodic";
		((Control)numPeriodic).Size = new Size(66, 21);
		((Control)numPeriodic).TabIndex = 7;
		((UpDownBase)numPeriodic).TextAlign = (HorizontalAlignment)2;
		((Control)mtxEndFri).Anchor = (AnchorStyles)9;
		mtxEndFri.BackgroundStyle.Class = "TextBoxBorder";
		mtxEndFri.BackgroundStyle.CornerType = (eCornerType)1;
		mtxEndFri.ButtonClear.Visible = true;
		((Control)mtxEndFri).Enabled = false;
		((Control)mtxEndFri).Location = new Point(9, 265);
		mtxEndFri.Mask = "00:00";
		((Control)mtxEndFri).Name = "mtxEndFri";
		((Control)mtxEndFri).Size = new Size(73, 21);
		((PopupItemControl)mtxEndFri).Style = (eDotNetBarStyle)9;
		((Control)mtxEndFri).TabIndex = 35;
		((Control)mtxEndFri).Text = "";
		mtxEndFri.ValidatingType = typeof(DateTime);
		((Control)mtxStartFri).Anchor = (AnchorStyles)9;
		mtxStartFri.BackgroundStyle.Class = "TextBoxBorder";
		mtxStartFri.BackgroundStyle.CornerType = (eCornerType)1;
		mtxStartFri.ButtonClear.Visible = true;
		((Control)mtxStartFri).Enabled = false;
		((Control)mtxStartFri).Location = new Point(9, 234);
		mtxStartFri.Mask = "00:00";
		((Control)mtxStartFri).Name = "mtxStartFri";
		((Control)mtxStartFri).Size = new Size(73, 21);
		((PopupItemControl)mtxStartFri).Style = (eDotNetBarStyle)9;
		((Control)mtxStartFri).TabIndex = 34;
		((Control)mtxStartFri).Text = "";
		mtxStartFri.ValidatingType = typeof(DateTime);
		((Control)mtxEndThu).Anchor = (AnchorStyles)9;
		mtxEndThu.BackgroundStyle.Class = "TextBoxBorder";
		mtxEndThu.BackgroundStyle.CornerType = (eCornerType)1;
		mtxEndThu.ButtonClear.Visible = true;
		((Control)mtxEndThu).Location = new Point(88, 265);
		mtxEndThu.Mask = "00:00";
		((Control)mtxEndThu).Name = "mtxEndThu";
		((Control)mtxEndThu).Size = new Size(73, 21);
		((PopupItemControl)mtxEndThu).Style = (eDotNetBarStyle)9;
		((Control)mtxEndThu).TabIndex = 32;
		((Control)mtxEndThu).Text = "";
		mtxEndThu.ValidatingType = typeof(DateTime);
		((Control)mtxStartThu).Anchor = (AnchorStyles)9;
		mtxStartThu.BackgroundStyle.Class = "TextBoxBorder";
		mtxStartThu.BackgroundStyle.CornerType = (eCornerType)1;
		mtxStartThu.ButtonClear.Visible = true;
		((Control)mtxStartThu).Location = new Point(88, 234);
		mtxStartThu.Mask = "00:00";
		((Control)mtxStartThu).Name = "mtxStartThu";
		((Control)mtxStartThu).Size = new Size(73, 21);
		((PopupItemControl)mtxStartThu).Style = (eDotNetBarStyle)9;
		((Control)mtxStartThu).TabIndex = 31;
		((Control)mtxStartThu).Text = "";
		mtxStartThu.ValidatingType = typeof(DateTime);
		((Control)mtxEndWed).Anchor = (AnchorStyles)9;
		mtxEndWed.BackgroundStyle.Class = "TextBoxBorder";
		mtxEndWed.BackgroundStyle.CornerType = (eCornerType)1;
		mtxEndWed.ButtonClear.Visible = true;
		((Control)mtxEndWed).Location = new Point(167, 265);
		mtxEndWed.Mask = "00:00";
		((Control)mtxEndWed).Name = "mtxEndWed";
		((Control)mtxEndWed).Size = new Size(71, 21);
		((PopupItemControl)mtxEndWed).Style = (eDotNetBarStyle)9;
		((Control)mtxEndWed).TabIndex = 29;
		((Control)mtxEndWed).Text = "";
		mtxEndWed.ValidatingType = typeof(DateTime);
		((Control)mtxStartWed).Anchor = (AnchorStyles)9;
		mtxStartWed.BackgroundStyle.Class = "TextBoxBorder";
		mtxStartWed.BackgroundStyle.CornerType = (eCornerType)1;
		mtxStartWed.ButtonClear.Visible = true;
		((Control)mtxStartWed).Location = new Point(167, 234);
		mtxStartWed.Mask = "00:00";
		((Control)mtxStartWed).Name = "mtxStartWed";
		((Control)mtxStartWed).Size = new Size(71, 21);
		((PopupItemControl)mtxStartWed).Style = (eDotNetBarStyle)9;
		((Control)mtxStartWed).TabIndex = 28;
		((Control)mtxStartWed).Text = "";
		mtxStartWed.ValidatingType = typeof(DateTime);
		((Control)mtxEndTue).Anchor = (AnchorStyles)9;
		mtxEndTue.BackgroundStyle.Class = "TextBoxBorder";
		mtxEndTue.BackgroundStyle.CornerType = (eCornerType)1;
		mtxEndTue.ButtonClear.Visible = true;
		((Control)mtxEndTue).Location = new Point(244, 265);
		mtxEndTue.Mask = "00:00";
		((Control)mtxEndTue).Name = "mtxEndTue";
		((Control)mtxEndTue).Size = new Size(71, 21);
		((PopupItemControl)mtxEndTue).Style = (eDotNetBarStyle)9;
		((Control)mtxEndTue).TabIndex = 26;
		((Control)mtxEndTue).Text = "";
		mtxEndTue.ValidatingType = typeof(DateTime);
		((Control)mtxStartTue).Anchor = (AnchorStyles)9;
		mtxStartTue.BackgroundStyle.Class = "TextBoxBorder";
		mtxStartTue.BackgroundStyle.CornerType = (eCornerType)1;
		mtxStartTue.ButtonClear.Visible = true;
		((Control)mtxStartTue).Location = new Point(244, 234);
		mtxStartTue.Mask = "00:00";
		((Control)mtxStartTue).Name = "mtxStartTue";
		((Control)mtxStartTue).Size = new Size(71, 21);
		((PopupItemControl)mtxStartTue).Style = (eDotNetBarStyle)9;
		((Control)mtxStartTue).TabIndex = 25;
		((Control)mtxStartTue).Text = "";
		mtxStartTue.ValidatingType = typeof(DateTime);
		((Control)mtxEndMon).Anchor = (AnchorStyles)9;
		mtxEndMon.BackgroundStyle.Class = "TextBoxBorder";
		mtxEndMon.BackgroundStyle.CornerType = (eCornerType)1;
		mtxEndMon.ButtonClear.Visible = true;
		((Control)mtxEndMon).Location = new Point(321, 265);
		mtxEndMon.Mask = "00:00";
		((Control)mtxEndMon).Name = "mtxEndMon";
		((Control)mtxEndMon).Size = new Size(71, 21);
		((PopupItemControl)mtxEndMon).Style = (eDotNetBarStyle)9;
		((Control)mtxEndMon).TabIndex = 23;
		((Control)mtxEndMon).Text = "";
		mtxEndMon.ValidatingType = typeof(DateTime);
		((Control)mtxStartMon).Anchor = (AnchorStyles)9;
		mtxStartMon.BackgroundStyle.Class = "TextBoxBorder";
		mtxStartMon.BackgroundStyle.CornerType = (eCornerType)1;
		mtxStartMon.ButtonClear.Visible = true;
		((Control)mtxStartMon).Location = new Point(321, 234);
		mtxStartMon.Mask = "00:00";
		((Control)mtxStartMon).Name = "mtxStartMon";
		((Control)mtxStartMon).Size = new Size(71, 21);
		((PopupItemControl)mtxStartMon).Style = (eDotNetBarStyle)9;
		((Control)mtxStartMon).TabIndex = 22;
		((Control)mtxStartMon).Text = "";
		mtxStartMon.ValidatingType = typeof(DateTime);
		((Control)mtxEndSun).Anchor = (AnchorStyles)9;
		mtxEndSun.BackgroundStyle.Class = "TextBoxBorder";
		mtxEndSun.BackgroundStyle.CornerType = (eCornerType)1;
		mtxEndSun.ButtonClear.Visible = true;
		((Control)mtxEndSun).Location = new Point(398, 265);
		mtxEndSun.Mask = "00:00";
		((Control)mtxEndSun).Name = "mtxEndSun";
		((Control)mtxEndSun).Size = new Size(71, 21);
		((PopupItemControl)mtxEndSun).Style = (eDotNetBarStyle)9;
		((Control)mtxEndSun).TabIndex = 20;
		((Control)mtxEndSun).Text = "";
		mtxEndSun.ValidatingType = typeof(DateTime);
		((Control)mtxEndSat).Anchor = (AnchorStyles)9;
		mtxEndSat.BackgroundStyle.Class = "TextBoxBorder";
		mtxEndSat.BackgroundStyle.CornerType = (eCornerType)1;
		mtxEndSat.ButtonClear.Visible = true;
		((Control)mtxEndSat).Location = new Point(475, 265);
		mtxEndSat.Mask = "00:00";
		((Control)mtxEndSat).Name = "mtxEndSat";
		((Control)mtxEndSat).Size = new Size(71, 21);
		((PopupItemControl)mtxEndSat).Style = (eDotNetBarStyle)9;
		((Control)mtxEndSat).TabIndex = 17;
		((Control)mtxEndSat).Text = "";
		mtxEndSat.ValidatingType = typeof(DateTime);
		((Control)mtxStartSun).Anchor = (AnchorStyles)9;
		mtxStartSun.BackgroundStyle.Class = "TextBoxBorder";
		mtxStartSun.BackgroundStyle.CornerType = (eCornerType)1;
		mtxStartSun.ButtonClear.Visible = true;
		((Control)mtxStartSun).Location = new Point(398, 234);
		mtxStartSun.Mask = "00:00";
		((Control)mtxStartSun).Name = "mtxStartSun";
		((Control)mtxStartSun).Size = new Size(71, 21);
		((PopupItemControl)mtxStartSun).Style = (eDotNetBarStyle)9;
		((Control)mtxStartSun).TabIndex = 19;
		((Control)mtxStartSun).Text = "";
		mtxStartSun.ValidatingType = typeof(DateTime);
		((Control)mtxStartSat).Anchor = (AnchorStyles)9;
		mtxStartSat.BackgroundStyle.Class = "TextBoxBorder";
		mtxStartSat.BackgroundStyle.CornerType = (eCornerType)1;
		mtxStartSat.ButtonClear.Visible = true;
		((Control)mtxStartSat).Location = new Point(475, 234);
		mtxStartSat.Mask = "00:00";
		((Control)mtxStartSat).Name = "mtxStartSat";
		((Control)mtxStartSat).Size = new Size(71, 21);
		((PopupItemControl)mtxStartSat).Style = (eDotNetBarStyle)9;
		((Control)mtxStartSat).TabIndex = 16;
		((Control)mtxStartSat).Text = "";
		mtxStartSat.ValidatingType = typeof(DateTime);
		txtStartDate.AllowNull = false;
		((Control)txtStartDate).Anchor = (AnchorStyles)9;
		txtStartDate.BackcolorTooltip = Color.Red;
		((TextBoxX)txtStartDate).Border.CornerType = (eCornerType)1;
		txtStartDate.EnterToTab = true;
		txtStartDate.EscToClose = true;
		txtStartDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		txtStartDate.FocusBackColor = Color.Yellow;
		txtStartDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		txtStartDate.FocusForeColor = Color.Blue;
		txtStartDate.FocusTextSelect = true;
		((Control)txtStartDate).Font = new Font("Times New Roman", 12f);
		((Control)txtStartDate).Location = new Point(413, 163);
		txtStartDate.MessageEmptyShowDialog = false;
		txtStartDate.MessegeEmpty = "";
		txtStartDate.MessegeEmptyInFormRight = true;
		txtStartDate.MessegeEmptyShowInForm = false;
		txtStartDate.MiladiDate = new DateTime(2022, 7, 6, 14, 30, 37, 658);
		((Control)txtStartDate).Name = "txtStartDate";
		((Control)txtStartDate).RightToLeft = (RightToLeft)0;
		txtStartDate.ShowNameOfDayOnToolTip = false;
		txtStartDate.ShowToolTipText = false;
		((Control)txtStartDate).Size = new Size(123, 20);
		((Control)txtStartDate).TabIndex = 9;
		((Control)txtStartDate).Text = "06/07/2022";
		((TextBox)txtStartDate).TextAlign = (HorizontalAlignment)2;
		txtStartDate.TootTipDelay = 500;
		txtFinishDate.AllowNull = false;
		((Control)txtFinishDate).Anchor = (AnchorStyles)9;
		txtFinishDate.BackcolorTooltip = Color.Red;
		((TextBoxX)txtFinishDate).Border.CornerType = (eCornerType)1;
		txtFinishDate.EnterToTab = true;
		txtFinishDate.EscToClose = true;
		txtFinishDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		txtFinishDate.FocusBackColor = Color.Yellow;
		txtFinishDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		txtFinishDate.FocusForeColor = Color.Blue;
		txtFinishDate.FocusTextSelect = true;
		((Control)txtFinishDate).Font = new Font("Times New Roman", 12f);
		((Control)txtFinishDate).Location = new Point(122, 163);
		txtFinishDate.MessageEmptyShowDialog = false;
		txtFinishDate.MessegeEmpty = "";
		txtFinishDate.MessegeEmptyInFormRight = true;
		txtFinishDate.MessegeEmptyShowInForm = false;
		txtFinishDate.MiladiDate = new DateTime(2022, 7, 6, 14, 30, 37, 658);
		((Control)txtFinishDate).Name = "txtFinishDate";
		((Control)txtFinishDate).RightToLeft = (RightToLeft)0;
		txtFinishDate.ShowNameOfDayOnToolTip = false;
		txtFinishDate.ShowToolTipText = false;
		((Control)txtFinishDate).Size = new Size(115, 20);
		((Control)txtFinishDate).TabIndex = 11;
		((Control)txtFinishDate).Text = "06/07/2022";
		((TextBox)txtFinishDate).TextAlign = (HorizontalAlignment)2;
		txtFinishDate.TootTipDelay = 500;
		((Control)labelX6).Anchor = (AnchorStyles)9;
		((Control)labelX6).BackColor = Color.Transparent;
		((BaseItemControl)labelX6).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX6).Location = new Point(248, 163);
		((Control)labelX6).Name = "labelX6";
		((Control)labelX6).Size = new Size(83, 21);
		((Control)labelX6).TabIndex = 10;
		((Control)labelX6).Text = "تاریخ پایان :";
		labelX6.TextAlignment = (StringAlignment)2;
		((Control)labelX7).Anchor = (AnchorStyles)9;
		((Control)labelX7).BackColor = Color.Transparent;
		((BaseItemControl)labelX7).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX7).Location = new Point(542, 163);
		((Control)labelX7).Name = "labelX7";
		((Control)labelX7).Size = new Size(70, 21);
		((Control)labelX7).TabIndex = 8;
		((Control)labelX7).Text = "تاریخ شروع :";
		labelX7.TextAlignment = (StringAlignment)2;
		((Control)RbtnPeriodic).Anchor = (AnchorStyles)9;
		((Control)RbtnPeriodic).ForeColor = SystemColors.HotTrack;
		((Control)RbtnPeriodic).Location = new Point(449, 122);
		((Control)RbtnPeriodic).Name = "RbtnPeriodic";
		((Control)RbtnPeriodic).Size = new Size(75, 21);
		((Control)RbtnPeriodic).TabIndex = 6;
		((Control)RbtnPeriodic).Text = "جلسه ای";
		((ButtonBase)RbtnPeriodic).UseVisualStyleBackColor = true;
		RbtnPeriodic.CheckedChanged += RbtnPeriodic_CheckedChanged;
		((Control)RbtnMonth).Anchor = (AnchorStyles)9;
		RbtnMonth.Checked = true;
		((Control)RbtnMonth).ForeColor = SystemColors.HotTrack;
		((Control)RbtnMonth).Location = new Point(449, 97);
		((Control)RbtnMonth).Name = "RbtnMonth";
		((Control)RbtnMonth).Size = new Size(75, 21);
		((Control)RbtnMonth).TabIndex = 5;
		RbtnMonth.TabStop = true;
		((Control)RbtnMonth).Text = "ماهانه";
		((ButtonBase)RbtnMonth).UseVisualStyleBackColor = true;
		((Control)txtDesc).Anchor = (AnchorStyles)9;
		txtDesc.Border.Class = "TextBoxBorder";
		txtDesc.Border.CornerType = (eCornerType)1;
		((Control)txtDesc).Location = new Point(12, 41);
		((TextBoxBase)txtDesc).Multiline = true;
		((Control)txtDesc).Name = "txtDesc";
		((Control)txtDesc).Size = new Size(524, 40);
		((Control)txtDesc).TabIndex = 3;
		((Control)labelX8).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX8).Location = new Point(552, 264);
		((Control)labelX8).Name = "labelX8";
		((Control)labelX8).Size = new Size(85, 23);
		((Control)labelX8).TabIndex = 14;
		((Control)labelX8).Text = "ساعت پایان :";
		labelX8.TextAlignment = (StringAlignment)2;
		((Control)labelX5).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX5).Location = new Point(552, 233);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(85, 23);
		((Control)labelX5).TabIndex = 13;
		((Control)labelX5).Text = "ساعت شروع :";
		labelX5.TextAlignment = (StringAlignment)2;
		((Control)labelX4).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX4).Location = new Point(552, 204);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(85, 23);
		((Control)labelX4).TabIndex = 12;
		((Control)labelX4).Text = "برنامه هفتگی";
		labelX4.TextAlignment = (StringAlignment)2;
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX3).Location = new Point(542, 96);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(72, 23);
		((Control)labelX3).TabIndex = 4;
		((Control)labelX3).Text = "نوع پکیج :";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX2).Location = new Point(542, 38);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(72, 23);
		((Control)labelX2).TabIndex = 2;
		((Control)labelX2).Text = "توضیحات :";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)txtName).Anchor = (AnchorStyles)9;
		txtName.Border.Class = "TextBoxBorder";
		txtName.Border.CornerType = (eCornerType)1;
		((Control)txtName).Location = new Point(12, 15);
		((Control)txtName).Name = "txtName";
		((Control)txtName).Size = new Size(524, 21);
		((Control)txtName).TabIndex = 1;
		((Control)LabelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LabelX1).Location = new Point(542, 14);
		((Control)LabelX1).Name = "LabelX1";
		((Control)LabelX1).Size = new Size(87, 23);
		((Control)LabelX1).TabIndex = 0;
		((Control)LabelX1).Text = "وضعیت خدمت :";
		LabelX1.TextAlignment = (StringAlignment)2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(650, 376);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(384, 186);
		((Control)this).Name = "FrmPackage";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "FrmPackage";
		((Control)PanelEx1).ResumeLayout(false);
		((Control)PanelEx2).ResumeLayout(false);
		((ISupportInitialize)numPeriodic).EndInit();
		((Control)this).ResumeLayout(false);
	}
}

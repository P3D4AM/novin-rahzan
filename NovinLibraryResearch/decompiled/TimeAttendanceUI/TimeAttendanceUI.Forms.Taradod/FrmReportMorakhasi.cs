using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using FormCustom.Waiting;
using PacketParser.Attendance.Entities;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Utility;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Taradod;

public class FrmReportMorakhasi : BaseForm
{
	private DateTime _startDateSh = DateTime.Now;

	private DateTime _endDateSh = DateTime.Now;

	private List<MorakhasiSaati> _lstMorakhasiSaati;

	private readonly Font _font;

	private GharardadNew _xclsGharardad;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private bool _isDisposed;

	private IContainer components;

	private PanelEx panelEx1;

	private LabelX labelX8;

	private LabelX labelX11;

	private LabelX labelX10;

	private LabelX labelX9;

	private LabelX labelX4;

	private ButtonX BtnAccept;

	private LabelX labelX18;

	private PanelEx panelEx2;

	private LabelX labelX1;

	internal ButtonX BtnDetailMorakhasi;

	private LabelX labelX6;

	private LabelX labelX2;

	private LabelX LblVacationLeft;

	internal ButtonX BtnDetailSaati;

	private LabelX LblVacationUsed;

	private LabelX LblValVacationLeft;

	private LabelX LblVacationSaatiDay;

	private LabelX LblVacationSaatiTime;

	private LabelX LblGharardadNo;

	private LabelX LblCanUsed;

	private LabelX LblVacationSaved;

	private LabelX LblMorakhasiOnGharardad;

	private LabelX LblCodeTafsil;

	private LabelX LblFullName;

	private LabelX LblAttention;

	private LabelX labelx3;

	private LabelX TxtVacationToNow;

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

	public FrmReportMorakhasi(string contextGuid, GharardadNew gharardad, Font font)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_font = font;
			_xclsGharardad = gharardad;
			InitializeComponent();
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FrmReportMorakhasi.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FrmReportMorakhasi.cs", 66);
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			Tuple<int, int, double, GharardadNew, List<MorakhasiSaati>> tuple = await Morakhasidaily.CalcLeaveBalanceAsync(((CacheBaseForm)this).ContextGuid, _xclsGharardad, token);
			_xclsGharardad = tuple.Item4;
			if (_xclsGharardad == null)
			{
				Utility.MessageBoxShow("قرارداد شخص پیدا نشد", ((CacheBaseForm)this).ContextGuid);
				WebErrorLog.Instance.StartLog($"قرارداد شخص شناسایی نشد . علت بررسی شود \r\n method=FrmReportMorakhasi.LoadData(),gharardadId={_xclsGharardad.ID},line=44", "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FrmReportMorakhasi.cs", 78);
				return;
			}
			_lstMorakhasiSaati = tuple.Item5;
			_startDateSh = _xclsGharardad.DateStartM;
			_endDateSh = _xclsGharardad.DateFinishM;
			((Control)LblCodeTafsil).Text = ((PersonBase)_xclsGharardad.ClsPerson).CodeTafsil.ToString();
			((Control)LblFullName).Text = _xclsGharardad.FullName;
			((Control)LblGharardadNo).Text = _xclsGharardad.No;
			((Control)LblMorakhasiOnGharardad).Text = _xclsGharardad.TotalLeaveDaysInGharardad.ToString();
			((Control)LblVacationSaved).Text = _xclsGharardad.LeaveSaved.ToString();
			((Control)LblCanUsed).Text = (_xclsGharardad.LeaveSaved + _xclsGharardad.TotalLeaveDaysInGharardad).ToString();
			((Control)LblVacationUsed).Text = tuple.Item1.ToString();
			((Control)LblVacationSaatiTime).Text = (tuple.Item2 % 60).ToString().PadLeft(2, '0') + " : " + (tuple.Item2 / 60).ToString().PadLeft(2, '0');
			((Control)LblVacationSaatiDay).Text = ((double)tuple.Item2 / (double)_xclsGharardad.LeaveMinutes).ToString("0.00");
			((Control)LblValVacationLeft).Text = tuple.Item3.ToString("0.00");
			if (tuple.Item3 > 0.0)
			{
				((Control)LblValVacationLeft).ForeColor = Color.Green;
				((Control)LblVacationLeft).ForeColor = Color.Green;
			}
			else
			{
				((Control)LblValVacationLeft).ForeColor = Color.Red;
				((Control)LblVacationLeft).ForeColor = Color.Red;
			}
			int days = (_xclsGharardad.DateFinishM - _xclsGharardad.DateStartM).Days;
			double num = (double)((DateTime.Now > _xclsGharardad.DateFinishM) ? (_xclsGharardad.DateFinishM - _xclsGharardad.DateStartM).Days : (DateTime.Now - _xclsGharardad.DateStartM).Days) / (double)days * (_xclsGharardad.LeaveSaved + _xclsGharardad.TotalLeaveDaysInGharardad);
			((Control)TxtVacationToNow).Text = num.ToString("N2");
			((Control)LblAttention).Text = $"توجه: نامبرده طی کل مدت قرارداد  (از {_xclsGharardad.DateStartCalture}  الی  {_xclsGharardad.DateFinishCalture})  {((Control)LblCanUsed).Text} روز سهمیه مرخصی دارند اما به تناسب کارکردشان تا این لحظه فقط {num} روز حق استفاده از مرخصی را خواهند داشت.";
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FrmReportMorakhasi.cs", 116);
		}
	}

	private async void BtnDetailMorakhasi_Click(object sender, EventArgs e)
	{
		try
		{
			FrmMorakhasiDailyMain frm = new FrmMorakhasiDailyMain(((CacheBaseForm)this).ContextGuid, _font, new List<int>(1) { _xclsGharardad.PersonId }, _startDateSh, _endDateSh, "", isCalcSaati: false);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "BtnDetailMorakhasi_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FrmReportMorakhasi.cs");
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDetailMorakhasi_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FrmReportMorakhasi.cs", 134);
		}
	}

	private async void BtnDetailSaati_Click(object sender, EventArgs e)
	{
		try
		{
			FrmMor_SaatiList frm = new FrmMor_SaatiList(((CacheBaseForm)this).ContextGuid, _lstMorakhasiSaati);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "BtnDetailSaati_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FrmReportMorakhasi.cs");
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDetailSaati_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FrmReportMorakhasi.cs", 151);
		}
	}

	private void BtnAccept_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	protected override void OnLoad(EventArgs e)
	{
		if (!UserAttendance.SelectedUser.AccessLevelMerg.Vacation.VacationMain)
		{
			((Form)this).Close();
			return;
		}
		((BaseForm)this).OnLoad(e);
		if (_xclsGharardad.ID <= 0)
		{
			Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)5, "[شخص مورد نظر در این تاریخ قراردادی ندارد و نمیتوان مانده مرخصی را محاسبه نمود]", "", 3);
			((Form)this).Close();
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
		//IL_128c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1296: Expected O, but got Unknown
		//IL_14ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b6: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		panelEx2 = new PanelEx();
		LblAttention = new LabelX();
		BtnDetailSaati = new ButtonX();
		BtnDetailMorakhasi = new ButtonX();
		labelX6 = new LabelX();
		LblVacationLeft = new LabelX();
		labelX2 = new LabelX();
		LblVacationUsed = new LabelX();
		LblValVacationLeft = new LabelX();
		LblVacationSaatiDay = new LabelX();
		LblVacationSaatiTime = new LabelX();
		labelX1 = new LabelX();
		labelX18 = new LabelX();
		labelX8 = new LabelX();
		labelX11 = new LabelX();
		labelX10 = new LabelX();
		LblGharardadNo = new LabelX();
		LblCanUsed = new LabelX();
		LblVacationSaved = new LabelX();
		LblMorakhasiOnGharardad = new LabelX();
		labelX9 = new LabelX();
		LblCodeTafsil = new LabelX();
		LblFullName = new LabelX();
		labelX4 = new LabelX();
		BtnAccept = new ButtonX();
		labelx3 = new LabelX();
		TxtVacationToNow = new LabelX();
		((Control)panelEx1).SuspendLayout();
		((Control)panelEx2).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)labelx3);
		((Control)panelEx1).Controls.Add((Control)(object)TxtVacationToNow);
		((Control)panelEx1).Controls.Add((Control)(object)panelEx2);
		((Control)panelEx1).Controls.Add((Control)(object)labelX18);
		((Control)panelEx1).Controls.Add((Control)(object)labelX8);
		((Control)panelEx1).Controls.Add((Control)(object)labelX11);
		((Control)panelEx1).Controls.Add((Control)(object)labelX10);
		((Control)panelEx1).Controls.Add((Control)(object)LblGharardadNo);
		((Control)panelEx1).Controls.Add((Control)(object)LblCanUsed);
		((Control)panelEx1).Controls.Add((Control)(object)LblVacationSaved);
		((Control)panelEx1).Controls.Add((Control)(object)LblMorakhasiOnGharardad);
		((Control)panelEx1).Controls.Add((Control)(object)labelX9);
		((Control)panelEx1).Controls.Add((Control)(object)LblCodeTafsil);
		((Control)panelEx1).Controls.Add((Control)(object)LblFullName);
		((Control)panelEx1).Controls.Add((Control)(object)labelX4);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).RightToLeft = (RightToLeft)1;
		((Control)panelEx1).Size = new Size(428, 347);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.Color = Color.LightSkyBlue;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)panelEx2).Anchor = (AnchorStyles)13;
		panelEx2.CanvasColor = SystemColors.Control;
		panelEx2.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx2).Controls.Add((Control)(object)LblAttention);
		((Control)panelEx2).Controls.Add((Control)(object)BtnDetailSaati);
		((Control)panelEx2).Controls.Add((Control)(object)BtnDetailMorakhasi);
		((Control)panelEx2).Controls.Add((Control)(object)labelX6);
		((Control)panelEx2).Controls.Add((Control)(object)LblVacationLeft);
		((Control)panelEx2).Controls.Add((Control)(object)labelX2);
		((Control)panelEx2).Controls.Add((Control)(object)LblVacationUsed);
		((Control)panelEx2).Controls.Add((Control)(object)LblValVacationLeft);
		((Control)panelEx2).Controls.Add((Control)(object)LblVacationSaatiDay);
		((Control)panelEx2).Controls.Add((Control)(object)LblVacationSaatiTime);
		((Control)panelEx2).Controls.Add((Control)(object)labelX1);
		panelEx2.DisabledBackColor = Color.Empty;
		((Control)panelEx2).Location = new Point(10, 148);
		((Control)panelEx2).Name = "panelEx2";
		((Control)panelEx2).Size = new Size(410, 148);
		panelEx2.Style.Alignment = (StringAlignment)1;
		panelEx2.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx2.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx2.Style.Border = (eBorderType)1;
		panelEx2.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx2.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx2.Style.GradientAngle = 90;
		((Control)panelEx2).TabIndex = 14;
		((Control)LblAttention).BackColor = Color.Transparent;
		((BaseItemControl)LblAttention).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblAttention).Dock = (DockStyle)2;
		((Control)LblAttention).ForeColor = Color.Crimson;
		((Control)LblAttention).Location = new Point(0, 82);
		((Control)LblAttention).Name = "LblAttention";
		LblAttention.PaddingBottom = 3;
		LblAttention.PaddingLeft = 3;
		LblAttention.PaddingRight = 3;
		LblAttention.PaddingTop = 3;
		((Control)LblAttention).RightToLeft = (RightToLeft)1;
		((Control)LblAttention).Size = new Size(410, 66);
		((Control)LblAttention).TabIndex = 6;
		((Control)LblAttention).Text = "توجه: نامبرده طی کل مدت قرارداد  (از 1403/01/01  الی  1403/12/29)  مدت 30 روز سهمیه استفاده از مرخصی دارند لذا به تناسب کارکردشان تا این لحظه فقط 10 روز حق استفاده از مرخصی خواهند داشت.";
		LblAttention.WordWrap = true;
		((Control)BtnDetailSaati).AccessibleRole = (AccessibleRole)43;
		BtnDetailSaati.ColorTable = (eButtonColor)6;
		BtnDetailSaati.Image = (Image)(object)Resources.search2;
		((Control)BtnDetailSaati).Location = new Point(10, 36);
		((Control)BtnDetailSaati).Name = "BtnDetailSaati";
		((Control)BtnDetailSaati).Size = new Size(22, 22);
		((PopupItemControl)BtnDetailSaati).Style = (eDotNetBarStyle)9;
		((Control)BtnDetailSaati).TabIndex = 2;
		((Control)BtnDetailSaati).TabStop = false;
		((Control)BtnDetailSaati).Click += BtnDetailSaati_Click;
		((Control)BtnDetailMorakhasi).AccessibleRole = (AccessibleRole)43;
		BtnDetailMorakhasi.ColorTable = (eButtonColor)6;
		BtnDetailMorakhasi.Image = (Image)(object)Resources.search2;
		((Control)BtnDetailMorakhasi).Location = new Point(10, 10);
		((Control)BtnDetailMorakhasi).Name = "BtnDetailMorakhasi";
		((Control)BtnDetailMorakhasi).Size = new Size(22, 22);
		((PopupItemControl)BtnDetailMorakhasi).Style = (eDotNetBarStyle)9;
		((Control)BtnDetailMorakhasi).TabIndex = 2;
		((Control)BtnDetailMorakhasi).TabStop = false;
		((Control)BtnDetailMorakhasi).Click += BtnDetailMorakhasi_Click;
		((Control)labelX6).BackColor = Color.Transparent;
		((BaseItemControl)labelX6).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX6).Location = new Point(87, 33);
		((Control)labelX6).Name = "labelX6";
		((Control)labelX6).RightToLeft = (RightToLeft)1;
		((Control)labelX6).Size = new Size(34, 27);
		((Control)labelX6).TabIndex = 5;
		((Control)labelX6).Text = "معادل:";
		labelX6.TextAlignment = (StringAlignment)2;
		((Control)LblVacationLeft).BackColor = Color.Transparent;
		((BaseItemControl)LblVacationLeft).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblVacationLeft).ForeColor = Color.Red;
		((Control)LblVacationLeft).Location = new Point(177, 62);
		((Control)LblVacationLeft).Name = "LblVacationLeft";
		((Control)LblVacationLeft).RightToLeft = (RightToLeft)1;
		((Control)LblVacationLeft).Size = new Size(180, 21);
		((Control)LblVacationLeft).TabIndex = 3;
		((Control)LblVacationLeft).Text = "[مانده مرخصی استحقاقی]:";
		LblVacationLeft.TextAlignment = (StringAlignment)2;
		((Control)labelX2).BackColor = Color.Transparent;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(177, 33);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).RightToLeft = (RightToLeft)1;
		((Control)labelX2).Size = new Size(189, 27);
		((Control)labelX2).TabIndex = 3;
		((Control)labelX2).Text = "[جمع مرخصی ساعتی استفاده شده]:";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)LblVacationUsed).BackColor = Color.White;
		((BaseItemControl)LblVacationUsed).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblVacationUsed).Location = new Point(43, 10);
		((Control)LblVacationUsed).Name = "LblVacationUsed";
		((Control)LblVacationUsed).Size = new Size(128, 21);
		((Control)LblVacationUsed).TabIndex = 0;
		LblVacationUsed.TextAlignment = (StringAlignment)1;
		((Control)LblValVacationLeft).BackColor = Color.White;
		((BaseItemControl)LblValVacationLeft).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblValVacationLeft).Location = new Point(43, 62);
		((Control)LblValVacationLeft).Name = "LblValVacationLeft";
		((Control)LblValVacationLeft).Size = new Size(129, 21);
		((Control)LblValVacationLeft).TabIndex = 0;
		LblValVacationLeft.TextAlignment = (StringAlignment)1;
		((Control)LblVacationSaatiDay).BackColor = Color.White;
		((BaseItemControl)LblVacationSaatiDay).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblVacationSaatiDay).Location = new Point(43, 36);
		((Control)LblVacationSaatiDay).Name = "LblVacationSaatiDay";
		((Control)LblVacationSaatiDay).Size = new Size(39, 21);
		((Control)LblVacationSaatiDay).TabIndex = 0;
		LblVacationSaatiDay.TextAlignment = (StringAlignment)1;
		((Control)LblVacationSaatiTime).BackColor = Color.White;
		((BaseItemControl)LblVacationSaatiTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblVacationSaatiTime).Location = new Point(131, 36);
		((Control)LblVacationSaatiTime).Name = "LblVacationSaatiTime";
		((Control)LblVacationSaatiTime).Size = new Size(41, 21);
		((Control)LblVacationSaatiTime).TabIndex = 0;
		LblVacationSaatiTime.TextAlignment = (StringAlignment)1;
		((Control)labelX1).BackColor = Color.Transparent;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(177, 7);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).RightToLeft = (RightToLeft)1;
		((Control)labelX1).Size = new Size(190, 27);
		((Control)labelX1).TabIndex = 0;
		((Control)labelX1).Text = "جمع مرخصی استحقاقی استفاده شده:";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)labelX18).BackColor = Color.Transparent;
		((BaseItemControl)labelX18).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX18).Location = new Point(77, 66);
		((Control)labelX18).Name = "labelX18";
		((Control)labelX18).RightToLeft = (RightToLeft)1;
		((Control)labelX18).Size = new Size(190, 21);
		((Control)labelX18).TabIndex = 9;
		((Control)labelX18).Text = "تعداد روز ذخیره مرخصی از قرارداد قبل :";
		labelX18.TextAlignment = (StringAlignment)2;
		((Control)labelX8).BackColor = Color.Transparent;
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).Location = new Point(77, 93);
		((Control)labelX8).Name = "labelX8";
		((Control)labelX8).RightToLeft = (RightToLeft)1;
		((Control)labelX8).Size = new Size(277, 21);
		((Control)labelX8).TabIndex = 11;
		((Control)labelX8).Text = "جمع مرخصی استحقاقی قابل استفاده در کل دوره قرارداد :";
		labelX8.TextAlignment = (StringAlignment)2;
		((Control)labelX11).Anchor = (AnchorStyles)9;
		((Control)labelX11).BackColor = Color.Transparent;
		((BaseItemControl)labelX11).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX11).Location = new Point(302, 39);
		((Control)labelX11).Name = "labelX11";
		((Control)labelX11).RightToLeft = (RightToLeft)1;
		((Control)labelX11).Size = new Size(89, 21);
		((Control)labelX11).TabIndex = 4;
		((Control)labelX11).Text = "شماره قرارداد :";
		labelX11.TextAlignment = (StringAlignment)2;
		((Control)labelX10).BackColor = Color.Transparent;
		((BaseItemControl)labelX10).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX10).Location = new Point(77, 39);
		((Control)labelX10).Name = "labelX10";
		((Control)labelX10).RightToLeft = (RightToLeft)1;
		((Control)labelX10).Size = new Size(146, 21);
		((Control)labelX10).TabIndex = 6;
		((Control)labelX10).Text = "[مقدار مرخصی در بازه قرارداد :]";
		labelX10.TextAlignment = (StringAlignment)2;
		((Control)LblGharardadNo).Anchor = (AnchorStyles)9;
		((Control)LblGharardadNo).BackColor = Color.White;
		((BaseItemControl)LblGharardadNo).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblGharardadNo).Location = new Point(235, 39);
		((Control)LblGharardadNo).Name = "LblGharardadNo";
		((Control)LblGharardadNo).Size = new Size(62, 21);
		((Control)LblGharardadNo).TabIndex = 0;
		LblGharardadNo.TextAlignment = (StringAlignment)1;
		((Control)LblCanUsed).BackColor = Color.White;
		((BaseItemControl)LblCanUsed).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblCanUsed).Location = new Point(10, 93);
		((Control)LblCanUsed).Name = "LblCanUsed";
		((Control)LblCanUsed).Size = new Size(62, 21);
		((Control)LblCanUsed).TabIndex = 0;
		LblCanUsed.TextAlignment = (StringAlignment)1;
		((Control)LblVacationSaved).BackColor = Color.White;
		((BaseItemControl)LblVacationSaved).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblVacationSaved).Location = new Point(10, 66);
		((Control)LblVacationSaved).Name = "LblVacationSaved";
		((Control)LblVacationSaved).Size = new Size(62, 21);
		((Control)LblVacationSaved).TabIndex = 0;
		LblVacationSaved.TextAlignment = (StringAlignment)1;
		((Control)LblMorakhasiOnGharardad).BackColor = Color.White;
		((BaseItemControl)LblMorakhasiOnGharardad).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblMorakhasiOnGharardad).Location = new Point(10, 39);
		((Control)LblMorakhasiOnGharardad).Name = "LblMorakhasiOnGharardad";
		((Control)LblMorakhasiOnGharardad).Size = new Size(62, 21);
		((Control)LblMorakhasiOnGharardad).TabIndex = 0;
		LblMorakhasiOnGharardad.TextAlignment = (StringAlignment)1;
		((Control)labelX9).BackColor = Color.Transparent;
		((BaseItemControl)labelX9).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX9).Location = new Point(77, 12);
		((Control)labelX9).Name = "labelX9";
		((Control)labelX9).RightToLeft = (RightToLeft)1;
		((Control)labelX9).Size = new Size(58, 21);
		((Control)labelX9).TabIndex = 2;
		((Control)labelX9).Text = "[کد تفصیل]:";
		labelX9.TextAlignment = (StringAlignment)2;
		((Control)LblCodeTafsil).BackColor = Color.White;
		((BaseItemControl)LblCodeTafsil).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblCodeTafsil).Location = new Point(10, 12);
		((Control)LblCodeTafsil).Name = "LblCodeTafsil";
		((Control)LblCodeTafsil).Size = new Size(62, 21);
		((Control)LblCodeTafsil).TabIndex = 0;
		LblCodeTafsil.TextAlignment = (StringAlignment)1;
		((Control)LblFullName).Anchor = (AnchorStyles)9;
		((Control)LblFullName).BackColor = Color.White;
		((BaseItemControl)LblFullName).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblFullName).Location = new Point(136, 10);
		((Control)LblFullName).Name = "LblFullName";
		((Control)LblFullName).Size = new Size(161, 21);
		((Control)LblFullName).TabIndex = 0;
		((Control)labelX4).Anchor = (AnchorStyles)9;
		((Control)labelX4).BackColor = Color.Transparent;
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Location = new Point(302, 12);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).RightToLeft = (RightToLeft)1;
		((Control)labelX4).Size = new Size(91, 21);
		((Control)labelX4).TabIndex = 0;
		((Control)labelX4).Text = "نام شخص:";
		labelX4.TextAlignment = (StringAlignment)2;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)14;
		BtnAccept.ColorTable = (eButtonColor)6;
		BtnAccept.DialogResult = (DialogResult)2;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(137, 305);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(146, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 15;
		((Control)BtnAccept).Text = "[تائید]";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((Control)labelx3).BackColor = Color.Transparent;
		((BaseItemControl)labelx3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelx3).ForeColor = Color.MediumBlue;
		((Control)labelx3).Location = new Point(77, 120);
		((Control)labelx3).Name = "labelx3";
		((Control)labelx3).RightToLeft = (RightToLeft)1;
		((Control)labelx3).Size = new Size(216, 21);
		((Control)labelx3).TabIndex = 19;
		((Control)labelx3).Text = "حق مرخصی مجاز به استفاده تا این لحظه:";
		labelx3.TextAlignment = (StringAlignment)2;
		((Control)TxtVacationToNow).BackColor = Color.White;
		((BaseItemControl)TxtVacationToNow).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)TxtVacationToNow).Location = new Point(10, 120);
		((Control)TxtVacationToNow).Name = "TxtVacationToNow";
		((Control)TxtVacationToNow).Size = new Size(62, 21);
		((Control)TxtVacationToNow).TabIndex = 18;
		TxtVacationToNow.TextAlignment = (StringAlignment)1;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)BtnAccept;
		((Form)this).ClientSize = new Size(428, 347);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmReportMorakhasi";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[شرح مرخصی ساعتی]";
		((Control)panelEx1).ResumeLayout(false);
		((Control)panelEx2).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}

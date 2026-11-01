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
using Serilog;
using TimeAttendance.ValidatorAttribute.Salary.Codeing;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Salary.Forms.Codeing;

public class FrmTaxTable : FormExtention
{
	private TaxTable _clsTaxTable;

	private string Frm_Text = "[افزودن میزان مالیات بر درآمد حقوق ماهانه]";

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private bool _isDisposed;

	private IContainer components;

	private PanelEx panelEx1;

	private TextBoxFa TxtFromPrice;

	private LabelX labelX1;

	private LabelX labelX5;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private LabelX labelX7;

	private TextBoxFa TxtToPrice;

	internal DateBoxFa TxtStartDate;

	private LabelX labelX3;

	private TextBoxX TxtDescription;

	internal DateBoxFa TxtEndDate;

	private LabelX labelX6;

	private NumericUpDown TxtYear;

	private LabelX labelX8;

	private TextBoxX TxtTaxPrecentage;

	private TextBoxFa TxtInitValue;

	private LabelX labelX10;

	private LabelX labelX9;

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
				((FormExtention)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public FrmTaxTable(string contextGuid)
		: base(contextGuid, true, true)
	{
		try
		{
			InitializeComponent();
			_clsTaxTable = new TaxTable(((CacheBaseForm)this).ContextGuid);
			Frm_Text = "[افزودن میزان مالیات بر درآمد حقوق ماهانه]";
			FillForm();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FrmTaxTable.cs", 63);
		}
	}

	public FrmTaxTable(string contextGuid, int id)
		: base(contextGuid, true, true)
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		_loadDataToken?.Cancel();
		_loadDataToken = new CancellationTokenSource();
		CancellationToken token = _loadDataToken.Token;
		new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(id, token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FrmTaxTable.cs");
		FillForm();
		Frm_Text = "[ویرایش میزان مالیات بر درآمد حقوق ماهانه]";
	}

	private async Task LoadDataAsync(int taxTableId, CancellationToken token)
	{
		try
		{
			if (taxTableId > 0)
			{
				_clsTaxTable = await TaxTable.GetAsync(((CacheBaseForm)this).ContextGuid, taxTableId, token);
			}
			else
			{
				_clsTaxTable = new TaxTable(((CacheBaseForm)this).ContextGuid);
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	private void FillForm()
	{
		((Control)TxtYear).Text = _clsTaxTable.Year.ToString();
		((Control)TxtStartDate).Text = Calendar.MiladiToCalture(_clsTaxTable.StartDateM);
		((Control)TxtEndDate).Text = Calendar.MiladiToCalture(_clsTaxTable.EndDateM);
		((Control)TxtFromPrice).Text = _clsTaxTable.FromPrice.ToString();
		((Control)TxtToPrice).Text = _clsTaxTable.ToPrice.ToString();
		((Control)TxtTaxPrecentage).Text = _clsTaxTable.TaxPercentage.ToString();
		((Control)TxtInitValue).Text = _clsTaxTable.InitialValue.ToString();
		((Control)TxtDescription).Text = _clsTaxTable.Description;
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		((FormExtention)this).OnLoad(e);
		((FormExtention)this).OK_Click += new CrmEventHandler(BtnAccept_Click);
		((Control)BtnCancel).Click += base.BtnCancel_Click;
		base.objsCanEmpty.Add((Control)(object)TxtDescription);
		base.objsCanEmpty.Add((Control)(object)TxtInitValue);
	}

	private void GetData()
	{
		_clsTaxTable.Year = Utilities.ParseToShort(((Control)TxtYear).Text, (short)0);
		_clsTaxTable.StartDateM = TxtStartDate.MiladiDate;
		_clsTaxTable.EndDateM = TxtEndDate.MiladiDate;
		_clsTaxTable.FromPrice = Utilities.ParseToDecimal(((Control)TxtFromPrice).Text, 0m, 0);
		_clsTaxTable.ToPrice = Utilities.ParseToDecimal(((Control)TxtToPrice).Text, 0m, 0);
		_clsTaxTable.TaxPercentage = Utilities.ParseToDecimal(((Control)TxtTaxPrecentage).Text, 0m, 0);
		_clsTaxTable.InitialValue = Utilities.ParseToDecimal(((Control)TxtInitValue).Text, 0m, 0);
		_clsTaxTable.Description = ((Control)TxtDescription).Text;
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		try
		{
			GetData();
			DialogResult val = TaxTableValidation.CheckValidation(((CacheBaseForm)this).ContextGuid, _clsTaxTable, isShowMessage: true);
			if ((int)val == 6 || (int)val == 5)
			{
				((Form)this).DialogResult = (DialogResult)((await _clsTaxTable.SaveAsync()) ? 1 : 2);
				((FormExtention)this).ResetControlChanged();
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FrmTaxTable.cs", 146);
		}
	}

	private void FrmTaxTable_Load(object sender, EventArgs e)
	{
		try
		{
			Frm_Text = Utilities.GetTranslate(Frm_Text);
			((Control)this).Text = Frm_Text;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmTaxTable_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FrmTaxTable.cs", 159);
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
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Expected O, but got Unknown
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0454: Expected O, but got Unknown
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0802: Unknown result type (might be due to invalid IL or missing references)
		//IL_080c: Expected O, but got Unknown
		//IL_083f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0849: Expected O, but got Unknown
		//IL_0a60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6a: Expected O, but got Unknown
		//IL_0b3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b45: Expected O, but got Unknown
		//IL_0c2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f58: Unknown result type (might be due to invalid IL or missing references)
		//IL_1151: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_126c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1312: Unknown result type (might be due to invalid IL or missing references)
		//IL_131c: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		TxtEndDate = new DateBoxFa();
		labelX6 = new LabelX();
		TxtYear = new NumericUpDown();
		labelX3 = new LabelX();
		TxtDescription = new TextBoxX();
		TxtStartDate = new DateBoxFa();
		TxtTaxPrecentage = new TextBoxX();
		labelX7 = new LabelX();
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		labelX5 = new LabelX();
		TxtInitValue = new TextBoxFa();
		TxtToPrice = new TextBoxFa();
		labelX10 = new LabelX();
		TxtFromPrice = new TextBoxFa();
		labelX9 = new LabelX();
		labelX8 = new LabelX();
		labelX1 = new LabelX();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)TxtYear).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)TxtEndDate);
		((Control)panelEx1).Controls.Add((Control)(object)labelX6);
		((Control)panelEx1).Controls.Add((Control)(object)TxtYear);
		((Control)panelEx1).Controls.Add((Control)(object)labelX3);
		((Control)panelEx1).Controls.Add((Control)(object)TxtDescription);
		((Control)panelEx1).Controls.Add((Control)(object)TxtStartDate);
		((Control)panelEx1).Controls.Add((Control)(object)TxtTaxPrecentage);
		((Control)panelEx1).Controls.Add((Control)(object)labelX7);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Controls.Add((Control)(object)labelX5);
		((Control)panelEx1).Controls.Add((Control)(object)TxtInitValue);
		((Control)panelEx1).Controls.Add((Control)(object)TxtToPrice);
		((Control)panelEx1).Controls.Add((Control)(object)labelX10);
		((Control)panelEx1).Controls.Add((Control)(object)TxtFromPrice);
		((Control)panelEx1).Controls.Add((Control)(object)labelX9);
		((Control)panelEx1).Controls.Add((Control)(object)labelX8);
		((Control)panelEx1).Controls.Add((Control)(object)labelX1);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Margin = new Padding(0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(402, 310);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		TxtEndDate.AllowNull = false;
		TxtEndDate.BackcolorTooltip = Color.Red;
		((TextBoxX)TxtEndDate).Border.CornerType = (eCornerType)1;
		TxtEndDate.EnterToTab = true;
		TxtEndDate.EscToClose = true;
		TxtEndDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		TxtEndDate.FocusBackColor = Color.Yellow;
		TxtEndDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TxtEndDate.FocusForeColor = Color.Blue;
		TxtEndDate.FocusTextSelect = true;
		((Control)TxtEndDate).Font = new Font("Tahoma", 11.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)TxtEndDate).Location = new Point(13, 41);
		TxtEndDate.MessageEmptyShowDialog = false;
		TxtEndDate.MessegeEmpty = "";
		TxtEndDate.MessegeEmptyInFormRight = true;
		TxtEndDate.MessegeEmptyShowInForm = false;
		TxtEndDate.MiladiDate = new DateTime(2021, 12, 19, 23, 8, 36, 922);
		((Control)TxtEndDate).Name = "TxtEndDate";
		((Control)TxtEndDate).RightToLeft = (RightToLeft)0;
		((Control)TxtEndDate).Size = new Size(95, 26);
		((Control)TxtEndDate).TabIndex = 5;
		((Control)TxtEndDate).Text = "19/12/2021";
		((TextBox)TxtEndDate).TextAlign = (HorizontalAlignment)2;
		TxtEndDate.TootTipDelay = 500;
		((BaseItemControl)labelX6).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX6).Location = new Point(113, 41);
		((Control)labelX6).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX6).Name = "labelX6";
		((Control)labelX6).Size = new Size(72, 26);
		((Control)labelX6).TabIndex = 4;
		((Control)labelX6).Text = "[تاریخ پایان] :";
		labelX6.TextAlignment = (StringAlignment)2;
		((Control)TxtYear).Anchor = (AnchorStyles)13;
		((Control)TxtYear).Location = new Point(215, 14);
		TxtYear.Maximum = new decimal(new int[4] { 2010, 0, 0, 0 });
		TxtYear.Minimum = new decimal(new int[4] { 1397, 0, 0, 0 });
		((Control)TxtYear).Name = "TxtYear";
		((Control)TxtYear).Size = new Size(95, 21);
		((Control)TxtYear).TabIndex = 1;
		((UpDownBase)TxtYear).TextAlign = (HorizontalAlignment)2;
		TxtYear.Value = new decimal(new int[4] { 1397, 0, 0, 0 });
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(316, 125);
		((Control)labelX3).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(60, 19);
		((Control)labelX3).TabIndex = 14;
		((Control)labelX3).Text = "[شرح] :";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)TxtDescription).Anchor = (AnchorStyles)15;
		TxtDescription.Border.Class = "TextBoxBorder";
		TxtDescription.Border.CornerType = (eCornerType)1;
		((Control)TxtDescription).Location = new Point(12, 127);
		((TextBoxBase)TxtDescription).Multiline = true;
		((Control)TxtDescription).Name = "TxtDescription";
		((Control)TxtDescription).Size = new Size(297, 113);
		((Control)TxtDescription).TabIndex = 15;
		TxtStartDate.AllowNull = false;
		((Control)TxtStartDate).Anchor = (AnchorStyles)13;
		TxtStartDate.BackcolorTooltip = Color.Red;
		((TextBoxX)TxtStartDate).Border.CornerType = (eCornerType)1;
		TxtStartDate.EnterToTab = true;
		TxtStartDate.EscToClose = true;
		TxtStartDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		TxtStartDate.FocusBackColor = Color.Yellow;
		TxtStartDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TxtStartDate.FocusForeColor = Color.Blue;
		TxtStartDate.FocusTextSelect = true;
		((Control)TxtStartDate).Font = new Font("Tahoma", 11.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)TxtStartDate).Location = new Point(215, 41);
		TxtStartDate.MessageEmptyShowDialog = false;
		TxtStartDate.MessegeEmpty = "";
		TxtStartDate.MessegeEmptyInFormRight = true;
		TxtStartDate.MessegeEmptyShowInForm = false;
		TxtStartDate.MiladiDate = new DateTime(2021, 12, 19, 23, 8, 37, 122);
		((Control)TxtStartDate).Name = "TxtStartDate";
		((Control)TxtStartDate).RightToLeft = (RightToLeft)0;
		((Control)TxtStartDate).Size = new Size(95, 26);
		((Control)TxtStartDate).TabIndex = 3;
		((Control)TxtStartDate).Text = "19/12/2021";
		((TextBox)TxtStartDate).TextAlign = (HorizontalAlignment)2;
		TxtStartDate.TootTipDelay = 500;
		((Control)TxtTaxPrecentage).Anchor = (AnchorStyles)13;
		TxtTaxPrecentage.Border.Class = "TextBoxBorder";
		TxtTaxPrecentage.Border.CornerType = (eCornerType)1;
		((Control)TxtTaxPrecentage).Location = new Point(215, 100);
		((Control)TxtTaxPrecentage).Name = "TxtTaxPrecentage";
		((Control)TxtTaxPrecentage).Size = new Size(95, 21);
		((Control)TxtTaxPrecentage).TabIndex = 11;
		((Control)labelX7).Anchor = (AnchorStyles)9;
		((Control)labelX7).BackColor = Color.Transparent;
		((BaseItemControl)labelX7).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX7).Location = new Point(316, 12);
		((Control)labelX7).Name = "labelX7";
		((Control)labelX7).Size = new Size(43, 21);
		((Control)labelX7).TabIndex = 0;
		((Control)labelX7).Text = "[سال] :";
		labelX7.TextAlignment = (StringAlignment)2;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(12, 246);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(117, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 17;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو]   (Esc)";
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(135, 246);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(111, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 16;
		((Control)BtnAccept).Text = "[ثبت]  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((Control)labelX5).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Location = new Point(316, 41);
		((Control)labelX5).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(73, 26);
		((Control)labelX5).TabIndex = 2;
		((Control)labelX5).Text = "[تاریخ شروع] :";
		labelX5.TextAlignment = (StringAlignment)2;
		TxtInitValue.AllowEmpty = true;
		TxtInitValue.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtInitValue.BackColorTextBoxWhenLostFocus = Color.White;
		TxtInitValue.BackcolorTooltip = Color.Red;
		TxtInitValue.CurrencyFormat = "";
		TxtInitValue.ForeColorTooltip = Color.Black;
		TxtInitValue.GetColoresFromSettingDLL = true;
		TxtInitValue.HandleKeydown = true;
		((Control)TxtInitValue).Location = new Point(12, 100);
		((TextBoxBase)TxtInitValue).MaxLength = 19;
		((Control)TxtInitValue).Name = "TxtInitValue";
		TxtInitValue.SendShiftTabAfterDown = true;
		TxtInitValue.SendShiftTabAfterEnter = true;
		TxtInitValue.SendTabAfterEnter = true;
		TxtInitValue.SendTabAfterUP = true;
		TxtInitValue.ShowHorofi = (ENShowHorofi)3;
		TxtInitValue.ShowToolTipText = true;
		((Control)TxtInitValue).Size = new Size(95, 21);
		((Control)TxtInitValue).TabIndex = 13;
		((Control)TxtInitValue).Text = "0";
		TxtInitValue.ToolTipPostFix = "";
		TxtInitValue.TootTipDelay = 1000;
		TxtInitValue.Value = new decimal(new int[4]);
		TxtToPrice.AllowEmpty = true;
		TxtToPrice.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtToPrice.BackColorTextBoxWhenLostFocus = Color.White;
		TxtToPrice.BackcolorTooltip = Color.Red;
		TxtToPrice.CurrencyFormat = "";
		TxtToPrice.ForeColorTooltip = Color.Black;
		TxtToPrice.GetColoresFromSettingDLL = true;
		TxtToPrice.HandleKeydown = true;
		((Control)TxtToPrice).Location = new Point(12, 73);
		((TextBoxBase)TxtToPrice).MaxLength = 19;
		((Control)TxtToPrice).Name = "TxtToPrice";
		TxtToPrice.SendShiftTabAfterDown = true;
		TxtToPrice.SendShiftTabAfterEnter = true;
		TxtToPrice.SendTabAfterEnter = true;
		TxtToPrice.SendTabAfterUP = true;
		TxtToPrice.ShowHorofi = (ENShowHorofi)3;
		TxtToPrice.ShowToolTipText = true;
		((Control)TxtToPrice).Size = new Size(95, 21);
		((Control)TxtToPrice).TabIndex = 9;
		((Control)TxtToPrice).Text = "0";
		TxtToPrice.ToolTipPostFix = "";
		TxtToPrice.TootTipDelay = 1000;
		TxtToPrice.Value = new decimal(new int[4]);
		((BaseItemControl)labelX10).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX10).Location = new Point(113, 100);
		((Control)labelX10).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX10).Name = "labelX10";
		((Control)labelX10).Size = new Size(72, 21);
		((Control)labelX10).TabIndex = 12;
		((Control)labelX10).Text = "[مقدار اولیه] :";
		labelX10.TextAlignment = (StringAlignment)2;
		TxtFromPrice.AllowEmpty = true;
		((Control)TxtFromPrice).Anchor = (AnchorStyles)13;
		TxtFromPrice.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtFromPrice.BackColorTextBoxWhenLostFocus = Color.White;
		TxtFromPrice.BackcolorTooltip = Color.Red;
		TxtFromPrice.CurrencyFormat = "";
		TxtFromPrice.ForeColorTooltip = Color.Black;
		TxtFromPrice.GetColoresFromSettingDLL = true;
		TxtFromPrice.HandleKeydown = true;
		((Control)TxtFromPrice).Location = new Point(215, 73);
		((TextBoxBase)TxtFromPrice).MaxLength = 19;
		((Control)TxtFromPrice).Name = "TxtFromPrice";
		TxtFromPrice.SendShiftTabAfterDown = true;
		TxtFromPrice.SendShiftTabAfterEnter = true;
		TxtFromPrice.SendTabAfterEnter = true;
		TxtFromPrice.SendTabAfterUP = true;
		TxtFromPrice.ShowHorofi = (ENShowHorofi)3;
		TxtFromPrice.ShowToolTipText = true;
		((Control)TxtFromPrice).Size = new Size(95, 21);
		((Control)TxtFromPrice).TabIndex = 7;
		((Control)TxtFromPrice).Text = "0";
		TxtFromPrice.ToolTipPostFix = "";
		TxtFromPrice.TootTipDelay = 1000;
		TxtFromPrice.Value = new decimal(new int[4]);
		((Control)labelX9).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX9).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX9).Location = new Point(316, 100);
		((Control)labelX9).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX9).Name = "labelX9";
		((Control)labelX9).Size = new Size(74, 21);
		((Control)labelX9).TabIndex = 10;
		((Control)labelX9).Text = "[درصد مالیات] :";
		labelX9.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).Location = new Point(113, 73);
		((Control)labelX8).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX8).Name = "labelX8";
		((Control)labelX8).Size = new Size(52, 21);
		((Control)labelX8).TabIndex = 8;
		((Control)labelX8).Text = "[ToPrice] :";
		labelX8.TextAlignment = (StringAlignment)2;
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(316, 73);
		((Control)labelX1).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(52, 21);
		((Control)labelX1).TabIndex = 6;
		((Control)labelX1).Text = "[از مبلغ] :";
		labelX1.TextAlignment = (StringAlignment)2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(402, 310);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(418, 323);
		((Control)this).Name = "FrmTaxTable";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[افزودن مزایا و کسورات]";
		((Form)this).Load += FrmTaxTable_Load;
		((Control)panelEx1).ResumeLayout(false);
		((Control)panelEx1).PerformLayout();
		((ISupportInitialize)TxtYear).EndInit();
		((Control)this).ResumeLayout(false);
	}
}

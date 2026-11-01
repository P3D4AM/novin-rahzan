using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevComponents.Editors;
using FormCustom;
using FormCustom.Cache;
using FormCustom.CurrencyTextBox;
using PacketParser;
using PacketParser.Entities;
using TimeAttendance.ValidatorAttribute.Salary.Codeing;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceUI.Properties;
using TimeAttendanceUI.Salary.Shift;

namespace TimeAttendanceUI.Salary.Forms.Codeing;

public class FrmHesab : FormExtention
{
	private readonly FishHesab _clsFishHesab;

	private string Frm_Text = "[افزودن حساب]";

	private string trComboItem1 = "[حقوق و مزایا]";

	private string trComboItem2 = "[کسورات]";

	private string trComboItem3 = "[ســـایــر]";

	private IContainer components;

	private PanelEx panelEx1;

	private TextBoxX TXTTitle;

	private LabelX labelX2;

	private LabelX labelX4;

	private LabelX labelX3;

	private TextBoxFa TXTAccount;

	private LabelX lblPrice;

	internal ComboBoxEx CMBType;

	private TextBoxX TXTDescription;

	private LabelX labelX5;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private ComboItem comboItem1;

	private ComboItem comboItem2;

	private LabelX labelX6;

	private ComboItem comboItem3;

	private CheckBoxX ChBxTaxable;

	private CheckBoxX ChBxInsurance;

	private CheckBoxX chBxPrsnTafsilUsed;

	internal ButtonX BtnHelp;

	private LabelX labelX1;

	private Panel panelTafsilUsed;

	private TextBoxX NumCodeTafsil;

	private TextBoxX numHesabdariCode;

	public FrmHesab(string contextGuid, FishHesab fishHesab)
		: base(contextGuid, true, true)
	{
		try
		{
			InitializeComponent();
			_clsFishHesab = fishHesab;
			if (fishHesab.Id <= 0)
			{
				Frm_Text = "[افزودن حساب]";
			}
			else
			{
				Frm_Text = "[ویرایش حساب]";
			}
			FillForm();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FRMHesab.cs", 36);
		}
	}

	private void FillForm()
	{
		if (_clsFishHesab.Type >= 1 && _clsFishHesab.Type <= 3)
		{
			((ListControl)CMBType).SelectedIndex = _clsFishHesab.Type - 1;
		}
		if (_clsFishHesab.Type == 2)
		{
			((Control)panelTafsilUsed).Visible = true;
		}
		chBxPrsnTafsilUsed.Checked = _clsFishHesab.PrsnTafsilUsed;
		((Control)TXTTitle).Text = _clsFishHesab.Name;
		((Control)TXTAccount).Text = _clsFishHesab.Account.ToString(CultureInfo.InvariantCulture);
		((Control)TXTDescription).Text = _clsFishHesab.Description;
		((Control)NumCodeTafsil).Text = _clsFishHesab.CodeTafsil.ToString();
		((Control)numHesabdariCode).Text = _clsFishHesab.CodeMoein.ToString();
		ChBxTaxable.Checked = _clsFishHesab.Taxable;
		ChBxInsurance.Checked = _clsFishHesab.Insurance;
		if (_clsFishHesab.Id < 1000)
		{
			TextBoxX tXTTitle = TXTTitle;
			bool readOnly = (((Control)CMBType).Enabled = false);
			((TextBoxBase)tXTTitle).ReadOnly = readOnly;
			if (_clsFishHesab.Id == 5)
			{
				TextBoxFa tXTAccount = TXTAccount;
				readOnly = (((Control)ChBxInsurance).Enabled = false);
				((Control)tXTAccount).Enabled = readOnly;
			}
			if (_clsFishHesab.Id == 6)
			{
				TextBoxFa tXTAccount2 = TXTAccount;
				CheckBoxX chBxTaxable = ChBxTaxable;
				bool flag4 = (((Control)ChBxInsurance).Enabled = false);
				readOnly = (((Control)chBxTaxable).Enabled = flag4);
				((Control)tXTAccount2).Enabled = readOnly;
			}
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		((FormExtention)this).OnLoad(e);
		((FormExtention)this).OK_Click += new CrmEventHandler(BtnAccept_Click);
		((Control)BtnCancel).Click += base.BtnCancel_Click;
		base.objsCanEmpty.Add((Control)(object)TXTDescription);
		base.objsCanEmpty.Add((Control)(object)TXTAccount);
		comboItem1.Text = trComboItem1;
		comboItem2.Text = trComboItem2;
		comboItem3.Text = trComboItem3;
	}

	private void GetData()
	{
		_clsFishHesab.Type = (short)(((ListControl)CMBType).SelectedIndex + 1);
		_clsFishHesab.Name = Text.FixString(((Control)TXTTitle).Text);
		_clsFishHesab.Account = Utilities.ParseToDecimal(((Control)TXTAccount).Text, 0m, 0);
		_clsFishHesab.HtreeId = Utilities.ParseToInt(((Control)numHesabdariCode).Text, 0);
		_clsFishHesab.CodeMoein = (long.TryParse(((Control)numHesabdariCode).Text, out var result) ? result : 0);
		_clsFishHesab.CodeTafsil = (long.TryParse(((Control)NumCodeTafsil).Text, out var result2) ? result2 : 0);
		_clsFishHesab.Description = ((Control)TXTDescription).Text;
		_clsFishHesab.Taxable = ChBxTaxable.Checked;
		_clsFishHesab.Insurance = ChBxInsurance.Checked;
		_clsFishHesab.PrsnTafsilUsed = chBxPrsnTafsilUsed.Checked;
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			GetData();
			DialogResult val = await CodeingHesabCheckValidation.CheckValidationAsync(((CacheBaseForm)this).ContextGuid, _clsFishHesab, isShowMessage: true);
			if ((int)val == 6 || (int)val == 5)
			{
				((Form)this).DialogResult = (DialogResult)((await _clsFishHesab.SaveAsync()) ? 1 : 2);
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FRMHesab.cs", 117);
		}
	}

	private async void BtnHelp_Click(object sender, EventArgs e)
	{
		string translate = Utilities.GetTranslate("[در هنگام ثبت کسورات توافقی میتوانید از کد تفصیل شخص درثبت سند حقوق استفاده نمائید\r\nبرای اینکار گزینه (استفاده از کد تفصیل شخص) انتخاب نمائید. در این حالت در هنگام ثبت سند حقوق  \r\nبجای استفاده از کد تفصیلی این حساب، سیستم از کد تفصیل شخص مورد نظر استفاده مینماید\r\n]");
		await ((CacheBaseForm)new FrmHelpNextDay(((CacheBaseForm)this).ContextGuid, translate, Utilities.GetTranslate("[تعیین کد تفصیل]"))).ShowAsync((IWin32Window)(object)this);
	}

	private void chBxPrsnTafsilUsed_CheckedChanged(object sender, EventArgs e)
	{
		((Control)NumCodeTafsil).Enabled = !chBxPrsnTafsilUsed.Checked;
	}

	private void FrmHesab_Load(object sender, EventArgs e)
	{
		try
		{
			Frm_Text = Utilities.GetTranslate(Frm_Text);
			((Control)this).Text = Frm_Text;
			trComboItem1 = Utilities.GetTranslate(trComboItem1);
			trComboItem2 = Utilities.GetTranslate(trComboItem2);
			trComboItem3 = Utilities.GetTranslate(trComboItem3);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmHesab_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\Codeing\\FRMHesab.cs", 154);
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
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0849: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e1: Expected O, but got Unknown
		//IL_09b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bc: Expected O, but got Unknown
		//IL_0b32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fed: Unknown result type (might be due to invalid IL or missing references)
		//IL_10fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ad: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		NumCodeTafsil = new TextBoxX();
		numHesabdariCode = new TextBoxX();
		panelTafsilUsed = new Panel();
		chBxPrsnTafsilUsed = new CheckBoxX();
		labelX1 = new LabelX();
		BtnHelp = new ButtonX();
		ChBxTaxable = new CheckBoxX();
		ChBxInsurance = new CheckBoxX();
		labelX6 = new LabelX();
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		TXTDescription = new TextBoxX();
		labelX5 = new LabelX();
		CMBType = new ComboBoxEx();
		comboItem1 = new ComboItem();
		comboItem2 = new ComboItem();
		comboItem3 = new ComboItem();
		labelX4 = new LabelX();
		labelX3 = new LabelX();
		TXTAccount = new TextBoxFa();
		lblPrice = new LabelX();
		TXTTitle = new TextBoxX();
		labelX2 = new LabelX();
		((Control)panelEx1).SuspendLayout();
		((Control)panelTafsilUsed).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)NumCodeTafsil);
		((Control)panelEx1).Controls.Add((Control)(object)numHesabdariCode);
		((Control)panelEx1).Controls.Add((Control)(object)panelTafsilUsed);
		((Control)panelEx1).Controls.Add((Control)(object)ChBxTaxable);
		((Control)panelEx1).Controls.Add((Control)(object)ChBxInsurance);
		((Control)panelEx1).Controls.Add((Control)(object)labelX6);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Controls.Add((Control)(object)TXTDescription);
		((Control)panelEx1).Controls.Add((Control)(object)labelX5);
		((Control)panelEx1).Controls.Add((Control)(object)CMBType);
		((Control)panelEx1).Controls.Add((Control)(object)labelX4);
		((Control)panelEx1).Controls.Add((Control)(object)labelX3);
		((Control)panelEx1).Controls.Add((Control)(object)TXTAccount);
		((Control)panelEx1).Controls.Add((Control)(object)lblPrice);
		((Control)panelEx1).Controls.Add((Control)(object)TXTTitle);
		((Control)panelEx1).Controls.Add((Control)(object)labelX2);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Margin = new Padding(0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(404, 375);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)NumCodeTafsil).Anchor = (AnchorStyles)13;
		NumCodeTafsil.Border.Class = "TextBoxBorder";
		NumCodeTafsil.Border.CornerType = (eCornerType)1;
		((Control)NumCodeTafsil).Location = new Point(12, 12);
		((Control)NumCodeTafsil).Name = "NumCodeTafsil";
		((Control)NumCodeTafsil).Size = new Size(109, 21);
		((Control)NumCodeTafsil).TabIndex = 24;
		((Control)numHesabdariCode).Anchor = (AnchorStyles)13;
		numHesabdariCode.Border.Class = "TextBoxBorder";
		numHesabdariCode.Border.CornerType = (eCornerType)1;
		((Control)numHesabdariCode).Location = new Point(202, 12);
		((Control)numHesabdariCode).Name = "numHesabdariCode";
		((Control)numHesabdariCode).Size = new Size(110, 21);
		((Control)numHesabdariCode).TabIndex = 23;
		((Control)panelTafsilUsed).Controls.Add((Control)(object)chBxPrsnTafsilUsed);
		((Control)panelTafsilUsed).Controls.Add((Control)(object)labelX1);
		((Control)panelTafsilUsed).Controls.Add((Control)(object)BtnHelp);
		((Control)panelTafsilUsed).Location = new Point(12, 39);
		((Control)panelTafsilUsed).Name = "panelTafsilUsed";
		((Control)panelTafsilUsed).Size = new Size(298, 30);
		((Control)panelTafsilUsed).TabIndex = 22;
		((Control)panelTafsilUsed).Visible = false;
		((BaseItemControl)chBxPrsnTafsilUsed).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chBxPrsnTafsilUsed).Location = new Point(3, 3);
		((Control)chBxPrsnTafsilUsed).Name = "chBxPrsnTafsilUsed";
		((Control)chBxPrsnTafsilUsed).Size = new Size(166, 23);
		((BaseItemControl)chBxPrsnTafsilUsed).Style = (eDotNetBarStyle)9;
		((Control)chBxPrsnTafsilUsed).TabIndex = 4;
		((Control)chBxPrsnTafsilUsed).Text = "[استفاده از کد تفصیل شخص]";
		chBxPrsnTafsilUsed.CheckedChanged += chBxPrsnTafsilUsed_CheckedChanged;
		((Control)labelX1).AutoSize = true;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).ForeColor = Color.Red;
		((Control)labelX1).Location = new Point(167, 8);
		((Control)labelX1).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(30, 16);
		((Control)labelX1).TabIndex = 21;
		((Control)labelX1).Text = "<----";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)BtnHelp).AccessibleRole = (AccessibleRole)43;
		BtnHelp.ColorTable = (eButtonColor)6;
		((Control)BtnHelp).Location = new Point(204, 3);
		((Control)BtnHelp).Name = "BtnHelp";
		((Control)BtnHelp).Size = new Size(91, 23);
		((PopupItemControl)BtnHelp).Style = (eDotNetBarStyle)9;
		((Control)BtnHelp).TabIndex = 20;
		((Control)BtnHelp).TabStop = false;
		((Control)BtnHelp).Text = "[توضیح بیشتر]";
		((Control)BtnHelp).Click += BtnHelp_Click;
		((Control)ChBxTaxable).Anchor = (AnchorStyles)6;
		((BaseItemControl)ChBxTaxable).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChBxTaxable).Location = new Point(12, 283);
		((Control)ChBxTaxable).Name = "ChBxTaxable";
		((Control)ChBxTaxable).Size = new Size(108, 23);
		((BaseItemControl)ChBxTaxable).Style = (eDotNetBarStyle)9;
		((Control)ChBxTaxable).TabIndex = 14;
		((Control)ChBxTaxable).Text = "[مشمول مالیات]";
		((Control)ChBxInsurance).Anchor = (AnchorStyles)6;
		((BaseItemControl)ChBxInsurance).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChBxInsurance).Location = new Point(202, 283);
		((Control)ChBxInsurance).Name = "ChBxInsurance";
		((Control)ChBxInsurance).Size = new Size(108, 23);
		((BaseItemControl)ChBxInsurance).Style = (eDotNetBarStyle)9;
		((Control)ChBxInsurance).TabIndex = 13;
		((Control)ChBxInsurance).Text = "[مشمول بیمه]";
		((BaseItemControl)labelX6).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX6).Location = new Point(126, 12);
		((Control)labelX6).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX6).Name = "labelX6";
		((Control)labelX6).Size = new Size(59, 19);
		((Control)labelX6).TabIndex = 2;
		((Control)labelX6).Text = "[کد تفصیل]:";
		labelX6.TextAlignment = (StringAlignment)2;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(12, 312);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(117, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 16;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو]   (Esc)";
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(135, 312);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(111, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 15;
		((Control)BtnAccept).Text = "[ثبت]  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((Control)TXTDescription).Anchor = (AnchorStyles)15;
		TXTDescription.Border.Class = "TextBoxBorder";
		TXTDescription.Border.CornerType = (eCornerType)1;
		((Control)TXTDescription).Location = new Point(12, 149);
		((TextBoxBase)TXTDescription).Multiline = true;
		((Control)TXTDescription).Name = "TXTDescription";
		((Control)TXTDescription).Size = new Size(297, 130);
		((Control)TXTDescription).TabIndex = 12;
		((Control)labelX5).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Location = new Point(318, 149);
		((Control)labelX5).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(80, 19);
		((Control)labelX5).TabIndex = 11;
		((Control)labelX5).Text = "[شرح] :";
		labelX5.TextAlignment = (StringAlignment)2;
		((Control)CMBType).Anchor = (AnchorStyles)13;
		((ListControl)CMBType).DisplayMember = "name";
		((ComboBox)CMBType).DrawMode = (DrawMode)1;
		((ComboBox)CMBType).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CMBType).FormattingEnabled = true;
		((ComboBox)CMBType).ItemHeight = 16;
		CMBType.Items.AddRange(new object[3] { comboItem1, comboItem2, comboItem3 });
		((Control)CMBType).Location = new Point(12, 95);
		((Control)CMBType).Name = "CMBType";
		((Control)CMBType).Size = new Size(298, 22);
		CMBType.Style = (eDotNetBarStyle)9;
		((Control)CMBType).TabIndex = 8;
		((Control)CMBType).TabStop = false;
		((ListControl)CMBType).ValueMember = "id";
		comboItem1.FontSize = 12f;
		comboItem1.FontStyle = (FontStyle)1;
		comboItem1.Text = "[حقوق و مزایا]";
		comboItem1.TextAlignment = (StringAlignment)2;
		comboItem2.FontSize = 12f;
		comboItem2.FontStyle = (FontStyle)1;
		comboItem2.Text = "[کسورات]";
		comboItem2.TextAlignment = (StringAlignment)2;
		comboItem3.FontSize = 12f;
		comboItem3.FontStyle = (FontStyle)1;
		comboItem3.Text = "[ســـایــر]";
		comboItem3.TextAlignment = (StringAlignment)2;
		((Control)labelX4).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Location = new Point(318, 95);
		((Control)labelX4).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(80, 19);
		((Control)labelX4).TabIndex = 7;
		((Control)labelX4).Text = "[نوع] :";
		labelX4.TextAlignment = (StringAlignment)2;
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(318, 12);
		((Control)labelX3).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(56, 19);
		((Control)labelX3).TabIndex = 0;
		((Control)labelX3).Text = "[کد معین] :";
		labelX3.TextAlignment = (StringAlignment)2;
		TXTAccount.AllowEmpty = true;
		((Control)TXTAccount).Anchor = (AnchorStyles)13;
		TXTAccount.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTAccount.BackColorTextBoxWhenLostFocus = Color.White;
		TXTAccount.BackcolorTooltip = Color.Red;
		TXTAccount.CurrencyFormat = "";
		TXTAccount.ForeColorTooltip = Color.Black;
		TXTAccount.GetColoresFromSettingDLL = true;
		TXTAccount.HandleKeydown = true;
		((Control)TXTAccount).Location = new Point(12, 122);
		((TextBoxBase)TXTAccount).MaxLength = 19;
		((Control)TXTAccount).Name = "TXTAccount";
		TXTAccount.SendShiftTabAfterDown = true;
		TXTAccount.SendShiftTabAfterEnter = true;
		TXTAccount.SendTabAfterEnter = true;
		TXTAccount.SendTabAfterUP = true;
		TXTAccount.ShowHorofi = (ENShowHorofi)3;
		TXTAccount.ShowToolTipText = true;
		((Control)TXTAccount).Size = new Size(298, 21);
		((Control)TXTAccount).TabIndex = 10;
		((Control)TXTAccount).Text = "0";
		TXTAccount.ToolTipPostFix = "";
		TXTAccount.TootTipDelay = 1000;
		TXTAccount.Value = new decimal(new int[4]);
		((Control)lblPrice).Anchor = (AnchorStyles)9;
		((BaseItemControl)lblPrice).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblPrice).Location = new Point(318, 122);
		((Control)lblPrice).Margin = new Padding(3, 2, 3, 2);
		((Control)lblPrice).Name = "lblPrice";
		((Control)lblPrice).Size = new Size(80, 19);
		((Control)lblPrice).TabIndex = 9;
		((Control)lblPrice).Text = "[مبلغ] :";
		lblPrice.TextAlignment = (StringAlignment)2;
		((Control)TXTTitle).Anchor = (AnchorStyles)13;
		TXTTitle.Border.Class = "TextBoxBorder";
		TXTTitle.Border.CornerType = (eCornerType)1;
		((Control)TXTTitle).Location = new Point(12, 68);
		((Control)TXTTitle).Name = "TXTTitle";
		((Control)TXTTitle).Size = new Size(297, 21);
		((Control)TXTTitle).TabIndex = 6;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(318, 70);
		((Control)labelX2).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(80, 19);
		((Control)labelX2).TabIndex = 5;
		((Control)labelX2).Text = "[نام حساب] :";
		labelX2.TextAlignment = (StringAlignment)2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(404, 375);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(420, 382);
		((Control)this).Name = "FrmHesab";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[افزودن مزایا و کسورات]";
		((Form)this).Load += FrmHesab_Load;
		((Control)panelEx1).ResumeLayout(false);
		((Control)panelEx1).PerformLayout();
		((Control)panelTafsilUsed).ResumeLayout(false);
		((Control)panelTafsilUsed).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}

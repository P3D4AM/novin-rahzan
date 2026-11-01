using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevComponents.Editors;
using FormCustom;
using FormCustom.CurrencyTextBox;
using PacketParser;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Gharardads;

public class FrmGharardadDet : FormExtention
{
	private readonly GharardadDetNew _gharardadDet;

	private string frm_Text = "[ویرایش حساب در قرارداد خاص]";

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

	private NumericUpDown numHesabdariCode;

	private NumericUpDown NumCodeTafsil;

	private CheckBoxX ChBxIsDecreese;

	public FrmGharardadDet(string contextGuid, GharardadDetNew pGharardadDet)
		: base(contextGuid, true, true)
	{
		InitializeComponent();
		_gharardadDet = pGharardadDet;
		((Control)this).Text = "ویرایش حساب در قرارداد خاص";
		frm_Text = "[ویرایش حساب در قرارداد خاص]";
		LoadData();
		FillForm();
	}

	public void FillForm()
	{
		if (_gharardadDet.TypeAccount >= 1)
		{
			((ListControl)CMBType).SelectedIndex = _gharardadDet.TypeAccount - 1;
		}
		((Control)TXTTitle).Text = _gharardadDet.Name;
		((Control)TXTAccount).Text = _gharardadDet.nValue.ToString("N0");
		((Control)TXTDescription).Text = _gharardadDet.Desc;
		ChBxIsDecreese.Checked = _gharardadDet.IsDecrease;
	}

	private void LoadData()
	{
		((Control)CMBType).Enabled = false;
		((Control)TXTTitle).Enabled = false;
		if (_gharardadDet.HesabId == 5 || _gharardadDet.HesabId == 6)
		{
			((Control)TXTAccount).Enabled = false;
		}
		base.objsCanEmpty.Add((Control)(object)numHesabdariCode);
		base.objsCanEmpty.Add((Control)(object)NumCodeTafsil);
		base.objsCanEmpty.Add((Control)(object)TXTTitle);
		base.objsCanEmpty.Add((Control)(object)CMBType);
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		((FormExtention)this).OnLoad(e);
		((FormExtention)this).OK_Click += new CrmEventHandler(BtnAccept_Click);
		((Control)BtnCancel).Click += base.BtnCancel_Click;
	}

	private void BtnAccept_Click(object sender, EventArgs e)
	{
		if (!((Control)BtnAccept).Visible || !((Control)BtnAccept).Enabled)
		{
			return;
		}
		try
		{
			_gharardadDet.nValue = Utilities.ParseToDecimal(((Control)TXTAccount).Text, 0m, 0);
			_gharardadDet.IsDecrease = ChBxIsDecreese.Checked;
			_gharardadDet.Desc = ((Control)TXTDescription).Text;
			((Form)this).DialogResult = (DialogResult)1;
			((FormExtention)this).ResetControlChanged();
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardadDet.cs", 66);
		}
	}

	private void FrmGharardadDet_Load(object sender, EventArgs e)
	{
		try
		{
			frm_Text = Utilities.GetTranslate(frm_Text);
			comboItem1.Text = Utilities.GetTranslate("[حقوق و مزایا]");
			comboItem2.Text = Utilities.GetTranslate("[کسورات]");
			comboItem3.Text = Utilities.GetTranslate("[ســـایــر]");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmGharardadDet_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardadDet.cs", 81);
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
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_0636: Unknown result type (might be due to invalid IL or missing references)
		//IL_0640: Expected O, but got Unknown
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_072b: Expected O, but got Unknown
		//IL_08d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa4: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		ChBxIsDecreese = new CheckBoxX();
		NumCodeTafsil = new NumericUpDown();
		numHesabdariCode = new NumericUpDown();
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
		((ISupportInitialize)NumCodeTafsil).BeginInit();
		((ISupportInitialize)numHesabdariCode).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)ChBxIsDecreese);
		((Control)panelEx1).Controls.Add((Control)(object)NumCodeTafsil);
		((Control)panelEx1).Controls.Add((Control)(object)numHesabdariCode);
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
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Margin = new Padding(0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(404, 281);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((BaseItemControl)ChBxIsDecreese).BackgroundStyle.Class = "";
		((BaseItemControl)ChBxIsDecreese).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChBxIsDecreese).Location = new Point(14, 116);
		((Control)ChBxIsDecreese).Name = "ChBxIsDecreese";
		((Control)ChBxIsDecreese).Size = new Size(298, 23);
		((BaseItemControl)ChBxIsDecreese).Style = (eDotNetBarStyle)9;
		((Control)ChBxIsDecreese).TabIndex = 10;
		((Control)ChBxIsDecreese).Text = "[محاسبه بر اساس روزهای کاری]";
		((Control)NumCodeTafsil).Location = new Point(14, 8);
		NumCodeTafsil.Maximum = new decimal(new int[4] { 100000000, 0, 0, 0 });
		NumCodeTafsil.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)NumCodeTafsil).Name = "NumCodeTafsil";
		((Control)NumCodeTafsil).Size = new Size(114, 21);
		((Control)NumCodeTafsil).TabIndex = 3;
		((UpDownBase)NumCodeTafsil).TextAlign = (HorizontalAlignment)2;
		NumCodeTafsil.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)NumCodeTafsil).Visible = false;
		((Control)numHesabdariCode).Location = new Point(206, 8);
		numHesabdariCode.Maximum = new decimal(new int[4] { 10000000, 0, 0, 0 });
		numHesabdariCode.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)numHesabdariCode).Name = "numHesabdariCode";
		((Control)numHesabdariCode).Size = new Size(106, 21);
		((Control)numHesabdariCode).TabIndex = 1;
		((UpDownBase)numHesabdariCode).TextAlign = (HorizontalAlignment)2;
		numHesabdariCode.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)numHesabdariCode).Visible = false;
		((Control)labelX6).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX6).BackgroundStyle.Class = "";
		((BaseItemControl)labelX6).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX6).Location = new Point(134, 9);
		((Control)labelX6).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX6).Name = "labelX6";
		((Control)labelX6).Size = new Size(60, 19);
		((Control)labelX6).TabIndex = 2;
		((Control)labelX6).Text = "[کد تفصیلی] :";
		labelX6.TextAlignment = (StringAlignment)2;
		((Control)labelX6).Visible = false;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		BtnCancel.DialogResult = (DialogResult)2;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnCancel).ForeColor = Color.Black;
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(14, 244);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(117, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 16;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو] (Esc)";
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnAccept).ForeColor = Color.Black;
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(137, 244);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		BtnAccept.Shortcuts.Add((eShortcut)116);
		((Control)BtnAccept).Size = new Size(111, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 15;
		((Control)BtnAccept).Text = "[ذخیره](F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((Control)TXTDescription).Anchor = (AnchorStyles)15;
		TXTDescription.Border.Class = "TextBoxBorder";
		TXTDescription.Border.CornerType = (eCornerType)1;
		((Control)TXTDescription).Location = new Point(14, 145);
		((TextBoxBase)TXTDescription).Multiline = true;
		((Control)TXTDescription).Name = "TXTDescription";
		((Control)TXTDescription).Size = new Size(298, 93);
		((Control)TXTDescription).TabIndex = 12;
		((Control)labelX5).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX5).BackgroundStyle.Class = "";
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Location = new Point(320, 145);
		((Control)labelX5).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(80, 19);
		((Control)labelX5).TabIndex = 11;
		((Control)labelX5).Text = "[شرح] :";
		labelX5.TextAlignment = (StringAlignment)2;
		((ListControl)CMBType).DisplayMember = "name";
		((ComboBox)CMBType).DrawMode = (DrawMode)1;
		((ComboBox)CMBType).DropDownStyle = (ComboBoxStyle)2;
		((Control)CMBType).Enabled = false;
		((ListControl)CMBType).FormattingEnabled = true;
		((ComboBox)CMBType).ItemHeight = 15;
		CMBType.Items.AddRange(new object[3] { comboItem1, comboItem2, comboItem3 });
		((Control)CMBType).Location = new Point(14, 62);
		((Control)CMBType).Name = "CMBType";
		((Control)CMBType).Size = new Size(298, 21);
		CMBType.Style = (eDotNetBarStyle)9;
		((Control)CMBType).TabIndex = 7;
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
		((BaseItemControl)labelX4).BackgroundStyle.Class = "";
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Location = new Point(320, 62);
		((Control)labelX4).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(80, 19);
		((Control)labelX4).TabIndex = 6;
		((Control)labelX4).Text = "[نوع] :";
		labelX4.TextAlignment = (StringAlignment)2;
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX3).BackgroundStyle.Class = "";
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(318, 9);
		((Control)labelX3).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(80, 19);
		((Control)labelX3).TabIndex = 0;
		((Control)labelX3).Text = "[کد معین] :";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)labelX3).Visible = false;
		TXTAccount.AllowEmpty = true;
		((Control)TXTAccount).Anchor = (AnchorStyles)9;
		TXTAccount.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTAccount.BackColorTextBoxWhenLostFocus = Color.White;
		TXTAccount.BackcolorTooltip = Color.Red;
		TXTAccount.CurrencyFormat = "";
		TXTAccount.ForeColorTooltip = Color.Black;
		TXTAccount.GetColoresFromSettingDLL = true;
		TXTAccount.HandleKeydown = true;
		((Control)TXTAccount).Location = new Point(14, 89);
		((TextBoxBase)TXTAccount).MaxLength = 19;
		((Control)TXTAccount).Name = "TXTAccount";
		TXTAccount.SendShiftTabAfterDown = true;
		TXTAccount.SendShiftTabAfterEnter = true;
		TXTAccount.SendTabAfterEnter = true;
		TXTAccount.SendTabAfterUP = true;
		TXTAccount.ShowHorofi = (ENShowHorofi)3;
		TXTAccount.ShowToolTipText = true;
		((Control)TXTAccount).Size = new Size(298, 21);
		((Control)TXTAccount).TabIndex = 9;
		((Control)TXTAccount).Text = "0";
		TXTAccount.ToolTipPostFix = "";
		TXTAccount.TootTipDelay = 1000;
		((Control)lblPrice).Anchor = (AnchorStyles)9;
		((BaseItemControl)lblPrice).BackgroundStyle.Class = "";
		((BaseItemControl)lblPrice).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblPrice).Location = new Point(320, 89);
		((Control)lblPrice).Margin = new Padding(3, 2, 3, 2);
		((Control)lblPrice).Name = "lblPrice";
		((Control)lblPrice).Size = new Size(80, 19);
		((Control)lblPrice).TabIndex = 8;
		((Control)lblPrice).Text = "[مبلغ]:";
		lblPrice.TextAlignment = (StringAlignment)2;
		((Control)TXTTitle).Anchor = (AnchorStyles)9;
		TXTTitle.Border.Class = "TextBoxBorder";
		TXTTitle.Border.CornerType = (eCornerType)1;
		((Control)TXTTitle).Enabled = false;
		((Control)TXTTitle).Location = new Point(14, 35);
		((Control)TXTTitle).Name = "TXTTitle";
		((Control)TXTTitle).Size = new Size(298, 21);
		((Control)TXTTitle).TabIndex = 5;
		((BaseItemControl)labelX2).BackgroundStyle.Class = "";
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(320, 37);
		((Control)labelX2).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(80, 19);
		((Control)labelX2).TabIndex = 4;
		((Control)labelX2).Text = "[نام حساب]:";
		labelX2.TextAlignment = (StringAlignment)2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)BtnCancel;
		((Form)this).ClientSize = new Size(404, 303);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(420, 342);
		((Control)this).Name = "FrmGharardadDet";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[افزودن مزایا و کسورات]*";
		((Form)this).Load += FrmGharardadDet_Load;
		((Control)this).Controls.SetChildIndex((Control)(object)panelEx1, 0);
		((Control)panelEx1).ResumeLayout(false);
		((Control)panelEx1).PerformLayout();
		((ISupportInitialize)NumCodeTafsil).EndInit();
		((ISupportInitialize)numHesabdariCode).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}

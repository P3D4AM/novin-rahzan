using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.CurrencyTextBox;
using PacketParser;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Salary.Forms;

public class FrmAccountEdit : FormExtention
{
	private readonly FishDetJson _fishDet;

	private string Frm_Text = "[ویرایش مبلغ]";

	private IContainer components;

	private PanelEx panelEx1;

	private TextBoxX TXTTitle;

	private LabelX labelX2;

	private TextBoxFa TXTAccount;

	private LabelX labelX1;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private TextBoxX TxtDesc;

	private LabelX labelX3;

	public decimal AccountChange => Utilities.ParseToDecimal(((Control)TXTAccount).Text, 0m, 0);

	public string DescChange => ((Control)TxtDesc).Text;

	public FrmAccountEdit(string contextGuid, FishDetJson pdet)
		: base(contextGuid, true, true)
	{
		InitializeComponent();
		_fishDet = pdet;
		FillForm();
		Frm_Text = "[ویرایش مبلغ]";
	}

	private void FillForm()
	{
		((Control)TXTTitle).Text = _fishDet.Name;
		((Control)TXTAccount).Text = _fishDet.TmpValue.ToString(CultureInfo.InvariantCulture);
		((Control)TxtDesc).Text = _fishDet.Description;
		base.objsCanEmpty.Add((Control)(object)TxtDesc);
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
			((Form)this).DialogResult = (DialogResult)1;
			((FormExtention)this).ResetControlChanged();
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMAccountEdit.cs", 50);
		}
	}

	private void FrmAccountEdit_Load(object sender, EventArgs e)
	{
		try
		{
			Frm_Text = Utilities.GetTranslate(Frm_Text);
			((Control)this).Text = Frm_Text;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmAccountEdit_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMAccountEdit.cs", 64);
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
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Expected O, but got Unknown
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Expected O, but got Unknown
		//IL_06bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_088c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0896: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		TxtDesc = new TextBoxX();
		labelX3 = new LabelX();
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		TXTAccount = new TextBoxFa();
		labelX1 = new LabelX();
		TXTTitle = new TextBoxX();
		labelX2 = new LabelX();
		((Control)panelEx1).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)TxtDesc);
		((Control)panelEx1).Controls.Add((Control)(object)labelX3);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Controls.Add((Control)(object)TXTAccount);
		((Control)panelEx1).Controls.Add((Control)(object)labelX1);
		((Control)panelEx1).Controls.Add((Control)(object)TXTTitle);
		((Control)panelEx1).Controls.Add((Control)(object)labelX2);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Margin = new Padding(0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(293, 174);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)TxtDesc).Anchor = (AnchorStyles)9;
		TxtDesc.Border.Class = "TextBoxBorder";
		TxtDesc.Border.CornerType = (eCornerType)1;
		((Control)TxtDesc).Location = new Point(14, 67);
		((TextBoxBase)TxtDesc).Multiline = true;
		((Control)TxtDesc).Name = "TxtDesc";
		((Control)TxtDesc).Size = new Size(218, 33);
		((Control)TxtDesc).TabIndex = 5;
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(237, 66);
		((Control)labelX3).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(44, 19);
		((Control)labelX3).TabIndex = 4;
		((Control)labelX3).Text = "[شرح]:";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(14, 110);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(103, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 7;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو]   (Esc)";
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(129, 110);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(103, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 6;
		((Control)BtnAccept).Text = "[ذخیره]  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		TXTAccount.AllowEmpty = true;
		((Control)TXTAccount).Anchor = (AnchorStyles)9;
		TXTAccount.BackColorTextBoxWhenFocus = Color.Yellow;
		TXTAccount.BackColorTextBoxWhenLostFocus = Color.White;
		TXTAccount.BackcolorTooltip = Color.Red;
		TXTAccount.CurrencyFormat = "";
		TXTAccount.ForeColorTooltip = Color.Black;
		TXTAccount.GetColoresFromSettingDLL = true;
		TXTAccount.HandleKeydown = true;
		((Control)TXTAccount).Location = new Point(14, 39);
		((TextBoxBase)TXTAccount).MaxLength = 19;
		((Control)TXTAccount).Name = "TXTAccount";
		TXTAccount.SendShiftTabAfterDown = true;
		TXTAccount.SendShiftTabAfterEnter = true;
		TXTAccount.SendTabAfterEnter = true;
		TXTAccount.SendTabAfterUP = true;
		TXTAccount.ShowHorofi = (ENShowHorofi)3;
		TXTAccount.ShowToolTipText = true;
		((Control)TXTAccount).Size = new Size(218, 21);
		((Control)TXTAccount).TabIndex = 3;
		((Control)TXTAccount).Text = "0";
		TXTAccount.ToolTipPostFix = "";
		TXTAccount.TootTipDelay = 1000;
		TXTAccount.Value = new decimal(new int[4]);
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(237, 39);
		((Control)labelX1).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(53, 19);
		((Control)labelX1).TabIndex = 2;
		((Control)labelX1).Text = "[مبلغ] :";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)TXTTitle).Anchor = (AnchorStyles)9;
		TXTTitle.Border.Class = "TextBoxBorder";
		TXTTitle.Border.CornerType = (eCornerType)1;
		((Control)TXTTitle).Enabled = false;
		((Control)TXTTitle).Location = new Point(14, 12);
		((Control)TXTTitle).Name = "TXTTitle";
		((Control)TXTTitle).Size = new Size(218, 21);
		((Control)TXTTitle).TabIndex = 1;
		((Control)TXTTitle).TabStop = false;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(237, 12);
		((Control)labelX2).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(53, 19);
		((Control)labelX2).TabIndex = 0;
		((Control)labelX2).Text = "[عنوان]:";
		labelX2.TextAlignment = (StringAlignment)2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(293, 174);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmAccountEdit";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[افزودن مزایا و کسورات]*";
		((Form)this).Load += FrmAccountEdit_Load;
		((Control)panelEx1).ResumeLayout(false);
		((Control)panelEx1).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Forms;
using PacketParser;
using PacketParser.Entities;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Taradod;

public class FRMProgress : BaseForm
{
	private int _Total;

	private int _ItemCount;

	private string labelX1_Text = "[اطلاعات جدیدی جهت ثبت وجود ندارد]";

	private string trFrmProgressStr7 = "[اطلاعات شما با موفقیت دریافت شد]";

	private IContainer components;

	private GroupPanel groupPanel1;

	private LabelX labelX1;

	private ButtonX BtnCancel;

	private ProgressBarX progressBarX1;

	private ListBox ListErr;

	private LabelX LblKolTarradod;

	private LabelX LblErrTaradod;

	private LabelX LblNewTaradod;

	private LabelX labelX3;

	private LabelX labelX2;

	private LabelX labelX4;

	public int ItemCount
	{
		set
		{
			_ItemCount = value;
			_ = Percent;
		}
	}

	public int Percent
	{
		get
		{
			if (_Total != 0)
			{
				int num = _ItemCount * 100 / _Total;
				if (num < 0 || num > 100)
				{
					num = 0;
				}
				Application.DoEvents();
				progressBarX1.Value = num;
				if (num == 100)
				{
					labelX1_Text = trFrmProgressStr7;
					((Form)this).Close();
				}
				return num;
			}
			return 0;
		}
	}

	public FRMProgress(string contextGuid, int Total)
		: base(contextGuid, (BaseFormConfig)null)
	{
		InitializeComponent();
		_Total = Total;
		((Control)labelX2).Visible = false;
		((Control)labelX3).Visible = false;
		((Control)labelX4).Visible = false;
		((Control)ListErr).Visible = false;
		((Control)LblErrTaradod).Visible = false;
		((Control)LblKolTarradod).Visible = false;
		((Control)LblNewTaradod).Visible = false;
	}

	public FRMProgress(string contextGuid, int CountKol, int CountCor, int CountInCor, List<string> TxtErr)
		: base(contextGuid, (BaseFormConfig)null)
	{
		InitializeComponent();
		((Control)progressBarX1).Visible = false;
		((Control)this).Width = 382;
		((Control)this).Height = 333;
		if (CountCor <= 0)
		{
			labelX1_Text = "[اطلاعات جدیدی جهت ثبت وجود ندارد]";
			((Control)labelX1).ForeColor = Color.Green;
		}
		else
		{
			labelX1_Text = "[اطلاعات شما با موفقیت ثبت شد]";
			((Control)labelX1).ForeColor = Color.Green;
		}
		((Control)LblKolTarradod).Text = CountKol.ToString();
		((Control)LblNewTaradod).Text = CountCor.ToString();
		((Control)LblErrTaradod).Text = CountInCor.ToString();
		if (TxtErr == null)
		{
			return;
		}
		foreach (string item in TxtErr)
		{
			ListErr.Items.Add((object)item);
		}
	}

	private void BtnCancel_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
		((Form)this).Close();
		((Component)this).Dispose();
	}

	private void FRMProgress_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
	}

	private void FRMProgress_Load(object sender, EventArgs e)
	{
		try
		{
			labelX1_Text = Utilities.GetTranslate(labelX1_Text);
			((Control)labelX1).Text = labelX1_Text;
			trFrmProgressStr7 = Utilities.GetTranslate(trFrmProgressStr7);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMProgress_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FRMProgress.cs", 129);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((BaseForm)this).Dispose(disposing);
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
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Expected O, but got Unknown
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Expected O, but got Unknown
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Expected O, but got Unknown
		//IL_0613: Unknown result type (might be due to invalid IL or missing references)
		//IL_061d: Expected O, but got Unknown
		//IL_06c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d3: Expected O, but got Unknown
		//IL_077f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0789: Expected O, but got Unknown
		//IL_0835: Unknown result type (might be due to invalid IL or missing references)
		//IL_083f: Expected O, but got Unknown
		//IL_0a97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa1: Expected O, but got Unknown
		groupPanel1 = new GroupPanel();
		ListErr = new ListBox();
		LblKolTarradod = new LabelX();
		LblErrTaradod = new LabelX();
		LblNewTaradod = new LabelX();
		labelX3 = new LabelX();
		labelX2 = new LabelX();
		labelX4 = new LabelX();
		labelX1 = new LabelX();
		BtnCancel = new ButtonX();
		progressBarX1 = new ProgressBarX();
		((Control)groupPanel1).SuspendLayout();
		((Control)this).SuspendLayout();
		((PanelControl)groupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel1).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel1).Controls.Add((Control)(object)ListErr);
		((Control)groupPanel1).Controls.Add((Control)(object)LblKolTarradod);
		((Control)groupPanel1).Controls.Add((Control)(object)LblErrTaradod);
		((Control)groupPanel1).Controls.Add((Control)(object)LblNewTaradod);
		((Control)groupPanel1).Controls.Add((Control)(object)labelX3);
		((Control)groupPanel1).Controls.Add((Control)(object)labelX2);
		((Control)groupPanel1).Controls.Add((Control)(object)labelX4);
		((Control)groupPanel1).Controls.Add((Control)(object)labelX1);
		((Control)groupPanel1).Controls.Add((Control)(object)BtnCancel);
		((Control)groupPanel1).Controls.Add((Control)(object)progressBarX1);
		groupPanel1.DisabledBackColor = Color.Empty;
		((Control)groupPanel1).Dock = (DockStyle)5;
		((Control)groupPanel1).Location = new Point(0, 0);
		((Control)groupPanel1).Name = "groupPanel1";
		((Control)groupPanel1).Size = new Size(374, 294);
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
		((Control)groupPanel1).TabIndex = 1;
		((Control)ListErr).Anchor = (AnchorStyles)15;
		((ListControl)ListErr).FormattingEnabled = true;
		ListErr.HorizontalScrollbar = true;
		((Control)ListErr).Location = new Point(6, 132);
		((Control)ListErr).Name = "ListErr";
		ListErr.ScrollAlwaysVisible = true;
		((Control)ListErr).Size = new Size(356, 82);
		((Control)ListErr).TabIndex = 29;
		((Control)LblKolTarradod).Anchor = (AnchorStyles)9;
		((Control)LblKolTarradod).BackColor = Color.Transparent;
		((BaseItemControl)LblKolTarradod).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblKolTarradod).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LblKolTarradod).ForeColor = Color.Red;
		((Control)LblKolTarradod).Location = new Point(59, 45);
		((Control)LblKolTarradod).Name = "LblKolTarradod";
		((Control)LblKolTarradod).Size = new Size(166, 21);
		((Control)LblKolTarradod).TabIndex = 28;
		((Control)LblKolTarradod).Text = "0";
		((Control)LblErrTaradod).Anchor = (AnchorStyles)9;
		((Control)LblErrTaradod).BackColor = Color.Transparent;
		((BaseItemControl)LblErrTaradod).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblErrTaradod).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LblErrTaradod).ForeColor = Color.Red;
		((Control)LblErrTaradod).Location = new Point(59, 103);
		((Control)LblErrTaradod).Name = "LblErrTaradod";
		((Control)LblErrTaradod).Size = new Size(166, 21);
		((Control)LblErrTaradod).TabIndex = 27;
		((Control)LblErrTaradod).Text = "0";
		((Control)LblNewTaradod).Anchor = (AnchorStyles)9;
		((Control)LblNewTaradod).BackColor = Color.Transparent;
		((BaseItemControl)LblNewTaradod).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblNewTaradod).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LblNewTaradod).ForeColor = Color.Red;
		((Control)LblNewTaradod).Location = new Point(59, 74);
		((Control)LblNewTaradod).Name = "LblNewTaradod";
		((Control)LblNewTaradod).Size = new Size(166, 21);
		((Control)LblNewTaradod).TabIndex = 26;
		((Control)LblNewTaradod).Text = "0";
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((Control)labelX3).BackColor = Color.Transparent;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX3).Location = new Point(231, 103);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(128, 23);
		((Control)labelX3).TabIndex = 25;
		((Control)labelX3).Text = "تردد تکراری یا نادرست:";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((Control)labelX2).BackColor = Color.Transparent;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX2).Location = new Point(231, 74);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(135, 23);
		((Control)labelX2).TabIndex = 24;
		((Control)labelX2).Text = "ترددهای جدید ثبت شده:";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)labelX4).Anchor = (AnchorStyles)9;
		((Control)labelX4).BackColor = Color.Transparent;
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX4).Location = new Point(231, 43);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(128, 23);
		((Control)labelX4).TabIndex = 23;
		((Control)labelX4).Text = "[تعداد کل تردد ها] :";
		labelX4.TextAlignment = (StringAlignment)2;
		((Control)labelX1).Anchor = (AnchorStyles)13;
		((Control)labelX1).BackColor = Color.Transparent;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX1).Location = new Point(10, 9);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(356, 23);
		((Control)labelX1).TabIndex = 0;
		((Control)labelX1).Text = "[درحال دریافت اطلاعات .........]";
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)14;
		BtnCancel.ColorTable = (eButtonColor)6;
		BtnCancel.Image = (Image)(object)Resources.accept1;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(6, 247);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).Size = new Size(356, 32);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 1;
		((Control)BtnCancel).Text = "[بسیار خوب]";
		((Control)BtnCancel).Click += BtnCancel_Click;
		((Control)progressBarX1).Anchor = (AnchorStyles)14;
		((BaseItemControl)progressBarX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)progressBarX1).ForeColor = SystemColors.Desktop;
		((Control)progressBarX1).Location = new Point(6, 209);
		((Control)progressBarX1).Name = "progressBarX1";
		((Control)progressBarX1).Size = new Size(356, 32);
		((Control)progressBarX1).TabIndex = 0;
		((Control)progressBarX1).Text = "progressBarX1";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(374, 294);
		((Control)this).Controls.Add((Control)(object)groupPanel1);
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(400, 333);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(382, 333);
		((Control)this).Name = "FRMProgress";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).ShowIcon = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Form)this).Load += FRMProgress_Load;
		((Control)this).KeyDown += new KeyEventHandler(FRMProgress_KeyDown);
		((Control)groupPanel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}

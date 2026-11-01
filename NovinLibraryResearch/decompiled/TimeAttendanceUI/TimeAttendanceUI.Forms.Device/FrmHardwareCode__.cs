using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Device;

public class FrmHardwareCode__ : BaseForm
{
	public HardwareCodeCus _cls;

	private IContainer components;

	private PanelEx panelEx1;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private GroupPanel groupPanel7;

	internal PictureBox PictureBox1;

	private LabelX labelX1;

	private LabelX labelX5;

	private ComboBoxEx CmbDevice;

	private ComboBoxEx CmbCode;

	private LabelX labelX2;

	private TextBox txtSearch;

	private LabelX lblFamily;

	private LabelX labelX4;

	private void GetData()
	{
		_cls = ((List<HardwareCodeCus>)((ComboBox)CmbCode).DataSource)[((ListControl)CmbCode).SelectedIndex];
	}

	private void BtnAccept_Click(object sender, EventArgs e)
	{
		if (((Control)BtnAccept).Enabled && ((Control)BtnAccept).Visible)
		{
			GetData();
			if (((ListControl)CmbDevice).SelectedIndex >= 0 && ((ListControl)CmbCode).SelectedIndex >= 0)
			{
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
			else
			{
				((Control)BtnAccept).Enabled = true;
				((Form)this).DialogResult = (DialogResult)2;
			}
		}
	}

	private void BtnCancel_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
		((Form)this).Close();
	}

	private void FRMHardwareCode_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		Keys keyCode = e.KeyCode;
		if ((int)keyCode != 13)
		{
			if ((int)keyCode != 27)
			{
				if ((int)keyCode == 116)
				{
					e.Handled = true;
					((PopupItemControl)BtnAccept).PerformClick();
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
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Expected O, but got Unknown
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Expected O, but got Unknown
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Expected O, but got Unknown
		//IL_0d76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d80: Expected O, but got Unknown
		//IL_0ddd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de7: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		groupPanel7 = new GroupPanel();
		txtSearch = new TextBox();
		CmbCode = new ComboBoxEx();
		CmbDevice = new ComboBoxEx();
		PictureBox1 = new PictureBox();
		lblFamily = new LabelX();
		labelX2 = new LabelX();
		labelX4 = new LabelX();
		labelX1 = new LabelX();
		labelX5 = new LabelX();
		((Control)panelEx1).SuspendLayout();
		((Control)groupPanel7).SuspendLayout();
		((ISupportInitialize)PictureBox1).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Controls.Add((Control)(object)groupPanel7);
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(494, 181);
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
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnCancel).ForeColor = Color.Black;
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(12, 135);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(100, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 2;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو] (Esc)";
		((Control)BtnCancel).Click += BtnCancel_Click;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnAccept).ForeColor = Color.Black;
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(118, 135);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(95, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 1;
		((Control)BtnAccept).Text = "[ذخیره] (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((Control)groupPanel7).Anchor = (AnchorStyles)15;
		((Control)groupPanel7).BackColor = Color.Transparent;
		((PanelControl)groupPanel7).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel7).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel7).Controls.Add((Control)(object)txtSearch);
		((Control)groupPanel7).Controls.Add((Control)(object)CmbCode);
		((Control)groupPanel7).Controls.Add((Control)(object)CmbDevice);
		((Control)groupPanel7).Controls.Add((Control)(object)PictureBox1);
		((Control)groupPanel7).Controls.Add((Control)(object)lblFamily);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX2);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX4);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX1);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX5);
		((Control)groupPanel7).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)groupPanel7).Location = new Point(12, 12);
		((Control)groupPanel7).Name = "groupPanel7";
		((Control)groupPanel7).RightToLeft = (RightToLeft)1;
		((Control)groupPanel7).Size = new Size(470, 117);
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
		((PanelControl)groupPanel7).Style.Class = "";
		((PanelControl)groupPanel7).Style.CornerDiameter = 4;
		((PanelControl)groupPanel7).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel7).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel7).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel7).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel7).StyleMouseDown.Class = "";
		((PanelControl)groupPanel7).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel7).StyleMouseOver.Class = "";
		((PanelControl)groupPanel7).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel7).TabIndex = 0;
		((Control)txtSearch).Anchor = (AnchorStyles)9;
		((Control)txtSearch).Location = new Point(112, 30);
		((Control)txtSearch).MaximumSize = new Size(248, 21);
		((Control)txtSearch).MinimumSize = new Size(248, 21);
		((Control)txtSearch).Name = "txtSearch";
		((Control)txtSearch).Size = new Size(248, 21);
		((Control)txtSearch).TabIndex = 19;
		((Control)CmbCode).Anchor = (AnchorStyles)9;
		((ListControl)CmbCode).DisplayMember = "DeviceName";
		((ComboBox)CmbCode).DrawMode = (DrawMode)1;
		((ComboBox)CmbCode).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CmbCode).FormattingEnabled = true;
		((ComboBox)CmbCode).ItemHeight = 15;
		((Control)CmbCode).Location = new Point(12, 84);
		((Control)CmbCode).MaximumSize = new Size(348, 0);
		((Control)CmbCode).MinimumSize = new Size(348, 0);
		((Control)CmbCode).Name = "CmbCode";
		((Control)CmbCode).Size = new Size(348, 21);
		CmbCode.Style = (eDotNetBarStyle)9;
		((Control)CmbCode).TabIndex = 18;
		((ListControl)CmbCode).ValueMember = "DeviceNo";
		((Control)CmbDevice).Anchor = (AnchorStyles)9;
		((ListControl)CmbDevice).DisplayMember = "DeviceName";
		((ComboBox)CmbDevice).DrawMode = (DrawMode)1;
		((ComboBox)CmbDevice).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CmbDevice).FormattingEnabled = true;
		((ComboBox)CmbDevice).ItemHeight = 15;
		((Control)CmbDevice).Location = new Point(112, 57);
		((Control)CmbDevice).MaximumSize = new Size(248, 0);
		((Control)CmbDevice).MinimumSize = new Size(248, 0);
		((Control)CmbDevice).Name = "CmbDevice";
		((Control)CmbDevice).Size = new Size(248, 21);
		CmbDevice.Style = (eDotNetBarStyle)9;
		((Control)CmbDevice).TabIndex = 18;
		((ListControl)CmbDevice).ValueMember = "DeviceNO";
		PictureBox1.BorderStyle = (BorderStyle)1;
		PictureBox1.Image = (Image)(object)Resources.ETC132;
		((Control)PictureBox1).Location = new Point(3, -1);
		((Control)PictureBox1).Name = "PictureBox1";
		((Control)PictureBox1).Size = new Size(94, 79);
		PictureBox1.SizeMode = (PictureBoxSizeMode)4;
		PictureBox1.TabIndex = 6;
		PictureBox1.TabStop = false;
		((Control)lblFamily).Anchor = (AnchorStyles)9;
		((Control)lblFamily).BackColor = Color.Transparent;
		((BaseItemControl)lblFamily).BackgroundStyle.Class = "";
		((BaseItemControl)lblFamily).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblFamily).ForeColor = Color.FromArgb(64, 0, 0);
		((Control)lblFamily).Location = new Point(112, 3);
		((Control)lblFamily).MaximumSize = new Size(248, 21);
		((Control)lblFamily).MinimumSize = new Size(248, 21);
		((Control)lblFamily).Name = "lblFamily";
		((Control)lblFamily).RightToLeft = (RightToLeft)0;
		((Control)lblFamily).Size = new Size(248, 21);
		((Control)lblFamily).TabIndex = 2;
		((Control)lblFamily).Text = "---";
		lblFamily.TextAlignment = (StringAlignment)2;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((Control)labelX2).BackColor = Color.Transparent;
		((BaseItemControl)labelX2).BackgroundStyle.Class = "";
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(366, 3);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(95, 23);
		((Control)labelX2).TabIndex = 2;
		((Control)labelX2).Text = "[نام شخص]:";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)labelX4).Anchor = (AnchorStyles)9;
		((Control)labelX4).BackColor = Color.Transparent;
		((BaseItemControl)labelX4).BackgroundStyle.Class = "";
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Location = new Point(366, 28);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(76, 23);
		((Control)labelX4).TabIndex = 2;
		((Control)labelX4).Text = "[جستجو]:";
		labelX4.TextAlignment = (StringAlignment)2;
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((Control)labelX1).BackColor = Color.Transparent;
		((BaseItemControl)labelX1).BackgroundStyle.Class = "";
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(366, 55);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(76, 23);
		((Control)labelX1).TabIndex = 2;
		((Control)labelX1).Text = "[نام دستگاه]:";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)labelX5).Anchor = (AnchorStyles)9;
		((Control)labelX5).BackColor = Color.Transparent;
		((BaseItemControl)labelX5).BackgroundStyle.Class = "";
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Location = new Point(366, 84);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(95, 23);
		((Control)labelX5).TabIndex = 0;
		((Control)labelX5).Text = "[HardwareCode]:";
		labelX5.TextAlignment = (StringAlignment)2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(494, 181);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(510, 220);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(510, 220);
		((Control)this).Name = "FrmHardwareCode__";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "کد سخت افزاری پرسنل";
		((Control)this).KeyDown += new KeyEventHandler(FRMHardwareCode_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((Control)groupPanel7).ResumeLayout(false);
		((Control)groupPanel7).PerformLayout();
		((ISupportInitialize)PictureBox1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}

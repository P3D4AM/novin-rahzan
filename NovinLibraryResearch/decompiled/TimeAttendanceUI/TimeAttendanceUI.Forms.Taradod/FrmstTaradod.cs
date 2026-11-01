using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Taradod;

public class FrmstTaradod : Form
{
	private IContainer components;

	private PanelEx panelEx1;

	private LabelX LblKolTarradod;

	private LabelX LblErrTaradod;

	private LabelX LblNewTaradod;

	private ButtonX BtnExit;

	private LabelX labelX3;

	private LabelX labelX2;

	private LabelX labelX1;

	private LabelX lblSTTaradod;

	private ListBox ListErr;

	private PictureBox pictureBox1;

	public FrmstTaradod(int countKol, int countCor, int contInCor, string[] txtErr)
	{
		InitializeComponent();
		if (countCor <= 0)
		{
			((Control)lblSTTaradod).Text = "تردد جدیدی جهت ثبت وجود ندارد";
			pictureBox1.Image = (Image)(object)Resources.onebit_33;
		}
		if (countKol != 0)
		{
			((Control)LblKolTarradod).Text = (countKol + 2).ToString();
			((Control)LblNewTaradod).Text = (countCor - 1).ToString();
			((Control)LblErrTaradod).Text = contInCor.ToString();
		}
		else
		{
			((Control)LblKolTarradod).Text = "0";
			((Control)LblNewTaradod).Text = "0";
			((Control)LblErrTaradod).Text = "0";
		}
		foreach (string text in txtErr)
		{
			ListErr.Items.Add((object)text);
		}
	}

	private void BtnExit_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
		((Form)this).Close();
	}

	private void FRMSTTaradod_KeyDown(object sender, KeyEventArgs e)
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

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
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
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Expected O, but got Unknown
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Expected O, but got Unknown
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Expected O, but got Unknown
		//IL_067c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0686: Expected O, but got Unknown
		//IL_0733: Unknown result type (might be due to invalid IL or missing references)
		//IL_073d: Expected O, but got Unknown
		//IL_07ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f7: Expected O, but got Unknown
		//IL_08a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ae: Expected O, but got Unknown
		//IL_0969: Unknown result type (might be due to invalid IL or missing references)
		//IL_0973: Expected O, but got Unknown
		//IL_09e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ef: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		pictureBox1 = new PictureBox();
		ListErr = new ListBox();
		LblKolTarradod = new LabelX();
		LblErrTaradod = new LabelX();
		LblNewTaradod = new LabelX();
		BtnExit = new ButtonX();
		labelX3 = new LabelX();
		labelX2 = new LabelX();
		labelX1 = new LabelX();
		lblSTTaradod = new LabelX();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)pictureBox1);
		((Control)panelEx1).Controls.Add((Control)(object)ListErr);
		((Control)panelEx1).Controls.Add((Control)(object)LblKolTarradod);
		((Control)panelEx1).Controls.Add((Control)(object)LblErrTaradod);
		((Control)panelEx1).Controls.Add((Control)(object)LblNewTaradod);
		((Control)panelEx1).Controls.Add((Control)(object)BtnExit);
		((Control)panelEx1).Controls.Add((Control)(object)labelX3);
		((Control)panelEx1).Controls.Add((Control)(object)labelX2);
		((Control)panelEx1).Controls.Add((Control)(object)labelX1);
		((Control)panelEx1).Controls.Add((Control)(object)lblSTTaradod);
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(407, 300);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)pictureBox1).Anchor = (AnchorStyles)9;
		pictureBox1.Image = (Image)(object)Resources.check;
		((Control)pictureBox1).Location = new Point(368, 5);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(27, 30);
		pictureBox1.SizeMode = (PictureBoxSizeMode)4;
		pictureBox1.TabIndex = 9;
		pictureBox1.TabStop = false;
		((Control)ListErr).Anchor = (AnchorStyles)15;
		((ListControl)ListErr).FormattingEnabled = true;
		((Control)ListErr).Location = new Point(12, 175);
		((Control)ListErr).Name = "ListErr";
		ListErr.ScrollAlwaysVisible = true;
		((Control)ListErr).Size = new Size(383, 43);
		((Control)ListErr).TabIndex = 8;
		((Control)LblKolTarradod).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblKolTarradod).BackgroundStyle.Class = "";
		((BaseItemControl)LblKolTarradod).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblKolTarradod).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LblKolTarradod).ForeColor = Color.Red;
		((Control)LblKolTarradod).Location = new Point(29, 60);
		((Control)LblKolTarradod).Name = "LblKolTarradod";
		((Control)LblKolTarradod).Size = new Size(209, 23);
		((Control)LblKolTarradod).TabIndex = 7;
		((Control)LblKolTarradod).Text = "0";
		((Control)LblErrTaradod).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblErrTaradod).BackgroundStyle.Class = "";
		((BaseItemControl)LblErrTaradod).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblErrTaradod).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LblErrTaradod).ForeColor = Color.Red;
		((Control)LblErrTaradod).Location = new Point(29, 118);
		((Control)LblErrTaradod).Name = "LblErrTaradod";
		((Control)LblErrTaradod).Size = new Size(209, 23);
		((Control)LblErrTaradod).TabIndex = 6;
		((Control)LblErrTaradod).Text = "0";
		((Control)LblNewTaradod).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblNewTaradod).BackgroundStyle.Class = "";
		((BaseItemControl)LblNewTaradod).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblNewTaradod).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LblNewTaradod).ForeColor = Color.Red;
		((Control)LblNewTaradod).Location = new Point(29, 89);
		((Control)LblNewTaradod).Name = "LblNewTaradod";
		((Control)LblNewTaradod).Size = new Size(209, 23);
		((Control)LblNewTaradod).TabIndex = 5;
		((Control)LblNewTaradod).Text = "0";
		((Control)BtnExit).AccessibleRole = (AccessibleRole)43;
		((Control)BtnExit).Anchor = (AnchorStyles)13;
		BtnExit.ColorTable = (eButtonColor)6;
		((Control)BtnExit).Location = new Point(12, 267);
		((Control)BtnExit).Name = "BtnExit";
		((Control)BtnExit).Size = new Size(383, 35);
		((PopupItemControl)BtnExit).Style = (eDotNetBarStyle)9;
		((Control)BtnExit).TabIndex = 4;
		((Control)BtnExit).Text = "خروج";
		((Control)BtnExit).Click += BtnExit_Click;
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX3).BackgroundStyle.Class = "";
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX3).Location = new Point(257, 120);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(108, 23);
		((Control)labelX3).TabIndex = 3;
		((Control)labelX3).Text = "تعداد کل خطاها :";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX2).BackgroundStyle.Class = "";
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX2).Location = new Point(257, 91);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(132, 23);
		((Control)labelX2).TabIndex = 2;
		((Control)labelX2).Text = "تعداد کل تردد های جدید :";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX1).BackgroundStyle.Class = "";
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX1).Location = new Point(257, 60);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(112, 23);
		((Control)labelX1).TabIndex = 1;
		((Control)labelX1).Text = "تعداد کل تردد ها :";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)lblSTTaradod).Anchor = (AnchorStyles)9;
		((BaseItemControl)lblSTTaradod).BackgroundStyle.Class = "";
		((BaseItemControl)lblSTTaradod).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblSTTaradod).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)lblSTTaradod).ForeColor = Color.ForestGreen;
		((Control)lblSTTaradod).Location = new Point(108, 12);
		((Control)lblSTTaradod).Name = "lblSTTaradod";
		((Control)lblSTTaradod).Size = new Size(257, 23);
		((Control)lblSTTaradod).TabIndex = 0;
		((Control)lblSTTaradod).Text = "ترددها با موفقیت ثبت گردید.";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(407, 300);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(423, 339);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(423, 339);
		((Control)this).Name = "FrmstTaradod";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "وضعیت دریافت اطلاعات پرسنل از دستگاه";
		((Control)this).KeyDown += new KeyEventHandler(FRMSTTaradod_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Validators;

public class FrmShowErrorMessage : Form
{
	private IContainer components;

	private PanelEx panelEx1;

	private ButtonX btnReturn;

	private LabelX LblMessage1;

	private LabelX LblMessage5;

	private LabelX LblMessage4;

	private LabelX LblMessage3;

	private LabelX LblMessage2;

	private LabelX LblCountErrors;

	private LabelX labelX1;

	private PictureBox pictureBox5;

	private PictureBox pictureBox4;

	private PictureBox pictureBox3;

	private PictureBox pictureBox2;

	private PictureBox pictureBox1;

	private ButtonX btnAbort;

	private ButtonX btnCancel;

	private LabelX lblMessage;

	private ButtonX btnContinue;

	public FrmShowErrorMessage(ReturnedSaveFuncInfo pList, MessageBoxButtons boxButtons)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Invalid comparison between Unknown and I4
		InitializeComponent();
		((Control)LblMessage1).Text = "";
		((Control)LblMessage2).Text = "";
		((Control)LblMessage3).Text = "";
		((Control)LblMessage4).Text = "";
		((Control)LblMessage5).Text = "";
		if ((int)boxButtons == 0)
		{
			ButtonX obj = btnContinue;
			ButtonX obj2 = btnCancel;
			ButtonX obj3 = btnAbort;
			bool flag2 = (((Control)btnReturn).Visible = false);
			bool flag4 = (((Control)obj3).Visible = flag2);
			bool visible = (((Control)obj2).Visible = flag4);
			((Control)obj).Visible = visible;
		}
		if ((int)boxButtons != 2)
		{
			((Control)btnAbort).Visible = false;
		}
		try
		{
			bool flag6 = false;
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < 5; i++)
			{
				LabelX val;
				PictureBox val2;
				switch (i)
				{
				case 0:
					val = LblMessage1;
					val2 = pictureBox1;
					break;
				case 1:
					val = LblMessage2;
					val2 = pictureBox2;
					break;
				case 2:
					val = LblMessage3;
					val2 = pictureBox3;
					break;
				case 3:
					val = LblMessage4;
					val2 = pictureBox4;
					break;
				case 4:
					val = LblMessage5;
					val2 = pictureBox5;
					break;
				default:
					val = LblMessage1;
					val2 = pictureBox1;
					break;
				}
				if (pList.ErrorList.Count > num)
				{
					((Control)val).Text = pList.ErrorList[num];
					num++;
					flag6 = true;
					val2.Image = (Image)(object)Resources.Error;
				}
				else if (pList.WarningList.Count > num2)
				{
					((Control)val).Text = pList.WarningList[num2];
					num2++;
					val2.Image = (Image)(object)Resources.Warning;
				}
			}
			((Control)LblCountErrors).Text = (pList.ErrorList.Count + pList.InformationList.Count).ToString();
			if (flag6)
			{
				((Control)lblMessage).Text = "بدلیل وجود خطا امکان ادامه کار وجود ندارد";
				((Control)btnContinue).Enabled = false;
			}
			else
			{
				((Control)lblMessage).Text = "خطایی وجود ندارد. ولی تعدادی هشدار وجود دارد مایل به ادامه کار هستید؟";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Validators\\FrmShowErrorMessageold.cs", 96);
		}
	}

	private void btnReturn_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)4;
	}

	private void btnContinue_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)5;
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
	}

	private void btnCancelAll_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)3;
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
		//IL_05bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c9: Expected O, but got Unknown
		//IL_067d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0687: Expected O, but got Unknown
		//IL_073e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0748: Expected O, but got Unknown
		//IL_07ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0809: Expected O, but got Unknown
		//IL_0cfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d07: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		pictureBox5 = new PictureBox();
		pictureBox4 = new PictureBox();
		pictureBox3 = new PictureBox();
		pictureBox2 = new PictureBox();
		pictureBox1 = new PictureBox();
		btnAbort = new ButtonX();
		btnContinue = new ButtonX();
		btnCancel = new ButtonX();
		btnReturn = new ButtonX();
		LblMessage1 = new LabelX();
		LblCountErrors = new LabelX();
		labelX1 = new LabelX();
		lblMessage = new LabelX();
		LblMessage5 = new LabelX();
		LblMessage4 = new LabelX();
		LblMessage3 = new LabelX();
		LblMessage2 = new LabelX();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)pictureBox5).BeginInit();
		((ISupportInitialize)pictureBox4).BeginInit();
		((ISupportInitialize)pictureBox3).BeginInit();
		((ISupportInitialize)pictureBox2).BeginInit();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)pictureBox5);
		((Control)panelEx1).Controls.Add((Control)(object)pictureBox4);
		((Control)panelEx1).Controls.Add((Control)(object)pictureBox3);
		((Control)panelEx1).Controls.Add((Control)(object)pictureBox2);
		((Control)panelEx1).Controls.Add((Control)(object)pictureBox1);
		((Control)panelEx1).Controls.Add((Control)(object)btnAbort);
		((Control)panelEx1).Controls.Add((Control)(object)btnContinue);
		((Control)panelEx1).Controls.Add((Control)(object)btnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)btnReturn);
		((Control)panelEx1).Controls.Add((Control)(object)LblMessage1);
		((Control)panelEx1).Controls.Add((Control)(object)LblCountErrors);
		((Control)panelEx1).Controls.Add((Control)(object)labelX1);
		((Control)panelEx1).Controls.Add((Control)(object)lblMessage);
		((Control)panelEx1).Controls.Add((Control)(object)LblMessage5);
		((Control)panelEx1).Controls.Add((Control)(object)LblMessage4);
		((Control)panelEx1).Controls.Add((Control)(object)LblMessage3);
		((Control)panelEx1).Controls.Add((Control)(object)LblMessage2);
		panelEx1.DialogResult = (DialogResult)2;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(655, 305);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.Color = Color.FromArgb(255, 192, 128);
		panelEx1.Style.BackColor2.Color = Color.White;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)pictureBox5).Location = new Point(609, 168);
		((Control)pictureBox5).Name = "pictureBox5";
		((Control)pictureBox5).Size = new Size(34, 33);
		pictureBox5.SizeMode = (PictureBoxSizeMode)1;
		pictureBox5.TabIndex = 8;
		pictureBox5.TabStop = false;
		((Control)pictureBox4).Location = new Point(609, 129);
		((Control)pictureBox4).Name = "pictureBox4";
		((Control)pictureBox4).Size = new Size(34, 33);
		pictureBox4.SizeMode = (PictureBoxSizeMode)1;
		pictureBox4.TabIndex = 8;
		pictureBox4.TabStop = false;
		((Control)pictureBox3).Location = new Point(609, 90);
		((Control)pictureBox3).Name = "pictureBox3";
		((Control)pictureBox3).Size = new Size(34, 33);
		pictureBox3.SizeMode = (PictureBoxSizeMode)1;
		pictureBox3.TabIndex = 8;
		pictureBox3.TabStop = false;
		((Control)pictureBox2).Location = new Point(609, 51);
		((Control)pictureBox2).Name = "pictureBox2";
		((Control)pictureBox2).Size = new Size(34, 33);
		pictureBox2.SizeMode = (PictureBoxSizeMode)1;
		pictureBox2.TabIndex = 8;
		pictureBox2.TabStop = false;
		((Control)pictureBox1).Location = new Point(609, 12);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(34, 33);
		pictureBox1.SizeMode = (PictureBoxSizeMode)1;
		pictureBox1.TabIndex = 8;
		pictureBox1.TabStop = false;
		((Control)btnAbort).AccessibleRole = (AccessibleRole)43;
		btnAbort.ColorTable = (eButtonColor)3;
		btnAbort.DialogResult = (DialogResult)2;
		((Control)btnAbort).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)btnAbort).Location = new Point(12, 246);
		((Control)btnAbort).Name = "btnAbort";
		((Control)btnAbort).Size = new Size(127, 49);
		((PopupItemControl)btnAbort).Style = (eDotNetBarStyle)9;
		((Control)btnAbort).TabIndex = 11;
		((Control)btnAbort).Text = "انصراف از همه";
		((Control)btnAbort).Click += btnCancelAll_Click;
		((Control)btnContinue).AccessibleRole = (AccessibleRole)43;
		btnContinue.ColorTable = (eButtonColor)3;
		btnContinue.DialogResult = (DialogResult)2;
		((Control)btnContinue).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)btnContinue).Location = new Point(278, 246);
		((Control)btnContinue).Name = "btnContinue";
		((Control)btnContinue).Size = new Size(127, 49);
		((PopupItemControl)btnContinue).Style = (eDotNetBarStyle)9;
		((Control)btnContinue).TabIndex = 9;
		((Control)btnContinue).Text = "ادامه";
		((Control)btnContinue).Click += btnContinue_Click;
		((Control)btnCancel).AccessibleRole = (AccessibleRole)43;
		btnCancel.ColorTable = (eButtonColor)3;
		btnCancel.DialogResult = (DialogResult)2;
		((Control)btnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)btnCancel).Location = new Point(145, 246);
		((Control)btnCancel).Name = "btnCancel";
		((Control)btnCancel).Size = new Size(127, 49);
		((PopupItemControl)btnCancel).Style = (eDotNetBarStyle)9;
		((Control)btnCancel).TabIndex = 10;
		((Control)btnCancel).Text = "انصراف";
		((Control)btnCancel).Click += btnCancel_Click;
		((Control)btnReturn).AccessibleRole = (AccessibleRole)43;
		btnReturn.ColorTable = (eButtonColor)3;
		btnReturn.DialogResult = (DialogResult)2;
		((Control)btnReturn).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)btnReturn).Location = new Point(411, 246);
		((Control)btnReturn).Name = "btnReturn";
		((Control)btnReturn).Size = new Size(139, 49);
		((PopupItemControl)btnReturn).Style = (eDotNetBarStyle)9;
		((Control)btnReturn).TabIndex = 8;
		((Control)btnReturn).Text = "برگشت و اصلاح خطاها";
		((Control)btnReturn).Click += btnReturn_Click;
		((BaseItemControl)LblMessage1).BackgroundStyle.Class = "";
		((BaseItemControl)LblMessage1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblMessage1).Location = new Point(12, 12);
		((Control)LblMessage1).Name = "LblMessage1";
		((Control)LblMessage1).Size = new Size(591, 33);
		((Control)LblMessage1).TabIndex = 0;
		((Control)LblMessage1).Text = "text";
		((BaseItemControl)LblCountErrors).BackgroundStyle.Class = "";
		((BaseItemControl)LblCountErrors).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblCountErrors).ForeColor = Color.Red;
		((Control)LblCountErrors).Location = new Point(445, 207);
		((Control)LblCountErrors).Name = "LblCountErrors";
		((Control)LblCountErrors).Size = new Size(28, 33);
		((Control)LblCountErrors).TabIndex = 6;
		((Control)LblCountErrors).Text = "100";
		((BaseItemControl)labelX1).BackgroundStyle.Class = "";
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(479, 207);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(71, 33);
		((Control)labelX1).TabIndex = 5;
		((Control)labelX1).Text = "تعداد خطاها :";
		((BaseItemControl)lblMessage).BackgroundStyle.Class = "";
		((BaseItemControl)lblMessage).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblMessage).ForeColor = Color.Maroon;
		((Control)lblMessage).Location = new Point(21, 207);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).Size = new Size(418, 33);
		((Control)lblMessage).TabIndex = 7;
		((Control)lblMessage).Text = "بدلیل وجود خطا امکان ادامه کار وجود ندارد";
		lblMessage.TextAlignment = (StringAlignment)1;
		((BaseItemControl)LblMessage5).BackgroundStyle.Class = "";
		((BaseItemControl)LblMessage5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblMessage5).Location = new Point(12, 168);
		((Control)LblMessage5).Name = "LblMessage5";
		((Control)LblMessage5).Size = new Size(591, 33);
		((Control)LblMessage5).TabIndex = 4;
		((Control)LblMessage5).Text = "text";
		((BaseItemControl)LblMessage4).BackgroundStyle.Class = "";
		((BaseItemControl)LblMessage4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblMessage4).Location = new Point(12, 129);
		((Control)LblMessage4).Name = "LblMessage4";
		((Control)LblMessage4).Size = new Size(591, 33);
		((Control)LblMessage4).TabIndex = 3;
		((Control)LblMessage4).Text = "text";
		((BaseItemControl)LblMessage3).BackgroundStyle.Class = "";
		((BaseItemControl)LblMessage3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblMessage3).Location = new Point(12, 90);
		((Control)LblMessage3).Name = "LblMessage3";
		((Control)LblMessage3).Size = new Size(591, 33);
		((Control)LblMessage3).TabIndex = 2;
		((Control)LblMessage3).Text = "text";
		((BaseItemControl)LblMessage2).BackgroundStyle.Class = "";
		((BaseItemControl)LblMessage2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblMessage2).Location = new Point(12, 51);
		((Control)LblMessage2).Name = "LblMessage2";
		((Control)LblMessage2).Size = new Size(591, 33);
		((Control)LblMessage2).TabIndex = 1;
		((Control)LblMessage2).Text = "text";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(655, 305);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmShowErrorMessage";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "نمایش خطا ها";
		((Form)this).TopMost = true;
		((Control)panelEx1).ResumeLayout(false);
		((ISupportInitialize)pictureBox5).EndInit();
		((ISupportInitialize)pictureBox4).EndInit();
		((ISupportInitialize)pictureBox3).EndInit();
		((ISupportInitialize)pictureBox2).EndInit();
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}

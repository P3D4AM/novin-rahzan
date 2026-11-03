using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom.Cache;
using PacketParser.Entities;

namespace PayamakUi;

public class FRMSTSentExcel : CacheBaseForm
{
	private IContainer components;

	internal PanelEx PanelEx1;

	internal ButtonX BTnExit;

	internal Label sentcount;

	internal Label FalseCount;

	internal Label IranCelCount;

	internal Label AvalCount;

	internal Label LabelX9;

	internal Label LabelX8;

	internal Label LabelX7;

	internal Label LabelX6;

	internal Label LblNotSent;

	internal Label LabelX1;

	internal Label LblReapet;

	internal Label LabelX2;

	public FRMSTSentExcel(string contextGuid)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FRMSTSentExcel.cs", 17);
		}
	}

	public FRMSTSentExcel(string contextGuid, int Hamrah, int Irancel, int InCorrect, int Sent, int NotSent, int Reapeat)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
			((Control)AvalCount).Text = Hamrah.ToString();
			((Control)IranCelCount).Text = Irancel.ToString();
			((Control)FalseCount).Text = InCorrect.ToString();
			((Control)sentcount).Text = Sent.ToString();
			((Control)LblNotSent).Text = NotSent.ToString();
			((Control)LblReapet).Text = Reapeat.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FRMSTSentExcel.cs", 35);
		}
	}

	private void BTnExit_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTnExit_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\FRMSTSentExcel.cs", 48);
		}
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_0540: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FRMSTSentExcel));
		PanelEx1 = new PanelEx();
		LblReapet = new Label();
		LabelX2 = new Label();
		LblNotSent = new Label();
		LabelX1 = new Label();
		BTnExit = new ButtonX();
		sentcount = new Label();
		FalseCount = new Label();
		IranCelCount = new Label();
		AvalCount = new Label();
		LabelX9 = new Label();
		LabelX8 = new Label();
		LabelX7 = new Label();
		LabelX6 = new Label();
		((Control)PanelEx1).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)PanelEx1).Controls.Add((Control)(object)LblReapet);
		((Control)PanelEx1).Controls.Add((Control)(object)LabelX2);
		((Control)PanelEx1).Controls.Add((Control)(object)LblNotSent);
		((Control)PanelEx1).Controls.Add((Control)(object)LabelX1);
		((Control)PanelEx1).Controls.Add((Control)(object)BTnExit);
		((Control)PanelEx1).Controls.Add((Control)(object)sentcount);
		((Control)PanelEx1).Controls.Add((Control)(object)FalseCount);
		((Control)PanelEx1).Controls.Add((Control)(object)IranCelCount);
		((Control)PanelEx1).Controls.Add((Control)(object)AvalCount);
		((Control)PanelEx1).Controls.Add((Control)(object)LabelX9);
		((Control)PanelEx1).Controls.Add((Control)(object)LabelX8);
		((Control)PanelEx1).Controls.Add((Control)(object)LabelX7);
		((Control)PanelEx1).Controls.Add((Control)(object)LabelX6);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(295, 208);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
		PanelEx1.Style.BackColor2.ColorSchemePart = eColorSchemePart.PanelBackground2;
		PanelEx1.Style.Border = eBorderType.SingleLine;
		PanelEx1.Style.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
		PanelEx1.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 1;
		((Control)LblReapet).Anchor = (AnchorStyles)9;
		((Control)LblReapet).Text = "";
		((Control)LblReapet).ForeColor = Color.DarkRed;
		((Control)LblReapet).Location = new Point(50, 43);
		((Control)LblReapet).Name = "LblReapet";
		((Control)LblReapet).Size = new Size(75, 23);
		((Control)LblReapet).TabIndex = 19;
		((Control)LblReapet).Text = "0";
		((Control)LabelX2).Anchor = (AnchorStyles)9;
		((Control)LabelX2).Text = "";
		((Control)LabelX2).Location = new Point(132, 42);
		((Control)LabelX2).Name = "LabelX2";
		((Control)LabelX2).Size = new Size(160, 23);
		((Control)LabelX2).TabIndex = 18;
		((Control)LabelX2).Text = "تعداد شماره ها با متن های تکراری :";
		LabelX2.TextAlign = (ContentAlignment)16;
		((Control)LblNotSent).Anchor = (AnchorStyles)9;
		((Control)LblNotSent).Text = "";
		((Control)LblNotSent).ForeColor = Color.DarkRed;
		((Control)LblNotSent).Location = new Point(50, 162);
		((Control)LblNotSent).Name = "LblNotSent";
		((Control)LblNotSent).Size = new Size(75, 23);
		((Control)LblNotSent).TabIndex = 17;
		((Control)LblNotSent).Text = "0";
		((Control)LblNotSent).Visible = false;
		((Control)LabelX1).Anchor = (AnchorStyles)9;
		((Control)LabelX1).Text = "";
		((Control)LabelX1).Location = new Point(132, 162);
		((Control)LabelX1).Name = "LabelX1";
		((Control)LabelX1).Size = new Size(147, 23);
		((Control)LabelX1).TabIndex = 16;
		((Control)LabelX1).Text = "تعداد شماره های ارسال نشده :";
		LabelX1.TextAlign = (ContentAlignment)16;
		((Control)LabelX1).Visible = false;
		((Control)BTnExit).AccessibleRole = (AccessibleRole)43;
		BTnExit.ColorTable = eButtonColor.Office2007WithBackground;
		BTnExit.Image = (Image)componentResourceManager.GetObject("BTnExit.Image");
		((Control)BTnExit).Location = new Point(3, 173);
		((Control)BTnExit).Name = "BTnExit";
		((Control)BTnExit).Size = new Size(45, 32);
		BTnExit.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BTnExit).TabIndex = 1;
		((Control)BTnExit).Click += BTnExit_Click;
		((Control)sentcount).Anchor = (AnchorStyles)9;
		((Control)sentcount).Text = "";
		((Control)sentcount).ForeColor = Color.DarkRed;
		((Control)sentcount).Location = new Point(50, 13);
		((Control)sentcount).Name = "sentcount";
		((Control)sentcount).Size = new Size(75, 23);
		((Control)sentcount).TabIndex = 15;
		((Control)sentcount).Text = "0";
		((Control)FalseCount).Anchor = (AnchorStyles)9;
		((Control)FalseCount).Text = "";
		((Control)FalseCount).ForeColor = Color.DarkRed;
		((Control)FalseCount).Location = new Point(50, 133);
		((Control)FalseCount).Name = "FalseCount";
		((Control)FalseCount).Size = new Size(75, 23);
		((Control)FalseCount).TabIndex = 14;
		((Control)FalseCount).Text = "0";
		((Control)IranCelCount).Anchor = (AnchorStyles)9;
		((Control)IranCelCount).Text = "";
		((Control)IranCelCount).ForeColor = Color.DarkRed;
		((Control)IranCelCount).Location = new Point(50, 103);
		((Control)IranCelCount).Name = "IranCelCount";
		((Control)IranCelCount).Size = new Size(75, 23);
		((Control)IranCelCount).TabIndex = 13;
		((Control)IranCelCount).Text = "0";
		((Control)AvalCount).Anchor = (AnchorStyles)9;
		((Control)AvalCount).Text = "";
		((Control)AvalCount).ForeColor = Color.DarkRed;
		((Control)AvalCount).Location = new Point(50, 73);
		((Control)AvalCount).Name = "AvalCount";
		((Control)AvalCount).Size = new Size(75, 23);
		((Control)AvalCount).TabIndex = 12;
		((Control)AvalCount).Text = "0";
		((Control)LabelX9).Anchor = (AnchorStyles)9;
		((Control)LabelX9).Text = "";
		((Control)LabelX9).Location = new Point(132, 102);
		((Control)LabelX9).Name = "LabelX9";
		((Control)LabelX9).Size = new Size(132, 23);
		((Control)LabelX9).TabIndex = 10;
		((Control)LabelX9).Text = "تعداد شماره های ایرانسل :";
		LabelX9.TextAlign = (ContentAlignment)16;
		((Control)LabelX8).Anchor = (AnchorStyles)9;
		((Control)LabelX8).Text = "";
		((Control)LabelX8).Location = new Point(132, 72);
		((Control)LabelX8).Name = "LabelX8";
		((Control)LabelX8).Size = new Size(132, 23);
		((Control)LabelX8).TabIndex = 9;
		((Control)LabelX8).Text = "تعداد شماره های همراه اول :";
		LabelX8.TextAlign = (ContentAlignment)16;
		((Control)LabelX7).Anchor = (AnchorStyles)9;
		((Control)LabelX7).Text = "";
		((Control)LabelX7).Location = new Point(132, 12);
		((Control)LabelX7).Name = "LabelX7";
		((Control)LabelX7).Size = new Size(140, 23);
		((Control)LabelX7).TabIndex = 8;
		((Control)LabelX7).Text = "تعداد شماره های ارسال شده :";
		LabelX7.TextAlign = (ContentAlignment)16;
		((Control)LabelX6).Anchor = (AnchorStyles)9;
		((Control)LabelX6).Text = "";
		((Control)LabelX6).Location = new Point(132, 132);
		((Control)LabelX6).Name = "LabelX6";
		((Control)LabelX6).Size = new Size(132, 23);
		((Control)LabelX6).TabIndex = 7;
		((Control)LabelX6).Text = "تعدادشماره های نادرست :";
		LabelX6.TextAlign = (ContentAlignment)16;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(295, 208);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FRMSTSentExcel";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "وضعیت ارسال از طریق فایل اکسل";
		((Control)PanelEx1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using TimeAttendanceDAL.Utility;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Gharardads;

public class FrmDisplayMessageGheybatIsOld : Form, IHasContext
{
	private IContainer components;

	private PanelEx panelEx1;

	private LabelX labelX1;

	private LabelX labelX4;

	private LabelX labelX3;

	private LabelX labelX2;

	private CheckBoxX checkBoxX1;

	private ButtonX BtnAccept;

	public string ContextGuid { get; set; }

	public FrmDisplayMessageGheybatIsOld(string contextGuid)
	{
		ContextGuid = contextGuid;
		InitializeComponent();
	}

	private void BtnAccept_Click(object sender, EventArgs e)
	{
		try
		{
			if (checkBoxX1.Checked)
			{
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
			else
			{
				Utility.MessageBoxShow("لطفا متن را بخوانید و تیک متوجه شدم را بزنید", ContextGuid);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmDisplayMessageGheybatIsOld.cs", 32);
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
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Expected O, but got Unknown
		//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		checkBoxX1 = new CheckBoxX();
		BtnAccept = new ButtonX();
		labelX4 = new LabelX();
		labelX3 = new LabelX();
		labelX2 = new LabelX();
		labelX1 = new LabelX();
		((Control)panelEx1).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)checkBoxX1);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Controls.Add((Control)(object)labelX4);
		((Control)panelEx1).Controls.Add((Control)(object)labelX3);
		((Control)panelEx1).Controls.Add((Control)(object)labelX2);
		((Control)panelEx1).Controls.Add((Control)(object)labelX1);
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(409, 190);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((BaseItemControl)checkBoxX1).BackgroundStyle.Class = "";
		((BaseItemControl)checkBoxX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)checkBoxX1).Location = new Point(300, 144);
		((Control)checkBoxX1).Name = "checkBoxX1";
		((Control)checkBoxX1).Size = new Size(97, 34);
		((BaseItemControl)checkBoxX1).Style = (eDotNetBarStyle)9;
		((Control)checkBoxX1).TabIndex = 3;
		((Control)checkBoxX1).Text = "متوجه شدم";
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnAccept).ForeColor = Color.Black;
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(12, 144);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		BtnAccept.Shortcuts.Add((eShortcut)116);
		((Control)BtnAccept).Size = new Size(95, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 2;
		((Control)BtnAccept).Text = "ثبت  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((BaseItemControl)labelX4).BackgroundStyle.Class = "";
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Location = new Point(12, 111);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(385, 27);
		((Control)labelX4).TabIndex = 0;
		((Control)labelX4).Text = "میشود پس میبایست نرخ غیبت در تمام قرارداد ها را اطلاح نمائید.";
		((BaseItemControl)labelX3).BackgroundStyle.Class = "";
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(12, 78);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(385, 27);
		((Control)labelX3).TabIndex = 0;
		((Control)labelX3).Text = "نرخ محاسبه غیبت ساعتی بود و از این نسخه به بعد  غیبت روزانه محاسبه ";
		((BaseItemControl)labelX2).BackgroundStyle.Class = "";
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(12, 45);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(385, 27);
		((Control)labelX2).TabIndex = 0;
		((Control)labelX2).Text = "با توجه به اینکه نسخه های قبلی نرم افزار حضور - غیاب و حقوق - دستمزد";
		((BaseItemControl)labelX1).BackgroundStyle.Class = "";
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(12, 12);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(385, 27);
		((Control)labelX1).TabIndex = 0;
		((Control)labelX1).Text = "کاربر محترم :";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(409, 190);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(425, 228);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(425, 228);
		((Control)this).Name = "FrmDisplayMessageGheybatIsOld";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)panelEx1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}

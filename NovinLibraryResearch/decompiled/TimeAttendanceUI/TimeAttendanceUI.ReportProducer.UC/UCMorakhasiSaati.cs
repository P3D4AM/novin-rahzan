using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using PacketParser.Entities;

namespace TimeAttendanceUI.ReportProducer.UC;

public class UCMorakhasiSaati : UserControl
{
	private bool _Enable = true;

	private IContainer components;

	private UCPanel ucPanel1;

	private PanelEx panelEx1;

	private CheckBoxX checkBoxX1;

	public bool isMorakhasiSaati => checkBoxX1.Checked;

	public bool EnalbleControl
	{
		get
		{
			_Enable = ucPanel1.Expand;
			return _Enable;
		}
		set
		{
			try
			{
				_Enable = ucPanel1.Expand;
				RefreshForm();
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "EnalbleControl", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCMorakhasiSaati.cs", 43);
			}
		}
	}

	public UCMorakhasiSaati()
	{
		InitializeComponent();
		((Control)ucPanel1.panelMain).Controls.Add((Control)(object)panelEx1);
		((Control)panelEx1).Top = 0;
	}

	private void UCMorakhasiSaati_Load(object sender, EventArgs e)
	{
		((Control)this).Size = ((Control)ucPanel1).Size;
		RefreshForm();
	}

	private void panelEx1_Resize(object sender, EventArgs e)
	{
		((Control)this).Size = ((Control)ucPanel1).Size;
		EnalbleControl = EnalbleControl;
	}

	private void RefreshForm()
	{
		try
		{
			if (EnalbleControl)
			{
				ucPanel1.HeaderText2 = "محاسبه مرخص ساعتی براساس قرارداد";
			}
			else
			{
				ucPanel1.HeaderText2 = "غیر فعال";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RefreshForm", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCMorakhasiSaati.cs", 56);
		}
	}

	private void ucPanel1_Load(object sender, EventArgs e)
	{
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Expected O, but got Unknown
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		checkBoxX1 = new CheckBoxX();
		ucPanel1 = new UCPanel();
		((Control)panelEx1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelEx1).Anchor = (AnchorStyles)15;
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)checkBoxX1);
		((Control)panelEx1).Location = new Point(0, 30);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).RightToLeft = (RightToLeft)1;
		((Control)panelEx1).Size = new Size(514, 38);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 2;
		((Control)panelEx1).Resize += panelEx1_Resize;
		((BaseItemControl)checkBoxX1).BackgroundStyle.Class = "";
		((BaseItemControl)checkBoxX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)checkBoxX1).Location = new Point(237, 3);
		((Control)checkBoxX1).Name = "checkBoxX1";
		((Control)checkBoxX1).Size = new Size(274, 32);
		((BaseItemControl)checkBoxX1).Style = (eDotNetBarStyle)9;
		((Control)checkBoxX1).TabIndex = 0;
		((Control)checkBoxX1).Text = "[MorkhasiSD]";
		((Control)ucPanel1).Dock = (DockStyle)5;
		ucPanel1.Expand = true;
		((Control)ucPanel1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		ucPanel1.HeaderText = " ";
		ucPanel1.HeaderText2 = "[محاسبه مرخص ساعتی براساس قرارداد]";
		((Control)ucPanel1).ImeMode = (ImeMode)0;
		((Control)ucPanel1).Location = new Point(0, 0);
		((Control)ucPanel1).Name = "ucPanel1";
		((Control)ucPanel1).Size = new Size(514, 68);
		((Control)ucPanel1).TabIndex = 0;
		((UserControl)ucPanel1).Load += ucPanel1_Load;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Controls.Add((Control)(object)ucPanel1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)this).Name = "UCMorakhasiSaati";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(514, 68);
		((UserControl)this).Load += UCMorakhasiSaati_Load;
		((Control)panelEx1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}

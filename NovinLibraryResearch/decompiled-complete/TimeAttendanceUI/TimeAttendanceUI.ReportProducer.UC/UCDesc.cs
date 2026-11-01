using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using PacketParser.Entities;

namespace TimeAttendanceUI.ReportProducer.UC;

public class UCDesc : UserControl
{
	private bool _Enable = true;

	private IContainer components;

	private UCPanel ucPanel1;

	private PanelEx panelEx1;

	private LabelX labelX2;

	private TextBoxX TxtDesc;

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
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "EnalbleControl", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCDesc.cs", 44);
			}
		}
	}

	public string Desc => ((Control)TxtDesc).Text;

	public UCDesc()
	{
		InitializeComponent();
		((Control)ucPanel1.panelMain).Controls.Add((Control)(object)panelEx1);
		((Control)panelEx1).Top = 0;
	}

	private void UCDesc_Load(object sender, EventArgs e)
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
			ucPanel1.HeaderText2 = (EnalbleControl ? "شرح" : "شرح - غیر فعال");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RefreshForm", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCDesc.cs", 59);
		}
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
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Expected O, but got Unknown
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		labelX2 = new LabelX();
		TxtDesc = new TextBoxX();
		ucPanel1 = new UCPanel();
		((Control)panelEx1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelEx1).Anchor = (AnchorStyles)15;
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)labelX2);
		((Control)panelEx1).Controls.Add((Control)(object)TxtDesc);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Location = new Point(0, 29);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(600, 41);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 1;
		((Control)panelEx1).Resize += panelEx1_Resize;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((Control)labelX2).BackColor = Color.Transparent;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(511, 8);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).RightToLeft = (RightToLeft)1;
		((Control)labelX2).Size = new Size(86, 23);
		((Control)labelX2).TabIndex = 10;
		((Control)labelX2).Text = "[شرح]";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)TxtDesc).Anchor = (AnchorStyles)13;
		TxtDesc.Border.Class = "TextBoxBorder";
		TxtDesc.Border.CornerType = (eCornerType)1;
		((Control)TxtDesc).Location = new Point(15, 8);
		((Control)TxtDesc).Name = "TxtDesc";
		((Control)TxtDesc).Size = new Size(490, 21);
		((Control)TxtDesc).TabIndex = 9;
		((Control)ucPanel1).Dock = (DockStyle)5;
		ucPanel1.Expand = true;
		((Control)ucPanel1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		ucPanel1.HeaderText = "";
		ucPanel1.HeaderText2 = "";
		((Control)ucPanel1).Location = new Point(0, 0);
		((Control)ucPanel1).MinimumSize = new Size(600, 10);
		((Control)ucPanel1).Name = "ucPanel1";
		((Control)ucPanel1).Size = new Size(600, 70);
		((Control)ucPanel1).TabIndex = 0;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Controls.Add((Control)(object)ucPanel1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)this).MaximumSize = new Size(600, 70);
		((Control)this).MinimumSize = new Size(600, 10);
		((Control)this).Name = "UCDesc";
		((Control)this).Size = new Size(600, 70);
		((UserControl)this).Load += UCDesc_Load;
		((Control)panelEx1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom.Cache;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendanceUI.Forms.Gharardads;

namespace TimeAttendanceUI.ReportProducer.UC;

public class UCTakGharardad : UserControl, IHasContext
{
	private bool ShiftProgrammableOnly;

	private Font _font;

	private IContainer components;

	private UCPanel ucPanel1;

	private PanelEx panelEx1;

	private LabelX lblname;

	private LabelX labelX2;

	private ButtonX BTNMore;

	private LabelX labelX1;

	private TextBoxX TxtGharardodNo;

	public string ContextGuid { get; set; }

	public GharardadNew ClsGharardad { get; set; }

	public bool EnalbleControl
	{
		get
		{
			return ucPanel1.Expand;
		}
		set
		{
			ucPanel1.Expand = value;
		}
	}

	public string HeaderText
	{
		get
		{
			return ucPanel1.HeaderText2;
		}
		set
		{
			ucPanel1.HeaderText2 = value;
		}
	}

	public UCTakGharardad(string contextGuid, Font font, TimeAttendanceSettings setting, bool pShiftProgrammableOnly = false)
	{
		try
		{
			ContextGuid = contextGuid;
			ClsGharardad = new GharardadNew(contextGuid, setting);
			InitializeComponent();
			_font = font;
			ShiftProgrammableOnly = pShiftProgrammableOnly;
			((Control)ucPanel1.panelMain).Controls.Add((Control)(object)panelEx1);
			((Control)panelEx1).Top = 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCTakGharardad.cs", 35);
		}
	}

	private void UCTakGHarardad_Load(object sender, EventArgs e)
	{
		try
		{
			((Control)this).Size = ((Control)ucPanel1).Size;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "UCTakGHarardad_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCTakGharardad.cs", 58);
		}
	}

	private async void BTNMore_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtGharardodNo).Focus();
			FrmGharardadList frm = new FrmGharardadList(ContextGuid, _font, isSelect: true, ShiftProgrammableOnly);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Control)lblname).ResetText();
			((Control)TxtGharardodNo).Text = "";
			ClsGharardad = frm.SelectedGharardad;
			if (ClsGharardad == null)
			{
				FMessageBox.Show("قراردادی انتخاب نشده است", "انتخاب قرارداد", FMessageBox.FMessageBoxButtons.OK, FMessageBox.FMessageBoxIcon.Question);
				return;
			}
			((Control)lblname).Text = ClsGharardad?.FullName;
			((Control)TxtGharardodNo).Text = ClsGharardad.No;
			((Component)(object)frm).Dispose();
			((Form)frm).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BTNMore_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCTakGharardad.cs", 103);
		}
	}

	private void TxtGharardodNo_Enter(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtGharardodNo).BackColor = Color.Yellow;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtGharardodNo_Enter", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCTakGharardad.cs", 112);
		}
	}

	private void TxtGharardodNo_Leave(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtGharardodNo).BackColor = Color.White;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtGharardodNo_Leave", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCTakGharardad.cs", 130);
		}
	}

	private async void TxtGharardodNo_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (!string.IsNullOrEmpty(ClsGharardad?.No))
			{
				((Control)lblname).Text = ClsGharardad?.FullName;
			}
			else
			{
				((Control)lblname).Text = "";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtGharardodNo_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCTakGharardad.cs", 155);
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fc: Expected O, but got Unknown
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0600: Expected O, but got Unknown
		//IL_0602: Unknown result type (might be due to invalid IL or missing references)
		panelEx1 = new PanelEx();
		lblname = new LabelX();
		labelX2 = new LabelX();
		BTNMore = new ButtonX();
		labelX1 = new LabelX();
		TxtGharardodNo = new TextBoxX();
		ucPanel1 = new UCPanel();
		((Control)panelEx1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelEx1).Anchor = (AnchorStyles)15;
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)lblname);
		((Control)panelEx1).Controls.Add((Control)(object)labelX2);
		((Control)panelEx1).Controls.Add((Control)(object)BTNMore);
		((Control)panelEx1).Controls.Add((Control)(object)labelX1);
		((Control)panelEx1).Controls.Add((Control)(object)TxtGharardodNo);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)panelEx1).Location = new Point(0, 29);
		((Control)panelEx1).Margin = new Padding(0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(600, 34);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 2;
		((Control)lblname).Anchor = (AnchorStyles)13;
		((BaseItemControl)lblname).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblname).ForeColor = Color.DarkRed;
		((Control)lblname).Location = new Point(3, 7);
		((Control)lblname).Name = "lblname";
		((Control)lblname).RightToLeft = (RightToLeft)1;
		((Control)lblname).Size = new Size(260, 23);
		((Control)lblname).TabIndex = 7;
		((Control)lblname).Text = "----------";
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(288, 7);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).RightToLeft = (RightToLeft)1;
		((Control)labelX2).Size = new Size(71, 23);
		((Control)labelX2).TabIndex = 6;
		((Control)labelX2).Text = "[نام شخص] :";
		((Control)BTNMore).AccessibleRole = (AccessibleRole)43;
		((Control)BTNMore).Anchor = (AnchorStyles)9;
		BTNMore.ColorTable = (eButtonColor)6;
		((Control)BTNMore).Location = new Point(378, 7);
		((Control)BTNMore).Name = "BTNMore";
		((Control)BTNMore).Size = new Size(35, 21);
		((PopupItemControl)BTNMore).Style = (eDotNetBarStyle)9;
		((Control)BTNMore).TabIndex = 5;
		((Control)BTNMore).Text = "...";
		((Control)BTNMore).Click += BTNMore_Click;
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(497, 7);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(85, 23);
		((Control)labelX1).TabIndex = 4;
		((Control)labelX1).Text = "[شماره قرارداد]: ";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)TxtGharardodNo).Anchor = (AnchorStyles)9;
		TxtGharardodNo.Border.Class = "TextBoxBorder";
		TxtGharardodNo.Border.CornerType = (eCornerType)1;
		((Control)TxtGharardodNo).Location = new Point(424, 7);
		((Control)TxtGharardodNo).Name = "TxtGharardodNo";
		((TextBoxBase)TxtGharardodNo).ReadOnly = true;
		((Control)TxtGharardodNo).Size = new Size(64, 21);
		((Control)TxtGharardodNo).TabIndex = 3;
		((Control)ucPanel1).Anchor = (AnchorStyles)13;
		ucPanel1.Expand = true;
		((Control)ucPanel1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		ucPanel1.HeaderText = "";
		ucPanel1.HeaderText2 = "[انتخاب قرارداد]";
		((Control)ucPanel1).Location = new Point(0, 0);
		((Control)ucPanel1).Margin = new Padding(0);
		((Control)ucPanel1).MaximumSize = new Size(600, 400);
		((Control)ucPanel1).MinimumSize = new Size(600, 10);
		((Control)ucPanel1).Name = "ucPanel1";
		((Control)ucPanel1).Size = new Size(600, 167);
		((Control)ucPanel1).TabIndex = 1;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Controls.Add((Control)(object)ucPanel1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(0);
		((Control)this).MaximumSize = new Size(600, 64);
		((Control)this).MinimumSize = new Size(600, 10);
		((Control)this).Name = "UCTakGharardad";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(600, 64);
		((UserControl)this).Load += UCTakGHarardad_Load;
		((Control)panelEx1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}

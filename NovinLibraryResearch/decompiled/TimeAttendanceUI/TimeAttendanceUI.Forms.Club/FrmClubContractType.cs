using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.FLRClub;
using TimeAttendanceDAL.Enums;

namespace TimeAttendanceUI.Forms.Club;

public class FrmClubContractType : FormExtention, IClubForms
{
	private readonly TimeAttendanceDAL.Dal.FLRClub.Club _club;

	private IClubForms _nextPage;

	private IContainer components;

	public PanelEx PanelEx1;

	private GroupPanel groupPanel4;

	internal LabelX labelX10;

	private RadioButton RbtnDaily;

	private RadioButton RbtnClockly;

	private RadioButton RbtnMonthly;

	public FrmClubContractType(string contextGuid, TimeAttendanceDAL.Dal.FLRClub.Club club)
		: base(contextGuid, true, true)
	{
		try
		{
			InitializeComponent();
			_club = club;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubContractType.cs", 21);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		try
		{
			((FormExtention)this).OnLoad(e);
			((Control)base.statusStrip1).Visible = false;
			FillFrmFields();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubContractType.cs", 34);
		}
	}

	public void FillFrmFields()
	{
		try
		{
			switch (_club.ContractType)
			{
			case EnumsHoghogh.EnumkarkardSt.Monthly:
				RbtnMonthly.Checked = true;
				break;
			case EnumsHoghogh.EnumkarkardSt.Daily:
				RbtnDaily.Checked = true;
				break;
			case EnumsHoghogh.EnumkarkardSt.Hourly:
				RbtnClockly.Checked = true;
				break;
			default:
				RbtnMonthly.Checked = true;
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillFrmFields", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubContractType.cs", 62);
		}
	}

	public IClubForms NextPage()
	{
		try
		{
			return _club.ContractType switch
			{
				EnumsHoghogh.EnumkarkardSt.Monthly => _nextPage = new FrmClubContractMonthly(((CacheBaseForm)this).ContextGuid, _club), 
				EnumsHoghogh.EnumkarkardSt.Daily => _nextPage = new FrmClubContractDaily(((CacheBaseForm)this).ContextGuid, _club), 
				EnumsHoghogh.EnumkarkardSt.Hourly => _nextPage = new FrmClubContractHourly(((CacheBaseForm)this).ContextGuid, _club), 
				_ => _nextPage = new FrmClubContractMonthly(((CacheBaseForm)this).ContextGuid, _club), 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "NextPage", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubContractType.cs", 80);
		}
		return null;
	}

	private void Rbtns_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (RbtnDaily.Checked)
			{
				_club.ContractType = EnumsHoghogh.EnumkarkardSt.Daily;
			}
			else if (RbtnClockly.Checked)
			{
				_club.ContractType = EnumsHoghogh.EnumkarkardSt.Hourly;
			}
			else
			{
				_club.ContractType = EnumsHoghogh.EnumkarkardSt.Monthly;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Rbtns_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubContractType.cs", 99);
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
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_041c: Expected O, but got Unknown
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d1: Expected O, but got Unknown
		//IL_0588: Unknown result type (might be due to invalid IL or missing references)
		//IL_0592: Expected O, but got Unknown
		//IL_066e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0678: Expected O, but got Unknown
		PanelEx1 = new PanelEx();
		groupPanel4 = new GroupPanel();
		RbtnDaily = new RadioButton();
		RbtnClockly = new RadioButton();
		RbtnMonthly = new RadioButton();
		labelX10 = new LabelX();
		((Control)PanelEx1).SuspendLayout();
		((Control)groupPanel4).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)groupPanel4);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Font = new Font("B Homa", 9.75f);
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Margin = new Padding(3, 4, 3, 4);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).RightToLeft = (RightToLeft)1;
		((Control)PanelEx1).Size = new Size(582, 500);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 0;
		((PanelControl)groupPanel4).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel4).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel4).Controls.Add((Control)(object)RbtnDaily);
		((Control)groupPanel4).Controls.Add((Control)(object)RbtnClockly);
		((Control)groupPanel4).Controls.Add((Control)(object)RbtnMonthly);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX10);
		groupPanel4.DisabledBackColor = Color.Empty;
		((Control)groupPanel4).Dock = (DockStyle)5;
		((Control)groupPanel4).Location = new Point(0, 0);
		((Control)groupPanel4).Name = "groupPanel4";
		((Control)groupPanel4).Size = new Size(582, 500);
		((PanelControl)groupPanel4).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel4).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel4).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel4).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel4).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel4).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel4).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel4).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel4).Style.CornerDiameter = 4;
		((PanelControl)groupPanel4).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel4).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel4).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel4).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel4).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel4).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel4).TabIndex = 5;
		((Control)groupPanel4).Text = "فرم انتخاب نوع قرارداد";
		((Control)RbtnDaily).Anchor = (AnchorStyles)9;
		((Control)RbtnDaily).Font = new Font("B Homa", 11f);
		((Control)RbtnDaily).ForeColor = SystemColors.HotTrack;
		((Control)RbtnDaily).Location = new Point(347, 162);
		((Control)RbtnDaily).Name = "RbtnDaily";
		((Control)RbtnDaily).Size = new Size(127, 33);
		((Control)RbtnDaily).TabIndex = 17;
		((Control)RbtnDaily).Text = "روزهای خاص";
		((ButtonBase)RbtnDaily).UseVisualStyleBackColor = true;
		RbtnDaily.CheckedChanged += Rbtns_CheckedChanged;
		((Control)RbtnClockly).Anchor = (AnchorStyles)9;
		((Control)RbtnClockly).Font = new Font("B Homa", 11f);
		((Control)RbtnClockly).ForeColor = SystemColors.HotTrack;
		((Control)RbtnClockly).Location = new Point(347, 202);
		((Control)RbtnClockly).Name = "RbtnClockly";
		((Control)RbtnClockly).Size = new Size(127, 33);
		((Control)RbtnClockly).TabIndex = 18;
		((Control)RbtnClockly).Text = "ساعتی";
		((ButtonBase)RbtnClockly).UseVisualStyleBackColor = true;
		RbtnClockly.CheckedChanged += Rbtns_CheckedChanged;
		((Control)RbtnMonthly).Anchor = (AnchorStyles)9;
		RbtnMonthly.Checked = true;
		((Control)RbtnMonthly).Font = new Font("B Homa", 11f);
		((Control)RbtnMonthly).ForeColor = SystemColors.HotTrack;
		((Control)RbtnMonthly).Location = new Point(347, 122);
		((Control)RbtnMonthly).Name = "RbtnMonthly";
		((Control)RbtnMonthly).Size = new Size(127, 33);
		((Control)RbtnMonthly).TabIndex = 16;
		RbtnMonthly.TabStop = true;
		((Control)RbtnMonthly).Text = "ماهیانه";
		((ButtonBase)RbtnMonthly).UseVisualStyleBackColor = true;
		RbtnMonthly.CheckedChanged += Rbtns_CheckedChanged;
		((Control)labelX10).Anchor = (AnchorStyles)9;
		((Control)labelX10).BackColor = Color.Transparent;
		((BaseItemControl)labelX10).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX10).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX10).Location = new Point(414, 72);
		((Control)labelX10).Name = "labelX10";
		((Control)labelX10).Size = new Size(159, 23);
		((Control)labelX10).TabIndex = 12;
		((Control)labelX10).Text = "انتخاب نوع قرارداد عضویت:";
		labelX10.TextAlignment = (StringAlignment)2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(582, 500);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "FrmClubContractInfo";
		((Control)this).Text = "FrmCheckOutVacation";
		((Control)PanelEx1).ResumeLayout(false);
		((Control)groupPanel4).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}

	bool IClubForms.get_TopLevel()
	{
		return ((Form)this).TopLevel;
	}

	void IClubForms.set_TopLevel(bool value)
	{
		((Form)this).TopLevel = value;
	}

	void IClubForms.Show()
	{
		((Control)this).Show();
	}
}

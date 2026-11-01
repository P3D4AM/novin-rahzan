using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom.Cache;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceDAL.Dal.Other;
using TimeAttendanceUI.Properties;
using TimeAttendanceUI.Salary.Shift;

namespace TimeAttendanceUI.Salary.UC;

public class UCLblDay : UserControl, IHasContext
{
	private Font _font;

	private List<ShiftCodeing> _tShiftCodeings;

	private IContainer components;

	private PanelEx panelEx1;

	private ToolTip toolTip1;

	public string ContextGuid { get; set; }

	public string txtLbl
	{
		get
		{
			return ((Control)panelEx1).Text;
		}
		set
		{
			((Control)panelEx1).Text = value;
		}
	}

	public bool isSelected { get; set; }

	public Holiday Holiday { get; set; }

	public bool BtnEnabled
	{
		get
		{
			return ((Control)panelEx1).Enabled;
		}
		set
		{
			((Control)panelEx1).Enabled = value;
		}
	}

	public DateTime DateM { get; set; }

	public List<ShiftCodeing> tShiftCodeings
	{
		get
		{
			return _tShiftCodeings ?? new List<ShiftCodeing>();
		}
		set
		{
			if (value != null && value.Count > 0)
			{
				_tShiftCodeings = value;
				if (_tShiftCodeings.Any((ShiftCodeing p) => p.H1 > 0 || p.H2 > 0 || p.M1 > 0 || p.M2 > 0))
				{
					ColorEx backColor = panelEx1.Style.BackColor1;
					Color color2 = (panelEx1.Style.BackColor2.Color = ColorTranslator.FromHtml(_tShiftCodeings[0].ShiftColor));
					backColor.Color = color2;
				}
				else
				{
					ColorEx backColor2 = panelEx1.Style.BackColor1;
					Color color2 = (panelEx1.Style.BackColor2.Color = Color.MistyRose);
					backColor2.Color = color2;
				}
			}
			else
			{
				_tShiftCodeings = new List<ShiftCodeing>();
			}
		}
	}

	public bool workOnHolidayWithOutFriday { get; set; }

	public ShiftDet.FriDayType FriDayType { get; set; } = ShiftDet.FriDayType.Elzami;


	public UCLblDay(string contextGuid, Font font)
	{
		_font = font;
		ContextGuid = contextGuid;
		InitializeComponent();
		((Control)this).DoubleBuffered = true;
	}

	public void SetShiftToolTip(Holiday pHoliday)
	{
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Invalid comparison between Unknown and I4
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Invalid comparison between Unknown and I4
		try
		{
			string text = "";
			if (pHoliday != null)
			{
				text += "[تعطیل رسمی] \n";
				text += pHoliday.Description;
				panelEx1.Style.ForeColor.Color = Color.Red;
			}
			List<ShiftCodeing> list = tShiftCodeings;
			if (list != null && list.Count > 0)
			{
				foreach (ShiftCodeing tShiftCodeing in tShiftCodeings)
				{
					text = ((!(text.Trim() == "")) ? (text + "\n\n,[شیفت ورود] :" + LanguageAndCalendar.Instance.CaltureCalender.DateToFormatTime(tShiftCodeing.Vorod) + "\n[شیفت خروج] :" + LanguageAndCalendar.Instance.CaltureCalender.DateToFormatTime(tShiftCodeing.Khoroj) + "\n" + tShiftCodeing.Description) : (((int)tShiftCodeing.TypeShift != 2) ? (((int)tShiftCodeing.TypeShift != 11) ? (text + "[شیفت ورود] :" + LanguageAndCalendar.Instance.CaltureCalender.DateToFormatTime(tShiftCodeing.Vorod) + "\n[شیفت خروج] :" + LanguageAndCalendar.Instance.CaltureCalender.DateToFormatTime(tShiftCodeing.Khoroj) + "\n" + tShiftCodeing.Description) : (text + "[شیفت استراحت]")) : (text + "[شیفت شناور]  \n" + LanguageAndCalendar.Instance.CaltureCalender.DateToFormatTime(tShiftCodeing.Khoroj) + "\n" + tShiftCodeing.Description)));
				}
			}
			else
			{
				text += "  [بدون شیفت] ...";
			}
			text = Utilities.GetTranslate(text);
			toolTip1.SetToolTip((Control)(object)panelEx1, text);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetShiftToolTip", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\UC\\UCLblDay.cs", 120);
		}
	}

	public void ResetLblDay()
	{
		try
		{
			((UserControl)this).BorderStyle = (BorderStyle)0;
			panelEx1.Style.BackgroundImage = (Image)(object)Resources.check;
			SetShiftToolTip(Holiday);
			isSelected = false;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ResetLblDay", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\UC\\UCLblDay.cs", 132);
		}
	}

	private async void labelX1_Click(object sender, EventArgs e)
	{
		try
		{
			if (FrmShiftProgrammable.ShiftCodingLst != null)
			{
				if (isSelected)
				{
					((UserControl)this).BorderStyle = (BorderStyle)0;
					panelEx1.Style.ResetBackgroundImage();
					isSelected = false;
				}
				else
				{
					((UserControl)this).BorderStyle = (BorderStyle)2;
					isSelected = true;
					panelEx1.Style.BackgroundImage = (Image)(object)Resources.accept1;
				}
			}
			else
			{
				FrmShiftCodeingList frm = new FrmShiftCodeingList(ContextGuid, _font, isSelect: true, (TypeShiftEnumType)0, tShiftCodeings);
				if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
				{
					tShiftCodeings = frm.ListShiftCodeingsSelected;
					SetShiftToolTip(Holiday);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "labelX1_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\UC\\UCLblDay.cs", 164);
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Expected O, but got Unknown
		components = new Container();
		panelEx1 = new PanelEx();
		toolTip1 = new ToolTip(components);
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(34, 26);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)panelEx1).Text = "01";
		((Control)panelEx1).Click += labelX1_Click;
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)this).Name = "UCLblDay";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(34, 26);
		((Control)this).ResumeLayout(false);
	}
}

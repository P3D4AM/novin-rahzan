using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using TimeAttendanceDAL.Dal.FLRShift;

namespace TimeAttendanceUI.Salary.Shift;

public class FrmCrtShiftCircle2 : BaseForm
{
	private Font _font;

	private IContainer components;

	public PanelEx PanelEx1;

	private GroupPanel groupPanel7;

	internal DataGridViewX DGrid;

	private DataGridViewTextBoxColumn DGShiftCodingID;

	private DataGridViewTextBoxColumn DGRadif;

	private DataGridViewTextBoxColumn DGDate;

	private DataGridViewButtonColumn DGSelectedShift;

	private DataGridViewTextBoxColumn DGAsHour;

	private DataGridViewTextBoxColumn DGTaHour;

	private DateTime DateStart { get; set; }

	private int DaysOfCircle { get; set; }

	public FrmCrtShiftCircle2(string contextGuid, Font font)
		: base(contextGuid, (BaseFormConfig)null)
	{
		_font = font;
		InitializeComponent();
	}

	public void Init(int dayofCircle, DateTime startDate)
	{
		try
		{
			DaysOfCircle = dayofCircle;
			DateStart = startDate;
			AddRowDGrid();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Init", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftCircle2.cs", 35);
		}
	}

	private void AddRowDGrid()
	{
		try
		{
			for (int i = ((DataGridView)DGrid).RowCount; i < DaysOfCircle; i++)
			{
				((DataGridView)DGrid).Rows.Add();
				((DataGridView)DGrid)[((DataGridViewBand)DGRadif).Index, ((DataGridView)DGrid).RowCount - 1].Value = ((DataGridView)DGrid).RowCount.ToString();
				DateTime dateTime = DateStart.AddDays(((DataGridView)DGrid).RowCount - 1);
				((DataGridView)DGrid)[((DataGridViewBand)DGDate).Index, ((DataGridView)DGrid).RowCount - 1].Value = Calendar.MiladiToCalture(dateTime) + " " + LanguageAndCalendar.Instance.CaltureCalender.GetDayNameOfWeek(dateTime);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "AddRowDGrid", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftCircle2.cs", 52);
		}
	}

	private async void DGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			FrmShiftCodeingList frm = new FrmShiftCodeingList(((CacheBaseForm)this).ContextGuid, _font, isSelect: true, (TypeShiftEnumType)100);
			if ((int)((Form)frm).ShowDialog() == 1)
			{
				ShiftCodeing shiftCodeing = await ShiftCodeing.GetAsync(((CacheBaseForm)this).ContextGuid, frm.SelectedRow.Id);
				((DataGridView)DGrid)[((DataGridViewBand)DGAsHour).Index, e.RowIndex].Value = $" {shiftCodeing.M1:00} : {shiftCodeing.H1:00}";
				((DataGridView)DGrid)[((DataGridViewBand)DGTaHour).Index, e.RowIndex].Value = $" {shiftCodeing.M2:00} : {shiftCodeing.H2:00}";
				((DataGridView)DGrid)[((DataGridViewBand)DGSelectedShift).Index, e.RowIndex].Value = shiftCodeing.Description;
				((DataGridView)DGrid)[((DataGridViewBand)DGShiftCodingID).Index, e.RowIndex].Value = frm.SelectedRow.Id.ToString();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_CellClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftCircle2.cs", 69);
		}
	}

	public short GetShiftCodingId(int rowidx)
	{
		try
		{
			if (((DataGridView)DGrid).Rows.Count == 0 || ((DataGridView)DGrid)[((DataGridViewBand)DGShiftCodingID).Index, rowidx].Value == null)
			{
				return 0;
			}
			return Convert.ToInt16(((DataGridView)DGrid)[((DataGridViewBand)DGShiftCodingID).Index, rowidx].Value.ToString());
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetShiftCodingId", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftCircle2.cs", 82);
		}
		return 0;
	}

	public List<short> GetLstShiftId()
	{
		List<short> list = new List<short>();
		try
		{
			for (int i = 0; i < ((DataGridView)DGrid).RowCount; i++)
			{
				short shiftCodingId = GetShiftCodingId(i);
				if (shiftCodingId > 0 && list.IndexOf(shiftCodingId) == -1)
				{
					list.Add(shiftCodingId);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetLstShiftId", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftCircle2.cs", 100);
		}
		return list;
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Expected O, but got Unknown
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		//IL_0506: Unknown result type (might be due to invalid IL or missing references)
		//IL_0510: Expected O, but got Unknown
		//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f3: Expected O, but got Unknown
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		PanelEx1 = new PanelEx();
		groupPanel7 = new GroupPanel();
		DGrid = new DataGridViewX();
		DGShiftCodingID = new DataGridViewTextBoxColumn();
		DGRadif = new DataGridViewTextBoxColumn();
		DGDate = new DataGridViewTextBoxColumn();
		DGSelectedShift = new DataGridViewButtonColumn();
		DGAsHour = new DataGridViewTextBoxColumn();
		DGTaHour = new DataGridViewTextBoxColumn();
		((Control)PanelEx1).SuspendLayout();
		((Control)groupPanel7).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)groupPanel7);
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
		((Control)PanelEx1).TabIndex = 2;
		((Control)groupPanel7).Anchor = (AnchorStyles)15;
		((Control)groupPanel7).BackColor = Color.Transparent;
		((PanelControl)groupPanel7).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel7).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel7).Controls.Add((Control)(object)DGrid);
		((Control)groupPanel7).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)groupPanel7).Location = new Point(14, 12);
		((Control)groupPanel7).Name = "groupPanel7";
		((Control)groupPanel7).Size = new Size(554, 476);
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
		((Control)groupPanel7).TabIndex = 1;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoSizeColumnsMode = (DataGridViewAutoSizeColumnsMode)6;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[6]
		{
			(DataGridViewColumn)DGShiftCodingID,
			(DataGridViewColumn)DGRadif,
			(DataGridViewColumn)DGDate,
			(DataGridViewColumn)DGSelectedShift,
			(DataGridViewColumn)DGAsHour,
			(DataGridViewColumn)DGTaHour
		});
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(3, 3);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(540, 464);
		((Control)DGrid).TabIndex = 12;
		((DataGridView)DGrid).CellClick += new DataGridViewCellEventHandler(DGrid_CellClick);
		((DataGridViewColumn)DGShiftCodingID).HeaderText = "ShiftCodingID";
		((DataGridViewColumn)DGShiftCodingID).Name = "DGShiftCodingID";
		((DataGridViewBand)DGShiftCodingID).ReadOnly = true;
		((DataGridViewBand)DGShiftCodingID).Visible = false;
		((DataGridViewColumn)DGRadif).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGRadif).HeaderText = "[ردیف]";
		((DataGridViewColumn)DGRadif).MinimumWidth = 30;
		((DataGridViewColumn)DGRadif).Name = "DGRadif";
		((DataGridViewBand)DGRadif).ReadOnly = true;
		((DataGridViewColumn)DGRadif).Width = 59;
		((DataGridViewColumn)DGDate).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGDate).HeaderText = "[تاریخ]";
		((DataGridViewColumn)DGDate).MinimumWidth = 150;
		((DataGridViewColumn)DGDate).Name = "DGDate";
		((DataGridViewBand)DGDate).ReadOnly = true;
		((DataGridViewColumn)DGDate).Width = 150;
		((DataGridViewColumn)DGSelectedShift).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DGSelectedShift).HeaderText = "[انتخاب شیفت]";
		((DataGridViewColumn)DGSelectedShift).MinimumWidth = 100;
		((DataGridViewColumn)DGSelectedShift).Name = "DGSelectedShift";
		((DataGridViewBand)DGSelectedShift).ReadOnly = true;
		((DataGridViewBand)DGSelectedShift).Resizable = (DataGridViewTriState)1;
		((DataGridViewColumn)DGSelectedShift).SortMode = (DataGridViewColumnSortMode)1;
		((DataGridViewColumn)DGAsHour).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGAsHour).HeaderText = "[از ساعت]";
		((DataGridViewColumn)DGAsHour).MinimumWidth = 30;
		((DataGridViewColumn)DGAsHour).Name = "DGAsHour";
		((DataGridViewBand)DGAsHour).ReadOnly = true;
		((DataGridViewColumn)DGAsHour).Width = 78;
		((DataGridViewColumn)DGTaHour).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGTaHour).HeaderText = "[تا ساعت]";
		((DataGridViewColumn)DGTaHour).MinimumWidth = 30;
		((DataGridViewColumn)DGTaHour).Name = "DGTaHour";
		((DataGridViewBand)DGTaHour).ReadOnly = true;
		((DataGridViewColumn)DGTaHour).Width = 77;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((ScrollableControl)this).AutoScroll = true;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(582, 500);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "FrmCrtShiftCircle2";
		((Control)this).Text = "[ایجاد شیفت]";
		((Control)PanelEx1).ResumeLayout(false);
		((Control)groupPanel7).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((Control)this).ResumeLayout(false);
	}
}

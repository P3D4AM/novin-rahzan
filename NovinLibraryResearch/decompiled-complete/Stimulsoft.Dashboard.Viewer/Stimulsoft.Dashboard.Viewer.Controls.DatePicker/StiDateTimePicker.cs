using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Dashboard.Viewer.Controls.Helpers;
using Stimulsoft.Dashboard.Viewer.Elements;
using Stimulsoft.Report;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Controls.DatePicker;

[ToolboxItem(false)]
public class StiDateTimePicker : Control
{
	private DateTime dropDownClosedTime;

	private StiDateTimeInput pickerSelectionStart;

	private StiDateTimeInput pickerSelectionEnd;

	private StiControlColorCollection colors;

	private MonthCalendar calendar;

	private StiDateRangeControl dateRangeControl;

	private bool isHotButton;

	private bool isEditing;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiControlColorCollection Colors
	{
		get
		{
			return colors;
		}
		set
		{
			if (value != colors)
			{
				colors = value;
				colors.Control = (Control)(object)this;
				((Control)this).Invalidate();
			}
		}
	}

	private MonthCalendar Calendar
	{
		get
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			if (calendar == null)
			{
				calendar = new MonthCalendar
				{
					MaxSelectionCount = 1,
					ShowTodayCircle = false
				};
				calendar.DateChanged += (DateRangeEventHandler)delegate
				{
					((Control)pickerSelectionStart).Text = SelectionStartStr;
					((Control)pickerSelectionEnd).Text = SelectionEndStr;
					((Control)this).Invalidate();
					this.DateChanged?.Invoke(this, EventArgs.Empty);
				};
			}
			return calendar;
		}
	}

	private StiDateRangeControl DateRangeControl
	{
		get
		{
			if (dateRangeControl == null)
			{
				dateRangeControl = new StiDateRangeControl();
				dateRangeControl.Colors = Colors;
				dateRangeControl.DateChanged += delegate
				{
					((Control)pickerSelectionStart).Text = SelectionStartStr;
					((Control)pickerSelectionEnd).Text = SelectionEndStr;
					((Control)this).Invalidate();
					this.DateChanged?.Invoke(this, EventArgs.Empty);
				};
			}
			return dateRangeControl;
		}
	}

	private StiDropDown DropDownPopup { get; set; }

	internal StiFormatService TextFormat { get; set; }

	private bool IsHotButton
	{
		get
		{
			return isHotButton;
		}
		set
		{
			if (isHotButton != value)
			{
				isHotButton = value;
				((Control)this).Invalidate();
			}
		}
	}

	internal StiReport Report { get; set; }

	private string DefaultDateFormat
	{
		get
		{
			string shortDatePattern = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
			string[] array = shortDatePattern.Split(new string[1] { CultureInfo.CurrentCulture.DateTimeFormat.DateSeparator }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Contains("y"))
				{
					array[i] = "yyyy";
				}
				else if (array[i].Contains("M"))
				{
					array[i] = "MM";
				}
				else if (array[i].Contains("d"))
				{
					array[i] = "dd";
				}
			}
			return string.Join(CultureInfo.CurrentCulture.DateTimeFormat.DateSeparator, array);
		}
	}

	private string SelectionStartStr
	{
		get
		{
			CultureInfo culture = StiCultureHelper.Set(Report);
			try
			{
				return SelectionStart.ToString(DefaultDateFormat);
			}
			finally
			{
				StiCultureHelper.Restore(culture);
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public DateTime SelectionStart
	{
		get
		{
			if (SelectionMode == StiDateSelectionMode.Single)
			{
				return Calendar.SelectionStart;
			}
			return DateRangeControl.SelectionStart;
		}
		set
		{
			if (SelectionMode == StiDateSelectionMode.Single)
			{
				Calendar.SelectionStart = value;
			}
			else
			{
				DateRangeControl.SelectionStart = value;
			}
		}
	}

	private string SelectionEndStr
	{
		get
		{
			CultureInfo culture = StiCultureHelper.Set(Report);
			try
			{
				return SelectionEnd.ToString(DefaultDateFormat);
			}
			finally
			{
				StiCultureHelper.Restore(culture);
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public DateTime SelectionEnd
	{
		get
		{
			if (SelectionMode == StiDateSelectionMode.Single)
			{
				return Calendar.SelectionStart;
			}
			return DateRangeControl.SelectionEnd;
		}
		set
		{
			if (SelectionMode == StiDateSelectionMode.Single)
			{
				Calendar.SelectionStart = value;
			}
			else
			{
				DateRangeControl.SelectionEnd = value;
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public DateTimeRange SelectionRange
	{
		get
		{
			return new DateTimeRange(SelectionStart, SelectionEnd);
		}
		set
		{
			SelectionStart = value.FromDate;
			SelectionEnd = value.ToDate;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public StiDateSelectionMode SelectionMode { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool DroppedDown
	{
		get
		{
			if (DropDownPopup != null)
			{
				return ((ToolStripDropDown)DropDownPopup).Visible;
			}
			return false;
		}
	}

	public bool IsTransparent { get; set; }

	public bool IsEditing
	{
		get
		{
			return isEditing;
		}
		set
		{
			if (isEditing != value)
			{
				isEditing = value;
				((Control)this).Invalidate();
			}
		}
	}

	public override Color BackColor
	{
		get
		{
			if (!IsTransparent)
			{
				return ((Control)this).BackColor;
			}
			return Color.Transparent;
		}
		set
		{
			((Control)this).BackColor = (IsTransparent ? Color.Transparent : value);
		}
	}

	public event EventHandler DateChanged;

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Rectangle rect = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		Color foreColor = Colors.GetForeColor(isSelected: false, isHot: false);
		Color backColor = Colors.GetBackColor(isSelected: false, isHot: false);
		if (IsEditing)
		{
			StiDrawing.FillRectangle(graphics, backColor, rect);
		}
		StringFormat val = new StringFormat();
		try
		{
			val.FormatFlags = (StringFormatFlags)4096;
			val.Trimming = (StringTrimming)3;
			val.LineAlignment = (StringAlignment)1;
			if (!((Control)pickerSelectionStart).Visible)
			{
				string text = Format(SelectionStart);
				Rectangle selectionStartRect = GetSelectionStartRect();
				StiDrawing.DrawString(graphics, text, ((Control)this).Font, foreColor, selectionStartRect, val);
			}
			if (SelectionMode != 0)
			{
				Rectangle selectionSeparatorRect = GetSelectionSeparatorRect();
				StiDrawing.DrawString(graphics, "-", ((Control)this).Font, foreColor, selectionSeparatorRect, val);
			}
			if (!((Control)pickerSelectionEnd).Visible && SelectionMode != 0)
			{
				string text2 = Format(SelectionEnd);
				Rectangle selectionEndRect = GetSelectionEndRect();
				StiDrawing.DrawString(graphics, text2, ((Control)this).Font, foreColor, selectionEndRect, val);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		DrawButton(graphics);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
		IsHotButton = GetIsHotButton();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		IsHotButton = false;
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		((Control)this).Cursor = (GetIsOverPicker() ? Cursors.IBeam : Cursors.Default);
		IsHotButton = GetIsHotButton();
		((Control)this).Invalidate();
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		if (IsHotButton && !DroppedDown && (DateTime.Now - dropDownClosedTime).TotalSeconds > 0.5)
		{
			PickerSelectionStartValidating();
			PickerSelectionEndValidating();
			ShowDropDown();
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
		Rectangle buttonRect = GetButtonRect();
		if (e.Location.X < buttonRect.X)
		{
			if (GetSelectionStartRect().Contains(e.Location))
			{
				((Control)pickerSelectionStart).Visible = true;
				((Control)pickerSelectionStart).Focus();
				RefreshLocations();
				((TextBoxBase)pickerSelectionStart).SelectionStart = 0;
				((TextBoxBase)pickerSelectionStart).SelectionStart = ((TextBoxBase)pickerSelectionStart).GetCharIndexFromPosition(e.Location);
				IsEditing = true;
			}
			else if (GetSelectionEndRect().Contains(e.Location))
			{
				((Control)pickerSelectionEnd).Visible = true;
				((Control)pickerSelectionEnd).Focus();
				RefreshLocations();
				((TextBoxBase)pickerSelectionEnd).SelectionStart = 0;
				((TextBoxBase)pickerSelectionEnd).SelectionStart = ((TextBoxBase)pickerSelectionEnd).GetCharIndexFromPosition(e.Location);
				IsEditing = true;
			}
		}
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		((Control)this).OnSizeChanged(e);
		RefreshLocations();
	}

	protected override void OnFontChanged(EventArgs e)
	{
		((Control)this).OnFontChanged(e);
		((Control)pickerSelectionStart).Font = ((Control)this).Font;
		((Control)pickerSelectionEnd).Font = ((Control)this).Font;
	}

	protected override void OnBackColorChanged(EventArgs e)
	{
		((Control)this).OnBackColorChanged(e);
		((Control)pickerSelectionStart).BackColor = ((Control)this).BackColor;
		((Control)pickerSelectionEnd).BackColor = ((Control)this).BackColor;
	}

	protected override void OnForeColorChanged(EventArgs e)
	{
		((Control)this).OnForeColorChanged(e);
		((Control)pickerSelectionStart).ForeColor = ((Control)this).ForeColor;
		((Control)pickerSelectionEnd).ForeColor = ((Control)this).ForeColor;
	}

	protected override void OnGotFocus(EventArgs e)
	{
		((Control)this).OnGotFocus(e);
		((Control)pickerSelectionStart).Visible = true;
		((Control)pickerSelectionStart).Focus();
		RefreshLocations();
		IsEditing = false;
	}

	private void RefreshLocations()
	{
		Rectangle selectionStartRect = GetSelectionStartRect();
		((Control)pickerSelectionStart).SetBounds(selectionStartRect.X, selectionStartRect.Y, selectionStartRect.Width, selectionStartRect.Height);
		Rectangle selectionEndRect = GetSelectionEndRect();
		((Control)pickerSelectionEnd).SetBounds(selectionEndRect.X, selectionEndRect.Y, selectionEndRect.Width, selectionEndRect.Height);
	}

	private Rectangle GetSelectionStartRect()
	{
		Rectangle buttonRect = GetButtonRect();
		int num = buttonRect.Left - StiScale.I4;
		if (SelectionMode == StiDateSelectionMode.Single)
		{
			return new Rectangle(StiScale.I2, (((Control)this).Height - ((Control)pickerSelectionStart).Height) / 2, buttonRect.Left - StiScale.I3, ((Control)pickerSelectionStart).Height);
		}
		int num2 = GetSelectionStartWidth();
		int selectionEndWidth = GetSelectionEndWidth();
		if (num2 + selectionEndWidth + StiScale.I8 > num)
		{
			num2 = (num - StiScale.I8) / 2;
		}
		return new Rectangle(StiScale.I2, (((Control)this).Height - ((Control)pickerSelectionStart).Height) / 2, num2, ((Control)pickerSelectionStart).Height);
	}

	private Rectangle GetSelectionSeparatorRect()
	{
		if (SelectionMode == StiDateSelectionMode.Single)
		{
			return Rectangle.Empty;
		}
		Rectangle selectionStartRect = GetSelectionStartRect();
		return new Rectangle(selectionStartRect.Right, selectionStartRect.Y, StiScale.I8 + StiScale.XXI(2.0), selectionStartRect.Height);
	}

	private Rectangle GetSelectionEndRect()
	{
		int num = GetButtonRect().Left - StiScale.I4;
		if (SelectionMode == StiDateSelectionMode.Single)
		{
			return Rectangle.Empty;
		}
		int selectionStartWidth = GetSelectionStartWidth();
		int num2 = GetSelectionEndWidth();
		if (selectionStartWidth + num2 + StiScale.I8 + StiScale.XXI(2.0) > num)
		{
			num2 = (num - StiScale.I8 + StiScale.XXI(2.0)) / 2;
		}
		return new Rectangle(StiScale.I2 + selectionStartWidth + StiScale.I8 + StiScale.XXI(2.0), (((Control)this).Height - ((Control)pickerSelectionStart).Height) / 2, num2, ((Control)pickerSelectionStart).Height);
	}

	private int GetSelectionStartWidth()
	{
		string text = (((Control)pickerSelectionStart).Visible ? ((Control)pickerSelectionStart).Text : Format(SelectionStart));
		int num = TextRenderer.MeasureText(text, ((Control)this).Font).Width + StiScale.XXI(2.0);
		if (((Control)pickerSelectionStart).Visible)
		{
			num += StiScale.I4;
		}
		return num;
	}

	private int GetSelectionEndWidth()
	{
		string text = (((Control)pickerSelectionEnd).Visible ? ((Control)pickerSelectionEnd).Text : Format(SelectionEnd));
		int num = TextRenderer.MeasureText(text, ((Control)this).Font).Width + StiScale.XXI(2.0);
		if (((Control)pickerSelectionEnd).Visible)
		{
			num += StiScale.I4;
		}
		return num;
	}

	private bool GetIsOverPicker()
	{
		Point pt = ((Control)this).PointToClient(Cursor.Position);
		if (GetSelectionStartRect().Contains(pt))
		{
			return true;
		}
		if (SelectionMode == StiDateSelectionMode.Single)
		{
			return false;
		}
		return GetSelectionEndRect().Contains(pt);
	}

	private bool GetIsHotButton()
	{
		return GetButtonRect().Contains(((Control)this).PointToClient(Cursor.Position));
	}

	private string Format(object value)
	{
		CultureInfo culture = StiCultureHelper.Set(Report);
		try
		{
			if (value is DateTime && !(TextFormat is StiDateFormatService))
			{
				return ((DateTime)value).ToString(DefaultDateFormat);
			}
			return TextFormat.Format(value);
		}
		finally
		{
			StiCultureHelper.Restore(culture);
		}
	}

	protected virtual void DrawButton(Graphics g)
	{
		Rectangle buttonRect = GetButtonRect();
		Color backColor = Colors.GetBackColor(isSelected: false, IsHotButton);
		StiDrawing.FillRectangle(g, backColor, buttonRect);
		StiButtonPainter.DrawButton(g, buttonRect, IsHotButton, Colors.GlyphColor, Colors.HotGlyphColor);
	}

	private Rectangle GetButtonRect()
	{
		return new Rectangle(((Control)this).Width - GetButtonWidth(), 0, GetButtonWidth(), ((Control)this).Height);
	}

	protected virtual int GetButtonWidth()
	{
		return SystemInformation.HorizontalScrollBarArrowWidth;
	}

	protected virtual void CloseDropDown()
	{
		StiDropDown dropDownPopup = DropDownPopup;
		if (dropDownPopup != null)
		{
			((ToolStripDropDown)dropDownPopup).Close();
		}
	}

	protected virtual void ShowDropDown()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		if (DropDownPopup == null)
		{
			DropDownPopup = new StiDropDown();
			((ToolStrip)DropDownPopup).BackColor = ((Control)this).BackColor;
			((ToolStripDropDown)DropDownPopup).Closed += new ToolStripDropDownClosedEventHandler(DropDownPopupClosed);
		}
		if (SelectionMode == StiDateSelectionMode.Single)
		{
			((Control)DropDownPopup).Width = ((Control)Calendar).Width;
			((Control)DropDownPopup).Height = ((Control)Calendar).Height;
			((Control)Calendar).Location = new Point(0, 0);
			((Control)Calendar).ForeColor = ((Control)this).ForeColor;
			((Control)Calendar).BackColor = ((Control)this).BackColor.RemoveAlpha();
		}
		else
		{
			DateRangeControl.RefreshSize();
			((Control)DropDownPopup).Width = ((Control)DateRangeControl).Width;
			((Control)DropDownPopup).Width = ((Control)DateRangeControl).Height;
			((Control)DateRangeControl).Location = new Point(0, 0);
		}
		Control val = (Control)(object)((SelectionMode == StiDateSelectionMode.Single) ? ((StiDateRangeControl)(object)Calendar) : DateRangeControl);
		ToolStripControlHost val2 = new ToolStripControlHost(val);
		((ToolStripItem)val2).AutoSize = false;
		if (SelectionMode == StiDateSelectionMode.Single)
		{
			((ToolStripItem)val2).Margin = new Padding(-1);
			((ToolStripItem)val2).Padding = new Padding(-1);
			((ToolStrip)DropDownPopup).BackColor = Color.Transparent;
			((ToolStripDropDown)DropDownPopup).AllowTransparency = true;
			((Control)DropDownPopup).Margin = new Padding(-1);
			((Control)DropDownPopup).Padding = new Padding(-1);
		}
		((ToolStrip)DropDownPopup).Items.Clear();
		((ToolStrip)DropDownPopup).Items.Add((ToolStripItem)(object)val2);
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		Point point = ((Control)this).PointToScreen(new Point(clientRectangle.Left, clientRectangle.Top + clientRectangle.Height));
		Rectangle workingArea = Screen.FromControl((Control)(object)this).WorkingArea;
		if (point.X + ((Control)this).Size.Width > workingArea.Left + workingArea.Width)
		{
			point.X = workingArea.Left + workingArea.Width - ((Control)this).Size.Width;
		}
		if (point.Y + ((Control)this).Size.Height > workingArea.Top + workingArea.Height)
		{
			point.Y -= ((Control)this).Size.Height + clientRectangle.Height;
		}
		point = ((Control)this).PointToClient(point);
		((ToolStripDropDown)DropDownPopup).Show((Control)(object)this, point);
	}

	private void DropDownPopupClosed(object sender, ToolStripDropDownClosedEventArgs e)
	{
		dropDownClosedTime = DateTime.Now;
	}

	private void PickerSelectionStartValidating()
	{
		CultureInfo culture = StiCultureHelper.Set(Report);
		try
		{
			if (DateTime.TryParseExact(((Control)pickerSelectionStart).Text, DefaultDateFormat, null, DateTimeStyles.None, out var result))
			{
				SelectionStart = result;
			}
			else
			{
				((Control)pickerSelectionStart).Text = SelectionStartStr;
			}
		}
		finally
		{
			StiCultureHelper.Restore(culture);
		}
	}

	private void PickerSelectionEndValidating()
	{
		CultureInfo culture = StiCultureHelper.Set(Report);
		try
		{
			if (DateTime.TryParseExact(((Control)pickerSelectionEnd).Text, DefaultDateFormat, null, DateTimeStyles.None, out var result))
			{
				SelectionEnd = result;
			}
			else
			{
				((Control)pickerSelectionEnd).Text = SelectionEndStr;
			}
		}
		finally
		{
			StiCultureHelper.Restore(culture);
		}
	}

	private void CreateDateTimePickerStart()
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		pickerSelectionStart = new StiDateTimeInput();
		((Control)pickerSelectionStart).Visible = false;
		((Control)pickerSelectionStart).Text = SelectionStartStr;
		((Control)pickerSelectionStart).GotFocus += delegate
		{
			((Control)pickerSelectionStart).Visible = true;
			IsEditing = true;
		};
		((Control)pickerSelectionStart).LostFocus += delegate
		{
			PickerSelectionStartValidating();
			((Control)pickerSelectionStart).Visible = false;
			IsEditing = false;
		};
		((Control)pickerSelectionStart).VisibleChanged += delegate
		{
			((Control)this).Invalidate();
		};
		((Control)pickerSelectionStart).KeyDown += (KeyEventHandler)delegate(object sender, KeyEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Invalid comparison between Unknown and I4
			if ((int)e.KeyCode == 13)
			{
				PickerSelectionStartValidating();
			}
		};
		pickerSelectionStart.NextPart += delegate
		{
			((Control)pickerSelectionEnd).Visible = true;
			((Control)pickerSelectionEnd).Focus();
			((TextBoxBase)pickerSelectionEnd).SelectionStart = 0;
			RefreshLocations();
			IsEditing = true;
		};
		((Control)this).Controls.Add((Control)(object)pickerSelectionStart);
	}

	private void CreateDateTimePickerEnd()
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		pickerSelectionEnd = new StiDateTimeInput();
		((Control)pickerSelectionEnd).Visible = false;
		((Control)pickerSelectionEnd).Text = SelectionEndStr;
		((Control)pickerSelectionEnd).GotFocus += delegate
		{
			((Control)pickerSelectionEnd).Visible = true;
			IsEditing = true;
		};
		((Control)pickerSelectionEnd).LostFocus += delegate
		{
			PickerSelectionEndValidating();
			((Control)pickerSelectionEnd).Visible = false;
			IsEditing = false;
		};
		((Control)pickerSelectionEnd).VisibleChanged += delegate
		{
			((Control)this).Invalidate();
		};
		((Control)pickerSelectionEnd).KeyDown += (KeyEventHandler)delegate(object sender, KeyEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Invalid comparison between Unknown and I4
			if ((int)e.KeyCode == 13)
			{
				PickerSelectionEndValidating();
			}
		};
		pickerSelectionEnd.PrevPart += delegate
		{
			((Control)pickerSelectionStart).Visible = true;
			((Control)pickerSelectionStart).Focus();
			RefreshLocations();
			IsEditing = true;
		};
		((Control)this).Controls.Add((Control)(object)pickerSelectionEnd);
	}

	protected override void Dispose(bool disposing)
	{
		((Control)this).Dispose(disposing);
		if (calendar != null && !((Control)calendar).IsDisposed)
		{
			((Component)(object)calendar).Dispose();
		}
		if (dateRangeControl != null && !((Control)dateRangeControl).IsDisposed)
		{
			((Component)(object)dateRangeControl).Dispose();
		}
		if (pickerSelectionStart != null && !((Control)pickerSelectionStart).IsDisposed)
		{
			((Component)(object)pickerSelectionStart).Dispose();
		}
		if (pickerSelectionEnd != null && !((Control)pickerSelectionEnd).IsDisposed)
		{
			((Component)(object)pickerSelectionEnd).Dispose();
		}
	}

	public StiDateTimePicker(StiFormatService textFormat = null, StiReport report = null)
	{
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)512, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).TabStop = true;
		TextFormat = textFormat;
		Report = report;
		CreateDateTimePickerStart();
		CreateDateTimePickerEnd();
		Colors = new StiControlColorCollection((Control)(object)this);
		((Control)this).Height = (int)(24.0 * StiScale.Factor);
		dropDownClosedTime = DateTime.Now;
		RefreshLocations();
	}
}

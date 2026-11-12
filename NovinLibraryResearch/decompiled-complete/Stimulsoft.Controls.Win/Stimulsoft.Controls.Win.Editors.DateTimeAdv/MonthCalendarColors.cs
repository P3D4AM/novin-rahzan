using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win.Editors.DateTimeAdv;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
[TypeConverter(typeof(ExpandableObjectConverter))]
public class MonthCalendarColors
{
	private BaseItem _Parent;

	private DateAppearanceDescription _Today = new DateAppearanceDescription();

	private DateAppearanceDescription _Selection = new DateAppearanceDescription();

	private DateAppearanceDescription _TrailingDay = new DateAppearanceDescription();

	private DateAppearanceDescription _WeekOfYear = new DateAppearanceDescription();

	private Color _DaysDividerBorderColors = Color.Empty;

	private DateAppearanceDescription _Day = new DateAppearanceDescription();

	private DateAppearanceDescription _Weekend = new DateAppearanceDescription();

	private DateAppearanceDescription _TrailingWeekend = new DateAppearanceDescription();

	private DateAppearanceDescription _DayLabel = new DateAppearanceDescription();

	private DateAppearanceDescription _MonthlyMarker = new DateAppearanceDescription();

	private DateAppearanceDescription _AnnualMarker = new DateAppearanceDescription();

	private DateAppearanceDescription _DayMarker = new DateAppearanceDescription();

	private DateAppearanceDescription _WeeklyMarker = new DateAppearanceDescription();

	internal BaseItem Parent
	{
		get
		{
			return _Parent;
		}
		set
		{
			_Parent = value;
			_WeekOfYear.Parent = value;
			_Day.Parent = value;
			_Weekend.Parent = value;
			_TrailingDay.Parent = value;
			_Today.Parent = value;
			_TrailingWeekend.Parent = value;
			_DayLabel.Parent = value;
			_DayMarker.Parent = value;
			_MonthlyMarker.Parent = value;
			_AnnualMarker.Parent = value;
			_Selection.Parent = value;
		}
	}

	[NotifyParentProperty(true)]
	[Description("Indicates appearance settings for the todays date.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DateAppearanceDescription Today => _Today;

	[NotifyParentProperty(true)]
	[Description("Indicates appearance settings for selected days.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DateAppearanceDescription Selection => _Selection;

	[NotifyParentProperty(true)]
	[Description("Indicates appearance settings for the trailing days on calendar.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DateAppearanceDescription TrailingDay => _TrailingDay;

	[NotifyParentProperty(true)]
	[Description("Indicates appearance settings for the labels that show week of year number.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DateAppearanceDescription WeekOfYear => _WeekOfYear;

	[Category("Colors")]
	[Description("Indicates days divider line color.")]
	public Color DaysDividerBorderColors
	{
		get
		{
			return _DaysDividerBorderColors;
		}
		set
		{
			_DaysDividerBorderColors = value;
			Refresh();
		}
	}

	[NotifyParentProperty(true)]
	[Description("Indicates appearance settings for the numeric day of month label.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DateAppearanceDescription Day => _Day;

	[NotifyParentProperty(true)]
	[Description("Indicates appearance settings for the weekend days on calendar (Saturday and Sunday).")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DateAppearanceDescription Weekend => _Weekend;

	[NotifyParentProperty(true)]
	[Description("Indicates appearance settings for the trailing weekend days on calendar (Saturday and Sunday).")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DateAppearanceDescription TrailingWeekend => _TrailingWeekend;

	[NotifyParentProperty(true)]
	[Description("Indicates appearance settings for the labels that display day name in calendar header.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DateAppearanceDescription DayLabel => _DayLabel;

	[NotifyParentProperty(true)]
	[Description("Gets marker settings for days specified by MonthCalendarAdv.MonthlyMarkedDates property.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DateAppearanceDescription MonthlyMarker => _MonthlyMarker;

	[NotifyParentProperty(true)]
	[Description("Gets marker settings for days specified by MonthCalendarAdv.AnnuallyMarkedDates property.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DateAppearanceDescription AnnualMarker => _AnnualMarker;

	[NotifyParentProperty(true)]
	[Description("Gets marker settings for days specified by MonthCalendarAdv.MarkedDates property.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DateAppearanceDescription DayMarker => _DayMarker;

	[NotifyParentProperty(true)]
	[Description("Gets marker settings for days specified by MonthCalendarAdv.WeeklyMarkedDays property.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DateAppearanceDescription WeeklyMarker => _WeeklyMarker;

	private void Refresh()
	{
		if (_Parent != null)
		{
			_Parent.NeedRecalcSize = true;
			_Parent.Refresh();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetToday()
	{
		_Today = new DateAppearanceDescription();
		_Today.Parent = _Parent;
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSelection()
	{
		_Selection = new DateAppearanceDescription();
		_Selection.Parent = _Parent;
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTrailingDay()
	{
		_TrailingDay = new DateAppearanceDescription();
		_TrailingDay.Parent = _Parent;
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetWeekOfYear()
	{
		_WeekOfYear = new DateAppearanceDescription();
		_WeekOfYear.Parent = _Parent;
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDaysDividerBorderColors()
	{
		return !DaysDividerBorderColors.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDaysDividerBorderColors()
	{
		DaysDividerBorderColors = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDay()
	{
		_Day = new DateAppearanceDescription();
		_Day.Parent = _Parent;
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetWeekend()
	{
		_Weekend = new DateAppearanceDescription();
		_Weekend.Parent = _Parent;
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTrailingWeekend()
	{
		_TrailingWeekend = new DateAppearanceDescription();
		_TrailingWeekend.Parent = _Parent;
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDayLabel()
	{
		_DayLabel = new DateAppearanceDescription();
		_DayLabel.Parent = _Parent;
		Refresh();
	}
}

using System.ComponentModel;
using System.Drawing;
using DevComponents.DotNetBar;

namespace DevComponents.Editors.DateTimeAdv;

[TypeConverter(typeof(ExpandableObjectConverter))]
[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class MonthCalendarColors
{
	private BaseItem _ᗚ;

	private DateAppearanceDescription __263E = new DateAppearanceDescription();

	private DateAppearanceDescription __263F = new DateAppearanceDescription();

	private DateAppearanceDescription __2640 = new DateAppearanceDescription();

	private DateAppearanceDescription __2641 = new DateAppearanceDescription();

	private Color __2642 = Color.Empty;

	private DateAppearanceDescription __2643 = new DateAppearanceDescription();

	private DateAppearanceDescription __2644 = new DateAppearanceDescription();

	private DateAppearanceDescription __2645 = new DateAppearanceDescription();

	private DateAppearanceDescription __2646 = new DateAppearanceDescription();

	private DateAppearanceDescription __2647 = new DateAppearanceDescription();

	private DateAppearanceDescription __2648 = new DateAppearanceDescription();

	private DateAppearanceDescription __2649 = new DateAppearanceDescription();

	private DateAppearanceDescription __264A = new DateAppearanceDescription();

	internal BaseItem ކ
	{
		get
		{
			return _ᗚ;
		}
		set
		{
			_ᗚ = value;
			__2641.ކ = value;
			__2643.ކ = value;
			__2644.ކ = value;
			__2640.ކ = value;
			__263E.ކ = value;
			__2645.ކ = value;
			__2646.ކ = value;
			__2649.ކ = value;
			__2647.ކ = value;
			__2648.ކ = value;
			__263F.ކ = value;
		}
	}

	[NotifyParentProperty(true)]
	[Description("Indicates appearance settings for the todays date.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DateAppearanceDescription Today => __263E;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Indicates appearance settings for selected days.")]
	[NotifyParentProperty(true)]
	public DateAppearanceDescription Selection => __263F;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[NotifyParentProperty(true)]
	[Description("Indicates appearance settings for the trailing days on calendar.")]
	public DateAppearanceDescription TrailingDay => __2640;

	[Description("Indicates appearance settings for the labels that show week of year number.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[NotifyParentProperty(true)]
	public DateAppearanceDescription WeekOfYear => __2641;

	[Category("Colors")]
	[Description("Indicates days divider line color.")]
	public Color DaysDividerBorderColors
	{
		get
		{
			return __2642;
		}
		set
		{
			__2642 = value;
			ᇌ();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[NotifyParentProperty(true)]
	[Description("Indicates appearance settings for the numeric day of month label.")]
	public DateAppearanceDescription Day => __2643;

	[NotifyParentProperty(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Indicates appearance settings for the weekend days on calendar (Saturday and Sunday).")]
	public DateAppearanceDescription Weekend => __2644;

	[NotifyParentProperty(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Indicates appearance settings for the trailing weekend days on calendar (Saturday and Sunday).")]
	public DateAppearanceDescription TrailingWeekend => __2645;

	[NotifyParentProperty(true)]
	[Description("Indicates appearance settings for the labels that display day name in calendar header.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DateAppearanceDescription DayLabel => __2646;

	[Description("Gets marker settings for days specified by MonthCalendarAdv.MonthlyMarkedDates property.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[NotifyParentProperty(true)]
	public DateAppearanceDescription MonthlyMarker => __2647;

	[NotifyParentProperty(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Gets marker settings for days specified by MonthCalendarAdv.AnnuallyMarkedDates property.")]
	public DateAppearanceDescription AnnualMarker => __2648;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[NotifyParentProperty(true)]
	[Description("Gets marker settings for days specified by MonthCalendarAdv.MarkedDates property.")]
	public DateAppearanceDescription DayMarker => __2649;

	[NotifyParentProperty(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Gets marker settings for days specified by MonthCalendarAdv.WeeklyMarkedDays property.")]
	public DateAppearanceDescription WeeklyMarker => __264A;

	private void ᇌ()
	{
		if (_ᗚ != null)
		{
			_ᗚ.NeedRecalcSize = true;
			_ᗚ.Refresh();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetToday()
	{
		__263E = new DateAppearanceDescription();
		__263E.ކ = _ᗚ;
		ᇌ();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSelection()
	{
		__263F = new DateAppearanceDescription();
		__263F.ކ = _ᗚ;
		ᇌ();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTrailingDay()
	{
		__2640 = new DateAppearanceDescription();
		__2640.ކ = _ᗚ;
		ᇌ();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetWeekOfYear()
	{
		__2641 = new DateAppearanceDescription();
		__2641.ކ = _ᗚ;
		ᇌ();
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
		__2643 = new DateAppearanceDescription();
		__2643.ކ = _ᗚ;
		ᇌ();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetWeekend()
	{
		__2644 = new DateAppearanceDescription();
		__2644.ކ = _ᗚ;
		ᇌ();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTrailingWeekend()
	{
		__2645 = new DateAppearanceDescription();
		__2645.ކ = _ᗚ;
		ᇌ();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDayLabel()
	{
		__2646 = new DateAppearanceDescription();
		__2646.ކ = _ᗚ;
		ᇌ();
	}
}

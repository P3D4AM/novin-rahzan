using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

public abstract class StiDialogInfoItem : IStiPropertyGridObject, ICloneable
{
	[Browsable(false)]
	public virtual StiComponentId ComponentId => StiComponentId.StiDialogInfoItem;

	[Browsable(false)]
	public string PropName => string.Empty;

	[Browsable(false)]
	public object KeyObject { get; set; }

	[Browsable(false)]
	public object KeyObjectTo { get; set; }

	[Browsable(false)]
	public List<object> ValueBinding { get; set; } = new List<object>();


	[Browsable(false)]
	public string Value { get; set; } = string.Empty;


	[Browsable(false)]
	public bool Checked { get; set; } = true;


	[StiOrder(300)]
	[Description("Gets or sets Value which will be displayed instead of the Key value in GUI.")]
	public string Label
	{
		get
		{
			return Value;
		}
		set
		{
			Value = value;
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public virtual StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		return null;
	}

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public string ToString(StiDateTimeType dateTimeType)
	{
		if (this is StiStringRangeDialogInfoItem || this is StiGuidRangeDialogInfoItem || this is StiCharRangeDialogInfoItem || this is StiTimeSpanRangeDialogInfoItem || this is StiDoubleRangeDialogInfoItem || this is StiDecimalRangeDialogInfoItem || this is StiLongRangeDialogInfoItem || this is StiExpressionRangeDialogInfoItem)
		{
			string text = ((KeyObject == null) ? StiLocalization.Get("Report", "NotAssigned") : KeyObject.ToString());
			string text2 = ((KeyObjectTo == null) ? StiLocalization.Get("Report", "NotAssigned") : KeyObjectTo.ToString());
			string text3 = ((string.IsNullOrEmpty(text) && string.IsNullOrEmpty(text2)) ? string.Empty : $"{text}-{text2}");
			if (!(this is StiExpressionRangeDialogInfoItem))
			{
				if (!string.IsNullOrEmpty(Value))
				{
					return Value;
				}
				return text3;
			}
			return $"{{{(string.IsNullOrEmpty(Value) ? text3 : Value)}}}";
		}
		if (this is StiDateTimeRangeDialogInfoItem)
		{
			StiDateTimeRangeDialogInfoItem stiDateTimeRangeDialogInfoItem = (StiDateTimeRangeDialogInfoItem)this;
			string arg = null;
			string arg2 = null;
			switch (dateTimeType)
			{
			case StiDateTimeType.DateAndTime:
				arg = stiDateTimeRangeDialogInfoItem.From.ToString();
				arg2 = stiDateTimeRangeDialogInfoItem.To.ToString();
				break;
			case StiDateTimeType.Date:
				arg = stiDateTimeRangeDialogInfoItem.From.ToShortDateString();
				arg2 = stiDateTimeRangeDialogInfoItem.To.ToShortDateString();
				break;
			case StiDateTimeType.Time:
				arg = stiDateTimeRangeDialogInfoItem.From.ToShortTimeString();
				arg2 = stiDateTimeRangeDialogInfoItem.To.ToShortTimeString();
				break;
			}
			string result = $"{arg}-{arg2}";
			if (!string.IsNullOrEmpty(Value))
			{
				return Value;
			}
			return result;
		}
		if (this is StiImageDialogInfoItem)
		{
			StiImageDialogInfoItem stiImageDialogInfoItem = this as StiImageDialogInfoItem;
			if (stiImageDialogInfoItem.Key == null)
			{
				return StiLocalization.Get("Report", "NotAssigned");
			}
			return StiLocalization.Get("Components", "StiImage");
		}
		if (this is StiDateTimeDialogInfoItem)
		{
			StiDateTimeDialogInfoItem stiDateTimeDialogInfoItem = (StiDateTimeDialogInfoItem)this;
			string result2 = null;
			switch (dateTimeType)
			{
			case StiDateTimeType.DateAndTime:
				result2 = stiDateTimeDialogInfoItem.Key.ToString();
				break;
			case StiDateTimeType.Date:
				result2 = stiDateTimeDialogInfoItem.Key.ToShortDateString();
				break;
			case StiDateTimeType.Time:
				result2 = stiDateTimeDialogInfoItem.Key.ToShortTimeString();
				break;
			}
			if (!string.IsNullOrEmpty(Value))
			{
				return Value;
			}
			return result2;
		}
		string text4 = ((KeyObject == null) ? StiLocalization.Get("Report", "NotAssigned") : KeyObject.ToString());
		if (this is StiExpressionDialogInfoItem)
		{
			return $"{{{(string.IsNullOrEmpty(Value) ? text4 : Value)}}}";
		}
		if (!string.IsNullOrEmpty(Value))
		{
			return Value;
		}
		return text4;
	}
}

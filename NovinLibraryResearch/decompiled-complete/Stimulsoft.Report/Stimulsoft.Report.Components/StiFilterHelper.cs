using System;
using System.Reflection;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

public sealed class StiFilterHelper
{
	public static StiMarkerType ConvertStringToMarkerType(string item)
	{
		if (item == StiLocalization.Get("PropertyEnum", "StiMarkerTypeCircle"))
		{
			return StiMarkerType.Circle;
		}
		if (item == StiLocalization.Get("PropertyEnum", "StiMarkerTypeHexagon"))
		{
			return StiMarkerType.Hexagon;
		}
		if (item == StiLocalization.Get("PropertyEnum", "StiMarkerTypeHalfCircle"))
		{
			return StiMarkerType.HalfCircle;
		}
		if (item == StiLocalization.Get("PropertyEnum", "StiMarkerTypeRectangle"))
		{
			return StiMarkerType.Rectangle;
		}
		if (item == StiLocalization.Get("PropertyEnum", "StiMarkerTypeStar5"))
		{
			return StiMarkerType.Star5;
		}
		if (item == StiLocalization.Get("PropertyEnum", "StiMarkerTypeStar6"))
		{
			return StiMarkerType.Star6;
		}
		if (item == StiLocalization.Get("PropertyEnum", "StiMarkerTypeStar7"))
		{
			return StiMarkerType.Star7;
		}
		if (item == StiLocalization.Get("PropertyEnum", "StiMarkerTypeStar8"))
		{
			return StiMarkerType.Star8;
		}
		if (item == StiLocalization.Get("PropertyEnum", "StiMarkerTypeTriangle"))
		{
			return StiMarkerType.Triangle;
		}
		return StiMarkerType.Circle;
	}

	public static StiFilterItem ConvertStringToItem(string item)
	{
		if (item == StiLocalization.Get("PropertyEnum", "StiFilterItemValue"))
		{
			return StiFilterItem.Value;
		}
		if (item == StiLocalization.Get("PropertyEnum", "StiFilterItemValueEnd"))
		{
			return StiFilterItem.ValueEnd;
		}
		if (item == StiLocalization.Get("PropertyEnum", "StiFilterItemArgument"))
		{
			return StiFilterItem.Argument;
		}
		if (item == StiLocalization.Get("PropertyEnum", "StiFilterItemValueOpen"))
		{
			return StiFilterItem.ValueOpen;
		}
		if (item == StiLocalization.Get("PropertyEnum", "StiFilterItemValueClose"))
		{
			return StiFilterItem.ValueClose;
		}
		if (item == StiLocalization.Get("PropertyEnum", "StiFilterItemValueLow"))
		{
			return StiFilterItem.ValueLow;
		}
		if (item == StiLocalization.Get("PropertyEnum", "StiFilterItemValueHigh"))
		{
			return StiFilterItem.ValueHigh;
		}
		return StiFilterItem.Argument;
	}

	public static string ConvertItemToString(StiFilterItem filterItem)
	{
		string result = null;
		switch (filterItem)
		{
		case StiFilterItem.Value:
			result = StiLocalization.Get("PropertyEnum", "StiFilterItemValue");
			break;
		case StiFilterItem.ValueEnd:
			result = StiLocalization.Get("PropertyEnum", "StiFilterItemValueEnd");
			break;
		case StiFilterItem.Argument:
			result = StiLocalization.Get("PropertyEnum", "StiFilterItemArgument");
			break;
		case StiFilterItem.ValueOpen:
			result = StiLocalization.Get("PropertyEnum", "StiFilterItemValueOpen");
			break;
		case StiFilterItem.ValueClose:
			result = StiLocalization.Get("PropertyEnum", "StiFilterItemValueClose");
			break;
		case StiFilterItem.ValueLow:
			result = StiLocalization.Get("PropertyEnum", "StiFilterItemValueLow");
			break;
		case StiFilterItem.ValueHigh:
			result = StiLocalization.Get("PropertyEnum", "StiFilterItemValueHigh");
			break;
		}
		return result;
	}

	public static StiFilterCondition ConvertStringToCondition(string condition)
	{
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionEqualTo"))
		{
			return StiFilterCondition.EqualTo;
		}
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionNotEqualTo"))
		{
			return StiFilterCondition.NotEqualTo;
		}
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionBetween"))
		{
			return StiFilterCondition.Between;
		}
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionNotBetween"))
		{
			return StiFilterCondition.NotBetween;
		}
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionGreaterThan"))
		{
			return StiFilterCondition.GreaterThan;
		}
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionGreaterThanOrEqualTo"))
		{
			return StiFilterCondition.GreaterThanOrEqualTo;
		}
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionLessThan"))
		{
			return StiFilterCondition.LessThan;
		}
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionLessThanOrEqualTo"))
		{
			return StiFilterCondition.LessThanOrEqualTo;
		}
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionContaining"))
		{
			return StiFilterCondition.Containing;
		}
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionNotContaining"))
		{
			return StiFilterCondition.NotContaining;
		}
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionBeginningWith"))
		{
			return StiFilterCondition.BeginningWith;
		}
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionEndingWith"))
		{
			return StiFilterCondition.EndingWith;
		}
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionIsNull"))
		{
			return StiFilterCondition.IsNull;
		}
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionIsNotNull"))
		{
			return StiFilterCondition.IsNotNull;
		}
		return StiFilterCondition.EqualTo;
	}

	public static string ConvertConditionToString(StiFilterCondition condition)
	{
		return condition switch
		{
			StiFilterCondition.EqualTo => Loc.Get("PropertyEnum", "StiFilterConditionEqualTo"), 
			StiFilterCondition.NotEqualTo => Loc.Get("PropertyEnum", "StiFilterConditionNotEqualTo"), 
			StiFilterCondition.Between => Loc.Get("PropertyEnum", "StiFilterConditionBetween"), 
			StiFilterCondition.NotBetween => Loc.Get("PropertyEnum", "StiFilterConditionNotBetween"), 
			StiFilterCondition.GreaterThan => Loc.Get("PropertyEnum", "StiFilterConditionGreaterThan"), 
			StiFilterCondition.GreaterThanOrEqualTo => Loc.Get("PropertyEnum", "StiFilterConditionGreaterThanOrEqualTo"), 
			StiFilterCondition.LessThan => Loc.Get("PropertyEnum", "StiFilterConditionLessThan"), 
			StiFilterCondition.LessThanOrEqualTo => Loc.Get("PropertyEnum", "StiFilterConditionLessThanOrEqualTo"), 
			StiFilterCondition.Containing => Loc.Get("PropertyEnum", "StiFilterConditionContaining"), 
			StiFilterCondition.NotContaining => Loc.Get("PropertyEnum", "StiFilterConditionNotContaining"), 
			StiFilterCondition.BeginningWith => Loc.Get("PropertyEnum", "StiFilterConditionBeginningWith"), 
			StiFilterCondition.EndingWith => Loc.Get("PropertyEnum", "StiFilterConditionEndingWith"), 
			StiFilterCondition.IsNull => Loc.Get("PropertyEnum", "StiFilterConditionIsNull"), 
			StiFilterCondition.IsNotNull => Loc.Get("PropertyEnum", "StiFilterConditionIsNotNull"), 
			_ => string.Empty, 
		};
	}

	public static StiFilterDataType ConvertStringToDataType(string dataType)
	{
		if (dataType == Loc.Get("PropertyEnum", "StiFilterDataTypeString"))
		{
			return StiFilterDataType.String;
		}
		if (dataType == Loc.Get("PropertyEnum", "StiFilterDataTypeNumeric"))
		{
			return StiFilterDataType.Numeric;
		}
		if (dataType == Loc.Get("PropertyEnum", "StiFilterDataTypeDateTime"))
		{
			return StiFilterDataType.DateTime;
		}
		if (dataType == Loc.Get("PropertyEnum", "StiFilterDataTypeBoolean"))
		{
			return StiFilterDataType.Boolean;
		}
		if (dataType == Loc.Get("PropertyEnum", "StiFilterDataTypeExpression"))
		{
			return StiFilterDataType.Expression;
		}
		return StiFilterDataType.String;
	}

	public static string ConvertDataTypeToString(StiFilterDataType dataType)
	{
		return dataType switch
		{
			StiFilterDataType.String => Loc.Get("PropertyEnum", "StiFilterDataTypeString"), 
			StiFilterDataType.Numeric => Loc.Get("PropertyEnum", "StiFilterDataTypeNumeric"), 
			StiFilterDataType.DateTime => Loc.Get("PropertyEnum", "StiFilterDataTypeDateTime"), 
			StiFilterDataType.Boolean => Loc.Get("PropertyEnum", "StiFilterDataTypeBoolean"), 
			StiFilterDataType.Expression => Loc.Get("PropertyEnum", "StiFilterDataTypeExpression"), 
			_ => string.Empty, 
		};
	}

	public static void SetFilter(StiComponent comp)
	{
		IStiFilter stiFilter = comp as IStiFilter;
		IStiDataSource stiDataSource = comp as IStiDataSource;
		IStiBusinessObject stiBusinessObject = comp as IStiBusinessObject;
		if ((stiDataSource.IsDataSourceEmpty && stiBusinessObject.IsBusinessObjectEmpty) || stiFilter.FilterMethodHandler != null || !stiFilter.FilterOn)
		{
			return;
		}
		string text = StiNameValidator.CorrectName(comp.Name, comp.Report);
		Type type = comp.Report.GetType();
		MethodInfo method = type.GetMethod(text + "__GetFilter", new Type[2]
		{
			typeof(object),
			typeof(StiFilterEventArgs)
		});
		if (method == null)
		{
			return;
		}
		try
		{
			stiFilter.FilterMethodHandler = Delegate.CreateDelegate(typeof(StiFilterEventHandler), comp.Report, text + "__GetFilter") as StiFilterEventHandler;
		}
		catch (Exception e)
		{
			StiLogService.Write(((object)comp).GetType(), "StiFilterEventHandler...ERROR");
			StiLogService.Write(((object)comp).GetType(), e);
		}
	}
}

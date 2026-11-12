using Stimulsoft.Base.Localization;

namespace Stimulsoft.Data.Engine;

public sealed class StiDataFilterHelper
{
	public static StiDataFilterCondition ConvertStringToCondition(string condition)
	{
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionEqualTo"))
		{
			return StiDataFilterCondition.EqualTo;
		}
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionNotEqualTo"))
		{
			return StiDataFilterCondition.NotEqualTo;
		}
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionBetween"))
		{
			return StiDataFilterCondition.Between;
		}
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionNotBetween"))
		{
			return StiDataFilterCondition.NotBetween;
		}
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionGreaterThan"))
		{
			return StiDataFilterCondition.GreaterThan;
		}
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionGreaterThanOrEqualTo"))
		{
			return StiDataFilterCondition.GreaterThanOrEqualTo;
		}
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionLessThan"))
		{
			return StiDataFilterCondition.LessThan;
		}
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionLessThanOrEqualTo"))
		{
			return StiDataFilterCondition.LessThanOrEqualTo;
		}
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionContaining"))
		{
			return StiDataFilterCondition.Containing;
		}
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionNotContaining"))
		{
			return StiDataFilterCondition.NotContaining;
		}
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionBeginningWith"))
		{
			return StiDataFilterCondition.BeginningWith;
		}
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionEndingWith"))
		{
			return StiDataFilterCondition.EndingWith;
		}
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionIsNull"))
		{
			return StiDataFilterCondition.IsNull;
		}
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionIsNotNull"))
		{
			return StiDataFilterCondition.IsNotNull;
		}
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionIsBlank"))
		{
			return StiDataFilterCondition.IsBlank;
		}
		if (condition == Loc.Get("PropertyEnum", "StiFilterConditionIsNotBlank"))
		{
			return StiDataFilterCondition.IsNotBlank;
		}
		return StiDataFilterCondition.EqualTo;
	}

	public static string ConvertConditionToString(StiDataFilterCondition condition)
	{
		return condition switch
		{
			StiDataFilterCondition.EqualTo => Loc.Get("PropertyEnum", "StiFilterConditionEqualTo"), 
			StiDataFilterCondition.NotEqualTo => Loc.Get("PropertyEnum", "StiFilterConditionNotEqualTo"), 
			StiDataFilterCondition.Between => Loc.Get("PropertyEnum", "StiFilterConditionBetween"), 
			StiDataFilterCondition.NotBetween => Loc.Get("PropertyEnum", "StiFilterConditionNotBetween"), 
			StiDataFilterCondition.GreaterThan => Loc.Get("PropertyEnum", "StiFilterConditionGreaterThan"), 
			StiDataFilterCondition.GreaterThanOrEqualTo => Loc.Get("PropertyEnum", "StiFilterConditionGreaterThanOrEqualTo"), 
			StiDataFilterCondition.LessThan => Loc.Get("PropertyEnum", "StiFilterConditionLessThan"), 
			StiDataFilterCondition.LessThanOrEqualTo => Loc.Get("PropertyEnum", "StiFilterConditionLessThanOrEqualTo"), 
			StiDataFilterCondition.Containing => Loc.Get("PropertyEnum", "StiFilterConditionContaining"), 
			StiDataFilterCondition.NotContaining => Loc.Get("PropertyEnum", "StiFilterConditionNotContaining"), 
			StiDataFilterCondition.BeginningWith => Loc.Get("PropertyEnum", "StiFilterConditionBeginningWith"), 
			StiDataFilterCondition.EndingWith => Loc.Get("PropertyEnum", "StiFilterConditionEndingWith"), 
			StiDataFilterCondition.IsNull => Loc.Get("PropertyEnum", "StiFilterConditionIsNull"), 
			StiDataFilterCondition.IsNotNull => Loc.Get("PropertyEnum", "StiFilterConditionIsNotNull"), 
			StiDataFilterCondition.IsBlank => Loc.Get("PropertyEnum", "StiFilterConditionIsBlank"), 
			StiDataFilterCondition.IsNotBlank => Loc.Get("PropertyEnum", "StiFilterConditionIsNotBlank"), 
			_ => string.Empty, 
		};
	}
}

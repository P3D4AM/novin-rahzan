using Stimulsoft.Report.Components.Table;

namespace Stimulsoft.Report.Components;

public sealed class StiRestrictionsHelper
{
	public static bool IsAllowChange(StiComponent comp)
	{
		if (comp != null && (comp.Restrictions & StiRestrictions.AllowChange) <= StiRestrictions.None)
		{
			return StiOptions.Designer.Restrictions.IgnoreAllowChange;
		}
		return true;
	}

	public static bool IsAllowDelete(StiRestrictions restrictions)
	{
		if ((restrictions & StiRestrictions.AllowDelete) <= StiRestrictions.None)
		{
			return StiOptions.Designer.Restrictions.IgnoreAllowDelete;
		}
		return true;
	}

	public static bool IsAllowDelete(StiComponent comp)
	{
		if (comp != null && (comp.Restrictions & StiRestrictions.AllowDelete) <= StiRestrictions.None)
		{
			return StiOptions.Designer.Restrictions.IgnoreAllowDelete;
		}
		return true;
	}

	public static bool IsAllowMove(StiComponent comp)
	{
		if (comp != null && (comp.Restrictions & StiRestrictions.AllowMove) <= StiRestrictions.None)
		{
			return StiOptions.Designer.Restrictions.IgnoreAllowMove;
		}
		return true;
	}

	public static bool IsAllowSelect(StiComponent comp)
	{
		if (comp != null && (comp.Restrictions & StiRestrictions.AllowSelect) <= StiRestrictions.None)
		{
			return StiOptions.Designer.Restrictions.IgnoreAllowSelect;
		}
		return true;
	}

	public static bool IsAllowResize(StiComponent comp)
	{
		if (comp != null && !(comp is IStiTableCell) && (comp.Restrictions & StiRestrictions.AllowResize) <= StiRestrictions.None)
		{
			return StiOptions.Designer.Restrictions.IgnoreAllowResize;
		}
		return true;
	}

	public static bool IsAllowChangePosition(StiComponent comp)
	{
		if (!IsAllowMove(comp))
		{
			return IsAllowResize(comp);
		}
		return true;
	}
}

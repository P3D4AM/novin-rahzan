using System.Collections;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;

namespace Stimulsoft.Report.Design;

public class StiInplaceEditorTitleElement
{
	public static int ButtonSize = 20;

	public static int ButtonMargin = 4;

	private static Hashtable isTitleFocusedHash = new Hashtable();

	private static Hashtable isSortFocusedHash = new Hashtable();

	private static Hashtable isViewStateFocusedHash = new Hashtable();

	private static Hashtable isComponentFocusedHash = new Hashtable();

	private static void CheckComponentGuid(StiComponent component)
	{
		component.Guid = StiKeyHelper.GetOrGeneratedKey(component.Guid);
	}

	public static bool IsComponentFocused(StiComponent component)
	{
		CheckComponentGuid(component);
		return isComponentFocusedHash[component.Guid] != null;
	}

	public static void SetComponentFocused(StiComponent component)
	{
		CheckComponentGuid(component);
		isComponentFocusedHash[component.Guid] = component.Guid;
	}

	public static void ResetComponentFocused(StiComponent component)
	{
		CheckComponentGuid(component);
		if (isComponentFocusedHash[component.Guid] != null)
		{
			isComponentFocusedHash.Remove(component.Guid);
		}
	}

	public static bool IsTitleFocused(StiComponent component)
	{
		CheckComponentGuid(component);
		return isTitleFocusedHash[component.Guid] != null;
	}

	public static void SetTitleFocused(StiComponent component)
	{
		CheckComponentGuid(component);
		isTitleFocusedHash[component.Guid] = component.Guid;
	}

	public static void ResetTitleFocused(StiComponent component)
	{
		CheckComponentGuid(component);
		if (isTitleFocusedHash[component.Guid] != null)
		{
			isTitleFocusedHash.Remove(component.Guid);
		}
	}

	public static bool IsSortFocused(StiComponent component)
	{
		CheckComponentGuid(component);
		return isSortFocusedHash[component.Guid] != null;
	}

	public static void SetSortFocused(StiComponent component)
	{
		CheckComponentGuid(component);
		isSortFocusedHash[component.Guid] = component.Guid;
	}

	public static void ResetSortFocused(StiComponent component)
	{
		CheckComponentGuid(component);
		if (isSortFocusedHash[component.Guid] != null)
		{
			isSortFocusedHash.Remove(component.Guid);
		}
	}

	public static bool IsViewStateFocused(StiComponent component, int index)
	{
		CheckComponentGuid(component);
		return isViewStateFocusedHash[$"{component.Guid}.{index}"] != null;
	}

	public static void SetViewStateFocused(StiComponent component, int index)
	{
		CheckComponentGuid(component);
		isViewStateFocusedHash[$"{component.Guid}.{index}"] = component.Guid;
	}

	public static void ResetViewStateFocused(StiComponent component)
	{
		CheckComponentGuid(component);
		for (int i = 0; i < 5; i++)
		{
			string key = $"{component.Guid}.{i}";
			if (isViewStateFocusedHash[key] != null)
			{
				isViewStateFocusedHash.Remove(key);
			}
		}
	}

	public static RectangleD GetTitleRect(RectangleD rect, StiComponent comp)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		rect = StiBorderElementHelper.GetBorderContentRect(rect, comp as IStiElement);
		return GetSimpleTitleRect(rect, comp);
	}

	public static RectangleD GetSimpleTitleRect(RectangleD rect, StiComponent comp)
	{
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		int num = StiScale.XXI((double)ButtonSize);
		int num2 = StiScale.XXI((double)ButtonMargin);
		double zoom = comp.Report.Info.Zoom;
		RectangleD result = default(RectangleD);
		((RectangleD)(ref result))._002Ector(((RectangleD)(ref rect)).Right - (double)num2 - (double)num, ((RectangleD)(ref rect)).Top + (double)num2, (double)num, (double)num);
		StiCornerRadius val = (comp as IStiCornerRadius)?.CornerRadius;
		if (val != null && val.TopRight > 0f)
		{
			((RectangleD)(ref result)).X = ((RectangleD)(ref result)).X - (double)StiScale.I((double)val.TopRight * zoom);
		}
		if (((RectangleD)(ref result)).X < ((RectangleD)(ref rect)).X)
		{
			return RectangleD.Empty;
		}
		return result;
	}

	public static RectangleD GetSortRect(RectangleD rect, StiComponent comp)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		rect = StiBorderElementHelper.GetBorderContentRect(rect, comp as IStiElement);
		return GetSimpleSortRect(rect, comp);
	}

	public static RectangleD GetSimpleSortRect(RectangleD rect, StiComponent comp)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		RectangleD simpleTitleRect = GetSimpleTitleRect(rect, comp);
		if (((RectangleD)(ref simpleTitleRect)).IsEmpty)
		{
			return RectangleD.Empty;
		}
		((RectangleD)(ref simpleTitleRect)).X = ((RectangleD)(ref simpleTitleRect)).X - (((RectangleD)(ref simpleTitleRect)).Width + (double)StiScale.I4);
		if (((RectangleD)(ref simpleTitleRect)).X < ((RectangleD)(ref rect)).X)
		{
			return RectangleD.Empty;
		}
		return simpleTitleRect;
	}

	public static RectangleD GetViewStateRect(RectangleD rect, StiComponent comp, int index)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		bool allowUserSorting = StiSortMenuHelper.IsAllowUserSorting(comp as IStiElement);
		rect = StiBorderElementHelper.GetBorderContentRect(rect, comp as IStiElement);
		return GetSimpleViewStateRect(rect, comp, allowUserSorting, index);
	}

	public static RectangleD GetSimpleViewStateRect(RectangleD rect, StiComponent comp, bool allowUserSorting, int index)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		RectangleD result = (allowUserSorting ? GetSimpleSortRect(rect, comp) : GetSimpleTitleRect(rect, comp));
		if (((RectangleD)(ref result)).IsEmpty)
		{
			return RectangleD.Empty;
		}
		((RectangleD)(ref result)).X = ((RectangleD)(ref result)).X - (((RectangleD)(ref result)).Width + (double)StiScale.I4) * (double)(index + 1);
		if (((RectangleD)(ref result)).X < ((RectangleD)(ref rect)).X)
		{
			return RectangleD.Empty;
		}
		return result;
	}

	public static void InvokeTitleClick(StiComponent component)
	{
		if (component is IStiTitleElement stiTitleElement && StiRestrictionsHelper.IsAllowChange(component))
		{
			stiTitleElement.Title.Visible = !stiTitleElement.Title.Visible;
		}
	}
}

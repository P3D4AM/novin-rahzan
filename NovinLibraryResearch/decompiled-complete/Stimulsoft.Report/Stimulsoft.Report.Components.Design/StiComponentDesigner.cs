using System;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report.Components.Design;

public class StiComponentDesigner : IStiComponentDesigner
{
	public bool FirstRun { get; set; }

	public IStiDesignerBase Designer { get; set; }

	public virtual void OnDoubleClick(StiComponent sender)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if (StiOptions.Designer.IsComponentEditorEnabled)
		{
			Design(sender);
		}
	}

	public virtual StiAction GetActionFromPoint(double x, double y, StiComponent component)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return GetActionFromPoint(x, y, component, forGetDesigner: false);
	}

	public virtual StiAction GetActionFromPoint(double x, double y, StiComponent component, bool forGetDesigner = false)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Invalid comparison between Unknown and I4
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Invalid comparison between Unknown and I4
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		double num = component.Page.Unit.ConvertFromHInches(4.0);
		bool flag = component.Locked || component.Inherited || !StiRestrictionsHelper.IsAllowChangePosition(component);
		StiAction val = StiActionUtils.PointInRect(num, x, y, component.ClientRectangle, component.SelectRectangle, component.IsSelected, flag);
		if ((int)val == 1)
		{
			if (!StiRestrictionsHelper.IsAllowMove(component) && !forGetDesigner)
			{
				return (StiAction)0;
			}
			return val;
		}
		if ((int)val == 2)
		{
			if (!StiRestrictionsHelper.IsAllowSelect(component))
			{
				return (StiAction)0;
			}
			return val;
		}
		if (!StiRestrictionsHelper.IsAllowResize(component))
		{
			return (StiAction)0;
		}
		return val;
	}

	public virtual StiAction PointInRect(IStiDesignerBase designer, StiComponent comp, int x, int y)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Invalid comparison between Unknown and I4
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Invalid comparison between Unknown and I4
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		double num = comp.Page.Unit.ConvertFromHInches(4.0);
		StiAction val = StiActionUtils.PointInRect(num, designer.XToPage(x), designer.YToPage(y), comp.ComponentToPage(comp.ClientRectangle), comp.ComponentToPage(comp.SelectRectangle), comp.IsSelected, !(comp is IStiTableCell) && (comp.Locked || comp.Inherited || !StiRestrictionsHelper.IsAllowChangePosition(comp)));
		if ((int)val == 1)
		{
			if (!StiRestrictionsHelper.IsAllowMove(comp))
			{
				return (StiAction)0;
			}
			return val;
		}
		if ((int)val == 2)
		{
			if (!StiRestrictionsHelper.IsAllowSelect(comp))
			{
				return (StiAction)0;
			}
			return val;
		}
		if (!StiRestrictionsHelper.IsAllowResize(comp))
		{
			return (StiAction)0;
		}
		return val;
	}

	public virtual DialogResult Design(StiComponent component)
	{
		return (DialogResult)0;
	}

	public virtual StiComponent CreateComponent(Type componentType, RectangleD region)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		return StiActivator.CreateObject(componentType, new object[1] { region }) as StiComponent;
	}

	public static StiComponentDesigner GetComponentDesigner(IStiDesignerBase designer, Type componentType)
	{
		return GetComponentDesigner(designer, componentType, (StiGuiMode)0);
	}

	public static StiComponentDesigner GetComponentDesigner(IStiDesignerBase designer, Type componentType, StiGuiMode guiMode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Invalid comparison between Unknown and I4
		if ((int)guiMode != 0)
		{
			if ((int)guiMode == 1)
			{
				if (!(componentType.GetCustomAttributes(typeof(StiWpfDesignerAttribute), inherit: true) is StiWpfDesignerAttribute[] array) || array.Length == 0)
				{
					return null;
				}
				string designerTypeName = array[0].DesignerTypeName;
				Type typeFromName = GetTypeFromName(designerTypeName);
				if (typeFromName == null)
				{
					return null;
				}
				return StiActivator.CreateObject(typeFromName, new object[1] { designer }) as StiComponentDesigner;
			}
			if (!(componentType.GetCustomAttributes(typeof(StiWpfDesignerAttribute), inherit: true) is StiWpfDesignerAttribute[] array2) || array2.Length == 0)
			{
				return null;
			}
			string text = array2[0].DesignerTypeName;
			if (!text.Contains("StiWpfComponentDesigner"))
			{
				text = text.Replace("Stimulsoft.Report.WpfDesign", "Stimulsoft.Client.Designer");
				text = text.Replace("StiWpf", "StiCloud");
			}
			Type typeFromName2 = GetTypeFromName(text);
			if (typeFromName2 == null)
			{
				return null;
			}
			return StiActivator.CreateObject(typeFromName2, new object[1] { designer }) as StiComponentDesigner;
		}
		if (!(componentType.GetCustomAttributes(typeof(StiDesignerAttribute), inherit: true) is StiDesignerAttribute[] array3) || array3.Length == 0)
		{
			return null;
		}
		string designerTypeName2 = array3[0].DesignerTypeName;
		Type typeFromName3 = GetTypeFromName(designerTypeName2);
		if (typeFromName3 == null)
		{
			return null;
		}
		return StiActivator.CreateObject(typeFromName3, new object[1] { designer }) as StiComponentDesigner;
	}

	internal static Type GetTypeFromName(string typeName)
	{
		if (!string.IsNullOrEmpty(typeName))
		{
			return Type.GetType(typeName);
		}
		return null;
	}

	public StiComponentDesigner(IStiDesignerBase designer)
	{
		Designer = designer;
	}
}

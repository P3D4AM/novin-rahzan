using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiConversionContainerInPanelAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "Convert");

	public override string Description => StiLocalizationExt.Get("CheckActions", "Convert");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (!(element is StiContainer stiContainer))
		{
			return;
		}
		StiPanel stiPanel = new StiPanel();
		stiPanel.Parent = stiContainer.Parent;
		stiPanel.Page = stiContainer.Page;
		stiPanel.SetPaintRectangle(stiContainer.GetPaintRectangle());
		stiPanel.ClientRectangle = stiContainer.ClientRectangle;
		stiPanel.MinSize = stiContainer.MinSize;
		stiPanel.MaxSize = stiContainer.MaxSize;
		stiPanel.Border = stiContainer.Border;
		stiPanel.Brush = stiContainer.Brush;
		stiPanel.Conditions = stiContainer.Conditions;
		stiPanel.ComponentStyle = stiContainer.ComponentStyle;
		stiPanel.UseParentStyles = stiContainer.UseParentStyles;
		stiPanel.CanGrow = stiContainer.CanGrow;
		stiPanel.CanShrink = stiContainer.CanShrink;
		stiPanel.GrowToHeight = stiContainer.GrowToHeight;
		stiPanel.DockStyle = stiContainer.DockStyle;
		stiPanel.Enabled = stiContainer.Enabled;
		stiPanel.Interaction = stiContainer.Interaction;
		stiPanel.Printable = stiContainer.Printable;
		stiPanel.PrintOn = stiContainer.PrintOn;
		stiPanel.ShiftMode = stiContainer.ShiftMode;
		stiPanel.Name = stiContainer.Name;
		stiPanel.Alias = stiContainer.Alias;
		stiPanel.Restrictions = stiContainer.Restrictions;
		stiPanel.Locked = stiContainer.Locked;
		stiPanel.Linked = stiContainer.Linked;
		foreach (StiComponent component in stiContainer.Components)
		{
			component.Parent = stiPanel;
			stiPanel.Components.Add(component);
		}
		StiContainer parent = stiContainer.Parent;
		if (parent != null)
		{
			int num = parent.Components.IndexOf(stiContainer);
			if (num != -1)
			{
				parent.Components.RemoveAt(num);
				parent.Components.Insert(num, stiPanel);
			}
		}
		StiPage page = stiPanel.Page;
		if (page == null)
		{
			return;
		}
		foreach (StiComponent component2 in page.GetComponents())
		{
			if (component2 is StiClone stiClone && stiClone.Container == stiContainer)
			{
				stiClone.Container = stiPanel;
			}
		}
	}
}

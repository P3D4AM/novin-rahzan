using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiCloneV2Builder : StiContainerV2Builder
{
	public override StiComponent InternalRender(StiComponent masterComp)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		StiClone stiClone = masterComp as StiClone;
		StiContainer container = stiClone.Container;
		if (IsParentClonation(stiClone))
		{
			return null;
		}
		while (container is StiClone)
		{
			container = ((StiClone)container).Container;
		}
		if (container == null)
		{
			return base.InternalRender((StiComponent)stiClone);
		}
		StiContainer stiContainer = container.Render() as StiContainer;
		object obj = stiClone.Border.Clone();
		stiContainer.Border = (StiBorder)((obj is StiBorder) ? obj : null);
		object obj2 = stiClone.Brush.Clone();
		stiContainer.Brush = (StiBrush)((obj2 is StiBrush) ? obj2 : null);
		stiContainer.ClientRectangle = stiClone.ClientRectangle;
		return stiContainer;
	}

	private bool IsParentClonation(StiClone clone)
	{
		if (clone.Container == null)
		{
			return false;
		}
		StiContainer parent = clone.Parent;
		while (parent != null && parent != clone.Page)
		{
			if (parent == clone.Container)
			{
				return true;
			}
			parent = parent.Parent;
		}
		return false;
	}
}

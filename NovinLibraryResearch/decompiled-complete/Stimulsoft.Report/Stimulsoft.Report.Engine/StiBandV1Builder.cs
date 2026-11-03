using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiBandV1Builder : StiContainerV1Builder
{
	public static StiComponentsCollection GetChildBands(StiBand masterBand)
	{
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		if (masterBand.Parent == null)
		{
			return stiComponentsCollection;
		}
		for (int i = masterBand.Parent.Components.IndexOf(masterBand) + 1; i < masterBand.Parent.Components.Count && masterBand.Parent.Components[i] is StiChildBand; i++)
		{
			stiComponentsCollection.Add(masterBand.Parent.Components[i]);
		}
		return stiComponentsCollection;
	}

	private static bool AllowBreak(StiBand masterBand)
	{
		if (masterBand is StiDataBand || masterBand is StiHeaderBand || masterBand is StiFooterBand || masterBand is StiGroupHeaderBand)
		{
			return masterBand.CanBreak;
		}
		return false;
	}

	public static bool RenderChilds(StiBand masterBand, StiContainer outContainer)
	{
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		bool result = true;
		StiComponentsCollection childBands = GetChildBands(masterBand);
		foreach (StiBand item in childBands)
		{
			double height = outContainer.Height;
			StiComponent renderedComponent = null;
			if (!item.Render(ref renderedComponent, outContainer))
			{
				result = false;
				break;
			}
			if (renderedComponent == null)
			{
				continue;
			}
			outContainer.Height += renderedComponent.Height;
			renderedComponent.DockStyle = StiDockStyle.None;
			renderedComponent.Top = height;
			if (!(renderedComponent is StiContainer stiContainer) || !AllowBreak(masterBand) || !stiContainer.Border.IsDefault || (!(stiContainer.Brush is StiEmptyBrush) && (!(stiContainer.Brush is StiSolidBrush) || !(((StiSolidBrush)stiContainer.Brush).Color == Color.Transparent))))
			{
				continue;
			}
			foreach (StiComponent component in stiContainer.Components)
			{
				outContainer.Components.Add(component);
				component.Left += stiContainer.Left;
				component.Top += stiContainer.Top;
				component.Parent = outContainer;
			}
			outContainer.Components.Remove(stiContainer);
		}
		return result;
	}

	public override bool InternalRender(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		StiBand stiBand = masterComp as StiBand;
		if (!stiBand.IsCross)
		{
			foreach (StiComponent component in stiBand.Components)
			{
				if (component.IsCross)
				{
					component.Prepare();
				}
				if (component is StiContainer && !(component is StiBand))
				{
					component.Prepare();
				}
			}
		}
		return base.InternalRender((StiComponent)stiBand, ref renderedComponent, outContainer);
	}
}

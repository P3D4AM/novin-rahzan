using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiViewV1Builder : StiComponentV1Builder
{
	public override bool InternalRender(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		StiView stiView = masterComp as StiView;
		StiView stiView2 = stiView.Clone(cloneProperties: false) as StiView;
		if (StiOptions.Engine.ImageCache.Enabled)
		{
			stiView2.ResetImageToDraw();
		}
		if (!stiView2.ExistImageToDraw())
		{
			stiView2.PutImageToDraw(stiView.GetImageFromSource());
		}
		outContainer.Components.Add(stiView2);
		renderedComponent = stiView2;
		renderedComponent.InvokeEvents();
		return true;
	}
}

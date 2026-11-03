using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiViewV2Builder : StiComponentV2Builder
{
	public override StiComponent InternalRender(StiComponent masterComp)
	{
		StiView stiView = masterComp as StiView;
		StiView stiView2 = (StiView)stiView.Clone(cloneProperties: false);
		if (StiOptions.Engine.ImageCache.Enabled && (!(stiView is StiImage stiImage) || !stiImage.ExistImage()))
		{
			stiView2.ResetImageToDraw();
			stiView2.Guid = null;
		}
		if (!stiView2.ExistImageToDraw())
		{
			stiView2.PutImageToDraw(stiView.GetImageFromSource());
		}
		return stiView2;
	}
}

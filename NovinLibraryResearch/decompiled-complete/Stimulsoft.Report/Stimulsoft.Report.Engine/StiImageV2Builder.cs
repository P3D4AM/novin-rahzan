using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiImageV2Builder : StiViewV2Builder
{
	public override StiComponent InternalRender(StiComponent masterComp)
	{
		StiImage masterComp2 = masterComp as StiImage;
		return base.InternalRender((StiComponent)masterComp2) as StiImage;
	}
}

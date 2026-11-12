using Stimulsoft.Report.Components;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiPanelInEngineV1Check : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiPanelInEngineV1CheckShort"), ElementName);

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiPanelInEngineV1CheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		StiComponent stiComponent = obj as StiComponent;
		if (obj.GetType() == typeof(StiPanel) && stiComponent.Report.EngineVersion == StiEngineVersion.EngineV1)
		{
			StiPanelInEngineV1Check stiPanelInEngineV1Check = new StiPanelInEngineV1Check();
			stiPanelInEngineV1Check.Element = obj;
			return stiPanelInEngineV1Check;
		}
		return null;
	}
}

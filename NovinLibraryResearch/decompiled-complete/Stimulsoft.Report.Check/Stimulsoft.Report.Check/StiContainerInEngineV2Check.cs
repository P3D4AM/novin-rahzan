using Stimulsoft.Report.Components;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiContainerInEngineV2Check : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiContainerInEngineV2CheckShort"), ElementName);

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiContainerInEngineV2CheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Information;

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		StiComponent stiComponent = obj as StiComponent;
		if (obj.GetType() == typeof(StiContainer) && stiComponent.Report.EngineVersion == StiEngineVersion.EngineV2)
		{
			StiContainerInEngineV2Check stiContainerInEngineV2Check = new StiContainerInEngineV2Check();
			stiContainerInEngineV2Check.Element = obj;
			stiContainerInEngineV2Check.Actions.Add(new StiConversionContainerInPanelAction());
			return stiContainerInEngineV2Check;
		}
		return null;
	}
}

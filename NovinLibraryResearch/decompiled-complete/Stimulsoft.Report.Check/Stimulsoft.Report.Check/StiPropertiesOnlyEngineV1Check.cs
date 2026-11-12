using Stimulsoft.Report.Components;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiPropertiesOnlyEngineV1Check : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiPropertiesOnlyEngineV1CheckShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiPropertiesOnlyEngineV1CheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Information;

	private bool Check()
	{
		if (base.Element is IStiStartNewPage && ((IStiStartNewPage)base.Element).StartNewPage)
		{
			return true;
		}
		return false;
	}

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		try
		{
			if (report.EngineVersion == StiEngineVersion.EngineV2 && Check())
			{
				StiPropertiesOnlyEngineV1Check stiPropertiesOnlyEngineV1Check = new StiPropertiesOnlyEngineV1Check();
				stiPropertiesOnlyEngineV1Check.Element = obj;
				stiPropertiesOnlyEngineV1Check.Actions.Add(new StiPropertiesOnlyEngineV1Action());
				return stiPropertiesOnlyEngineV1Check;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}

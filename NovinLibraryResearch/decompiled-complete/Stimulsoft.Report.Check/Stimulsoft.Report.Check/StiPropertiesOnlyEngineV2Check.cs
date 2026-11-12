using Stimulsoft.Report.Components;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiPropertiesOnlyEngineV2Check : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiPropertiesOnlyEngineV2CheckShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiPropertiesOnlyEngineV2CheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Information;

	private bool Check()
	{
		if (base.Element is IStiPageBreak && (((IStiPageBreak)base.Element).NewPageBefore || ((IStiPageBreak)base.Element).NewPageAfter || ((IStiPageBreak)base.Element).NewColumnBefore || ((IStiPageBreak)base.Element).NewColumnAfter || !((IStiPageBreak)base.Element).SkipFirst))
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
			if (report.EngineVersion == StiEngineVersion.EngineV1 && Check())
			{
				StiPropertiesOnlyEngineV2Check stiPropertiesOnlyEngineV2Check = new StiPropertiesOnlyEngineV2Check();
				stiPropertiesOnlyEngineV2Check.Element = obj;
				stiPropertiesOnlyEngineV2Check.Actions.Add(new StiPropertiesOnlyEngineV2Action());
				return stiPropertiesOnlyEngineV2Check;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}

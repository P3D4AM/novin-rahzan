using Stimulsoft.Report.Components;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiPrintOnDoublePassCheck : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiPrintOnDoublePassCheckShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiPrintOnDoublePassCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	private bool Check()
	{
		StiComponent stiComponent = base.Element as StiComponent;
		if (stiComponent.Report != null && stiComponent.Report.NumberOfPass != StiNumberOfPass.DoublePass && stiComponent != null && ((IStiPrintOn)stiComponent).PrintOn != 0 && ((stiComponent.Report.EngineVersion == StiEngineVersion.EngineV1 && StiOptions.Engine.UseAdvancedPrintOnEngineV1) || (stiComponent.Report.EngineVersion == StiEngineVersion.EngineV2 && StiOptions.Engine.UseAdvancedPrintOnEngineV2)))
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
			if (Check())
			{
				StiPrintOnDoublePassCheck stiPrintOnDoublePassCheck = new StiPrintOnDoublePassCheck();
				stiPrintOnDoublePassCheck.Element = obj;
				stiPrintOnDoublePassCheck.Actions.Add(new StiAllowDoublePassAction());
				return stiPrintOnDoublePassCheck;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}

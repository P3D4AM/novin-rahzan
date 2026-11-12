using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiNoNameComponentCheck : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiNoNameComponentCheckShort"), ElementName);

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiNoNameComponentCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Error;

	public override object ProcessCheck(StiReport report, object obj)
	{
		StiComponent stiComponent = obj as StiComponent;
		bool flag = false;
		if (string.IsNullOrEmpty(stiComponent.Name))
		{
			flag = true;
		}
		if (flag)
		{
			StiNoNameComponentCheck stiNoNameComponentCheck = new StiNoNameComponentCheck();
			stiNoNameComponentCheck.Element = obj;
			stiNoNameComponentCheck.Actions.Add(new StiGenerateNewNameComponentAction());
			stiNoNameComponentCheck.Actions.Add(new StiDeleteComponentAction());
			return stiNoNameComponentCheck;
		}
		return null;
	}
}

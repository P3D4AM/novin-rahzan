using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiWidthHeightZeroComponentCheck : StiComponentCheck
{
	public override string ShortMessage
	{
		get
		{
			StiComponent stiComponent = base.Element as StiComponent;
			if (stiComponent.Width == 0.0 && stiComponent.Height == 0.0)
			{
				return string.Format(StiLocalizationExt.Get("CheckComponent", "StiWidthHeightZeroComponentCheckShortWidthHeight"), ElementName);
			}
			if (stiComponent.Width == 0.0)
			{
				return string.Format(StiLocalizationExt.Get("CheckComponent", "StiWidthHeightZeroComponentCheckShortWidth"), ElementName);
			}
			return string.Format(StiLocalizationExt.Get("CheckComponent", "StiWidthHeightZeroComponentCheckShortHeight"), ElementName);
		}
	}

	public override string LongMessage
	{
		get
		{
			if (base.Element is StiComponent stiComponent)
			{
				if (stiComponent.Width == 0.0 && stiComponent.Height == 0.0)
				{
					return string.Format(StiLocalizationExt.Get("CheckComponent", "StiWidthHeightZeroComponentCheckLongWidthHeight"), ElementName);
				}
				if (stiComponent.Width == 0.0)
				{
					return string.Format(StiLocalizationExt.Get("CheckComponent", "StiWidthHeightZeroComponentCheckLongWidth"), ElementName);
				}
				return string.Format(StiLocalizationExt.Get("CheckComponent", "StiWidthHeightZeroComponentCheckLongHeight"), ElementName);
			}
			return string.Format(StiLocalizationExt.Get("CheckComponent", "StiWidthHeightZeroComponentCheckLongWidthHeight"), ElementName);
		}
	}

	public override StiCheckStatus Status => StiCheckStatus.Information;

	public override object ProcessCheck(StiReport report, object obj)
	{
		StiComponent stiComponent = obj as StiComponent;
		bool flag = false;
		if (!(obj is IStiElement) && (stiComponent.Width == 0.0 || stiComponent.Height == 0.0) && !(stiComponent is StiBand) && !(stiComponent is StiPointPrimitive))
		{
			flag = true;
		}
		if (flag)
		{
			StiWidthHeightZeroComponentCheck stiWidthHeightZeroComponentCheck = new StiWidthHeightZeroComponentCheck();
			stiWidthHeightZeroComponentCheck.Element = obj;
			stiWidthHeightZeroComponentCheck.Actions.Add(new StiDeleteComponentAction());
			return stiWidthHeightZeroComponentCheck;
		}
		return null;
	}
}

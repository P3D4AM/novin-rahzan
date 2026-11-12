using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiVerySmallSizesOfComponentsCheck : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiVerySmallSizesOfComponentsCheckShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiVerySmallSizesOfComponentsCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Information;

	public override object ProcessCheck(StiReport report, object obj)
	{
		bool flag = false;
		if (report.Designer != null && report.Designer.Info != null)
		{
			if (obj is StiPointPrimitive)
			{
				flag = false;
			}
			else if (obj is StiHorizontalLinePrimitive)
			{
				if (((StiHorizontalLinePrimitive)obj).Width >= 0.0 && ((StiHorizontalLinePrimitive)obj).Width < report.Designer.Info.GridSize)
				{
					flag = true;
				}
			}
			else if (obj is StiVerticalLinePrimitive)
			{
				if (((StiVerticalLinePrimitive)obj).Height >= 0.0 && ((StiVerticalLinePrimitive)obj).Height < report.Designer.Info.GridSize)
				{
					flag = true;
				}
			}
			else if (obj is StiBand)
			{
				StiBand stiBand = obj as StiBand;
				if (stiBand.Components.Count > 0 && stiBand.Height < report.Designer.Info.GridSize)
				{
					flag = true;
				}
			}
			else if (!(obj is IStiElement))
			{
				StiComponent stiComponent = obj as StiComponent;
				if ((stiComponent.Width >= 0.0 && stiComponent.Width < report.Designer.Info.GridSize) || (stiComponent.Height >= 0.0 && stiComponent.Height < report.Designer.Info.GridSize))
				{
					flag = true;
				}
			}
		}
		try
		{
			if (flag)
			{
				StiVerySmallSizesOfComponentsCheck stiVerySmallSizesOfComponentsCheck = new StiVerySmallSizesOfComponentsCheck();
				stiVerySmallSizesOfComponentsCheck.Element = obj;
				stiVerySmallSizesOfComponentsCheck.Actions.Add(new StiDeleteComponentAction());
				stiVerySmallSizesOfComponentsCheck.Actions.Add(new StiVerySmallSizesOfComponentsAction());
				return stiVerySmallSizesOfComponentsCheck;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}

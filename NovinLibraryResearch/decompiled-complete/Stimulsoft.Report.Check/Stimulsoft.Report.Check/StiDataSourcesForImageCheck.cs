using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiDataSourcesForImageCheck : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiDataSourcesForImageCheckShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiDataSourcesForImageCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Information;

	private bool Check()
	{
		if (base.Element is StiImage stiImage)
		{
			int num = 0;
			if (stiImage.ExistImage())
			{
				num++;
			}
			if (!string.IsNullOrEmpty(stiImage.DataColumn))
			{
				num++;
			}
			if (stiImage.ImageData != null && !string.IsNullOrWhiteSpace(stiImage.ImageData.Value))
			{
				num++;
			}
			if (stiImage.ImageURL != null && !string.IsNullOrWhiteSpace(stiImage.ImageURL.Value))
			{
				num++;
			}
			if (!string.IsNullOrWhiteSpace(stiImage.File))
			{
				num++;
			}
			if (num > 1)
			{
				return true;
			}
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
				StiDataSourcesForImageCheck stiDataSourcesForImageCheck = new StiDataSourcesForImageCheck();
				stiDataSourcesForImageCheck.Element = obj;
				return stiDataSourcesForImageCheck;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}

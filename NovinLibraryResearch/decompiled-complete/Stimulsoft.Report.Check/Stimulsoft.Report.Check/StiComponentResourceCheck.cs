using System.Linq;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helper;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Check;

public class StiComponentResourceCheck : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiComponentResourceCheckShort");

	public override string LongMessage
	{
		get
		{
			string arg = string.Empty;
			if (base.Element is StiImage)
			{
				arg = StiHyperlinkProcessor.GetResourceNameFromHyperlink(((StiImage)base.Element).ImageURL.Value);
			}
			else if (base.Element is StiRichText)
			{
				arg = StiHyperlinkProcessor.GetResourceNameFromHyperlink(((StiRichText)base.Element).DataUrl.Value);
			}
			else if (base.Element is StiSubReport)
			{
				arg = StiHyperlinkProcessor.GetResourceNameFromHyperlink(((StiSubReport)base.Element).SubReportUrl);
			}
			return string.Format(StiLocalizationExt.Get("CheckComponent", "StiComponentResourceCheckLong"), arg, ElementName);
		}
	}

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	private bool Check()
	{
		if (base.Element is StiImage stiImage && StiHyperlinkProcessor.IsResourceHyperlink(stiImage.ImageURL.Value))
		{
			try
			{
				string resourceName3 = StiHyperlinkProcessor.GetResourceNameFromHyperlink(stiImage.ImageURL.Value).ToLowerInvariant().Trim();
				StiResource stiResource = stiImage.Report.Dictionary.Resources.ToList().FirstOrDefault((StiResource r) => r.Name != null && r.Name.ToLowerInvariant().Trim() == resourceName3);
				return stiResource == null;
			}
			catch
			{
				return true;
			}
		}
		if (base.Element is StiRichText stiRichText && StiHyperlinkProcessor.IsResourceHyperlink(stiRichText.DataUrl.Value))
		{
			try
			{
				string resourceName2 = StiHyperlinkProcessor.GetResourceNameFromHyperlink(stiRichText.DataUrl.Value).ToLowerInvariant().Trim();
				StiResource stiResource2 = stiRichText.Report.Dictionary.Resources.ToList().FirstOrDefault((StiResource r) => r.Name != null && r.Name.ToLowerInvariant().Trim() == resourceName2);
				return stiResource2 == null;
			}
			catch
			{
				return true;
			}
		}
		if (base.Element is StiSubReport stiSubReport && StiHyperlinkProcessor.IsResourceHyperlink(stiSubReport.SubReportUrl))
		{
			try
			{
				string resourceName = StiHyperlinkProcessor.GetResourceNameFromHyperlink(stiSubReport.SubReportUrl).ToLowerInvariant().Trim();
				StiResource stiResource3 = stiSubReport.Report.Dictionary.Resources.ToList().FirstOrDefault((StiResource r) => r.Name != null && r.Name.ToLowerInvariant().Trim() == resourceName);
				return stiResource3 == null;
			}
			catch
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
				StiComponentResourceCheck stiComponentResourceCheck = new StiComponentResourceCheck();
				stiComponentResourceCheck.Element = obj;
				return stiComponentResourceCheck;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}

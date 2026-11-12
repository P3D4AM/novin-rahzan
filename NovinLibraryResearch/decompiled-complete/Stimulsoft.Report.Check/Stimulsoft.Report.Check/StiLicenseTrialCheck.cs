using Stimulsoft.Base;
using Stimulsoft.Base.Licenses;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiLicenseTrialCheck : StiCheck
{
	public override StiCheckObjectType ObjectType => StiCheckObjectType.Report;

	public override string ElementName => null;

	public override string ShortMessage => StiLocalizationExt.Get("Publish", "TrialVersion");

	public override string LongMessage
	{
		get
		{
			if (StiLicenseKeyValidator.IsTrial(StiLicense.LicenseKey))
			{
				return StiLocalizationExt.Get("CheckLicense", "StiLicenseTrialCheckLong");
			}
			if (!StiLicenseKeyValidator.IsValidOnAnyPlatform(StiLicense.LicenseKey))
			{
				return StiLocalizationExt.Get("CheckLicense", "StiValidSubscriptioRequiredCheckLong");
			}
			return "";
		}
	}

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	public override object ProcessCheck(StiReport report, object obj)
	{
		if (StiLicenseKeyValidator.IsTrial(StiLicense.LicenseKey) || !StiLicenseKeyValidator.IsValidOnAnyPlatform(StiLicense.LicenseKey))
		{
			StiLicenseTrialCheck stiLicenseTrialCheck = new StiLicenseTrialCheck();
			stiLicenseTrialCheck.Actions.Add(new StiBuyProductAction());
			return stiLicenseTrialCheck;
		}
		return null;
	}
}

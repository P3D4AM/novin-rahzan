namespace Stimulsoft.Base.Licenses;

public class StiLicenseActivationResponse : StiLicenseObject
{
	protected override string EncryptKey => "aoc#wm5eoAtrr$a5@m9w";

	public StiLicenseKey LicenseKey { get; set; }

	public string Exception { get; set; }

	public bool ResultSuccess { get; set; }

	public StiNotice ResultNotice { get; set; }

	public static StiLicenseActivationResponse MakeWrong(StiNotice notice)
	{
		return new StiLicenseActivationResponse
		{
			ResultSuccess = false,
			ResultNotice = notice
		};
	}

	public static StiLicenseActivationResponse MakeFine(StiLicenseKey licenseKey)
	{
		return new StiLicenseActivationResponse
		{
			ResultSuccess = true,
			LicenseKey = licenseKey
		};
	}

	public static StiLicenseActivationResponse Get(byte[] bytes)
	{
		StiLicenseActivationResponse stiLicenseActivationResponse = new StiLicenseActivationResponse();
		stiLicenseActivationResponse.DecryptFromBytes(bytes);
		return stiLicenseActivationResponse;
	}

	public static StiLicenseActivationResponse Get(string str)
	{
		StiLicenseActivationResponse stiLicenseActivationResponse = new StiLicenseActivationResponse();
		stiLicenseActivationResponse.DecryptFromString(str);
		return stiLicenseActivationResponse;
	}
}

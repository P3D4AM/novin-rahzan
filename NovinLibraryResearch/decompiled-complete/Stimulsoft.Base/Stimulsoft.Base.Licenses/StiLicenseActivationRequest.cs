using System.ComponentModel;

namespace Stimulsoft.Base.Licenses;

public class StiLicenseActivationRequest : StiLicenseObject
{
	protected override string EncryptKey => "aoc#wm5eoAtrr$a5@m9w";

	public string MachineName { get; set; }

	public string MachineUserName { get; set; }

	public string MachineUserDomainName { get; set; }

	public string MachineOSVersion { get; set; }

	public string MachineAddress { get; set; }

	public string MachineGuid { get; set; }

	public string UserName { get; set; }

	public string Password { get; set; }

	[DefaultValue(StiActivationType.Server)]
	public StiActivationType Type { get; set; }

	public string DeviceId { get; set; }

	public string SessionKey { get; set; }

	public string Version { get; set; }

	public static StiLicenseActivationRequest Get(byte[] bytes)
	{
		StiLicenseActivationRequest stiLicenseActivationRequest = new StiLicenseActivationRequest();
		stiLicenseActivationRequest.DecryptFromBytes(bytes);
		return stiLicenseActivationRequest;
	}

	public static StiLicenseActivationRequest Get(string str)
	{
		StiLicenseActivationRequest stiLicenseActivationRequest = new StiLicenseActivationRequest();
		stiLicenseActivationRequest.DecryptFromString(str);
		return stiLicenseActivationRequest;
	}

	public StiLicenseActivationRequest()
	{
		UserName = string.Empty;
		Password = string.Empty;
		DeviceId = string.Empty;
		Type = StiActivationType.Server;
	}
}

namespace mCore;

public interface ILicense
{
	string Company { get; set; }

	string Key { get; set; }

	string LicenseType { get; set; }

	bool IsLicensed { get; }
}

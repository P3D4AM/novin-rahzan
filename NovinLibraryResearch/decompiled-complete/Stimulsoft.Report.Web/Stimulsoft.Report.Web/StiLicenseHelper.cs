using System;
using System.Security.Cryptography;
using Stimulsoft.Base;
using Stimulsoft.Base.Licenses;

namespace Stimulsoft.Report.Web;

internal class StiLicenseHelper
{
	internal static bool CheckAnyLicense()
	{
		return StiLicenseKeyValidator.GetLicenseKey() != null;
	}

	internal static string GetUserName()
	{
		StiLicenseKey licenseKey = StiLicenseKeyValidator.GetLicenseKey();
		if (licenseKey == null)
		{
			return "";
		}
		return licenseKey.UserName;
	}

	internal static bool IsValidOnWeb()
	{
		StiLicenseKey licenseKey = StiLicenseKeyValidator.GetLicenseKey();
		bool flag = !StiLicenseKeyValidator.IsValidOnWebFramework(licenseKey);
		if (!typeof(StiLicense).AssemblyQualifiedName.Contains("PublicKeyToken=ebe6666cba19647a"))
		{
			flag = true;
		}
		if (!flag)
		{
			try
			{
				using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(512);
				using SHA1CryptoServiceProvider halg = new SHA1CryptoServiceProvider();
				rSACryptoServiceProvider.FromXmlString("<RSAKeyValue><Modulus>2GCA4rfVor/2q7m+NVisL3ZFVvnklg14zeP0F+yNpBdoSm9iuOUBLodsOH6I3Xb0j5/OznYKFpii35RcjiPvww7Uthz1he2G8PGCvFMKuA4Wzgp5Kdv0uR7zSQ8J9lA/rZ7u8wh+h72umBku1pLFL2nvJIT56u4Qe7+OJFLrqWk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
				flag = !rSACryptoServiceProvider.VerifyData(licenseKey.GetCheckBytes(), halg, licenseKey.GetSignatureBytes());
			}
			catch (Exception)
			{
				flag = true;
			}
		}
		return !flag;
	}
}

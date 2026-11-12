using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using Stimulsoft.Base.Licenses;

namespace Stimulsoft.Base;

public static class StiLicense
{
	private static StiLicenseKey licenseKey;

	private static string key;

	internal static StiLicenseKey LicenseKey
	{
		get
		{
			return licenseKey;
		}
		set
		{
			if (value != licenseKey)
			{
				licenseKey = value;
				StiLicense.LicenseChanged?.Invoke(null, EventArgs.Empty);
			}
		}
	}

	public static string Key
	{
		get
		{
			if (key == null)
			{
				LoadFromFile(StiLicensePath.GetStimulsoftLicenseKeyPath());
			}
			return key;
		}
		set
		{
			SetNewLicenseKey(value);
		}
	}

	internal static event EventHandler LicenseChanged;

	internal static void SetNewLicenseKey(string value, bool throwException = true)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			key = null;
			LicenseKey = null;
			return;
		}
		StiLicenseKey stiLicenseKey = null;
		try
		{
			stiLicenseKey = StiLicenseKey.Get(value);
		}
		catch
		{
		}
		if (IsValidLicenseKey(stiLicenseKey))
		{
			LicenseKey = stiLicenseKey;
			key = value;
			return;
		}
		if (throwException)
		{
			throw new Exception("The license key is not valid!");
		}
		key = null;
		LicenseKey = null;
	}

	internal static bool IsValidLicenseKey(StiLicenseKey licenseKey)
	{
		try
		{
			if (licenseKey == null || licenseKey.Signature == null || !typeof(StiLicense).AssemblyQualifiedName.Contains("PublicKeyToken=ebe6666cba19647a"))
			{
				return false;
			}
			using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(512);
			using SHA1CryptoServiceProvider halg = new SHA1CryptoServiceProvider();
			rSACryptoServiceProvider.FromXmlString("<RSAKeyValue><Modulus>2GCA4rfVor/2q7m+NVisL3ZFVvnklg14zeP0F+yNpBdoSm9iuOUBLodsOH6I3Xb0j5/OznYKFpii35RcjiPvww7Uthz1he2G8PGCvFMKuA4Wzgp5Kdv0uR7zSQ8J9lA/rZ7u8wh+h72umBku1pLFL2nvJIT56u4Qe7+OJFLrqWk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
			return rSACryptoServiceProvider.VerifyData(licenseKey.GetCheckBytes(), halg, licenseKey.GetSignatureBytes());
		}
		catch (Exception)
		{
			return false;
		}
	}

	public static void LoadFromFile(string file)
	{
		LoadFromString(File.ReadAllText(file));
	}

	public static void LoadFromStream(Stream stream)
	{
		using StreamReader streamReader = new StreamReader(stream);
		LoadFromString(streamReader.ReadToEnd());
	}

	public static void LoadFromString(string licenseKey)
	{
		Key = licenseKey;
	}

	public static void LoadFromEntryAssembly(string resourceName = "license.key")
	{
		LoadFromAssembly(Assembly.GetEntryAssembly(), resourceName);
	}

	public static void LoadFromAssembly(Assembly a, string resourceName = "license.key")
	{
		Stream manifestResourceStream = a.GetManifestResourceStream(null, resourceName);
		if (manifestResourceStream != null)
		{
			LoadFromStream(manifestResourceStream);
			manifestResourceStream.Dispose();
			return;
		}
		resourceName = a.GetManifestResourceNames().FirstOrDefault((string n) => n.ToLowerInvariant().EndsWithInvariantIgnoreCase(resourceName));
		if (!string.IsNullOrWhiteSpace(resourceName))
		{
			manifestResourceStream = a.GetManifestResourceStream(null, resourceName);
			if (manifestResourceStream != null)
			{
				LoadFromStream(manifestResourceStream);
				manifestResourceStream.Dispose();
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Identity.Client.OAuth2;
using Microsoft.Identity.Client.Platforms.net45.Native;
using Microsoft.Identity.Client.PlatformsCommon;
using Microsoft.Identity.Client.PlatformsCommon.Interfaces;
using Microsoft.Identity.Client.Utils;

namespace Microsoft.Identity.Client.Platforms.net45;

internal class NetDesktopDeviceAuthManager : IDeviceAuthManager
{
	public bool TryCreateDeviceAuthChallengeResponseAsync(HttpResponseHeaders responseHeaders, Uri endpointUri, out string responseHeader)
	{
		responseHeader = string.Empty;
		string format = "PKeyAuth {0}, Context=\"{1}\", Version=\"{2}\"";
		X509Certificate2 x509Certificate = null;
		if (!DeviceAuthHelper.IsDeviceAuthChallenge(responseHeaders))
		{
			return false;
		}
		if (!DeviceAuthHelper.CanOSPerformPKeyAuth())
		{
			responseHeader = DeviceAuthHelper.GetBypassChallengeResponse(responseHeaders);
			return true;
		}
		IDictionary<string, string> dictionary = DeviceAuthHelper.ParseChallengeData(responseHeaders);
		if (!dictionary.ContainsKey("SubmitUrl"))
		{
			dictionary["SubmitUrl"] = endpointUri.AbsoluteUri;
		}
		try
		{
			x509Certificate = FindCertificate(dictionary);
		}
		catch (MsalException ex)
		{
			if (ex.ErrorCode == "device_certificate_not_found")
			{
				responseHeader = DeviceAuthHelper.GetBypassChallengeResponse(responseHeaders);
				return true;
			}
		}
		DeviceAuthJWTResponse deviceAuthJWTResponse = new DeviceAuthJWTResponse(dictionary["SubmitUrl"], dictionary["nonce"], Convert.ToBase64String(x509Certificate.GetRawCertData()));
		CngKey cngPrivateKey = NetDesktopCryptographyManager.GetCngPrivateKey(x509Certificate);
		byte[] arg = null;
		using (Microsoft.Identity.Client.Platforms.net45.Native.RSACng rSACng = new Microsoft.Identity.Client.Platforms.net45.Native.RSACng(cngPrivateKey))
		{
			rSACng.SignatureHashAlgorithm = CngAlgorithm.Sha256;
			arg = rSACng.SignData(deviceAuthJWTResponse.GetResponseToSign().ToByteArray());
		}
		string arg2 = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", deviceAuthJWTResponse.GetResponseToSign(), Base64UrlHelpers.Encode(arg));
		string arg3 = string.Format(CultureInfo.InvariantCulture, " AuthToken=\"{0}\"", arg2);
		responseHeader = string.Format(CultureInfo.InvariantCulture, format, arg3, dictionary["Context"], dictionary["Version"]);
		return true;
	}

	private static X509Certificate2 FindCertificate(IDictionary<string, string> challengeData)
	{
		X509Store x509Store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
		try
		{
			x509Store.Open(OpenFlags.ReadOnly);
			X509Certificate2Collection certificates = x509Store.Certificates;
			if (challengeData.ContainsKey("CertAuthorities"))
			{
				return FindCertificateByCertAuthorities(challengeData, certificates);
			}
			X509Certificate2Collection x509Certificate2Collection = certificates.Find(X509FindType.FindByThumbprint, challengeData["CertThumbprint"], validOnly: false);
			if (x509Certificate2Collection.Count == 0)
			{
				throw new MsalException("device_certificate_not_found", string.Format(CultureInfo.CurrentCulture, "Device Certificate was not found for {0}", "Cert thumbprint:" + challengeData["CertThumbprint"]));
			}
			return x509Certificate2Collection[0];
		}
		finally
		{
			x509Store.Close();
		}
	}

	private static X509Certificate2 FindCertificateByCertAuthorities(IDictionary<string, string> challengeData, X509Certificate2Collection certCollection)
	{
		X509Certificate2Collection x509Certificate2Collection = null;
		string[] array = challengeData["CertAuthorities"].Split(new string[1] { ";" }, StringSplitOptions.None);
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(new string[1] { "," }, StringSplitOptions.None);
			string text = array2[^1];
			for (int num = array2.Length - 2; num >= 0; num--)
			{
				text = text + " + " + array2[num].Trim();
			}
			x509Certificate2Collection = certCollection.Find(X509FindType.FindByIssuerDistinguishedName, text, validOnly: false);
			if (x509Certificate2Collection.Count > 0)
			{
				break;
			}
		}
		if (x509Certificate2Collection == null || x509Certificate2Collection.Count == 0)
		{
			throw new MsalException("device_certificate_not_found", string.Format(CultureInfo.CurrentCulture, "Device Certificate was not found for {0}", "Cert Authorities:" + challengeData["CertAuthorities"]));
		}
		return x509Certificate2Collection[0];
	}
}

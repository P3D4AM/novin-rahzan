using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading;

namespace Microsoft.Data.SqlClient;

internal class HostGuardianServiceEnclaveProvider : VirtualizationBasedSecurityEnclaveProviderBase
{
	private const string AttestationUrlSuffix = "/v2.0/signingCertificates";

	private int enclaveRetrySleepInSeconds = 3;

	public uint MaxNumRetries { get; set; }

	public int EnclaveRetrySleepInSeconds
	{
		get
		{
			return enclaveRetrySleepInSeconds;
		}
		set
		{
			if (value < 1)
			{
				throw new ArgumentException(Strings.EnclaveRetrySleepInSecondsValueException);
			}
			enclaveRetrySleepInSeconds = value;
		}
	}

	protected override string GetAttestationUrl(string attestationUrl)
	{
		return attestationUrl.TrimEnd(new char[1] { '/' }) + "/v2.0/signingCertificates";
	}

	protected override byte[] MakeRequest(string url)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		Exception ex = null;
		for (int i = 0; i < MaxNumRetries + 1; i++)
		{
			try
			{
				if (i != 0)
				{
					Thread.Sleep(EnclaveRetrySleepInSeconds * 1000);
				}
				WebRequest webRequest = WebRequest.Create(url);
				using WebResponse webResponse = webRequest.GetResponse();
				using Stream stream = webResponse.GetResponseStream();
				DataContractJsonSerializer val = new DataContractJsonSerializer(typeof(byte[]));
				return (byte[])((XmlObjectSerializer)val).ReadObject(stream);
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
		}
		throw new AlwaysEncryptedAttestationException(string.Format(Strings.GetAttestationSigningCertificateRequestFailedFormat, url, ex.Message), ex);
	}
}

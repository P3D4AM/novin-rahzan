using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.Data.SqlClient;

internal class AzureAttestationEnclaveProvider : EnclaveProviderBase
{
	internal class AzureAttestationInfo
	{
		public uint TotalSize { get; set; }

		public EnclavePublicKey Identity { get; set; }

		public AzureAttestationToken AttestationToken { get; set; }

		public long SessionId { get; set; }

		public EnclaveType EnclaveType { get; set; }

		public EnclaveDiffieHellmanInfo EnclaveDHInfo { get; set; }

		public AzureAttestationInfo(byte[] attestationInfo)
		{
			try
			{
				int num = 0;
				TotalSize = BitConverter.ToUInt32(attestationInfo, num);
				num += 4;
				int num2 = BitConverter.ToInt32(attestationInfo, num);
				num += 4;
				int num3 = BitConverter.ToInt32(attestationInfo, num);
				num += 4;
				int enclaveType = BitConverter.ToInt32(attestationInfo, num);
				EnclaveType = (EnclaveType)enclaveType;
				num += 4;
				byte[] payload = attestationInfo.Skip(num).Take(num2).ToArray();
				Identity = new EnclavePublicKey(payload);
				num += num2;
				byte[] payload2 = attestationInfo.Skip(num).Take(num3).ToArray();
				AttestationToken = new AzureAttestationToken(payload2);
				num += num3;
				uint value = BitConverter.ToUInt32(attestationInfo, num);
				num += 4;
				SessionId = BitConverter.ToInt64(attestationInfo, num);
				num += 8;
				int count = Convert.ToInt32(value) - 4;
				byte[] payload3 = attestationInfo.Skip(num).Take(count).ToArray();
				EnclaveDHInfo = new EnclaveDiffieHellmanInfo(payload3);
				num += Convert.ToInt32(EnclaveDHInfo.Size);
			}
			catch (Exception ex)
			{
				throw new AlwaysEncryptedAttestationException(string.Format(Strings.FailToParseAttestationInfo, ex.Message));
			}
		}
	}

	internal class AzureAttestationToken
	{
		public string AttestationToken { get; set; }

		public AzureAttestationToken(byte[] payload)
		{
			string @string = Encoding.Default.GetString(payload);
			AttestationToken = @string.Trim().Trim(new char[1] { '"' });
		}
	}

	private const int DiffieHellmanKeySize = 384;

	private const int AzureBasedAttestationProtocolId = 1;

	private const int SigningKeyRetryInSec = 3;

	private const string AttestationUrlSuffix = "/.well-known/openid-configuration";

	private static readonly MemoryCache OpenIdConnectConfigurationCache = new MemoryCache("OpenIdConnectConfigurationCache", (NameValueCollection)null);

	internal override void GetEnclaveSession(EnclaveSessionParameters enclaveSessionParameters, bool generateCustomData, out SqlEnclaveSession sqlEnclaveSession, out long counter, out byte[] customData, out int customDataLength)
	{
		GetEnclaveSessionHelper(enclaveSessionParameters, generateCustomData, out sqlEnclaveSession, out counter, out customData, out customDataLength);
	}

	internal override SqlEnclaveAttestationParameters GetAttestationParameters(string attestationUrl, byte[] customData, int customDataLength)
	{
		ECDiffieHellman clientDiffieHellmanKey = KeyConverter.CreateECDiffieHellman(384);
		byte[] input = PrepareAttestationParameters(attestationUrl, customData, customDataLength);
		return new SqlEnclaveAttestationParameters(1, input, clientDiffieHellmanKey);
	}

	internal override void CreateEnclaveSession(byte[] attestationInfo, ECDiffieHellman clientDHKey, EnclaveSessionParameters enclaveSessionParameters, byte[] customData, int customDataLength, out SqlEnclaveSession sqlEnclaveSession, out long counter)
	{
		sqlEnclaveSession = null;
		counter = 0L;
		try
		{
			((ObjectCache)EnclaveProviderBase.ThreadRetryCache).Remove(Thread.CurrentThread.ManagedThreadId.ToString(), (string)null);
			sqlEnclaveSession = GetEnclaveSessionFromCache(enclaveSessionParameters, out counter);
			if (sqlEnclaveSession == null)
			{
				if (string.IsNullOrEmpty(enclaveSessionParameters.AttestationUrl) || customData == null || customDataLength <= 0)
				{
					throw new AlwaysEncryptedAttestationException(Strings.FailToCreateEnclaveSession);
				}
				IdentityModelEventSource.ShowPII = true;
				AzureAttestationInfo azureAttestationInfo = new AzureAttestationInfo(attestationInfo);
				VerifyAzureAttestationInfo(enclaveSessionParameters.AttestationUrl, azureAttestationInfo.EnclaveType, azureAttestationInfo.AttestationToken.AttestationToken, azureAttestationInfo.Identity, customData);
				byte[] sharedSecret = GetSharedSecret(azureAttestationInfo.Identity, customData, azureAttestationInfo.EnclaveType, azureAttestationInfo.EnclaveDHInfo, clientDHKey);
				sqlEnclaveSession = AddEnclaveSessionToCache(enclaveSessionParameters, sharedSecret, azureAttestationInfo.SessionId, out counter);
			}
		}
		finally
		{
			UpdateEnclaveSessionLockStatus(sqlEnclaveSession);
		}
	}

	internal override void InvalidateEnclaveSession(EnclaveSessionParameters enclaveSessionParameters, SqlEnclaveSession enclaveSessionToInvalidate)
	{
		InvalidateEnclaveSessionHelper(enclaveSessionParameters, enclaveSessionToInvalidate);
	}

	internal byte[] PrepareAttestationParameters(string attestationUrl, byte[] attestNonce, int attestNonceLength)
	{
		if (!string.IsNullOrEmpty(attestationUrl) && attestNonce != null && attestNonceLength > 0)
		{
			string s = attestationUrl + "\0";
			byte[] bytes = Encoding.Unicode.GetBytes(s);
			byte[] bytes2 = BitConverter.GetBytes(bytes.Length);
			byte[] bytes3 = BitConverter.GetBytes(attestNonceLength);
			int num = bytes.Length + bytes2.Length + attestNonce.Length + bytes3.Length;
			int num2 = 0;
			byte[] array = new byte[num];
			Buffer.BlockCopy(bytes2, 0, array, num2, bytes2.Length);
			num2 += bytes2.Length;
			Buffer.BlockCopy(bytes, 0, array, num2, bytes.Length);
			num2 += bytes.Length;
			Buffer.BlockCopy(bytes3, 0, array, num2, bytes3.Length);
			num2 += bytes3.Length;
			Buffer.BlockCopy(attestNonce, 0, array, num2, attestNonce.Length);
			num2 += attestNonce.Length;
			return array;
		}
		throw new AlwaysEncryptedAttestationException(Strings.FailToCreateEnclaveSession);
	}

	private void VerifyAzureAttestationInfo(string attestationUrl, EnclaveType enclaveType, string attestationToken, EnclavePublicKey enclavePublicKey, byte[] nonce)
	{
		bool flag = false;
		string attestationInstanceUrl = GetAttestationInstanceUrl(attestationUrl);
		string exceptionMessage = string.Empty;
		bool flag2;
		bool flag3;
		do
		{
			flag2 = false;
			OpenIdConnectConfiguration openIdConfigForSigningKeys = GetOpenIdConfigForSigningKeys(attestationInstanceUrl, flag);
			flag3 = VerifyTokenSignature(attestationToken, attestationInstanceUrl, openIdConfigForSigningKeys.SigningKeys, out var isKeySigningExpired, out exceptionMessage);
			if (!flag3 && isKeySigningExpired && !flag)
			{
				flag = true;
				flag2 = true;
			}
		}
		while (flag2);
		if (!flag3)
		{
			throw new AlwaysEncryptedAttestationException(string.Format(Strings.AttestationTokenSignatureValidationFailed, exceptionMessage));
		}
		ValidateAttestationClaims(enclaveType, attestationToken, enclavePublicKey, nonce);
	}

	private static string GetInnerMostExceptionMessage(Exception exception)
	{
		Exception ex = exception;
		while (ex.InnerException != null)
		{
			ex = ex.InnerException;
		}
		return ex.Message;
	}

	private OpenIdConnectConfiguration GetOpenIdConfigForSigningKeys(string url, bool forceUpdate)
	{
		OpenIdConnectConfiguration openIdConnectConfiguration = ((ObjectCache)OpenIdConnectConfigurationCache)[url] as OpenIdConnectConfiguration;
		if (forceUpdate || openIdConnectConfiguration == null)
		{
			string metadataAddress = url + "/.well-known/openid-configuration";
			try
			{
				IConfigurationManager<OpenIdConnectConfiguration> configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(metadataAddress, new OpenIdConnectConfigurationRetriever());
				openIdConnectConfiguration = configurationManager.GetConfigurationAsync(CancellationToken.None).Result;
			}
			catch (Exception ex)
			{
				throw new AlwaysEncryptedAttestationException(string.Format(Strings.GetAttestationTokenSigningKeysFailed, GetInnerMostExceptionMessage(ex)), ex);
			}
			((ObjectCache)OpenIdConnectConfigurationCache).Add(url, (object)openIdConnectConfiguration, (DateTimeOffset)DateTime.UtcNow.AddDays(1.0), (string)null);
		}
		return openIdConnectConfiguration;
	}

	private string GetAttestationInstanceUrl(string attestationUrl)
	{
		Uri uri = new Uri(attestationUrl);
		return uri.GetLeftPart(UriPartial.Authority);
	}

	private static ICollection<string> GenerateListOfIssuers(string tokenIssuerUrl)
	{
		List<string> list = new List<string>();
		Uri uri = new Uri(tokenIssuerUrl);
		int port = uri.Port;
		bool isDefaultPort = uri.IsDefaultPort;
		string leftPart = uri.GetLeftPart(UriPartial.Authority);
		list.Add(leftPart);
		if (isDefaultPort)
		{
			list.Add(leftPart + ":" + port);
		}
		return list;
	}

	private bool VerifyTokenSignature(string attestationToken, string tokenIssuerUrl, ICollection<SecurityKey> issuerSigningKeys, out bool isKeySigningExpired, out string exceptionMessage)
	{
		exceptionMessage = string.Empty;
		bool result = false;
		isKeySigningExpired = false;
		TokenValidationParameters validationParameters = new TokenValidationParameters
		{
			RequireExpirationTime = true,
			ValidateLifetime = true,
			ValidateIssuer = true,
			ValidateAudience = false,
			RequireSignedTokens = true,
			ValidIssuers = GenerateListOfIssuers(tokenIssuerUrl),
			IssuerSigningKeys = issuerSigningKeys
		};
		try
		{
			JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
			SecurityToken validatedToken;
			ClaimsPrincipal claimsPrincipal = jwtSecurityTokenHandler.ValidateToken(attestationToken, validationParameters, out validatedToken);
			result = true;
		}
		catch (SecurityTokenExpiredException innerException)
		{
			throw new AlwaysEncryptedAttestationException(Strings.ExpiredAttestationToken, innerException);
		}
		catch (SecurityTokenValidationException exception)
		{
			isKeySigningExpired = true;
			Thread.Sleep(3000);
			exceptionMessage = GetInnerMostExceptionMessage(exception);
		}
		catch (Exception exception2)
		{
			throw new AlwaysEncryptedAttestationException(string.Format(Strings.InvalidAttestationToken, GetInnerMostExceptionMessage(exception2)));
		}
		return result;
	}

	private byte[] ComputeSHA256(byte[] data)
	{
		byte[] array = null;
		try
		{
			using SHA256 sHA = SHA256.Create();
			return sHA.ComputeHash(data);
		}
		catch (Exception innerException)
		{
			throw new AlwaysEncryptedAttestationException(Strings.InvalidArgumentToSHA256, innerException);
		}
	}

	private void ValidateAttestationClaims(EnclaveType enclaveType, string attestationToken, EnclavePublicKey enclavePublicKey, byte[] nonce)
	{
		JsonWebToken jsonWebToken;
		try
		{
			JsonWebTokenHandler jsonWebTokenHandler = new JsonWebTokenHandler();
			jsonWebToken = jsonWebTokenHandler.ReadJsonWebToken(attestationToken);
		}
		catch (ArgumentException ex)
		{
			throw new AlwaysEncryptedAttestationException(string.Format(Strings.FailToParseAttestationToken, ex.Message));
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (Claim item in jsonWebToken.Claims.ToList())
		{
			dictionary.Add(item.Type, item.Value);
		}
		ValidateClaim(dictionary, "aas-ehd", enclavePublicKey.PublicKey);
		if (enclaveType == EnclaveType.Vbs)
		{
			ValidateClaim(dictionary, "rp_data", nonce);
		}
	}

	private void ValidateClaim(Dictionary<string, string> claims, string claimName, byte[] actualData)
	{
		if (!claims.TryGetValue(claimName, out var value))
		{
			throw new AlwaysEncryptedAttestationException(string.Format(Strings.MissingClaimInAttestationToken, claimName));
		}
		string empty = string.Empty;
		try
		{
			empty = Base64UrlEncoder.Encode(actualData);
		}
		catch (Exception)
		{
			throw new AlwaysEncryptedAttestationException(Strings.InvalidArgumentToBase64UrlDecoder);
		}
		if (!string.Equals(empty, value, StringComparison.Ordinal))
		{
			throw new AlwaysEncryptedAttestationException(string.Format(Strings.InvalidClaimInAttestationToken, claimName, value));
		}
	}

	private byte[] GetSharedSecret(EnclavePublicKey enclavePublicKey, byte[] nonce, EnclaveType enclaveType, EnclaveDiffieHellmanInfo enclaveDHInfo, ECDiffieHellman clientDHKey)
	{
		byte[] publicKey = enclavePublicKey.PublicKey;
		if (enclaveType == EnclaveType.Sgx)
		{
			for (int i = 0; i < publicKey.Length; i++)
			{
				publicKey[i] ^= nonce[i % nonce.Length];
			}
		}
		using (RSA rSA = KeyConverter.CreateRSAFromPublicKeyBlob(publicKey))
		{
			if (!rSA.VerifyData(enclaveDHInfo.PublicKey, enclaveDHInfo.PublicKeySignature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1))
			{
				throw new ArgumentException(Strings.GetSharedSecretFailed);
			}
		}
		using ECDiffieHellman eCDiffieHellman = KeyConverter.CreateECDiffieHellmanFromPublicKeyBlob(enclaveDHInfo.PublicKey);
		return KeyConverter.DeriveKey(clientDHKey, eCDiffieHellman.PublicKey);
	}
}

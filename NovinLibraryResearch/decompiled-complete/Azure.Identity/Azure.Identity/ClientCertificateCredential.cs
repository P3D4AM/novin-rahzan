using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;
using Microsoft.Identity.Client;

namespace Azure.Identity;

public class ClientCertificateCredential : TokenCredential
{
	internal interface IX509Certificate2Provider
	{
		ValueTask<X509Certificate2> GetCertificateAsync(bool async, CancellationToken cancellationToken);
	}

	private class X509Certificate2FromObjectProvider : IX509Certificate2Provider
	{
		private X509Certificate2 Certificate { get; }

		public X509Certificate2FromObjectProvider(X509Certificate2 clientCertificate)
		{
			Certificate = clientCertificate ?? throw new ArgumentNullException("clientCertificate");
		}

		public ValueTask<X509Certificate2> GetCertificateAsync(bool async, CancellationToken cancellationToken)
		{
			return new ValueTask<X509Certificate2>(Certificate);
		}
	}

	internal class X509Certificate2FromFileProvider : IX509Certificate2Provider
	{
		private delegate void ImportPkcs8PrivateKeyDelegate(ReadOnlySpan<byte> blob, out int bytesRead);

		private X509Certificate2 Certificate { get; set; }

		internal string CertificatePath { get; }

		public X509Certificate2FromFileProvider(string clientCertificatePath)
		{
			CertificatePath = clientCertificatePath ?? throw new ArgumentNullException("clientCertificatePath");
		}

		public ValueTask<X509Certificate2> GetCertificateAsync(bool async, CancellationToken cancellationToken)
		{
			if (Certificate != null)
			{
				return new ValueTask<X509Certificate2>(Certificate);
			}
			return Path.GetExtension(CertificatePath).ToLowerInvariant() switch
			{
				".pfx" => LoadCertificateFromPfxFileAsync(async, CertificatePath, cancellationToken), 
				".pem" => LoadCertificateFromPemFileAsync(async, CertificatePath, cancellationToken), 
				_ => throw new CredentialUnavailableException("Only .pfx and .pem files are supported."), 
			};
		}

		private async ValueTask<X509Certificate2> LoadCertificateFromPfxFileAsync(bool async, string clientCertificatePath, CancellationToken cancellationToken)
		{
			if (Certificate != null)
			{
				return Certificate;
			}
			try
			{
				if (!async)
				{
					Certificate = new X509Certificate2(clientCertificatePath);
				}
				else
				{
					List<byte> certContents = new List<byte>();
					byte[] buf = new byte[4096];
					int offset = 0;
					using (Stream s = File.OpenRead(clientCertificatePath))
					{
						int num;
						do
						{
							num = await s.ReadAsync(buf, offset, buf.Length, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
							for (int i = 0; i < num; i++)
							{
								certContents.Add(buf[i]);
							}
						}
						while (num != 0);
					}
					Certificate = new X509Certificate2(certContents.ToArray());
				}
				return Certificate;
			}
			catch (Exception ex) when (!(ex is OperationCanceledException))
			{
				throw new CredentialUnavailableException("Could not load certificate file", ex);
			}
		}

		private async ValueTask<X509Certificate2> LoadCertificateFromPemFileAsync(bool async, string clientCertificatePath, CancellationToken cancellationToken)
		{
			if (Certificate != null)
			{
				return Certificate;
			}
			try
			{
				string input;
				if (!async)
				{
					input = File.ReadAllText(clientCertificatePath);
				}
				else
				{
					cancellationToken.ThrowIfCancellationRequested();
					using StreamReader sr = new StreamReader(clientCertificatePath);
					input = await sr.ReadToEndAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
				Regex regex = new Regex("(-+BEGIN CERTIFICATE-+)(\n\r?|\r\n?)([A-Za-z0-9+/\n\r]+=*)(\n\r?|\r\n?)(-+END CERTIFICATE-+)", RegexOptions.CultureInvariant, TimeSpan.FromSeconds(5.0));
				Regex regex2 = new Regex("(-+BEGIN PRIVATE KEY-+)(\n\r?|\r\n?)([A-Za-z0-9+/\n\r]+=*)(\n\r?|\r\n?)(-+END PRIVATE KEY-+)", RegexOptions.CultureInvariant, TimeSpan.FromSeconds(5.0));
				Match match = regex.Match(input);
				Match match2 = regex2.Match(input);
				if (!match.Success)
				{
					throw new InvalidDataException("Could not find certificate in PEM file");
				}
				if (!match2.Success)
				{
					throw new InvalidDataException("Could not find private key in PEM file");
				}
				MethodInfo method = typeof(RSA).GetMethod("ImportPkcs8PrivateKey", BindingFlags.Instance | BindingFlags.Public, null, new Type[2]
				{
					typeof(ReadOnlySpan<byte>),
					typeof(int).MakeByRefType()
				}, null);
				MethodInfo method2 = typeof(RSACertificateExtensions).GetMethod("CopyWithPrivateKey", BindingFlags.Static | BindingFlags.Public, null, new Type[2]
				{
					typeof(X509Certificate2),
					typeof(RSA)
				}, null);
				if (method2 == null)
				{
					throw new PlatformNotSupportedException("The current platform does not support reading a private key from a PEM file");
				}
				RSA rSA;
				if (method != null)
				{
					rSA = RSA.Create();
					((ImportPkcs8PrivateKeyDelegate)method.CreateDelegate(typeof(ImportPkcs8PrivateKeyDelegate), rSA))(Convert.FromBase64String(match2.Groups[3].Value), out var _);
				}
				else
				{
					rSA = LightweightPkcs8Decoder.DecodeRSAPkcs8(Convert.FromBase64String(match2.Groups[3].Value));
				}
				X509Certificate2 x509Certificate = new X509Certificate2(Convert.FromBase64String(match.Groups[3].Value));
				Certificate = (X509Certificate2)method2.Invoke(null, new object[2] { x509Certificate, rSA });
				if (Certificate.PrivateKey == null)
				{
					Certificate.PrivateKey = rSA;
				}
				return Certificate;
			}
			catch (Exception ex) when (!(ex is OperationCanceledException))
			{
				throw new CredentialUnavailableException("Could not load certificate file", ex);
			}
		}
	}

	private readonly MsalConfidentialClient _client;

	private readonly CredentialPipeline _pipeline;

	internal string TenantId { get; }

	internal string ClientId { get; }

	internal IX509Certificate2Provider ClientCertificateProvider { get; }

	protected ClientCertificateCredential()
	{
	}

	public ClientCertificateCredential(string tenantId, string clientId, string clientCertificatePath)
		: this(tenantId, clientId, clientCertificatePath, null, null, null)
	{
	}

	public ClientCertificateCredential(string tenantId, string clientId, string clientCertificatePath, TokenCredentialOptions options)
		: this(tenantId, clientId, clientCertificatePath, options, null, null)
	{
	}

	public ClientCertificateCredential(string tenantId, string clientId, string clientCertificatePath, ClientCertificateCredentialOptions options)
		: this(tenantId, clientId, clientCertificatePath, options, null, null)
	{
	}

	public ClientCertificateCredential(string tenantId, string clientId, X509Certificate2 clientCertificate)
		: this(tenantId, clientId, clientCertificate, null, null, null)
	{
	}

	public ClientCertificateCredential(string tenantId, string clientId, X509Certificate2 clientCertificate, TokenCredentialOptions options)
		: this(tenantId, clientId, clientCertificate, options, null, null)
	{
	}

	public ClientCertificateCredential(string tenantId, string clientId, X509Certificate2 clientCertificate, ClientCertificateCredentialOptions options)
		: this(tenantId, clientId, clientCertificate, options, null, null)
	{
	}

	internal ClientCertificateCredential(string tenantId, string clientId, string certificatePath, TokenCredentialOptions options, CredentialPipeline pipeline, MsalConfidentialClient client)
		: this(tenantId, clientId, new X509Certificate2FromFileProvider(certificatePath ?? throw new ArgumentNullException("certificatePath")), options, pipeline, client)
	{
	}

	internal ClientCertificateCredential(string tenantId, string clientId, X509Certificate2 certificate, TokenCredentialOptions options, CredentialPipeline pipeline, MsalConfidentialClient client)
		: this(tenantId, clientId, new X509Certificate2FromObjectProvider(certificate ?? throw new ArgumentNullException("certificate")), options, pipeline, client)
	{
	}

	internal ClientCertificateCredential(string tenantId, string clientId, IX509Certificate2Provider certificateProvider, TokenCredentialOptions options, CredentialPipeline pipeline, MsalConfidentialClient client)
	{
		TenantId = Validations.ValidateTenantId(tenantId, "tenantId");
		ClientId = clientId ?? throw new ArgumentNullException("clientId");
		ClientCertificateProvider = certificateProvider;
		_pipeline = pipeline ?? CredentialPipeline.GetInstance(options);
		_client = client ?? new MsalConfidentialClient(_pipeline, tenantId, clientId, certificateProvider, (options as ClientCertificateCredentialOptions)?.SendCertificateChain ?? false, options as ITokenCacheOptions);
	}

	public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken = default(CancellationToken))
	{
		using CredentialDiagnosticScope credentialDiagnosticScope = _pipeline.StartGetTokenScope("ClientCertificateCredential.GetToken", requestContext);
		try
		{
			AuthenticationResult authenticationResult = _client.AcquireTokenForClientAsync(requestContext.Scopes, async: false, cancellationToken).EnsureCompleted();
			return credentialDiagnosticScope.Succeeded(new AccessToken(authenticationResult.AccessToken, authenticationResult.ExpiresOn));
		}
		catch (Exception ex)
		{
			throw credentialDiagnosticScope.FailWrapAndThrow(ex);
		}
	}

	public override async ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken = default(CancellationToken))
	{
		using CredentialDiagnosticScope scope = _pipeline.StartGetTokenScope("ClientCertificateCredential.GetToken", requestContext);
		try
		{
			AuthenticationResult authenticationResult = await _client.AcquireTokenForClientAsync(requestContext.Scopes, async: true, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			return scope.Succeeded(new AccessToken(authenticationResult.AccessToken, authenticationResult.ExpiresOn));
		}
		catch (Exception ex)
		{
			throw scope.FailWrapAndThrow(ex);
		}
	}
}

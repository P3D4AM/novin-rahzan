using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;

namespace Azure.Identity;

public class VisualStudioCredential : TokenCredential
{
	private readonly struct VisualStudioTokenProvider : IComparable<VisualStudioTokenProvider>
	{
		private readonly int _preference;

		public string Path { get; }

		public string[] Arguments { get; }

		public VisualStudioTokenProvider(string path, string[] arguments, int preference)
		{
			Path = path;
			Arguments = arguments;
			_preference = preference;
		}

		public int CompareTo(VisualStudioTokenProvider other)
		{
			int preference = _preference;
			return preference.CompareTo(other._preference);
		}
	}

	private const string TokenProviderFilePath = ".IdentityService\\AzureServiceAuth\\tokenprovider.json";

	private const string ResourceArgumentName = "--resource";

	private const string TenantArgumentName = "--tenant";

	private readonly CredentialPipeline _pipeline;

	private readonly string _tenantId;

	private readonly IFileSystemService _fileSystem;

	private readonly IProcessService _processService;

	public VisualStudioCredential()
		: this(null)
	{
	}

	public VisualStudioCredential(VisualStudioCredentialOptions options)
		: this(options?.TenantId, CredentialPipeline.GetInstance(options), null, null)
	{
	}

	internal VisualStudioCredential(string tenantId, CredentialPipeline pipeline, IFileSystemService fileSystem, IProcessService processService)
	{
		_tenantId = tenantId;
		_pipeline = pipeline ?? CredentialPipeline.GetInstance(null);
		_fileSystem = fileSystem ?? FileSystemService.Default;
		_processService = processService ?? ProcessService.Default;
	}

	public override async ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
	{
		return await GetTokenImplAsync(requestContext, async: true, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
	{
		return GetTokenImplAsync(requestContext, async: false, cancellationToken).EnsureCompleted();
	}

	private async ValueTask<AccessToken> GetTokenImplAsync(TokenRequestContext requestContext, bool async, CancellationToken cancellationToken)
	{
		using CredentialDiagnosticScope scope = _pipeline.StartGetTokenScope("VisualStudioCredential.GetToken", requestContext);
		try
		{
			if (string.Equals(_tenantId, "adfs", StringComparison.Ordinal))
			{
				throw new CredentialUnavailableException("VisualStudioCredential authentication unavailable. ADFS tenant/authorities are not supported.");
			}
			string tokenProviderPath = GetTokenProviderPath();
			VisualStudioTokenProvider[] tokenProviders = GetTokenProviders(tokenProviderPath);
			string resource = ScopeUtilities.ScopesToResource(requestContext.Scopes);
			List<ProcessStartInfo> processStartInfos = GetProcessStartInfos(tokenProviders, resource, cancellationToken);
			if (processStartInfos.Count == 0)
			{
				throw new CredentialUnavailableException("No installed instance of Visual Studio was found");
			}
			return scope.Succeeded(await RunProcessesAsync(processStartInfos, async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
		}
		catch (Exception ex)
		{
			throw scope.FailWrapAndThrow(ex);
		}
	}

	private static string GetTokenProviderPath()
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ".IdentityService\\AzureServiceAuth\\tokenprovider.json");
		}
		throw new CredentialUnavailableException("Operating system " + RuntimeInformation.OSDescription + " isn't supported.");
	}

	private async Task<AccessToken> RunProcessesAsync(List<ProcessStartInfo> processStartInfos, bool async, CancellationToken cancellationToken)
	{
		List<Exception> exceptions = new List<Exception>();
		foreach (ProcessStartInfo processStartInfo in processStartInfos)
		{
			string output = string.Empty;
			try
			{
				using ProcessRunner processRunner = new ProcessRunner(_processService.Create(processStartInfo), TimeSpan.FromSeconds(30.0), cancellationToken);
				string text = ((!async) ? processRunner.Run() : (await processRunner.RunAsync().ConfigureAwait(continueOnCapturedContext: false)));
				output = text;
				JsonElement rootElement = JsonDocument.Parse(output).RootElement;
				string @string = rootElement.GetProperty("access_token").GetString();
				DateTimeOffset dateTimeOffset = rootElement.GetProperty("expires_on").GetDateTimeOffset();
				return new AccessToken(@string, dateTimeOffset);
			}
			catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
			{
				exceptions.Add(new CredentialUnavailableException("Process \"" + processStartInfo.FileName + "\" has failed to get access token in 30 seconds."));
			}
			catch (JsonException innerException)
			{
				exceptions.Add(new CredentialUnavailableException("Process \"" + processStartInfo.FileName + "\" has non-json output: " + output + ".", innerException));
			}
			catch (Exception ex2) when (!(ex2 is OperationCanceledException))
			{
				exceptions.Add(new CredentialUnavailableException("Process \"" + processStartInfo.FileName + "\" has failed with unexpected error: " + ex2.Message + ".", ex2));
			}
		}
		switch (exceptions.Count)
		{
		case 0:
			throw new CredentialUnavailableException("No installed instance of Visual Studio was able to get credentials.");
		case 1:
			ExceptionDispatchInfo.Capture(exceptions[0]).Throw();
			return default(AccessToken);
		default:
			throw new AggregateException(exceptions);
		}
	}

	private List<ProcessStartInfo> GetProcessStartInfos(VisualStudioTokenProvider[] visualStudioTokenProviders, string resource, CancellationToken cancellationToken)
	{
		List<ProcessStartInfo> list = new List<ProcessStartInfo>();
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < visualStudioTokenProviders.Length; i++)
		{
			VisualStudioTokenProvider visualStudioTokenProvider = visualStudioTokenProviders[i];
			cancellationToken.ThrowIfCancellationRequested();
			if (!_fileSystem.FileExists(visualStudioTokenProvider.Path))
			{
				continue;
			}
			stringBuilder.Clear();
			stringBuilder.Append("--resource").Append(' ').Append(resource);
			if (_tenantId != null)
			{
				stringBuilder.Append(' ').Append("--tenant").Append(' ')
					.Append(_tenantId);
			}
			string[] arguments = visualStudioTokenProvider.Arguments;
			if (arguments != null && arguments.Length != 0)
			{
				string[] arguments2 = visualStudioTokenProvider.Arguments;
				foreach (string value in arguments2)
				{
					stringBuilder.Append(' ').Append(value);
				}
			}
			ProcessStartInfo item = new ProcessStartInfo
			{
				FileName = visualStudioTokenProvider.Path,
				Arguments = stringBuilder.ToString(),
				ErrorDialog = false,
				CreateNoWindow = true
			};
			list.Add(item);
		}
		return list;
	}

	private VisualStudioTokenProvider[] GetTokenProviders(string tokenProviderPath)
	{
		string tokenProviderContent = GetTokenProviderContent(tokenProviderPath);
		try
		{
			using JsonDocument jsonDocument = JsonDocument.Parse(tokenProviderContent);
			JsonElement property = jsonDocument.RootElement.GetProperty("TokenProviders");
			VisualStudioTokenProvider[] array = new VisualStudioTokenProvider[property.GetArrayLength()];
			for (int i = 0; i < array.Length; i++)
			{
				JsonElement element = property[i];
				string @string = element.GetProperty("Path").GetString();
				int @int = element.GetProperty("Preference").GetInt32();
				string[] stringArrayPropertyValue = GetStringArrayPropertyValue(element, "Arguments");
				array[i] = new VisualStudioTokenProvider(@string, stringArrayPropertyValue, @int);
			}
			Array.Sort(array);
			return array;
		}
		catch (JsonException innerException)
		{
			throw new CredentialUnavailableException("File found at \"" + tokenProviderPath + "\" isn't a valid JSON file", innerException);
		}
		catch (Exception innerException2)
		{
			throw new CredentialUnavailableException("JSON file found at \"" + tokenProviderPath + "\" has invalid schema.", innerException2);
		}
	}

	private string GetTokenProviderContent(string tokenProviderPath)
	{
		try
		{
			return _fileSystem.ReadAllText(tokenProviderPath);
		}
		catch (FileNotFoundException innerException)
		{
			throw new CredentialUnavailableException("Visual Studio Token provider file not found at " + tokenProviderPath, innerException);
		}
		catch (IOException innerException2)
		{
			throw new CredentialUnavailableException("Visual Studio Token provider can't be accessed at " + tokenProviderPath, innerException2);
		}
	}

	private static string[] GetStringArrayPropertyValue(JsonElement element, string name)
	{
		JsonElement property = element.GetProperty(name);
		string[] array = new string[property.GetArrayLength()];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = property[i].GetString();
		}
		return array;
	}
}

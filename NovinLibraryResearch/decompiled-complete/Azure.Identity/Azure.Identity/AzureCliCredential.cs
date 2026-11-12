using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;

namespace Azure.Identity;

public class AzureCliCredential : TokenCredential
{
	private const string AzureCLINotInstalled = "Azure CLI not installed";

	private const string AzNotLogIn = "Please run 'az login' to set up account";

	private const string WinAzureCLIError = "'az' is not recognized";

	private const string AzureCliTimeoutError = "Azure CLI authentication timed out.";

	private const string AzureCliFailedError = "Azure CLI authentication failed due to an unknown error.";

	private const int CliProcessTimeoutMs = 10000;

	private static readonly string DefaultPathWindows = EnvironmentVariables.ProgramFilesX86 + "\\Microsoft SDKs\\Azure\\CLI2\\wbin;" + EnvironmentVariables.ProgramFiles + "\\Microsoft SDKs\\Azure\\CLI2\\wbin";

	private static readonly string DefaultWorkingDirWindows = Environment.GetFolderPath(Environment.SpecialFolder.System);

	private const string DefaultPathNonWindows = "/usr/bin:/usr/local/bin";

	private const string DefaultWorkingDirNonWindows = "/bin/";

	private static readonly string DefaultPath = (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? DefaultPathWindows : "/usr/bin:/usr/local/bin");

	private static readonly string DefaultWorkingDir = (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? DefaultWorkingDirWindows : "/bin/");

	private static readonly Regex AzNotFoundPattern = new Regex("az:(.*)not found");

	private readonly string _path;

	private readonly CredentialPipeline _pipeline;

	private readonly IProcessService _processService;

	public AzureCliCredential()
		: this(CredentialPipeline.GetInstance(null), null)
	{
	}

	internal AzureCliCredential(CredentialPipeline pipeline, IProcessService processService)
	{
		_pipeline = pipeline;
		_path = ((!string.IsNullOrEmpty(EnvironmentVariables.Path)) ? EnvironmentVariables.Path : DefaultPath);
		_processService = processService ?? ProcessService.Default;
	}

	public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken = default(CancellationToken))
	{
		return GetTokenImplAsync(async: false, requestContext, cancellationToken).EnsureCompleted();
	}

	public override async ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken = default(CancellationToken))
	{
		return await GetTokenImplAsync(async: true, requestContext, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	private async ValueTask<AccessToken> GetTokenImplAsync(bool async, TokenRequestContext requestContext, CancellationToken cancellationToken)
	{
		using CredentialDiagnosticScope scope = _pipeline.StartGetTokenScope("AzureCliCredential.GetToken", requestContext);
		try
		{
			return scope.Succeeded(await RequestCliAccessTokenAsync(async, requestContext.Scopes, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
		}
		catch (Exception ex)
		{
			throw scope.FailWrapAndThrow(ex);
		}
	}

	private async ValueTask<AccessToken> RequestCliAccessTokenAsync(bool async, string[] scopes, CancellationToken cancellationToken)
	{
		string text = ScopeUtilities.ScopesToResource(scopes);
		ScopeUtilities.ValidateScope(text);
		GetFileNameAndArguments(text, out var fileName, out var argument);
		ProcessStartInfo azureCliProcessStartInfo = GetAzureCliProcessStartInfo(fileName, argument);
		using ProcessRunner processRunner = new ProcessRunner(_processService.Create(azureCliProcessStartInfo), TimeSpan.FromMilliseconds(10000.0), cancellationToken);
		string output;
		try
		{
			string text2 = ((!async) ? processRunner.Run() : (await processRunner.RunAsync().ConfigureAwait(continueOnCapturedContext: false)));
			output = text2;
		}
		catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
		{
			throw new AuthenticationFailedException("Azure CLI authentication timed out.");
		}
		catch (InvalidOperationException ex2)
		{
			bool num = ex2.Message.StartsWith("'az' is not recognized", StringComparison.CurrentCultureIgnoreCase);
			bool flag = AzNotFoundPattern.IsMatch(ex2.Message);
			if (num || flag)
			{
				throw new CredentialUnavailableException("Azure CLI not installed");
			}
			if (ex2.Message.IndexOf("az login", StringComparison.OrdinalIgnoreCase) != -1 || ex2.Message.IndexOf("az account set", StringComparison.OrdinalIgnoreCase) != -1)
			{
				throw new CredentialUnavailableException("Please run 'az login' to set up account");
			}
			throw new AuthenticationFailedException("Azure CLI authentication failed due to an unknown error. " + ex2.Message);
		}
		return DeserializeOutput(output);
	}

	private ProcessStartInfo GetAzureCliProcessStartInfo(string fileName, string argument)
	{
		return new ProcessStartInfo
		{
			FileName = fileName,
			Arguments = argument,
			UseShellExecute = false,
			ErrorDialog = false,
			CreateNoWindow = true,
			WorkingDirectory = DefaultWorkingDir,
			Environment = { { "PATH", _path } }
		};
	}

	private static void GetFileNameAndArguments(string resource, out string fileName, out string argument)
	{
		string text = "az account get-access-token --output json --resource " + resource;
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmd.exe");
			argument = "/c \"" + text + "\"";
		}
		else
		{
			fileName = "/bin/sh";
			argument = "-c \"" + text + "\"";
		}
	}

	private static AccessToken DeserializeOutput(string output)
	{
		using JsonDocument jsonDocument = JsonDocument.Parse(output);
		JsonElement rootElement = jsonDocument.RootElement;
		string? @string = rootElement.GetProperty("accessToken").GetString();
		JsonElement value;
		DateTimeOffset expiresOn = (rootElement.TryGetProperty("expiresIn", out value) ? (DateTimeOffset.UtcNow + TimeSpan.FromSeconds(value.GetInt64())) : DateTimeOffset.ParseExact(rootElement.GetProperty("expiresOn").GetString(), "yyyy-MM-dd HH:mm:ss.ffffff", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeLocal));
		return new AccessToken(@string, expiresOn);
	}
}

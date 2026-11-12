using System.Collections.Generic;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Executors;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;

namespace Microsoft.Identity.Client;

public sealed class AcquireTokenByUsernamePasswordParameterBuilder : AbstractPublicClientAcquireTokenParameterBuilder<AcquireTokenByUsernamePasswordParameterBuilder>
{
	private AcquireTokenByUsernamePasswordParameters Parameters { get; } = new AcquireTokenByUsernamePasswordParameters();


	internal override ApiTelemetryId ApiTelemetryId => ApiTelemetryId.AcquireTokenByUsernamePassword;

	internal AcquireTokenByUsernamePasswordParameterBuilder(IPublicClientApplicationExecutor publicClientApplicationExecutor)
		: base(publicClientApplicationExecutor)
	{
	}

	internal static AcquireTokenByUsernamePasswordParameterBuilder Create(IPublicClientApplicationExecutor publicClientApplicationExecutor, IEnumerable<string> scopes, string username, SecureString password)
	{
		return new AcquireTokenByUsernamePasswordParameterBuilder(publicClientApplicationExecutor).WithScopes(scopes).WithUsername(username).WithPassword(password);
	}

	private AcquireTokenByUsernamePasswordParameterBuilder WithUsername(string username)
	{
		Parameters.Username = username;
		return this;
	}

	private AcquireTokenByUsernamePasswordParameterBuilder WithPassword(SecureString password)
	{
		Parameters.Password = password;
		return this;
	}

	internal override Task<AuthenticationResult> ExecuteInternalAsync(CancellationToken cancellationToken)
	{
		return base.PublicClientApplicationExecutor.ExecuteAsync(base.CommonParameters, Parameters, cancellationToken);
	}

	internal override ApiEvent.ApiIds CalculateApiEventId()
	{
		return ApiEvent.ApiIds.AcquireTokenByUsernamePassword;
	}
}

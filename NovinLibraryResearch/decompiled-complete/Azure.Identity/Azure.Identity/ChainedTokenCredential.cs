using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;

namespace Azure.Identity;

public class ChainedTokenCredential : TokenCredential
{
	private const string AggregateAllUnavailableErrorMessage = "The ChainedTokenCredential failed to retrieve a token from the included credentials.";

	private const string AuthenticationFailedErrorMessage = "The ChainedTokenCredential failed due to an unhandled exception: ";

	private readonly TokenCredential[] _sources;

	internal ChainedTokenCredential()
	{
		_sources = Array.Empty<TokenCredential>();
	}

	public ChainedTokenCredential(params TokenCredential[] sources)
	{
		if (sources == null)
		{
			throw new ArgumentNullException("sources");
		}
		if (sources.Length == 0)
		{
			throw new ArgumentException("sources must not be empty", "sources");
		}
		for (int i = 0; i < sources.Length; i++)
		{
			if (sources[i] == null)
			{
				throw new ArgumentException("sources must not contain null", "sources");
			}
		}
		_sources = sources;
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
		ScopeGroupHandler groupScopeHandler = new ScopeGroupHandler(null);
		try
		{
			List<CredentialUnavailableException> exceptions = new List<CredentialUnavailableException>();
			TokenCredential[] sources = _sources;
			foreach (TokenCredential tokenCredential in sources)
			{
				try
				{
					AccessToken result = ((!async) ? tokenCredential.GetToken(requestContext, cancellationToken) : (await tokenCredential.GetTokenAsync(requestContext, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)));
					Azure.Core.Pipeline.DiagnosticScope scope = default(Azure.Core.Pipeline.DiagnosticScope);
					groupScopeHandler.Dispose(null, in scope);
					return result;
				}
				catch (CredentialUnavailableException item)
				{
					exceptions.Add(item);
				}
				catch (Exception ex) when (!cancellationToken.IsCancellationRequested)
				{
					throw new AuthenticationFailedException("The ChainedTokenCredential failed due to an unhandled exception: " + ex.Message, ex);
				}
			}
			throw CredentialUnavailableException.CreateAggregateException("The ChainedTokenCredential failed to retrieve a token from the included credentials.", exceptions);
		}
		catch (Exception exception)
		{
			Azure.Core.Pipeline.DiagnosticScope scope = default(Azure.Core.Pipeline.DiagnosticScope);
			groupScopeHandler.Fail(null, in scope, exception);
			throw;
		}
	}
}

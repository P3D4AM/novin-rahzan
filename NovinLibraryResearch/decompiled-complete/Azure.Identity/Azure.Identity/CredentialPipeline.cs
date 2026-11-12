using System;
using Azure.Core;
using Azure.Core.Pipeline;
using Microsoft.Identity.Client;

namespace Azure.Identity;

internal class CredentialPipeline
{
	private class CredentialResponseClassifier : ResponseClassifier
	{
		public override bool IsRetriableResponse(HttpMessage message)
		{
			if (!base.IsRetriableResponse(message))
			{
				return message.Response.Status == 404;
			}
			return true;
		}
	}

	private class ScopeHandler : IScopeHandler
	{
		public Azure.Core.Pipeline.DiagnosticScope CreateScope(Azure.Core.Pipeline.ClientDiagnostics diagnostics, string name)
		{
			return diagnostics.CreateScope(name);
		}

		public void Start(string name, in Azure.Core.Pipeline.DiagnosticScope scope)
		{
			scope.Start();
		}

		public void Dispose(string name, in Azure.Core.Pipeline.DiagnosticScope scope)
		{
			scope.Dispose();
		}

		public void Fail(string name, in Azure.Core.Pipeline.DiagnosticScope scope, Exception exception)
		{
			scope.Failed(exception);
		}

		void IScopeHandler.Start(string name, in Azure.Core.Pipeline.DiagnosticScope scope)
		{
			Start(name, in scope);
		}

		void IScopeHandler.Dispose(string name, in Azure.Core.Pipeline.DiagnosticScope scope)
		{
			Dispose(name, in scope);
		}

		void IScopeHandler.Fail(string name, in Azure.Core.Pipeline.DiagnosticScope scope, Exception exception)
		{
			Fail(name, in scope, exception);
		}
	}

	private static readonly Lazy<CredentialPipeline> s_singleton = new Lazy<CredentialPipeline>(() => new CredentialPipeline(new TokenCredentialOptions()));

	private static readonly IScopeHandler _defaultScopeHandler = new ScopeHandler();

	public Uri AuthorityHost { get; }

	public HttpPipeline HttpPipeline { get; }

	public Azure.Core.Pipeline.ClientDiagnostics Diagnostics { get; }

	private CredentialPipeline(TokenCredentialOptions options)
	{
		AuthorityHost = options.AuthorityHost;
		HttpPipeline = HttpPipelineBuilder.Build(options, Array.Empty<HttpPipelinePolicy>(), Array.Empty<HttpPipelinePolicy>(), new CredentialResponseClassifier());
		Diagnostics = new Azure.Core.Pipeline.ClientDiagnostics(options);
	}

	public CredentialPipeline(Uri authorityHost, HttpPipeline httpPipeline, Azure.Core.Pipeline.ClientDiagnostics diagnostics)
	{
		AuthorityHost = authorityHost;
		HttpPipeline = httpPipeline;
		Diagnostics = diagnostics;
	}

	public static CredentialPipeline GetInstance(TokenCredentialOptions options)
	{
		if (options != null)
		{
			return new CredentialPipeline(options);
		}
		return s_singleton.Value;
	}

	public IConfidentialClientApplication CreateMsalConfidentialClient(string tenantId, string clientId, string clientSecret)
	{
		return ConfidentialClientApplicationBuilder.Create(clientId).WithHttpClientFactory(new HttpPipelineClientFactory(HttpPipeline)).WithTenantId(tenantId)
			.WithClientSecret(clientSecret)
			.Build();
	}

	public CredentialDiagnosticScope StartGetTokenScope(string fullyQualifiedMethod, TokenRequestContext context)
	{
		IScopeHandler scopeHandler = ScopeGroupHandler.Current ?? _defaultScopeHandler;
		CredentialDiagnosticScope result = new CredentialDiagnosticScope(Diagnostics, fullyQualifiedMethod, context, scopeHandler);
		result.Start();
		return result;
	}

	public CredentialDiagnosticScope StartGetTokenScopeGroup(string fullyQualifiedMethod, TokenRequestContext context)
	{
		ScopeGroupHandler scopeHandler = new ScopeGroupHandler(fullyQualifiedMethod);
		CredentialDiagnosticScope result = new CredentialDiagnosticScope(Diagnostics, fullyQualifiedMethod, context, scopeHandler);
		result.Start();
		return result;
	}
}

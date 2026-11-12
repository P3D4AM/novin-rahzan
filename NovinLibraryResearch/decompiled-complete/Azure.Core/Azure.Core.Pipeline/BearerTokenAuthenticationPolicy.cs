using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core.Diagnostics;

namespace Azure.Core.Pipeline;

public class BearerTokenAuthenticationPolicy : HttpPipelinePolicy
{
	private class AccessTokenCache
	{
		private readonly struct HeaderValueInfo
		{
			public string HeaderValue { get; }

			public DateTimeOffset ExpiresOn { get; }

			public DateTimeOffset RefreshOn { get; }

			public HeaderValueInfo(string headerValue, DateTimeOffset expiresOn, DateTimeOffset refreshOn)
			{
				HeaderValue = headerValue;
				ExpiresOn = expiresOn;
				RefreshOn = refreshOn;
			}
		}

		private readonly object _syncObj = new object();

		private readonly TokenCredential _credential;

		private readonly string[] _scopes;

		private readonly TimeSpan _tokenRefreshOffset;

		private readonly TimeSpan _tokenRefreshRetryDelay;

		private TaskCompletionSource<HeaderValueInfo>? _infoTcs;

		private TaskCompletionSource<HeaderValueInfo>? _backgroundUpdateTcs;

		public AccessTokenCache(TokenCredential credential, string[] scopes, TimeSpan tokenRefreshOffset, TimeSpan tokenRefreshRetryDelay)
		{
			_credential = credential;
			_scopes = scopes;
			_tokenRefreshOffset = tokenRefreshOffset;
			_tokenRefreshRetryDelay = tokenRefreshRetryDelay;
		}

		public async ValueTask<string> GetHeaderValueAsync(HttpMessage message, bool async)
		{
			HttpMessage message2 = message;
			(TaskCompletionSource<HeaderValueInfo>, TaskCompletionSource<HeaderValueInfo>, bool) taskCompletionSources = GetTaskCompletionSources();
			var (headerValueTcs, backgroundUpdateTcs, _) = taskCompletionSources;
			if (taskCompletionSources.Item3)
			{
				if (backgroundUpdateTcs != null)
				{
					HeaderValueInfo info = headerValueTcs.Task.EnsureCompleted();
					Task.Run(() => GetHeaderValueFromCredentialInBackgroundAsync(backgroundUpdateTcs, info, message2, async));
					return info.HeaderValue;
				}
				try
				{
					headerValueTcs.SetResult(await GetHeaderValueFromCredentialAsync(message2, async, message2.CancellationToken));
				}
				catch (OperationCanceledException)
				{
					headerValueTcs.SetCanceled();
					throw;
				}
				catch (Exception exception)
				{
					headerValueTcs.SetException(exception);
					throw;
				}
			}
			Task<HeaderValueInfo> task = headerValueTcs.Task;
			if (!task.IsCompleted)
			{
				if (async)
				{
					await task.AwaitWithCancellation(message2.CancellationToken);
				}
				else
				{
					try
					{
						task.Wait(message2.CancellationToken);
					}
					catch (AggregateException)
					{
					}
				}
			}
			return headerValueTcs.Task.EnsureCompleted().HeaderValue;
		}

		private (TaskCompletionSource<HeaderValueInfo> tcs, TaskCompletionSource<HeaderValueInfo>? backgroundUpdateTcs, bool getTokenFromCredential) GetTaskCompletionSources()
		{
			lock (_syncObj)
			{
				if (_infoTcs == null)
				{
					_infoTcs = new TaskCompletionSource<HeaderValueInfo>(TaskCreationOptions.RunContinuationsAsynchronously);
					return (tcs: _infoTcs, backgroundUpdateTcs: null, getTokenFromCredential: true);
				}
				if (!_infoTcs.Task.IsCompleted)
				{
					_backgroundUpdateTcs = null;
					return (tcs: _infoTcs, backgroundUpdateTcs: _backgroundUpdateTcs, getTokenFromCredential: false);
				}
				DateTimeOffset utcNow = DateTimeOffset.UtcNow;
				if (_backgroundUpdateTcs != null && _backgroundUpdateTcs.Task.Status == TaskStatus.RanToCompletion && _backgroundUpdateTcs.Task.Result.ExpiresOn > utcNow)
				{
					_infoTcs = _backgroundUpdateTcs;
					_backgroundUpdateTcs = null;
				}
				if (_infoTcs.Task.Status != TaskStatus.RanToCompletion || utcNow >= _infoTcs.Task.Result.ExpiresOn)
				{
					_infoTcs = new TaskCompletionSource<HeaderValueInfo>(TaskCreationOptions.RunContinuationsAsynchronously);
					return (tcs: _infoTcs, backgroundUpdateTcs: null, getTokenFromCredential: true);
				}
				if (utcNow >= _infoTcs.Task.Result.RefreshOn && _backgroundUpdateTcs == null)
				{
					_backgroundUpdateTcs = new TaskCompletionSource<HeaderValueInfo>(TaskCreationOptions.RunContinuationsAsynchronously);
					return (tcs: _infoTcs, backgroundUpdateTcs: _backgroundUpdateTcs, getTokenFromCredential: true);
				}
				return (tcs: _infoTcs, backgroundUpdateTcs: null, getTokenFromCredential: false);
			}
		}

		private async ValueTask GetHeaderValueFromCredentialInBackgroundAsync(TaskCompletionSource<HeaderValueInfo> backgroundUpdateTcs, HeaderValueInfo info, HttpMessage httpMessage, bool async)
		{
			CancellationTokenSource cts = new CancellationTokenSource(_tokenRefreshRetryDelay);
			try
			{
				backgroundUpdateTcs.SetResult(await GetHeaderValueFromCredentialAsync(httpMessage, async, cts.Token));
			}
			catch (OperationCanceledException ex) when (cts.IsCancellationRequested)
			{
				backgroundUpdateTcs.SetResult(new HeaderValueInfo(info.HeaderValue, info.ExpiresOn, DateTimeOffset.UtcNow));
				AzureCoreEventSource.Singleton.BackgroundRefreshFailed(httpMessage.Request.ClientRequestId, ex.ToString());
			}
			catch (Exception ex2)
			{
				backgroundUpdateTcs.SetResult(new HeaderValueInfo(info.HeaderValue, info.ExpiresOn, DateTimeOffset.UtcNow + _tokenRefreshRetryDelay));
				AzureCoreEventSource.Singleton.BackgroundRefreshFailed(httpMessage.Request.ClientRequestId, ex2.ToString());
			}
			finally
			{
				cts.Dispose();
			}
		}

		private async ValueTask<HeaderValueInfo> GetHeaderValueFromCredentialAsync(HttpMessage message, bool async, CancellationToken cancellationToken)
		{
			TokenRequestContext requestContext = new TokenRequestContext(_scopes, message.Request.ClientRequestId);
			AccessToken accessToken = ((!async) ? _credential.GetToken(requestContext, cancellationToken) : (await _credential.GetTokenAsync(requestContext, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)));
			AccessToken accessToken2 = accessToken;
			return new HeaderValueInfo("Bearer " + accessToken2.Token, accessToken2.ExpiresOn, accessToken2.ExpiresOn - _tokenRefreshOffset);
		}
	}

	private readonly AccessTokenCache _accessTokenCache;

	public BearerTokenAuthenticationPolicy(TokenCredential credential, string scope)
		: this(credential, new string[1] { scope })
	{
	}

	public BearerTokenAuthenticationPolicy(TokenCredential credential, IEnumerable<string> scopes)
		: this(credential, scopes, TimeSpan.FromMinutes(5.0), TimeSpan.FromSeconds(30.0))
	{
	}

	internal BearerTokenAuthenticationPolicy(TokenCredential credential, IEnumerable<string> scopes, TimeSpan tokenRefreshOffset, TimeSpan tokenRefreshRetryDelay)
	{
		Argument.AssertNotNull(credential, "credential");
		Argument.AssertNotNull(scopes, "scopes");
		_accessTokenCache = new AccessTokenCache(credential, scopes.ToArray(), tokenRefreshOffset, tokenRefreshRetryDelay);
	}

	public override ValueTask ProcessAsync(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
	{
		return ProcessAsync(message, pipeline, async: true);
	}

	public override void Process(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
	{
		ProcessAsync(message, pipeline, async: false).EnsureCompleted();
	}

	private async ValueTask ProcessAsync(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline, bool async)
	{
		if (message.Request.Uri.Scheme != Uri.UriSchemeHttps)
		{
			throw new InvalidOperationException("Bearer token authentication is not permitted for non TLS protected (https) endpoints.");
		}
		string value = await _accessTokenCache.GetHeaderValueAsync(message, async);
		message.Request.SetHeader(HttpHeader.Names.Authorization, value);
		if (async)
		{
			await HttpPipelinePolicy.ProcessNextAsync(message, pipeline).ConfigureAwait(continueOnCapturedContext: false);
		}
		else
		{
			HttpPipelinePolicy.ProcessNext(message, pipeline);
		}
	}
}

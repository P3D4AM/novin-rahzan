using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.Http;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.UI;

namespace Microsoft.Identity.Client.Platforms.net45;

internal abstract class WebUI : IWebUI
{
	protected Uri RequestUri { get; private set; }

	protected Uri CallbackUri { get; private set; }

	public object OwnerWindow { get; set; }

	protected SynchronizationContext SynchronizationContext { get; set; }

	public RequestContext RequestContext { get; set; }

	public async Task<AuthorizationResult> AcquireAuthorizationAsync(Uri authorizationUri, Uri redirectUri, RequestContext requestContext, CancellationToken cancellationToken)
	{
		AuthorizationResult authorizationResult = null;
		Action action = delegate
		{
			authorizationResult = Authenticate(authorizationUri, redirectUri);
		};
		Action<object> @object = delegate(object tcs)
		{
			try
			{
				authorizationResult = Authenticate(authorizationUri, redirectUri);
				((TaskCompletionSource<object>)tcs).TrySetResult(null);
			}
			catch (Exception exception)
			{
				((TaskCompletionSource<object>)tcs).TrySetException(exception);
			}
		};
		if (Thread.CurrentThread.GetApartmentState() == ApartmentState.MTA)
		{
			if (SynchronizationContext != null)
			{
				TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
				SynchronizationContext.Post(@object.Invoke, taskCompletionSource);
				await taskCompletionSource.Task.ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				using StaTaskScheduler scheduler = new StaTaskScheduler(1);
				try
				{
					Task.Factory.StartNew(action, cancellationToken, TaskCreationOptions.None, scheduler).Wait();
				}
				catch (AggregateException ex)
				{
					requestContext.Logger.ErrorPii(ex.InnerException);
					Exception ex2 = ex.InnerExceptions[0];
					if (ex2 is AggregateException)
					{
						ex2 = ((AggregateException)ex2).InnerExceptions[0];
					}
					throw ex2;
				}
			}
		}
		else
		{
			action();
		}
		return await Task.Factory.StartNew(() => authorizationResult).ConfigureAwait(continueOnCapturedContext: false);
	}

	internal AuthorizationResult Authenticate(Uri requestUri, Uri callbackUri)
	{
		RequestUri = requestUri;
		CallbackUri = callbackUri;
		return OnAuthenticate();
	}

	protected abstract AuthorizationResult OnAuthenticate();

	public Uri UpdateRedirectUri(Uri redirectUri)
	{
		RedirectUriHelper.Validate(redirectUri);
		return redirectUri;
	}
}

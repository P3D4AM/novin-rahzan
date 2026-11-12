using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.Extensibility;
using Microsoft.Identity.Client.Http;
using Microsoft.Identity.Client.Internal;

namespace Microsoft.Identity.Client.UI;

internal class CustomWebUiHandler : IWebUI
{
	private readonly ICustomWebUi _customWebUi;

	public CustomWebUiHandler(ICustomWebUi customWebUi)
	{
		_customWebUi = customWebUi;
	}

	public async Task<AuthorizationResult> AcquireAuthorizationAsync(Uri authorizationUri, Uri redirectUri, RequestContext requestContext, CancellationToken cancellationToken)
	{
		requestContext.Logger.Info("Using CustomWebUi to acquire the authorization code");
		try
		{
			requestContext.Logger.InfoPii(LogMessages.CustomWebUiCallingAcquireAuthorizationCodePii(authorizationUri, redirectUri), "Calling CustomWebUi.AcquireAuthorizationCode");
			Uri uri = await _customWebUi.AcquireAuthorizationCodeAsync(authorizationUri, redirectUri, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (uri == null || string.IsNullOrWhiteSpace(uri.Query))
			{
				throw new MsalClientException("custom_webui_returned_invalid_uri", "ICustomWebUi returned an invalid uri - it is empty or has no query.");
			}
			if (uri.Authority.Equals(redirectUri.Authority, StringComparison.OrdinalIgnoreCase) && uri.AbsolutePath.Equals(redirectUri.AbsolutePath))
			{
				requestContext.Logger.Info("Redirect Uri was matched.  Returning success from CustomWebUiHandler.");
				return AuthorizationResult.FromUri(uri.OriginalString);
			}
			throw new MsalClientException("custom_webui_invalid_mismatch", MsalErrorMessage.RedirectUriMismatch(uri.AbsolutePath, redirectUri.AbsolutePath));
		}
		catch (OperationCanceledException)
		{
			requestContext.Logger.Info("CustomWebUi AcquireAuthorizationCode was canceled");
			return AuthorizationResult.FromStatus(AuthorizationStatus.UserCancel);
		}
		catch (Exception exWithPii)
		{
			requestContext.Logger.WarningPiiWithPrefix(exWithPii, "CustomWebUi AcquireAuthorizationCode failed");
			throw;
		}
	}

	public Uri UpdateRedirectUri(Uri redirectUri)
	{
		RedirectUriHelper.Validate(redirectUri);
		return redirectUri;
	}
}

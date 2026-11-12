using System;
using System.Globalization;
using Microsoft.Identity.Client.Internal;

namespace Microsoft.Identity.Client.ApiConfig.Executors;

internal abstract class AbstractExecutor
{
	private readonly ClientApplicationBase _clientApplicationBase;

	public IServiceBundle ServiceBundle { get; }

	protected AbstractExecutor(IServiceBundle serviceBundle, ClientApplicationBase clientApplicationBase)
	{
		ServiceBundle = serviceBundle;
		_clientApplicationBase = clientApplicationBase;
	}

	protected RequestContext CreateRequestContextAndLogVersionInfo(Guid correlationId)
	{
		RequestContext requestContext = new RequestContext(ServiceBundle, correlationId);
		requestContext.Logger.Info(string.Format(CultureInfo.InvariantCulture, "MSAL {0} with assembly version '{1}'. CorrelationId({2})", ServiceBundle.PlatformProxy.GetProductName(), MsalIdHelper.GetMsalVersion(), requestContext.CorrelationId));
		return requestContext;
	}
}

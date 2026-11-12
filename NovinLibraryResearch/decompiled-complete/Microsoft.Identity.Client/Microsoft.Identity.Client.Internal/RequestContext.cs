using System;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.Internal.Logger;
using Microsoft.Identity.Client.TelemetryCore;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;

namespace Microsoft.Identity.Client.Internal;

internal class RequestContext
{
	public Guid CorrelationId { get; }

	public ICoreLogger Logger { get; }

	public IServiceBundle ServiceBundle { get; }

	public ApiEvent ApiEvent { get; set; }

	public RequestContext(IServiceBundle serviceBundle, Guid correlationId)
	{
		ServiceBundle = serviceBundle ?? throw new ArgumentNullException("serviceBundle");
		Logger = MsalLogger.Create(correlationId, ServiceBundle.Config);
		CorrelationId = correlationId;
	}

	public TelemetryHelper CreateTelemetryHelper(EventBase eventToStart)
	{
		return new TelemetryHelper(ServiceBundle.MatsTelemetryManager, ServiceBundle.HttpTelemetryManager, eventToStart);
	}
}

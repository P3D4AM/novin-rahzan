using System;
using System.Collections.Generic;
using Microsoft.Identity.Client.PlatformsCommon.Interfaces;

namespace Microsoft.Identity.Client.TelemetryCore.Internal;

internal class TelemetryUploader : IUploader
{
	private readonly Action<ITelemetryEventPayload> _dispatchAction;

	private readonly IPlatformProxy _platformProxy;

	public string AppName { get; }

	public TelemetryUploader(Action<ITelemetryEventPayload> dispatchAction, IPlatformProxy platformProxy, string appName)
	{
		_dispatchAction = dispatchAction;
		_platformProxy = platformProxy;
		AppName = appName;
	}

	public void Upload(IEnumerable<PropertyBagContents> uploadEvents)
	{
		if (_dispatchAction == null)
		{
			return;
		}
		foreach (PropertyBagContents uploadEvent in uploadEvents)
		{
			string uploadEventName = UploadEventUtils.GetUploadEventName(_platformProxy, uploadEvent.EventType, AppName);
			_dispatchAction(TelemetryEventPayload.Create(uploadEventName, uploadEvent));
		}
	}
}

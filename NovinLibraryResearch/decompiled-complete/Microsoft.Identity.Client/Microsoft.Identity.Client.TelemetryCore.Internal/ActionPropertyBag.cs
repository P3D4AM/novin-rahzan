namespace Microsoft.Identity.Client.TelemetryCore.Internal;

internal class ActionPropertyBag : PropertyBag
{
	private bool _isAggregable;

	private bool _readyForUpload;

	private readonly object _lockObj = new object();

	public bool IsAggregable
	{
		get
		{
			lock (_lockObj)
			{
				return _isAggregable;
			}
		}
		set
		{
			lock (_lockObj)
			{
				_isAggregable = value;
			}
		}
	}

	public bool ReadyForUpload
	{
		get
		{
			lock (_lockObj)
			{
				return _readyForUpload;
			}
		}
		set
		{
			lock (_lockObj)
			{
				_readyForUpload = value;
			}
		}
	}

	public ActionPropertyBag(IErrorStore errorStore)
		: base(EventType.Action, errorStore)
	{
	}
}

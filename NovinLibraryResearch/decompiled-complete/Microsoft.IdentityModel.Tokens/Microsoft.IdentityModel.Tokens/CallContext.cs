using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.IdentityModel.Tokens;

public class CallContext
{
	public Guid ActivityId { get; set; } = Guid.Empty;


	public bool CaptureLogs { get; set; }

	public ICollection<string> Logs { get; private set; } = new Collection<string>();


	public IDictionary<string, object> PropertyBag { get; set; }

	public CallContext()
	{
	}

	public CallContext(Guid activityId)
	{
		ActivityId = activityId;
	}
}

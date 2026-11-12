using System.Collections.Generic;

namespace Microsoft.Identity.Client.TelemetryCore.Internal;

internal interface IUploader
{
	string AppName { get; }

	void Upload(IEnumerable<PropertyBagContents> uploadEvents);
}

using System.Collections.Generic;
using Microsoft.Identity.Client.Core;

namespace Microsoft.Identity.Client.Instance.Discovery;

internal interface IKnownMetadataProvider
{
	InstanceDiscoveryMetadataEntry GetMetadata(string environment, IEnumerable<string> existingEnvironmentsInCache, ICoreLogger logger);
}

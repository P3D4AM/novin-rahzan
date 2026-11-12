using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Identity.Client.Internal;

namespace Microsoft.Identity.Client.Instance.Discovery;

internal interface IInstanceDiscoveryManager
{
	Task<InstanceDiscoveryMetadataEntry> GetMetadataEntryTryAvoidNetworkAsync(string authority, IEnumerable<string> existingEnvironmentsInCache, RequestContext requestContext);

	Task<InstanceDiscoveryMetadataEntry> GetMetadataEntryAsync(string authority, RequestContext requestContext);
}

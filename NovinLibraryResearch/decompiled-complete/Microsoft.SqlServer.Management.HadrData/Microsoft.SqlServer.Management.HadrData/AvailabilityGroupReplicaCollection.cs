using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.HadrData;

internal class AvailabilityGroupReplicaCollection : SortableBindingList<AvailabilityGroupReplica>
{
	public AvailabilityGroupReplicaCollection()
	{
	}

	public AvailabilityGroupReplicaCollection(IList<AvailabilityGroupReplica> replicaList)
		: base(replicaList)
	{
	}
}

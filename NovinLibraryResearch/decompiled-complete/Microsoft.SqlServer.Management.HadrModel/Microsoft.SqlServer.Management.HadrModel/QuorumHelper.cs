using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

internal class QuorumHelper
{
	public static bool ValidateQuorumVoteConfiguration(AvailabilityGroup group)
	{
		if (group == null)
		{
			throw new ArgumentNullException("AvailabilityGroup");
		}
		string primaryReplicaServerName = group.PrimaryReplicaServerName;
		AvailabilityReplica availabilityReplica = null;
		if (!string.IsNullOrEmpty(primaryReplicaServerName))
		{
			availabilityReplica = group.AvailabilityReplicas[primaryReplicaServerName];
		}
		if (availabilityReplica == null)
		{
			throw new ArgumentNullException("primaryReplica");
		}
		bool flag = availabilityReplica.FailoverMode == AvailabilityReplicaFailoverMode.Automatic;
		DataTable table = group.EnumReplicaClusterNodes();
		if (!AllNodesHaveQuorumVote(availabilityReplica.Name, table))
		{
			throw new QuorumHelperException(availabilityReplica.Name);
		}
		foreach (AvailabilityReplica availabilityReplica2 in group.AvailabilityReplicas)
		{
			if (string.Compare(availabilityReplica2.Name, availabilityReplica.Name, StringComparison.OrdinalIgnoreCase) != 0 && flag && availabilityReplica2.FailoverMode == AvailabilityReplicaFailoverMode.Automatic && !AllNodesHaveQuorumVote(availabilityReplica2.Name, table))
			{
				throw new QuorumHelperException(availabilityReplica2.Name);
			}
		}
		return true;
	}

	private static bool AllNodesHaveQuorumVote(string replica, DataTable table)
	{
		return (from row in GetReplicaNodes(replica, table)
			select row["NumberOfQuorumVotes"]).All((object value) => value != null && value is int && (int)value > 0);
	}

	private static bool AnyNodesHaveQuorumVote(string replica, DataTable table)
	{
		return (from row in GetReplicaNodes(replica, table)
			select row["NumberOfQuorumVotes"]).Any((object value) => value != null && value is int && (int)value > 0);
	}

	private static IEnumerable<DataRow> GetReplicaNodes(string targetReplicaName, DataTable table)
	{
		if (table == null)
		{
			yield break;
		}
		foreach (DataRow row in table.Rows)
		{
			string strB = row["ReplicaName"] as string;
			if (string.Compare(targetReplicaName, strB, StringComparison.OrdinalIgnoreCase) == 0)
			{
				yield return row;
			}
		}
	}
}

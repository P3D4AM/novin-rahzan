using System;
using System.Globalization;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

public class WaitForAvailabilityGroupOnlineTask : HadrTask
{
	private readonly AvailabilityGroupData availabilityGroupData;

	public WaitForAvailabilityGroupOnlineTask(AvailabilityGroupData availabilityGroupData)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.WaitForAvailabilityGroupOnlineText, availabilityGroupData.GroupName))
	{
		if (availabilityGroupData == null)
		{
			throw new ArgumentNullException("availabilityGroupData");
		}
		this.availabilityGroupData = availabilityGroupData;
	}

	protected override void Perform(IExecutionPolicy policy)
	{
		UpdateStatus(new TaskEventArgs(base.Name, string.Format(Resource.WaitForAvailabilityGroupOnline, availabilityGroupData.GroupName), TaskEventStatus.Running));
		AvailabilityGroup availabilityGroup = availabilityGroupData.PrimaryServer.AvailabilityGroups[availabilityGroupData.GroupName];
		if (availabilityGroup != null)
		{
			availabilityGroup.Refresh();
			policy.Expired = !string.IsNullOrEmpty(availabilityGroup.PrimaryReplicaServerName);
		}
	}

	protected override void Rollback(IExecutionPolicy policy)
	{
		throw new NotSupportedException();
	}
}

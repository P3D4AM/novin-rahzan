using System;
using Microsoft.SqlServer.Management.HadrData;

namespace Microsoft.SqlServer.Management.HadrModel;

public class ListenerConfigurationValidator : Validator
{
	private readonly AvailabilityGroupData availabilityGroupData;

	public ListenerConfigurationValidator(AvailabilityGroupData availabilityGroupData)
		: base(Resource.CheckingListenerConfiguration)
	{
		if (availabilityGroupData == null)
		{
			throw new ArgumentNullException("availabilityGroupData");
		}
		this.availabilityGroupData = availabilityGroupData;
	}

	protected override void Validate(IExecutionPolicy policy)
	{
		if (availabilityGroupData.AvailabilityGroupListener == null)
		{
			throw new ListenerConfigurationException();
		}
	}
}

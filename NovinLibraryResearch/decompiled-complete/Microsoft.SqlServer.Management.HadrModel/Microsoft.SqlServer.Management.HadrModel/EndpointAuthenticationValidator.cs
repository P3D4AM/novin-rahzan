using System;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

public class EndpointAuthenticationValidator : Validator
{
	private AvailabilityGroupData availabilityGroupData;

	public EndpointAuthenticationValidator(AvailabilityGroupData agData)
		: base("Checking if existing endpoints support Windows authentication")
	{
		if (agData == null)
		{
			throw new ArgumentNullException("AvailabilityGroupData");
		}
		availabilityGroupData = agData;
	}

	protected override void Validate(IExecutionPolicy policy)
	{
		if (availabilityGroupData.AvailabilityGroupReplicas.Count < 1)
		{
			policy.Expired = true;
		}
		foreach (AvailabilityGroupReplica availabilityGroupReplica in availabilityGroupData.AvailabilityGroupReplicas)
		{
			Microsoft.SqlServer.Management.Smo.Server server = availabilityGroupReplica.GetServer();
			if (server == null || server.ConnectionContext == null)
			{
				break;
			}
			foreach (Endpoint endpoint in server.Endpoints)
			{
				if (endpoint.EndpointType == EndpointType.DatabaseMirroring)
				{
					EndpointAuthenticationOrder endpointAuthenticationOrder = endpoint.Payload.DatabaseMirroring.EndpointAuthenticationOrder;
					if (endpointAuthenticationOrder == EndpointAuthenticationOrder.Certificate)
					{
						policy.Expired = true;
						throw new EndpointAuthenticationValidationException(availabilityGroupReplica.EndpointName, endpointAuthenticationOrder.ToString());
					}
					break;
				}
			}
			policy.Expired = true;
		}
	}
}

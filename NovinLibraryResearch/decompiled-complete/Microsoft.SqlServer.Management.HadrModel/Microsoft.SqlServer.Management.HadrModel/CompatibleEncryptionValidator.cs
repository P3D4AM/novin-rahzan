using System;
using System.Text;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

public class CompatibleEncryptionValidator : Validator
{
	private AvailabilityGroupData AvailabilityGroupData { get; set; }

	public CompatibleEncryptionValidator(AvailabilityGroupData data)
		: base(Resource.ValidatingEndpointEncryption)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		AvailabilityGroupData = data;
	}

	protected override void Validate(IExecutionPolicy policy)
	{
		policy.Expired = true;
		EndpointEncryption encryption = AvailabilityGroupData.AvailabilityGroupReplicas[0].EndpointEncryption;
		EndpointEncryptionAlgorithm endpointEncryptionAlgorithm = AvailabilityGroupData.AvailabilityGroupReplicas[0].EndpointEncryptionAlgorithm;
		foreach (AvailabilityGroupReplica availabilityGroupReplica in AvailabilityGroupData.AvailabilityGroupReplicas)
		{
			encryption = ValidateReplicaEndpointEncryption(availabilityGroupReplica, encryption);
			ValidateReplicaEndpointEncryptionAlgorithm(availabilityGroupReplica, endpointEncryptionAlgorithm);
		}
	}

	private void ValidateReplicaEndpointEncryptionAlgorithm(AvailabilityGroupReplica replica, EndpointEncryptionAlgorithm encryptionAlgorithm)
	{
		if (replica.EndpointEncryptionAlgorithm != EndpointEncryptionAlgorithm.AesRC4 && replica.EndpointEncryptionAlgorithm != EndpointEncryptionAlgorithm.RC4Aes)
		{
			if (replica.EndpointEncryptionAlgorithm == EndpointEncryptionAlgorithm.Aes && encryptionAlgorithm == EndpointEncryptionAlgorithm.RC4)
			{
				throw new EncryptionAlgorithmMismatchException(GetReplicasString());
			}
			if (replica.EndpointEncryptionAlgorithm == EndpointEncryptionAlgorithm.RC4 && encryptionAlgorithm == EndpointEncryptionAlgorithm.Aes)
			{
				throw new EncryptionAlgorithmMismatchException(GetReplicasString());
			}
		}
	}

	private EndpointEncryption ValidateReplicaEndpointEncryption(AvailabilityGroupReplica replica, EndpointEncryption encryption)
	{
		if (encryption == EndpointEncryption.Required && replica.EndpointEncryption == EndpointEncryption.Disabled)
		{
			throw new EncryptionMismatchException(GetReplicasString());
		}
		if (encryption == EndpointEncryption.Disabled && replica.EndpointEncryption == EndpointEncryption.Required)
		{
			throw new EncryptionMismatchException(GetReplicasString());
		}
		if (encryption == EndpointEncryption.Supported && replica.EndpointEncryption != EndpointEncryption.Supported)
		{
			encryption = replica.EndpointEncryption;
		}
		return encryption;
	}

	private string GetReplicasString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (AvailabilityGroupReplica availabilityGroupReplica in AvailabilityGroupData.AvailabilityGroupReplicas)
		{
			LocalizableEnumConverter localizableEnumConverter = new LocalizableEnumConverter(typeof(EndpointEncryptionAlgorithm));
			LocalizableEnumConverter localizableEnumConverter2 = new LocalizableEnumConverter(typeof(EndpointEncryption));
			stringBuilder.Append(string.Format(Resource.ReplicaEndpointStringOutputFormat, availabilityGroupReplica.EndpointName, availabilityGroupReplica.InitialRoleString, localizableEnumConverter2.ConvertToString(availabilityGroupReplica.EndpointEncryption), localizableEnumConverter.ConvertToString(availabilityGroupReplica.EndpointEncryptionAlgorithm)));
		}
		return stringBuilder.ToString();
	}
}

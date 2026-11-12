using System;
using Microsoft.SqlServer.Management.HadrData;

namespace Microsoft.SqlServer.Management.HadrModel;

public class DatabaseMasterKeyValidator : Validator
{
	private bool validateExistingDbMode;

	public readonly AvailabilityGroupData availabilityGroupData;

	public AvailabilityGroupData AvailabilityGroupData => availabilityGroupData;

	public bool ValidateExistingDbMode
	{
		get
		{
			return validateExistingDbMode;
		}
		set
		{
			validateExistingDbMode = value;
		}
	}

	public DatabaseMasterKeyValidator(AvailabilityGroupData availabilityGroupData)
		: base(Resource.ValidatingDatabaseMasterKey)
	{
		this.availabilityGroupData = availabilityGroupData;
	}

	protected override void Validate(IExecutionPolicy policy)
	{
		foreach (PrimaryDatabaseData item in validateExistingDbMode ? availabilityGroupData.ExistingAvailabilityDatabases : availabilityGroupData.NewAvailabilityDatabases)
		{
			if (availabilityGroupData.PrimaryServer.Databases[item.Name].MasterKey != null && !FailoverUtilities.TryDecrypt(availabilityGroupData.PrimaryServer.Databases[item.Name], item.DBMKPassword))
			{
				throw new InvalidOperationException(Resource.DBMKPasswordIncorrect);
			}
		}
	}
}

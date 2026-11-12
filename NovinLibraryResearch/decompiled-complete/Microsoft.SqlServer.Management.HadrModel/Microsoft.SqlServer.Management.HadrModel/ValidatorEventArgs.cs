using System;

namespace Microsoft.SqlServer.Management.HadrModel;

public sealed class ValidatorEventArgs : EventArgs
{
	public string Name { get; set; }

	public string Status { get; set; }

	public string Details { get; set; }

	public ValidatorEventArgs(string validatorName, string validatorDetails, string validatorStatus)
	{
		Name = validatorName;
		Status = validatorStatus;
		Details = validatorDetails;
	}

	public ValidatorEventArgs(string validatorName, string validatorDetails)
	{
		Name = validatorName;
		Details = validatorDetails;
	}
}

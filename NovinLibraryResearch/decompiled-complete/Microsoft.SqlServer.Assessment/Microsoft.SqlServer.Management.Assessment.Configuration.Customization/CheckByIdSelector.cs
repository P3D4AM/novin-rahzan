using System;
using Microsoft.SqlServer.Management.Assessment.Checks;
using Microsoft.SqlServer.Management.Assessment.Configuration.Json;
using Microsoft.SqlServer.Management.Assessment.Properties;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Customization;

internal class CheckByIdSelector : CheckSelector
{
	public string CheckId { get; }

	public CheckByIdSelector(string checkId, JsonReader reader)
	{
		if (string.IsNullOrWhiteSpace(checkId))
		{
			throw SqlAssessmentJsonException.Create(reader, Resources.ErrorInvalidCheckId);
		}
		CheckId = checkId;
	}

	public override bool IsMatch(Check check)
	{
		return string.Compare(check.Id, CheckId, StringComparison.OrdinalIgnoreCase) == 0;
	}

	public override string ToString()
	{
		return "{" + CheckId + "}";
	}
}

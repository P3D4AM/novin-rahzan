using System;
using System.Globalization;
using Microsoft.SqlServer.Management.Assessment.Checks;
using Microsoft.SqlServer.Management.Assessment.Expressions;

namespace Microsoft.SqlServer.Management.Assessment.Configuration;

public class WarningCheck : Check, IQuasiCheck, ICheck
{
	public WarningCheck(string message)
	{
		base.Message = message;
		base.Condition = new Condition(new Literal<bool>(value: false));
		base.Id = Guid.NewGuid().ToString();
	}

	public string GetMessage(ISqlObjectLocator target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return string.Format(CultureInfo.CurrentCulture, base.Message, target.Type, target.Name);
	}

	internal override IAssessmentResult GetRecommendations(ISqlObjectLocator target, PermutationContext data)
	{
		string message = base.AssessmentMessage?.ToString(data) ?? string.Empty;
		return new AssessmentWarning(this, target?.Type ?? SqlObjectType.None, target?.Urn ?? string.Empty, message);
	}
}

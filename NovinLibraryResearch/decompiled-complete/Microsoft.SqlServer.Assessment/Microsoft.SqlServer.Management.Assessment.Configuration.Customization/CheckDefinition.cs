using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.Assessment.Checks;
using Microsoft.SqlServer.Management.Assessment.Logics;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Customization;

internal class CheckDefinition : CheckRule
{
	public string CheckId { get; }

	public CheckDefinition(string checkId, SqlObjectLocatorPattern target, IEnumerable<IItemChange<Check>> changes)
		: base(target, changes)
	{
		CheckId = checkId;
	}

	public Check CreateCheck(string rulesetName, Version rulesetVersion)
	{
		Check check = new Check
		{
			Id = CheckId,
			OriginName = rulesetName,
			OriginVersion = rulesetVersion,
			Logics = TwoPhaseLogics.Instance
		};
		ApplyTo(check);
		return check;
	}

	public bool IsMatch(ISqlObjectLocator target)
	{
		return base.Target.IsMatch(target);
	}

	public override void ApplyTo(Check target)
	{
		base.ApplyTo(target);
		target.Target = base.Target;
	}
}

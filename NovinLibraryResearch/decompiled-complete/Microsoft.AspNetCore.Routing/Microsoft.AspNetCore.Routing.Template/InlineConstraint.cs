using System;

namespace Microsoft.AspNetCore.Routing.Template;

public class InlineConstraint
{
	public string Constraint { get; }

	public InlineConstraint(string constraint)
	{
		if (constraint == null)
		{
			throw new ArgumentNullException("constraint");
		}
		Constraint = constraint;
	}
}

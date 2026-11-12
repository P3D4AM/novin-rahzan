using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Design;

public class NestedClosureCodeFragment
{
	public virtual string Parameter { get; }

	public virtual MethodCallCodeFragment MethodCall { get; }

	public NestedClosureCodeFragment([JetBrains.Annotations.NotNull] string parameter, [JetBrains.Annotations.NotNull] MethodCallCodeFragment methodCall)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(parameter, "parameter");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(methodCall, "methodCall");
		Parameter = parameter;
		MethodCall = methodCall;
	}
}

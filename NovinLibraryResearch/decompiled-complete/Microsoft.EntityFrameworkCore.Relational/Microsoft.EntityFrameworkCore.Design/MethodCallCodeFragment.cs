using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Design;

public class MethodCallCodeFragment
{
	private readonly List<object> _arguments;

	public virtual string Method { get; }

	public virtual IReadOnlyList<object> Arguments => _arguments;

	public virtual MethodCallCodeFragment ChainedCall { get; }

	public MethodCallCodeFragment([JetBrains.Annotations.NotNull] string method, [JetBrains.Annotations.NotNull] params object[] arguments)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(method, "method");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(arguments, "arguments");
		Method = method;
		_arguments = new List<object>(arguments);
	}

	public MethodCallCodeFragment([JetBrains.Annotations.NotNull] string method, [JetBrains.Annotations.NotNull] object[] arguments, [JetBrains.Annotations.NotNull] MethodCallCodeFragment chainedCall)
		: this(method, arguments)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(chainedCall, "chainedCall");
		ChainedCall = chainedCall;
	}

	public virtual MethodCallCodeFragment Chain([JetBrains.Annotations.NotNull] string method, [JetBrains.Annotations.NotNull] params object[] arguments)
	{
		return Chain(new MethodCallCodeFragment(method, arguments));
	}

	public virtual MethodCallCodeFragment Chain([JetBrains.Annotations.NotNull] MethodCallCodeFragment call)
	{
		return new MethodCallCodeFragment(Method, _arguments.ToArray(), ChainedCall?.Chain(call) ?? call);
	}
}

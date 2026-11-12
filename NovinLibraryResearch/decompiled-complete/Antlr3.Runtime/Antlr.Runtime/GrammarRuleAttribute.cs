using System;

namespace Antlr.Runtime;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class GrammarRuleAttribute : Attribute
{
	private readonly string _name;

	public string Name => _name;

	public GrammarRuleAttribute(string name)
	{
		_name = name;
	}
}

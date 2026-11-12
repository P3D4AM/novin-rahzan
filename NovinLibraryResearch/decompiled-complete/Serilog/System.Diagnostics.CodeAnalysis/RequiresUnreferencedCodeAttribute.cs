namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
[Conditional("MULTI_TARGETING_SUPPORT_ATTRIBUTES")]
internal sealed class RequiresUnreferencedCodeAttribute : Attribute
{
	public string Message { get; }

	public string? Url { get; set; }

	public RequiresUnreferencedCodeAttribute(string message)
	{
		Message = message;
	}
}

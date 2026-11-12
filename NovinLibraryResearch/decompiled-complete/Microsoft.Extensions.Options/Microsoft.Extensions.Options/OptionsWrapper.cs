using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.Options;

public class OptionsWrapper<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions> : IOptions<TOptions> where TOptions : class
{
	public TOptions Value { get; }

	public OptionsWrapper(TOptions options)
	{
		Value = options;
	}
}

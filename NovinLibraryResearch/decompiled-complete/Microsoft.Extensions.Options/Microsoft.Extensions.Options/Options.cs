using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.Options;

public static class Options
{
	internal const DynamicallyAccessedMemberTypes DynamicallyAccessedMembers = DynamicallyAccessedMemberTypes.PublicParameterlessConstructor;

	public static readonly string DefaultName = string.Empty;

	public static IOptions<TOptions> Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions>(TOptions options) where TOptions : class
	{
		return new OptionsWrapper<TOptions>(options);
	}
}

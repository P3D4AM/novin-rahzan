using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.Options;

public interface IOptionsSnapshot<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] out TOptions> : IOptions<TOptions> where TOptions : class
{
	TOptions Get(string? name);
}

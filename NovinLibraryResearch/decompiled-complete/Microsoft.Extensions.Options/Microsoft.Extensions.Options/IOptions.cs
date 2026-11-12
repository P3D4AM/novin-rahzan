using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.Options;

public interface IOptions<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] out TOptions> where TOptions : class
{
	TOptions Value { get; }
}

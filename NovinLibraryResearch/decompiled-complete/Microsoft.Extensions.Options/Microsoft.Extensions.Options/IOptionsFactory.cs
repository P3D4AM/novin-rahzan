using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.Options;

public interface IOptionsFactory<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions> where TOptions : class
{
	TOptions Create(string name);
}

using Microsoft.EntityFrameworkCore.Design;

namespace Microsoft.EntityFrameworkCore.Scaffolding;

public interface IProviderCodeGeneratorPlugin
{
	MethodCallCodeFragment GenerateProviderOptions();

	MethodCallCodeFragment GenerateContextOptions();
}

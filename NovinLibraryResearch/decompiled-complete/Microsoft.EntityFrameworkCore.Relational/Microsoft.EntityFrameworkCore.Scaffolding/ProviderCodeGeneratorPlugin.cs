using Microsoft.EntityFrameworkCore.Design;

namespace Microsoft.EntityFrameworkCore.Scaffolding;

public class ProviderCodeGeneratorPlugin : IProviderCodeGeneratorPlugin
{
	public virtual MethodCallCodeFragment GenerateContextOptions()
	{
		return null;
	}

	public virtual MethodCallCodeFragment GenerateProviderOptions()
	{
		return null;
	}
}

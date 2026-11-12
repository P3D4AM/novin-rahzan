using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Scaffolding;

public abstract class ProviderCodeGenerator : IProviderConfigurationCodeGenerator
{
	protected virtual ProviderCodeGeneratorDependencies Dependencies { get; }

	protected ProviderCodeGenerator([JetBrains.Annotations.NotNull] ProviderCodeGeneratorDependencies dependencies)
	{
		Dependencies = Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
	}

	public virtual MethodCallCodeFragment GenerateProviderOptions()
	{
		MethodCallCodeFragment methodCallCodeFragment = null;
		foreach (IProviderCodeGeneratorPlugin plugin in Dependencies.Plugins)
		{
			MethodCallCodeFragment methodCallCodeFragment2 = plugin.GenerateProviderOptions();
			if (methodCallCodeFragment2 != null)
			{
				methodCallCodeFragment = methodCallCodeFragment?.Chain(methodCallCodeFragment2) ?? methodCallCodeFragment2;
			}
		}
		return methodCallCodeFragment;
	}

	public abstract MethodCallCodeFragment GenerateUseProvider(string connectionString, MethodCallCodeFragment providerOptions);

	public virtual MethodCallCodeFragment GenerateContextOptions()
	{
		MethodCallCodeFragment methodCallCodeFragment = null;
		foreach (IProviderCodeGeneratorPlugin plugin in Dependencies.Plugins)
		{
			MethodCallCodeFragment methodCallCodeFragment2 = plugin.GenerateContextOptions();
			if (methodCallCodeFragment2 != null)
			{
				methodCallCodeFragment = methodCallCodeFragment?.Chain(methodCallCodeFragment2) ?? methodCallCodeFragment2;
			}
		}
		return methodCallCodeFragment;
	}
}

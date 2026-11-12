using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;

namespace Microsoft.EntityFrameworkCore.Scaffolding;

public interface IProviderConfigurationCodeGenerator
{
	MethodCallCodeFragment GenerateProviderOptions();

	MethodCallCodeFragment GenerateUseProvider([JetBrains.Annotations.NotNull] string connectionString, [JetBrains.Annotations.CanBeNull] MethodCallCodeFragment providerOptions);

	MethodCallCodeFragment GenerateContextOptions();
}

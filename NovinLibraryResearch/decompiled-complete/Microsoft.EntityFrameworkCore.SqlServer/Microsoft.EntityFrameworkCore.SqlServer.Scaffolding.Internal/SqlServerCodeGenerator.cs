using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;

namespace Microsoft.EntityFrameworkCore.SqlServer.Scaffolding.Internal;

public class SqlServerCodeGenerator : ProviderCodeGenerator
{
	public SqlServerCodeGenerator([JetBrains.Annotations.NotNull] ProviderCodeGeneratorDependencies dependencies)
		: base(dependencies)
	{
	}

	public override MethodCallCodeFragment GenerateUseProvider(string connectionString, MethodCallCodeFragment providerOptions)
	{
		return new MethodCallCodeFragment("UseSqlServer", (providerOptions != null) ? new object[2]
		{
			connectionString,
			new NestedClosureCodeFragment("x", providerOptions)
		} : new object[1] { connectionString });
	}
}

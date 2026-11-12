using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public class DatabaseProvider<TOptionsExtension> : IDatabaseProvider where TOptionsExtension : class, IDbContextOptionsExtension
{
	public virtual string Name => typeof(TOptionsExtension).GetTypeInfo().Assembly.GetName().Name;

	public DatabaseProvider([JetBrains.Annotations.NotNull] DatabaseProviderDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
	}

	public virtual bool IsConfigured(IDbContextOptions options)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(options, "options").Extensions.OfType<TOptionsExtension>().Any();
	}
}

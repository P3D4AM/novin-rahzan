using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public class RelationalDatabase : Database
{
	protected virtual RelationalDatabaseDependencies RelationalDependencies { get; }

	public RelationalDatabase([JetBrains.Annotations.NotNull] DatabaseDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalDatabaseDependencies relationalDependencies)
		: base(dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(relationalDependencies, "relationalDependencies");
		RelationalDependencies = relationalDependencies;
	}

	public override int SaveChanges(IList<IUpdateEntry> entries)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entries, "entries");
		return RelationalDependencies.BatchExecutor.Execute(RelationalDependencies.BatchPreparer.BatchCommands(entries, Dependencies.UpdateAdapterFactory.Create()), RelationalDependencies.Connection);
	}

	public override Task<int> SaveChangesAsync(IList<IUpdateEntry> entries, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entries, "entries");
		return RelationalDependencies.BatchExecutor.ExecuteAsync(RelationalDependencies.BatchPreparer.BatchCommands(entries, Dependencies.UpdateAdapterFactory.Create()), RelationalDependencies.Connection, cancellationToken);
	}
}

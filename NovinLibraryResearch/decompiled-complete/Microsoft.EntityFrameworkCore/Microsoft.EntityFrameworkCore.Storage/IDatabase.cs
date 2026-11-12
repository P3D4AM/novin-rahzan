using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Update;

namespace Microsoft.EntityFrameworkCore.Storage;

public interface IDatabase
{
	int SaveChanges([JetBrains.Annotations.NotNull] IList<IUpdateEntry> entries);

	Task<int> SaveChangesAsync([JetBrains.Annotations.NotNull] IList<IUpdateEntry> entries, CancellationToken cancellationToken = default(CancellationToken));

	Func<QueryContext, TResult> CompileQuery<TResult>([JetBrains.Annotations.NotNull] Expression query, bool async);
}

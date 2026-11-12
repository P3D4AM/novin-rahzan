using System.Collections.Concurrent;
using System.Data.Common;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore.SqlServer.ValueGeneration.Internal;

public class SqlServerValueGeneratorCache : ValueGeneratorCache, ISqlServerValueGeneratorCache, IValueGeneratorCache
{
	private readonly ConcurrentDictionary<string, SqlServerSequenceValueGeneratorState> _sequenceGeneratorCache = new ConcurrentDictionary<string, SqlServerSequenceValueGeneratorState>();

	public SqlServerValueGeneratorCache([JetBrains.Annotations.NotNull] ValueGeneratorCacheDependencies dependencies)
		: base(dependencies)
	{
	}

	public virtual SqlServerSequenceValueGeneratorState GetOrAddSequenceState(IProperty property, IRelationalConnection connection)
	{
		ISequence sequence = property.FindHiLoSequence();
		return _sequenceGeneratorCache.GetOrAdd(GetSequenceName(sequence, connection), (string sequenceName) => new SqlServerSequenceValueGeneratorState(sequence));
	}

	private static string GetSequenceName(ISequence sequence, IRelationalConnection connection)
	{
		DbConnection dbConnection = connection.DbConnection;
		return dbConnection.Database.ToUpperInvariant() + "::" + dbConnection.DataSource?.ToUpperInvariant() + "::" + ((sequence.Schema == null) ? "" : (sequence.Schema + ".")) + sequence.Name;
	}
}

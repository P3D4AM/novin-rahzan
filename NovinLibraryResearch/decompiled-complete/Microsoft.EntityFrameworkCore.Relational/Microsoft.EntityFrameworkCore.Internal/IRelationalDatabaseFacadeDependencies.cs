using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Internal;

public interface IRelationalDatabaseFacadeDependencies : IDatabaseFacadeDependencies
{
	IRelationalConnection RelationalConnection { get; }

	IRawSqlCommandBuilder RawSqlCommandBuilder { get; }
}

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace Microsoft.EntityFrameworkCore.Scaffolding.Internal;

public interface IScaffoldingModelFactory
{
	IModel Create([JetBrains.Annotations.NotNull] DatabaseModel databaseModel, bool useDatabaseNames);
}

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore.SqlServer.ValueGeneration.Internal;

public interface ISqlServerValueGeneratorCache : IValueGeneratorCache
{
	SqlServerSequenceValueGeneratorState GetOrAddSequenceState([JetBrains.Annotations.NotNull] IProperty property, [JetBrains.Annotations.NotNull] IRelationalConnection connection);
}

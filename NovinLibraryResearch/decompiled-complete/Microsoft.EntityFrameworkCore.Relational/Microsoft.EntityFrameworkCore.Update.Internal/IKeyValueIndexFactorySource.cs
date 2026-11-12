using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Update.Internal;

public interface IKeyValueIndexFactorySource
{
	IKeyValueIndexFactory GetKeyValueIndexFactory([JetBrains.Annotations.NotNull] IKey key);
}

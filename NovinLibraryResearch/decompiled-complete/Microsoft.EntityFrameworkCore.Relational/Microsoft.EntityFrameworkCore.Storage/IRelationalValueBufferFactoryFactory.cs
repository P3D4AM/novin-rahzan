using System.Collections.Generic;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage;

public interface IRelationalValueBufferFactoryFactory
{
	IRelationalValueBufferFactory Create([JetBrains.Annotations.NotNull] IReadOnlyList<TypeMaterializationInfo> types);
}

using System.Data.Common;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage;

public interface IRelationalValueBufferFactory
{
	ValueBuffer Create([JetBrains.Annotations.NotNull] DbDataReader dataReader);
}

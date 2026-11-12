using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.EntityFrameworkCore.Query;

public interface IIncludableQueryable<out TEntity, out TProperty> : IQueryable<TEntity>, IEnumerable<TEntity>, IEnumerable, IQueryable
{
}

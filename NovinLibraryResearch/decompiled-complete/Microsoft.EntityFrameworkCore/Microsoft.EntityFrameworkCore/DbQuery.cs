using System;

namespace Microsoft.EntityFrameworkCore;

[Obsolete("Use DbSet<T> instead")]
public abstract class DbQuery<TQuery> : DbSet<TQuery> where TQuery : class
{
}

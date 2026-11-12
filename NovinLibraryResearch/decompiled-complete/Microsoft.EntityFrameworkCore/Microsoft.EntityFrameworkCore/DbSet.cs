using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore;

public abstract class DbSet<TEntity> : IQueryable<TEntity>, IEnumerable<TEntity>, IEnumerable, IQueryable, IAsyncEnumerable<TEntity>, IInfrastructure<IServiceProvider>, IListSource where TEntity : class
{
	public virtual LocalView<TEntity> Local
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	Type IQueryable.ElementType
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	Expression IQueryable.Expression
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	IQueryProvider IQueryable.Provider
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	IServiceProvider IInfrastructure<IServiceProvider>.Instance
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	bool IListSource.ContainsListCollection => false;

	public virtual IAsyncEnumerable<TEntity> AsAsyncEnumerable()
	{
		return this;
	}

	public virtual IQueryable<TEntity> AsQueryable()
	{
		return this;
	}

	public virtual TEntity Find([JetBrains.Annotations.CanBeNull] params object[] keyValues)
	{
		throw new NotImplementedException();
	}

	public virtual ValueTask<TEntity> FindAsync([JetBrains.Annotations.CanBeNull] params object[] keyValues)
	{
		throw new NotImplementedException();
	}

	public virtual ValueTask<TEntity> FindAsync([JetBrains.Annotations.CanBeNull] object[] keyValues, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}

	public virtual EntityEntry<TEntity> Add([JetBrains.Annotations.NotNull] TEntity entity)
	{
		throw new NotImplementedException();
	}

	public virtual ValueTask<EntityEntry<TEntity>> AddAsync([JetBrains.Annotations.NotNull] TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}

	public virtual EntityEntry<TEntity> Attach([JetBrains.Annotations.NotNull] TEntity entity)
	{
		throw new NotImplementedException();
	}

	public virtual EntityEntry<TEntity> Remove([JetBrains.Annotations.NotNull] TEntity entity)
	{
		throw new NotImplementedException();
	}

	public virtual EntityEntry<TEntity> Update([JetBrains.Annotations.NotNull] TEntity entity)
	{
		throw new NotImplementedException();
	}

	public virtual void AddRange([JetBrains.Annotations.NotNull] params TEntity[] entities)
	{
		throw new NotImplementedException();
	}

	public virtual Task AddRangeAsync([JetBrains.Annotations.NotNull] params TEntity[] entities)
	{
		throw new NotImplementedException();
	}

	public virtual void AttachRange([JetBrains.Annotations.NotNull] params TEntity[] entities)
	{
		throw new NotImplementedException();
	}

	public virtual void RemoveRange([JetBrains.Annotations.NotNull] params TEntity[] entities)
	{
		throw new NotImplementedException();
	}

	public virtual void UpdateRange([JetBrains.Annotations.NotNull] params TEntity[] entities)
	{
		throw new NotImplementedException();
	}

	public virtual void AddRange([JetBrains.Annotations.NotNull] IEnumerable<TEntity> entities)
	{
		throw new NotImplementedException();
	}

	public virtual Task AddRangeAsync([JetBrains.Annotations.NotNull] IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}

	public virtual void AttachRange([JetBrains.Annotations.NotNull] IEnumerable<TEntity> entities)
	{
		throw new NotImplementedException();
	}

	public virtual void RemoveRange([JetBrains.Annotations.NotNull] IEnumerable<TEntity> entities)
	{
		throw new NotImplementedException();
	}

	public virtual void UpdateRange([JetBrains.Annotations.NotNull] IEnumerable<TEntity> entities)
	{
		throw new NotImplementedException();
	}

	IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
	{
		throw new NotImplementedException();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw new NotImplementedException();
	}

	IAsyncEnumerator<TEntity> IAsyncEnumerable<TEntity>.GetAsyncEnumerator(CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}

	IList IListSource.GetList()
	{
		throw new NotSupportedException(CoreStrings.DataBindingWithIListSource);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override string ToString()
	{
		return base.ToString();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}

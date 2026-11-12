using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Internal;

public class InternalDbSet<TEntity> : DbQuery<TEntity>, IQueryable<TEntity>, IEnumerable<TEntity>, IEnumerable, IQueryable, IAsyncEnumerable<TEntity>, IInfrastructure<IServiceProvider>, IResettableService where TEntity : class
{
	private readonly DbContext _context;

	private IEntityType _entityType;

	private EntityQueryable<TEntity> _entityQueryable;

	private LocalView<TEntity> _localView;

	private IEntityType EntityType
	{
		get
		{
			if (_entityType != null)
			{
				return _entityType;
			}
			_entityType = _context.Model.FindEntityType(typeof(TEntity));
			if (_entityType == null)
			{
				if (_context.Model.HasEntityTypeWithDefiningNavigation(typeof(TEntity)))
				{
					throw new InvalidOperationException(CoreStrings.InvalidSetTypeWeak(typeof(TEntity).ShortDisplayName()));
				}
				throw new InvalidOperationException(CoreStrings.InvalidSetType(typeof(TEntity).ShortDisplayName()));
			}
			if (_entityType.IsOwned())
			{
				_entityType = null;
				throw new InvalidOperationException(CoreStrings.InvalidSetTypeOwned(typeof(TEntity).ShortDisplayName()));
			}
			return _entityType;
		}
	}

	private EntityQueryable<TEntity> EntityQueryable
	{
		get
		{
			CheckState();
			return NonCapturingLazyInitializer.EnsureInitialized(ref _entityQueryable, this, (InternalDbSet<TEntity> internalSet) => internalSet.CreateEntityQueryable());
		}
	}

	public override LocalView<TEntity> Local
	{
		get
		{
			CheckKey();
			if (_context.ChangeTracker.AutoDetectChangesEnabled)
			{
				_context.ChangeTracker.DetectChanges();
			}
			return _localView ?? (_localView = new LocalView<TEntity>(this));
		}
	}

	private IEntityFinder<TEntity> Finder => (IEntityFinder<TEntity>)_context.GetDependencies().EntityFinderFactory.Create(EntityType);

	Type IQueryable.ElementType => EntityQueryable.ElementType;

	Expression IQueryable.Expression => EntityQueryable.Expression;

	IQueryProvider IQueryable.Provider => EntityQueryable.Provider;

	IServiceProvider IInfrastructure<IServiceProvider>.Instance => _context.GetInfrastructure();

	public InternalDbSet([JetBrains.Annotations.NotNull] DbContext context)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(context, "context");
		_context = context;
	}

	private void CheckState()
	{
		_ = EntityType;
	}

	private void CheckKey()
	{
		if (EntityType.FindPrimaryKey() == null)
		{
			throw new InvalidOperationException(CoreStrings.InvalidSetKeylessOperation(typeof(TEntity).ShortDisplayName()));
		}
	}

	private EntityQueryable<TEntity> CreateEntityQueryable()
	{
		return new EntityQueryable<TEntity>(_context.GetDependencies().QueryProvider);
	}

	public override TEntity Find(params object[] keyValues)
	{
		return Finder.Find(keyValues);
	}

	public override ValueTask<TEntity> FindAsync(params object[] keyValues)
	{
		return Finder.FindAsync(keyValues);
	}

	public override ValueTask<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken)
	{
		return Finder.FindAsync(keyValues, cancellationToken);
	}

	public override EntityEntry<TEntity> Add(TEntity entity)
	{
		return _context.Add(entity);
	}

	public override ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
	{
		return _context.AddAsync(entity, cancellationToken);
	}

	public override EntityEntry<TEntity> Attach(TEntity entity)
	{
		return _context.Attach(entity);
	}

	public override EntityEntry<TEntity> Remove(TEntity entity)
	{
		return _context.Remove(entity);
	}

	public override EntityEntry<TEntity> Update(TEntity entity)
	{
		return _context.Update(entity);
	}

	public override void AddRange(params TEntity[] entities)
	{
		_context.AddRange(entities);
	}

	public override Task AddRangeAsync(params TEntity[] entities)
	{
		return _context.AddRangeAsync(entities);
	}

	public override void AttachRange(params TEntity[] entities)
	{
		_context.AttachRange(entities);
	}

	public override void RemoveRange(params TEntity[] entities)
	{
		_context.RemoveRange(entities);
	}

	public override void UpdateRange(params TEntity[] entities)
	{
		_context.UpdateRange(entities);
	}

	public override void AddRange(IEnumerable<TEntity> entities)
	{
		_context.AddRange(entities);
	}

	public override Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken))
	{
		return _context.AddRangeAsync(entities, cancellationToken);
	}

	public override void AttachRange(IEnumerable<TEntity> entities)
	{
		_context.AttachRange(entities);
	}

	public override void RemoveRange(IEnumerable<TEntity> entities)
	{
		_context.RemoveRange(entities);
	}

	public override void UpdateRange(IEnumerable<TEntity> entities)
	{
		_context.UpdateRange(entities);
	}

	IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
	{
		return EntityQueryable.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return EntityQueryable.GetEnumerator();
	}

	IAsyncEnumerator<TEntity> IAsyncEnumerable<TEntity>.GetAsyncEnumerator(CancellationToken cancellationToken)
	{
		return EntityQueryable.GetAsyncEnumerator(cancellationToken);
	}

	void IResettableService.ResetState()
	{
		_localView = null;
	}

	Task IResettableService.ResetStateAsync(CancellationToken cancellationToken)
	{
		((IResettableService)this).ResetState();
		return null;
	}
}

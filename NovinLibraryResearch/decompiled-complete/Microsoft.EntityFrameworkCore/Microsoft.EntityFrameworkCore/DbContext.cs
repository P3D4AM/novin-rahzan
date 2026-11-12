using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore;

public class DbContext : IDisposable, IAsyncDisposable, IInfrastructure<IServiceProvider>, IDbContextDependencies, IDbSetCache, IDbContextPoolable, IResettableService
{
	private IDictionary<Type, object> _sets;

	private readonly DbContextOptions _options;

	private IDbContextServices _contextServices;

	private IDbContextDependencies _dbContextDependencies;

	private DatabaseFacade _database;

	private ChangeTracker _changeTracker;

	private IServiceScope _serviceScope;

	private IDbContextPool _dbContextPool;

	private bool _initializing;

	private bool _disposed;

	private readonly Guid _contextId = Guid.NewGuid();

	private int _lease;

	public virtual DatabaseFacade Database
	{
		get
		{
			CheckDisposed();
			return _database ?? (_database = new DatabaseFacade(this));
		}
	}

	public virtual ChangeTracker ChangeTracker => _changeTracker ?? (_changeTracker = ServiceProviderServiceExtensions.GetRequiredService<IChangeTrackerFactory>(InternalServiceProvider).Create());

	public virtual IModel Model
	{
		[DebuggerStepThrough]
		get
		{
			return DbContextDependencies.Model;
		}
	}

	public virtual DbContextId ContextId => new DbContextId(_contextId, _lease);

	[EntityFrameworkInternal]
	IDbSetSource IDbContextDependencies.SetSource => DbContextDependencies.SetSource;

	[EntityFrameworkInternal]
	IEntityFinderFactory IDbContextDependencies.EntityFinderFactory => DbContextDependencies.EntityFinderFactory;

	[EntityFrameworkInternal]
	IAsyncQueryProvider IDbContextDependencies.QueryProvider => DbContextDependencies.QueryProvider;

	[EntityFrameworkInternal]
	IStateManager IDbContextDependencies.StateManager => DbContextDependencies.StateManager;

	[EntityFrameworkInternal]
	IChangeDetector IDbContextDependencies.ChangeDetector => DbContextDependencies.ChangeDetector;

	[EntityFrameworkInternal]
	IEntityGraphAttacher IDbContextDependencies.EntityGraphAttacher => DbContextDependencies.EntityGraphAttacher;

	[EntityFrameworkInternal]
	IDiagnosticsLogger<DbLoggerCategory.Update> IDbContextDependencies.UpdateLogger => DbContextDependencies.UpdateLogger;

	[EntityFrameworkInternal]
	IDiagnosticsLogger<DbLoggerCategory.Infrastructure> IDbContextDependencies.InfrastructureLogger => DbContextDependencies.InfrastructureLogger;

	private IServiceProvider InternalServiceProvider
	{
		get
		{
			CheckDisposed();
			if (_contextServices != null)
			{
				return _contextServices.InternalServiceProvider;
			}
			if (_initializing)
			{
				throw new InvalidOperationException(CoreStrings.RecursiveOnConfiguring);
			}
			try
			{
				_initializing = true;
				DbContextOptionsBuilder dbContextOptionsBuilder = new DbContextOptionsBuilder(_options);
				OnConfiguring(dbContextOptionsBuilder);
				if (_options.IsFrozen && _options != dbContextOptionsBuilder.Options)
				{
					throw new InvalidOperationException(CoreStrings.PoolingOptionsModified);
				}
				DbContextOptions options = dbContextOptionsBuilder.Options;
				_serviceScope = ServiceProviderServiceExtensions.GetRequiredService<IServiceScopeFactory>(ServiceProviderCache.Instance.GetOrAdd(options, providerRequired: true)).CreateScope();
				IServiceProvider serviceProvider = _serviceScope.ServiceProvider;
				IDbContextServices service = ServiceProviderServiceExtensions.GetService<IDbContextServices>(serviceProvider);
				service.Initialize(serviceProvider, options, this);
				_contextServices = service;
				DbContextDependencies.InfrastructureLogger.ContextInitialized(this, options);
			}
			finally
			{
				_initializing = false;
			}
			return _contextServices.InternalServiceProvider;
		}
	}

	private IDbContextDependencies DbContextDependencies
	{
		[DebuggerStepThrough]
		get
		{
			CheckDisposed();
			return _dbContextDependencies ?? (_dbContextDependencies = ServiceProviderServiceExtensions.GetRequiredService<IDbContextDependencies>(InternalServiceProvider));
		}
	}

	IServiceProvider IInfrastructure<IServiceProvider>.Instance => InternalServiceProvider;

	protected DbContext()
		: this(new DbContextOptions<DbContext>())
	{
	}

	public DbContext([JetBrains.Annotations.NotNull] DbContextOptions options)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(options, "options");
		if (!options.ContextType.GetTypeInfo().IsAssignableFrom(GetType().GetTypeInfo()))
		{
			throw new InvalidOperationException(CoreStrings.NonGenericOptions(GetType().ShortDisplayName()));
		}
		_options = options;
		ServiceProviderServiceExtensions.GetRequiredService<IDbSetInitializer>(ServiceProviderCache.Instance.GetOrAdd(options, providerRequired: false)).InitializeSets(this);
	}

	[EntityFrameworkInternal]
	object IDbSetCache.GetOrAddSet(IDbSetSource source, Type type)
	{
		CheckDisposed();
		if (_sets == null)
		{
			_sets = new Dictionary<Type, object>();
		}
		if (!_sets.TryGetValue(type, out var value))
		{
			value = source.Create(this, type);
			_sets[type] = value;
		}
		return value;
	}

	public virtual DbSet<TEntity> Set<TEntity>() where TEntity : class
	{
		return (DbSet<TEntity>)((IDbSetCache)this).GetOrAddSet(DbContextDependencies.SetSource, typeof(TEntity));
	}

	[Obsolete("Use Set() for entity types without keys")]
	public virtual DbQuery<TQuery> Query<TQuery>() where TQuery : class
	{
		return (DbQuery<TQuery>)((IDbSetCache)this).GetOrAddSet(DbContextDependencies.SetSource, typeof(TQuery));
	}

	private IEntityFinder Finder(Type type)
	{
		IEntityType entityType = Model.FindEntityType(type);
		if (entityType == null)
		{
			if (Model.HasEntityTypeWithDefiningNavigation(type))
			{
				throw new InvalidOperationException(CoreStrings.InvalidSetTypeWeak(type.ShortDisplayName()));
			}
			throw new InvalidOperationException(CoreStrings.InvalidSetType(type.ShortDisplayName()));
		}
		if (entityType.FindPrimaryKey() == null)
		{
			throw new InvalidOperationException(CoreStrings.InvalidSetKeylessOperation(type.ShortDisplayName()));
		}
		return DbContextDependencies.EntityFinderFactory.Create(entityType);
	}

	[DebuggerStepThrough]
	private void CheckDisposed()
	{
		if (_disposed)
		{
			throw new ObjectDisposedException(GetType().ShortDisplayName(), CoreStrings.ContextDisposed);
		}
	}

	protected internal virtual void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
	}

	protected internal virtual void OnModelCreating(ModelBuilder modelBuilder)
	{
	}

	public virtual int SaveChanges()
	{
		return SaveChanges(acceptAllChangesOnSuccess: true);
	}

	public virtual int SaveChanges(bool acceptAllChangesOnSuccess)
	{
		CheckDisposed();
		DbContextDependencies.UpdateLogger.SaveChangesStarting(this);
		TryDetectChanges();
		try
		{
			int num = DbContextDependencies.StateManager.SaveChanges(acceptAllChangesOnSuccess);
			DbContextDependencies.UpdateLogger.SaveChangesCompleted(this, num);
			return num;
		}
		catch (DbUpdateConcurrencyException exception)
		{
			DbContextDependencies.UpdateLogger.OptimisticConcurrencyException(this, exception);
			throw;
		}
		catch (Exception exception2)
		{
			DbContextDependencies.UpdateLogger.SaveChangesFailed(this, exception2);
			throw;
		}
	}

	private void TryDetectChanges()
	{
		if (ChangeTracker.AutoDetectChangesEnabled)
		{
			ChangeTracker.DetectChanges();
		}
	}

	private void TryDetectChanges(EntityEntry entry)
	{
		if (ChangeTracker.AutoDetectChangesEnabled)
		{
			entry.DetectChanges();
		}
	}

	public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return SaveChangesAsync(acceptAllChangesOnSuccess: true, cancellationToken);
	}

	public virtual async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
	{
		CheckDisposed();
		DbContextDependencies.UpdateLogger.SaveChangesStarting(this);
		TryDetectChanges();
		try
		{
			int num = await DbContextDependencies.StateManager.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
			DbContextDependencies.UpdateLogger.SaveChangesCompleted(this, num);
			return num;
		}
		catch (DbUpdateConcurrencyException exception)
		{
			DbContextDependencies.UpdateLogger.OptimisticConcurrencyException(this, exception);
			throw;
		}
		catch (Exception exception2)
		{
			DbContextDependencies.UpdateLogger.SaveChangesFailed(this, exception2);
			throw;
		}
	}

	void IDbContextPoolable.SetPool(IDbContextPool contextPool)
	{
		_dbContextPool = contextPool;
		_lease = 1;
	}

	DbContextPoolConfigurationSnapshot IDbContextPoolable.SnapshotConfiguration()
	{
		return new DbContextPoolConfigurationSnapshot(_changeTracker?.AutoDetectChangesEnabled, _changeTracker?.QueryTrackingBehavior, _database?.AutoTransactionsEnabled, _changeTracker?.LazyLoadingEnabled, _changeTracker?.CascadeDeleteTiming, _changeTracker?.DeleteOrphansTiming);
	}

	void IDbContextPoolable.Resurrect(DbContextPoolConfigurationSnapshot configurationSnapshot)
	{
		_disposed = false;
		_lease++;
		if (configurationSnapshot.AutoDetectChangesEnabled.HasValue)
		{
			ChangeTracker.AutoDetectChangesEnabled = configurationSnapshot.AutoDetectChangesEnabled.Value;
			ChangeTracker.QueryTrackingBehavior = configurationSnapshot.QueryTrackingBehavior.Value;
			ChangeTracker.LazyLoadingEnabled = configurationSnapshot.LazyLoadingEnabled.Value;
			ChangeTracker.CascadeDeleteTiming = configurationSnapshot.CascadeDeleteTiming.Value;
			ChangeTracker.DeleteOrphansTiming = configurationSnapshot.DeleteOrphansTiming.Value;
		}
		else
		{
			((IResettableService)_changeTracker)?.ResetState();
		}
		if (_database != null)
		{
			_database.AutoTransactionsEnabled = !configurationSnapshot.AutoTransactionsEnabled.HasValue || configurationSnapshot.AutoTransactionsEnabled.Value;
		}
	}

	void IResettableService.ResetState()
	{
		foreach (IResettableService resettableService in GetResettableServices())
		{
			resettableService.ResetState();
		}
		_disposed = true;
	}

	async Task IResettableService.ResetStateAsync(CancellationToken cancellationToken)
	{
		foreach (IResettableService resettableService in GetResettableServices())
		{
			await resettableService.ResetStateAsync(cancellationToken);
		}
		_disposed = true;
	}

	private IEnumerable<IResettableService> GetResettableServices()
	{
		IDbContextServices contextServices = _contextServices;
		object obj;
		if (contextServices == null)
		{
			obj = null;
		}
		else
		{
			IServiceProvider internalServiceProvider = contextServices.InternalServiceProvider;
			obj = ((internalServiceProvider == null) ? null : ServiceProviderServiceExtensions.GetService<IEnumerable<IResettableService>>(internalServiceProvider)?.ToList());
		}
		List<IResettableService> list = (List<IResettableService>)obj;
		if (list != null)
		{
			foreach (IResettableService item in list)
			{
				yield return item;
			}
		}
		if (_sets == null)
		{
			yield break;
		}
		foreach (object value in _sets.Values)
		{
			if (value is IResettableService resettableService)
			{
				yield return resettableService;
			}
		}
	}

	public virtual void Dispose()
	{
		if (DisposeSync())
		{
			_serviceScope?.Dispose();
		}
	}

	private bool DisposeSync()
	{
		if (_dbContextPool == null && !_disposed)
		{
			_dbContextDependencies?.InfrastructureLogger.ContextDisposed(this);
			_disposed = true;
			_dbContextDependencies?.StateManager.Unsubscribe();
			_dbContextDependencies = null;
			_changeTracker = null;
			_database = null;
			return true;
		}
		return false;
	}

	public virtual ValueTask DisposeAsync()
	{
		if (!DisposeSync())
		{
			return default(ValueTask);
		}
		return _serviceScope.DisposeAsyncIfAvailable();
	}

	public virtual EntityEntry<TEntity> Entry<TEntity>([JetBrains.Annotations.NotNull] TEntity entity) where TEntity : class
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entity, "entity");
		CheckDisposed();
		EntityEntry<TEntity> entityEntry = EntryWithoutDetectChanges(entity);
		TryDetectChanges(entityEntry);
		return entityEntry;
	}

	private EntityEntry<TEntity> EntryWithoutDetectChanges<TEntity>(TEntity entity) where TEntity : class
	{
		return new EntityEntry<TEntity>(DbContextDependencies.StateManager.GetOrCreateEntry(entity));
	}

	public virtual EntityEntry Entry([JetBrains.Annotations.NotNull] object entity)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entity, "entity");
		CheckDisposed();
		EntityEntry entityEntry = EntryWithoutDetectChanges(entity);
		TryDetectChanges(entityEntry);
		return entityEntry;
	}

	private EntityEntry EntryWithoutDetectChanges(object entity)
	{
		return new EntityEntry(DbContextDependencies.StateManager.GetOrCreateEntry(entity));
	}

	private void SetEntityState(InternalEntityEntry entry, EntityState entityState)
	{
		if (entry.EntityState == EntityState.Detached)
		{
			DbContextDependencies.EntityGraphAttacher.AttachGraph(entry, entityState, entityState, forceStateWhenUnknownKey: true);
		}
		else
		{
			entry.SetEntityState(entityState, acceptChanges: true, modifyProperties: true, entityState);
		}
	}

	private Task SetEntityStateAsync(InternalEntityEntry entry, EntityState entityState, CancellationToken cancellationToken)
	{
		if (entry.EntityState != 0)
		{
			return entry.SetEntityStateAsync(entityState, acceptChanges: true, modifyProperties: true, entityState, cancellationToken);
		}
		return DbContextDependencies.EntityGraphAttacher.AttachGraphAsync(entry, entityState, entityState, forceStateWhenUnknownKey: true, cancellationToken);
	}

	public virtual EntityEntry<TEntity> Add<TEntity>([JetBrains.Annotations.NotNull] TEntity entity) where TEntity : class
	{
		CheckDisposed();
		return SetEntityState(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entity, "entity"), EntityState.Added);
	}

	public virtual async ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>([JetBrains.Annotations.NotNull] TEntity entity, CancellationToken cancellationToken = default(CancellationToken)) where TEntity : class
	{
		CheckDisposed();
		EntityEntry<TEntity> entry = EntryWithoutDetectChanges(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entity, "entity"));
		await SetEntityStateAsync(entry.GetInfrastructure(), EntityState.Added, cancellationToken);
		return entry;
	}

	public virtual EntityEntry<TEntity> Attach<TEntity>([JetBrains.Annotations.NotNull] TEntity entity) where TEntity : class
	{
		CheckDisposed();
		return SetEntityState(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entity, "entity"), EntityState.Unchanged);
	}

	public virtual EntityEntry<TEntity> Update<TEntity>([JetBrains.Annotations.NotNull] TEntity entity) where TEntity : class
	{
		CheckDisposed();
		return SetEntityState(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entity, "entity"), EntityState.Modified);
	}

	public virtual EntityEntry<TEntity> Remove<TEntity>([JetBrains.Annotations.NotNull] TEntity entity) where TEntity : class
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entity, "entity");
		CheckDisposed();
		EntityEntry<TEntity> entityEntry = EntryWithoutDetectChanges(entity);
		EntityState state = entityEntry.State;
		if (state == EntityState.Detached)
		{
			SetEntityState(entityEntry.GetInfrastructure(), EntityState.Unchanged);
		}
		entityEntry.State = ((state != EntityState.Added) ? EntityState.Deleted : EntityState.Detached);
		return entityEntry;
	}

	private EntityEntry<TEntity> SetEntityState<TEntity>(TEntity entity, EntityState entityState) where TEntity : class
	{
		EntityEntry<TEntity> entityEntry = EntryWithoutDetectChanges(entity);
		SetEntityState(entityEntry.GetInfrastructure(), entityState);
		return entityEntry;
	}

	public virtual EntityEntry Add([JetBrains.Annotations.NotNull] object entity)
	{
		CheckDisposed();
		return SetEntityState(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entity, "entity"), EntityState.Added);
	}

	public virtual async ValueTask<EntityEntry> AddAsync([JetBrains.Annotations.NotNull] object entity, CancellationToken cancellationToken = default(CancellationToken))
	{
		CheckDisposed();
		EntityEntry entry = EntryWithoutDetectChanges(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entity, "entity"));
		await SetEntityStateAsync(entry.GetInfrastructure(), EntityState.Added, cancellationToken);
		return entry;
	}

	public virtual EntityEntry Attach([JetBrains.Annotations.NotNull] object entity)
	{
		CheckDisposed();
		return SetEntityState(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entity, "entity"), EntityState.Unchanged);
	}

	public virtual EntityEntry Update([JetBrains.Annotations.NotNull] object entity)
	{
		CheckDisposed();
		return SetEntityState(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entity, "entity"), EntityState.Modified);
	}

	public virtual EntityEntry Remove([JetBrains.Annotations.NotNull] object entity)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entity, "entity");
		CheckDisposed();
		EntityEntry entityEntry = EntryWithoutDetectChanges(entity);
		EntityState state = entityEntry.State;
		if (state == EntityState.Detached)
		{
			SetEntityState(entityEntry.GetInfrastructure(), EntityState.Unchanged);
		}
		entityEntry.State = ((state != EntityState.Added) ? EntityState.Deleted : EntityState.Detached);
		return entityEntry;
	}

	private EntityEntry SetEntityState(object entity, EntityState entityState)
	{
		EntityEntry entityEntry = EntryWithoutDetectChanges(entity);
		SetEntityState(entityEntry.GetInfrastructure(), entityState);
		return entityEntry;
	}

	public virtual void AddRange([JetBrains.Annotations.NotNull] params object[] entities)
	{
		CheckDisposed();
		AddRange((IEnumerable<object>)entities);
	}

	public virtual Task AddRangeAsync([JetBrains.Annotations.NotNull] params object[] entities)
	{
		CheckDisposed();
		return AddRangeAsync(entities, default(CancellationToken));
	}

	public virtual void AttachRange([JetBrains.Annotations.NotNull] params object[] entities)
	{
		CheckDisposed();
		AttachRange((IEnumerable<object>)entities);
	}

	public virtual void UpdateRange([JetBrains.Annotations.NotNull] params object[] entities)
	{
		CheckDisposed();
		UpdateRange((IEnumerable<object>)entities);
	}

	public virtual void RemoveRange([JetBrains.Annotations.NotNull] params object[] entities)
	{
		CheckDisposed();
		RemoveRange((IEnumerable<object>)entities);
	}

	private void SetEntityStates(IEnumerable<object> entities, EntityState entityState)
	{
		IStateManager stateManager = DbContextDependencies.StateManager;
		foreach (object entity in entities)
		{
			SetEntityState(stateManager.GetOrCreateEntry(entity), entityState);
		}
	}

	public virtual void AddRange([JetBrains.Annotations.NotNull] IEnumerable<object> entities)
	{
		CheckDisposed();
		SetEntityStates(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entities, "entities"), EntityState.Added);
	}

	public virtual async Task AddRangeAsync([JetBrains.Annotations.NotNull] IEnumerable<object> entities, CancellationToken cancellationToken = default(CancellationToken))
	{
		CheckDisposed();
		IStateManager stateManager = DbContextDependencies.StateManager;
		foreach (object entity in entities)
		{
			await SetEntityStateAsync(stateManager.GetOrCreateEntry(entity), EntityState.Added, cancellationToken);
		}
	}

	public virtual void AttachRange([JetBrains.Annotations.NotNull] IEnumerable<object> entities)
	{
		CheckDisposed();
		SetEntityStates(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entities, "entities"), EntityState.Unchanged);
	}

	public virtual void UpdateRange([JetBrains.Annotations.NotNull] IEnumerable<object> entities)
	{
		CheckDisposed();
		SetEntityStates(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entities, "entities"), EntityState.Modified);
	}

	public virtual void RemoveRange([JetBrains.Annotations.NotNull] IEnumerable<object> entities)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entities, "entities");
		CheckDisposed();
		IStateManager stateManager = DbContextDependencies.StateManager;
		foreach (object entity in entities)
		{
			InternalEntityEntry orCreateEntry = stateManager.GetOrCreateEntry(entity);
			EntityState entityState = orCreateEntry.EntityState;
			if (entityState == EntityState.Detached)
			{
				SetEntityState(orCreateEntry, EntityState.Unchanged);
			}
			orCreateEntry.SetEntityState((entityState != EntityState.Added) ? EntityState.Deleted : EntityState.Detached);
		}
	}

	public virtual object Find([JetBrains.Annotations.NotNull] Type entityType, [JetBrains.Annotations.CanBeNull] params object[] keyValues)
	{
		CheckDisposed();
		return Finder(entityType).Find(keyValues);
	}

	public virtual ValueTask<object> FindAsync([JetBrains.Annotations.NotNull] Type entityType, [JetBrains.Annotations.CanBeNull] params object[] keyValues)
	{
		CheckDisposed();
		return Finder(entityType).FindAsync(keyValues);
	}

	public virtual ValueTask<object> FindAsync([JetBrains.Annotations.NotNull] Type entityType, [JetBrains.Annotations.CanBeNull] object[] keyValues, CancellationToken cancellationToken)
	{
		CheckDisposed();
		return Finder(entityType).FindAsync(keyValues, cancellationToken);
	}

	public virtual TEntity Find<TEntity>([JetBrains.Annotations.CanBeNull] params object[] keyValues) where TEntity : class
	{
		CheckDisposed();
		return ((IEntityFinder<TEntity>)Finder(typeof(TEntity))).Find(keyValues);
	}

	public virtual ValueTask<TEntity> FindAsync<TEntity>([JetBrains.Annotations.CanBeNull] params object[] keyValues) where TEntity : class
	{
		CheckDisposed();
		return ((IEntityFinder<TEntity>)Finder(typeof(TEntity))).FindAsync(keyValues);
	}

	public virtual ValueTask<TEntity> FindAsync<TEntity>([JetBrains.Annotations.CanBeNull] object[] keyValues, CancellationToken cancellationToken) where TEntity : class
	{
		CheckDisposed();
		return ((IEntityFinder<TEntity>)Finder(typeof(TEntity))).FindAsync(keyValues, cancellationToken);
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

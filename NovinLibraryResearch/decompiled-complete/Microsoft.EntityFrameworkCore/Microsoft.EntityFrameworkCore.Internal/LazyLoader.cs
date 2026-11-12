using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Internal;

public class LazyLoader : ILazyLoader, IDisposable
{
	private bool _disposed;

	private IDictionary<string, bool> _loadedStates;

	protected virtual IDiagnosticsLogger<DbLoggerCategory.Infrastructure> Logger { get; }

	protected virtual DbContext Context { get; }

	public LazyLoader([JetBrains.Annotations.NotNull] ICurrentDbContext currentContext, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Infrastructure> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(currentContext, "currentContext");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(logger, "logger");
		Context = currentContext.Context;
		Logger = logger;
	}

	public virtual void SetLoaded(object entity, [CallerMemberName] string navigationName = "", bool loaded = true)
	{
		if (_loadedStates == null)
		{
			_loadedStates = new Dictionary<string, bool>();
		}
		_loadedStates[navigationName] = loaded;
	}

	public virtual void Load(object entity, [CallerMemberName] string navigationName = "")
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entity, "entity");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(navigationName, "navigationName");
		if (ShouldLoad(entity, navigationName, out var navigationEntry))
		{
			navigationEntry.Load();
		}
	}

	public virtual Task LoadAsync(object entity, CancellationToken cancellationToken = default(CancellationToken), [CallerMemberName] string navigationName = "")
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entity, "entity");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(navigationName, "navigationName");
		if (!ShouldLoad(entity, navigationName, out var navigationEntry))
		{
			return Task.CompletedTask;
		}
		return navigationEntry.LoadAsync(cancellationToken);
	}

	private bool ShouldLoad(object entity, string navigationName, out NavigationEntry navigationEntry)
	{
		bool value = default(bool);
		if (_loadedStates != null && _loadedStates.TryGetValue(navigationName, out value) && value)
		{
			navigationEntry = null;
			return false;
		}
		if (_disposed)
		{
			Logger.LazyLoadOnDisposedContextWarning(Context, entity, navigationName);
		}
		else if (Context.ChangeTracker.LazyLoadingEnabled)
		{
			EntityEntry entityEntry = Context.Entry(entity);
			NavigationEntry navigationEntry2 = entityEntry.Navigation(navigationName);
			if (entityEntry.State == EntityState.Detached)
			{
				Logger.DetachedLazyLoadingWarning(Context, entity, navigationName);
			}
			else if (!navigationEntry2.IsLoaded)
			{
				Logger.NavigationLazyLoading(Context, entity, navigationName);
				navigationEntry = navigationEntry2;
				return true;
			}
		}
		navigationEntry = null;
		return false;
	}

	public virtual void Dispose()
	{
		_disposed = true;
	}
}

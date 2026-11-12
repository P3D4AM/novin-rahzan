using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Internal;

public class DbContextPool<TContext> : IDbContextPool, IDisposable, IAsyncDisposable where TContext : DbContext
{
	public sealed class Lease : IDisposable, IAsyncDisposable
	{
		private DbContextPool<TContext> _contextPool;

		public TContext Context { get; private set; }

		public Lease([JetBrains.Annotations.NotNull] DbContextPool<TContext> contextPool)
		{
			_contextPool = contextPool;
			Context = _contextPool.Rent();
		}

		void IDisposable.Dispose()
		{
			if (_contextPool != null)
			{
				if (!_contextPool.Return(Context))
				{
					((IDbContextPoolable)Context).SetPool((IDbContextPool)null);
					Context.Dispose();
				}
				_contextPool = null;
				Context = null;
			}
		}

		async ValueTask IAsyncDisposable.DisposeAsync()
		{
			if (_contextPool != null)
			{
				if (!_contextPool.Return(Context))
				{
					((IDbContextPoolable)Context).SetPool((IDbContextPool)null);
					await Context.DisposeAsync();
				}
				_contextPool = null;
				Context = null;
			}
		}
	}

	private const int DefaultPoolSize = 32;

	private readonly ConcurrentQueue<TContext> _pool = new ConcurrentQueue<TContext>();

	private readonly Func<TContext> _activator;

	private int _maxSize;

	private int _count;

	private DbContextPoolConfigurationSnapshot _configurationSnapshot;

	public DbContextPool([JetBrains.Annotations.NotNull] DbContextOptions options)
	{
		_maxSize = options.FindExtension<CoreOptionsExtension>()?.MaxPoolSize ?? 32;
		options.Freeze();
		_activator = CreateActivator(options);
		if (_activator == null)
		{
			throw new InvalidOperationException(CoreStrings.PoolingContextCtorError(typeof(TContext).ShortDisplayName()));
		}
	}

	private static Func<TContext> CreateActivator(DbContextOptions options)
	{
		ConstructorInfo[] array = typeof(TContext).GetTypeInfo().DeclaredConstructors.Where((ConstructorInfo c) => !c.IsStatic && c.IsPublic).ToArray();
		if (array.Length == 1)
		{
			ParameterInfo[] parameters = array[0].GetParameters();
			if (parameters.Length == 1 && (parameters[0].ParameterType == typeof(DbContextOptions) || parameters[0].ParameterType == typeof(DbContextOptions<TContext>)))
			{
				return Expression.Lambda<Func<TContext>>(Expression.New(array[0], Expression.Constant(options)), Array.Empty<ParameterExpression>()).Compile();
			}
		}
		return null;
	}

	public virtual TContext Rent()
	{
		if (_pool.TryDequeue(out var result))
		{
			Interlocked.Decrement(ref _count);
			((IDbContextPoolable)result).Resurrect(_configurationSnapshot);
			return result;
		}
		result = _activator();
		NonCapturingLazyInitializer.EnsureInitialized(ref _configurationSnapshot, result, (IDbContextPoolable c) => c.SnapshotConfiguration());
		((IDbContextPoolable)result).SetPool((IDbContextPool)this);
		return result;
	}

	public virtual bool Return([JetBrains.Annotations.NotNull] TContext context)
	{
		if (Interlocked.Increment(ref _count) <= _maxSize)
		{
			((IResettableService)context).ResetState();
			_pool.Enqueue(context);
			return true;
		}
		Interlocked.Decrement(ref _count);
		return false;
	}

	DbContext IDbContextPool.Rent()
	{
		return Rent();
	}

	bool IDbContextPool.Return(DbContext context)
	{
		return Return((TContext)context);
	}

	public virtual void Dispose()
	{
		_maxSize = 0;
		TContext result;
		while (_pool.TryDequeue(out result))
		{
			((IDbContextPoolable)result).SetPool((IDbContextPool)null);
			result.Dispose();
		}
	}

	public virtual async ValueTask DisposeAsync()
	{
		_maxSize = 0;
		TContext result;
		while (_pool.TryDequeue(out result))
		{
			((IDbContextPoolable)result).SetPool((IDbContextPool)null);
			await result.DisposeAsync();
		}
	}
}

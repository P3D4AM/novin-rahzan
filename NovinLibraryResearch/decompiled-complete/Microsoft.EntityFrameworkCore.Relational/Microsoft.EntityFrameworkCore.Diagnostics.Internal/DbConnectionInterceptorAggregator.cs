using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics.Internal;

public class DbConnectionInterceptorAggregator : InterceptorAggregator<IDbConnectionInterceptor>
{
	private sealed class CompositeDbConnectionInterceptor : IDbConnectionInterceptor, IInterceptor
	{
		private readonly IDbConnectionInterceptor[] _interceptors;

		public CompositeDbConnectionInterceptor([JetBrains.Annotations.NotNull] IEnumerable<IDbConnectionInterceptor> interceptors)
		{
			_interceptors = interceptors.ToArray();
		}

		public InterceptionResult ConnectionOpening(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
		{
			for (int i = 0; i < _interceptors.Length; i++)
			{
				result = _interceptors[i].ConnectionOpening(connection, eventData, result);
			}
			return result;
		}

		public async Task<InterceptionResult> ConnectionOpeningAsync(DbConnection connection, ConnectionEventData eventData, InterceptionResult result, CancellationToken cancellationToken = default(CancellationToken))
		{
			for (int i = 0; i < _interceptors.Length; i++)
			{
				result = await _interceptors[i].ConnectionOpeningAsync(connection, eventData, result, cancellationToken);
			}
			return result;
		}

		public void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
		{
			for (int i = 0; i < _interceptors.Length; i++)
			{
				_interceptors[i].ConnectionOpened(connection, eventData);
			}
		}

		public async Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = default(CancellationToken))
		{
			for (int i = 0; i < _interceptors.Length; i++)
			{
				await _interceptors[i].ConnectionOpenedAsync(connection, eventData, cancellationToken);
			}
		}

		public InterceptionResult ConnectionClosing(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
		{
			for (int i = 0; i < _interceptors.Length; i++)
			{
				result = _interceptors[i].ConnectionClosing(connection, eventData, result);
			}
			return result;
		}

		public async Task<InterceptionResult> ConnectionClosingAsync(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
		{
			for (int i = 0; i < _interceptors.Length; i++)
			{
				result = await _interceptors[i].ConnectionClosingAsync(connection, eventData, result);
			}
			return result;
		}

		public void ConnectionClosed(DbConnection connection, ConnectionEndEventData eventData)
		{
			for (int i = 0; i < _interceptors.Length; i++)
			{
				_interceptors[i].ConnectionClosed(connection, eventData);
			}
		}

		public async Task ConnectionClosedAsync(DbConnection connection, ConnectionEndEventData eventData)
		{
			for (int i = 0; i < _interceptors.Length; i++)
			{
				await _interceptors[i].ConnectionClosedAsync(connection, eventData);
			}
		}

		public void ConnectionFailed(DbConnection connection, ConnectionErrorEventData eventData)
		{
			for (int i = 0; i < _interceptors.Length; i++)
			{
				_interceptors[i].ConnectionFailed(connection, eventData);
			}
		}

		public async Task ConnectionFailedAsync(DbConnection connection, ConnectionErrorEventData eventData, CancellationToken cancellationToken = default(CancellationToken))
		{
			for (int i = 0; i < _interceptors.Length; i++)
			{
				await _interceptors[i].ConnectionFailedAsync(connection, eventData, cancellationToken);
			}
		}
	}

	protected override IDbConnectionInterceptor CreateChain(IEnumerable<IDbConnectionInterceptor> interceptors)
	{
		return new CompositeDbConnectionInterceptor(interceptors);
	}
}

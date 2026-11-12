using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.SignalR.Internal;

internal static class AsyncEnumeratorAdapters
{
	internal class AsyncEnumerator<T> : IAsyncEnumerator<object>, IDisposable
	{
		private readonly ChannelReader<T> _channel;

		private readonly CancellationToken _cancellationToken;

		private object _current;

		private readonly IDisposable _disposable;

		public object Current => _current;

		internal AsyncEnumerator(ChannelReader<T> channel, CancellationToken cancellationToken, IDisposable disposable)
		{
			_channel = channel;
			_cancellationToken = cancellationToken;
			_disposable = disposable;
		}

		public Task<bool> MoveNextAsync()
		{
			ValueTask<T> valueTask = _channel.ReadAsync(_cancellationToken);
			if (valueTask.IsCompletedSuccessfully)
			{
				_current = valueTask.Result;
				return Task.FromResult(result: true);
			}
			return valueTask.AsTask().ContinueWith(delegate(Task<T> t, object s)
			{
				AsyncEnumerator<T> asyncEnumerator = (AsyncEnumerator<T>)s;
				if (t.IsFaulted && t.Exception.InnerException is ChannelClosedException { InnerException: null })
				{
					return false;
				}
				asyncEnumerator._current = t.GetAwaiter().GetResult();
				return true;
			}, this, CancellationToken.None, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
		}

		public void Dispose()
		{
			_disposable?.Dispose();
		}
	}

	public static IAsyncEnumerator<object> GetAsyncEnumerator<T>(ChannelReader<T> channel, CancellationToken cancellationToken = default(CancellationToken))
	{
		return new AsyncEnumerator<T>(channel, cancellationToken, null);
	}
}

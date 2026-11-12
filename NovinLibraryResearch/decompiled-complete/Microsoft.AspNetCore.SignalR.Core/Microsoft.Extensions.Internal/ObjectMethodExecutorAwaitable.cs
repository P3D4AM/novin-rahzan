using System;
using System.Runtime.CompilerServices;

namespace Microsoft.Extensions.Internal;

internal struct ObjectMethodExecutorAwaitable
{
	public struct Awaiter : ICriticalNotifyCompletion, INotifyCompletion
	{
		private readonly object _customAwaiter;

		private readonly Func<object, bool> _isCompletedMethod;

		private readonly Func<object, object> _getResultMethod;

		private readonly Action<object, Action> _onCompletedMethod;

		private readonly Action<object, Action> _unsafeOnCompletedMethod;

		public bool IsCompleted => _isCompletedMethod(_customAwaiter);

		public Awaiter(object customAwaiter, Func<object, bool> isCompletedMethod, Func<object, object> getResultMethod, Action<object, Action> onCompletedMethod, Action<object, Action> unsafeOnCompletedMethod)
		{
			_customAwaiter = customAwaiter;
			_isCompletedMethod = isCompletedMethod;
			_getResultMethod = getResultMethod;
			_onCompletedMethod = onCompletedMethod;
			_unsafeOnCompletedMethod = unsafeOnCompletedMethod;
		}

		public object GetResult()
		{
			return _getResultMethod(_customAwaiter);
		}

		public void OnCompleted(Action continuation)
		{
			_onCompletedMethod(_customAwaiter, continuation);
		}

		public void UnsafeOnCompleted(Action continuation)
		{
			(_unsafeOnCompletedMethod ?? _onCompletedMethod)(_customAwaiter, continuation);
		}
	}

	private readonly object _customAwaitable;

	private readonly Func<object, object> _getAwaiterMethod;

	private readonly Func<object, bool> _isCompletedMethod;

	private readonly Func<object, object> _getResultMethod;

	private readonly Action<object, Action> _onCompletedMethod;

	private readonly Action<object, Action> _unsafeOnCompletedMethod;

	public ObjectMethodExecutorAwaitable(object customAwaitable, Func<object, object> getAwaiterMethod, Func<object, bool> isCompletedMethod, Func<object, object> getResultMethod, Action<object, Action> onCompletedMethod, Action<object, Action> unsafeOnCompletedMethod)
	{
		_customAwaitable = customAwaitable;
		_getAwaiterMethod = getAwaiterMethod;
		_isCompletedMethod = isCompletedMethod;
		_getResultMethod = getResultMethod;
		_onCompletedMethod = onCompletedMethod;
		_unsafeOnCompletedMethod = unsafeOnCompletedMethod;
	}

	public Awaiter GetAwaiter()
	{
		return new Awaiter(_getAwaiterMethod(_customAwaitable), _isCompletedMethod, _getResultMethod, _onCompletedMethod, _unsafeOnCompletedMethod);
	}
}

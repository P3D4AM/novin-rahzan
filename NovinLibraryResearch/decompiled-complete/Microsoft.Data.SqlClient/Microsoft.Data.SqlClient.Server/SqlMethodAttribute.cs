using System;

namespace Microsoft.Data.SqlClient.Server;

[Serializable]
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public sealed class SqlMethodAttribute : SqlFunctionAttribute
{
	private bool _isCalledOnNullInputs;

	private bool _isMutator;

	private bool _shouldInvokeIfReceiverIsNull;

	public bool OnNullCall
	{
		get
		{
			return _isCalledOnNullInputs;
		}
		set
		{
			_isCalledOnNullInputs = value;
		}
	}

	public bool IsMutator
	{
		get
		{
			return _isMutator;
		}
		set
		{
			_isMutator = value;
		}
	}

	public bool InvokeIfReceiverIsNull
	{
		get
		{
			return _shouldInvokeIfReceiverIsNull;
		}
		set
		{
			_shouldInvokeIfReceiverIsNull = value;
		}
	}

	public SqlMethodAttribute()
	{
		_isCalledOnNullInputs = true;
		_isMutator = false;
		_shouldInvokeIfReceiverIsNull = false;
	}
}

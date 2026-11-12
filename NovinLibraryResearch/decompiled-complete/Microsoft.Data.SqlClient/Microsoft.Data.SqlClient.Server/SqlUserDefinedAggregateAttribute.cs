using System;
using Microsoft.Data.Common;

namespace Microsoft.Data.SqlClient.Server;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public sealed class SqlUserDefinedAggregateAttribute : Attribute
{
	private int _maxByteSize;

	private bool _isInvariantToDup;

	private bool _isInvariantToNulls;

	private bool _isInvariantToOrder = true;

	private bool _isNullIfEmpty;

	private Format _format;

	private string _name;

	public const int MaxByteSizeValue = 8000;

	public int MaxByteSize
	{
		get
		{
			return _maxByteSize;
		}
		set
		{
			if (value < -1 || value > 8000)
			{
				throw ADP.ArgumentOutOfRange(StringsHelper.GetString(Strings.SQLUDT_MaxByteSizeValue), "MaxByteSize", value);
			}
			_maxByteSize = value;
		}
	}

	public bool IsInvariantToDuplicates
	{
		get
		{
			return _isInvariantToDup;
		}
		set
		{
			_isInvariantToDup = value;
		}
	}

	public bool IsInvariantToNulls
	{
		get
		{
			return _isInvariantToNulls;
		}
		set
		{
			_isInvariantToNulls = value;
		}
	}

	public bool IsInvariantToOrder
	{
		get
		{
			return _isInvariantToOrder;
		}
		set
		{
			_isInvariantToOrder = value;
		}
	}

	public bool IsNullIfEmpty
	{
		get
		{
			return _isNullIfEmpty;
		}
		set
		{
			_isNullIfEmpty = value;
		}
	}

	public Format Format => _format;

	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
		}
	}

	public SqlUserDefinedAggregateAttribute(Format format)
	{
		switch (format)
		{
		case Format.Unknown:
			throw ADP.NotSupportedUserDefinedTypeSerializationFormat(format, "format");
		case Format.Native:
		case Format.UserDefined:
			_format = format;
			break;
		default:
			throw ADP.InvalidUserDefinedTypeSerializationFormat(format);
		}
	}
}

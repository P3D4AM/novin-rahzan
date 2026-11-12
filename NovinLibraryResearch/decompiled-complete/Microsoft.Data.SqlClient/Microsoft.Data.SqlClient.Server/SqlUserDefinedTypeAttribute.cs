using System;
using Microsoft.Data.Common;

namespace Microsoft.Data.SqlClient.Server;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
public sealed class SqlUserDefinedTypeAttribute : Attribute
{
	private int _maxByteSize;

	private bool _isFixedLength;

	private bool _isByteOrdered;

	private Format _format;

	private string _name;

	internal const int YukonMaxByteSizeValue = 8000;

	private string _validationMethodName;

	public int MaxByteSize
	{
		get
		{
			return _maxByteSize;
		}
		set
		{
			if (value < -1)
			{
				throw ADP.ArgumentOutOfRange("MaxByteSize");
			}
			_maxByteSize = value;
		}
	}

	public bool IsFixedLength
	{
		get
		{
			return _isFixedLength;
		}
		set
		{
			_isFixedLength = value;
		}
	}

	public bool IsByteOrdered
	{
		get
		{
			return _isByteOrdered;
		}
		set
		{
			_isByteOrdered = value;
		}
	}

	public Format Format => _format;

	public string ValidationMethodName
	{
		get
		{
			return _validationMethodName;
		}
		set
		{
			_validationMethodName = value;
		}
	}

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

	public SqlUserDefinedTypeAttribute(Format format)
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

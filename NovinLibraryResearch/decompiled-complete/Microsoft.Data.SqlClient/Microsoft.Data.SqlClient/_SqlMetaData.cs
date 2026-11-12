using System;
using System.Data;

namespace Microsoft.Data.SqlClient;

internal sealed class _SqlMetaData : SqlMetaDataPriv, ICloneable
{
	[Flags]
	private enum _SqlMetadataFlags
	{
		None = 0,
		Updatable = 1,
		UpdateableUnknown = 2,
		IsDifferentName = 4,
		IsKey = 8,
		IsHidden = 0x10,
		IsExpression = 0x20,
		IsIdentity = 0x40,
		IsColumnSet = 0x80,
		IsUpdatableMask = 3
	}

	internal string column;

	internal string baseColumn;

	internal MultiPartTableName multiPartTableName;

	internal readonly int ordinal;

	internal byte tableNum;

	internal byte op;

	internal ushort operand;

	private _SqlMetadataFlags flags;

	internal string serverName => multiPartTableName.ServerName;

	internal string catalogName => multiPartTableName.CatalogName;

	internal string schemaName => multiPartTableName.SchemaName;

	internal string tableName => multiPartTableName.TableName;

	public byte Updatability
	{
		get
		{
			return (byte)(flags & _SqlMetadataFlags.IsUpdatableMask);
		}
		set
		{
			flags = (_SqlMetadataFlags)((value & 3) | (int)(flags & ~_SqlMetadataFlags.IsUpdatableMask));
		}
	}

	public bool IsReadOnly => (flags & _SqlMetadataFlags.IsUpdatableMask) == 0;

	public bool IsDifferentName
	{
		get
		{
			return flags.HasFlag(_SqlMetadataFlags.IsDifferentName);
		}
		set
		{
			Set(_SqlMetadataFlags.IsDifferentName, value);
		}
	}

	public bool IsKey
	{
		get
		{
			return flags.HasFlag(_SqlMetadataFlags.IsKey);
		}
		set
		{
			Set(_SqlMetadataFlags.IsKey, value);
		}
	}

	public bool IsHidden
	{
		get
		{
			return flags.HasFlag(_SqlMetadataFlags.IsHidden);
		}
		set
		{
			Set(_SqlMetadataFlags.IsHidden, value);
		}
	}

	public bool IsExpression
	{
		get
		{
			return flags.HasFlag(_SqlMetadataFlags.IsExpression);
		}
		set
		{
			Set(_SqlMetadataFlags.IsExpression, value);
		}
	}

	public bool IsIdentity
	{
		get
		{
			return flags.HasFlag(_SqlMetadataFlags.IsIdentity);
		}
		set
		{
			Set(_SqlMetadataFlags.IsIdentity, value);
		}
	}

	public bool IsColumnSet
	{
		get
		{
			return flags.HasFlag(_SqlMetadataFlags.IsColumnSet);
		}
		set
		{
			Set(_SqlMetadataFlags.IsColumnSet, value);
		}
	}

	internal bool IsNewKatmaiDateTimeType
	{
		get
		{
			if (SqlDbType.Date != type && SqlDbType.Time != type && SqlDbType.DateTime2 != type)
			{
				return SqlDbType.DateTimeOffset == type;
			}
			return true;
		}
	}

	internal bool IsLargeUdt
	{
		get
		{
			if (type == SqlDbType.Udt)
			{
				return length == int.MaxValue;
			}
			return false;
		}
	}

	internal _SqlMetaData(int ordinal)
	{
		this.ordinal = ordinal;
	}

	private void Set(_SqlMetadataFlags flag, bool value)
	{
		flags = (value ? (flags | flag) : (flags & ~flag));
	}

	public object Clone()
	{
		_SqlMetaData sqlMetaData = new _SqlMetaData(ordinal);
		sqlMetaData.CopyFrom(this);
		sqlMetaData.column = column;
		sqlMetaData.baseColumn = baseColumn;
		sqlMetaData.multiPartTableName = multiPartTableName;
		sqlMetaData.tableNum = tableNum;
		sqlMetaData.flags = flags;
		sqlMetaData.op = op;
		sqlMetaData.operand = operand;
		return sqlMetaData;
	}
}

using System;
using System.Data;
using System.Text;

namespace Microsoft.Data.SqlClient;

internal class SqlMetaDataPriv
{
	[Flags]
	private enum SqlMetaDataPrivFlags : byte
	{
		None = 0,
		IsNullable = 2,
		IsMultiValued = 4
	}

	internal SqlDbType type;

	internal byte tdsType;

	internal byte precision = byte.MaxValue;

	internal byte scale = byte.MaxValue;

	private SqlMetaDataPrivFlags flags;

	internal int length;

	internal SqlCollation collation;

	internal int codePage;

	internal Encoding encoding;

	internal MetaType metaType;

	public SqlMetaDataUdt udt;

	public SqlMetaDataXmlSchemaCollection xmlSchemaCollection;

	internal bool isEncrypted;

	internal SqlMetaDataPriv baseTI;

	internal SqlCipherMetadata cipherMD;

	public bool IsNullable
	{
		get
		{
			return flags.HasFlag(SqlMetaDataPrivFlags.IsNullable);
		}
		set
		{
			Set(SqlMetaDataPrivFlags.IsNullable, value);
		}
	}

	public bool IsMultiValued
	{
		get
		{
			return flags.HasFlag(SqlMetaDataPrivFlags.IsMultiValued);
		}
		set
		{
			Set(SqlMetaDataPrivFlags.IsMultiValued, value);
		}
	}

	internal byte NormalizationRuleVersion
	{
		get
		{
			if (cipherMD != null)
			{
				return cipherMD.NormalizationRuleVersion;
			}
			return 0;
		}
	}

	internal SqlMetaDataPriv()
	{
	}

	private void Set(SqlMetaDataPrivFlags flag, bool value)
	{
		flags = (value ? (flags | flag) : (flags & (SqlMetaDataPrivFlags)(~(uint)flag)));
	}

	internal virtual void CopyFrom(SqlMetaDataPriv original)
	{
		type = original.type;
		tdsType = original.tdsType;
		precision = original.precision;
		scale = original.scale;
		length = original.length;
		collation = original.collation;
		codePage = original.codePage;
		encoding = original.encoding;
		metaType = original.metaType;
		flags = original.flags;
		if (original.udt != null)
		{
			udt = new SqlMetaDataUdt();
			udt.CopyFrom(original.udt);
		}
		if (original.xmlSchemaCollection != null)
		{
			xmlSchemaCollection = new SqlMetaDataXmlSchemaCollection();
			xmlSchemaCollection.CopyFrom(original.xmlSchemaCollection);
		}
	}

	internal bool IsAlgorithmInitialized()
	{
		if (cipherMD != null)
		{
			return cipherMD.IsAlgorithmInitialized();
		}
		return false;
	}
}

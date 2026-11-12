using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public class DataTypeSpec
{
	private static class Helper
	{
		public static readonly DataTypeSpec[] AllDataTypes;

		public static readonly DataTypeSpec BigInt;

		public static readonly DataTypeSpec Binary;

		public static readonly DataTypeSpec Bit;

		public static readonly DataTypeSpec Char;

		public static readonly DataTypeSpec Date;

		public static readonly DataTypeSpec DateTime;

		public static readonly DataTypeSpec DateTime2;

		public static readonly DataTypeSpec DateTimeOffset;

		public static readonly DataTypeSpec Decimal;

		public static readonly DataTypeSpec Float;

		public static readonly DataTypeSpec Image;

		public static readonly DataTypeSpec Int;

		public static readonly DataTypeSpec Money;

		public static readonly DataTypeSpec NChar;

		public static readonly DataTypeSpec NText;

		public static readonly DataTypeSpec Numeric;

		public static readonly DataTypeSpec NVarChar;

		public static readonly DataTypeSpec Real;

		public static readonly DataTypeSpec SmallDateTime;

		public static readonly DataTypeSpec SmallInt;

		public static readonly DataTypeSpec SmallMoney;

		public static readonly DataTypeSpec Text;

		public static readonly DataTypeSpec Time;

		public static readonly DataTypeSpec Timestamp;

		public static readonly DataTypeSpec TinyInt;

		public static readonly DataTypeSpec UniqueIdentifier;

		public static readonly DataTypeSpec VarBinary;

		public static readonly DataTypeSpec VarChar;

		public static readonly DataTypeSpec Variant;

		public static readonly DataTypeSpec Xml;

		public static readonly DataTypeSpec Geography;

		public static readonly DataTypeSpec Geometry;

		public static readonly DataTypeSpec HierarchyId;

		public static readonly DataTypeSpec SysName;

		public static readonly DataTypeSpec NVarCharMax;

		public static readonly DataTypeSpec VarBinaryMax;

		public static readonly DataTypeSpec VarCharMax;

		static Helper()
		{
			BigInt = new DataTypeSpec(SqlDataType.BigInt, "bigint", requireLength: false, argIsScale: false);
			Binary = new DataTypeSpec(SqlDataType.Binary, "binary", requireLength: true, argIsScale: false, DataTypeArgSpec.SingleByteLength);
			Bit = new DataTypeSpec(SqlDataType.Bit, "bit", requireLength: false, argIsScale: false);
			Char = new DataTypeSpec(SqlDataType.Char, "char", requireLength: true, argIsScale: false, DataTypeArgSpec.SingleByteLength);
			Date = new DataTypeSpec(SqlDataType.Date, "date", requireLength: false, argIsScale: false);
			DateTime = new DataTypeSpec(SqlDataType.DateTime, "datetime", requireLength: false, argIsScale: false);
			DateTime2 = new DataTypeSpec(SqlDataType.DateTime2, "datetime2", requireLength: false, argIsScale: true, DataTypeArgSpec.TimeScale);
			DateTimeOffset = new DataTypeSpec(SqlDataType.DateTimeOffset, "datetimeoffset", requireLength: false, argIsScale: true, DataTypeArgSpec.TimeScale);
			Decimal = new DataTypeSpec(SqlDataType.Decimal, "decimal", requireLength: false, argIsScale: false, DataTypeArgSpec.NumericPrecision, DataTypeArgSpec.NumericScale);
			Float = new DataTypeSpec(SqlDataType.Float, "float", requireLength: false, argIsScale: false);
			Image = new DataTypeSpec(SqlDataType.Image, "image", requireLength: false, argIsScale: false);
			Int = new DataTypeSpec(SqlDataType.Int, "int", requireLength: false, argIsScale: false);
			Money = new DataTypeSpec(SqlDataType.Money, "money", requireLength: false, argIsScale: false);
			NChar = new DataTypeSpec(SqlDataType.NChar, "nchar", requireLength: true, argIsScale: false, DataTypeArgSpec.DoubleByteLength);
			NText = new DataTypeSpec(SqlDataType.NText, "ntext", requireLength: false, argIsScale: false);
			Numeric = new DataTypeSpec(SqlDataType.Numeric, "numeric", requireLength: false, argIsScale: false, DataTypeArgSpec.NumericPrecision, DataTypeArgSpec.NumericScale);
			NVarChar = new DataTypeSpec(SqlDataType.NVarChar, "nvarchar", requireLength: true, argIsScale: false, DataTypeArgSpec.DoubleByteLength);
			Real = new DataTypeSpec(SqlDataType.Real, "real", requireLength: false, argIsScale: false);
			SmallDateTime = new DataTypeSpec(SqlDataType.SmallDateTime, "smalldatetime", requireLength: false, argIsScale: false);
			SmallInt = new DataTypeSpec(SqlDataType.SmallInt, "smallint", requireLength: false, argIsScale: false);
			SmallMoney = new DataTypeSpec(SqlDataType.SmallMoney, "smallmoney", requireLength: false, argIsScale: false);
			Text = new DataTypeSpec(SqlDataType.Text, "text", requireLength: false, argIsScale: false);
			Time = new DataTypeSpec(SqlDataType.Time, "time", requireLength: false, argIsScale: true, DataTypeArgSpec.TimeScale);
			Timestamp = new DataTypeSpec(SqlDataType.Timestamp, "timestamp", requireLength: false, argIsScale: false);
			TinyInt = new DataTypeSpec(SqlDataType.TinyInt, "tinyint", requireLength: false, argIsScale: false);
			UniqueIdentifier = new DataTypeSpec(SqlDataType.UniqueIdentifier, "uniqueidentifier", requireLength: false, argIsScale: false);
			VarBinary = new DataTypeSpec(SqlDataType.VarBinary, "varbinary", requireLength: true, argIsScale: false, DataTypeArgSpec.SingleByteLength);
			VarChar = new DataTypeSpec(SqlDataType.VarChar, "varchar", requireLength: true, argIsScale: false, DataTypeArgSpec.SingleByteLength);
			Variant = new DataTypeSpec(SqlDataType.Variant, "sql_variant", requireLength: false, argIsScale: false);
			Xml = new DataTypeSpec(SqlDataType.Xml, "xml", requireLength: false, argIsScale: false);
			Geography = new DataTypeSpec(SqlDataType.Geography, "geography", requireLength: false, argIsScale: false);
			Geometry = new DataTypeSpec(SqlDataType.Geometry, "geometry", requireLength: false, argIsScale: false);
			HierarchyId = new DataTypeSpec(SqlDataType.HierarchyId, "hierarchyid", requireLength: false, argIsScale: false);
			SysName = new DataTypeSpec(SqlDataType.SysName, "sysname", requireLength: false, argIsScale: false);
			NVarCharMax = new DataTypeSpec(SqlDataType.NVarCharMax, "nvarchar", requireLength: false, argIsScale: false);
			VarBinaryMax = new DataTypeSpec(SqlDataType.VarBinaryMax, "varbinary", requireLength: false, argIsScale: false);
			VarCharMax = new DataTypeSpec(SqlDataType.VarCharMax, "varchar", requireLength: false, argIsScale: false);
			AllDataTypes = new DataTypeSpec[34]
			{
				BigInt, Binary, Bit, Char, Date, DateTime, DateTime2, DateTimeOffset, Decimal, Float,
				Image, Int, Money, NChar, NText, Numeric, NVarChar, Real, SmallDateTime, SmallInt,
				SmallMoney, Text, Time, Timestamp, TinyInt, UniqueIdentifier, VarBinary, VarChar, Variant, Xml,
				Geography, Geometry, HierarchyId, SysName
			};
		}
	}

	private readonly SqlDataType m_sqlDataType;

	private readonly string m_name;

	private readonly bool m_requireLength;

	private readonly bool m_argIsScale;

	private readonly DataTypeArgSpec m_argSpec1;

	private readonly DataTypeArgSpec m_argSpec2;

	public SqlDataType SqlDataType => m_sqlDataType;

	public string Name => m_name;

	public bool RequireLength => m_requireLength;

	public bool ArgIsScale => m_argIsScale;

	public DataTypeArgSpec ArgSpec1 => m_argSpec1;

	public DataTypeArgSpec ArgSpec2 => m_argSpec2;

	public static DataTypeSpec BigInt => Helper.BigInt;

	public static DataTypeSpec Binary => Helper.Binary;

	public static DataTypeSpec Bit => Helper.Bit;

	public static DataTypeSpec Char => Helper.Char;

	public static DataTypeSpec Date => Helper.Date;

	public static DataTypeSpec DateTime => Helper.DateTime;

	public static DataTypeSpec DateTime2 => Helper.DateTime2;

	public static DataTypeSpec DateTimeOffset => Helper.DateTimeOffset;

	public static DataTypeSpec Decimal => Helper.Decimal;

	public static DataTypeSpec Float => Helper.Float;

	public static DataTypeSpec Geography => Helper.Geography;

	public static DataTypeSpec Geometry => Helper.Geometry;

	public static DataTypeSpec HierarchyId => Helper.HierarchyId;

	public static DataTypeSpec Image => Helper.Image;

	public static DataTypeSpec Int => Helper.Int;

	public static DataTypeSpec Money => Helper.Money;

	public static DataTypeSpec NChar => Helper.NChar;

	public static DataTypeSpec NText => Helper.NText;

	public static DataTypeSpec Numeric => Helper.Numeric;

	public static DataTypeSpec NVarChar => Helper.NVarChar;

	public static DataTypeSpec NVarCharMax => Helper.NVarCharMax;

	public static DataTypeSpec Real => Helper.Real;

	public static DataTypeSpec SmallDateTime => Helper.SmallDateTime;

	public static DataTypeSpec SmallInt => Helper.SmallInt;

	public static DataTypeSpec SmallMoney => Helper.SmallMoney;

	public static DataTypeSpec SysName => Helper.SysName;

	public static DataTypeSpec Text => Helper.Text;

	public static DataTypeSpec Time => Helper.Time;

	public static DataTypeSpec Timestamp => Helper.Timestamp;

	public static DataTypeSpec TinyInt => Helper.TinyInt;

	public static DataTypeSpec UniqueIdentifier => Helper.UniqueIdentifier;

	public static DataTypeSpec VarBinary => Helper.VarBinary;

	public static DataTypeSpec VarBinaryMax => Helper.VarBinaryMax;

	public static DataTypeSpec VarChar => Helper.VarChar;

	public static DataTypeSpec VarCharMax => Helper.VarCharMax;

	public static DataTypeSpec Variant => Helper.Variant;

	public static DataTypeSpec Xml => Helper.Xml;

	public static IEnumerable<DataTypeSpec> AllDataTypes => Helper.AllDataTypes;

	private DataTypeSpec(SqlDataType sqlDataType, string name, bool requireLength, bool argIsScale)
		: this(sqlDataType, name, requireLength, argIsScale, null, null)
	{
	}

	private DataTypeSpec(SqlDataType sqlDataType, string name, bool requireLength, bool argIsScale, DataTypeArgSpec argSpec1)
		: this(sqlDataType, name, requireLength, argIsScale, argSpec1, null)
	{
	}

	private DataTypeSpec(SqlDataType sqlDataType, string name, bool requireLength, bool argIsScale, DataTypeArgSpec argSpec1, DataTypeArgSpec argSpec2)
	{
		m_sqlDataType = sqlDataType;
		m_name = name;
		m_requireLength = requireLength;
		m_argIsScale = argIsScale;
		m_argSpec1 = argSpec1;
		m_argSpec2 = argSpec2;
	}

	public static DataTypeSpec GetDataTypeSpec(string typeName)
	{
		if (typeName == null)
		{
			throw new ArgumentNullException("typeName");
		}
		switch (StringTokenLookup.Find(typeName))
		{
		case StringToken.BigInt:
			return BigInt;
		case StringToken.Binary:
			return Binary;
		case StringToken.Bit:
			return Bit;
		case StringToken.Char:
			return Char;
		case StringToken.Date:
			return Date;
		case StringToken.DateTime:
			return DateTime;
		case StringToken.DateTime2:
			return DateTime2;
		case StringToken.DateTimeOffset:
			return DateTimeOffset;
		case StringToken.Decimal:
			return Decimal;
		case StringToken.Float:
			return Float;
		case StringToken.Geography:
			return Geography;
		case StringToken.Geometry:
			return Geometry;
		case StringToken.HierarchyId:
			return HierarchyId;
		case StringToken.Image:
			return Image;
		case StringToken.Int:
			return Int;
		case StringToken.Money:
			return Money;
		case StringToken.NChar:
			return NChar;
		case StringToken.NText:
			return NText;
		case StringToken.Numeric:
			return Numeric;
		case StringToken.NVarChar:
			return NVarChar;
		case StringToken.NVarCharMax:
			return NVarCharMax;
		case StringToken.Real:
			return Real;
		case StringToken.SmallDateTime:
			return SmallDateTime;
		case StringToken.SmallInt:
			return SmallInt;
		case StringToken.SmallMoney:
			return SmallMoney;
		case StringToken.SysName:
			return SysName;
		case StringToken.Text:
			return Text;
		case StringToken.Time:
			return Time;
		case StringToken.Timestamp:
			return Timestamp;
		case StringToken.TinyInt:
			return TinyInt;
		case StringToken.UniqueIdentifier:
			return UniqueIdentifier;
		case StringToken.VarBinary:
			return VarBinary;
		case StringToken.VarBinaryMax:
			return VarBinaryMax;
		case StringToken.VarChar:
			return VarChar;
		case StringToken.VarCharMax:
			return VarCharMax;
		case StringToken.Variant:
		case StringToken.SqlVariant:
			return Variant;
		case StringToken.Xml:
			return Xml;
		default:
			return null;
		}
	}
}

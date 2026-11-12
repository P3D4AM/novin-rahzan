using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public abstract class SystemDataTypeLookupBase : ISystemDataTypeLookup
{
	private readonly ISystemDataType m_bigInt;

	private readonly ISystemDataType m_bit;

	private readonly ISystemDataType m_date;

	private readonly ISystemDataType m_dateTime;

	private readonly ISystemDataType m_float;

	private readonly ISystemDataType m_geography;

	private readonly ISystemDataType m_geometry;

	private readonly ISystemDataType m_hierarchyId;

	private readonly ISystemDataType m_image;

	private readonly ISystemDataType m_int;

	private readonly ISystemDataType m_money;

	private readonly ISystemDataType m_ntext;

	private readonly ISystemDataType m_nvarCharMax;

	private readonly ISystemDataType m_real;

	private readonly ISystemDataType m_smallDateTime;

	private readonly ISystemDataType m_smallInt;

	private readonly ISystemDataType m_smallMoney;

	private readonly ISystemDataType m_sysName;

	private readonly ISystemDataType m_text;

	private readonly ISystemDataType m_timestamp;

	private readonly ISystemDataType m_tinyInt;

	private readonly ISystemDataType m_uniqueIdentifier;

	private readonly ISystemDataType m_varBinaryMax;

	private readonly ISystemDataType m_varCharMax;

	private readonly ISystemDataType m_variant;

	public ISystemDataType BigInt => m_bigInt;

	public ISystemDataType Bit => m_bit;

	public ISystemDataType Date => m_date;

	public ISystemDataType DateTime => m_dateTime;

	public ISystemDataType Float => m_float;

	public ISystemDataType Geography => m_geography;

	public ISystemDataType Geometry => m_geometry;

	public ISystemDataType HierarchyId => m_hierarchyId;

	public ISystemDataType Image => m_image;

	public ISystemDataType Int => m_int;

	public ISystemDataType Money => m_money;

	public ISystemDataType NText => m_ntext;

	public ISystemDataType NVarCharMax => m_nvarCharMax;

	public ISystemDataType Real => m_real;

	public ISystemDataType SmallDateTime => m_smallDateTime;

	public ISystemDataType SmallInt => m_smallInt;

	public ISystemDataType SmallMoney => m_smallMoney;

	public ISystemDataType SysName => m_sysName;

	public ISystemDataType Text => m_text;

	public ISystemDataType Timestamp => m_timestamp;

	public ISystemDataType TinyInt => m_tinyInt;

	public ISystemDataType UniqueIdentifier => m_uniqueIdentifier;

	public ISystemDataType VarBinaryMax => m_varBinaryMax;

	public ISystemDataType VarCharMax => m_varCharMax;

	public ISystemDataType Variant => m_variant;

	protected SystemDataTypeLookupBase()
	{
		m_bigInt = SystemDataType.SystemDataTypeFactory.BigInt.GetDataType();
		m_bit = SystemDataType.SystemDataTypeFactory.Bit.GetDataType();
		m_date = SystemDataType.SystemDataTypeFactory.Date.GetDataType();
		m_dateTime = SystemDataType.SystemDataTypeFactory.DateTime.GetDataType();
		m_float = SystemDataType.SystemDataTypeFactory.Float.GetDataType();
		m_geography = SystemDataType.SystemDataTypeFactory.Geography.GetDataType();
		m_geometry = SystemDataType.SystemDataTypeFactory.Geometry.GetDataType();
		m_hierarchyId = SystemDataType.SystemDataTypeFactory.HierarchyId.GetDataType();
		m_image = SystemDataType.SystemDataTypeFactory.Image.GetDataType();
		m_int = SystemDataType.SystemDataTypeFactory.Int.GetDataType();
		m_money = SystemDataType.SystemDataTypeFactory.Money.GetDataType();
		m_ntext = SystemDataType.SystemDataTypeFactory.NText.GetDataType();
		m_nvarCharMax = SystemDataType.SystemDataTypeFactory.NVarCharMax.GetDataType();
		m_real = SystemDataType.SystemDataTypeFactory.Real.GetDataType();
		m_smallDateTime = SystemDataType.SystemDataTypeFactory.SmallDateTime.GetDataType();
		m_smallInt = SystemDataType.SystemDataTypeFactory.SmallInt.GetDataType();
		m_smallMoney = SystemDataType.SystemDataTypeFactory.SmallMoney.GetDataType();
		m_sysName = SystemDataType.SystemDataTypeFactory.SysName.GetDataType();
		m_text = SystemDataType.SystemDataTypeFactory.Text.GetDataType();
		m_timestamp = SystemDataType.SystemDataTypeFactory.Timestamp.GetDataType();
		m_tinyInt = SystemDataType.SystemDataTypeFactory.TinyInt.GetDataType();
		m_uniqueIdentifier = SystemDataType.SystemDataTypeFactory.UniqueIdentifier.GetDataType();
		m_varBinaryMax = SystemDataType.SystemDataTypeFactory.VarBinaryMax.GetDataType();
		m_varCharMax = SystemDataType.SystemDataTypeFactory.VarCharMax.GetDataType();
		m_variant = SystemDataType.SystemDataTypeFactory.Variant.GetDataType();
	}

	public ISystemDataType Find(DataTypeSpec typeSpec, bool isMaximum)
	{
		if (typeSpec == null)
		{
			throw new ArgumentNullException("typeSpec");
		}
		ISystemDataType result = null;
		if (isMaximum)
		{
			switch (typeSpec.SqlDataType)
			{
			case SqlDataType.NVarChar:
				result = NVarCharMax;
				break;
			case SqlDataType.VarBinary:
				result = VarBinaryMax;
				break;
			case SqlDataType.VarChar:
				result = VarCharMax;
				break;
			}
		}
		else
		{
			result = SystemDataType.SystemDataTypeFactory.GetDataTypeFactory(typeSpec.SqlDataType).GetDataType();
		}
		return result;
	}

	public ISystemDataType Find(DataTypeSpec typeSpec, int precisionOrMaxLength)
	{
		if (typeSpec == null)
		{
			throw new ArgumentNullException("typeSpec");
		}
		return SystemDataType.SystemDataTypeFactory.GetDataTypeFactory(typeSpec.SqlDataType).GetDataType(precisionOrMaxLength);
	}

	public ISystemDataType Find(DataTypeSpec typeSpec, int precision, int scale)
	{
		if (typeSpec == null)
		{
			throw new ArgumentNullException("typeSpec");
		}
		return SystemDataType.SystemDataTypeFactory.GetDataTypeFactory(typeSpec.SqlDataType).GetDataType(precision, scale);
	}

	public ISystemDataType Binary()
	{
		return SystemDataType.SystemDataTypeFactory.Binary.GetDataType();
	}

	public ISystemDataType Binary(int maxLength)
	{
		return SystemDataType.SystemDataTypeFactory.Binary.GetDataType(maxLength);
	}

	public ISystemDataType Char()
	{
		return SystemDataType.SystemDataTypeFactory.Char.GetDataType();
	}

	public ISystemDataType Char(int maxLength)
	{
		return SystemDataType.SystemDataTypeFactory.Char.GetDataType(maxLength);
	}

	public ISystemDataType DateTime2()
	{
		return SystemDataType.SystemDataTypeFactory.DateTime2.GetDataType();
	}

	public ISystemDataType DateTime2(int scale)
	{
		return SystemDataType.SystemDataTypeFactory.DateTime2.GetDataType(scale);
	}

	public ISystemDataType DateTimeOffset()
	{
		return SystemDataType.SystemDataTypeFactory.DateTimeOffset.GetDataType();
	}

	public ISystemDataType DateTimeOffset(int scale)
	{
		return SystemDataType.SystemDataTypeFactory.DateTimeOffset.GetDataType(scale);
	}

	public ISystemDataType NChar()
	{
		return SystemDataType.SystemDataTypeFactory.NChar.GetDataType();
	}

	public ISystemDataType NChar(int maxLength)
	{
		return SystemDataType.SystemDataTypeFactory.NChar.GetDataType(maxLength);
	}

	public ISystemDataType NVarChar()
	{
		return SystemDataType.SystemDataTypeFactory.NVarChar.GetDataType();
	}

	public ISystemDataType NVarChar(int maxLength)
	{
		return SystemDataType.SystemDataTypeFactory.NVarChar.GetDataType(maxLength);
	}

	public ISystemDataType Time()
	{
		return SystemDataType.SystemDataTypeFactory.Time.GetDataType();
	}

	public ISystemDataType Time(int scale)
	{
		return SystemDataType.SystemDataTypeFactory.Time.GetDataType(scale);
	}

	public ISystemDataType VarBinary()
	{
		return SystemDataType.SystemDataTypeFactory.VarBinary.GetDataType();
	}

	public ISystemDataType VarBinary(int maxLength)
	{
		return SystemDataType.SystemDataTypeFactory.VarBinary.GetDataType(maxLength);
	}

	public ISystemDataType VarChar()
	{
		return SystemDataType.SystemDataTypeFactory.VarChar.GetDataType();
	}

	public ISystemDataType VarChar(int maxLength)
	{
		return SystemDataType.SystemDataTypeFactory.VarChar.GetDataType(maxLength);
	}

	public ISystemDataType Decimal()
	{
		return SystemDataType.SystemDataTypeFactory.Decimal.GetDataType();
	}

	public ISystemDataType Decimal(int precision)
	{
		return SystemDataType.SystemDataTypeFactory.Decimal.GetDataType(precision);
	}

	public ISystemDataType Decimal(int precision, int scale)
	{
		return SystemDataType.SystemDataTypeFactory.Decimal.GetDataType(precision, scale);
	}

	public ISystemDataType Numeric()
	{
		return SystemDataType.SystemDataTypeFactory.Numeric.GetDataType();
	}

	public ISystemDataType Numeric(int precision)
	{
		return SystemDataType.SystemDataTypeFactory.Numeric.GetDataType(precision);
	}

	public ISystemDataType Numeric(int precision, int scale)
	{
		return SystemDataType.SystemDataTypeFactory.Numeric.GetDataType(precision, scale);
	}

	public ISystemDataType Xml()
	{
		return SystemDataType.SystemDataTypeFactory.Xml.GetDataType();
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;
using Microsoft.Data.Common;
using Microsoft.Data.SqlClient.Server;

namespace Microsoft.Data.SqlClient;

[TypeConverter(typeof(SqlParameterConverter))]
public sealed class SqlParameter : DbParameter, IDbDataParameter, IDataParameter, ICloneable
{
	internal sealed class SqlParameterConverter : ExpandableObjectConverter
	{
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (typeof(InstanceDescriptor) == destinationType)
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw ADP.ArgumentNull("destinationType");
			}
			if (typeof(InstanceDescriptor) == destinationType && value is SqlParameter)
			{
				return ConvertToInstanceDescriptor(value as SqlParameter);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		private InstanceDescriptor ConvertToInstanceDescriptor(SqlParameter p)
		{
			int num = 0;
			if (p.ShouldSerializeSqlDbType())
			{
				num |= 1;
			}
			if (p.ShouldSerializeSize())
			{
				num |= 2;
			}
			if (!string.IsNullOrEmpty(p.SourceColumn))
			{
				num |= 4;
			}
			if (p.Value != null)
			{
				num |= 8;
			}
			if (ParameterDirection.Input != p.Direction || p.IsNullable || p.ShouldSerializePrecision() || p.ShouldSerializeScale() || DataRowVersion.Current != p.SourceVersion)
			{
				num |= 0x10;
			}
			if (p.SourceColumnNullMapping || !string.IsNullOrEmpty(p.XmlSchemaCollectionDatabase) || !string.IsNullOrEmpty(p.XmlSchemaCollectionOwningSchema) || !string.IsNullOrEmpty(p.XmlSchemaCollectionName))
			{
				num |= 0x20;
			}
			Type[] types;
			object[] arguments;
			switch (num)
			{
			case 0:
			case 1:
				types = new Type[2]
				{
					typeof(string),
					typeof(SqlDbType)
				};
				arguments = new object[2] { p.ParameterName, p.SqlDbType };
				break;
			case 2:
			case 3:
				types = new Type[3]
				{
					typeof(string),
					typeof(SqlDbType),
					typeof(int)
				};
				arguments = new object[3] { p.ParameterName, p.SqlDbType, p.Size };
				break;
			case 4:
			case 5:
			case 6:
			case 7:
				types = new Type[4]
				{
					typeof(string),
					typeof(SqlDbType),
					typeof(int),
					typeof(string)
				};
				arguments = new object[4] { p.ParameterName, p.SqlDbType, p.Size, p.SourceColumn };
				break;
			case 8:
				types = new Type[2]
				{
					typeof(string),
					typeof(object)
				};
				arguments = new object[2] { p.ParameterName, p.Value };
				break;
			default:
				if ((0x20 & num) == 0)
				{
					types = new Type[10]
					{
						typeof(string),
						typeof(SqlDbType),
						typeof(int),
						typeof(ParameterDirection),
						typeof(bool),
						typeof(byte),
						typeof(byte),
						typeof(string),
						typeof(DataRowVersion),
						typeof(object)
					};
					arguments = new object[10] { p.ParameterName, p.SqlDbType, p.Size, p.Direction, p.IsNullable, p.PrecisionInternal, p.ScaleInternal, p.SourceColumn, p.SourceVersion, p.Value };
				}
				else
				{
					types = new Type[13]
					{
						typeof(string),
						typeof(SqlDbType),
						typeof(int),
						typeof(ParameterDirection),
						typeof(byte),
						typeof(byte),
						typeof(string),
						typeof(DataRowVersion),
						typeof(bool),
						typeof(object),
						typeof(string),
						typeof(string),
						typeof(string)
					};
					arguments = new object[13]
					{
						p.ParameterName, p.SqlDbType, p.Size, p.Direction, p.PrecisionInternal, p.ScaleInternal, p.SourceColumn, p.SourceVersion, p.SourceColumnNullMapping, p.Value,
						p.XmlSchemaCollectionDatabase, p.XmlSchemaCollectionOwningSchema, p.XmlSchemaCollectionName
					};
				}
				break;
			}
			ConstructorInfo constructor = typeof(SqlParameter).GetConstructor(types);
			return new InstanceDescriptor(constructor, arguments);
		}
	}

	private MetaType _metaType;

	private SqlCollation _collation;

	private SqlMetaDataXmlSchemaCollection _xmlSchemaCollection;

	private string _udtTypeName;

	private string _typeName;

	private Exception _udtLoadError;

	private string _parameterName;

	private byte _precision;

	private byte _scale;

	private bool _hasScale;

	private MetaType _internalMetaType;

	private SqlBuffer _sqlBufferReturnValue;

	private INullable _valueAsINullable;

	private bool _isSqlParameterSqlType;

	private bool _isNull;

	private bool _coercedValueIsSqlType;

	private bool _coercedValueIsDataFeed;

	private int _actualSize;

	private object _value;

	private object _coercedValue;

	private object _parent;

	private ParameterDirection _direction;

	private int _size;

	private int _offset;

	private string _sourceColumn;

	private DataRowVersion _sourceVersion;

	private bool _sourceColumnNullMapping;

	private bool _isNullable;

	internal SqlCipherMetadata CipherMetadata { get; set; }

	internal bool HasReceivedMetadata { get; set; }

	internal byte NormalizationRuleVersion => CipherMetadata?.NormalizationRuleVersion ?? 0;

	[Browsable(false)]
	public SqlCompareOptions CompareInfo
	{
		get
		{
			return _collation?.SqlCompareOptions ?? SqlCompareOptions.None;
		}
		set
		{
			SqlCollation sqlCollation = _collation;
			if (sqlCollation == null)
			{
				sqlCollation = (_collation = new SqlCollation());
			}
			SqlCompareOptions sqlCompareOptions = SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreNonSpace | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth | SqlCompareOptions.BinarySort | SqlCompareOptions.BinarySort2;
			if ((value & sqlCompareOptions) != value)
			{
				throw ADP.ArgumentOutOfRange("CompareInfo");
			}
			sqlCollation.SqlCompareOptions = value;
		}
	}

	[ResCategory("XML")]
	public string XmlSchemaCollectionDatabase
	{
		get
		{
			return _xmlSchemaCollection?.Database ?? string.Empty;
		}
		set
		{
			EnsureXmlSchemaCollection().Database = value;
		}
	}

	[ResCategory("XML")]
	public string XmlSchemaCollectionOwningSchema
	{
		get
		{
			return _xmlSchemaCollection?.OwningSchema ?? string.Empty;
		}
		set
		{
			EnsureXmlSchemaCollection().OwningSchema = value;
		}
	}

	[ResCategory("XML")]
	public string XmlSchemaCollectionName
	{
		get
		{
			return _xmlSchemaCollection?.Name ?? string.Empty;
		}
		set
		{
			EnsureXmlSchemaCollection().Name = value;
		}
	}

	[DefaultValue(false)]
	[ResCategory("Data")]
	public bool ForceColumnEncryption { get; set; }

	public override DbType DbType
	{
		get
		{
			return GetMetaTypeOnly().DbType;
		}
		set
		{
			MetaType metaType = _metaType;
			if (metaType == null || metaType.DbType != value)
			{
				PropertyTypeChanging();
				_metaType = MetaType.GetMetaTypeFromDbType(value);
			}
		}
	}

	public override string ParameterName
	{
		get
		{
			return _parameterName ?? string.Empty;
		}
		set
		{
			if (string.IsNullOrEmpty(value) || value.Length < 128 || (value[0] == '@' && value.Length <= 128))
			{
				if (_parameterName != value)
				{
					PropertyChanging();
					_parameterName = value;
				}
				return;
			}
			throw SQL.InvalidParameterNameLength(value);
		}
	}

	[Browsable(false)]
	public int LocaleId
	{
		get
		{
			return _collation?.LCID ?? 0;
		}
		set
		{
			SqlCollation sqlCollation = _collation;
			if (sqlCollation == null)
			{
				sqlCollation = (_collation = new SqlCollation());
			}
			if ((ulong)value != (0xFFFFFuL & (ulong)value))
			{
				throw ADP.ArgumentOutOfRange("LocaleId");
			}
			sqlCollation.LCID = value;
		}
	}

	[DefaultValue(0)]
	[ResCategory("Data")]
	public new byte Precision
	{
		get
		{
			return PrecisionInternal;
		}
		set
		{
			PrecisionInternal = value;
		}
	}

	[DefaultValue(0)]
	[ResCategory("Data")]
	public new byte Scale
	{
		get
		{
			return ScaleInternal;
		}
		set
		{
			ScaleInternal = value;
		}
	}

	internal byte ScaleInternal
	{
		get
		{
			byte b = _scale;
			SqlDbType metaSqlDbTypeOnly = GetMetaSqlDbTypeOnly();
			if (b == 0 && metaSqlDbTypeOnly == SqlDbType.Decimal)
			{
				b = ValueScale(SqlValue);
			}
			return b;
		}
		set
		{
			if (_scale != value || !_hasScale)
			{
				PropertyChanging();
				_scale = value;
				_hasScale = true;
				_actualSize = -1;
			}
		}
	}

	[RefreshProperties(RefreshProperties.All)]
	[ResCategory("Data")]
	[DbProviderSpecificTypeProperty(true)]
	public SqlDbType SqlDbType
	{
		get
		{
			return GetMetaTypeOnly().SqlDbType;
		}
		set
		{
			MetaType metaType = _metaType;
			if ((SqlDbType)24 == value)
			{
				throw SQL.InvalidSqlDbType(value);
			}
			if (metaType == null || metaType.SqlDbType != value)
			{
				PropertyTypeChanging();
				_metaType = MetaType.GetMetaTypeFromSqlDbType(value, value == SqlDbType.Structured);
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public object SqlValue
	{
		get
		{
			if (_udtLoadError != null)
			{
				throw _udtLoadError;
			}
			if (_value != null)
			{
				if (_value == DBNull.Value)
				{
					return MetaType.GetNullSqlValue(GetMetaTypeOnly().SqlType);
				}
				if (_value is INullable)
				{
					return _value;
				}
				if (_value is DateTime)
				{
					SqlDbType sqlDbType = GetMetaTypeOnly().SqlDbType;
					if (sqlDbType == SqlDbType.Date || sqlDbType == SqlDbType.DateTime2)
					{
						return _value;
					}
				}
				return MetaType.GetSqlValueFromComVariant(_value);
			}
			if (_sqlBufferReturnValue != null)
			{
				return _sqlBufferReturnValue.SqlValue;
			}
			return null;
		}
		set
		{
			Value = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public string UdtTypeName
	{
		get
		{
			return _udtTypeName ?? string.Empty;
		}
		set
		{
			_udtTypeName = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public string TypeName
	{
		get
		{
			return _typeName ?? string.Empty;
		}
		set
		{
			_typeName = value;
			IsDerivedParameterTypeName = false;
		}
	}

	[RefreshProperties(RefreshProperties.All)]
	[ResCategory("Data")]
	[TypeConverter(typeof(StringConverter))]
	public override object Value
	{
		get
		{
			if (_udtLoadError != null)
			{
				throw _udtLoadError;
			}
			if (_value != null)
			{
				return _value;
			}
			if (_sqlBufferReturnValue != null)
			{
				if (ParameterIsSqlType)
				{
					return _sqlBufferReturnValue.SqlValue;
				}
				return _sqlBufferReturnValue.Value;
			}
			return null;
		}
		set
		{
			_value = value;
			_sqlBufferReturnValue = null;
			_coercedValue = null;
			_valueAsINullable = _value as INullable;
			_isSqlParameterSqlType = _valueAsINullable != null;
			_isNull = _value == null || _value == DBNull.Value || (_isSqlParameterSqlType && _valueAsINullable.IsNull);
			_udtLoadError = null;
			_actualSize = -1;
		}
	}

	[RefreshProperties(RefreshProperties.All)]
	[ResCategory("Data")]
	public override ParameterDirection Direction
	{
		get
		{
			return _direction;
		}
		set
		{
			if (_direction != value)
			{
				if ((uint)(value - 1) > 2u && value != ParameterDirection.ReturnValue)
				{
					throw ADP.InvalidParameterDirection(value);
				}
				PropertyChanging();
				_direction = value;
			}
		}
	}

	public override bool IsNullable
	{
		get
		{
			return _isNullable;
		}
		set
		{
			_isNullable = value;
		}
	}

	public int Offset
	{
		get
		{
			return _offset;
		}
		set
		{
			if (value < 0)
			{
				throw ADP.InvalidOffsetValue(value);
			}
			_offset = value;
		}
	}

	[ResCategory("Data")]
	public override int Size
	{
		get
		{
			int num = _size;
			if (num == 0)
			{
				num = ValueSize(Value);
			}
			return num;
		}
		set
		{
			if (value != _size)
			{
				if (value < -1)
				{
					throw ADP.InvalidSizeValue(value);
				}
				PropertyChanging();
				_size = value;
			}
		}
	}

	[ResCategory("Update")]
	public override string SourceColumn
	{
		get
		{
			return _sourceColumn ?? string.Empty;
		}
		set
		{
			_sourceColumn = value;
		}
	}

	public override bool SourceColumnNullMapping
	{
		get
		{
			return _sourceColumnNullMapping;
		}
		set
		{
			_sourceColumnNullMapping = value;
		}
	}

	[ResCategory("Update")]
	public override DataRowVersion SourceVersion
	{
		get
		{
			DataRowVersion sourceVersion = _sourceVersion;
			if (sourceVersion == (DataRowVersion)0)
			{
				return DataRowVersion.Current;
			}
			return sourceVersion;
		}
		set
		{
			switch (value)
			{
			case DataRowVersion.Original:
			case DataRowVersion.Current:
			case DataRowVersion.Proposed:
			case DataRowVersion.Default:
				_sourceVersion = value;
				break;
			default:
				throw ADP.InvalidDataRowVersion(value);
			}
		}
	}

	private object CoercedValue
	{
		get
		{
			return _coercedValue;
		}
		set
		{
			_coercedValue = value;
		}
	}

	internal bool CoercedValueIsDataFeed
	{
		get
		{
			if (_coercedValue == null)
			{
				GetCoercedValue();
			}
			return _coercedValueIsDataFeed;
		}
	}

	internal bool CoercedValueIsSqlType
	{
		get
		{
			if (_coercedValue == null)
			{
				GetCoercedValue();
			}
			return _coercedValueIsSqlType;
		}
	}

	internal SqlCollation Collation
	{
		get
		{
			return _collation;
		}
		set
		{
			_collation = value;
		}
	}

	internal bool IsNull
	{
		get
		{
			if (_internalMetaType.SqlDbType == SqlDbType.Udt)
			{
				_isNull = _value == null || _value == DBNull.Value || (_isSqlParameterSqlType && _valueAsINullable.IsNull);
			}
			return _isNull;
		}
	}

	internal MetaType InternalMetaType
	{
		get
		{
			return _internalMetaType;
		}
		set
		{
			_internalMetaType = value;
		}
	}

	internal byte PrecisionInternal
	{
		get
		{
			byte b = _precision;
			SqlDbType metaSqlDbTypeOnly = GetMetaSqlDbTypeOnly();
			if (b == 0 && SqlDbType.Decimal == metaSqlDbTypeOnly)
			{
				b = ValuePrecision(SqlValue);
			}
			return b;
		}
		set
		{
			SqlDbType sqlDbType = SqlDbType;
			if (sqlDbType == SqlDbType.Decimal && value > 38)
			{
				throw SQL.PrecisionValueOutOfRange(value);
			}
			if (_precision != value)
			{
				PropertyChanging();
				_precision = value;
			}
		}
	}

	internal bool ParameterIsSqlType
	{
		get
		{
			return _isSqlParameterSqlType;
		}
		set
		{
			_isSqlParameterSqlType = value;
		}
	}

	internal string ParameterNameFixed
	{
		get
		{
			string text = ParameterName;
			if (text.Length > 0 && text[0] != '@')
			{
				text = "@" + text;
			}
			return text;
		}
	}

	internal bool SizeInferred => _size == 0;

	internal INullable ValueAsINullable => _valueAsINullable;

	internal bool IsDerivedParameterTypeName { get; set; }

	public SqlParameter()
	{
		_isNull = true;
		_actualSize = -1;
		_direction = ParameterDirection.Input;
	}

	public SqlParameter(string parameterName, SqlDbType dbType)
		: this()
	{
		ParameterName = parameterName;
		SqlDbType = dbType;
	}

	public SqlParameter(string parameterName, object value)
		: this()
	{
		ParameterName = parameterName;
		Value = value;
	}

	public SqlParameter(string parameterName, SqlDbType dbType, int size)
		: this()
	{
		ParameterName = parameterName;
		SqlDbType = dbType;
		Size = size;
	}

	public SqlParameter(string parameterName, SqlDbType dbType, int size, string sourceColumn)
		: this()
	{
		ParameterName = parameterName;
		SqlDbType = dbType;
		Size = size;
		SourceColumn = sourceColumn;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public SqlParameter(string parameterName, SqlDbType dbType, int size, ParameterDirection direction, bool isNullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
		: this(parameterName, dbType, size, sourceColumn)
	{
		Direction = direction;
		IsNullable = isNullable;
		PrecisionInternal = precision;
		ScaleInternal = scale;
		SourceVersion = sourceVersion;
		Value = value;
	}

	public SqlParameter(string parameterName, SqlDbType dbType, int size, ParameterDirection direction, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, bool sourceColumnNullMapping, object value, string xmlSchemaCollectionDatabase, string xmlSchemaCollectionOwningSchema, string xmlSchemaCollectionName)
		: this()
	{
		ParameterName = parameterName;
		SqlDbType = dbType;
		Size = size;
		Direction = direction;
		PrecisionInternal = precision;
		ScaleInternal = scale;
		SourceColumn = sourceColumn;
		SourceVersion = sourceVersion;
		SourceColumnNullMapping = sourceColumnNullMapping;
		Value = value;
		if (!string.IsNullOrEmpty(xmlSchemaCollectionDatabase) || !string.IsNullOrEmpty(xmlSchemaCollectionOwningSchema) || !string.IsNullOrEmpty(xmlSchemaCollectionName))
		{
			EnsureXmlSchemaCollection();
			_xmlSchemaCollection.Database = xmlSchemaCollectionDatabase;
			_xmlSchemaCollection.OwningSchema = xmlSchemaCollectionOwningSchema;
			_xmlSchemaCollection.Name = xmlSchemaCollectionName;
		}
	}

	private SqlParameter(SqlParameter source)
		: this()
	{
		ADP.CheckArgumentNull(source, "source");
		source.CloneHelper(this);
		if (_value is ICloneable cloneable)
		{
			_value = cloneable.Clone();
		}
	}

	public override void ResetDbType()
	{
		ResetSqlDbType();
	}

	private bool ShouldSerializePrecision()
	{
		return _precision != 0;
	}

	private bool ShouldSerializeScale()
	{
		return _scale != 0;
	}

	private bool ShouldSerializeSqlDbType()
	{
		return _metaType != null;
	}

	public void ResetSqlDbType()
	{
		if (_metaType != null)
		{
			PropertyTypeChanging();
			_metaType = null;
		}
	}

	private void ResetSize()
	{
		if (_size != 0)
		{
			PropertyChanging();
			_size = 0;
		}
	}

	private bool ShouldSerializeSize()
	{
		return _size != 0;
	}

	public override string ToString()
	{
		return ParameterName;
	}

	object ICloneable.Clone()
	{
		return new SqlParameter(this);
	}

	private void CloneHelper(SqlParameter destination)
	{
		destination._value = _value;
		destination._direction = _direction;
		destination._size = _size;
		destination._offset = _offset;
		destination._sourceColumn = _sourceColumn;
		destination._sourceVersion = _sourceVersion;
		destination._sourceColumnNullMapping = _sourceColumnNullMapping;
		destination._isNullable = _isNullable;
		destination._metaType = _metaType;
		destination._collation = _collation;
		if (_xmlSchemaCollection != null)
		{
			destination.EnsureXmlSchemaCollection().CopyFrom(_xmlSchemaCollection);
		}
		destination._udtTypeName = _udtTypeName;
		destination._typeName = _typeName;
		destination._udtLoadError = _udtLoadError;
		destination._parameterName = _parameterName;
		destination._precision = _precision;
		destination._scale = _scale;
		destination._sqlBufferReturnValue = _sqlBufferReturnValue;
		destination._isSqlParameterSqlType = _isSqlParameterSqlType;
		destination._internalMetaType = _internalMetaType;
		destination.CoercedValue = CoercedValue;
		destination._valueAsINullable = _valueAsINullable;
		destination._isNull = _isNull;
		destination._coercedValueIsDataFeed = _coercedValueIsDataFeed;
		destination._coercedValueIsSqlType = _coercedValueIsSqlType;
		destination._actualSize = _actualSize;
		destination.ForceColumnEncryption = ForceColumnEncryption;
	}

	internal void CopyTo(SqlParameter destination)
	{
		ADP.CheckArgumentNull(destination, "destination");
		CloneHelper(destination);
	}

	internal object CompareExchangeParent(object value, object comparand)
	{
		object parent = _parent;
		if (comparand == parent)
		{
			_parent = value;
		}
		return parent;
	}

	private SqlMetaDataXmlSchemaCollection EnsureXmlSchemaCollection()
	{
		if (_xmlSchemaCollection == null)
		{
			_xmlSchemaCollection = new SqlMetaDataXmlSchemaCollection();
		}
		return _xmlSchemaCollection;
	}

	internal void FixStreamDataForNonPLP()
	{
		object coercedValue = GetCoercedValue();
		if (!_coercedValueIsDataFeed)
		{
			return;
		}
		_coercedValueIsDataFeed = false;
		if (coercedValue is TextDataFeed textDataFeed)
		{
			if (Size > 0)
			{
				char[] array = new char[Size];
				int length = textDataFeed._source.ReadBlock(array, 0, Size);
				CoercedValue = new string(array, 0, length);
			}
			else
			{
				CoercedValue = textDataFeed._source.ReadToEnd();
			}
		}
		else if (coercedValue is StreamDataFeed streamDataFeed)
		{
			if (Size > 0)
			{
				byte[] array2 = new byte[Size];
				int i = 0;
				Stream source = streamDataFeed._source;
				int num;
				for (; i < Size; i += num)
				{
					num = source.Read(array2, i, Size - i);
					if (num == 0)
					{
						break;
					}
				}
				if (i < Size)
				{
					Array.Resize(ref array2, i);
				}
				CoercedValue = array2;
			}
			else
			{
				MemoryStream memoryStream = new MemoryStream();
				streamDataFeed._source.CopyTo(memoryStream);
				CoercedValue = memoryStream.ToArray();
			}
		}
		else if (coercedValue is XmlDataFeed xmlDataFeed)
		{
			CoercedValue = MetaType.GetStringFromXml(xmlDataFeed._source);
		}
	}

	private void GetActualFieldsAndProperties(out List<SmiExtendedMetaData> fields, out SmiMetaDataPropertyCollection props, out ParameterPeekAheadValue peekAhead)
	{
		fields = null;
		props = null;
		peekAhead = null;
		object coercedValue = GetCoercedValue();
		if (coercedValue is DataTable dataTable)
		{
			if (dataTable.Columns.Count <= 0)
			{
				throw SQL.NotEnoughColumnsInStructuredType();
			}
			fields = new List<SmiExtendedMetaData>(dataTable.Columns.Count);
			bool[] array = new bool[dataTable.Columns.Count];
			bool flag = false;
			if (dataTable.PrimaryKey != null && dataTable.PrimaryKey.Length != 0)
			{
				DataColumn[] primaryKey = dataTable.PrimaryKey;
				foreach (DataColumn dataColumn in primaryKey)
				{
					array[dataColumn.Ordinal] = true;
					flag = true;
				}
			}
			for (int j = 0; j < dataTable.Columns.Count; j++)
			{
				fields.Add(MetaDataUtilsSmi.SmiMetaDataFromDataColumn(dataTable.Columns[j], dataTable));
				if (!flag && dataTable.Columns[j].Unique)
				{
					array[j] = true;
					flag = true;
				}
			}
			if (flag)
			{
				props = new SmiMetaDataPropertyCollection();
				props[SmiPropertySelector.UniqueKey] = new SmiUniqueKeyProperty(new List<bool>(array));
			}
			return;
		}
		if (coercedValue is SqlDataReader sqlDataReader)
		{
			fields = new List<SmiExtendedMetaData>(sqlDataReader.GetInternalSmiMetaData());
			if (fields.Count <= 0)
			{
				throw SQL.NotEnoughColumnsInStructuredType();
			}
			bool[] array2 = new bool[fields.Count];
			bool flag2 = false;
			for (int k = 0; k < fields.Count; k++)
			{
				if (fields[k] is SmiQueryMetaData { IsKey: { IsNull: false }, IsKey: { Value: not false } })
				{
					array2[k] = true;
					flag2 = true;
				}
			}
			if (flag2)
			{
				props = new SmiMetaDataPropertyCollection();
				props[SmiPropertySelector.UniqueKey] = new SmiUniqueKeyProperty(new List<bool>(array2));
			}
			return;
		}
		if (coercedValue is IEnumerable<SqlDataRecord> enumerable)
		{
			IEnumerator<SqlDataRecord> enumerator = enumerable.GetEnumerator();
			SqlDataRecord sqlDataRecord = null;
			try
			{
				if (enumerator.MoveNext())
				{
					sqlDataRecord = enumerator.Current;
					int fieldCount = sqlDataRecord.FieldCount;
					if (0 < fieldCount)
					{
						bool[] array3 = new bool[fieldCount];
						bool[] array4 = new bool[fieldCount];
						bool[] array5 = new bool[fieldCount];
						int num = -1;
						bool flag3 = false;
						bool flag4 = false;
						int num2 = 0;
						SmiOrderProperty.SmiColumnOrder[] array6 = new SmiOrderProperty.SmiColumnOrder[fieldCount];
						fields = new List<SmiExtendedMetaData>(fieldCount);
						for (int l = 0; l < fieldCount; l++)
						{
							SqlMetaData sqlMetaData = sqlDataRecord.GetSqlMetaData(l);
							fields.Add(MetaDataUtilsSmi.SqlMetaDataToSmiExtendedMetaData(sqlMetaData));
							if (sqlMetaData.IsUniqueKey)
							{
								array3[l] = true;
								flag3 = true;
							}
							if (sqlMetaData.UseServerDefault)
							{
								array4[l] = true;
								flag4 = true;
							}
							PropertyInfo property = sqlMetaData.GetType().GetProperty("SortOrder", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
							MethodInfo getMethod = property.GetGetMethod(nonPublic: true);
							SortOrder sortOrder = (SortOrder)getMethod.Invoke(sqlMetaData, null);
							array6[l].Order = sortOrder;
							if (SortOrder.Unspecified != sortOrder)
							{
								if (fieldCount <= sqlMetaData.SortOrdinal)
								{
									throw SQL.SortOrdinalGreaterThanFieldCount(l, sqlMetaData.SortOrdinal);
								}
								if (array5[sqlMetaData.SortOrdinal])
								{
									throw SQL.DuplicateSortOrdinal(sqlMetaData.SortOrdinal);
								}
								array6[l].SortOrdinal = sqlMetaData.SortOrdinal;
								array5[sqlMetaData.SortOrdinal] = true;
								if (sqlMetaData.SortOrdinal > num)
								{
									num = sqlMetaData.SortOrdinal;
								}
								num2++;
							}
						}
						if (flag3)
						{
							props = new SmiMetaDataPropertyCollection();
							props[SmiPropertySelector.UniqueKey] = new SmiUniqueKeyProperty(new List<bool>(array3));
						}
						if (flag4)
						{
							if (props == null)
							{
								props = new SmiMetaDataPropertyCollection();
							}
							props[SmiPropertySelector.DefaultFields] = new SmiDefaultFieldsProperty(new List<bool>(array4));
						}
						if (0 < num2)
						{
							if (num >= num2)
							{
								int m;
								for (m = 0; m < num2 && array5[m]; m++)
								{
								}
								throw SQL.MissingSortOrdinal(m);
							}
							if (props == null)
							{
								props = new SmiMetaDataPropertyCollection();
							}
							props[SmiPropertySelector.SortOrder] = new SmiOrderProperty(new List<SmiOrderProperty.SmiColumnOrder>(array6));
						}
						peekAhead = new ParameterPeekAheadValue
						{
							Enumerator = enumerator,
							FirstRecord = sqlDataRecord
						};
						enumerator = null;
						return;
					}
					throw SQL.NotEnoughColumnsInStructuredType();
				}
				throw SQL.IEnumerableOfSqlDataRecordHasNoRows();
			}
			finally
			{
				enumerator?.Dispose();
			}
		}
		if (!(coercedValue is DbDataReader dbDataReader))
		{
			return;
		}
		DataTable schemaTable = dbDataReader.GetSchemaTable();
		if (schemaTable.Rows.Count <= 0)
		{
			throw SQL.NotEnoughColumnsInStructuredType();
		}
		int count = schemaTable.Rows.Count;
		fields = new List<SmiExtendedMetaData>(count);
		bool[] array7 = new bool[count];
		bool flag5 = false;
		int ordinal = schemaTable.Columns[SchemaTableColumn.IsKey].Ordinal;
		int ordinal2 = schemaTable.Columns[SchemaTableColumn.ColumnOrdinal].Ordinal;
		for (int n = 0; n < count; n++)
		{
			DataRow dataRow = schemaTable.Rows[n];
			SmiExtendedMetaData smiExtendedMetaData = MetaDataUtilsSmi.SmiMetaDataFromSchemaTableRow(dataRow);
			int num3 = n;
			if (!dataRow.IsNull(ordinal2))
			{
				num3 = (int)dataRow[ordinal2];
			}
			if (num3 >= count || num3 < 0)
			{
				throw SQL.InvalidSchemaTableOrdinals();
			}
			while (num3 > fields.Count)
			{
				fields.Add(null);
			}
			if (fields.Count == num3)
			{
				fields.Add(smiExtendedMetaData);
			}
			else
			{
				if (fields[num3] != null)
				{
					throw SQL.InvalidSchemaTableOrdinals();
				}
				fields[num3] = smiExtendedMetaData;
			}
			if (!dataRow.IsNull(ordinal) && (bool)dataRow[ordinal])
			{
				array7[num3] = true;
				flag5 = true;
			}
		}
		if (flag5)
		{
			props = new SmiMetaDataPropertyCollection();
			props[SmiPropertySelector.UniqueKey] = new SmiUniqueKeyProperty(new List<bool>(array7));
		}
	}

	internal byte GetActualScale()
	{
		if (ShouldSerializeScale())
		{
			return ScaleInternal;
		}
		if (GetMetaTypeOnly().IsVarTime)
		{
			return 7;
		}
		return ValueScale(CoercedValue);
	}

	internal int GetActualSize()
	{
		MetaType metaType = InternalMetaType;
		SqlDbType sqlDbType = metaType.SqlDbType;
		if (_actualSize == -1 || sqlDbType == SqlDbType.Udt)
		{
			_actualSize = 0;
			object coercedValue = GetCoercedValue();
			bool flag = false;
			if (IsNull && !metaType.IsVarTime)
			{
				return 0;
			}
			if (sqlDbType == SqlDbType.Variant)
			{
				metaType = MetaType.GetMetaTypeFromValue(coercedValue, streamAllowed: false);
				sqlDbType = MetaType.GetSqlDataType(metaType.TDSType, 0u, 0).SqlDbType;
				flag = true;
			}
			if (metaType.IsFixed)
			{
				_actualSize = metaType.FixedLength;
			}
			else
			{
				int num = 0;
				switch (sqlDbType)
				{
				case SqlDbType.NChar:
				case SqlDbType.NText:
				case SqlDbType.NVarChar:
				case SqlDbType.Xml:
					num = ((!_isNull && !_coercedValueIsDataFeed) ? StringSize(coercedValue, _coercedValueIsSqlType) : 0);
					_actualSize = (ShouldSerializeSize() ? Size : 0);
					_actualSize = ((ShouldSerializeSize() && _actualSize <= num) ? _actualSize : num);
					if (_actualSize == -1)
					{
						_actualSize = num;
					}
					_actualSize <<= 1;
					break;
				case SqlDbType.Char:
				case SqlDbType.Text:
				case SqlDbType.VarChar:
					num = ((!_isNull && !_coercedValueIsDataFeed) ? StringSize(coercedValue, _coercedValueIsSqlType) : 0);
					_actualSize = (ShouldSerializeSize() ? Size : 0);
					_actualSize = ((ShouldSerializeSize() && _actualSize <= num) ? _actualSize : num);
					if (_actualSize == -1)
					{
						_actualSize = num;
					}
					break;
				case SqlDbType.Binary:
				case SqlDbType.Image:
				case SqlDbType.Timestamp:
				case SqlDbType.VarBinary:
					num = ((!_isNull && !_coercedValueIsDataFeed) ? BinarySize(coercedValue, _coercedValueIsSqlType) : 0);
					_actualSize = (ShouldSerializeSize() ? Size : 0);
					_actualSize = ((ShouldSerializeSize() && _actualSize <= num) ? _actualSize : num);
					if (_actualSize == -1)
					{
						_actualSize = num;
					}
					break;
				case SqlDbType.Udt:
					if (!IsNull)
					{
						num = AssemblyCache.GetLength(coercedValue);
					}
					break;
				case SqlDbType.Structured:
					num = -1;
					break;
				case SqlDbType.Time:
					_actualSize = (flag ? 5 : MetaType.GetTimeSizeFromScale(GetActualScale()));
					break;
				case SqlDbType.DateTime2:
					_actualSize = 3 + (flag ? 5 : MetaType.GetTimeSizeFromScale(GetActualScale()));
					break;
				case SqlDbType.DateTimeOffset:
					_actualSize = 5 + (flag ? 5 : MetaType.GetTimeSizeFromScale(GetActualScale()));
					break;
				}
				if (flag && num > 8000)
				{
					throw SQL.ParameterInvalidVariant(ParameterName);
				}
			}
		}
		return _actualSize;
	}

	internal byte GetActualPrecision()
	{
		if (!ShouldSerializePrecision())
		{
			return ValuePrecision(CoercedValue);
		}
		return PrecisionInternal;
	}

	internal object GetCoercedValue()
	{
		if (_coercedValue == null || _internalMetaType.SqlDbType == SqlDbType.Udt)
		{
			bool flag = Value is DataFeed;
			if (IsNull || flag)
			{
				_coercedValue = Value;
				_coercedValueIsSqlType = _coercedValue != null && _isSqlParameterSqlType;
				_coercedValueIsDataFeed = flag;
				_actualSize = ((!IsNull) ? (-1) : 0);
			}
			else
			{
				_coercedValue = CoerceValue(Value, _internalMetaType, out _coercedValueIsDataFeed, out var typeChanged);
				_coercedValueIsSqlType = _isSqlParameterSqlType && !typeChanged;
				_actualSize = -1;
			}
		}
		return _coercedValue;
	}

	internal int GetParameterSize()
	{
		if (!ShouldSerializeSize())
		{
			return ValueSize(CoercedValue);
		}
		return Size;
	}

	internal SmiParameterMetaData GetMetadataForTypeInfo()
	{
		if (_internalMetaType == null)
		{
			_internalMetaType = GetMetaTypeOnly();
		}
		ParameterPeekAheadValue peekAhead;
		return MetaDataForSmi(out peekAhead);
	}

	internal SmiParameterMetaData MetaDataForSmi(out ParameterPeekAheadValue peekAhead)
	{
		peekAhead = null;
		MetaType metaType = ValidateTypeLengths(yukonOrNewer: true);
		long num = GetActualSize();
		long num2 = Size;
		if (!metaType.IsLong)
		{
			if (metaType.SqlDbType == SqlDbType.NChar || metaType.SqlDbType == SqlDbType.NVarChar)
			{
				num /= 2;
			}
			if (num > num2)
			{
				num2 = num;
			}
		}
		if (num2 == 0L)
		{
			if (metaType.SqlDbType == SqlDbType.Binary || metaType.SqlDbType == SqlDbType.VarBinary)
			{
				num2 = 8000L;
			}
			else if (metaType.SqlDbType == SqlDbType.Char || metaType.SqlDbType == SqlDbType.VarChar)
			{
				num2 = 8000L;
			}
			else if (metaType.SqlDbType == SqlDbType.NChar || metaType.SqlDbType == SqlDbType.NVarChar)
			{
				num2 = 4000L;
			}
		}
		else if ((num2 > 8000 && (SqlDbType.Binary == metaType.SqlDbType || SqlDbType.VarBinary == metaType.SqlDbType)) || (num2 > 8000 && (SqlDbType.Char == metaType.SqlDbType || SqlDbType.VarChar == metaType.SqlDbType)) || (num2 > 4000 && (SqlDbType.NChar == metaType.SqlDbType || SqlDbType.NVarChar == metaType.SqlDbType)))
		{
			num2 = -1L;
		}
		int num3 = LocaleId;
		if (num3 == 0 && metaType.IsCharType)
		{
			num3 = ((!(GetCoercedValue() is SqlString { IsNull: false } sqlString)) ? CultureInfo.CurrentCulture.LCID : sqlString.LCID);
		}
		SqlCompareOptions sqlCompareOptions = CompareInfo;
		if (sqlCompareOptions == SqlCompareOptions.None && metaType.IsCharType)
		{
			sqlCompareOptions = ((!(GetCoercedValue() is SqlString { IsNull: false } sqlString2)) ? SmiMetaData.GetDefaultForType(metaType.SqlDbType).CompareOptions : sqlString2.SqlCompareOptions);
		}
		string text = null;
		string text2 = null;
		string text3 = null;
		if (SqlDbType.Xml == metaType.SqlDbType)
		{
			text = XmlSchemaCollectionDatabase;
			text2 = XmlSchemaCollectionOwningSchema;
			text3 = XmlSchemaCollectionName;
		}
		else if (SqlDbType.Udt == metaType.SqlDbType || (SqlDbType.Structured == metaType.SqlDbType && !string.IsNullOrEmpty(TypeName)))
		{
			string[] array = ((metaType.SqlDbType != SqlDbType.Udt) ? ParseTypeName(TypeName, isUdtTypeName: false) : ParseTypeName(UdtTypeName, isUdtTypeName: true));
			if (array.Length == 1)
			{
				text3 = array[0];
			}
			else if (array.Length == 2)
			{
				text2 = array[0];
				text3 = array[1];
			}
			else
			{
				if (array.Length != 3)
				{
					throw ADP.ArgumentOutOfRange("names");
				}
				text = array[0];
				text2 = array[1];
				text3 = array[2];
			}
			if ((!string.IsNullOrEmpty(text) && 255 < text.Length) || (!string.IsNullOrEmpty(text2) && 255 < text2.Length) || (!string.IsNullOrEmpty(text3) && 255 < text3.Length))
			{
				throw ADP.ArgumentOutOfRange("names");
			}
		}
		byte b = GetActualPrecision();
		byte actualScale = GetActualScale();
		if (metaType.SqlDbType == SqlDbType.Decimal && b == 0)
		{
			b = 29;
		}
		List<SmiExtendedMetaData> fields = null;
		SmiMetaDataPropertyCollection props = null;
		if (metaType.SqlDbType == SqlDbType.Structured)
		{
			GetActualFieldsAndProperties(out fields, out props, out peekAhead);
		}
		return new SmiParameterMetaData(metaType.SqlDbType, num2, b, actualScale, num3, sqlCompareOptions, null, SqlDbType.Structured == metaType.SqlDbType, fields, props, ParameterNameFixed, text, text2, text3, Direction);
	}

	[Conditional("DEBUG")]
	internal void AssertCachedPropertiesAreValid()
	{
	}

	[Conditional("DEBUG")]
	internal void AssertPropertiesAreValid(object value, bool? isSqlType = null, bool? isDataFeed = null, bool? isNull = null)
	{
	}

	private SqlDbType GetMetaSqlDbTypeOnly()
	{
		MetaType metaType = _metaType;
		if (metaType == null)
		{
			metaType = MetaType.GetDefaultMetaType();
		}
		return metaType.SqlDbType;
	}

	private MetaType GetMetaTypeOnly()
	{
		if (_metaType != null)
		{
			return _metaType;
		}
		if (_value != null && DBNull.Value != _value)
		{
			Type type = _value.GetType();
			if (type == typeof(char))
			{
				_value = _value.ToString();
				type = typeof(string);
			}
			else if (type == typeof(char[]))
			{
				_value = new string((char[])_value);
				type = typeof(string);
			}
			return MetaType.GetMetaTypeFromType(type);
		}
		if (_sqlBufferReturnValue != null)
		{
			Type typeFromStorageType = _sqlBufferReturnValue.GetTypeFromStorageType(_isSqlParameterSqlType);
			if (typeFromStorageType != null)
			{
				return MetaType.GetMetaTypeFromType(typeFromStorageType);
			}
		}
		return MetaType.GetDefaultMetaType();
	}

	internal void Prepare(SqlCommand cmd)
	{
		if (_metaType == null)
		{
			throw ADP.PrepareParameterType(cmd);
		}
		if (!ShouldSerializeSize() && !_metaType.IsFixed)
		{
			throw ADP.PrepareParameterSize(cmd);
		}
		if (!ShouldSerializePrecision() && !ShouldSerializeScale() && _metaType.SqlDbType == SqlDbType.Decimal)
		{
			throw ADP.PrepareParameterScale(cmd, SqlDbType.ToString());
		}
	}

	private void PropertyChanging()
	{
		_internalMetaType = null;
	}

	private void PropertyTypeChanging()
	{
		PropertyChanging();
		CoercedValue = null;
	}

	internal void ResetParent()
	{
		_parent = null;
	}

	internal void SetSqlBuffer(SqlBuffer buff)
	{
		_sqlBufferReturnValue = buff;
		_value = null;
		_coercedValue = null;
		_isNull = _sqlBufferReturnValue.IsNull;
		_coercedValueIsDataFeed = false;
		_coercedValueIsSqlType = false;
		_udtLoadError = null;
		_actualSize = -1;
	}

	internal void SetUdtLoadError(Exception e)
	{
		_udtLoadError = e;
	}

	internal void Validate(int index, bool isCommandProc)
	{
		MetaType metaType = (_internalMetaType = GetMetaTypeOnly());
		if (ADP.IsDirection(this, ParameterDirection.Output) && !ADP.IsDirection(this, ParameterDirection.ReturnValue) && !metaType.IsFixed && !ShouldSerializeSize() && (_value == null || Convert.IsDBNull(_value)) && SqlDbType != SqlDbType.Timestamp && SqlDbType != SqlDbType.Udt && SqlDbType != SqlDbType.Xml && !metaType.IsVarTime)
		{
			throw ADP.UninitializedParameterSize(index, metaType.ClassType);
		}
		if (metaType.SqlDbType != SqlDbType.Udt && Direction != ParameterDirection.Output)
		{
			GetCoercedValue();
		}
		if (metaType.SqlDbType == SqlDbType.Udt)
		{
			if (string.IsNullOrEmpty(UdtTypeName))
			{
				throw SQL.MustSetUdtTypeNameForUdtParams();
			}
		}
		else if (!string.IsNullOrEmpty(UdtTypeName))
		{
			throw SQL.UnexpectedUdtTypeNameForNonUdtParams();
		}
		if (metaType.SqlDbType == SqlDbType.Structured)
		{
			if (!isCommandProc && string.IsNullOrEmpty(TypeName))
			{
				throw SQL.MustSetTypeNameForParam(metaType.TypeName, ParameterName);
			}
			if (Direction != ParameterDirection.Input)
			{
				throw SQL.UnsupportedTVPOutputParameter(Direction, ParameterName);
			}
			if (GetCoercedValue() == DBNull.Value)
			{
				throw SQL.DBNullNotSupportedForTVPValues(ParameterName);
			}
		}
		else if (!string.IsNullOrEmpty(TypeName))
		{
			throw SQL.UnexpectedTypeNameForNonStructParams(ParameterName);
		}
	}

	internal MetaType ValidateTypeLengths(bool yukonOrNewer)
	{
		MetaType metaType = InternalMetaType;
		if (metaType.SqlDbType != SqlDbType.Udt && !metaType.IsFixed && !metaType.IsLong)
		{
			long num = GetActualSize();
			long num2 = Size;
			long num3 = ((!(metaType.IsNCharType && yukonOrNewer)) ? ((num2 > num) ? num2 : num) : ((num2 * 2 > num) ? (num2 * 2) : num));
			if (num3 > 8000 || _coercedValueIsDataFeed || num2 == -1 || num == -1)
			{
				if (yukonOrNewer)
				{
					metaType = (InternalMetaType = (_metaType = MetaType.GetMaxMetaTypeFromMetaType(metaType)));
					if (!metaType.IsPlp)
					{
						if (metaType.SqlDbType == SqlDbType.Xml)
						{
							throw ADP.InvalidMetaDataValue();
						}
						if (metaType.SqlDbType == SqlDbType.NVarChar || metaType.SqlDbType == SqlDbType.VarChar || metaType.SqlDbType == SqlDbType.VarBinary)
						{
							Size = -1;
						}
					}
				}
				else
				{
					switch (metaType.SqlDbType)
					{
					case SqlDbType.Binary:
					case SqlDbType.VarBinary:
						metaType = (InternalMetaType = (_metaType = MetaType.GetMetaTypeFromSqlDbType(SqlDbType.Image, isMultiValued: false)));
						break;
					case SqlDbType.Char:
					case SqlDbType.VarChar:
						metaType = (InternalMetaType = (_metaType = MetaType.GetMetaTypeFromSqlDbType(SqlDbType.Text, isMultiValued: false)));
						break;
					case SqlDbType.NChar:
					case SqlDbType.NVarChar:
						metaType = (InternalMetaType = (_metaType = MetaType.GetMetaTypeFromSqlDbType(SqlDbType.NText, isMultiValued: false)));
						break;
					}
				}
			}
		}
		return metaType;
	}

	private byte ValuePrecision(object value)
	{
		if (value is SqlDecimal sqlDecimal)
		{
			if (sqlDecimal.IsNull)
			{
				return 0;
			}
			return sqlDecimal.Precision;
		}
		return ValuePrecisionCore(value);
	}

	private byte ValueScale(object value)
	{
		if (value is SqlDecimal sqlDecimal)
		{
			if (sqlDecimal.IsNull)
			{
				return 0;
			}
			return sqlDecimal.Scale;
		}
		return ValueScaleCore(value);
	}

	private int ValueSize(object value)
	{
		if (value is SqlString sqlString)
		{
			if (sqlString.IsNull)
			{
				return 0;
			}
			return sqlString.Value.Length;
		}
		if (value is SqlChars sqlChars)
		{
			if (sqlChars.IsNull)
			{
				return 0;
			}
			return sqlChars.Value.Length;
		}
		if (value is SqlBinary sqlBinary)
		{
			if (sqlBinary.IsNull)
			{
				return 0;
			}
			return sqlBinary.Length;
		}
		if (value is SqlBytes sqlBytes)
		{
			if (sqlBytes.IsNull)
			{
				return 0;
			}
			return (int)sqlBytes.Length;
		}
		if (value is DataFeed)
		{
			return 0;
		}
		return ValueSizeCore(value);
	}

	private byte ValuePrecisionCore(object value)
	{
		if (value is decimal num)
		{
			return ((SqlDecimal)num).Precision;
		}
		return 0;
	}

	private byte ValueScaleCore(object value)
	{
		if (value is decimal d)
		{
			return (byte)((decimal.GetBits(d)[3] & 0xFF0000) >> 16);
		}
		return 0;
	}

	private int ValueSizeCore(object value)
	{
		if (!ADP.IsNull(value))
		{
			if (value is string text)
			{
				return text.Length;
			}
			if (value is byte[] array)
			{
				return array.Length;
			}
			if (value is char[] array2)
			{
				return array2.Length;
			}
			if (value is byte || value is char)
			{
				return 1;
			}
		}
		return 0;
	}

	internal static object CoerceValue(object value, MetaType destinationType, out bool coercedToDataFeed, out bool typeChanged, bool allowStreaming = true)
	{
		coercedToDataFeed = false;
		typeChanged = false;
		Type type = value.GetType();
		if (destinationType.ClassType != typeof(object) && destinationType.ClassType != type && (destinationType.SqlType != type || destinationType.SqlDbType == SqlDbType.Xml))
		{
			try
			{
				typeChanged = true;
				if (destinationType.ClassType == typeof(string))
				{
					if (type == typeof(SqlXml))
					{
						value = MetaType.GetStringFromXml(((SqlXml)value).CreateReader());
					}
					else if (type == typeof(SqlString))
					{
						typeChanged = false;
					}
					else if (typeof(XmlReader).IsAssignableFrom(type))
					{
						if (allowStreaming)
						{
							coercedToDataFeed = true;
							value = new XmlDataFeed((XmlReader)value);
						}
						else
						{
							value = MetaType.GetStringFromXml((XmlReader)value);
						}
					}
					else if (type == typeof(char[]))
					{
						value = new string((char[])value);
					}
					else if (type == typeof(SqlChars))
					{
						value = new string(((SqlChars)value).Value);
					}
					else
					{
						TextReader textReader = value as TextReader;
						if (textReader != null && allowStreaming)
						{
							coercedToDataFeed = true;
							value = new TextDataFeed(textReader);
						}
						else
						{
							value = Convert.ChangeType(value, destinationType.ClassType, null);
						}
					}
				}
				else if (destinationType.DbType == DbType.Currency && type == typeof(string))
				{
					value = decimal.Parse((string)value, NumberStyles.Currency, null);
				}
				else if (type == typeof(SqlBytes) && destinationType.ClassType == typeof(byte[]))
				{
					typeChanged = false;
				}
				else if (type == typeof(string) && destinationType.SqlDbType == SqlDbType.Time)
				{
					value = TimeSpan.Parse((string)value);
				}
				else if (type == typeof(string) && destinationType.SqlDbType == SqlDbType.DateTimeOffset)
				{
					value = DateTimeOffset.Parse((string)value, null);
				}
				else if (type == typeof(DateTime) && destinationType.SqlDbType == SqlDbType.DateTimeOffset)
				{
					value = new DateTimeOffset((DateTime)value);
				}
				else if (243 == destinationType.TDSType && (value is DataTable || value is DbDataReader || value is IEnumerable<SqlDataRecord>))
				{
					typeChanged = false;
				}
				else if (destinationType.ClassType == typeof(byte[]) && allowStreaming && value is Stream source)
				{
					coercedToDataFeed = true;
					value = new StreamDataFeed(source);
				}
				else
				{
					value = Convert.ChangeType(value, destinationType.ClassType, null);
				}
			}
			catch (Exception ex)
			{
				if (!ADP.IsCatchableExceptionType(ex))
				{
					throw;
				}
				throw ADP.ParameterConversionFailed(value, destinationType.ClassType, ex);
			}
		}
		return value;
	}

	private static int StringSize(object value, bool isSqlType)
	{
		if (isSqlType)
		{
			if (value is SqlString sqlString)
			{
				return sqlString.Value.Length;
			}
			if (value is SqlChars sqlChars)
			{
				return sqlChars.Value.Length;
			}
		}
		else
		{
			if (value is string text)
			{
				return text.Length;
			}
			if (value is char[] array)
			{
				return array.Length;
			}
			if (value is char)
			{
				return 1;
			}
		}
		return 0;
	}

	private static int BinarySize(object value, bool isSqlType)
	{
		if (isSqlType)
		{
			if (value is SqlBinary sqlBinary)
			{
				return sqlBinary.Length;
			}
			if (value is SqlBytes sqlBytes)
			{
				return sqlBytes.Value.Length;
			}
		}
		else
		{
			if (value is byte[] array)
			{
				return array.Length;
			}
			if (value is byte)
			{
				return 1;
			}
		}
		return 0;
	}

	internal static string[] ParseTypeName(string typeName, bool isUdtTypeName)
	{
		try
		{
			string property = (isUdtTypeName ? Strings.SQL_UDTTypeName : Strings.SQL_TypeName);
			return MultipartIdentifier.ParseMultipartIdentifier(typeName, "[\"", "]\"", '.', 3, removequotes: true, property, ThrowOnEmptyMultipartName: true);
		}
		catch (ArgumentException)
		{
			if (isUdtTypeName)
			{
				throw SQL.InvalidUdt3PartNameFormat();
			}
			throw SQL.InvalidParameterTypeNameFormat();
		}
	}
}

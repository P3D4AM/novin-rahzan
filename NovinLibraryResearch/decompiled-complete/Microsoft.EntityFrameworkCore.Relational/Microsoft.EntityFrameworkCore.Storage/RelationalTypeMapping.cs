using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public abstract class RelationalTypeMapping : CoreTypeMapping
{
	protected readonly struct RelationalTypeMappingParameters
	{
		public CoreTypeMappingParameters CoreParameters { get; }

		public string StoreType { get; }

		public DbType? DbType { get; }

		public bool Unicode { get; }

		public int? Size { get; }

		public int? Precision { get; }

		public int? Scale { get; }

		public bool FixedLength { get; }

		public StoreTypePostfix StoreTypePostfix { get; }

		public RelationalTypeMappingParameters(CoreTypeMappingParameters coreParameters, [JetBrains.Annotations.NotNull] string storeType, StoreTypePostfix storeTypePostfix = StoreTypePostfix.None, DbType? dbType = null, bool unicode = false, int? size = null, bool fixedLength = false, int? precision = null, int? scale = null)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(storeType, "storeType");
			ConverterMappingHints converterMappingHints = coreParameters.Converter?.MappingHints;
			CoreParameters = coreParameters;
			StoreType = storeType;
			StoreTypePostfix = storeTypePostfix;
			DbType = dbType;
			Unicode = unicode;
			Size = size ?? converterMappingHints?.Size;
			Precision = precision ?? converterMappingHints?.Precision;
			Scale = scale ?? converterMappingHints?.Scale;
			FixedLength = fixedLength;
		}

		public RelationalTypeMappingParameters WithTypeMappingInfo(in RelationalTypeMappingInfo mappingInfo)
		{
			return new RelationalTypeMappingParameters(CoreParameters, mappingInfo.StoreTypeName ?? StoreType, StoreTypePostfix, DbType, mappingInfo.IsUnicode ?? Unicode, mappingInfo.Size ?? Size, mappingInfo.IsFixedLength ?? FixedLength, mappingInfo.Precision ?? Precision, mappingInfo.Scale ?? Scale);
		}

		public RelationalTypeMappingParameters WithStoreTypeAndSize([JetBrains.Annotations.NotNull] string storeType, int? size, StoreTypePostfix? storeTypePostfix = null)
		{
			return new RelationalTypeMappingParameters(CoreParameters, storeType, storeTypePostfix ?? StoreTypePostfix, DbType, Unicode, size, FixedLength, Precision, Scale);
		}

		public RelationalTypeMappingParameters WithPrecisionAndScale(int? precision, int? scale)
		{
			return new RelationalTypeMappingParameters(CoreParameters, StoreType, StoreTypePostfix, DbType, Unicode, Size, FixedLength, precision, scale);
		}

		public RelationalTypeMappingParameters WithComposedConverter([JetBrains.Annotations.CanBeNull] ValueConverter converter)
		{
			return new RelationalTypeMappingParameters(CoreParameters.WithComposedConverter(converter), StoreType, StoreTypePostfix, DbType, Unicode, Size, FixedLength, Precision, Scale);
		}
	}

	private class NullTypeMapping : RelationalTypeMapping
	{
		public NullTypeMapping(string storeType)
			: base(storeType, typeof(object))
		{
		}

		protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
		{
			return this;
		}
	}

	private readonly bool _quirk19128;

	private static readonly MethodInfo _getFieldValueMethod = GetDataReaderMethod("GetFieldValue");

	private static readonly IDictionary<Type, MethodInfo> _getXMethods = new Dictionary<Type, MethodInfo>
	{
		{
			typeof(bool),
			GetDataReaderMethod("GetBoolean")
		},
		{
			typeof(byte),
			GetDataReaderMethod("GetByte")
		},
		{
			typeof(char),
			GetDataReaderMethod("GetChar")
		},
		{
			typeof(DateTime),
			GetDataReaderMethod("GetDateTime")
		},
		{
			typeof(decimal),
			GetDataReaderMethod("GetDecimal")
		},
		{
			typeof(double),
			GetDataReaderMethod("GetDouble")
		},
		{
			typeof(float),
			GetDataReaderMethod("GetFloat")
		},
		{
			typeof(Guid),
			GetDataReaderMethod("GetGuid")
		},
		{
			typeof(short),
			GetDataReaderMethod("GetInt16")
		},
		{
			typeof(int),
			GetDataReaderMethod("GetInt32")
		},
		{
			typeof(long),
			GetDataReaderMethod("GetInt64")
		},
		{
			typeof(string),
			GetDataReaderMethod("GetString")
		}
	};

	public static readonly RelationalTypeMapping NullMapping = new NullTypeMapping("NULL");

	protected new virtual RelationalTypeMappingParameters Parameters { get; }

	public virtual StoreTypePostfix StoreTypePostfix => Parameters.StoreTypePostfix;

	public virtual string StoreType { get; }

	public virtual string StoreTypeNameBase { get; }

	public virtual DbType? DbType => Parameters.DbType;

	public virtual bool IsUnicode => Parameters.Unicode;

	public virtual int? Size => Parameters.Size;

	public virtual bool IsFixedLength => Parameters.FixedLength;

	protected virtual string SqlLiteralFormatString { get; } = "{0}";


	private static MethodInfo GetDataReaderMethod(string name)
	{
		return typeof(DbDataReader).GetRuntimeMethod(name, new Type[1] { typeof(int) });
	}

	protected RelationalTypeMapping(RelationalTypeMappingParameters parameters)
		: base(parameters.CoreParameters)
	{
		Parameters = parameters;
		string text = parameters.StoreType;
		if (text != null)
		{
			text = ProcessStoreType(storeTypeNameBase: StoreTypeNameBase = GetBaseName(text), parameters: parameters, storeType: text);
		}
		StoreType = text;
		_quirk19128 = AppContext.TryGetSwitch("Microsoft.EntityFrameworkCore.Issue19825", out var isEnabled) && isEnabled;
	}

	protected virtual string ProcessStoreType(RelationalTypeMappingParameters parameters, [JetBrains.Annotations.NotNull] string storeType, [JetBrains.Annotations.NotNull] string storeTypeNameBase)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(storeType, "storeType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(storeTypeNameBase, "storeTypeNameBase");
		int? size = parameters.Size;
		if (size.HasValue && parameters.StoreTypePostfix == StoreTypePostfix.Size)
		{
			int? num = size;
			storeType = storeTypeNameBase + "(" + num + ")";
		}
		else if (parameters.StoreTypePostfix == StoreTypePostfix.PrecisionAndScale || parameters.StoreTypePostfix == StoreTypePostfix.Precision)
		{
			int? precision = parameters.Precision;
			if (precision.HasValue)
			{
				int? scale = parameters.Scale;
				string? text2;
				if (scale.HasValue && parameters.StoreTypePostfix != StoreTypePostfix.Precision)
				{
					int? num = precision;
					string? text = num.ToString();
					num = scale;
					text2 = text + "," + num;
				}
				else
				{
					text2 = precision.ToString();
				}
				storeType = storeTypeNameBase + "(" + text2 + ")";
			}
		}
		return storeType;
	}

	private static string GetBaseName(string storeType)
	{
		int num = storeType.IndexOf("(", StringComparison.Ordinal);
		if (num >= 0)
		{
			storeType = storeType.Substring(0, num);
		}
		return storeType;
	}

	protected RelationalTypeMapping([JetBrains.Annotations.NotNull] string storeType, [JetBrains.Annotations.NotNull] Type clrType, DbType? dbType = null, bool unicode = false, int? size = null)
		: this(new RelationalTypeMappingParameters(new CoreTypeMappingParameters(clrType), storeType, StoreTypePostfix.None, dbType, unicode, size))
	{
	}

	protected abstract RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters);

	public virtual RelationalTypeMapping Clone([JetBrains.Annotations.NotNull] string storeType, int? size)
	{
		return Clone(Parameters.WithStoreTypeAndSize(storeType, size));
	}

	public virtual RelationalTypeMapping Clone(int? precision, int? scale)
	{
		return Clone(Parameters.WithPrecisionAndScale(precision, scale));
	}

	public override CoreTypeMapping Clone(ValueConverter converter)
	{
		return Clone(Parameters.WithComposedConverter(converter));
	}

	public virtual RelationalTypeMapping Clone(in RelationalTypeMappingInfo mappingInfo)
	{
		return Clone(Parameters.WithTypeMappingInfo(in mappingInfo));
	}

	public virtual DbParameter CreateParameter([JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] object value, bool? nullable = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(command, "command");
		DbParameter dbParameter = command.CreateParameter();
		dbParameter.Direction = ParameterDirection.Input;
		dbParameter.ParameterName = name;
		if (!_quirk19128)
		{
			value = ConvertUnderlyingEnumValueToEnum(value);
		}
		if (Converter != null)
		{
			value = Converter.ConvertToProvider(value);
		}
		dbParameter.Value = value ?? DBNull.Value;
		if (nullable.HasValue)
		{
			dbParameter.IsNullable = nullable.Value;
		}
		if (DbType.HasValue)
		{
			dbParameter.DbType = DbType.Value;
		}
		ConfigureParameter(dbParameter);
		return dbParameter;
	}

	private object ConvertUnderlyingEnumValueToEnum(object value)
	{
		if (value == null || !value.GetType().IsInteger() || !ClrType.UnwrapNullableType().IsEnum)
		{
			return value;
		}
		return Enum.ToObject(ClrType.UnwrapNullableType(), value);
	}

	protected virtual void ConfigureParameter([JetBrains.Annotations.NotNull] DbParameter parameter)
	{
	}

	public virtual string GenerateSqlLiteral([JetBrains.Annotations.CanBeNull] object value)
	{
		value = ConvertUnderlyingEnumValueToEnum(value);
		if (Converter != null)
		{
			value = Converter.ConvertToProvider(value);
		}
		return GenerateProviderValueSqlLiteral(value);
	}

	public virtual string GenerateProviderValueSqlLiteral([JetBrains.Annotations.CanBeNull] object value)
	{
		if (value != null)
		{
			return GenerateNonNullSqlLiteral(value);
		}
		return "NULL";
	}

	protected virtual string GenerateNonNullSqlLiteral([JetBrains.Annotations.NotNull] object value)
	{
		return string.Format(CultureInfo.InvariantCulture, SqlLiteralFormatString, Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(value, "value"));
	}

	public virtual MethodInfo GetDataReaderMethod()
	{
		return GetDataReaderMethod((Converter?.ProviderClrType ?? ClrType).UnwrapNullableType());
	}

	public static MethodInfo GetDataReaderMethod([JetBrains.Annotations.NotNull] Type type)
	{
		if (!_getXMethods.TryGetValue(type, out var value))
		{
			return _getFieldValueMethod.MakeGenericMethod(type);
		}
		return value;
	}

	public virtual Expression CustomizeDataReaderExpression([JetBrains.Annotations.NotNull] Expression expression)
	{
		return expression;
	}
}

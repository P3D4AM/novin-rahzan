using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;

namespace Microsoft.EntityFrameworkCore.Storage.Internal;

public static class DbParameterCollectionExtensions
{
	public static string FormatParameters([JetBrains.Annotations.NotNull] this DbParameterCollection parameters, bool logParameterValues)
	{
		return (from DbParameter p in parameters
			select FormatParameter(p.ParameterName, logParameterValues ? p.Value : "?", logParameterValues, p.Direction, p.DbType, p.IsNullable, p.Size, p.Precision, p.Scale)).Join();
	}

	public static string FormatParameter([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] object value, bool hasValue, ParameterDirection direction, DbType dbType, bool nullable, int size, byte precision, byte scale)
	{
		StringBuilder stringBuilder = new StringBuilder();
		Type type = value?.GetType();
		stringBuilder.Append(name).Append("=");
		FormatParameterValue(stringBuilder, value);
		if (nullable && value != null && !type.IsNullableType())
		{
			stringBuilder.Append(" (Nullable = true)");
		}
		else if (!nullable && hasValue && (value == null || type.IsNullableType()))
		{
			stringBuilder.Append(" (Nullable = false)");
		}
		if (size != 0)
		{
			stringBuilder.Append(" (Size = ").Append(size.ToString(CultureInfo.InvariantCulture)).Append(')');
		}
		if (precision != 0)
		{
			stringBuilder.Append(" (Precision = ").Append(precision.ToString(CultureInfo.InvariantCulture)).Append(')');
		}
		if (scale != 0)
		{
			stringBuilder.Append(" (Scale = ").Append(scale.ToString(CultureInfo.InvariantCulture)).Append(')');
		}
		if (direction != ParameterDirection.Input)
		{
			stringBuilder.Append(" (Direction = ").Append(direction).Append(')');
		}
		if (ShouldShowDbType(hasValue, dbType, type))
		{
			stringBuilder.Append(" (DbType = ").Append(dbType).Append(')');
		}
		return stringBuilder.ToString();
	}

	private static void FormatParameterValue(StringBuilder builder, object parameterValue)
	{
		if (parameterValue == null || parameterValue == DBNull.Value)
		{
			builder.Append("NULL");
			return;
		}
		if (parameterValue.GetType() == typeof(DateTime))
		{
			builder.Append('\'').Append(((DateTime)parameterValue).ToString("s")).Append('\'');
			return;
		}
		if (parameterValue.GetType() == typeof(DateTimeOffset))
		{
			builder.Append('\'').Append(((DateTimeOffset)parameterValue).ToString("o")).Append('\'');
			return;
		}
		if (parameterValue.GetType() == typeof(byte[]))
		{
			byte[] array = (byte[])parameterValue;
			builder.Append("'0x");
			for (int i = 0; i < array.Length; i++)
			{
				if (i > 31)
				{
					builder.Append("...");
					break;
				}
				builder.Append(array[i].ToString("X2", CultureInfo.InvariantCulture));
			}
			builder.Append('\'');
			return;
		}
		PropertyInfo runtimeProperty = parameterValue.GetType().GetRuntimeProperty("Value");
		if (runtimeProperty != null && runtimeProperty.PropertyType != parameterValue.GetType())
		{
			PropertyInfo runtimeProperty2 = parameterValue.GetType().GetRuntimeProperty("IsNull");
			if (runtimeProperty2 != null && (bool)runtimeProperty2.GetValue(parameterValue))
			{
				builder.Append("''");
			}
			else
			{
				FormatParameterValue(builder, runtimeProperty.GetValue(parameterValue));
			}
		}
		else
		{
			builder.Append('\'').Append(Convert.ToString(parameterValue, CultureInfo.InvariantCulture)).Append('\'');
		}
	}

	private static bool ShouldShowDbType(bool hasValue, DbType dbType, Type clrType)
	{
		if (!hasValue || clrType == null || clrType == typeof(DBNull))
		{
			return dbType != DbType.String;
		}
		clrType = clrType.UnwrapNullableType().UnwrapEnumType();
		return dbType switch
		{
			DbType.Binary => clrType != typeof(byte[]), 
			DbType.Byte => clrType != typeof(byte), 
			DbType.Boolean => clrType != typeof(bool), 
			DbType.Decimal => clrType != typeof(decimal), 
			DbType.Double => clrType != typeof(double), 
			DbType.Guid => clrType != typeof(Guid), 
			DbType.Int16 => clrType != typeof(short), 
			DbType.Int32 => clrType != typeof(int), 
			DbType.Int64 => clrType != typeof(long), 
			DbType.Object => clrType != typeof(object), 
			DbType.SByte => clrType != typeof(sbyte), 
			DbType.Single => clrType != typeof(float), 
			DbType.String => clrType != typeof(string), 
			DbType.Time => clrType != typeof(TimeSpan), 
			DbType.UInt16 => clrType != typeof(ushort), 
			DbType.UInt32 => clrType != typeof(uint), 
			DbType.UInt64 => clrType != typeof(ulong), 
			DbType.DateTime2 => clrType != typeof(DateTime), 
			DbType.DateTimeOffset => clrType != typeof(DateTimeOffset), 
			_ => true, 
		};
	}
}

using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Microsoft.EntityFrameworkCore.Storage;

public abstract class RelationalGeometryTypeMapping<TGeometry, TProvider> : RelationalTypeMapping
{
	protected virtual ValueConverter<TGeometry, TProvider> SpatialConverter { get; }

	protected abstract Type WKTReaderType { get; }

	protected RelationalGeometryTypeMapping([JetBrains.Annotations.NotNull] ValueConverter<TGeometry, TProvider> converter, [JetBrains.Annotations.NotNull] string storeType)
		: base(CreateRelationalTypeMappingParameters(storeType))
	{
		SpatialConverter = converter;
	}

	protected RelationalGeometryTypeMapping(RelationalTypeMappingParameters parameters, ValueConverter<TGeometry, TProvider> converter)
		: base(parameters)
	{
		SpatialConverter = converter;
	}

	private static RelationalTypeMappingParameters CreateRelationalTypeMappingParameters(string storeType)
	{
		GeometryValueComparer<TGeometry> geometryValueComparer = new GeometryValueComparer<TGeometry>();
		return new RelationalTypeMappingParameters(new CoreTypeMappingParameters(typeof(TGeometry), null, geometryValueComparer, geometryValueComparer), storeType);
	}

	public override DbParameter CreateParameter(DbCommand command, string name, object value, bool? nullable = null)
	{
		DbParameter dbParameter = command.CreateParameter();
		dbParameter.Direction = ParameterDirection.Input;
		dbParameter.ParameterName = name;
		if (Converter != null)
		{
			value = Converter.ConvertToProvider(value);
		}
		dbParameter.Value = ((value == null) ? DBNull.Value : SpatialConverter.ConvertToProvider(value));
		if (nullable.HasValue)
		{
			dbParameter.IsNullable = nullable.Value;
		}
		ConfigureParameter(dbParameter);
		return dbParameter;
	}

	public override Expression CustomizeDataReaderExpression(Expression expression)
	{
		if (expression.Type != SpatialConverter.ProviderClrType)
		{
			expression = Expression.Convert(expression, SpatialConverter.ProviderClrType);
		}
		return ReplacingExpressionVisitor.Replace(SpatialConverter.ConvertFromProviderExpression.Parameters.Single(), expression, SpatialConverter.ConvertFromProviderExpression.Body);
	}

	public override Expression GenerateCodeLiteral(object value)
	{
		return Expression.Convert(Expression.Call(Expression.New(WKTReaderType), WKTReaderType.GetMethod("Read", new Type[1] { typeof(string) }), Expression.Constant(CreateWktWithSrid(value), typeof(string))), value.GetType());
	}

	private string CreateWktWithSrid(object value)
	{
		int srid = GetSrid(value);
		string text = AsText(value);
		if (srid != -1)
		{
			text = $"SRID={srid};" + text;
		}
		return text;
	}

	protected abstract string AsText([JetBrains.Annotations.NotNull] object value);

	protected abstract int GetSrid([JetBrains.Annotations.NotNull] object value);
}

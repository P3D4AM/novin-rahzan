using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public class TypedRelationalValueBufferFactoryFactory : IRelationalValueBufferFactoryFactory
{
	private readonly struct CacheKey
	{
		public IReadOnlyList<TypeMaterializationInfo> TypeMaterializationInfo { get; }

		public CacheKey(IReadOnlyList<TypeMaterializationInfo> materializationInfo)
		{
			TypeMaterializationInfo = materializationInfo;
		}

		public override bool Equals(object obj)
		{
			if (obj is CacheKey other)
			{
				return Equals(other);
			}
			return false;
		}

		private bool Equals(CacheKey other)
		{
			return TypeMaterializationInfo.SequenceEqual(other.TypeMaterializationInfo);
		}

		public override int GetHashCode()
		{
			HashCode hashCode = default(HashCode);
			for (int i = 0; i < TypeMaterializationInfo.Count; i++)
			{
				hashCode.Add(TypeMaterializationInfo[i]);
			}
			return hashCode.ToHashCode();
		}
	}

	public static readonly ParameterExpression DataReaderParameter = Expression.Parameter(typeof(DbDataReader), "dataReader");

	private static readonly MethodInfo _getFieldValueMethod = typeof(DbDataReader).GetRuntimeMethod("GetFieldValue", new Type[1] { typeof(int) });

	private static readonly MethodInfo _isDbNullMethod = typeof(DbDataReader).GetRuntimeMethod("IsDBNull", new Type[1] { typeof(int) });

	private static readonly MethodInfo _throwReadValueExceptionMethod = typeof(TypedRelationalValueBufferFactoryFactory).GetTypeInfo().GetDeclaredMethod("ThrowReadValueException");

	private readonly ConcurrentDictionary<CacheKey, TypedRelationalValueBufferFactory> _cache = new ConcurrentDictionary<CacheKey, TypedRelationalValueBufferFactory>();

	protected virtual RelationalValueBufferFactoryDependencies Dependencies { get; }

	public TypedRelationalValueBufferFactoryFactory([JetBrains.Annotations.NotNull] RelationalValueBufferFactoryDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Dependencies = dependencies;
	}

	public virtual IRelationalValueBufferFactory Create(IReadOnlyList<TypeMaterializationInfo> types)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(types, "types");
		return _cache.GetOrAdd(new CacheKey(types), (CacheKey k) => new TypedRelationalValueBufferFactory(Dependencies, CreateArrayInitializer(k, Dependencies.CoreOptions.AreDetailedErrorsEnabled)));
	}

	public virtual IReadOnlyList<Expression> CreateAssignmentExpressions([JetBrains.Annotations.NotNull] IReadOnlyList<TypeMaterializationInfo> types)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(types, "types").Select((TypeMaterializationInfo mi, int i) => CreateGetValueExpression(DataReaderParameter, i, mi, Dependencies.CoreOptions.AreDetailedErrorsEnabled, box: false)).ToArray();
	}

	private static Func<DbDataReader, object[]> CreateArrayInitializer(CacheKey cacheKey, bool detailedErrorsEnabled)
	{
		return Expression.Lambda<Func<DbDataReader, object[]>>(Expression.NewArrayInit(typeof(object), cacheKey.TypeMaterializationInfo.Select((TypeMaterializationInfo mi, int i) => CreateGetValueExpression(DataReaderParameter, i, mi, detailedErrorsEnabled))), new ParameterExpression[1] { DataReaderParameter }).Compile();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static TValue ThrowReadValueException<TValue>(Exception exception, object value, IPropertyBase property = null)
	{
		Type typeFromHandle = typeof(TValue);
		Type actualType = value?.GetType();
		string message;
		if (property != null)
		{
			string entityType = property.DeclaringType.DisplayName();
			string name = property.Name;
			message = ((exception is NullReferenceException || object.Equals(value, DBNull.Value)) ? CoreStrings.ErrorMaterializingPropertyNullReference(entityType, name, typeFromHandle) : ((exception is InvalidCastException) ? CoreStrings.ErrorMaterializingPropertyInvalidCast(entityType, name, typeFromHandle, actualType) : CoreStrings.ErrorMaterializingProperty(entityType, name)));
		}
		else
		{
			message = ((exception is NullReferenceException) ? CoreStrings.ErrorMaterializingValueNullReference(typeFromHandle) : ((exception is InvalidCastException) ? CoreStrings.ErrorMaterializingValueInvalidCast(typeFromHandle, actualType) : CoreStrings.ErrorMaterializingValue));
		}
		throw new InvalidOperationException(message, exception);
	}

	private static Expression CreateGetValueExpression(Expression dataReaderExpression, int index, TypeMaterializationInfo materializationInfo, bool detailedErrorsEnabled, bool box = true)
	{
		MethodInfo dataReaderMethod = materializationInfo.Mapping.GetDataReaderMethod();
		index = ((materializationInfo.Index == -1) ? index : materializationInfo.Index);
		ConstantExpression constantExpression = Expression.Constant(index);
		Expression expression = Expression.Call((dataReaderMethod.DeclaringType != typeof(DbDataReader)) ? Expression.Convert(dataReaderExpression, dataReaderMethod.DeclaringType) : dataReaderExpression, dataReaderMethod, constantExpression);
		expression = materializationInfo.Mapping.CustomizeDataReaderExpression(expression);
		ValueConverter converter = materializationInfo.Mapping.Converter;
		if (converter != null)
		{
			if (expression.Type != converter.ProviderClrType)
			{
				expression = Expression.Convert(expression, converter.ProviderClrType);
			}
			expression = ReplacingExpressionVisitor.Replace(converter.ConvertFromProviderExpression.Parameters.Single(), expression, converter.ConvertFromProviderExpression.Body);
		}
		if (expression.Type != materializationInfo.ModelClrType)
		{
			expression = Expression.Convert(expression, materializationInfo.ModelClrType);
		}
		ParameterExpression parameterExpression = Expression.Parameter(typeof(Exception), "e");
		IProperty property = materializationInfo.Property;
		if (detailedErrorsEnabled)
		{
			CatchBlock catchBlock = Expression.Catch(parameterExpression, Expression.Call(_throwReadValueExceptionMethod.MakeGenericMethod(expression.Type), parameterExpression, Expression.Call(dataReaderExpression, _getFieldValueMethod.MakeGenericMethod(typeof(object)), constantExpression), Expression.Constant(property, typeof(IPropertyBase))));
			expression = Expression.TryCatch(expression, catchBlock);
		}
		if (box && expression.Type.GetTypeInfo().IsValueType)
		{
			expression = Expression.Convert(expression, typeof(object));
		}
		if (property == null || property.IsNullable || property.DeclaringEntityType.BaseType != null || materializationInfo.IsFromLeftOuterJoin != false)
		{
			expression = Expression.Condition(Expression.Call(dataReaderExpression, _isDbNullMethod, constantExpression), Expression.Default(expression.Type), expression);
		}
		return expression;
	}
}

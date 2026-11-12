using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Linq.Expressions;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;

public class SqlServerUdtTypeMapping : RelationalTypeMapping
{
	private static Action<DbParameter, string> _udtTypeNameSetter;

	public virtual string UdtTypeName { get; }

	public virtual Func<object, Expression> LiteralGenerator { get; }

	public SqlServerUdtTypeMapping([JetBrains.Annotations.NotNull] Type clrType, [JetBrains.Annotations.NotNull] string storeType, [JetBrains.Annotations.NotNull] Func<object, Expression> literalGenerator, StoreTypePostfix storeTypePostfix = StoreTypePostfix.None, [JetBrains.Annotations.CanBeNull] string udtTypeName = null, [JetBrains.Annotations.CanBeNull] ValueConverter converter = null, [JetBrains.Annotations.CanBeNull] ValueComparer comparer = null, [JetBrains.Annotations.CanBeNull] ValueComparer keyComparer = null, DbType? dbType = null, bool unicode = false, int? size = null, bool fixedLength = false, int? precision = null, int? scale = null)
		: base(new RelationalTypeMappingParameters(new CoreTypeMappingParameters(clrType, converter, comparer, keyComparer), storeType, storeTypePostfix, dbType, unicode, size, fixedLength, precision, scale))
	{
		LiteralGenerator = literalGenerator;
		UdtTypeName = udtTypeName ?? storeType;
	}

	protected SqlServerUdtTypeMapping(RelationalTypeMappingParameters parameters, [JetBrains.Annotations.NotNull] Func<object, Expression> literalGenerator, [JetBrains.Annotations.CanBeNull] string udtTypeName)
		: base(parameters)
	{
		LiteralGenerator = literalGenerator;
		UdtTypeName = udtTypeName ?? parameters.StoreType;
	}

	protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
	{
		return new SqlServerUdtTypeMapping(parameters, LiteralGenerator, UdtTypeName);
	}

	protected override void ConfigureParameter(DbParameter parameter)
	{
		SetUdtTypeName(parameter);
	}

	private void SetUdtTypeName(DbParameter parameter)
	{
		LazyInitializer.EnsureInitialized(ref _udtTypeNameSetter, () => CreateUdtTypeNameAccessor(parameter.GetType()));
		if (parameter.Value != null && parameter.Value != DBNull.Value)
		{
			_udtTypeNameSetter(parameter, UdtTypeName);
		}
	}

	public override Expression GenerateCodeLiteral(object value)
	{
		return LiteralGenerator(value);
	}

	private static Action<DbParameter, string> CreateUdtTypeNameAccessor(Type paramType)
	{
		ParameterExpression parameterExpression = Expression.Parameter(typeof(DbParameter), "parameter");
		ParameterExpression parameterExpression2 = Expression.Parameter(typeof(string), "value");
		return Expression.Lambda<Action<DbParameter, string>>(Expression.Call(Expression.Convert(parameterExpression, paramType), paramType.GetProperty("UdtTypeName").SetMethod, parameterExpression2), new ParameterExpression[2] { parameterExpression, parameterExpression2 }).Compile();
	}

	public static SqlServerUdtTypeMapping CreateSqlHierarchyIdMapping(Type udtType)
	{
		return new SqlServerUdtTypeMapping(udtType, "hierarchyid", (object v) => Expression.Call(v.GetType().GetMethod("Parse"), Expression.New(typeof(SqlString).GetConstructor(new Type[1] { typeof(string) }), Expression.Constant(v.ToString(), typeof(string)))));
	}

	public static SqlServerUdtTypeMapping CreateSqlSpatialMapping(Type udtType, string storeName)
	{
		return new SqlServerUdtTypeMapping(udtType, storeName, delegate(object v)
		{
			Type type = v.GetType();
			object[] parameters = new object[0];
			string value = ((SqlChars)type.GetMethod("AsTextZM").Invoke(v, parameters)).ToSqlString().ToString();
			return Expression.Call(arg1: Expression.Constant(((SqlInt32)type.GetMethod("get_STSrid").Invoke(v, parameters)).Value, typeof(int)), method: type.GetMethod("STGeomFromText"), arg0: Expression.New(typeof(SqlChars).GetConstructor(new Type[1] { typeof(SqlString) }), Expression.New(typeof(SqlString).GetConstructor(new Type[1] { typeof(string) }), Expression.Constant(value, typeof(string)))));
		});
	}
}

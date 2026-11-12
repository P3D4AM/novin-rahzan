using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Query;

public class SqlExpressionFactory : ISqlExpressionFactory
{
	private readonly IRelationalTypeMappingSource _typeMappingSource;

	private readonly RelationalTypeMapping _boolTypeMapping;

	public SqlExpressionFactory(SqlExpressionFactoryDependencies dependencies)
	{
		_typeMappingSource = dependencies.TypeMappingSource;
		_boolTypeMapping = _typeMappingSource.FindMapping(typeof(bool));
	}

	public virtual SqlExpression ApplyDefaultTypeMapping(SqlExpression sqlExpression)
	{
		if (sqlExpression == null || sqlExpression.TypeMapping != null)
		{
			return sqlExpression;
		}
		if (sqlExpression is SqlUnaryExpression { OperatorType: ExpressionType.Convert } sqlUnaryExpression && sqlUnaryExpression.Type == typeof(object))
		{
			return sqlUnaryExpression.Operand;
		}
		return ApplyTypeMapping(sqlExpression, _typeMappingSource.FindMapping(sqlExpression.Type));
	}

	public virtual SqlExpression ApplyTypeMapping(SqlExpression sqlExpression, RelationalTypeMapping typeMapping)
	{
		if (sqlExpression == null || sqlExpression.TypeMapping != null)
		{
			return sqlExpression;
		}
		if (!(sqlExpression is CaseExpression caseExpression))
		{
			if (!(sqlExpression is LikeExpression likeExpression))
			{
				if (!(sqlExpression is SqlBinaryExpression sqlBinaryExpression))
				{
					if (!(sqlExpression is SqlUnaryExpression sqlUnaryExpression))
					{
						if (!(sqlExpression is SqlConstantExpression sqlConstantExpression))
						{
							if (!(sqlExpression is SqlFragmentExpression result))
							{
								if (!(sqlExpression is SqlFunctionExpression sqlFunctionExpression))
								{
									if (sqlExpression is SqlParameterExpression sqlParameterExpression)
									{
										return sqlParameterExpression.ApplyTypeMapping(typeMapping);
									}
									return sqlExpression;
								}
								return sqlFunctionExpression.ApplyTypeMapping(typeMapping);
							}
							return result;
						}
						return sqlConstantExpression.ApplyTypeMapping(typeMapping);
					}
					return ApplyTypeMappingOnSqlUnary(sqlUnaryExpression, typeMapping);
				}
				return ApplyTypeMappingOnSqlBinary(sqlBinaryExpression, typeMapping);
			}
			return ApplyTypeMappingOnLike(likeExpression);
		}
		return ApplyTypeMappingOnCase(caseExpression, typeMapping);
	}

	private SqlExpression ApplyTypeMappingOnLike(LikeExpression likeExpression)
	{
		RelationalTypeMapping typeMapping = ((likeExpression.EscapeChar == null) ? ExpressionExtensions.InferTypeMapping(likeExpression.Match, likeExpression.Pattern) : ExpressionExtensions.InferTypeMapping(likeExpression.Match, likeExpression.Pattern, likeExpression.EscapeChar)) ?? _typeMappingSource.FindMapping(likeExpression.Match.Type);
		return new LikeExpression(ApplyTypeMapping(likeExpression.Match, typeMapping), ApplyTypeMapping(likeExpression.Pattern, typeMapping), ApplyTypeMapping(likeExpression.EscapeChar, typeMapping), _boolTypeMapping);
	}

	private SqlExpression ApplyTypeMappingOnCase(CaseExpression caseExpression, RelationalTypeMapping typeMapping)
	{
		List<CaseWhenClause> list = new List<CaseWhenClause>();
		foreach (CaseWhenClause whenClause in caseExpression.WhenClauses)
		{
			list.Add(new CaseWhenClause(whenClause.Test, ApplyTypeMapping(whenClause.Result, typeMapping)));
		}
		SqlExpression elseResult = ApplyTypeMapping(caseExpression.ElseResult, typeMapping);
		return caseExpression.Update(caseExpression.Operand, list, elseResult);
	}

	private SqlExpression ApplyTypeMappingOnSqlUnary(SqlUnaryExpression sqlUnaryExpression, RelationalTypeMapping typeMapping)
	{
		RelationalTypeMapping typeMapping2;
		SqlExpression operand;
		switch (sqlUnaryExpression.OperatorType)
		{
		case ExpressionType.Equal:
		case ExpressionType.Not:
		case ExpressionType.NotEqual:
			typeMapping2 = _boolTypeMapping;
			operand = ApplyDefaultTypeMapping(sqlUnaryExpression.Operand);
			break;
		case ExpressionType.Convert:
			typeMapping2 = typeMapping;
			operand = ApplyDefaultTypeMapping(sqlUnaryExpression.Operand);
			break;
		case ExpressionType.Negate:
			typeMapping2 = typeMapping;
			operand = ApplyTypeMapping(sqlUnaryExpression.Operand, typeMapping);
			break;
		default:
			throw new InvalidOperationException($"The unary expression operator type {sqlUnaryExpression.OperatorType} is not supported.");
		}
		return new SqlUnaryExpression(sqlUnaryExpression.OperatorType, operand, sqlUnaryExpression.Type, typeMapping2);
	}

	private SqlExpression ApplyTypeMappingOnSqlBinary(SqlBinaryExpression sqlBinaryExpression, RelationalTypeMapping typeMapping)
	{
		SqlExpression left = sqlBinaryExpression.Left;
		SqlExpression right = sqlBinaryExpression.Right;
		RelationalTypeMapping relationalTypeMapping;
		Type type;
		RelationalTypeMapping typeMapping2;
		switch (sqlBinaryExpression.OperatorType)
		{
		case ExpressionType.Equal:
		case ExpressionType.GreaterThan:
		case ExpressionType.GreaterThanOrEqual:
		case ExpressionType.LessThan:
		case ExpressionType.LessThanOrEqual:
		case ExpressionType.NotEqual:
			relationalTypeMapping = ExpressionExtensions.InferTypeMapping(left, right) ?? _typeMappingSource.FindMapping(left.Type);
			type = typeof(bool);
			typeMapping2 = _boolTypeMapping;
			break;
		case ExpressionType.AndAlso:
		case ExpressionType.OrElse:
			relationalTypeMapping = _boolTypeMapping;
			type = typeof(bool);
			typeMapping2 = _boolTypeMapping;
			break;
		case ExpressionType.Add:
		case ExpressionType.And:
		case ExpressionType.Coalesce:
		case ExpressionType.Divide:
		case ExpressionType.Modulo:
		case ExpressionType.Multiply:
		case ExpressionType.Or:
		case ExpressionType.Subtract:
			relationalTypeMapping = typeMapping ?? ExpressionExtensions.InferTypeMapping(left, right);
			type = left.Type;
			typeMapping2 = relationalTypeMapping;
			break;
		default:
			throw new InvalidOperationException("Incorrect operatorType for SqlBinaryExpression");
		}
		return new SqlBinaryExpression(sqlBinaryExpression.OperatorType, ApplyTypeMapping(left, relationalTypeMapping), ApplyTypeMapping(right, relationalTypeMapping), type, typeMapping2);
	}

	public virtual RelationalTypeMapping GetTypeMappingForValue(object value)
	{
		return _typeMappingSource.GetMappingForValue(value);
	}

	public virtual RelationalTypeMapping FindMapping(Type type)
	{
		return _typeMappingSource.FindMapping(type);
	}

	public virtual SqlBinaryExpression MakeBinary(ExpressionType operatorType, SqlExpression left, SqlExpression right, RelationalTypeMapping typeMapping)
	{
		Type type = left.Type;
		switch (operatorType)
		{
		case ExpressionType.AndAlso:
		case ExpressionType.Equal:
		case ExpressionType.GreaterThan:
		case ExpressionType.GreaterThanOrEqual:
		case ExpressionType.LessThan:
		case ExpressionType.LessThanOrEqual:
		case ExpressionType.NotEqual:
		case ExpressionType.OrElse:
			type = typeof(bool);
			break;
		}
		return (SqlBinaryExpression)ApplyTypeMapping(new SqlBinaryExpression(operatorType, left, right, type, null), typeMapping);
	}

	public virtual SqlBinaryExpression Equal(SqlExpression left, SqlExpression right)
	{
		return MakeBinary(ExpressionType.Equal, left, right, null);
	}

	public virtual SqlBinaryExpression NotEqual(SqlExpression left, SqlExpression right)
	{
		return MakeBinary(ExpressionType.NotEqual, left, right, null);
	}

	public virtual SqlBinaryExpression GreaterThan(SqlExpression left, SqlExpression right)
	{
		return MakeBinary(ExpressionType.GreaterThan, left, right, null);
	}

	public virtual SqlBinaryExpression GreaterThanOrEqual(SqlExpression left, SqlExpression right)
	{
		return MakeBinary(ExpressionType.GreaterThanOrEqual, left, right, null);
	}

	public virtual SqlBinaryExpression LessThan(SqlExpression left, SqlExpression right)
	{
		return MakeBinary(ExpressionType.LessThan, left, right, null);
	}

	public virtual SqlBinaryExpression LessThanOrEqual(SqlExpression left, SqlExpression right)
	{
		return MakeBinary(ExpressionType.LessThanOrEqual, left, right, null);
	}

	public virtual SqlBinaryExpression AndAlso(SqlExpression left, SqlExpression right)
	{
		return MakeBinary(ExpressionType.AndAlso, left, right, null);
	}

	public virtual SqlBinaryExpression OrElse(SqlExpression left, SqlExpression right)
	{
		return MakeBinary(ExpressionType.OrElse, left, right, null);
	}

	public virtual SqlBinaryExpression Add(SqlExpression left, SqlExpression right, RelationalTypeMapping typeMapping = null)
	{
		return MakeBinary(ExpressionType.Add, left, right, typeMapping);
	}

	public virtual SqlBinaryExpression Subtract(SqlExpression left, SqlExpression right, RelationalTypeMapping typeMapping = null)
	{
		return MakeBinary(ExpressionType.Subtract, left, right, typeMapping);
	}

	public virtual SqlBinaryExpression Multiply(SqlExpression left, SqlExpression right, RelationalTypeMapping typeMapping = null)
	{
		return MakeBinary(ExpressionType.Multiply, left, right, typeMapping);
	}

	public virtual SqlBinaryExpression Divide(SqlExpression left, SqlExpression right, RelationalTypeMapping typeMapping = null)
	{
		return MakeBinary(ExpressionType.Divide, left, right, typeMapping);
	}

	public virtual SqlBinaryExpression Modulo(SqlExpression left, SqlExpression right, RelationalTypeMapping typeMapping = null)
	{
		return MakeBinary(ExpressionType.Modulo, left, right, typeMapping);
	}

	public virtual SqlBinaryExpression And(SqlExpression left, SqlExpression right, RelationalTypeMapping typeMapping = null)
	{
		return MakeBinary(ExpressionType.And, left, right, typeMapping);
	}

	public virtual SqlBinaryExpression Or(SqlExpression left, SqlExpression right, RelationalTypeMapping typeMapping = null)
	{
		return MakeBinary(ExpressionType.Or, left, right, typeMapping);
	}

	public virtual SqlBinaryExpression Coalesce(SqlExpression left, SqlExpression right, RelationalTypeMapping typeMapping = null)
	{
		return MakeBinary(ExpressionType.Coalesce, left, right, typeMapping);
	}

	public virtual SqlUnaryExpression MakeUnary(ExpressionType operatorType, SqlExpression operand, Type type, RelationalTypeMapping typeMapping = null)
	{
		return (SqlUnaryExpression)ApplyTypeMapping(new SqlUnaryExpression(operatorType, operand, type, null), typeMapping);
	}

	public virtual SqlUnaryExpression IsNull(SqlExpression operand)
	{
		return MakeUnary(ExpressionType.Equal, operand, typeof(bool));
	}

	public virtual SqlUnaryExpression IsNotNull(SqlExpression operand)
	{
		return MakeUnary(ExpressionType.NotEqual, operand, typeof(bool));
	}

	public virtual SqlUnaryExpression Convert(SqlExpression operand, Type type, RelationalTypeMapping typeMapping = null)
	{
		return MakeUnary(ExpressionType.Convert, operand, type, typeMapping);
	}

	public virtual SqlUnaryExpression Not(SqlExpression operand)
	{
		return MakeUnary(ExpressionType.Not, operand, typeof(bool));
	}

	public virtual SqlUnaryExpression Negate(SqlExpression operand)
	{
		return MakeUnary(ExpressionType.Negate, operand, operand.Type, operand.TypeMapping);
	}

	public virtual CaseExpression Case(SqlExpression operand, SqlExpression elseResult, params CaseWhenClause[] whenClauses)
	{
		RelationalTypeMapping typeMapping = operand.TypeMapping ?? whenClauses.Select((CaseWhenClause wc) => wc.Test.TypeMapping).FirstOrDefault((RelationalTypeMapping t) => t != null) ?? _typeMappingSource.FindMapping(operand.Type);
		RelationalTypeMapping typeMapping2 = elseResult?.TypeMapping ?? whenClauses.Select((CaseWhenClause wc) => wc.Result.TypeMapping).FirstOrDefault((RelationalTypeMapping t) => t != null);
		operand = ApplyTypeMapping(operand, typeMapping);
		List<CaseWhenClause> list = new List<CaseWhenClause>();
		foreach (CaseWhenClause caseWhenClause in whenClauses)
		{
			list.Add(new CaseWhenClause(ApplyTypeMapping(caseWhenClause.Test, typeMapping), ApplyTypeMapping(caseWhenClause.Result, typeMapping2)));
		}
		elseResult = ApplyTypeMapping(elseResult, typeMapping2);
		return new CaseExpression(operand, list, elseResult);
	}

	public virtual CaseExpression Case(SqlExpression operand, params CaseWhenClause[] whenClauses)
	{
		return Case(operand, null, whenClauses);
	}

	public virtual CaseExpression Case(IReadOnlyList<CaseWhenClause> whenClauses, SqlExpression elseResult)
	{
		RelationalTypeMapping typeMapping = elseResult?.TypeMapping ?? whenClauses.Select((CaseWhenClause wc) => wc.Result.TypeMapping).FirstOrDefault((RelationalTypeMapping t) => t != null);
		List<CaseWhenClause> list = new List<CaseWhenClause>();
		foreach (CaseWhenClause whenClause in whenClauses)
		{
			list.Add(new CaseWhenClause(ApplyTypeMapping(whenClause.Test, _boolTypeMapping), ApplyTypeMapping(whenClause.Result, typeMapping)));
		}
		elseResult = ApplyTypeMapping(elseResult, typeMapping);
		return new CaseExpression(list, elseResult);
	}

	public virtual SqlFunctionExpression Function(string name, IEnumerable<SqlExpression> arguments, Type returnType, RelationalTypeMapping typeMapping = null)
	{
		List<SqlExpression> list = new List<SqlExpression>();
		foreach (SqlExpression argument in arguments)
		{
			list.Add(ApplyDefaultTypeMapping(argument));
		}
		return SqlFunctionExpression.Create(name, list, returnType, typeMapping);
	}

	public virtual SqlFunctionExpression Function(string schema, string name, IEnumerable<SqlExpression> arguments, Type returnType, RelationalTypeMapping typeMapping = null)
	{
		List<SqlExpression> list = new List<SqlExpression>();
		foreach (SqlExpression argument in arguments)
		{
			list.Add(ApplyDefaultTypeMapping(argument));
		}
		return SqlFunctionExpression.Create(schema, name, list, returnType, typeMapping);
	}

	public virtual SqlFunctionExpression Function(SqlExpression instance, string name, IEnumerable<SqlExpression> arguments, Type returnType, RelationalTypeMapping typeMapping = null)
	{
		instance = ApplyDefaultTypeMapping(instance);
		List<SqlExpression> list = new List<SqlExpression>();
		foreach (SqlExpression argument in arguments)
		{
			list.Add(ApplyDefaultTypeMapping(argument));
		}
		return SqlFunctionExpression.Create(instance, name, list, returnType, typeMapping);
	}

	public virtual SqlFunctionExpression Function(string name, Type returnType, RelationalTypeMapping typeMapping = null)
	{
		return SqlFunctionExpression.CreateNiladic(name, returnType, typeMapping);
	}

	public virtual SqlFunctionExpression Function(string schema, string name, Type returnType, RelationalTypeMapping typeMapping = null)
	{
		return SqlFunctionExpression.CreateNiladic(schema, name, returnType, typeMapping);
	}

	public virtual SqlFunctionExpression Function(SqlExpression instance, string name, Type returnType, RelationalTypeMapping typeMapping = null)
	{
		return SqlFunctionExpression.CreateNiladic(ApplyDefaultTypeMapping(instance), name, returnType, typeMapping);
	}

	public virtual ExistsExpression Exists(SelectExpression subquery, bool negated)
	{
		return new ExistsExpression(subquery, negated, _boolTypeMapping);
	}

	public virtual InExpression In(SqlExpression item, SqlExpression values, bool negated)
	{
		RelationalTypeMapping typeMapping = item.TypeMapping ?? _typeMappingSource.FindMapping(item.Type);
		item = ApplyTypeMapping(item, typeMapping);
		values = ApplyTypeMapping(values, typeMapping);
		return new InExpression(item, negated, values, _boolTypeMapping);
	}

	public virtual InExpression In(SqlExpression item, SelectExpression subquery, bool negated)
	{
		SqlExpression expression = subquery.Projection.Single().Expression;
		RelationalTypeMapping typeMapping = expression.TypeMapping;
		if (typeMapping == null)
		{
			throw new InvalidOperationException($"The subquery '{subquery.Print()}' references type '{expression.Type}' for which no type mapping could be found.");
		}
		item = ApplyTypeMapping(item, typeMapping);
		return new InExpression(item, negated, subquery, _boolTypeMapping);
	}

	public virtual LikeExpression Like(SqlExpression match, SqlExpression pattern, SqlExpression escapeChar = null)
	{
		return (LikeExpression)ApplyDefaultTypeMapping(new LikeExpression(match, pattern, escapeChar, null));
	}

	public virtual SqlFragmentExpression Fragment(string sql)
	{
		return new SqlFragmentExpression(sql);
	}

	public virtual SqlConstantExpression Constant(object value, RelationalTypeMapping typeMapping = null)
	{
		return new SqlConstantExpression(Expression.Constant(value), typeMapping);
	}

	public virtual SelectExpression Select(SqlExpression projection)
	{
		SelectExpression selectExpression = new SelectExpression(null, new List<ProjectionExpression>(), new List<TableExpressionBase>(), new List<SqlExpression>(), new List<OrderingExpression>());
		if (projection != null)
		{
			selectExpression.ReplaceProjectionMapping(new Dictionary<ProjectionMember, Expression> { 
			{
				new ProjectionMember(),
				projection
			} });
		}
		return selectExpression;
	}

	public virtual SelectExpression Select(IEntityType entityType)
	{
		SelectExpression selectExpression = new SelectExpression(entityType);
		AddConditions(selectExpression, entityType);
		return selectExpression;
	}

	public virtual SelectExpression Select(IEntityType entityType, string sql, Expression sqlArguments)
	{
		SelectExpression selectExpression = new SelectExpression(entityType, sql, sqlArguments);
		AddConditions(selectExpression, entityType);
		return selectExpression;
	}

	private void AddConditions(SelectExpression selectExpression, IEntityType entityType, ICollection<IEntityType> sharingTypes = null, bool skipJoins = false)
	{
		if (entityType.FindPrimaryKey() == null)
		{
			AddDiscriminatorCondition(selectExpression, entityType);
			return;
		}
		if (sharingTypes == null)
		{
			sharingTypes = new HashSet<IEntityType>(from et in entityType.Model.GetEntityTypes()
				where et.FindPrimaryKey() != null && et.GetTableName() == entityType.GetTableName() && et.GetSchema() == entityType.GetSchema()
				select et);
		}
		if (sharingTypes.Count <= 0)
		{
			return;
		}
		bool flag = AddDiscriminatorCondition(selectExpression, entityType);
		List<IForeignKey> list = (from fk in entityType.GetRootType().FindForeignKeys(entityType.FindPrimaryKey().Properties)
			where fk.PrincipalKey.IsPrimaryKey() && fk.PrincipalEntityType != entityType && sharingTypes.Contains(fk.PrincipalEntityType)
			select fk).ToList();
		if (list.Count <= 0)
		{
			return;
		}
		if (!flag)
		{
			AddOptionalDependentConditions(selectExpression, entityType, sharingTypes);
		}
		if (skipJoins)
		{
			return;
		}
		if (AppContext.TryGetSwitch("Microsoft.EntityFrameworkCore.Issue18299", out var isEnabled) && isEnabled)
		{
			AddInnerJoin(selectExpression, list[0], sharingTypes, skipInnerJoins: false);
			{
				foreach (IForeignKey item in list.Skip(1))
				{
					SelectExpression selectExpression2 = new SelectExpression(entityType);
					AddInnerJoin(selectExpression2, item, sharingTypes, skipInnerJoins: false);
					selectExpression.ApplyUnion(selectExpression2, distinct: true);
				}
				return;
			}
		}
		bool flag2 = true;
		foreach (IForeignKey item2 in list)
		{
			if (entityType.FindOwnership() != item2 || item2.PrincipalEntityType.BaseType != null)
			{
				SelectExpression selectExpression3 = (flag2 ? selectExpression : new SelectExpression(entityType));
				AddInnerJoin(selectExpression3, item2, sharingTypes, skipInnerJoins: false);
				if (flag2)
				{
					flag2 = false;
				}
				else
				{
					selectExpression.ApplyUnion(selectExpression3, distinct: true);
				}
			}
		}
	}

	private void AddInnerJoin(SelectExpression selectExpression, IForeignKey foreignKey, ICollection<IEntityType> sharingTypes, bool skipInnerJoins)
	{
		SelectExpression innerSelect;
		SqlExpression joinPredicate = GenerateJoinPredicate(selectExpression, foreignKey, sharingTypes, skipInnerJoins, out innerSelect);
		selectExpression.AddInnerJoin(innerSelect, joinPredicate, null);
	}

	private SqlExpression GenerateJoinPredicate(SelectExpression selectExpression, IForeignKey foreignKey, ICollection<IEntityType> sharingTypes, bool skipInnerJoins, out SelectExpression innerSelect)
	{
		EntityProjectionExpression outerEntityProjection = GetMappedEntityProjectionExpression(selectExpression);
		bool flag = foreignKey.PrincipalEntityType.IsAssignableFrom(outerEntityProjection.EntityType);
		innerSelect = (flag ? new SelectExpression(foreignKey.DeclaringEntityType) : new SelectExpression(foreignKey.PrincipalEntityType));
		AddConditions(innerSelect, flag ? foreignKey.DeclaringEntityType : foreignKey.PrincipalEntityType, sharingTypes, skipInnerJoins);
		EntityProjectionExpression innerEntityProjection = GetMappedEntityProjectionExpression(innerSelect);
		IEnumerable<ColumnExpression> first = (flag ? foreignKey.PrincipalKey.Properties : foreignKey.Properties).Select((IProperty p) => outerEntityProjection.BindProperty(p));
		IEnumerable<ColumnExpression> second = (flag ? foreignKey.Properties : foreignKey.PrincipalKey.Properties).Select((IProperty p) => innerEntityProjection.BindProperty(p));
		return ((IEnumerable<SqlExpression>)first).Zip((IEnumerable<SqlExpression>)second, (Func<SqlExpression, SqlExpression, SqlExpression>)Equal).Aggregate(AndAlso);
	}

	private bool AddDiscriminatorCondition(SelectExpression selectExpression, IEntityType entityType)
	{
		List<IEntityType> list = entityType.GetConcreteDerivedTypesInclusive().ToList();
		SqlExpression expression;
		if (list.Count == 1)
		{
			IEntityType entityType2 = list[0];
			if (entityType2.BaseType == null)
			{
				return false;
			}
			ColumnExpression left = GetMappedEntityProjectionExpression(selectExpression).BindProperty(entityType2.GetDiscriminatorProperty());
			expression = Equal(left, Constant(entityType2.GetDiscriminatorValue()));
		}
		else
		{
			ColumnExpression item = GetMappedEntityProjectionExpression(selectExpression).BindProperty(list[0].GetDiscriminatorProperty());
			expression = In(item, Constant(list.Select((IEntityType et) => et.GetDiscriminatorValue()).ToList()), negated: false);
		}
		selectExpression.ApplyPredicate(expression);
		return true;
	}

	private void AddOptionalDependentConditions(SelectExpression selectExpression, IEntityType entityType, ICollection<IEntityType> sharingTypes)
	{
		SqlExpression sqlExpression = null;
		List<IProperty> list = (from p in entityType.GetProperties()
			where !p.IsNullable && !p.IsPrimaryKey()
			select p).ToList();
		if (list.Count > 0)
		{
			EntityProjectionExpression entityProjectionExpression2 = GetMappedEntityProjectionExpression(selectExpression);
			sqlExpression = IsNotNull(list[0], entityProjectionExpression2);
			if (list.Count > 1)
			{
				sqlExpression = list.Skip(1).Aggregate(sqlExpression, (SqlExpression current, IProperty property) => AndAlso(IsNotNull(property, entityProjectionExpression2), current));
			}
			selectExpression.ApplyPredicate(sqlExpression);
			return;
		}
		List<IProperty> list2 = (from p in entityType.GetProperties()
			where !p.IsPrimaryKey()
			select p).ToList();
		if (list2.Count <= 0)
		{
			return;
		}
		EntityProjectionExpression entityProjectionExpression = GetMappedEntityProjectionExpression(selectExpression);
		sqlExpression = IsNotNull(list2[0], entityProjectionExpression);
		if (list2.Count > 1)
		{
			sqlExpression = list2.Skip(1).Aggregate(sqlExpression, (SqlExpression current, IProperty property) => OrElse(IsNotNull(property, entityProjectionExpression), current));
		}
		selectExpression.ApplyPredicate(sqlExpression);
		foreach (IForeignKey referencingForeignKey in entityType.GetReferencingForeignKeys())
		{
			SelectExpression selectExpression2 = new SelectExpression(entityType);
			bool flag = sharingTypes.Contains(referencingForeignKey.DeclaringEntityType);
			AddInnerJoin(selectExpression2, referencingForeignKey, flag ? sharingTypes : null, flag);
			selectExpression.ApplyUnion(selectExpression2, distinct: true);
		}
	}

	private EntityProjectionExpression GetMappedEntityProjectionExpression(SelectExpression selectExpression)
	{
		return (EntityProjectionExpression)selectExpression.GetMappedProjection(new ProjectionMember());
	}

	private SqlExpression IsNotNull(IProperty property, EntityProjectionExpression entityProjection)
	{
		return IsNotNull(entityProjection.BindProperty(property));
	}
}

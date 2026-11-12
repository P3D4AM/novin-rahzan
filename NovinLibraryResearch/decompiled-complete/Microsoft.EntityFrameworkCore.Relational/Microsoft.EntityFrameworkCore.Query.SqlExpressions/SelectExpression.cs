using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public class SelectExpression : TableExpressionBase
{
	private enum SetOperationType
	{
		Except,
		Intersect,
		Union
	}

	private class ShaperRemappingExpressionVisitor : ExpressionVisitor
	{
		private readonly SelectExpression _queryExpression;

		private readonly SelectExpression _innerSelectExpression;

		private readonly int _offset;

		private readonly int[] _indexMap;

		public ShaperRemappingExpressionVisitor(SelectExpression queryExpression, SelectExpression innerSelectExpression, int offset)
		{
			_queryExpression = queryExpression;
			_innerSelectExpression = innerSelectExpression;
			_offset = offset;
		}

		public ShaperRemappingExpressionVisitor(SelectExpression queryExpression, SelectExpression innerSelectExpression, int[] indexMap)
		{
			_queryExpression = queryExpression;
			_innerSelectExpression = innerSelectExpression;
			_indexMap = indexMap;
		}

		protected override Expression VisitExtension(Expression extensionExpression)
		{
			if (extensionExpression is ProjectionBindingExpression projectionBindingExpression)
			{
				int num = (int)GetProjectionIndex(projectionBindingExpression);
				int[] indexMap = _indexMap;
				int index = ((indexMap != null) ? indexMap[num] : (num + _offset));
				return new ProjectionBindingExpression(_queryExpression, index, projectionBindingExpression.Type);
			}
			if (extensionExpression is EntityShaperExpression entityShaperExpression)
			{
				IDictionary<IProperty, int> obj = (IDictionary<IProperty, int>)GetProjectionIndex((ProjectionBindingExpression)entityShaperExpression.ValueBufferExpression);
				Dictionary<IProperty, int> dictionary = new Dictionary<IProperty, int>();
				foreach (KeyValuePair<IProperty, int> item in obj)
				{
					int value = item.Value;
					IProperty key = item.Key;
					int[] indexMap2 = _indexMap;
					dictionary[key] = ((indexMap2 != null) ? indexMap2[value] : (value + _offset));
				}
				return new EntityShaperExpression(entityShaperExpression.EntityType, new ProjectionBindingExpression(_queryExpression, dictionary), nullable: true);
			}
			return base.VisitExtension(extensionExpression);
		}

		private object GetProjectionIndex(ProjectionBindingExpression projectionBindingExpression)
		{
			if (projectionBindingExpression.ProjectionMember == null)
			{
				if (!projectionBindingExpression.Index.HasValue)
				{
					return projectionBindingExpression.IndexMap;
				}
				return projectionBindingExpression.Index;
			}
			return ((ConstantExpression)_innerSelectExpression.GetMappedProjection(projectionBindingExpression.ProjectionMember)).Value;
		}
	}

	private class SelectExpressionCorrelationFindingExpressionVisitor : ExpressionVisitor
	{
		private readonly SelectExpression _outerSelectExpression;

		private bool _containsOuterReference;

		private readonly bool _quirkMode19825;

		public SelectExpressionCorrelationFindingExpressionVisitor(SelectExpression outerSelectExpression)
		{
			_outerSelectExpression = outerSelectExpression;
			_quirkMode19825 = AppContext.TryGetSwitch("Microsoft.EntityFrameworkCore.Issue19825", out var isEnabled) && isEnabled;
		}

		public bool ContainsOuterReference(SelectExpression selectExpression)
		{
			_containsOuterReference = false;
			Visit(selectExpression);
			return _containsOuterReference;
		}

		public override Expression Visit(Expression expression)
		{
			if (_containsOuterReference)
			{
				return expression;
			}
			if (_quirkMode19825)
			{
				if (expression is ColumnExpression columnExpression && _outerSelectExpression.Tables.Contains(columnExpression.Table))
				{
					_containsOuterReference = true;
					return expression;
				}
			}
			else if (expression is ColumnExpression columnExpression2 && _outerSelectExpression.ContainsTableReference(columnExpression2.Table))
			{
				_containsOuterReference = true;
				return expression;
			}
			return base.Visit(expression);
		}
	}

	private enum JoinType
	{
		InnerJoin,
		LeftJoin,
		CrossJoin,
		CrossApply,
		OuterApply
	}

	private class SqlRemappingVisitor : ExpressionVisitor
	{
		private readonly SelectExpression _subquery;

		private readonly IDictionary<SqlExpression, ColumnExpression> _mappings;

		public SqlRemappingVisitor(IDictionary<SqlExpression, ColumnExpression> mappings, SelectExpression subquery)
		{
			_subquery = subquery;
			_mappings = mappings;
		}

		public SqlExpression Remap(SqlExpression sqlExpression)
		{
			return (SqlExpression)Visit(sqlExpression);
		}

		public SelectExpression Remap(SelectExpression sqlExpression)
		{
			return (SelectExpression)Visit(sqlExpression);
		}

		public override Expression Visit(Expression expression)
		{
			if (expression is SqlExpression sqlExpression)
			{
				SqlExpression key = sqlExpression;
				if (_mappings.TryGetValue(key, out var value))
				{
					return value;
				}
				if (expression is ColumnExpression columnExpression)
				{
					ColumnExpression columnExpression2 = columnExpression;
					if (_subquery.ContainsTableReference(columnExpression2.Table))
					{
						int index = _subquery.AddToProjection(columnExpression2);
						return new ColumnExpression(_subquery._projection[index], _subquery);
					}
				}
			}
			return base.Visit(expression);
		}
	}

	private readonly IDictionary<EntityProjectionExpression, IDictionary<IProperty, int>> _entityProjectionCache = new Dictionary<EntityProjectionExpression, IDictionary<IProperty, int>>();

	private readonly List<ProjectionExpression> _projection = new List<ProjectionExpression>();

	private readonly List<TableExpressionBase> _tables = new List<TableExpressionBase>();

	private readonly List<SqlExpression> _groupBy = new List<SqlExpression>();

	private readonly List<OrderingExpression> _orderings = new List<OrderingExpression>();

	private readonly List<SqlExpression> _identifier = new List<SqlExpression>();

	private readonly List<SqlExpression> _childIdentifiers = new List<SqlExpression>();

	private readonly List<SelectExpression> _pendingCollections = new List<SelectExpression>();

	private IDictionary<ProjectionMember, Expression> _projectionMapping = new Dictionary<ProjectionMember, Expression>();

	public IReadOnlyList<ProjectionExpression> Projection => _projection;

	public IReadOnlyList<TableExpressionBase> Tables => _tables;

	public IReadOnlyList<SqlExpression> GroupBy => _groupBy;

	public IReadOnlyList<OrderingExpression> Orderings => _orderings;

	public ISet<string> Tags { get; private set; } = new HashSet<string>();


	public SqlExpression Predicate { get; private set; }

	public SqlExpression Having { get; private set; }

	public SqlExpression Limit { get; private set; }

	public SqlExpression Offset { get; private set; }

	public bool IsDistinct { get; private set; }

	public void ApplyTags(ISet<string> tags)
	{
		Tags = tags;
	}

	internal SelectExpression(string alias, List<ProjectionExpression> projections, List<TableExpressionBase> tables, List<SqlExpression> groupBy, List<OrderingExpression> orderings)
		: base(alias)
	{
		_projection = projections;
		_tables = tables;
		_groupBy = groupBy;
		_orderings = orderings;
	}

	internal SelectExpression(IEntityType entityType)
		: this(entityType, new TableExpression(entityType.GetTableName(), entityType.GetSchema(), entityType.GetTableName().ToLower().Substring(0, 1)))
	{
	}

	internal SelectExpression(IEntityType entityType, string sql, Expression arguments)
		: this(entityType, new FromSqlExpression(sql, arguments, entityType.GetTableName().ToLower().Substring(0, 1)))
	{
	}

	private SelectExpression(IEntityType entityType, TableExpressionBase tableExpression)
		: base(null)
	{
		_tables.Add(tableExpression);
		EntityProjectionExpression entityProjectionExpression = new EntityProjectionExpression(entityType, tableExpression, nullable: false);
		_projectionMapping[new ProjectionMember()] = entityProjectionExpression;
		if (entityType.FindPrimaryKey() == null)
		{
			return;
		}
		foreach (IProperty property in entityType.FindPrimaryKey().Properties)
		{
			_identifier.Add(entityProjectionExpression.BindProperty(property));
		}
	}

	public bool IsNonComposedFromSql()
	{
		if (Limit == null && Offset == null && !IsDistinct && Predicate == null && GroupBy.Count == 0 && Having == null && Orderings.Count == 0 && Tables.Count == 1)
		{
			TableExpressionBase tableExpressionBase = Tables[0];
			FromSqlExpression fromSql = tableExpressionBase as FromSqlExpression;
			if (fromSql != null)
			{
				return Projection.All((ProjectionExpression pe) => pe.Expression is ColumnExpression columnExpression && string.Equals(fromSql.Alias, columnExpression.Table.Alias, StringComparison.OrdinalIgnoreCase));
			}
		}
		return false;
	}

	public void ApplyProjection()
	{
		if (Projection.Any())
		{
			return;
		}
		Dictionary<ProjectionMember, Expression> dictionary = new Dictionary<ProjectionMember, Expression>();
		foreach (KeyValuePair<ProjectionMember, Expression> item in _projectionMapping)
		{
			if (item.Value is EntityProjectionExpression entityProjectionExpression)
			{
				Dictionary<IProperty, int> dictionary2 = new Dictionary<IProperty, int>();
				foreach (IProperty item2 in GetAllPropertiesInHierarchy(entityProjectionExpression.EntityType))
				{
					dictionary2[item2] = AddToProjection(entityProjectionExpression.BindProperty(item2));
				}
				dictionary[item.Key] = Expression.Constant(dictionary2);
			}
			else
			{
				dictionary[item.Key] = Expression.Constant(AddToProjection((SqlExpression)item.Value, item.Key.Last?.Name));
			}
		}
		_projectionMapping = dictionary;
	}

	private static IEnumerable<IProperty> GetAllPropertiesInHierarchy(IEntityType entityType)
	{
		return entityType.GetTypesInHierarchy().SelectMany(EntityTypeExtensions.GetDeclaredProperties);
	}

	public void ReplaceProjectionMapping(IDictionary<ProjectionMember, Expression> projectionMapping)
	{
		_projectionMapping.Clear();
		foreach (KeyValuePair<ProjectionMember, Expression> item in projectionMapping)
		{
			_projectionMapping[item.Key] = item.Value;
		}
	}

	public Expression GetMappedProjection(ProjectionMember projectionMember)
	{
		return _projectionMapping[projectionMember];
	}

	public int AddToProjection(SqlExpression sqlExpression)
	{
		return AddToProjection(sqlExpression, null);
	}

	private int AddToProjection(SqlExpression sqlExpression, string alias)
	{
		int num = _projection.FindIndex((ProjectionExpression pe) => pe.Expression.Equals(sqlExpression));
		if (num != -1)
		{
			return num;
		}
		string text = alias ?? (sqlExpression as ColumnExpression)?.Name ?? ((Alias != null) ? "c" : null);
		string currentAlias = text ?? "";
		if (Alias != null && text != null)
		{
			int num2 = 0;
			while (_projection.Any((ProjectionExpression pe) => string.Equals(pe.Alias, currentAlias, StringComparison.OrdinalIgnoreCase)))
			{
				currentAlias = $"{text}{num2++}";
			}
		}
		_projection.Add(new ProjectionExpression(sqlExpression, currentAlias));
		return _projection.Count - 1;
	}

	public IDictionary<IProperty, int> AddToProjection(EntityProjectionExpression entityProjection)
	{
		if (!_entityProjectionCache.TryGetValue(entityProjection, out var value))
		{
			value = new Dictionary<IProperty, int>();
			foreach (IProperty item in GetAllPropertiesInHierarchy(entityProjection.EntityType))
			{
				value[item] = AddToProjection(entityProjection.BindProperty(item));
			}
			_entityProjectionCache[entityProjection] = value;
		}
		return value;
	}

	public void PrepareForAggregate()
	{
		if (IsDistinct || Limit != null || Offset != null || GroupBy.Count > 0)
		{
			PushdownIntoSubquery();
		}
	}

	public void ApplyPredicate(SqlExpression expression)
	{
		if (!(expression is SqlConstantExpression sqlConstantExpression) || !(bool)sqlConstantExpression.Value)
		{
			if (Limit != null || Offset != null)
			{
				expression = new SqlRemappingVisitor(PushdownIntoSubquery(), (SelectExpression)Tables[0]).Remap(expression);
			}
			if (_groupBy.Count > 0)
			{
				Having = ((Having == null) ? expression : new SqlBinaryExpression(ExpressionType.AndAlso, Having, expression, typeof(bool), expression.TypeMapping));
			}
			else
			{
				Predicate = ((Predicate == null) ? expression : new SqlBinaryExpression(ExpressionType.AndAlso, Predicate, expression, typeof(bool), expression.TypeMapping));
			}
		}
	}

	public void ApplyGrouping(Expression keySelector)
	{
		ClearOrdering();
		AppendGroupBy(keySelector);
	}

	private void AppendGroupBy(Expression keySelector)
	{
		if (!(keySelector is SqlExpression sqlExpression))
		{
			if (keySelector is NewExpression newExpression)
			{
				{
					foreach (Expression argument in newExpression.Arguments)
					{
						AppendGroupBy(argument);
					}
					return;
				}
			}
			if (keySelector is MemberInitExpression memberInitExpression)
			{
				AppendGroupBy(memberInitExpression.NewExpression);
				{
					foreach (MemberBinding binding in memberInitExpression.Bindings)
					{
						AppendGroupBy(((MemberAssignment)binding).Expression);
					}
					return;
				}
			}
			if (!(keySelector is UnaryExpression unaryExpression) || (unaryExpression.NodeType != ExpressionType.Convert && unaryExpression.NodeType != ExpressionType.ConvertChecked))
			{
				throw new InvalidOperationException("Invalid keySelector for Group By");
			}
			AppendGroupBy(unaryExpression.Operand);
		}
		else if (!(sqlExpression is SqlConstantExpression) && !(sqlExpression is SqlParameterExpression))
		{
			_groupBy.Add(sqlExpression);
		}
	}

	public void ApplyOrdering(OrderingExpression orderingExpression)
	{
		if (IsDistinct || Limit != null || Offset != null)
		{
			orderingExpression = orderingExpression.Update(new SqlRemappingVisitor(PushdownIntoSubquery(), (SelectExpression)Tables[0]).Remap(orderingExpression.Expression));
		}
		_orderings.Clear();
		_orderings.Add(orderingExpression);
	}

	public void AppendOrdering(OrderingExpression orderingExpression)
	{
		if (_orderings.FirstOrDefault((OrderingExpression o) => o.Expression.Equals(orderingExpression.Expression)) == null)
		{
			_orderings.Add(orderingExpression);
		}
	}

	public void ApplyLimit(SqlExpression sqlExpression)
	{
		if (Limit != null)
		{
			PushdownIntoSubquery();
		}
		Limit = sqlExpression;
	}

	public void ApplyOffset(SqlExpression sqlExpression)
	{
		if (Limit != null || Offset != null)
		{
			PushdownIntoSubquery();
		}
		Offset = sqlExpression;
	}

	public void ReverseOrderings()
	{
		if (Limit != null || Offset != null)
		{
			PushdownIntoSubquery();
		}
		OrderingExpression[] array = _orderings.ToArray();
		_orderings.Clear();
		for (int i = 0; i < array.Length; i++)
		{
			_orderings.Add(new OrderingExpression(array[i].Expression, !array[i].IsAscending));
		}
	}

	public void ApplyDistinct()
	{
		if (Limit != null || Offset != null)
		{
			PushdownIntoSubquery();
		}
		IsDistinct = true;
		ClearOrdering();
	}

	public void ApplyDefaultIfEmpty(ISqlExpressionFactory sqlExpressionFactory)
	{
		SqlExpression expression = sqlExpressionFactory.ApplyDefaultTypeMapping(new SqlConstantExpression(Expression.Constant(null, typeof(string)), null));
		SelectExpression item = new SelectExpression("empty", new List<ProjectionExpression>
		{
			new ProjectionExpression(expression, "empty")
		}, new List<TableExpressionBase>(), new List<SqlExpression>(), new List<OrderingExpression>());
		if (Orderings.Any() || Limit != null || Offset != null || IsDistinct || Predicate != null || Tables.Count > 1 || GroupBy.Count > 1)
		{
			PushdownIntoSubquery();
		}
		SqlBinaryExpression joinPredicate = sqlExpressionFactory.Equal(sqlExpressionFactory.Constant(1), sqlExpressionFactory.Constant(1));
		LeftJoinExpression item2 = new LeftJoinExpression(Tables.Single(), joinPredicate);
		_tables.Clear();
		_tables.Add(item);
		_tables.Add(item2);
		Dictionary<ProjectionMember, Expression> dictionary = new Dictionary<ProjectionMember, Expression>();
		foreach (KeyValuePair<ProjectionMember, Expression> item3 in _projectionMapping)
		{
			Expression expression2 = item3.Value;
			if (expression2 is EntityProjectionExpression entityProjectionExpression)
			{
				expression2 = entityProjectionExpression.MakeNullable();
			}
			else if (expression2 is ColumnExpression columnExpression)
			{
				expression2 = columnExpression.MakeNullable();
			}
			dictionary[item3.Key] = expression2;
		}
		for (int i = 0; i < _identifier.Count; i++)
		{
			if (_identifier[i] is ColumnExpression columnExpression2)
			{
				_identifier[i] = columnExpression2.MakeNullable();
			}
		}
		for (int j = 0; j < _childIdentifiers.Count; j++)
		{
			if (_childIdentifiers[j] is ColumnExpression columnExpression3)
			{
				_childIdentifiers[j] = columnExpression3.MakeNullable();
			}
		}
		_projectionMapping = dictionary;
	}

	public void ClearOrdering()
	{
		_orderings.Clear();
	}

	public void ApplyExcept(SelectExpression source2, bool distinct)
	{
		ApplySetOperation(SetOperationType.Except, source2, distinct);
	}

	public void ApplyIntersect(SelectExpression source2, bool distinct)
	{
		ApplySetOperation(SetOperationType.Intersect, source2, distinct);
	}

	public void ApplyUnion(SelectExpression source2, bool distinct)
	{
		ApplySetOperation(SetOperationType.Union, source2, distinct);
	}

	private void ApplySetOperation(SetOperationType setOperationType, SelectExpression select2, bool distinct)
	{
		SelectExpression select3 = new SelectExpression(null, new List<ProjectionExpression>(), _tables.ToList(), _groupBy.ToList(), _orderings.ToList())
		{
			IsDistinct = IsDistinct,
			Predicate = Predicate,
			Having = Having,
			Offset = Offset,
			Limit = Limit
		};
		select3._projectionMapping = new Dictionary<ProjectionMember, Expression>(_projectionMapping);
		_projectionMapping.Clear();
		select3._identifier.AddRange(_identifier);
		_identifier.Clear();
		if (select3.Orderings.Count != 0 || select3.Limit != null || select3.Offset != null)
		{
			select3.PushdownIntoSubquery();
			select3.ClearOrdering();
		}
		if (select2.Orderings.Count != 0 || select2.Limit != null || select2.Offset != null)
		{
			select2.PushdownIntoSubquery();
			select2.ClearOrdering();
		}
		SetOperationBase setExpression = setOperationType switch
		{
			SetOperationType.Except => new ExceptExpression("t", select3, select2, distinct), 
			SetOperationType.Intersect => new IntersectExpression("t", select3, select2, distinct), 
			SetOperationType.Union => new UnionExpression("t", select3, select2, distinct), 
			_ => throw new InvalidOperationException(string.Format("Invalid {0}: {1}", "setOperationType", setOperationType)), 
		};
		if (_projection.Any() || select2._projection.Any())
		{
			throw new InvalidOperationException("Can't process set operations after client evaluation, consider moving the operation before the last Select() call (see issue #16243)");
		}
		if (select3._projectionMapping.Count != select2._projectionMapping.Count)
		{
			throw new InvalidOperationException("Different projection mapping count in set operation");
		}
		foreach (var item in select3._projectionMapping.Join(select2._projectionMapping, (KeyValuePair<ProjectionMember, Expression> kv) => kv.Key, (KeyValuePair<ProjectionMember, Expression> kv) => kv.Key, (KeyValuePair<ProjectionMember, Expression> kv1, KeyValuePair<ProjectionMember, Expression> kv2) => (Key: kv1.Key, Value1: kv1.Value, Value2: kv2.Value)))
		{
			if (item.Value1 is EntityProjectionExpression projection3 && item.Value2 is EntityProjectionExpression projection4)
			{
				HandleEntityMapping(item.Key, select3, projection3, select2, projection4);
				continue;
			}
			if (item.Value1 is SqlExpression sqlExpression && item.Value2 is SqlExpression sqlExpression2)
			{
				if (sqlExpression.TypeMapping.StoreType != sqlExpression2.TypeMapping.StoreType)
				{
					throw new InvalidOperationException("Set operations over different store types are currently unsupported");
				}
				string alias = GenerateUniqueAlias(item.Key.Last?.Name ?? (sqlExpression as ColumnExpression)?.Name ?? "c");
				ProjectionExpression projectionExpression2 = new ProjectionExpression(sqlExpression, alias);
				ProjectionExpression projectionExpression3 = new ProjectionExpression(sqlExpression2, alias);
				select3._projection.Add(projectionExpression2);
				select2._projection.Add(projectionExpression3);
				ColumnExpression columnExpression = new ColumnExpression(projectionExpression2, setExpression);
				if (IsNullableProjection(projectionExpression2) || IsNullableProjection(projectionExpression3))
				{
					columnExpression = columnExpression.MakeNullable();
				}
				_projectionMapping[item.Key] = columnExpression;
				continue;
			}
			throw new InvalidOperationException("Non-matching or unknown projection mapping type in set operation (" + item.Value1.GetType().Name + " and " + item.Value2.GetType().Name + ")");
		}
		Offset = null;
		Limit = null;
		IsDistinct = false;
		Predicate = null;
		Having = null;
		_groupBy.Clear();
		_orderings.Clear();
		_tables.Clear();
		_tables.Add(setExpression);
		ColumnExpression AddSetOperationColumnProjections(SelectExpression select1, ColumnExpression column1, SelectExpression select2, ColumnExpression column2)
		{
			string alias2 = GenerateUniqueAlias(column1.Name);
			ProjectionExpression projectionExpression4 = new ProjectionExpression(column1, alias2);
			ProjectionExpression projectionExpression5 = new ProjectionExpression(column2, alias2);
			select1._projection.Add(projectionExpression4);
			select2._projection.Add(projectionExpression5);
			ColumnExpression columnExpression3 = new ColumnExpression(projectionExpression4, setExpression);
			if (IsNullableProjection(projectionExpression4) || IsNullableProjection(projectionExpression5))
			{
				columnExpression3 = columnExpression3.MakeNullable();
			}
			if (select1._identifier.Contains(column1))
			{
				_identifier.Add(columnExpression3);
			}
			return columnExpression3;
		}
		string GenerateUniqueAlias(string baseAlias)
		{
			string currentAlias = baseAlias ?? "";
			int num = 0;
			while (select3._projection.Any((ProjectionExpression pe) => string.Equals(pe.Alias, currentAlias, StringComparison.OrdinalIgnoreCase)))
			{
				currentAlias = $"{baseAlias}{num++}";
			}
			return currentAlias;
		}
		void HandleEntityMapping(ProjectionMember projectionMember, SelectExpression select1, EntityProjectionExpression projection1, SelectExpression select2, EntityProjectionExpression projection2)
		{
			if (projection1.EntityType != projection2.EntityType)
			{
				throw new InvalidOperationException("Set operations over different entity types are currently unsupported (see #16298)");
			}
			Dictionary<IProperty, ColumnExpression> dictionary = new Dictionary<IProperty, ColumnExpression>();
			foreach (IProperty item2 in GetAllPropertiesInHierarchy(projection1.EntityType))
			{
				dictionary[item2] = AddSetOperationColumnProjections(select1, projection1.BindProperty(item2), select2, projection2.BindProperty(item2));
			}
			_projectionMapping[projectionMember] = new EntityProjectionExpression(projection1.EntityType, dictionary);
		}
		static bool IsNullableProjection(ProjectionExpression projectionExpression)
		{
			SqlExpression expression = projectionExpression.Expression;
			if (!(expression is ColumnExpression { IsNullable: var isNullable }))
			{
				if (expression is SqlConstantExpression sqlConstantExpression)
				{
					return sqlConstantExpression.Value == null;
				}
				return true;
			}
			return isNullable;
		}
	}

	private ColumnExpression GenerateOuterColumn(SqlExpression projection, string alias = null)
	{
		int index = AddToProjection(projection, alias);
		return new ColumnExpression(_projection[index], this);
	}

	public IDictionary<SqlExpression, ColumnExpression> PushdownIntoSubquery()
	{
		SelectExpression subquery = new SelectExpression("t", new List<ProjectionExpression>(), _tables.ToList(), _groupBy.ToList(), _orderings.ToList())
		{
			IsDistinct = IsDistinct,
			Predicate = Predicate,
			Having = Having,
			Offset = Offset,
			Limit = Limit
		};
		Dictionary<SqlExpression, ColumnExpression> projectionMap = new Dictionary<SqlExpression, ColumnExpression>();
		if (_projection.Any())
		{
			List<SqlExpression> list = _projection.Select((ProjectionExpression pe) => pe.Expression).ToList();
			_projection.Clear();
			foreach (SqlExpression item in list)
			{
				ColumnExpression columnExpression = subquery.GenerateOuterColumn(item);
				AddToProjection(columnExpression);
				projectionMap[item] = columnExpression;
			}
		}
		foreach (KeyValuePair<ProjectionMember, Expression> item2 in _projectionMapping.ToList())
		{
			if (item2.Value is ConstantExpression)
			{
				break;
			}
			if (item2.Value is EntityProjectionExpression entityProjection2)
			{
				_projectionMapping[item2.Key] = LiftEntityProjectionFromSubquery(entityProjection2);
				continue;
			}
			SqlExpression sqlExpression = (SqlExpression)item2.Value;
			ColumnExpression value = subquery.GenerateOuterColumn(sqlExpression);
			projectionMap[sqlExpression] = value;
			_projectionMapping[item2.Key] = value;
		}
		List<SqlExpression> list2 = _identifier.ToList();
		_identifier.Clear();
		foreach (SqlExpression item3 in list2)
		{
			if (projectionMap.TryGetValue(item3, out var value2))
			{
				_identifier.Add(value2);
			}
			else if (!IsDistinct && GroupBy.Count == 0)
			{
				value2 = subquery.GenerateOuterColumn(item3);
				_identifier.Add(value2);
			}
		}
		List<SqlExpression> list3 = _childIdentifiers.ToList();
		_childIdentifiers.Clear();
		foreach (SqlExpression item4 in list3)
		{
			if (projectionMap.TryGetValue(item4, out var value3))
			{
				_childIdentifiers.Add(value3);
			}
			else if (!IsDistinct && GroupBy.Count == 0)
			{
				value3 = subquery.GenerateOuterColumn(item4);
				_childIdentifiers.Add(value3);
			}
		}
		List<SelectExpression> source = _pendingCollections.ToList();
		_pendingCollections.Clear();
		_pendingCollections.AddRange(source.Select(new SqlRemappingVisitor(projectionMap, subquery).Remap));
		_orderings.Clear();
		if (!subquery.IsDistinct)
		{
			foreach (OrderingExpression ordering in subquery._orderings)
			{
				SqlExpression expression = ordering.Expression;
				if (!projectionMap.TryGetValue(expression, out var value4))
				{
					value4 = subquery.GenerateOuterColumn(expression);
				}
				_orderings.Add(ordering.Update(value4));
			}
		}
		if (subquery.Offset == null && subquery.Limit == null)
		{
			subquery.ClearOrdering();
		}
		Offset = null;
		Limit = null;
		IsDistinct = false;
		Predicate = null;
		Having = null;
		_tables.Clear();
		_tables.Add(subquery);
		_groupBy.Clear();
		return projectionMap;
		EntityProjectionExpression LiftEntityProjectionFromSubquery(EntityProjectionExpression entityProjection)
		{
			Dictionary<IProperty, ColumnExpression> dictionary = new Dictionary<IProperty, ColumnExpression>();
			foreach (IProperty item5 in GetAllPropertiesInHierarchy(entityProjection.EntityType))
			{
				ColumnExpression columnExpression2 = entityProjection.BindProperty(item5);
				ColumnExpression value5 = subquery.GenerateOuterColumn(columnExpression2);
				projectionMap[columnExpression2] = value5;
				dictionary[item5] = value5;
			}
			EntityProjectionExpression entityProjectionExpression = new EntityProjectionExpression(entityProjection.EntityType, dictionary);
			foreach (INavigation item6 in entityProjection.EntityType.GetTypesInHierarchy().SelectMany(EntityTypeExtensions.GetDeclaredNavigations))
			{
				EntityShaperExpression entityShaperExpression = entityProjection.BindNavigation(item6);
				if (entityShaperExpression != null)
				{
					EntityProjectionExpression valueBufferExpression = LiftEntityProjectionFromSubquery((EntityProjectionExpression)entityShaperExpression.ValueBufferExpression);
					entityShaperExpression = entityShaperExpression.Update(valueBufferExpression);
					entityProjectionExpression.AddNavigationBinding(item6, entityShaperExpression);
				}
			}
			return entityProjectionExpression;
		}
	}

	public Expression AddSingleProjection(ShapedQueryExpression shapedQueryExpression)
	{
		SelectExpression selectExpression = (SelectExpression)shapedQueryExpression.QueryExpression;
		Expression expression2 = shapedQueryExpression.ShaperExpression;
		if (!(RemoveConvert(expression2) is EntityShaperExpression))
		{
			SqlExpression limit = selectExpression.Limit;
			ProjectionBindingExpression projectionBindingExpression;
			if (selectExpression.Projection.Any())
			{
				int index = selectExpression.AddToProjection(limit);
				projectionBindingExpression = new ProjectionBindingExpression(selectExpression, index, limit.Type);
			}
			else
			{
				selectExpression._projectionMapping[new ProjectionMember()] = limit;
				projectionBindingExpression = new ProjectionBindingExpression(selectExpression, new ProjectionMember(), limit.Type);
			}
			expression2 = Expression.Condition(Expression.Equal(projectionBindingExpression, Expression.Default(projectionBindingExpression.Type)), Expression.Default(expression2.Type), expression2);
		}
		selectExpression.ApplyProjection();
		int count = selectExpression.Projection.Count;
		AddOuterApply(selectExpression, null);
		SelectExpression selectExpression2 = (SelectExpression)((JoinExpressionBase)_tables[_tables.Count - 1]).Table;
		int count2 = _projection.Count;
		foreach (ProjectionExpression item in selectExpression2.Projection.Take(count))
		{
			AddToProjection(MakeNullable(selectExpression2.GenerateOuterColumn(item.Expression)));
		}
		return new ShaperRemappingExpressionVisitor(this, selectExpression, count2).Visit(expression2);
		static Expression RemoveConvert(Expression expression)
		{
			if (!(expression is UnaryExpression { NodeType: ExpressionType.Convert } unaryExpression))
			{
				return expression;
			}
			return RemoveConvert(unaryExpression.Operand);
		}
	}

	public CollectionShaperExpression AddCollectionProjection(ShapedQueryExpression shapedQueryExpression, INavigation navigation, Type elementType)
	{
		SelectExpression item = (SelectExpression)shapedQueryExpression.QueryExpression;
		_pendingCollections.Add(item);
		return new CollectionShaperExpression(new ProjectionBindingExpression(this, _pendingCollections.Count - 1, typeof(object)), shapedQueryExpression.ShaperExpression, navigation, elementType);
	}

	public Expression ApplyCollectionJoin(int collectionIndex, int collectionId, Expression innerShaper, INavigation navigation, Type elementType)
	{
		SelectExpression selectExpression = _pendingCollections[collectionIndex];
		_pendingCollections[collectionIndex] = null;
		Expression identifierAccessor = GetIdentifierAccessor(_identifier);
		Expression identifierAccessor2 = GetIdentifierAccessor(_identifier.Concat(_childIdentifiers));
		selectExpression.ApplyProjection();
		Expression identifierAccessor3 = selectExpression.GetIdentifierAccessor(selectExpression._identifier);
		if (collectionIndex == 0)
		{
			foreach (SqlExpression item2 in _identifier)
			{
				AppendOrdering(new OrderingExpression(item2, ascending: true));
			}
		}
		SqlExpression sqlExpression = TryExtractJoinKey(selectExpression);
		if (new SelectExpressionCorrelationFindingExpressionVisitor(this).ContainsOuterReference(selectExpression) && sqlExpression != null)
		{
			selectExpression.ApplyPredicate(sqlExpression);
			sqlExpression = null;
		}
		if (selectExpression.Offset != null || selectExpression.Limit != null || selectExpression.IsDistinct || selectExpression.Predicate != null || selectExpression.Tables.Count > 1 || selectExpression.GroupBy.Count > 1)
		{
			sqlExpression = new SqlRemappingVisitor(selectExpression.PushdownIntoSubquery(), (SelectExpression)selectExpression.Tables[0]).Remap(sqlExpression);
		}
		TableExpressionBase item = ((sqlExpression == null) ? ((JoinExpressionBase)new OuterApplyExpression(selectExpression.Tables.Single())) : ((JoinExpressionBase)new LeftJoinExpression(selectExpression.Tables.Single(), sqlExpression)));
		_tables.Add(item);
		foreach (OrderingExpression ordering in selectExpression.Orderings)
		{
			AppendOrdering(ordering.Update(MakeNullable(ordering.Expression)));
		}
		int count = _projection.Count;
		int count2 = selectExpression.Projection.Count;
		int[] array = new int[count2];
		for (int i = 0; i < count2; i++)
		{
			array[i] = AddToProjection(MakeNullable(selectExpression.Projection[i].Expression));
		}
		foreach (SqlExpression item3 in selectExpression._identifier.Concat(selectExpression._childIdentifiers))
		{
			SqlExpression sqlExpression2 = MakeNullable(item3);
			_childIdentifiers.Add(sqlExpression2);
			AppendOrdering(new OrderingExpression(sqlExpression2, ascending: true));
		}
		bool isEnabled;
		ShaperRemappingExpressionVisitor obj = ((AppContext.TryGetSwitch("Microsoft.EntityFrameworkCore.Issue19616", out isEnabled) && isEnabled) ? new ShaperRemappingExpressionVisitor(this, selectExpression, count) : new ShaperRemappingExpressionVisitor(this, selectExpression, array));
		innerShaper = obj.Visit(innerShaper);
		identifierAccessor3 = obj.Visit(identifierAccessor3);
		return new RelationalCollectionShaperExpression(collectionId, identifierAccessor, identifierAccessor2, identifierAccessor3, innerShaper, navigation, elementType);
	}

	private static SqlExpression MakeNullable(SqlExpression sqlExpression)
	{
		if (!(sqlExpression is ColumnExpression columnExpression))
		{
			return sqlExpression;
		}
		return columnExpression.MakeNullable();
	}

	private Expression GetIdentifierAccessor(IEnumerable<SqlExpression> identifyingProjection)
	{
		List<Expression> list = new List<Expression>();
		foreach (SqlExpression item in identifyingProjection)
		{
			int index = AddToProjection(item);
			ProjectionBindingExpression projectionBindingExpression = new ProjectionBindingExpression(this, index, item.Type.MakeNullable());
			list.Add(projectionBindingExpression.Type.IsValueType ? ((Expression)Expression.Convert(projectionBindingExpression, typeof(object))) : ((Expression)projectionBindingExpression));
		}
		return Expression.NewArrayInit(typeof(object), list);
	}

	private SqlExpression TryExtractJoinKey(SelectExpression selectExpression)
	{
		if (selectExpression.Limit == null && selectExpression.Offset == null && selectExpression.Predicate != null)
		{
			List<ColumnExpression> columnExpressions = new List<ColumnExpression>();
			SqlExpression updatedPredicate;
			SqlExpression sqlExpression = TryExtractJoinKey(selectExpression, selectExpression.Predicate, columnExpressions, out updatedPredicate);
			if (sqlExpression != null)
			{
				sqlExpression = RemoveRedundantNullChecks(sqlExpression, columnExpressions);
			}
			selectExpression.Predicate = updatedPredicate;
			return sqlExpression;
		}
		return null;
	}

	private SqlExpression TryExtractJoinKey(SelectExpression selectExpression, SqlExpression predicate, List<ColumnExpression> columnExpressions, out SqlExpression updatedPredicate)
	{
		if (predicate is SqlBinaryExpression sqlBinaryExpression)
		{
			SqlBinaryExpression sqlBinaryExpression2 = ValidateKeyComparison(selectExpression, sqlBinaryExpression, columnExpressions);
			if (sqlBinaryExpression2 != null)
			{
				updatedPredicate = null;
				return sqlBinaryExpression2;
			}
			if (sqlBinaryExpression.OperatorType == ExpressionType.AndAlso)
			{
				SqlExpression updatedPredicate2;
				SqlExpression left = TryExtractJoinKey(selectExpression, sqlBinaryExpression.Left, columnExpressions, out updatedPredicate2);
				SqlExpression updatedPredicate3;
				SqlExpression right = TryExtractJoinKey(selectExpression, sqlBinaryExpression.Right, columnExpressions, out updatedPredicate3);
				updatedPredicate = CombineNonNullExpressions(updatedPredicate2, updatedPredicate3);
				return CombineNonNullExpressions(left, right);
			}
		}
		updatedPredicate = predicate;
		return null;
	}

	private static SqlExpression CombineNonNullExpressions(SqlExpression left, SqlExpression right)
	{
		if (left == null)
		{
			return right;
		}
		if (right == null)
		{
			return left;
		}
		return new SqlBinaryExpression(ExpressionType.AndAlso, left, right, left.Type, left.TypeMapping);
	}

	private SqlBinaryExpression ValidateKeyComparison(SelectExpression inner, SqlBinaryExpression sqlBinaryExpression, List<ColumnExpression> columnExpressions)
	{
		if (sqlBinaryExpression.OperatorType == ExpressionType.Equal && sqlBinaryExpression.Left is ColumnExpression columnExpression && sqlBinaryExpression.Right is ColumnExpression columnExpression2)
		{
			if (ContainsTableReference(columnExpression.Table) && inner.ContainsTableReference(columnExpression2.Table))
			{
				columnExpressions.Add(columnExpression);
				return sqlBinaryExpression;
			}
			if (ContainsTableReference(columnExpression2.Table) && inner.ContainsTableReference(columnExpression.Table))
			{
				columnExpressions.Add(columnExpression2);
				return sqlBinaryExpression.Update(sqlBinaryExpression.Right, sqlBinaryExpression.Left);
			}
		}
		if (sqlBinaryExpression.OperatorType == ExpressionType.NotEqual)
		{
			if (sqlBinaryExpression.Left is ColumnExpression columnExpression3 && ContainsTableReference(columnExpression3.Table) && sqlBinaryExpression.Right is SqlConstantExpression { Value: null })
			{
				return sqlBinaryExpression;
			}
			if (sqlBinaryExpression.Right is ColumnExpression columnExpression4 && ContainsTableReference(columnExpression4.Table) && sqlBinaryExpression.Left is SqlConstantExpression { Value: null })
			{
				return sqlBinaryExpression.Update(sqlBinaryExpression.Right, sqlBinaryExpression.Left);
			}
		}
		return null;
	}

	private SqlExpression RemoveRedundantNullChecks(SqlExpression predicate, List<ColumnExpression> columnExpressions)
	{
		if (predicate is SqlBinaryExpression sqlBinaryExpression)
		{
			if (sqlBinaryExpression.OperatorType == ExpressionType.NotEqual && sqlBinaryExpression.Left is ColumnExpression item && columnExpressions.Contains(item) && sqlBinaryExpression.Right is SqlConstantExpression { Value: null })
			{
				return null;
			}
			if (sqlBinaryExpression.OperatorType == ExpressionType.AndAlso)
			{
				SqlExpression left = RemoveRedundantNullChecks(sqlBinaryExpression.Left, columnExpressions);
				SqlExpression right = RemoveRedundantNullChecks(sqlBinaryExpression.Right, columnExpressions);
				return CombineNonNullExpressions(left, right);
			}
		}
		return predicate;
	}

	private bool ContainsTableReference(TableExpressionBase table)
	{
		return Tables.Any((TableExpressionBase te) => ((te is JoinExpressionBase joinExpressionBase) ? joinExpressionBase.Table : te) == table);
	}

	private void GetPartitions(SqlExpression sqlExpression, List<SqlExpression> partitions)
	{
		if (sqlExpression is SqlBinaryExpression sqlBinaryExpression)
		{
			if (sqlBinaryExpression.OperatorType == ExpressionType.Equal)
			{
				partitions.Add(sqlBinaryExpression.Right);
			}
			else if (sqlBinaryExpression.OperatorType == ExpressionType.AndAlso)
			{
				GetPartitions(sqlBinaryExpression.Left, partitions);
				GetPartitions(sqlBinaryExpression.Right, partitions);
			}
		}
	}

	private void AddJoin(JoinType joinType, SelectExpression innerSelectExpression, Type transparentIdentifierType, SqlExpression joinPredicate = null)
	{
		if (joinType == JoinType.CrossApply || joinType == JoinType.OuterApply)
		{
			SqlExpression limit = innerSelectExpression.Limit;
			innerSelectExpression.Limit = null;
			joinPredicate = TryExtractJoinKey(innerSelectExpression);
			if (joinPredicate != null)
			{
				if (!new SelectExpressionCorrelationFindingExpressionVisitor(this).ContainsOuterReference(innerSelectExpression))
				{
					if (limit != null)
					{
						List<SqlExpression> partitions = new List<SqlExpression>();
						GetPartitions(joinPredicate, partitions);
						IEnumerable<OrderingExpression> enumerable;
						if (!innerSelectExpression.Orderings.Any())
						{
							enumerable = innerSelectExpression._identifier.Select((SqlExpression e) => new OrderingExpression(e, ascending: true));
						}
						else
						{
							IEnumerable<OrderingExpression> orderings = innerSelectExpression.Orderings;
							enumerable = orderings;
						}
						IEnumerable<OrderingExpression> source = enumerable;
						RowNumberExpression projection = new RowNumberExpression(partitions, source.ToList(), limit.TypeMapping);
						innerSelectExpression.ClearOrdering();
						IDictionary<SqlExpression, ColumnExpression> mappings = innerSelectExpression.PushdownIntoSubquery();
						SelectExpression selectExpression = (SelectExpression)innerSelectExpression.Tables[0];
						joinPredicate = new SqlRemappingVisitor(mappings, selectExpression).Remap(joinPredicate);
						ColumnExpression left = selectExpression.GenerateOuterColumn(projection, "row");
						SqlBinaryExpression expression = new SqlBinaryExpression(ExpressionType.LessThanOrEqual, left, limit, typeof(bool), joinPredicate.TypeMapping);
						innerSelectExpression.ApplyPredicate(expression);
					}
					AddJoin((joinType != JoinType.CrossApply) ? JoinType.LeftJoin : JoinType.InnerJoin, innerSelectExpression, transparentIdentifierType, joinPredicate);
					return;
				}
				innerSelectExpression.ApplyPredicate(joinPredicate);
				innerSelectExpression.ApplyLimit(limit);
			}
			else
			{
				innerSelectExpression.ApplyLimit(limit);
			}
		}
		if (Limit != null || Offset != null || IsDistinct || GroupBy.Count > 0)
		{
			SqlRemappingVisitor sqlRemappingVisitor = new SqlRemappingVisitor(PushdownIntoSubquery(), (SelectExpression)Tables[0]);
			innerSelectExpression = sqlRemappingVisitor.Remap(innerSelectExpression);
			joinPredicate = sqlRemappingVisitor.Remap(joinPredicate);
		}
		if (innerSelectExpression.Orderings.Any() || innerSelectExpression.Limit != null || innerSelectExpression.Offset != null || innerSelectExpression.IsDistinct || innerSelectExpression.Predicate != null || innerSelectExpression.Tables.Count > 1 || innerSelectExpression.GroupBy.Count > 0)
		{
			joinPredicate = new SqlRemappingVisitor(innerSelectExpression.PushdownIntoSubquery(), (SelectExpression)innerSelectExpression.Tables[0]).Remap(joinPredicate);
		}
		if (joinType != JoinType.LeftJoin)
		{
			_identifier.AddRange(innerSelectExpression._identifier);
		}
		TableExpressionBase table = innerSelectExpression.Tables.Single();
		TableExpressionBase item = joinType switch
		{
			JoinType.InnerJoin => new InnerJoinExpression(table, joinPredicate), 
			JoinType.LeftJoin => new LeftJoinExpression(table, joinPredicate), 
			JoinType.CrossJoin => new CrossJoinExpression(table), 
			JoinType.CrossApply => new CrossApplyExpression(table), 
			JoinType.OuterApply => new OuterApplyExpression(table), 
			_ => throw new InvalidOperationException(string.Format("Invalid {0}: {1}", "joinType", joinType)), 
		};
		_tables.Add(item);
		if (!(transparentIdentifierType != null))
		{
			return;
		}
		FieldInfo declaredField = transparentIdentifierType.GetTypeInfo().GetDeclaredField("Outer");
		Dictionary<ProjectionMember, Expression> dictionary = new Dictionary<ProjectionMember, Expression>();
		foreach (KeyValuePair<ProjectionMember, Expression> item2 in _projectionMapping)
		{
			dictionary[item2.Key.Prepend(declaredField)] = item2.Value;
		}
		FieldInfo declaredField2 = transparentIdentifierType.GetTypeInfo().GetDeclaredField("Inner");
		bool flag = joinType == JoinType.LeftJoin || joinType == JoinType.OuterApply;
		foreach (KeyValuePair<ProjectionMember, Expression> item3 in innerSelectExpression._projectionMapping)
		{
			Expression expression2 = item3.Value;
			if (flag)
			{
				if (expression2 is EntityProjectionExpression entityProjectionExpression)
				{
					expression2 = entityProjectionExpression.MakeNullable();
				}
				else if (expression2 is ColumnExpression columnExpression)
				{
					expression2 = columnExpression.MakeNullable();
				}
			}
			dictionary[item3.Key.Prepend(declaredField2)] = expression2;
		}
		_projectionMapping = dictionary;
	}

	public void AddInnerJoin(SelectExpression innerSelectExpression, SqlExpression joinPredicate, Type transparentIdentifierType)
	{
		AddJoin(JoinType.InnerJoin, innerSelectExpression, transparentIdentifierType, joinPredicate);
	}

	public void AddLeftJoin(SelectExpression innerSelectExpression, SqlExpression joinPredicate, Type transparentIdentifierType)
	{
		AddJoin(JoinType.LeftJoin, innerSelectExpression, transparentIdentifierType, joinPredicate);
	}

	public void AddCrossJoin(SelectExpression innerSelectExpression, Type transparentIdentifierType)
	{
		AddJoin(JoinType.CrossJoin, innerSelectExpression, transparentIdentifierType);
	}

	public void AddCrossApply(SelectExpression innerSelectExpression, Type transparentIdentifierType)
	{
		AddJoin(JoinType.CrossApply, innerSelectExpression, transparentIdentifierType);
	}

	public void AddOuterApply(SelectExpression innerSelectExpression, Type transparentIdentifierType)
	{
		AddJoin(JoinType.OuterApply, innerSelectExpression, transparentIdentifierType);
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		if (_pendingCollections.Any((SelectExpression e) => e != null))
		{
			if (Projection.Any())
			{
				List<ProjectionExpression> source = _projection.ToList();
				_projection.Clear();
				_projection.AddRange(source.Select((ProjectionExpression e) => (ProjectionExpression)visitor.Visit(e)));
			}
			else
			{
				Dictionary<ProjectionMember, Expression> dictionary = new Dictionary<ProjectionMember, Expression>();
				foreach (KeyValuePair<ProjectionMember, Expression> item in _projectionMapping)
				{
					Expression value = visitor.Visit(item.Value);
					dictionary[item.Key] = value;
				}
				_projectionMapping = dictionary;
			}
			List<TableExpressionBase> source2 = _tables.ToList();
			_tables.Clear();
			_tables.AddRange(source2.Select((TableExpressionBase e) => (TableExpressionBase)visitor.Visit(e)));
			Predicate = (SqlExpression)visitor.Visit(Predicate);
			List<SqlExpression> source3 = _groupBy.ToList();
			_groupBy.Clear();
			_groupBy.AddRange(from e in source3
				select (SqlExpression)visitor.Visit(e) into e
				where !(e is SqlConstantExpression) && !(e is SqlParameterExpression)
				select e);
			Having = (SqlExpression)visitor.Visit(Having);
			List<OrderingExpression> source4 = _orderings.ToList();
			_orderings.Clear();
			_orderings.AddRange(source4.Select((OrderingExpression e) => e.Update((SqlExpression)visitor.Visit(e.Expression))));
			Offset = (SqlExpression)visitor.Visit(Offset);
			Limit = (SqlExpression)visitor.Visit(Limit);
			return this;
		}
		bool flag = false;
		List<ProjectionExpression> list = _projection;
		IDictionary<ProjectionMember, Expression> dictionary2 = _projectionMapping;
		if (_projection.Any())
		{
			for (int i = 0; i < _projection.Count; i++)
			{
				ProjectionExpression projectionExpression = _projection[i];
				ProjectionExpression projectionExpression2 = (ProjectionExpression)visitor.Visit(projectionExpression);
				if (projectionExpression2 != projectionExpression && list == _projection)
				{
					list = new List<ProjectionExpression>(_projection.Count);
					for (int j = 0; j < i; j++)
					{
						list.Add(_projection[j]);
					}
					flag = true;
				}
				if (list != _projection)
				{
					list.Add(projectionExpression2);
				}
			}
		}
		else
		{
			foreach (KeyValuePair<ProjectionMember, Expression> item2 in _projectionMapping)
			{
				Expression expression = visitor.Visit(item2.Value);
				if (expression != item2.Value && dictionary2 == _projectionMapping)
				{
					dictionary2 = new Dictionary<ProjectionMember, Expression>(_projectionMapping);
					flag = true;
				}
				if (dictionary2 != _projectionMapping)
				{
					dictionary2[item2.Key] = expression;
				}
			}
		}
		List<TableExpressionBase> list2 = _tables;
		for (int k = 0; k < _tables.Count; k++)
		{
			TableExpressionBase tableExpressionBase = _tables[k];
			TableExpressionBase tableExpressionBase2 = (TableExpressionBase)visitor.Visit(tableExpressionBase);
			if (tableExpressionBase2 != tableExpressionBase && list2 == _tables)
			{
				list2 = new List<TableExpressionBase>(_tables.Count);
				for (int l = 0; l < k; l++)
				{
					list2.Add(_tables[l]);
				}
				flag = true;
			}
			if (list2 != _tables)
			{
				list2.Add(tableExpressionBase2);
			}
		}
		SqlExpression sqlExpression = (SqlExpression)visitor.Visit(Predicate);
		flag |= sqlExpression != Predicate;
		List<SqlExpression> list3 = _groupBy;
		for (int m = 0; m < _groupBy.Count; m++)
		{
			SqlExpression sqlExpression2 = _groupBy[m];
			SqlExpression sqlExpression3 = (SqlExpression)visitor.Visit(sqlExpression2);
			if (sqlExpression3 != sqlExpression2 || sqlExpression3 is SqlConstantExpression || sqlExpression3 is SqlParameterExpression)
			{
				if (list3 == _groupBy)
				{
					list3 = new List<SqlExpression>(_groupBy.Count);
					for (int n = 0; n < m; n++)
					{
						list3.Add(_groupBy[n]);
					}
				}
				flag = true;
			}
			if (list3 != _groupBy && !(sqlExpression3 is SqlConstantExpression) && !(sqlExpression3 is SqlParameterExpression))
			{
				list3.Add(sqlExpression3);
			}
		}
		SqlExpression sqlExpression4 = (SqlExpression)visitor.Visit(Having);
		flag |= sqlExpression4 != Having;
		List<OrderingExpression> list4 = _orderings;
		for (int num = 0; num < _orderings.Count; num++)
		{
			OrderingExpression orderingExpression = _orderings[num];
			OrderingExpression orderingExpression2 = (OrderingExpression)visitor.Visit(orderingExpression);
			if (orderingExpression2 != orderingExpression && list4 == _orderings)
			{
				list4 = new List<OrderingExpression>(_orderings.Count);
				for (int num2 = 0; num2 < num; num2++)
				{
					list4.Add(_orderings[num2]);
				}
				flag = true;
			}
			if (list4 != _orderings)
			{
				list4.Add(orderingExpression2);
			}
		}
		SqlExpression sqlExpression5 = (SqlExpression)visitor.Visit(Offset);
		flag |= sqlExpression5 != Offset;
		SqlExpression sqlExpression6 = (SqlExpression)visitor.Visit(Limit);
		if (flag | (sqlExpression6 != Limit))
		{
			SelectExpression selectExpression = new SelectExpression(Alias, list, list2, list3, list4);
			selectExpression._projectionMapping = dictionary2;
			selectExpression.Predicate = sqlExpression;
			selectExpression.Having = sqlExpression4;
			selectExpression.Offset = sqlExpression5;
			selectExpression.Limit = sqlExpression6;
			selectExpression.IsDistinct = IsDistinct;
			selectExpression.Tags = Tags;
			selectExpression._identifier.AddRange(_identifier);
			selectExpression._identifier.AddRange(_childIdentifiers);
			return selectExpression;
		}
		return this;
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj is SelectExpression selectExpression)
				{
					return Equals(selectExpression);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(SelectExpression selectExpression)
	{
		if (!base.Equals((object)selectExpression))
		{
			return false;
		}
		if (_projectionMapping.Count != selectExpression._projectionMapping.Count)
		{
			return false;
		}
		foreach (KeyValuePair<ProjectionMember, Expression> item in _projectionMapping)
		{
			if (!selectExpression._projectionMapping.TryGetValue(item.Key, out var value))
			{
				return false;
			}
			if (!item.Value.Equals(value))
			{
				return false;
			}
		}
		if (!Tags.SequenceEqual(selectExpression.Tags))
		{
			return false;
		}
		if (!_tables.SequenceEqual(selectExpression._tables))
		{
			return false;
		}
		if ((Predicate != null || selectExpression.Predicate != null) && (Predicate == null || !Predicate.Equals(selectExpression.Predicate)))
		{
			return false;
		}
		if (!_pendingCollections.SequenceEqual(selectExpression._pendingCollections))
		{
			return false;
		}
		if (!_groupBy.SequenceEqual(selectExpression._groupBy))
		{
			return false;
		}
		if ((Having != null || selectExpression.Having != null) && (Having == null || !Predicate.Equals(selectExpression.Having)))
		{
			return false;
		}
		if (!_orderings.SequenceEqual(selectExpression._orderings))
		{
			return false;
		}
		if ((Offset != null || selectExpression.Offset != null) && (Offset == null || !Offset.Equals(selectExpression.Offset)))
		{
			return false;
		}
		if ((Limit != null || selectExpression.Limit != null) && (Limit == null || !Limit.Equals(selectExpression.Limit)))
		{
			return false;
		}
		return IsDistinct == selectExpression.IsDistinct;
	}

	public SelectExpression Update(List<ProjectionExpression> projections, List<TableExpressionBase> tables, SqlExpression predicate, List<SqlExpression> groupBy, SqlExpression havingExpression, List<OrderingExpression> orderings, SqlExpression limit, SqlExpression offset, bool distinct, string alias)
	{
		Dictionary<ProjectionMember, Expression> dictionary = new Dictionary<ProjectionMember, Expression>();
		foreach (KeyValuePair<ProjectionMember, Expression> item in _projectionMapping)
		{
			dictionary[item.Key] = item.Value;
		}
		return new SelectExpression(alias, projections, tables, groupBy, orderings)
		{
			_projectionMapping = dictionary,
			Predicate = predicate,
			Having = havingExpression,
			Offset = offset,
			Limit = limit,
			IsDistinct = distinct,
			Tags = Tags
		};
	}

	public override int GetHashCode()
	{
		HashCode hashCode = default(HashCode);
		hashCode.Add(base.GetHashCode());
		foreach (KeyValuePair<ProjectionMember, Expression> item in _projectionMapping)
		{
			hashCode.Add(item.Key);
			hashCode.Add(item.Value);
		}
		foreach (string tag in Tags)
		{
			hashCode.Add(tag);
		}
		foreach (TableExpressionBase table in _tables)
		{
			hashCode.Add(table);
		}
		hashCode.Add(Predicate);
		foreach (SqlExpression item2 in _groupBy)
		{
			hashCode.Add(item2);
		}
		hashCode.Add(Having);
		foreach (OrderingExpression ordering in _orderings)
		{
			hashCode.Add(ordering);
		}
		hashCode.Add(Offset);
		hashCode.Add(Limit);
		hashCode.Add(IsDistinct);
		return hashCode.ToHashCode();
	}

	public override void Print(ExpressionPrinter expressionPrinter)
	{
		expressionPrinter.AppendLine("Projection Mapping:");
		using (expressionPrinter.Indent())
		{
			foreach (KeyValuePair<ProjectionMember, Expression> item in _projectionMapping)
			{
				expressionPrinter.AppendLine();
				expressionPrinter.Append(item.Key?.ToString() + " -> ");
				expressionPrinter.Visit(item.Value);
			}
		}
		expressionPrinter.AppendLine();
		foreach (string tag in Tags)
		{
			expressionPrinter.Append("-- " + tag);
		}
		IDisposable disposable2 = null;
		if (Alias != null)
		{
			expressionPrinter.AppendLine("(");
			disposable2 = expressionPrinter.Indent();
		}
		expressionPrinter.Append("SELECT ");
		if (IsDistinct)
		{
			expressionPrinter.Append("DISTINCT ");
		}
		if (Limit != null && Offset == null)
		{
			expressionPrinter.Append("TOP(");
			expressionPrinter.Visit(Limit);
			expressionPrinter.Append(") ");
		}
		if (Projection.Any())
		{
			expressionPrinter.VisitList(Projection);
		}
		else
		{
			expressionPrinter.Append("1");
		}
		if (Tables.Any())
		{
			expressionPrinter.AppendLine().Append("FROM ");
			expressionPrinter.VisitList(Tables, delegate(ExpressionPrinter p)
			{
				p.AppendLine();
			});
		}
		if (Predicate != null)
		{
			expressionPrinter.AppendLine().Append("WHERE ");
			expressionPrinter.Visit(Predicate);
		}
		if (GroupBy.Any())
		{
			expressionPrinter.AppendLine().Append("GROUP BY ");
			expressionPrinter.VisitList(GroupBy);
		}
		if (Having != null)
		{
			expressionPrinter.AppendLine().Append("HAVING ");
			expressionPrinter.Visit(Having);
		}
		if (Orderings.Any())
		{
			expressionPrinter.AppendLine().Append("ORDER BY ");
			expressionPrinter.VisitList(Orderings);
		}
		else if (Offset != null)
		{
			expressionPrinter.AppendLine().Append("ORDER BY (SELECT 1)");
		}
		if (Offset != null)
		{
			expressionPrinter.AppendLine().Append("OFFSET ");
			expressionPrinter.Visit(Offset);
			expressionPrinter.Append(" ROWS");
			if (Limit != null)
			{
				expressionPrinter.Append(" FETCH NEXT ");
				expressionPrinter.Visit(Limit);
				expressionPrinter.Append(" ROWS ONLY");
			}
		}
		if (Alias != null)
		{
			disposable2?.Dispose();
			expressionPrinter.AppendLine().Append(") AS " + Alias);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.Query;

public class EntityProjectionExpression : Expression
{
	private readonly IDictionary<IProperty, ColumnExpression> _propertyExpressionsCache = new Dictionary<IProperty, ColumnExpression>();

	private readonly IDictionary<INavigation, EntityShaperExpression> _navigationExpressionsCache = new Dictionary<INavigation, EntityShaperExpression>();

	private readonly TableExpressionBase _innerTable;

	private readonly bool _nullable;

	public virtual IEntityType EntityType { get; }

	public sealed override ExpressionType NodeType => ExpressionType.Extension;

	public override Type Type => EntityType.ClrType;

	public EntityProjectionExpression(IEntityType entityType, TableExpressionBase innerTable, bool nullable)
	{
		EntityType = entityType;
		_innerTable = innerTable;
		_nullable = nullable;
	}

	public EntityProjectionExpression(IEntityType entityType, IDictionary<IProperty, ColumnExpression> propertyExpressions)
	{
		EntityType = entityType;
		_propertyExpressionsCache = propertyExpressions;
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		if (_innerTable != null)
		{
			TableExpressionBase tableExpressionBase = (TableExpressionBase)visitor.Visit(_innerTable);
			if (tableExpressionBase == _innerTable)
			{
				return this;
			}
			return new EntityProjectionExpression(EntityType, tableExpressionBase, _nullable);
		}
		bool flag = false;
		Dictionary<IProperty, ColumnExpression> dictionary = new Dictionary<IProperty, ColumnExpression>();
		foreach (KeyValuePair<IProperty, ColumnExpression> item in _propertyExpressionsCache)
		{
			ColumnExpression columnExpression = (ColumnExpression)visitor.Visit(item.Value);
			flag |= columnExpression != item.Value;
			dictionary[item.Key] = columnExpression;
		}
		if (!flag)
		{
			return this;
		}
		return new EntityProjectionExpression(EntityType, dictionary);
	}

	public virtual EntityProjectionExpression MakeNullable()
	{
		if (_innerTable != null)
		{
			return new EntityProjectionExpression(EntityType, _innerTable, nullable: true);
		}
		Dictionary<IProperty, ColumnExpression> dictionary = new Dictionary<IProperty, ColumnExpression>();
		foreach (KeyValuePair<IProperty, ColumnExpression> item in _propertyExpressionsCache)
		{
			dictionary[item.Key] = item.Value.MakeNullable();
		}
		return new EntityProjectionExpression(EntityType, dictionary);
	}

	public virtual EntityProjectionExpression UpdateEntityType(IEntityType derivedType)
	{
		if (_innerTable != null)
		{
			return new EntityProjectionExpression(derivedType, _innerTable, _nullable);
		}
		Dictionary<IProperty, ColumnExpression> dictionary = new Dictionary<IProperty, ColumnExpression>();
		foreach (KeyValuePair<IProperty, ColumnExpression> item in _propertyExpressionsCache)
		{
			IProperty key = item.Key;
			if (derivedType.IsAssignableFrom(key.DeclaringEntityType) || key.DeclaringEntityType.IsAssignableFrom(derivedType))
			{
				dictionary[key] = item.Value;
			}
		}
		return new EntityProjectionExpression(derivedType, dictionary);
	}

	public virtual ColumnExpression BindProperty(IProperty property)
	{
		if (!EntityType.IsAssignableFrom(property.DeclaringEntityType) && !property.DeclaringEntityType.IsAssignableFrom(EntityType))
		{
			throw new InvalidOperationException("Called EntityProjectionExpression.BindProperty() with incorrect IProperty. EntityType:" + EntityType.DisplayName() + ", Property:" + property.Name);
		}
		if (!_propertyExpressionsCache.TryGetValue(property, out var value))
		{
			value = new ColumnExpression(property, _innerTable, _nullable);
			_propertyExpressionsCache[property] = value;
		}
		return value;
	}

	public virtual void AddNavigationBinding(INavigation navigation, EntityShaperExpression entityShaper)
	{
		if (!EntityType.IsAssignableFrom(navigation.DeclaringEntityType) && !navigation.DeclaringEntityType.IsAssignableFrom(EntityType))
		{
			throw new InvalidOperationException("Called EntityProjectionExpression.AddNavigationBinding() with incorrect INavigation. EntityType:" + EntityType.DisplayName() + ", Property:" + navigation.Name);
		}
		_navigationExpressionsCache[navigation] = entityShaper;
	}

	public virtual EntityShaperExpression BindNavigation(INavigation navigation)
	{
		if (!EntityType.IsAssignableFrom(navigation.DeclaringEntityType) && !navigation.DeclaringEntityType.IsAssignableFrom(EntityType))
		{
			throw new InvalidOperationException("Called EntityProjectionExpression.BindNavigation() with incorrect INavigation. EntityType:" + EntityType.DisplayName() + ", Property:" + navigation.Name);
		}
		if (!_navigationExpressionsCache.TryGetValue(navigation, out var value))
		{
			return null;
		}
		return value;
	}
}

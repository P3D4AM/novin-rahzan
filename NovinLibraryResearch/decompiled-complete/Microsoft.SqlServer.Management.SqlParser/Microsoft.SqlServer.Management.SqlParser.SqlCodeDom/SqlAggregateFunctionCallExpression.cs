using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlAggregateFunctionCallExpression : SqlBuiltinScalarFunctionCallExpression
{
	private class InvalidAggregateFunctionCallExpression : SqlAggregateFunctionCallExpression
	{
		internal override void Bind(IBindingContext bindingContext)
		{
			ReportBindError(SR.InvalidAggregateFunction(base.FunctionName), ErrorFlags.UnresolvedDatabaseObjectReference);
			base.BindingState |= BindingState.BindFailed;
		}

		public InvalidAggregateFunctionCallExpression(string name, SqlSetQuantifier setQuantifier, SqlScalarExpression expression)
			: base(name, setQuantifier, expression)
		{
		}
	}

	private static readonly List<string> validAggFunctionList = new List<string>(new string[13]
	{
		"APPROX_COUNT_DISTINCT", "AVG", "CHECKSUM_AGG", "COUNT", "COUNT_BIG", "GROUPING", "MAX", "MIN", "STDEV", "STDEVP",
		"SUM", "VAR", "VARP"
	});

	private SqlSetQuantifier setQuantifier;

	internal override bool IsVisitable => true;

	public SqlSetQuantifier SetQuantifier => setQuantifier;

	public override void Accept(ISqlCodeObjectVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override T Accept<T>(ISqlCodeObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	public override void Accept<C>(ISqlCodeObjectContextVisitor<C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this, context);
	}

	public override T Accept<T, C>(ISqlCodeObjectContextVisitor<T, C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this, context);
	}

	internal static SqlAggregateFunctionCallExpression Create(string functionName, SqlSetQuantifier setQuantifier, SqlScalarExpression expression)
	{
		if (validAggFunctionList.BinarySearch(functionName, StringComparer.OrdinalIgnoreCase) >= 0)
		{
			return new SqlAggregateFunctionCallExpression(functionName, setQuantifier, expression);
		}
		return new InvalidAggregateFunctionCallExpression(functionName, setQuantifier, expression);
	}

	internal static SqlAggregateFunctionCallExpression Create(string functionName, SqlSetQuantifier setQuantifier, IEnumerable<SqlScalarExpression> expressions)
	{
		if (validAggFunctionList.BinarySearch(functionName, StringComparer.OrdinalIgnoreCase) >= 0)
		{
			return new SqlAggregateFunctionCallExpression(functionName, expressions);
		}
		return null;
	}

	internal static SqlAggregateFunctionCallExpression Create(string functionName)
	{
		StringComparer ordinalIgnoreCase = StringComparer.OrdinalIgnoreCase;
		if (ordinalIgnoreCase.Equals(functionName, "COUNT") || ordinalIgnoreCase.Equals(functionName, "COUNT_BIG"))
		{
			return new SqlAggregateFunctionCallExpression(functionName);
		}
		return null;
	}

	private SqlAggregateFunctionCallExpression(string name, SqlSetQuantifier setQuantifier, SqlScalarExpression expression)
		: base(name, expression)
	{
		this.setQuantifier = setQuantifier;
	}

	private SqlAggregateFunctionCallExpression(string name, IEnumerable<SqlScalarExpression> expressions)
		: base(name, expressions)
	{
	}

	private SqlAggregateFunctionCallExpression(string name)
		: base(name, isStar: true)
	{
	}
}

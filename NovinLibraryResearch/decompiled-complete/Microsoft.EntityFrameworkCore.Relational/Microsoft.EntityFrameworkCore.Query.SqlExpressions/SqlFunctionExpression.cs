using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public class SqlFunctionExpression : SqlExpression
{
	public virtual string Name { get; }

	public virtual string Schema { get; }

	public virtual bool IsNiladic { get; }

	public virtual bool IsBuiltIn { get; }

	public virtual IReadOnlyList<SqlExpression> Arguments { get; }

	public virtual Expression Instance { get; }

	public static SqlFunctionExpression CreateNiladic(string name, Type type, RelationalTypeMapping typeMapping)
	{
		return new SqlFunctionExpression(null, null, name, niladic: true, null, builtIn: true, type, typeMapping);
	}

	public static SqlFunctionExpression CreateNiladic(string schema, string name, Type type, RelationalTypeMapping typeMapping)
	{
		return new SqlFunctionExpression(null, schema, name, niladic: true, null, builtIn: true, type, typeMapping);
	}

	public static SqlFunctionExpression CreateNiladic(SqlExpression instance, string name, Type type, RelationalTypeMapping typeMapping)
	{
		return new SqlFunctionExpression(instance, null, name, niladic: true, null, builtIn: true, type, typeMapping);
	}

	public static SqlFunctionExpression Create(SqlExpression instance, string name, IEnumerable<SqlExpression> arguments, Type type, RelationalTypeMapping typeMapping)
	{
		return new SqlFunctionExpression(instance, null, name, niladic: false, arguments, builtIn: true, type, typeMapping);
	}

	public static SqlFunctionExpression Create(string name, IEnumerable<SqlExpression> arguments, Type type, RelationalTypeMapping typeMapping)
	{
		return new SqlFunctionExpression(null, null, name, niladic: false, arguments, builtIn: true, type, typeMapping);
	}

	public static SqlFunctionExpression Create(string schema, string name, IEnumerable<SqlExpression> arguments, Type type, RelationalTypeMapping typeMapping)
	{
		return new SqlFunctionExpression(null, schema, name, niladic: false, arguments, builtIn: false, type, typeMapping);
	}

	public SqlFunctionExpression(Expression instance, string schema, string name, bool niladic, IEnumerable<SqlExpression> arguments, bool builtIn, Type type, RelationalTypeMapping typeMapping)
		: base(type, typeMapping)
	{
		Instance = instance;
		Name = name;
		Schema = schema;
		IsNiladic = niladic;
		IsBuiltIn = builtIn;
		Arguments = (arguments ?? Array.Empty<SqlExpression>()).ToList();
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		bool flag = false;
		SqlExpression sqlExpression = (SqlExpression)visitor.Visit(Instance);
		flag |= sqlExpression != Instance;
		SqlExpression[] array = new SqlExpression[Arguments.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (SqlExpression)visitor.Visit(Arguments[i]);
			flag |= array[i] != Arguments[i];
		}
		if (!flag)
		{
			return this;
		}
		return new SqlFunctionExpression(sqlExpression, Schema, Name, IsNiladic, array, IsBuiltIn, Type, TypeMapping);
	}

	public virtual SqlFunctionExpression ApplyTypeMapping(RelationalTypeMapping typeMapping)
	{
		return new SqlFunctionExpression(Instance, Schema, Name, IsNiladic, Arguments, IsBuiltIn, Type, typeMapping ?? TypeMapping);
	}

	public virtual SqlFunctionExpression Update(SqlExpression instance, IReadOnlyList<SqlExpression> arguments)
	{
		if (instance == Instance && arguments.SequenceEqual(Arguments))
		{
			return this;
		}
		return new SqlFunctionExpression(instance, Schema, Name, IsNiladic, arguments, IsBuiltIn, Type, TypeMapping);
	}

	public override void Print(ExpressionPrinter expressionPrinter)
	{
		if (!string.IsNullOrEmpty(Schema))
		{
			expressionPrinter.Append(Schema).Append(".").Append(Name);
		}
		else
		{
			if (Instance != null)
			{
				expressionPrinter.Visit(Instance);
				expressionPrinter.Append(".");
			}
			expressionPrinter.Append(Name);
		}
		if (!IsNiladic)
		{
			expressionPrinter.Append("(");
			expressionPrinter.VisitList(Arguments);
			expressionPrinter.Append(")");
		}
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj is SqlFunctionExpression sqlFunctionExpression)
				{
					return Equals(sqlFunctionExpression);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(SqlFunctionExpression sqlFunctionExpression)
	{
		if (base.Equals((object)sqlFunctionExpression) && string.Equals(Name, sqlFunctionExpression.Name) && string.Equals(Schema, sqlFunctionExpression.Schema) && ((Instance == null && sqlFunctionExpression.Instance == null) || (Instance != null && Instance.Equals(sqlFunctionExpression.Instance))))
		{
			return Arguments.SequenceEqual(sqlFunctionExpression.Arguments);
		}
		return false;
	}

	public override int GetHashCode()
	{
		HashCode hashCode = default(HashCode);
		hashCode.Add(base.GetHashCode());
		hashCode.Add(Name);
		hashCode.Add(IsNiladic);
		hashCode.Add(Schema);
		hashCode.Add(Instance);
		for (int i = 0; i < Arguments.Count; i++)
		{
			hashCode.Add(Arguments[i]);
		}
		return hashCode.ToHashCode();
	}
}

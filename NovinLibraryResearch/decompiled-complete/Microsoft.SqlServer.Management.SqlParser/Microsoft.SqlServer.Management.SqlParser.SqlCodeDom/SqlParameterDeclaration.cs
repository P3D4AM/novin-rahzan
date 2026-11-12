using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlParameterDeclaration : SqlVariableDeclaration
{
	private readonly SqlParameterOption option;

	private SqlParameterOptionType optionTypes;

	internal override bool IsVisitable => true;

	internal SqlParameterOption Option => option;

	public bool IsOutput => (optionTypes & SqlParameterOptionType.Output) == SqlParameterOptionType.Output;

	public bool IsReadOnly => (optionTypes & SqlParameterOptionType.ReadOnly) == SqlParameterOptionType.ReadOnly;

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

	internal override IScalarVariable CreateScalarVariable(IScalarDataType dataType, IMetadataFactory factory)
	{
		if (IsReadOnly)
		{
			ReportBindError(SR.ParameterCannotBeReadOnly(base.Name));
		}
		string defaultValue = ((base.Value != null) ? base.Value.Sql : null);
		return factory.Parameter.CreateScalarParameter(base.Name, dataType, IsOutput, defaultValue);
	}

	internal override ITableVariable CreateTableVariable(ITableDataType dataType, IMetadataFactory factory)
	{
		if (!IsReadOnly)
		{
			ReportBindError(SR.TableValuedParameterMustBeReadOnly(base.Name));
		}
		return factory.Parameter.CreateTableParameter(base.Name, dataType);
	}

	internal override ICursorVariable CreateCursorVariable(IMetadataFactory factory)
	{
		if (Statement is SqlVariableDeclareStatement)
		{
			return base.CreateCursorVariable(factory);
		}
		if (IsReadOnly)
		{
			ReportBindError(SR.ParameterCannotBeReadOnly(base.Name));
		}
		return factory.Parameter.CreateCursorParameter(base.Name);
	}

	internal SqlParameterDeclaration(string name, SqlDataTypeSpecification type, SqlScalarExpression defaultValue, SqlParameterOption option)
		: base(name, type, defaultValue)
	{
		this.option = option;
		optionTypes = option?.Type ?? SqlParameterOptionType.None;
	}

	internal void Validate(SqlParameterOptionType validOptions)
	{
		optionTypes = SqlParameterOptionType.None;
		if (option != null)
		{
			if ((option.Type == SqlParameterOptionType.Invalid) | ((validOptions | option.Type) != validOptions))
			{
				option.ReportParseError(SR.OptionNotRecognized(option.ToString()));
			}
			else
			{
				optionTypes |= option.Type;
			}
		}
	}

	internal static void Validate(IEnumerable<SqlParameterDeclaration> paramDecls, SqlParameterOptionType validOptions)
	{
		foreach (SqlParameterDeclaration paramDecl in paramDecls)
		{
			paramDecl.Validate(validOptions);
		}
	}
}

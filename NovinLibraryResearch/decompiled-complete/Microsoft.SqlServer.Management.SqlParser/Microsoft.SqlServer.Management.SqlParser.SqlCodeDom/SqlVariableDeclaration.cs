using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlVariableDeclaration : SqlCodeObject
{
	private LexicalScope lexicalScope;

	private ILocalVariable boundVariable;

	private readonly string name;

	private readonly SqlDataTypeSpecification type;

	private readonly SqlScalarExpression value;

	internal override bool IsVisitable => true;

	public ILocalVariable BoundVariable => boundVariable;

	public override IMetadataObject BoundObject => boundVariable;

	public string Name => name;

	public SqlDataTypeSpecification Type => type;

	public SqlScalarExpression Value => value;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return type;
			if (value != null)
			{
				yield return value;
			}
		}
	}

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

	internal sealed override LexicalScope GetLexicalScope()
	{
		return lexicalScope;
	}

	internal sealed override void Bind(IBindingContext bindingContext)
	{
		Bind(bindingContext, LexicalScopeOverride.None);
	}

	internal void Bind(IBindingContext bindingContext, LexicalScopeOverride scopeOverride)
	{
		LexicalScope lexicalScope = base.Parent.GetLexicalScope();
		LexicalScope lexicalScope2;
		if (scopeOverride == LexicalScopeOverride.PartitionScope)
		{
			this.lexicalScope = lexicalScope.CreateChildScope();
			lexicalScope2 = this.lexicalScope;
		}
		else
		{
			this.lexicalScope = lexicalScope;
			lexicalScope2 = ((scopeOverride != LexicalScopeOverride.UseRootScope) ? this.lexicalScope : this.lexicalScope.Root);
		}
		IMetadataFactory metadataFactory = bindingContext.MetadataFactory;
		type.Bind(bindingContext);
		IDataType dataType = type.BoundType;
		if (dataType == null)
		{
			type.ReportInvalidTypeBindError(Name, isColumn: false);
			dataType = metadataFactory.DataType.UnknownScalar;
		}
		base.BindingState |= type.BindingState;
		if (value != null)
		{
			value.Bind(bindingContext);
			base.BindingState |= value.BindingState;
		}
		if (dataType.IsScalar)
		{
			IScalarDataType dataType2 = BindUtils.DataType.AsScalarDataType(dataType);
			IScalarVariable variable = CreateScalarVariable(dataType2, metadataFactory);
			if (bindingContext.Batch.AddLocalScalarVariable(variable, lexicalScope2))
			{
				boundVariable = variable;
				base.BindingState |= BindingState.BindSucceeded;
			}
			else
			{
				ReportBindError(SR.VariableNameNotUnique(Name));
				base.BindingState |= BindingState.BindFailed;
			}
		}
		else if (dataType.IsTable)
		{
			ITableDataType dataType3 = BindUtils.DataType.AsTableDataType(dataType);
			ITableVariable variable2 = CreateTableVariable(dataType3, metadataFactory);
			if (bindingContext.Batch.AddLocalTableVariable(variable2, lexicalScope2))
			{
				boundVariable = variable2;
				base.BindingState |= BindingState.BindSucceeded;
			}
			else
			{
				ReportBindError(SR.VariableNameNotUnique(Name));
				base.BindingState |= BindingState.BindFailed;
			}
		}
		else
		{
			ICursorVariable variable3 = CreateCursorVariable(metadataFactory);
			if (bindingContext.Batch.AddLocalCursorVariable(variable3, lexicalScope2))
			{
				boundVariable = variable3;
				base.BindingState |= BindingState.BindSucceeded;
			}
			else
			{
				ReportBindError(SR.VariableNameNotUnique(Name));
				base.BindingState |= BindingState.BindFailed;
			}
		}
	}

	internal override void Unbind()
	{
		boundVariable = null;
		base.Unbind();
		lexicalScope = null;
	}

	internal virtual IScalarVariable CreateScalarVariable(IScalarDataType dataType, IMetadataFactory factory)
	{
		return factory.Variable.CreateScalarVariable(Name, dataType);
	}

	internal virtual ITableVariable CreateTableVariable(ITableDataType dataType, IMetadataFactory factory)
	{
		return factory.Variable.CreateTableVariable(Name, dataType);
	}

	internal virtual ICursorVariable CreateCursorVariable(IMetadataFactory factory)
	{
		return factory.Variable.CreateCursorVariable(Name);
	}

	internal SqlVariableDeclaration(string name, SqlDataTypeSpecification type, SqlScalarExpression value)
	{
		this.name = name;
		this.type = type;
		this.value = value;
	}

	internal static SqlVariableDeclaration Create(SqlParameterDeclaration paramDecl)
	{
		if (paramDecl.Option != null)
		{
			paramDecl.Option.ReportParseError(SR.OptionNotRecognized(paramDecl.Option.ToString()));
		}
		return new SqlVariableDeclaration(paramDecl.Name, paramDecl.Type, paramDecl.Value);
	}
}

using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlInlineTableVariableDeclaration : SqlCodeObject
{
	private ITableVariable boundTable;

	private readonly string name;

	private readonly SqlTableDefinition definition;

	internal override bool IsVisitable => true;

	public ITableVariable BoundTable => boundTable;

	public override IMetadataObject BoundObject => boundTable;

	public string Name => name;

	public SqlTableDefinition Definition => definition;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return definition;
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

	internal override void Bind(IBindingContext bindingContext)
	{
		IMetadataFactory metadataFactory = bindingContext.MetadataFactory;
		CollationInfo serverCollation = bindingContext.ServerCollation;
		IMutableTableDataType dataType = metadataFactory.DataType.CreateTableDataType(Name, serverCollation);
		definition.Bind(bindingContext.Batch, dataType);
		base.BindingState = definition.BindingState;
		boundTable = metadataFactory.Variable.CreateTableVariable(Name, dataType);
	}

	internal override void Unbind()
	{
		boundTable = null;
		base.Unbind();
	}

	internal SqlInlineTableVariableDeclaration(string name, SqlTableDefinition definition)
	{
		this.name = name;
		this.definition = definition;
	}
}

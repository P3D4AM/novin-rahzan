using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlCreateSynonymStatement : SqlDdlStatement
{
	private IMutableSynonym boundSynonym;

	private readonly SqlObjectIdentifier name;

	private readonly SqlObjectIdentifier baseObjectName;

	internal override bool IsVisitable => true;

	internal sealed override string StatementPhrase => "CREATE SYNONYM";

	internal sealed override bool IsCreateStatement => true;

	internal sealed override bool IsAlterStatement => false;

	internal sealed override bool IsDropStatement => false;

	internal sealed override DdlTargetObjectType TargetObjectType => DdlTargetObjectType.Synonym;

	public ISynonym BoundSynonym => boundSynonym;

	public override IMetadataObject BoundObject => boundSynonym;

	public SqlObjectIdentifier Name => name;

	public SqlObjectIdentifier BaseObjectName => baseObjectName;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return name;
			yield return baseObjectName;
		}
	}

	public override void Accept(ISqlStatementVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override T Accept<T>(ISqlStatementVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	public override void Accept<C>(ISqlStatementContextVisitor<C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this, context);
	}

	public override T Accept<T, C>(ISqlStatementContextVisitor<T, C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this, context);
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

	internal sealed override void InitiateBindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		string value = Name.SchemaName.Value;
		string value2 = Name.ObjectName.Value;
		LexicalScope lexicalScope = GetLexicalScope();
		SchemaEx cDASchema = bindingContext.GetCDASchema(value, lexicalScope);
		boundSynonym = bindingContext.MetadataFactory.Synonym.CreateSynonym(cDASchema.ProxyObject, value2);
		if (cDASchema.IsEmptySchema)
		{
			if (value != null)
			{
				Name.SchemaName.ReportBindError(SR.SchemaNotExist(value), ErrorFlags.UnresolvedDatabaseObjectReference);
			}
			base.BindingState |= BindingState.BindFailed;
		}
		else if (BatchBindingContext.ObjectExistsInSchema(cDASchema, value2, lexicalScope))
		{
			Name.ReportBindError(SR.DatabaseObjectExist(value2));
			base.BindingState |= BindingState.BindFailed;
		}
		else
		{
			cDASchema.Synonyms.Create(boundSynonym, this, bindMode);
			base.BindingState |= BindingState.BindSucceeded;
		}
	}

	internal override void FinalizeBindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		boundSynonym.BaseObjectName = SqlObjectIdentifierToString(baseObjectName);
	}

	internal override void Unbind()
	{
		boundSynonym = null;
		base.Unbind();
	}

	private static string SqlObjectIdentifierToString(SqlObjectIdentifier objectIdentifier)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < objectIdentifier.Count; i++)
		{
			if (i > 0)
			{
				stringBuilder.Append(".");
			}
			if (!SqlIdentifier.IsNullOrEmpty(objectIdentifier[i]))
			{
				stringBuilder.Append(EscapeSequence.BracketedEscapeSequence.Escape(objectIdentifier[i].Value));
			}
		}
		return stringBuilder.ToString();
	}

	internal SqlCreateSynonymStatement(SqlObjectIdentifier name, SqlObjectIdentifier baseObjectName)
	{
		this.name = name;
		this.baseObjectName = baseObjectName;
		if (!SqlIdentifier.IsNullOrEmpty(this.name.DatabaseName))
		{
			this.name.DatabaseName.ReportParseError(SR.DbNameIsNotAllowedForCreateSynonym);
		}
	}
}

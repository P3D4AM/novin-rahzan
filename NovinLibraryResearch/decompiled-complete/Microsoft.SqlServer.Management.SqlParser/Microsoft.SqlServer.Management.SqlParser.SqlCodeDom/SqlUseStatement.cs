using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlUseStatement : SqlStatement
{
	private IDatabase boundDatabase;

	private readonly SqlIdentifier databaseName;

	internal override bool IsVisitable => true;

	internal sealed override string StatementPhrase => "USE";

	public override IMetadataObject BoundObject => boundDatabase;

	public SqlIdentifier DatabaseName => databaseName;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return databaseName;
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

	internal override void BindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		DatabaseEx databaseEx = null;
		if (!SqlIdentifier.IsNullOrEmpty(databaseName))
		{
			databaseEx = bindingContext.FindDatabase(databaseName.Value);
		}
		if (databaseEx != null)
		{
			LexicalScope lexicalScope = GetLexicalScope();
			RegisterDatabaseContextChange(new DatabaseContext(lexicalScope, databaseEx));
			boundDatabase = databaseEx.ProxyObject;
			base.BindingState = BindingState.BindSucceeded;
		}
		else
		{
			databaseName.ReportBindError(SR.CouldNotLocateEntryInSysdatabases(databaseName.ToString()), ErrorFlags.UnresolvedReference);
			base.BindingState = BindingState.BindFailed;
		}
	}

	internal override void Unbind()
	{
		boundDatabase = null;
		base.Unbind();
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref caretLocation);
		if (databaseName != sqlCodeObject)
		{
			return null;
		}
		return RetrieveCompletionListItems(CompletionContext.Databases);
	}

	internal override string GetQuickInfoText(LexLocation cursorLocation, IMetadataDisplayInfoProvider displayInfoProvider)
	{
		string result = null;
		if (FindChildContainingLocation(ref cursorLocation) != null)
		{
			result = ((boundDatabase != null) ? displayInfoProvider.GetDescription(boundDatabase) : null);
		}
		return result;
	}

	internal SqlUseStatement(SqlIdentifier databaseName)
	{
		this.databaseName = databaseName;
	}
}

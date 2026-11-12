using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Babel;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlBatch : SqlCodeObject
{
	private List<DatabaseContext> databaseContexts;

	private LexicalScope lexicalScope;

	private BatchBindingContext bindingContext;

	private readonly SqlStatementCollection statements;

	internal override bool IsVisitable => true;

	public sealed override SqlStatement Statement => null;

	internal override IBindingContext BindingContext => bindingContext;

	public SqlStatementCollection Statements => statements;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			foreach (SqlStatement statement in statements)
			{
				yield return statement;
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

	internal override LexicalScope GetLexicalScope()
	{
		return lexicalScope;
	}

	internal override void Bind(IBindingContext bindingContext)
	{
		Bind(bindingContext.Batch);
	}

	internal void Bind(BatchBindingContext bindingContext)
	{
		BindBatch(bindingContext.Batch, null);
	}

	internal void Bind(BatchBindingContext bindingContext, BindWorkList workList)
	{
		BindBatch(bindingContext.Batch, workList);
	}

	private void BindBatch(BatchBindingContext bindingContext, BindWorkList workList)
	{
		lexicalScope = CreateLexicalScope(isModule: false);
		this.bindingContext = bindingContext;
		if (databaseContexts == null)
		{
			databaseContexts = new List<DatabaseContext>();
		}
		else
		{
			databaseContexts.Clear();
		}
		if (workList != null)
		{
			foreach (SqlStatement statement in statements)
			{
				workList.AddStatement(statement);
			}
			base.BindingState = BindingState.BindSucceeded;
		}
		else
		{
			base.Bind(bindingContext);
		}
	}

	internal override void Unbind()
	{
		databaseContexts = null;
		bindingContext = null;
		base.Unbind();
		if (databaseContexts != null)
		{
			databaseContexts.Clear();
		}
		lexicalScope = null;
	}

	internal sealed override void RegisterDatabaseContextChange(DatabaseContext databaseContext)
	{
		databaseContexts.Add(databaseContext);
	}

	internal sealed override DatabaseContext GetDatabaseContext(LexicalScope lexScope)
	{
		DatabaseContext result = null;
		for (int num = databaseContexts.Count - 1; num >= 0; num--)
		{
			DatabaseContext databaseContext = databaseContexts[num];
			if (databaseContext.LexicalScope.IsVisibleTo(lexScope))
			{
				result = databaseContext;
				break;
			}
		}
		return result;
	}

	internal DatabaseContext GetFinalDatabaseContext()
	{
		int num = ((databaseContexts != null) ? databaseContexts.Count : 0);
		if (num <= 0)
		{
			return null;
		}
		return databaseContexts[num - 1];
	}

	internal override void PopulateRegions(List<Region> regions)
	{
		Location startLocation = base.StartLocation;
		Location endLocation = base.EndLocation;
		if (startLocation.LineNumber != endLocation.LineNumber && Statements.Count > 0)
		{
			startLocation = Statements[0].StartLocation;
			TokenManager tokenManager = TokenManager;
			LexLocation position = base.Position;
			if (tokenManager.GetId(position.endTokenNumber - 1) == 986 && tokenManager.GetText(position.endTokenNumber - 1).EndsWith("\n", StringComparison.Ordinal))
			{
				int num = endLocation.LineNumber - 1;
				endLocation = new Location(num, tokenManager.GetLastCaretColumnNumber(num));
			}
			if (startLocation.LineNumber != endLocation.LineNumber)
			{
				AddRegion(regions, startLocation, endLocation);
			}
		}
		foreach (SqlCodeObject child in Children)
		{
			child.PopulateRegions(regions);
		}
	}

	internal SqlBatch(List<SqlStatement> statements)
	{
		this.statements = ((statements != null) ? new SqlStatementCollection(statements) : new SqlStatementCollection());
	}

	internal SqlBatch(params SqlStatement[] statements)
	{
		this.statements = new SqlStatementCollection(statements);
	}

	internal override void WriteXml(XmlWriter writer)
	{
		writer.WriteStartElement("SqlBatch");
		if (base.Position != null)
		{
			writer.WriteAttributeString("Location", string.Format(CultureInfo.CurrentCulture, "({0}, {1})", base.StartLocation, base.EndLocation));
		}
		writer.WriteComment(base.Sql);
		foreach (SqlCodeObject child in Children)
		{
			child.WriteXml(writer);
		}
		writer.WriteStartElement("Tokens");
		foreach (Token token in base.Tokens)
		{
			token.WriteXml(writer);
		}
		writer.WriteEndElement();
		writer.WriteEndElement();
	}
}

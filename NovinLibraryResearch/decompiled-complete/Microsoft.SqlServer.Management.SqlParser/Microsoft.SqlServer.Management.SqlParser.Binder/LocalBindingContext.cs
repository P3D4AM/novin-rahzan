using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

internal class LocalBindingContext : IBindingContext
{
	protected static readonly Predicate<IMetadataObject> isNotAnonymousMetadataObjectFilter = IsNotAnonymousMetadataObject;

	private readonly BatchBindingContext m_batchBindingContext;

	private readonly LocalBindingContext m_parentBindingContext;

	private readonly CteScopeSnapshot m_cteScopeSnapshot;

	private readonly TabularCollection m_localTableExpressions;

	private readonly ColumnOrderedCollection m_localColumnExpressions;

	public LocalBindingContext Parent => m_parentBindingContext;

	public BatchBindingContext Batch => m_batchBindingContext;

	public BatchBindingContext AsBatchBindingContext => null;

	public LocalBindingContext AsLocalBindingContext => this;

	public CollationInfo DatabaseCollation => m_batchBindingContext.DatabaseCollation;

	public CollationInfo ServerCollation => m_batchBindingContext.ServerCollation;

	public IMetadataFactory MetadataFactory => m_batchBindingContext.MetadataFactory;

	public virtual IMetadataCollection<ITabular> LocalTableExpressions => m_localTableExpressions;

	public virtual IMetadataOrderedCollection<IColumn> LocalColumnExpressions => m_localColumnExpressions;

	private static bool IsNotAnonymousMetadataObject(IMetadataObject metadataObject)
	{
		return metadataObject.Name.Length > 0;
	}

	public LocalBindingContext(BatchBindingContext batchBindingContext)
		: this(batchBindingContext, null)
	{
	}

	private LocalBindingContext(BatchBindingContext batchBindingContext, LocalBindingContext localBindingContext)
	{
		m_batchBindingContext = batchBindingContext;
		m_parentBindingContext = localBindingContext;
		CollationInfo databaseCollation = batchBindingContext.DatabaseCollation;
		m_localTableExpressions = new TabularCollection(databaseCollation);
		m_localColumnExpressions = new ColumnOrderedCollection(databaseCollation);
		if (localBindingContext != null)
		{
			m_cteScopeSnapshot = localBindingContext.m_cteScopeSnapshot;
		}
		else
		{
			m_cteScopeSnapshot = batchBindingContext.CaptureCurrentCteSnapshot();
		}
	}

	protected LocalBindingContext(LocalBindingContext baseBindingContext)
	{
		m_batchBindingContext = baseBindingContext.m_batchBindingContext;
		m_parentBindingContext = baseBindingContext.m_parentBindingContext;
		m_cteScopeSnapshot = baseBindingContext.m_cteScopeSnapshot;
		m_localColumnExpressions = baseBindingContext.m_localColumnExpressions;
		m_localTableExpressions = baseBindingContext.m_localTableExpressions;
	}

	protected LocalBindingContext(LocalBindingContext baseBindingContext, TabularCollection localTables, ColumnOrderedCollection localColumns)
		: this(baseBindingContext)
	{
		m_localTableExpressions = localTables;
		m_localColumnExpressions = localColumns;
	}

	public LocalBindingContext CreateLocalBindingContext()
	{
		return new LocalBindingContext(m_batchBindingContext, this);
	}

	public ITableVariable FindLocalTableVariable(string name, LexicalScope lexicalScope)
	{
		return m_batchBindingContext.FindLocalTableVariable(name, lexicalScope);
	}

	public IScalarVariable FindLocalScalarVariable(string name, LexicalScope lexicalScope)
	{
		return m_batchBindingContext.FindLocalScalarVariable(name, lexicalScope);
	}

	public ICursorVariable FindLocalCursorVariable(string name, LexicalScope lexicalScope)
	{
		return m_batchBindingContext.FindLocalCursorVariable(name, lexicalScope);
	}

	public ILocalVariable FindLocalVariable(string name, LexicalScope lexicalScope)
	{
		return m_batchBindingContext.FindLocalVariable(name, lexicalScope);
	}

	public void OnBindingToSelfCte()
	{
		m_cteScopeSnapshot.QueryFlags |= CteQueryFlags.HasSelfReference;
		m_batchBindingContext.OnBindingToSelfCte();
	}

	public ITabular FindCommonTableExpression(string name, out CteBindMode bindMode, out CteQueryFlags queryFlags)
	{
		bindMode = CteBindMode.None;
		queryFlags = CteQueryFlags.None;
		ITabular result;
		if (m_cteScopeSnapshot != null)
		{
			if (DatabaseCollation.EqualityComparer.Equals(name, m_cteScopeSnapshot.SelfCte.Name))
			{
				result = m_cteScopeSnapshot.SelfCte;
				bindMode = m_cteScopeSnapshot.BindMode;
				queryFlags = m_cteScopeSnapshot.QueryFlags;
			}
			else
			{
				result = m_cteScopeSnapshot.InScopeCteList[name];
			}
		}
		else
		{
			result = m_batchBindingContext.FindCommonTableExpression(name, out bindMode, out queryFlags);
		}
		return result;
	}

	public virtual ITabular FindDmlTable(string name)
	{
		return null;
	}

	public virtual IColumn AddLocalColumnExpression(IColumn expression)
	{
		m_localColumnExpressions.Add(expression);
		return null;
	}

	public virtual ITabular AddLocalTableExpression(ITabular expression)
	{
		ITabular tabular = m_localTableExpressions[expression.Name];
		if (tabular == null)
		{
			m_localTableExpressions.Add(expression);
		}
		return tabular;
	}

	public IScalar FindLocalScalarExpression(string name)
	{
		return LocalColumnExpressions[name];
	}

	public virtual ITabular FindLocalTableExpression(string databaseName, string schemaName, string name)
	{
		ITabular tabular = LocalTableExpressions[name];
		if (tabular != null && schemaName != null)
		{
			ISchemaOwnedObject schemaOwnedObject = BindUtils.AsSchemaOwnedObject(tabular);
			if (schemaOwnedObject != null)
			{
				if (string.IsNullOrEmpty(databaseName))
				{
					databaseName = m_batchBindingContext.ConnectedDatabaseName;
				}
				if (schemaName != string.Empty)
				{
					ISchema schema = schemaOwnedObject.Schema;
					IDatabase database = schema.Database;
					if (!BindUtils.DatabaseObject.NameMatches(schema, schemaName) || !BindUtils.DatabaseObject.NameMatches(database, databaseName))
					{
						tabular = null;
					}
				}
				else if (tabular != m_batchBindingContext.FindTableOrView(databaseName, null, name, null))
				{
					tabular = null;
				}
			}
			else
			{
				tabular = null;
			}
		}
		if (tabular == null && m_parentBindingContext != null)
		{
			tabular = m_parentBindingContext.FindLocalTableExpression(databaseName, schemaName, name);
		}
		return tabular;
	}

	public virtual IColumn FindColumn(string name, out bool isAmbiguous)
	{
		IColumn column = null;
		isAmbiguous = false;
		LocalBindingContext localBindingContext = this;
		while (localBindingContext != null && column == null)
		{
			foreach (ITabular localTableExpression in localBindingContext.LocalTableExpressions)
			{
				if (column == null)
				{
					column = localTableExpression.Columns[name];
				}
				else if (localTableExpression.Columns.Contains(name))
				{
					column = null;
					isAmbiguous = true;
					break;
				}
			}
			localBindingContext = localBindingContext.Parent;
		}
		return column;
	}

	public virtual IEnumerable<IColumn> FindAllColumns(string name)
	{
		for (LocalBindingContext bindingContext = this; bindingContext != null; bindingContext = bindingContext.Parent)
		{
			foreach (ITabular localTableExpression in bindingContext.LocalTableExpressions)
			{
				IColumn column = localTableExpression.Columns[name];
				if (column != null)
				{
					yield return column;
				}
			}
		}
	}

	public IMetadataCollection<IMetadataObject> FindAll(CompletionContext context, LexicalScope lexicalScope)
	{
		List<IMetadataCollection<IMetadataObject>> list = new List<IMetadataCollection<IMetadataObject>>();
		bool flag = (context & CompletionContext.Columns) == CompletionContext.Columns;
		bool flag2 = (context & CompletionContext.Tables) == CompletionContext.Tables;
		if (flag || flag2)
		{
			IMetadataCollection<ITabular> localTableExpressions = LocalTableExpressions;
			if (flag2)
			{
				list.Add(localTableExpressions.AsMetadataObjectCollection);
			}
			if (flag)
			{
				foreach (ITabular item in localTableExpressions)
				{
					list.Add(item.Columns.AsMetadataObjectCollection);
				}
			}
		}
		if ((context & CompletionContext.ColumnAliases) == CompletionContext.ColumnAliases)
		{
			IMetadataOrderedCollection<IColumn> localColumnExpressions = LocalColumnExpressions;
			list.Add(Collection<IMetadataObject>.Filter(localColumnExpressions.AsMetadataObjectCollection, isNotAnonymousMetadataObjectFilter));
		}
		if (m_parentBindingContext != null)
		{
			list.Add(m_parentBindingContext.FindAll(context, lexicalScope));
		}
		else
		{
			if ((context & CompletionContext.LocalTableExpressionsOnly) == CompletionContext.LocalTableExpressionsOnly)
			{
				context &= ~(CompletionContext.TablesExpressions | CompletionContext.LocalTableExpressionsOnly);
			}
			IMetadataCollection<IMetadataObject> commonTableExpressions = ((m_cteScopeSnapshot != null) ? m_cteScopeSnapshot.GetCteCompletionList() : null);
			list.Add(m_batchBindingContext.FindAll(context, lexicalScope, commonTableExpressions));
		}
		return Collection<IMetadataObject>.Merge(list.ToArray());
	}

	public IMetadataCollection<IMetadataObject> FindAll(List<string> ids, CompletionContext context, LexicalScope lexicalScope)
	{
		if (ids == null || ids.Count == 0)
		{
			return FindAll(context, lexicalScope);
		}
		if (ids.Count == 1)
		{
			ITabular tabular = FindLocalTableExpression(null, null, ids[0]);
			if (tabular != null)
			{
				return tabular.Columns.AsMetadataObjectCollection;
			}
		}
		IMetadataCollection<IMetadataObject> commonTableExpressions = ((m_cteScopeSnapshot != null) ? m_cteScopeSnapshot.GetCteCompletionList() : null);
		if ((context & CompletionContext.LocalTableExpressionsOnly) == CompletionContext.LocalTableExpressionsOnly)
		{
			context &= ~(CompletionContext.TablesExpressions | CompletionContext.LocalTableExpressionsOnly);
		}
		return m_batchBindingContext.FindAll(ids, context, lexicalScope, commonTableExpressions);
	}

	public virtual void EnterTempBindMode(TemporaryBindMode mode)
	{
	}

	public virtual void LeaveTempBindMode(TemporaryBindMode mode)
	{
	}
}

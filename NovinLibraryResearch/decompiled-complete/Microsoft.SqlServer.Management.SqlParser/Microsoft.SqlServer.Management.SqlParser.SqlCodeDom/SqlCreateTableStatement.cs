using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlCreateTableStatement : SqlDdlStatement
{
	private IMutableTable boundTable;

	private readonly SqlObjectIdentifier name;

	private readonly SqlTableDefinition definition;

	private readonly SqlStorageSpecification storageSpecification;

	private readonly SqlLargeDataStorageInformation largeDataStorageInformation;

	internal override bool IsVisitable => true;

	internal sealed override string StatementPhrase => "CREATE TABLE";

	internal sealed override bool IsCreateStatement => true;

	internal sealed override bool IsAlterStatement => false;

	internal sealed override bool IsDropStatement => false;

	internal sealed override DdlTargetObjectType TargetObjectType => DdlTargetObjectType.Table;

	public ITabular BoundTable => boundTable;

	public override IMetadataObject BoundObject => boundTable;

	public SqlObjectIdentifier Name => name;

	public SqlTableDefinition Definition => definition;

	public SqlStorageSpecification StorageSpecification => storageSpecification;

	public SqlLargeDataStorageInformation LargeDataStorageInformation => largeDataStorageInformation;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return name;
			yield return definition;
			if (storageSpecification != null)
			{
				yield return storageSpecification;
			}
			if (largeDataStorageInformation != null)
			{
				yield return largeDataStorageInformation;
			}
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

	internal override void InitiateBindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		string value = name.DatabaseName.Value;
		string value2 = name.SchemaName.Value;
		string value3 = name.ObjectName.Value;
		bindingContext = bindingContext.Root;
		LexicalScope lexicalScope = GetLexicalScope();
		SchemaEx schemaEx;
		ISchema schema;
		if (BindUtils.Table.IsTempTableName(value3))
		{
			if (bindingContext.FindTempTable(value3, lexicalScope) != null)
			{
				if (!base.IsInConditionalStatement)
				{
					name.ReportBindError(SR.DatabaseObjectExist(value3));
				}
				schemaEx = null;
				schema = bindingContext.ConnectedDatabase.EmptySchema.ProxyObject;
			}
			else
			{
				schemaEx = bindingContext.GetTempTableSchema();
				schema = schemaEx.ProxyObject;
			}
		}
		else
		{
			ISchemaOwnedObject schemaOwnedObject = bindingContext.FindSchemaOwnedObject(value, value2, value3, lexicalScope);
			if (schemaOwnedObject != null)
			{
				if (!base.IsInConditionalStatement)
				{
					name.ReportBindError(SR.DatabaseObjectExist(value3));
				}
				schemaEx = null;
				schema = schemaOwnedObject.Schema;
			}
			else
			{
				schemaEx = bindingContext.FindSchema(value, value2 ?? string.Empty, lexicalScope);
				if (schemaEx == null)
				{
					DatabaseEx databaseEx = bindingContext.FindDatabase(value ?? string.Empty);
					if (databaseEx != null)
					{
						if (value2 != null)
						{
							name.SchemaName.ReportBindError(SR.SchemaNotExist(value2), ErrorFlags.UnresolvedDatabaseObjectReference);
						}
						schema = databaseEx.EmptySchema.ProxyObject;
					}
					else
					{
						name.DatabaseName.ReportBindError(SR.DatabaseNotExist(value), ErrorFlags.UnresolvedDatabaseObjectReference);
						schema = bindingContext.ConnectedDatabase.EmptySchema.ProxyObject;
					}
					base.BindingState |= BindingState.BindFailed;
				}
				else
				{
					schemaEx = schemaEx.AsSingleSchema;
					schema = schemaEx.ProxyObject;
				}
			}
		}
		boundTable = bindingContext.MetadataFactory.Tabular.CreateTable(schema, value3);
		if (!SchemaEx.IsNullOrEmpty(schemaEx))
		{
			schemaEx.Tables.Create(boundTable, this, bindMode);
			base.BindingState |= BindingState.BindSucceeded;
		}
		else
		{
			base.BindingState |= BindingState.BindFailed;
		}
	}

	internal override void FinalizeBindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		definition.Bind(bindingContext, boundTable);
		base.BindingState = definition.BindingState;
	}

	internal override void Unbind()
	{
		boundTable = null;
		base.Unbind();
	}

	internal SqlCreateTableStatement(SqlObjectIdentifier name, SqlTableDefinition definition, SqlStorageSpecification storageSpecification, SqlLargeDataStorageInformation largeDataStorageInformation)
	{
		this.name = name;
		this.definition = definition;
		this.storageSpecification = storageSpecification;
		this.largeDataStorageInformation = largeDataStorageInformation;
	}
}

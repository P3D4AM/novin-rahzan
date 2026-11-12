using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class TableValuedFunction : UserDefinedFunction, IMutableTableValuedFunction, ITableValuedFunction, IDatabaseTable, ITabular, IMetadataObject, IUserDefinedFunction, IFunction, IFunctionModuleBase, IUserDefinedFunctionModuleBase, ISchemaOwnedObject, IDatabaseObject, IMutableDatabaseTable, IMutableTabular, IMutableMetadataObject, IMutableUserDefinedFunction, IMutableFunction, IMutableFunctionModuleBase, IMutableUserDefinedFunctionModuleBase, IMutableSchemaOwnedObject, IMutableDatabaseObject
{
	private readonly ColumnOrderedCollection m_columns;

	private readonly ConstraintCollection m_constraints;

	private readonly IndexCollection m_indexes;

	private readonly StatisticsCollection m_statistics;

	private bool m_isInline;

	private string m_tableVariableName;

	public bool IsInline
	{
		get
		{
			return m_isInline;
		}
		set
		{
			m_isInline = value;
		}
	}

	public string TableVariableName
	{
		get
		{
			return m_tableVariableName;
		}
		set
		{
			m_tableVariableName = value;
		}
	}

	IMetadataCollection<IConstraint> IDatabaseTable.Constraints => m_constraints;

	IMetadataCollection<IIndex> IDatabaseTable.Indexes => m_indexes;

	IMetadataCollection<IStatistics> IDatabaseTable.Statistics => m_statistics;

	public IMutableMetadataOrderedCollection<IColumn> Columns => m_columns;

	public TabularType TabularType => TabularType.TableValuedFunction;

	IMetadataOrderedCollection<IColumn> ITabular.Columns => m_columns;

	public ITabular Unaliased => this;

	public IMutableMetadataCollection<IConstraint> Constraints => m_constraints;

	public IMutableMetadataCollection<IIndex> Indexes => m_indexes;

	public IMutableMetadataCollection<IStatistics> Statistics => m_statistics;

	public TableValuedFunction(ISchema schema, string name)
		: base(schema, name)
	{
		CollationInfo collationInfo = schema.Database.CollationInfo;
		m_columns = new ColumnOrderedCollection(collationInfo);
		m_constraints = new ConstraintCollection(collationInfo);
		m_indexes = new IndexCollection(collationInfo);
		m_statistics = new StatisticsCollection(collationInfo);
	}

	public override T Accept<T>(ISchemaOwnedObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	public static TableValuedFunction CreateBuiltIn(ISchema schema, string name, params ScalarParameter[] parameters)
	{
		TableValuedFunction tableValuedFunction = new TableValuedFunction(schema, name);
		foreach (ScalarParameter scalarParameter in parameters)
		{
			if (scalarParameter.IsOutput)
			{
				tableValuedFunction.Columns.Add(ObjectFactoryBase<ColumnFactory>.Instance.CreateSimpleColumn(tableValuedFunction, scalarParameter.Name, scalarParameter.DataType, nullable: true));
			}
			else
			{
				tableValuedFunction.Parameters.Add(scalarParameter);
			}
		}
		return tableValuedFunction;
	}

	public static void PopulateBuiltInCollection(ISchema schema, TableValuedFunctionCollection collection)
	{
		TableValuedFunction[] collection2 = new TableValuedFunction[1] { CreateBuiltIn(schema, "string_split", new ScalarParameter("value", SystemDataTypeLookup.Instance.IgnoreArgs.VarChar, isOutput: true, null), new ScalarParameter("ordinal", SystemDataTypeLookup.Instance.BigInt, isOutput: true, null), new ScalarParameter("input string", SystemDataTypeLookup.Instance.VarCharMax), new ScalarParameter("separator", SystemDataTypeLookup.Instance.VarChar(1)), new ScalarParameter("enable_ordinal", SystemDataTypeLookup.Instance.Int)) };
		collection.AddRange(collection2);
	}
}

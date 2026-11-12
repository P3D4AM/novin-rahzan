using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlUniqueConstraintBase : SqlConstraint, ISqlTableElement
{
	private readonly SqlClusterOption clusterOption;

	private readonly SqlIndexedColumnCollection indexedColumns;

	private readonly SqlIndexOptionCollection indexOptions;

	public SqlClusterOption ClusterOption => clusterOption;

	public SqlIndexedColumnCollection IndexedColumns => indexedColumns;

	public SqlIndexOptionCollection IndexOptions => indexOptions;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (base.Name != null)
			{
				yield return base.Name;
			}
			if (indexedColumns != null)
			{
				foreach (SqlIndexedColumn indexedColumn in indexedColumns)
				{
					yield return indexedColumn;
				}
			}
			if (indexOptions == null)
			{
				yield break;
			}
			foreach (SqlIndexOption indexOption in indexOptions)
			{
				yield return indexOption;
			}
		}
	}

	bool ISqlTableElement.IsConstraint => true;

	SqlConstraint ISqlTableElement.AsConstraint => this;

	bool ISqlTableElement.IsColumnDefinition => false;

	SqlColumnDefinition ISqlTableElement.AsColumnDefinition => null;

	SqlTemporalPeriodDefinition ISqlTableElement.AsTemporalPeriodDefinition => null;

	bool ISqlTableElement.IsTemporalPeriodDefinition => false;

	internal sealed override void Bind(BatchBindingContext bindingContext, ConstraintBindingHelper bindingHelper, IMutableDatabaseTable boundTable)
	{
		if (indexedColumns == null)
		{
			ReportBindError(SR.TableConstraintHasNoColumnList(boundTable.Name));
			base.BindingState |= BindingState.BindFailed;
		}
		else
		{
			Bind(bindingContext, bindingHelper, boundTable, null);
		}
	}

	internal sealed override void Bind(BatchBindingContext bindingContext, ConstraintBindingHelper bindingHelper, IMutableDatabaseTable boundTable, IMutableColumn boundColumn)
	{
		bool flag = false;
		if (base.Type == SqlConstraintType.PrimaryKey)
		{
			foreach (IConstraint constraint in boundTable.Constraints)
			{
				if (constraint.Type == ConstraintType.PrimaryKey)
				{
					ReportBindError(SR.MultiplePrimaryKey(boundTable.Name));
					base.BindingState |= BindingState.BindFailed;
					flag = true;
					break;
				}
			}
		}
		IMetadataFactory metadataFactory = bindingContext.MetadataFactory;
		string text;
		bool isSystemNamed;
		if (SqlIdentifier.IsNullOrEmpty(base.Name))
		{
			List<string> list = new List<string>();
			if (indexedColumns == null)
			{
				list.Add((boundColumn != null) ? boundColumn.Name : boundTable.Name);
			}
			else
			{
				foreach (SqlIndexedColumn indexedColumn in IndexedColumns)
				{
					list.Add(indexedColumn.Name.Value);
				}
			}
			text = ((base.Type == SqlConstraintType.PrimaryKey) ? BindUtils.Constraint.GeneratePrimaryKeyName(boundTable, list) : BindUtils.Constraint.GenerateUniqueName(boundTable, list));
			isSystemNamed = true;
		}
		else
		{
			text = base.Name.Value;
			isSystemNamed = false;
		}
		IMutableRelationalIndex mutableRelationalIndex = metadataFactory.Index.CreateRelationalIndex(boundTable, text, boundTable.CollationInfo);
		mutableRelationalIndex.IsUnique = true;
		if (base.Type == SqlConstraintType.PrimaryKey && clusterOption == SqlClusterOption.Default)
		{
			mutableRelationalIndex.IsClustered = true;
		}
		else
		{
			mutableRelationalIndex.IsClustered = ClusterOption == SqlClusterOption.Clustered;
		}
		mutableRelationalIndex.IsSystemNamed = isSystemNamed;
		if (!flag)
		{
			boundTable.Indexes.Add(mutableRelationalIndex);
		}
		switch (base.Type)
		{
		case SqlConstraintType.PrimaryKey:
			mutableRelationalIndex.IndexKey = metadataFactory.Constraint.CreatePrimaryKeyConstraint(boundTable, mutableRelationalIndex);
			break;
		case SqlConstraintType.Unique:
			mutableRelationalIndex.IndexKey = metadataFactory.Constraint.CreateUniqueConstraint(boundTable, mutableRelationalIndex);
			break;
		}
		if (!flag)
		{
			boundTable.Constraints.Add(mutableRelationalIndex.IndexKey);
		}
		if (indexedColumns != null)
		{
			foreach (SqlIndexedColumn indexedColumn2 in indexedColumns)
			{
				indexedColumn2.Bind(bindingContext, boundTable, mutableRelationalIndex, base.Type);
				base.BindingState |= indexedColumn2.BindingState;
			}
		}
		else
		{
			IIndexedColumn item = metadataFactory.Index.CreateIndexedColumn(boundColumn);
			mutableRelationalIndex.IndexedColumns.Add(item);
			base.BindingState |= BindingState.BindSucceeded;
		}
		if (base.Type == SqlConstraintType.PrimaryKey)
		{
			foreach (IIndexedColumn indexedColumn3 in mutableRelationalIndex.IndexedColumns)
			{
				if (bindingHelper.NullableColumns.TryGetValue(indexedColumn3.ReferencedColumn, out var value))
				{
					if (value)
					{
						ReportBindError(SR.CannotDefinePrimaryKeyOnNullable(indexedColumn3.Name));
						base.BindingState |= BindingState.BindFailed;
					}
				}
				else
				{
					bindingHelper.NullableColumns[indexedColumn3.ReferencedColumn] = false;
				}
			}
		}
		if (indexOptions != null)
		{
			SqlCreateIndexStatement.BindIndexOptions(indexOptions, mutableRelationalIndex, out var _);
		}
	}

	internal static SqlUniqueConstraintBase Create(SqlConstraintType type, SqlClusterOption clusterOption, List<SqlIndexedColumn> indexedColumnList, List<SqlIndexOption> indexOptions)
	{
		SqlUniqueConstraintBase result = null;
		switch (type)
		{
		case SqlConstraintType.PrimaryKey:
			result = new SqlPrimaryKeyConstraint(clusterOption, indexedColumnList, indexOptions);
			break;
		case SqlConstraintType.Unique:
			result = new SqlUniqueConstraint(clusterOption, indexedColumnList, indexOptions);
			break;
		}
		return result;
	}

	internal SqlUniqueConstraintBase(SqlConstraintType type, SqlClusterOption clusterOption, List<SqlIndexedColumn> indexedColumnList, List<SqlIndexOption> indexOptions)
		: base(type)
	{
		this.clusterOption = clusterOption;
		indexedColumns = ((indexedColumnList != null) ? new SqlIndexedColumnCollection(indexedColumnList) : null);
		if (indexOptions == null)
		{
			return;
		}
		this.indexOptions = new SqlIndexOptionCollection(indexOptions);
		foreach (SqlIndexOption indexOption in this.indexOptions)
		{
			switch (indexOption.Type)
			{
			case SqlIndexOptionType.DropExisting:
			case SqlIndexOptionType.MaxDegreeOfParallelism:
			case SqlIndexOptionType.SortInTempDb:
			case SqlIndexOptionType.StatisticsOnly:
				indexOption.ReportParseError(SR.UnrecognizedCreateTableOption(indexOption.Phrase));
				break;
			}
		}
	}
}

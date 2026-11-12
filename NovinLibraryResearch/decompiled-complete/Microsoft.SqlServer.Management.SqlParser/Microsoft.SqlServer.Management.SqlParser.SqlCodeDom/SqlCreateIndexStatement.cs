using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlCreateIndexStatement : SqlDdlStatement
{
	private sealed class IRelationalIndexProxy : IMutableRelationalIndex, IRelationalIndex, IIndex, IMetadataObject, IMutableIndex, IMutableMetadataObject, IProxyMetadataObject
	{
		private readonly IMutableRelationalIndex index;

		private IBindFinalizer finalizer;

		public IMutableRelationalIndex Index => index;

		public bool CompactLargeObjects
		{
			get
			{
				FinalizeBind();
				return index.CompactLargeObjects;
			}
			set
			{
				FinalizeBind();
				index.CompactLargeObjects = value;
			}
		}

		public bool DisallowPageLocks
		{
			get
			{
				FinalizeBind();
				return index.DisallowPageLocks;
			}
			set
			{
				FinalizeBind();
				index.DisallowPageLocks = value;
			}
		}

		public bool DisallowRowLocks
		{
			get
			{
				FinalizeBind();
				return index.DisallowRowLocks;
			}
			set
			{
				FinalizeBind();
				index.DisallowRowLocks = value;
			}
		}

		public IFileGroup FileGroup
		{
			get
			{
				FinalizeBind();
				return index.FileGroup;
			}
			set
			{
				FinalizeBind();
				index.FileGroup = value;
			}
		}

		public IFileGroup FileStreamFileGroup
		{
			get
			{
				FinalizeBind();
				return index.FileStreamFileGroup;
			}
			set
			{
				FinalizeBind();
				index.FileStreamFileGroup = value;
			}
		}

		public IPartitionScheme FileStreamPartitionScheme
		{
			get
			{
				FinalizeBind();
				return index.FileStreamPartitionScheme;
			}
			set
			{
				FinalizeBind();
				index.FileStreamPartitionScheme = value;
			}
		}

		public byte FillFactor
		{
			get
			{
				FinalizeBind();
				return index.FillFactor;
			}
			set
			{
				FinalizeBind();
				index.FillFactor = value;
			}
		}

		public string FilterDefinition
		{
			get
			{
				FinalizeBind();
				return index.FilterDefinition;
			}
			set
			{
				FinalizeBind();
				index.FilterDefinition = value;
			}
		}

		public bool IgnoreDuplicateKeys
		{
			get
			{
				FinalizeBind();
				return index.IgnoreDuplicateKeys;
			}
			set
			{
				FinalizeBind();
				index.IgnoreDuplicateKeys = value;
			}
		}

		public IMutableMetadataOrderedCollection<IIndexedColumn> IndexedColumns
		{
			get
			{
				FinalizeBind();
				return index.IndexedColumns;
			}
		}

		public IUniqueConstraintBase IndexKey
		{
			get
			{
				FinalizeBind();
				return index.IndexKey;
			}
			set
			{
				FinalizeBind();
				index.IndexKey = value;
			}
		}

		public bool IsClustered
		{
			get
			{
				return index.IsClustered;
			}
			set
			{
				index.IsClustered = value;
			}
		}

		public bool IsDisabled
		{
			get
			{
				FinalizeBind();
				return index.IsDisabled;
			}
			set
			{
				FinalizeBind();
				index.IsDisabled = value;
			}
		}

		public bool IsSystemNamed
		{
			get
			{
				FinalizeBind();
				return index.IsSystemNamed;
			}
			set
			{
				FinalizeBind();
				index.IsSystemNamed = value;
			}
		}

		public bool IsUnique
		{
			get
			{
				return index.IsUnique;
			}
			set
			{
				index.IsUnique = value;
			}
		}

		public bool NoAutomaticRecomputation
		{
			get
			{
				FinalizeBind();
				return index.NoAutomaticRecomputation;
			}
			set
			{
				FinalizeBind();
				index.NoAutomaticRecomputation = value;
			}
		}

		public bool PadIndex
		{
			get
			{
				FinalizeBind();
				return index.PadIndex;
			}
			set
			{
				FinalizeBind();
				index.PadIndex = value;
			}
		}

		public IPartitionScheme PartitionScheme
		{
			get
			{
				FinalizeBind();
				return index.PartitionScheme;
			}
			set
			{
				FinalizeBind();
				index.PartitionScheme = value;
			}
		}

		IMetadataOrderedCollection<IIndexedColumn> IRelationalIndex.IndexedColumns
		{
			get
			{
				FinalizeBind();
				return index.IndexedColumns;
			}
		}

		public string Name => index.Name;

		public IMetadataObject UnderlyingObject => index;

		public ITabular Parent => index.Parent;

		public IndexType Type => index.Type;

		public IRelationalIndexProxy(IMutableRelationalIndex index, IBindFinalizer finalizer)
		{
			this.index = index;
			this.finalizer = finalizer;
		}

		private void FinalizeBind()
		{
			if (finalizer != null)
			{
				IBindFinalizer bindFinalizer = finalizer;
				finalizer = null;
				bindFinalizer.FinalizeBind();
			}
		}

		public T Accept<T>(IMetadataObjectVisitor<T> visitor)
		{
			FinalizeBind();
			return index.Accept(visitor);
		}
	}

	private const int MaxKeyColumnCount = 16;

	private IMutableRelationalIndex boundIndex;

	private ITableViewBase boundTable;

	private bool executionSuccedded;

	private IMutableTableViewBase alteredTable;

	private IRelationalIndexProxy proxyIndex;

	private readonly bool isUnique;

	private readonly SqlClusterOption clusterOption;

	private readonly SqlIdentifier name;

	private readonly SqlObjectIdentifier targetObject;

	private readonly SqlIndexedColumnCollection indexedColumns;

	private readonly SqlIdentifierCollection includedColumns;

	private readonly SqlFilterClause filterClause;

	private readonly SqlIndexOptionCollection options;

	private readonly SqlStorageSpecification storageSpecification;

	private readonly SqlStorageSpecification fileStreamStorageSpecification;

	private readonly bool isValid = true;

	private readonly bool hasDropExistingOption;

	private static IEnumerable<SqlIndexedColumn> EmptySqlIndexedColumn = Enumerable.Empty<SqlIndexedColumn>();

	private static IEnumerable<SqlIdentifier> EmptySqlIdentifier = Enumerable.Empty<SqlIdentifier>();

	internal override bool IsVisitable => true;

	internal sealed override string StatementPhrase => "CREATE INDEX";

	internal sealed override bool IsCreateStatement => true;

	internal sealed override bool IsAlterStatement => false;

	internal sealed override bool IsDropStatement => false;

	internal sealed override DdlTargetObjectType TargetObjectType => DdlTargetObjectType.Index;

	public IRelationalIndex BoundIndex => boundIndex;

	public override IMetadataObject BoundObject => BoundIndex;

	internal bool HasDropExistingOption => hasDropExistingOption;

	public bool IsUnique => isUnique;

	public SqlClusterOption ClusterOption => clusterOption;

	public SqlIdentifier Name => name;

	public SqlObjectIdentifier TargetObject => targetObject;

	public SqlIndexedColumnCollection IndexedColunms => indexedColumns;

	public SqlIdentifierCollection IncludedColumns => includedColumns;

	public SqlFilterClause FilterClause => filterClause;

	public SqlIndexOptionCollection Options => options;

	public SqlStorageSpecification StorageSpecification => storageSpecification;

	public SqlStorageSpecification FileStream => fileStreamStorageSpecification;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return name;
			if (targetObject != null)
			{
				yield return targetObject;
			}
			foreach (SqlIndexedColumn indexedColumn in indexedColumns)
			{
				yield return indexedColumn;
			}
			if (includedColumns != null)
			{
				foreach (SqlIdentifier includedColumn in includedColumns)
				{
					yield return includedColumn;
				}
			}
			if (filterClause != null)
			{
				yield return filterClause;
			}
			if (options != null)
			{
				foreach (SqlIndexOption option in options)
				{
					yield return option;
				}
			}
			if (storageSpecification != null)
			{
				yield return storageSpecification;
			}
			if (fileStreamStorageSpecification != null)
			{
				yield return fileStreamStorageSpecification;
			}
		}
	}

	internal bool IsValid => isValid;

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

	private static IMutableTableViewBase GetAlterable(SchemaEx schema, ITableViewBase tableOrView, LexicalScope lexicalScope, StatementBindMode bindMode)
	{
		ITable immutableObject;
		if ((immutableObject = BindUtils.Table.AsTable(tableOrView)) != null)
		{
			return schema.Tables.GetAlterableObject(immutableObject, lexicalScope, bindMode);
		}
		IView immutableObject2;
		if ((immutableObject2 = BindUtils.Table.AsView(tableOrView)) != null)
		{
			return schema.Views.GetAlterableObject(immutableObject2, lexicalScope, bindMode);
		}
		return null;
	}

	internal override void InitiateBindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		LexicalScope lexicalScope = GetLexicalScope();
		string value = targetObject.DatabaseName.Value;
		string value2 = targetObject.SchemaName.Value;
		string value3 = targetObject.ObjectName.Value;
		ITableViewBase tableViewBase = bindingContext.FindTableOrView(value, value2, value3, lexicalScope);
		SchemaEx schemaEx;
		CollationInfo collationInfo;
		if (tableViewBase != null)
		{
			schemaEx = bindingContext.SchemaToSchemaEx(tableViewBase.Schema);
			collationInfo = tableViewBase.CollationInfo;
		}
		else
		{
			schemaEx = bindingContext.GetCDASchema(value2, lexicalScope);
			collationInfo = schemaEx.Database.CollationInfo;
		}
		IIndexFactory index = bindingContext.MetadataFactory.Index;
		ITableViewBase tableViewBase2 = tableViewBase;
		if (tableViewBase2 == null)
		{
			tableViewBase2 = schemaEx.EmptyTable;
		}
		IMutableRelationalIndex mutableRelationalIndex = (boundIndex = index.CreateRelationalIndex(tableViewBase2, name.Value, collationInfo));
		base.BindingState |= BindingState.BindSucceeded;
		boundTable = tableViewBase;
		boundIndex.IsClustered = clusterOption == SqlClusterOption.Clustered;
		boundIndex.IsUnique = isUnique;
		if (boundTable != null && BindUtils.Table.IsView(boundTable) && boundIndex.IsClustered && !boundIndex.IsUnique)
		{
			name.ReportBindError(SR.CannotCreateNonuniqueClusteredIndexOnView(targetObject.ToString()));
			boundIndex.IsUnique = true;
		}
		IIndex index2;
		if (!HasDropExistingOption)
		{
			if (tableViewBase == null)
			{
				targetObject.ReportBindError(SR.InvalidObjectName(targetObject.ToString()), ErrorFlags.UnresolvedDatabaseObjectReference);
				base.BindingState |= BindingState.BindFailed;
			}
			else if (tableViewBase.Indexes.Contains(mutableRelationalIndex.Name))
			{
				name.ReportBindError(SR.IndexOrStatisticsExists(name.ToString(), targetObject.ToString()));
				base.BindingState |= BindingState.BindFailed;
			}
			else
			{
				alteredTable = GetAlterable(schemaEx, tableViewBase, lexicalScope, bindMode);
				proxyIndex = new IRelationalIndexProxy(mutableRelationalIndex, this);
				alteredTable.Indexes.Add(proxyIndex);
				boundTable = alteredTable;
				executionSuccedded = true;
			}
		}
		else if (tableViewBase == null)
		{
			targetObject.ReportBindError(SR.InvalidObjectName(targetObject.ToString()), ErrorFlags.UnresolvedDatabaseObjectReference);
			base.BindingState |= BindingState.BindFailed;
		}
		else if ((index2 = tableViewBase.Indexes[mutableRelationalIndex.Name]) == null)
		{
			name.ReportBindError(SR.CouldNotFindIndex(name.ToString(), targetObject.ToString()), ErrorFlags.UnresolvedDatabaseObjectReference);
			base.BindingState |= BindingState.BindFailed;
		}
		else
		{
			IRelationalIndex relationalIndex = BindUtils.Index.AsRelationalIndex(index2);
			if (relationalIndex == null)
			{
				name.ReportBindError(SR.CannotConvertXmlOrSpatialIndexToRelational(name.ToString()));
				base.BindingState |= BindingState.BindFailed;
			}
			else if (relationalIndex.IsClustered && !mutableRelationalIndex.IsClustered)
			{
				name.ReportBindError(SR.CannotConvertClusteredIndexToNonclustered);
				base.BindingState |= BindingState.BindFailed;
			}
			else
			{
				alteredTable = GetAlterable(schemaEx, tableViewBase, lexicalScope, bindMode);
				proxyIndex = new IRelationalIndexProxy(mutableRelationalIndex, this);
				alteredTable.Indexes[mutableRelationalIndex.Name] = proxyIndex;
				boundTable = alteredTable;
				executionSuccedded = true;
			}
		}
		if (boundTable == null)
		{
			boundTable = bindingContext.MetadataFactory.Tabular.CreateTable(schemaEx, value3);
		}
		else
		{
			bindingContext.SetBoundObject(targetObject.ObjectName, boundTable);
		}
	}

	private IMutableIndexedColumn CreateAndAddIndexedColumn(IIndexFactory factory, SqlIdentifier columnName, bool isKeyColumn)
	{
		IColumn column = boundTable.Columns[columnName.Value];
		IMutableIndexedColumn mutableIndexedColumn = null;
		if (column == null)
		{
			columnName.ReportBindError(SR.ColumnNameNotInTargetTable(columnName.ToString()), ErrorFlags.UnresolvedReference);
			base.BindingState |= BindingState.BindFailed;
		}
		else if (boundIndex.IndexedColumns.Contains(column.Name))
		{
			columnName.ReportBindError(SR.DuplicateColumnNamesInIndex(columnName.ToString()));
			base.BindingState |= BindingState.BindFailed;
		}
		else
		{
			mutableIndexedColumn = factory.CreateIndexedColumn(column);
			boundIndex.IndexedColumns.Add(mutableIndexedColumn);
		}
		return mutableIndexedColumn;
	}

	private void ValidateIndexedColumnType(IIndexedColumn column, SqlIdentifier columnName, out bool mustBeOffline)
	{
		IScalarDataType dataType = column.ReferencedColumn.DataType;
		bool flag = true;
		mustBeOffline = false;
		if (dataType != null && !dataType.IsVoid)
		{
			ISystemDataType baseSystemDataType = dataType.BaseSystemDataType;
			if (baseSystemDataType != null && baseSystemDataType.TypeSpec != null)
			{
				switch (baseSystemDataType.TypeSpec.SqlDataType)
				{
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.Image:
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.NText:
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.Text:
					flag = false;
					break;
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.Geography:
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.Geometry:
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.Xml:
					if (!column.IsIncluded)
					{
						flag = false;
					}
					break;
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.NVarCharMax:
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.VarBinaryMax:
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.VarCharMax:
					if (!column.IsIncluded)
					{
						flag = false;
					}
					else
					{
						mustBeOffline = true;
					}
					break;
				}
			}
		}
		if (!flag)
		{
			if (column.IsIncluded)
			{
				columnName.ReportBindError(SR.InvalidIndexIncludedColumnType(columnName.ToString(), targetObject.ToString()));
				base.BindingState |= BindingState.BindFailed;
			}
			else
			{
				columnName.ReportBindError(SR.InvalidIndexKeyColumnType(columnName.ToString(), targetObject.ToString()));
				base.BindingState |= BindingState.BindFailed;
			}
		}
	}

	internal override void FinalizeBindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		IIndexFactory index = bindingContext.MetadataFactory.Index;
		bool flag = false;
		foreach (SqlIndexedColumn indexedColumn in indexedColumns)
		{
			IMutableIndexedColumn mutableIndexedColumn = CreateAndAddIndexedColumn(index, indexedColumn.Name, isKeyColumn: true);
			if (mutableIndexedColumn != null)
			{
				mutableIndexedColumn.SortOrder = ((indexedColumn.SortOrder == SqlSortOrder.Descending) ? SortOrder.Descending : SortOrder.Ascending);
				mutableIndexedColumn.IsIncluded = false;
				ValidateIndexedColumnType(mutableIndexedColumn, indexedColumn.Name, out var mustBeOffline);
				flag = flag || mustBeOffline;
				if (boundTable.Columns[indexedColumn.Name.Value].IsSparse && boundIndex.IsClustered)
				{
					ReportBindError(SR.ColumnIsInvalidForUseAsKeyColumnInIndex(indexedColumn.Name.Value, boundTable.Name));
					base.BindingState |= BindingState.BindFailed;
				}
			}
		}
		if (includedColumns != null)
		{
			foreach (SqlIdentifier includedColumn in includedColumns)
			{
				IMutableIndexedColumn mutableIndexedColumn2 = CreateAndAddIndexedColumn(index, includedColumn, isKeyColumn: false);
				if (mutableIndexedColumn2 != null)
				{
					mutableIndexedColumn2.IsIncluded = true;
					ValidateIndexedColumnType(mutableIndexedColumn2, includedColumn, out var mustBeOffline2);
					flag = flag || mustBeOffline2;
				}
			}
			if (boundIndex.IsClustered)
			{
				name.ReportBindError(SR.CannotSpecifyIncludedColumnsForClusteredIndex);
				base.BindingState |= BindingState.BindFailed;
			}
		}
		if (filterClause != null)
		{
			filterClause.Bind(bindingContext, boundTable);
			if (bindMode == StatementBindMode.OutOfOrder)
			{
				boundIndex.FilterDefinition = filterClause.FilterExpression.Sql;
			}
		}
		bool mustBeOnline = false;
		if (options != null)
		{
			BindIndexOptions(options, boundIndex, out mustBeOnline);
		}
		if (flag && mustBeOnline)
		{
			name.ReportBindError(SR.OnlineOperationCannotBePerformedOnIndexInvalidColumns(name.ToString()));
			base.BindingState |= BindingState.BindFailed;
		}
		IIndex index2 = null;
		if (executionSuccedded)
		{
			foreach (IRelationalIndex index3 in boundTable.Indexes)
			{
				if (index3 != null && index3 != proxyIndex && index3.IsClustered)
				{
					index2 = index3;
					break;
				}
			}
			if (boundIndex.IsClustered && index2 != null)
			{
				name.ReportBindError(SR.ClusteredIndexExists(targetObject.ToString(), index2.Name));
				base.BindingState |= BindingState.BindFailed;
			}
		}
		IView view;
		if ((view = BindUtils.Table.AsView(boundTable)) == null)
		{
			return;
		}
		if (!view.IsSchemaBound)
		{
			targetObject.ReportBindError(SR.CannotCreateIndexOnViewNotSchemaBound(targetObject.ToString()));
			base.BindingState |= BindingState.BindFailed;
		}
		foreach (IColumn column in view.Columns)
		{
			IScalarDataType dataType = column.DataType;
			bool flag2 = true;
			if (dataType != null && !dataType.IsVoid)
			{
				ISystemDataType baseSystemDataType = dataType.BaseSystemDataType;
				if (baseSystemDataType != null && baseSystemDataType.TypeSpec != null)
				{
					switch (baseSystemDataType.TypeSpec.SqlDataType)
					{
					case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.Image:
					case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.NText:
					case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.Text:
					case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.Xml:
						flag2 = false;
						break;
					}
				}
			}
			if (!flag2)
			{
				targetObject.ReportBindError(SR.CannotCreateIndexOnViewContainsInvalidColumns(targetObject.ToString()));
				base.BindingState |= BindingState.BindFailed;
				break;
			}
		}
		if (executionSuccedded && !boundIndex.IsClustered && index2 == null)
		{
			name.ReportBindError(SR.CannotCreateIndexOnViewDoesNotHaveUniqueClusteredIndex(targetObject.ToString()));
			base.BindingState |= BindingState.BindFailed;
		}
	}

	public static void BindIndexOptions(SqlIndexOptionCollection options, IMutableRelationalIndex boundIndex, out bool mustBeOnline)
	{
		mustBeOnline = false;
		foreach (SqlIndexOption option in options)
		{
			if (option.Type == SqlIndexOptionType.Invalid)
			{
				continue;
			}
			switch (option.Type)
			{
			case SqlIndexOptionType.AllowPageLocks:
			{
				SqlAllowPageLocksIndexOption sqlAllowPageLocksIndexOption = option as SqlAllowPageLocksIndexOption;
				boundIndex.DisallowPageLocks = sqlAllowPageLocksIndexOption.OnOffValue == SqlOnOffValue.Off;
				break;
			}
			case SqlIndexOptionType.AllowRowLocks:
			{
				SqlAllowRowLocksIndexOption sqlAllowRowLocksIndexOption = option as SqlAllowRowLocksIndexOption;
				boundIndex.DisallowRowLocks = sqlAllowRowLocksIndexOption.OnOffValue == SqlOnOffValue.Off;
				break;
			}
			case SqlIndexOptionType.FillFactor:
			{
				SqlFillFactorIndexOption sqlFillFactorIndexOption = option as SqlFillFactorIndexOption;
				if (sqlFillFactorIndexOption.IsValid)
				{
					boundIndex.FillFactor = (byte)sqlFillFactorIndexOption.FillFactor;
				}
				break;
			}
			case SqlIndexOptionType.IgnoreDupKey:
			{
				SqlIgnoreDupKeyIndexOption sqlIgnoreDupKeyIndexOption = option as SqlIgnoreDupKeyIndexOption;
				boundIndex.IgnoreDuplicateKeys = sqlIgnoreDupKeyIndexOption.OnOffValue != SqlOnOffValue.Off;
				break;
			}
			case SqlIndexOptionType.Online:
				mustBeOnline = true;
				break;
			case SqlIndexOptionType.PadIndex:
			{
				SqlPadIndexOption sqlPadIndexOption = option as SqlPadIndexOption;
				boundIndex.PadIndex = sqlPadIndexOption.OnOffValue != SqlOnOffValue.Off;
				break;
			}
			case SqlIndexOptionType.StatisticsNoRecompute:
			{
				SqlStatisticsNoRecomputeIndexOption sqlStatisticsNoRecomputeIndexOption = option as SqlStatisticsNoRecomputeIndexOption;
				boundIndex.NoAutomaticRecomputation = sqlStatisticsNoRecomputeIndexOption.OnOffValue != SqlOnOffValue.Off;
				break;
			}
			}
		}
	}

	internal override void PostBindFinalize()
	{
		if (executionSuccedded)
		{
			alteredTable.Indexes[boundIndex.Name] = proxyIndex.Index;
			proxyIndex = null;
		}
	}

	internal override void Unbind()
	{
		boundIndex = null;
		boundTable = null;
		executionSuccedded = false;
		alteredTable = null;
		proxyIndex = null;
		base.Unbind();
	}

	internal SqlCreateIndexStatement(CreateIndexStart createIndexStart, SqlIdentifier name, SqlObjectIdentifier targetObject, CreateIndexKeyList createIndexKeyList, SqlFilterClause filterClause, IEnumerable<SqlIndexOption> options, SqlStorageSpecification storageSpecification, SqlStorageSpecification fileStreamStorageSpecification)
		: this(createIndexStart.IsUnique, createIndexStart.ClusterOption, name, targetObject, (createIndexKeyList == null) ? EmptySqlIndexedColumn : createIndexKeyList.IndexedColumns, (createIndexKeyList == null) ? EmptySqlIdentifier : createIndexKeyList.IncludedColumns, filterClause, options, storageSpecification, fileStreamStorageSpecification)
	{
	}

	internal SqlCreateIndexStatement(bool isUnique, SqlClusterOption clusterOption, SqlIdentifier name, SqlObjectIdentifier targetObject, IEnumerable<SqlIndexedColumn> indexedColumns, IEnumerable<SqlIdentifier> includedColumns, SqlFilterClause filterClause, IEnumerable<SqlIndexOption> options, SqlStorageSpecification storageSpecification, SqlStorageSpecification fileStreamStorageSpecification)
	{
		this.isUnique = isUnique;
		this.clusterOption = clusterOption;
		this.name = name;
		this.targetObject = targetObject;
		if (targetObject.Count > 3)
		{
			targetObject.ReportParseError(SR.TypeNameMaxPrefixError(targetObject.ToString(), 2));
			isValid = false;
		}
		this.indexedColumns = new SqlIndexedColumnCollection(indexedColumns);
		if (includedColumns != null)
		{
			this.includedColumns = new SqlIdentifierCollection(includedColumns);
		}
		if (filterClause != null)
		{
			this.filterClause = filterClause;
			if (!filterClause.FilterExpression.IsValid)
			{
				filterClause.FilterExpression.ReportParseError(SR.IncorrectWhereClauseForFilteredIndex(name.Value, targetObject.ToString()));
			}
		}
		if (options != null)
		{
			this.options = new SqlIndexOptionCollection(options);
			foreach (SqlIndexOption option in this.options)
			{
				if (option.Type != 0 && option.Type == SqlIndexOptionType.DropExisting)
				{
					SqlDropExistingIndexOption sqlDropExistingIndexOption = option as SqlDropExistingIndexOption;
					hasDropExistingOption = sqlDropExistingIndexOption.OnOffValue == SqlOnOffValue.On || sqlDropExistingIndexOption.OnOffValue == SqlOnOffValue.None;
				}
			}
		}
		this.storageSpecification = storageSpecification;
		this.fileStreamStorageSpecification = fileStreamStorageSpecification;
	}
}

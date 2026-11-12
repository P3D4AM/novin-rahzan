using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations.Internal;

public class MigrationsModelDiffer : IMigrationsModelDiffer
{
	private class PropertyInfoEqualityComparer : IEqualityComparer<PropertyInfo>
	{
		public static readonly PropertyInfoEqualityComparer Instance = new PropertyInfoEqualityComparer();

		private PropertyInfoEqualityComparer()
		{
		}

		public bool Equals(PropertyInfo x, PropertyInfo y)
		{
			return x.IsSameAs(y);
		}

		public int GetHashCode(PropertyInfo obj)
		{
			throw new NotImplementedException();
		}
	}

	private class EntryMapping
	{
		public HashSet<IUpdateEntry> SourceEntries { get; } = new HashSet<IUpdateEntry>();


		public HashSet<IUpdateEntry> TargetEntries { get; } = new HashSet<IUpdateEntry>();


		public bool RecreateRow { get; set; }
	}

	protected class DiffContext
	{
		private readonly IReadOnlyList<TableMapping> _sourceTables;

		private readonly IReadOnlyList<TableMapping> _targetTables;

		private readonly IDictionary<IEntityType, TableMapping> _sourceEntitiesMap = new Dictionary<IEntityType, TableMapping>();

		private readonly IDictionary<IEntityType, TableMapping> _targetEntitiesMap = new Dictionary<IEntityType, TableMapping>();

		private readonly IDictionary<object, object> _targetToSource = new Dictionary<object, object>();

		private readonly IDictionary<object, object> _sourceToTarget = new Dictionary<object, object>();

		private readonly IDictionary<IEntityType, CreateTableOperation> _createTableOperations = new Dictionary<IEntityType, CreateTableOperation>();

		private readonly IDictionary<IEntityType, DropTableOperation> _dropTableOperations = new Dictionary<IEntityType, DropTableOperation>();

		private readonly IDictionary<DropTableOperation, TableMapping> _removedTables = new Dictionary<DropTableOperation, TableMapping>();

		public DiffContext([JetBrains.Annotations.CanBeNull] IModel source, [JetBrains.Annotations.CanBeNull] IModel target)
		{
			if (source != null)
			{
				_sourceTables = TableMapping.GetTableMappings(source);
				foreach (TableMapping sourceTable in _sourceTables)
				{
					foreach (IEntityType entityType in sourceTable.EntityTypes)
					{
						_sourceEntitiesMap.Add(entityType, sourceTable);
					}
				}
			}
			if (target == null)
			{
				return;
			}
			_targetTables = TableMapping.GetTableMappings(target);
			foreach (TableMapping targetTable in _targetTables)
			{
				foreach (IEntityType entityType2 in targetTable.EntityTypes)
				{
					_targetEntitiesMap.Add(entityType2, targetTable);
				}
			}
		}

		public virtual IEnumerable<TableMapping> GetSourceTables()
		{
			return _sourceTables;
		}

		public virtual IEnumerable<TableMapping> GetTargetTables()
		{
			return _targetTables;
		}

		public virtual void AddMapping<T>([JetBrains.Annotations.NotNull] T source, [JetBrains.Annotations.NotNull] T target)
		{
			_targetToSource.Add(target, source);
			_sourceToTarget.Add(source, target);
		}

		public virtual void AddCreate([JetBrains.Annotations.NotNull] IEntityType target, [JetBrains.Annotations.NotNull] CreateTableOperation operation)
		{
			_createTableOperations.Add(target, operation);
		}

		public virtual void AddDrop([JetBrains.Annotations.NotNull] TableMapping source, [JetBrains.Annotations.NotNull] DropTableOperation operation)
		{
			foreach (IEntityType entityType in source.EntityTypes)
			{
				_dropTableOperations.Add(entityType, operation);
			}
			_removedTables.Add(operation, source);
		}

		public virtual TableMapping FindSourceTable(IEntityType entityType)
		{
			if (!_sourceEntitiesMap.TryGetValue(entityType, out var value))
			{
				return null;
			}
			return value;
		}

		public virtual TableMapping FindTargetTable(IEntityType entityType)
		{
			if (!_targetEntitiesMap.TryGetValue(entityType, out var value))
			{
				return null;
			}
			return value;
		}

		public virtual T FindSource<T>([JetBrains.Annotations.CanBeNull] T target) where T : class
		{
			if (target != null)
			{
				if (!_targetToSource.TryGetValue(target, out var value))
				{
					return null;
				}
				return (T)value;
			}
			return null;
		}

		public virtual IProperty FindSource([JetBrains.Annotations.NotNull] IProperty target)
		{
			IProperty property = this.FindSource<IProperty>(target);
			if (property != null)
			{
				return property;
			}
			foreach (IProperty item in from p in FindTargetTable(target.DeclaringEntityType).GetProperties()
				where p != target && p.GetColumnName() == target.GetColumnName()
				select p)
			{
				property = this.FindSource<IProperty>(item);
				if (property != null)
				{
					_targetToSource.Add(target, property);
					return property;
				}
			}
			return null;
		}

		public virtual T FindTarget<T>([JetBrains.Annotations.CanBeNull] T source) where T : class
		{
			if (source != null)
			{
				if (!_sourceToTarget.TryGetValue(source, out var value))
				{
					return null;
				}
				return (T)value;
			}
			return null;
		}

		public virtual CreateTableOperation FindCreate([JetBrains.Annotations.NotNull] IEntityType target)
		{
			if (!_createTableOperations.TryGetValue(target, out var value))
			{
				return null;
			}
			return value;
		}

		public virtual DropTableOperation FindDrop([JetBrains.Annotations.NotNull] IEntityType source)
		{
			if (!_dropTableOperations.TryGetValue(source, out var value))
			{
				return null;
			}
			return value;
		}

		public virtual TableMapping FindTable([JetBrains.Annotations.NotNull] DropTableOperation operation)
		{
			if (!_removedTables.TryGetValue(operation, out var value))
			{
				return null;
			}
			return value;
		}
	}

	private static readonly Type[] _dropOperationTypes = new Type[5]
	{
		typeof(DropIndexOperation),
		typeof(DropPrimaryKeyOperation),
		typeof(DropSequenceOperation),
		typeof(DropUniqueConstraintOperation),
		typeof(DropCheckConstraintOperation)
	};

	private static readonly Type[] _alterOperationTypes = new Type[3]
	{
		typeof(AddPrimaryKeyOperation),
		typeof(AddUniqueConstraintOperation),
		typeof(AlterSequenceOperation)
	};

	private static readonly Type[] _renameOperationTypes = new Type[3]
	{
		typeof(RenameColumnOperation),
		typeof(RenameIndexOperation),
		typeof(RenameSequenceOperation)
	};

	private static readonly Type[] _columnOperationTypes = new Type[2]
	{
		typeof(AddColumnOperation),
		typeof(AlterColumnOperation)
	};

	private static readonly Type[] _constraintOperationTypes = new Type[2]
	{
		typeof(AddForeignKeyOperation),
		typeof(CreateIndexOperation)
	};

	private IUpdateAdapter _sourceUpdateAdapter;

	private IUpdateAdapter _targetUpdateAdapter;

	private readonly List<SharedTableEntryMap<EntryMapping>> _sharedTableEntryMaps = new List<SharedTableEntryMap<EntryMapping>>();

	protected virtual IRelationalTypeMappingSource TypeMappingSource { get; }

	protected virtual IMigrationsAnnotationProvider MigrationsAnnotations { get; }

	protected virtual IUpdateAdapterFactory UpdateAdapterFactory { get; }

	protected virtual CommandBatchPreparerDependencies CommandBatchPreparerDependencies { get; }

	protected virtual IChangeDetector ChangeDetector { get; }

	public MigrationsModelDiffer([JetBrains.Annotations.NotNull] IRelationalTypeMappingSource typeMappingSource, [JetBrains.Annotations.NotNull] IMigrationsAnnotationProvider migrationsAnnotations, [JetBrains.Annotations.NotNull] IChangeDetector changeDetector, [JetBrains.Annotations.NotNull] IUpdateAdapterFactory updateAdapterFactory, [JetBrains.Annotations.NotNull] CommandBatchPreparerDependencies commandBatchPreparerDependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(typeMappingSource, "typeMappingSource");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(migrationsAnnotations, "migrationsAnnotations");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(updateAdapterFactory, "updateAdapterFactory");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandBatchPreparerDependencies, "commandBatchPreparerDependencies");
		TypeMappingSource = typeMappingSource;
		MigrationsAnnotations = migrationsAnnotations;
		ChangeDetector = changeDetector;
		UpdateAdapterFactory = updateAdapterFactory;
		CommandBatchPreparerDependencies = commandBatchPreparerDependencies;
	}

	public virtual bool HasDifferences(IModel source, IModel target)
	{
		return Diff(source, target, new DiffContext(source, target)).Any();
	}

	public virtual IReadOnlyList<MigrationOperation> GetDifferences(IModel source, IModel target)
	{
		DiffContext diffContext = new DiffContext(source, target);
		return Sort(Diff(source, target, diffContext), diffContext);
	}

	protected virtual IReadOnlyList<MigrationOperation> Sort([JetBrains.Annotations.NotNull] IEnumerable<MigrationOperation> operations, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operations, "operations");
		List<MigrationOperation> dropForeignKeyOperations = new List<MigrationOperation>();
		List<MigrationOperation> list = new List<MigrationOperation>();
		List<MigrationOperation> list2 = new List<MigrationOperation>();
		List<DropTableOperation> list3 = new List<DropTableOperation>();
		List<MigrationOperation> list4 = new List<MigrationOperation>();
		List<MigrationOperation> list5 = new List<MigrationOperation>();
		List<MigrationOperation> list6 = new List<MigrationOperation>();
		List<CreateTableOperation> list7 = new List<CreateTableOperation>();
		List<MigrationOperation> list8 = new List<MigrationOperation>();
		List<MigrationOperation> list9 = new List<MigrationOperation>();
		List<MigrationOperation> list10 = new List<MigrationOperation>();
		List<MigrationOperation> list11 = new List<MigrationOperation>();
		List<MigrationOperation> list12 = new List<MigrationOperation>();
		List<MigrationOperation> list13 = new List<MigrationOperation>();
		List<MigrationOperation> constraintOperations = new List<MigrationOperation>();
		List<MigrationOperation> list14 = new List<MigrationOperation>();
		List<MigrationOperation> list15 = new List<MigrationOperation>();
		List<MigrationOperation> list16 = new List<MigrationOperation>();
		List<MigrationOperation> list17 = new List<MigrationOperation>();
		List<MigrationOperation> list18 = new List<MigrationOperation>();
		foreach (MigrationOperation operation in operations)
		{
			Type type = operation.GetType();
			if (type == typeof(DropForeignKeyOperation))
			{
				dropForeignKeyOperations.Add(operation);
			}
			else if (_dropOperationTypes.Contains(type))
			{
				list.Add(operation);
			}
			else if (type == typeof(DropColumnOperation))
			{
				list2.Add(operation);
			}
			else if (type == typeof(DropTableOperation))
			{
				list3.Add((DropTableOperation)operation);
			}
			else if (type == typeof(EnsureSchemaOperation))
			{
				list4.Add(operation);
			}
			else if (type == typeof(CreateSequenceOperation))
			{
				list5.Add(operation);
			}
			else if (type == typeof(CreateCheckConstraintOperation))
			{
				list6.Add(operation);
			}
			else if (type == typeof(CreateTableOperation))
			{
				list7.Add((CreateTableOperation)operation);
			}
			else if (type == typeof(AlterDatabaseOperation))
			{
				list8.Add(operation);
			}
			else if (type == typeof(AlterTableOperation))
			{
				list9.Add(operation);
			}
			else if (_columnOperationTypes.Contains(type))
			{
				if (string.IsNullOrWhiteSpace(((ColumnOperation)operation).ComputedColumnSql))
				{
					list10.Add(operation);
				}
				else
				{
					list11.Add(operation);
				}
			}
			else if (_alterOperationTypes.Contains(type))
			{
				list12.Add(operation);
			}
			else if (type == typeof(RestartSequenceOperation))
			{
				list13.Add(operation);
			}
			else if (_constraintOperationTypes.Contains(type))
			{
				constraintOperations.Add(operation);
			}
			else if (_renameOperationTypes.Contains(type))
			{
				list14.Add(operation);
			}
			else if (type == typeof(RenameTableOperation))
			{
				list15.Add(operation);
			}
			else if (type == typeof(DeleteDataOperation))
			{
				list16.Add(operation);
			}
			else if (type == typeof(InsertDataOperation) || type == typeof(UpdateDataOperation))
			{
				list17.Add(operation);
			}
			else
			{
				list18.Add(operation);
			}
		}
		Multigraph<CreateTableOperation, AddForeignKeyOperation> multigraph = new Multigraph<CreateTableOperation, AddForeignKeyOperation>();
		multigraph.AddVertices(list7);
		foreach (CreateTableOperation item in list7)
		{
			foreach (AddForeignKeyOperation addForeignKeyOperation in item.ForeignKeys)
			{
				if (!(addForeignKeyOperation.Table == addForeignKeyOperation.PrincipalTable) || !(addForeignKeyOperation.Schema == addForeignKeyOperation.PrincipalSchema))
				{
					CreateTableOperation createTableOperation2 = list7.FirstOrDefault((CreateTableOperation o) => o.Name == addForeignKeyOperation.PrincipalTable && o.Schema == addForeignKeyOperation.PrincipalSchema);
					if (createTableOperation2 != null)
					{
						multigraph.AddEdge(createTableOperation2, item, addForeignKeyOperation);
					}
				}
			}
		}
		list7 = multigraph.TopologicalSort(delegate(CreateTableOperation principalCreateTableOperation, CreateTableOperation createTableOperation, IEnumerable<AddForeignKeyOperation> cyclicAddForeignKeyOperations)
		{
			foreach (AddForeignKeyOperation cyclicAddForeignKeyOperation in cyclicAddForeignKeyOperations)
			{
				createTableOperation.ForeignKeys.Remove(cyclicAddForeignKeyOperation);
				constraintOperations.Add(cyclicAddForeignKeyOperation);
			}
			return true;
		}).ToList();
		Multigraph<DropTableOperation, IForeignKey> multigraph2 = new Multigraph<DropTableOperation, IForeignKey>();
		multigraph2.AddVertices(list3);
		foreach (DropTableOperation item2 in list3)
		{
			foreach (IForeignKey foreignKey in diffContext.FindTable(item2).GetForeignKeys())
			{
				IEntityType principalEntityType = foreignKey.PrincipalEntityType;
				DropTableOperation dropTableOperation2 = diffContext.FindDrop(principalEntityType);
				if (dropTableOperation2 != null && dropTableOperation2 != item2)
				{
					multigraph2.AddEdge(item2, dropTableOperation2, foreignKey);
				}
			}
		}
		DiffContext newDiffContext = new DiffContext(null, null);
		list3 = multigraph2.TopologicalSort(delegate(DropTableOperation dropTableOperation, DropTableOperation principalDropTableOperation, IEnumerable<IForeignKey> foreignKeys)
		{
			dropForeignKeyOperations.AddRange(foreignKeys.SelectMany((IForeignKey c) => Remove(c, newDiffContext)));
			return true;
		}).ToList();
		return dropForeignKeyOperations.Concat(list3).Concat(list).Concat(list16)
			.Concat(list2)
			.Concat(list4)
			.Concat(list15)
			.Concat(list14)
			.Concat(list8)
			.Concat(list5)
			.Concat(list6)
			.Concat(list9)
			.Concat(list10)
			.Concat(list11)
			.Concat(list12)
			.Concat(list13)
			.Concat(list7)
			.Concat(list17)
			.Concat(constraintOperations)
			.Concat(list18)
			.ToList();
	}

	protected virtual IEnumerable<MigrationOperation> Diff([JetBrains.Annotations.CanBeNull] IModel source, [JetBrains.Annotations.CanBeNull] IModel target, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		TrackData(source, target);
		return ((source != null && target != null) ? DiffAnnotations(source, target).Concat(Diff(GetSchemas(source), GetSchemas(target), diffContext)).Concat(Diff(diffContext.GetSourceTables(), diffContext.GetTargetTables(), diffContext)).Concat(Diff(source.GetSequences(), target.GetSequences(), diffContext))
			.Concat(Diff(diffContext.GetSourceTables().SelectMany((TableMapping s) => s.GetForeignKeys()), diffContext.GetTargetTables().SelectMany((TableMapping t) => t.GetForeignKeys()), diffContext)) : ((target != null) ? Add(target, diffContext) : ((source != null) ? Remove(source, diffContext) : Enumerable.Empty<MigrationOperation>()))).Concat(GetDataOperations(diffContext));
	}

	private IEnumerable<MigrationOperation> DiffAnnotations(IModel source, IModel target)
	{
		List<IAnnotation> list = ((source == null) ? null : MigrationsAnnotations.For(source).ToList());
		List<IAnnotation> list2 = ((target == null) ? null : MigrationsAnnotations.For(target).ToList());
		if (source == null)
		{
			if (list2 != null && list2.Count > 0)
			{
				AlterDatabaseOperation alterDatabaseOperation = new AlterDatabaseOperation();
				alterDatabaseOperation.AddAnnotations(list2);
				yield return alterDatabaseOperation;
			}
		}
		else if (target == null)
		{
			list = MigrationsAnnotations.ForRemove(source).ToList();
			if (list.Count > 0)
			{
				AlterDatabaseOperation alterDatabaseOperation2 = new AlterDatabaseOperation();
				alterDatabaseOperation2.OldDatabase.AddAnnotations(MigrationsAnnotations.ForRemove(source));
				yield return alterDatabaseOperation2;
			}
		}
		else if (HasDifferences(list, list2))
		{
			AlterDatabaseOperation alterDatabaseOperation3 = new AlterDatabaseOperation();
			alterDatabaseOperation3.AddAnnotations(list2);
			alterDatabaseOperation3.OldDatabase.AddAnnotations(list);
			yield return alterDatabaseOperation3;
		}
	}

	protected virtual IEnumerable<MigrationOperation> Add([JetBrains.Annotations.NotNull] IModel target, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		return DiffAnnotations(null, target).Concat(GetSchemas(target).SelectMany((string t) => Add(t, diffContext))).Concat(diffContext.GetTargetTables().SelectMany((TableMapping t) => Add(t, diffContext))).Concat(target.GetSequences().SelectMany((ISequence t) => Add(t, diffContext)))
			.Concat(diffContext.GetTargetTables().SelectMany((TableMapping t) => t.GetForeignKeys()).SelectMany((IForeignKey k) => Add(k, diffContext)));
	}

	protected virtual IEnumerable<MigrationOperation> Remove([JetBrains.Annotations.NotNull] IModel source, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		return DiffAnnotations(source, null).Concat(diffContext.GetSourceTables().SelectMany((TableMapping t) => Remove(t, diffContext))).Concat(source.GetSequences().SelectMany((ISequence t) => Remove(t, diffContext)));
	}

	protected virtual IEnumerable<MigrationOperation> Diff([JetBrains.Annotations.NotNull] IEnumerable<string> source, [JetBrains.Annotations.NotNull] IEnumerable<string> target, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		return DiffCollection(source, target, diffContext, Diff, Add, Remove, (string s, string t, DiffContext c) => s == t);
	}

	protected virtual IEnumerable<MigrationOperation> Diff([JetBrains.Annotations.NotNull] string source, [JetBrains.Annotations.NotNull] string target, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		return Enumerable.Empty<MigrationOperation>();
	}

	protected virtual IEnumerable<MigrationOperation> Add([JetBrains.Annotations.NotNull] string target, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		yield return new EnsureSchemaOperation
		{
			Name = target
		};
	}

	protected virtual IEnumerable<MigrationOperation> Remove([JetBrains.Annotations.NotNull] string source, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		return Enumerable.Empty<MigrationOperation>();
	}

	protected virtual IEnumerable<MigrationOperation> Diff([JetBrains.Annotations.NotNull] IEnumerable<TableMapping> source, [JetBrains.Annotations.NotNull] IEnumerable<TableMapping> target, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		return DiffCollection(source, target, diffContext, Diff, Add, Remove, (TableMapping s, TableMapping t, DiffContext c) => string.Equals(s.Schema, t.Schema, StringComparison.OrdinalIgnoreCase) && string.Equals(s.Name, t.Name, StringComparison.OrdinalIgnoreCase), (TableMapping s, TableMapping t, DiffContext c) => string.Equals(s.Name, t.Name, StringComparison.OrdinalIgnoreCase), (TableMapping s, TableMapping t, DiffContext c) => string.Equals(s.GetRootType().Name, t.GetRootType().Name, StringComparison.OrdinalIgnoreCase), (TableMapping s, TableMapping t, DiffContext c) => s.EntityTypes.Any((IEntityType se) => t.EntityTypes.Any((IEntityType te) => string.Equals(se.Name, te.Name, StringComparison.OrdinalIgnoreCase))));
	}

	protected virtual IEnumerable<MigrationOperation> Diff([JetBrains.Annotations.NotNull] TableMapping source, [JetBrains.Annotations.NotNull] TableMapping target, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		if (source.Schema != target.Schema || source.Name != target.Name)
		{
			yield return new RenameTableOperation
			{
				Schema = source.Schema,
				Name = source.Name,
				NewSchema = target.Schema,
				NewName = target.Name
			};
		}
		List<IAnnotation> list = MigrationsAnnotations.For(source.EntityTypes[0]).ToList();
		List<IAnnotation> list2 = MigrationsAnnotations.For(target.EntityTypes[0]).ToList();
		if (source.GetComment() != target.GetComment() || HasDifferences(list, list2))
		{
			AlterTableOperation alterTableOperation = new AlterTableOperation
			{
				Name = target.Name,
				Schema = target.Schema,
				Comment = target.GetComment(),
				OldTable = 
				{
					Comment = source.GetComment()
				}
			};
			alterTableOperation.AddAnnotations(list2);
			alterTableOperation.OldTable.AddAnnotations(list);
			yield return alterTableOperation;
		}
		IEnumerable<MigrationOperation> enumerable = Diff(source.GetProperties(), target.GetProperties(), diffContext).Concat(Diff(source.GetKeys(), target.GetKeys(), diffContext)).Concat(Diff(source.GetIndexes(), target.GetIndexes(), diffContext)).Concat(Diff(source.GetCheckConstraints(), target.GetCheckConstraints(), diffContext));
		foreach (MigrationOperation item in enumerable)
		{
			yield return item;
		}
		DiffData(source, target, diffContext);
	}

	protected virtual IEnumerable<MigrationOperation> Add([JetBrains.Annotations.NotNull] TableMapping target, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		IEntityType entityType = target.EntityTypes[0];
		CreateTableOperation createTableOperation = new CreateTableOperation
		{
			Schema = target.Schema,
			Name = target.Name,
			Comment = target.GetComment()
		};
		createTableOperation.AddAnnotations(MigrationsAnnotations.For(entityType));
		createTableOperation.Columns.AddRange(GetSortedProperties(target).SelectMany((IProperty p) => Add(p, diffContext, inline: true)).Cast<AddColumnOperation>());
		IKey key = target.EntityTypes[0].FindPrimaryKey();
		if (key != null)
		{
			createTableOperation.PrimaryKey = Add(key, diffContext).Cast<AddPrimaryKeyOperation>().Single();
		}
		createTableOperation.UniqueConstraints.AddRange((from k in target.GetKeys()
			where !k.IsPrimaryKey()
			select k).SelectMany((IKey k) => Add(k, diffContext)).Cast<AddUniqueConstraintOperation>());
		createTableOperation.CheckConstraints.AddRange(target.GetCheckConstraints().SelectMany((ICheckConstraint c) => Add(c, diffContext)).Cast<CreateCheckConstraintOperation>());
		foreach (IEntityType entityType2 in target.EntityTypes)
		{
			diffContext.AddCreate(entityType2, createTableOperation);
		}
		yield return createTableOperation;
		foreach (MigrationOperation item in target.GetIndexes().SelectMany((IIndex i) => Add(i, diffContext)))
		{
			yield return item;
		}
	}

	protected virtual IEnumerable<MigrationOperation> Remove([JetBrains.Annotations.NotNull] TableMapping source, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		DropTableOperation dropTableOperation = new DropTableOperation
		{
			Schema = source.Schema,
			Name = source.Name
		};
		dropTableOperation.AddAnnotations(MigrationsAnnotations.ForRemove(source.EntityTypes[0]));
		diffContext.AddDrop(source, dropTableOperation);
		yield return dropTableOperation;
	}

	private static IEnumerable<IProperty> GetSortedProperties(TableMapping target)
	{
		return GetSortedProperties(target.GetRootType()).Distinct((IProperty x, IProperty y) => x.GetColumnName() == y.GetColumnName());
	}

	private static IEnumerable<IProperty> GetSortedProperties(IEntityType entityType)
	{
		List<IProperty> list = new List<IProperty>();
		List<IProperty> list2 = new List<IProperty>();
		Dictionary<PropertyInfo, IProperty> primaryKeyPropertyGroups = new Dictionary<PropertyInfo, IProperty>();
		Dictionary<PropertyInfo, List<IProperty>> groups = new Dictionary<PropertyInfo, List<IProperty>>();
		Dictionary<PropertyInfo, SortedDictionary<int, IProperty>> dictionary = new Dictionary<PropertyInfo, SortedDictionary<int, IProperty>>();
		Dictionary<Type, SortedDictionary<int, PropertyInfo>> types = new Dictionary<Type, SortedDictionary<int, PropertyInfo>>();
		foreach (IProperty declaredProperty in entityType.GetDeclaredProperties())
		{
			PropertyInfo propertyInfo = declaredProperty.PropertyInfo;
			if (propertyInfo == null)
			{
				if (declaredProperty.IsPrimaryKey())
				{
					list2.Add(declaredProperty);
					continue;
				}
				IForeignKey foreignKey = declaredProperty.GetContainingForeignKeys().FirstOrDefault((IForeignKey fk) => fk.DependentToPrincipal?.PropertyInfo != null);
				if (foreignKey == null)
				{
					list.Add(declaredProperty);
					continue;
				}
				propertyInfo = foreignKey.DependentToPrincipal.PropertyInfo;
				int key = foreignKey.Properties.IndexOf(declaredProperty);
				dictionary.GetOrAddNew(propertyInfo).Add(key, declaredProperty);
			}
			else
			{
				if (declaredProperty.IsPrimaryKey())
				{
					primaryKeyPropertyGroups.Add(propertyInfo, declaredProperty);
				}
				groups.Add(propertyInfo, new List<IProperty> { declaredProperty });
			}
			Type declaringType = propertyInfo.DeclaringType;
			int key2 = declaringType.GetTypeInfo().DeclaredProperties.IndexOf<PropertyInfo>(propertyInfo, PropertyInfoEqualityComparer.Instance);
			types.GetOrAddNew(declaringType)[key2] = propertyInfo;
		}
		foreach (KeyValuePair<PropertyInfo, SortedDictionary<int, IProperty>> item in dictionary)
		{
			groups.Add(item.Key, item.Value.Values.ToList());
		}
		foreach (IForeignKey item2 in from fk in entityType.GetDeclaredReferencingForeignKeys()
			where fk.DeclaringEntityType.GetRootType() != entityType.GetRootType() && fk.DeclaringEntityType.GetTableName() == entityType.GetTableName() && fk.DeclaringEntityType.GetSchema() == entityType.GetSchema() && fk == fk.DeclaringEntityType.FindForeignKey(fk.DeclaringEntityType.FindPrimaryKey().Properties, entityType.FindPrimaryKey(), entityType)
			select fk)
		{
			PropertyInfo propertyInfo2 = item2.PrincipalToDependent?.PropertyInfo;
			List<IProperty> list3 = GetSortedProperties(item2.DeclaringEntityType).ToList();
			if (propertyInfo2 == null)
			{
				list.AddRange(list3);
				continue;
			}
			groups.Add(propertyInfo2, list3);
			Type declaringType2 = propertyInfo2.DeclaringType;
			int key3 = declaringType2.GetTypeInfo().DeclaredProperties.IndexOf<PropertyInfo>(propertyInfo2, PropertyInfoEqualityComparer.Instance);
			types.GetOrAddNew(declaringType2)[key3] = propertyInfo2;
		}
		Multigraph<Type, object> multigraph = new Multigraph<Type, object>();
		multigraph.AddVertices(types.Keys);
		foreach (Type key4 in types.Keys)
		{
			bool flag = false;
			foreach (Type baseType in key4.GetBaseTypes())
			{
				foreach (Type key5 in types.Keys)
				{
					if (key5 == baseType)
					{
						multigraph.AddEdge(key5, key4, null);
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
		}
		List<PropertyInfo> source = multigraph.TopologicalSort().SelectMany((Type e) => types[e].Values).ToList();
		return (from pi in source
			select (!primaryKeyPropertyGroups.ContainsKey(pi)) ? null : primaryKeyPropertyGroups[pi] into e
			where e != null
			select e).Concat(list2).Concat(source.Where((PropertyInfo pi) => !primaryKeyPropertyGroups.ContainsKey(pi)).SelectMany((PropertyInfo p) => groups[p])).Concat(list)
			.Concat(entityType.GetDirectlyDerivedTypes().SelectMany(GetSortedProperties));
	}

	protected virtual IEnumerable<MigrationOperation> Diff([JetBrains.Annotations.NotNull] IEnumerable<IProperty> source, [JetBrains.Annotations.NotNull] IEnumerable<IProperty> target, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		return DiffCollection(source, target, diffContext, Diff, (IProperty t, DiffContext c) => Add(t, c), Remove, (IProperty s, IProperty t, DiffContext c) => string.Equals(s.GetColumnName(), t.GetColumnName(), StringComparison.OrdinalIgnoreCase), (IProperty s, IProperty t, DiffContext c) => string.Equals(s.Name, t.Name, StringComparison.OrdinalIgnoreCase) && EntityTypePathEquals(s.DeclaringEntityType, t.DeclaringEntityType, c), (IProperty s, IProperty t, DiffContext c) => string.Equals(s.Name, t.Name, StringComparison.OrdinalIgnoreCase), (IProperty s, IProperty t, DiffContext c) => EntityTypePathEquals(s.DeclaringEntityType, t.DeclaringEntityType, c) && PropertyStructureEquals(s, t), (IProperty s, IProperty t, DiffContext c) => PropertyStructureEquals(s, t));
	}

	private bool PropertyStructureEquals(IProperty source, IProperty target)
	{
		if (source.ClrType == target.ClrType && source.IsConcurrencyToken == target.IsConcurrencyToken && source.ValueGenerated == target.ValueGenerated && source.GetMaxLength() == target.GetMaxLength() && source.IsColumnNullable() == target.IsColumnNullable() && source.IsUnicode() == target.IsUnicode() && source.IsFixedLength() == target.IsFixedLength() && source.GetConfiguredColumnType() == target.GetConfiguredColumnType() && source.GetComputedColumnSql() == target.GetComputedColumnSql() && object.Equals(GetDefaultValue(source), GetDefaultValue(target)))
		{
			return source.GetDefaultValueSql() == target.GetDefaultValueSql();
		}
		return false;
	}

	private static bool EntityTypePathEquals(IEntityType source, IEntityType target, DiffContext diffContext)
	{
		TableMapping tableMapping = diffContext.FindSourceTable(source);
		TableMapping tableMapping2 = diffContext.FindTargetTable(target);
		if (tableMapping.EntityTypes.Count == 1 && tableMapping2.EntityTypes.Count == 1)
		{
			return true;
		}
		if (!string.Equals(GetDefiningNavigationName(source), GetDefiningNavigationName(target), StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		IEntityType entityType = source.DefiningEntityType ?? source.BaseType;
		IEntityType entityType2 = target.DefiningEntityType ?? target.BaseType;
		if (entityType != null && tableMapping.EntityTypes.Contains(entityType) && entityType2 != null && tableMapping2.EntityTypes.Contains(entityType2))
		{
			return EntityTypePathEquals(entityType, entityType2, diffContext);
		}
		return true;
	}

	private static string GetDefiningNavigationName(IEntityType entityType)
	{
		if (entityType.DefiningNavigationName != null)
		{
			return entityType.DefiningNavigationName;
		}
		IKey key = entityType.FindDeclaredPrimaryKey();
		if (key != null)
		{
			IForeignKey foreignKey = entityType.FindForeignKeys(key.Properties).FirstOrDefault((IForeignKey fk) => fk.PrincipalEntityType.GetTableName() == entityType.GetTableName());
			if (foreignKey?.DependentToPrincipal != null)
			{
				return foreignKey.DependentToPrincipal.Name;
			}
		}
		return entityType.Name;
	}

	protected virtual IEnumerable<MigrationOperation> Diff([JetBrains.Annotations.NotNull] IProperty source, [JetBrains.Annotations.NotNull] IProperty target, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		IEntityType targetEntityType = target.DeclaringEntityType.GetRootType();
		if (source.GetColumnName() != target.GetColumnName())
		{
			yield return new RenameColumnOperation
			{
				Schema = targetEntityType.GetSchema(),
				Table = targetEntityType.GetTableName(),
				Name = source.GetColumnName(),
				NewName = target.GetColumnName()
			};
		}
		RelationalTypeMapping mapping = TypeMappingSource.GetMapping(source);
		RelationalTypeMapping mapping2 = TypeMappingSource.GetMapping(target);
		string text = source.GetColumnType() ?? mapping.StoreType;
		string text2 = target.GetColumnType() ?? mapping2.StoreType;
		List<IAnnotation> list = MigrationsAnnotations.For(source).ToList();
		List<IAnnotation> list2 = MigrationsAnnotations.For(target).ToList();
		bool flag = source.IsColumnNullable();
		bool flag2 = target.IsColumnNullable();
		bool flag3 = flag != flag2;
		bool flag4 = text != text2;
		if (flag3 || flag4 || source.GetDefaultValueSql() != target.GetDefaultValueSql() || source.GetComputedColumnSql() != target.GetComputedColumnSql() || !object.Equals(GetDefaultValue(source), GetDefaultValue(target)) || source.GetComment() != target.GetComment() || HasDifferences(list, list2))
		{
			bool isDestructiveChange = (flag3 && flag) || flag4;
			AlterColumnOperation alterColumnOperation = new AlterColumnOperation
			{
				Schema = targetEntityType.GetSchema(),
				Table = targetEntityType.GetTableName(),
				Name = target.GetColumnName(),
				IsDestructiveChange = isDestructiveChange
			};
			Initialize(alterColumnOperation, target, mapping2, flag2, list2, inline: true);
			Initialize(alterColumnOperation.OldColumn, source, mapping, flag, list, inline: true);
			yield return alterColumnOperation;
		}
	}

	protected virtual IEnumerable<MigrationOperation> Add([JetBrains.Annotations.NotNull] IProperty target, [JetBrains.Annotations.NotNull] DiffContext diffContext, bool inline = false)
	{
		IEntityType rootType = target.DeclaringEntityType.GetRootType();
		AddColumnOperation addColumnOperation = new AddColumnOperation
		{
			Schema = rootType.GetSchema(),
			Table = rootType.GetTableName(),
			Name = target.GetColumnName()
		};
		Initialize(addColumnOperation, target, TypeMappingSource.GetMapping(target), target.IsColumnNullable(), MigrationsAnnotations.For(target), inline);
		yield return addColumnOperation;
	}

	protected virtual IEnumerable<MigrationOperation> Remove([JetBrains.Annotations.NotNull] IProperty source, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		IEntityType rootType = source.DeclaringEntityType.GetRootType();
		DropColumnOperation dropColumnOperation = new DropColumnOperation
		{
			Schema = rootType.GetSchema(),
			Table = rootType.GetTableName(),
			Name = source.GetColumnName()
		};
		dropColumnOperation.AddAnnotations(MigrationsAnnotations.ForRemove(source));
		yield return dropColumnOperation;
	}

	private void Initialize(ColumnOperation columnOperation, IProperty property, CoreTypeMapping typeMapping, bool isNullable, IEnumerable<IAnnotation> migrationsAnnotations, bool inline = false)
	{
		columnOperation.ClrType = (typeMapping.Converter?.ProviderClrType ?? typeMapping.ClrType).UnwrapNullableType();
		columnOperation.ColumnType = property.GetConfiguredColumnType();
		columnOperation.MaxLength = property.GetMaxLength();
		columnOperation.IsUnicode = property.IsUnicode();
		columnOperation.IsFixedLength = property.IsFixedLength();
		columnOperation.IsRowVersion = property.ClrType == typeof(byte[]) && property.IsConcurrencyToken && property.ValueGenerated == ValueGenerated.OnAddOrUpdate;
		columnOperation.IsNullable = isNullable;
		object defaultValue = GetDefaultValue(property);
		columnOperation.DefaultValue = ((defaultValue == DBNull.Value) ? null : defaultValue) ?? ((inline || isNullable) ? null : GetDefaultValue(columnOperation.ClrType));
		columnOperation.DefaultValueSql = property.GetDefaultValueSql();
		columnOperation.ComputedColumnSql = property.GetComputedColumnSql();
		columnOperation.Comment = property.GetComment();
		columnOperation.AddAnnotations(migrationsAnnotations);
	}

	protected virtual IEnumerable<MigrationOperation> Diff([JetBrains.Annotations.NotNull] IEnumerable<IKey> source, [JetBrains.Annotations.NotNull] IEnumerable<IKey> target, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		return DiffCollection(source, target, diffContext, Diff, Add, Remove, (IKey s, IKey t, DiffContext c) => s.GetName() == t.GetName() && s.Properties.Select((IProperty p) => p.GetColumnName()).SequenceEqual(t.Properties.Select((IProperty p) => c.FindSource(p)?.GetColumnName())) && s.IsPrimaryKey() == t.IsPrimaryKey() && !HasDifferences(MigrationsAnnotations.For(s), MigrationsAnnotations.For(t)));
	}

	protected virtual IEnumerable<MigrationOperation> Diff([JetBrains.Annotations.NotNull] IKey source, [JetBrains.Annotations.NotNull] IKey target, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		return Enumerable.Empty<MigrationOperation>();
	}

	protected virtual IEnumerable<MigrationOperation> Add([JetBrains.Annotations.NotNull] IKey target, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		IEntityType rootType = target.DeclaringEntityType.GetRootType();
		string[] columns = GetColumns(target.Properties);
		MigrationOperation migrationOperation = ((!target.IsPrimaryKey()) ? ((MigrationOperation)new AddUniqueConstraintOperation
		{
			Schema = rootType.GetSchema(),
			Table = rootType.GetTableName(),
			Name = target.GetName(),
			Columns = columns
		}) : ((MigrationOperation)new AddPrimaryKeyOperation
		{
			Schema = rootType.GetSchema(),
			Table = rootType.GetTableName(),
			Name = target.GetName(),
			Columns = columns
		}));
		migrationOperation.AddAnnotations(MigrationsAnnotations.For(target));
		yield return migrationOperation;
	}

	protected virtual IEnumerable<MigrationOperation> Remove([JetBrains.Annotations.NotNull] IKey source, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		IEntityType rootType = source.DeclaringEntityType.GetRootType();
		MigrationOperation migrationOperation = ((!source.IsPrimaryKey()) ? ((MigrationOperation)new DropUniqueConstraintOperation
		{
			Schema = rootType.GetSchema(),
			Table = rootType.GetTableName(),
			Name = source.GetName()
		}) : ((MigrationOperation)new DropPrimaryKeyOperation
		{
			Schema = rootType.GetSchema(),
			Table = rootType.GetTableName(),
			Name = source.GetName()
		}));
		migrationOperation.AddAnnotations(MigrationsAnnotations.ForRemove(source));
		yield return migrationOperation;
	}

	protected virtual IEnumerable<MigrationOperation> Diff([JetBrains.Annotations.NotNull] IEnumerable<IForeignKey> source, [JetBrains.Annotations.NotNull] IEnumerable<IForeignKey> target, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		return DiffCollection(source, target, diffContext, Diff, Add, Remove, (IForeignKey s, IForeignKey t, DiffContext c) => s.GetConstraintName() == t.GetConstraintName() && s.Properties.Select((IProperty p) => p.GetColumnName()).SequenceEqual(t.Properties.Select((IProperty p) => c.FindSource(p)?.GetColumnName())) && c.FindSourceTable(s.PrincipalEntityType) == c.FindSource(c.FindTargetTable(t.PrincipalEntityType)) && s.PrincipalKey.Properties.Select((IProperty p) => p.GetColumnName()).SequenceEqual(t.PrincipalKey.Properties.Select((IProperty p) => c.FindSource(p)?.GetColumnName())) && ToReferentialAction(s.DeleteBehavior) == ToReferentialAction(t.DeleteBehavior) && !HasDifferences(MigrationsAnnotations.For(s), MigrationsAnnotations.For(t)));
	}

	protected virtual IEnumerable<MigrationOperation> Diff([JetBrains.Annotations.NotNull] IForeignKey source, [JetBrains.Annotations.NotNull] IForeignKey target, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		return Enumerable.Empty<MigrationOperation>();
	}

	protected virtual IEnumerable<MigrationOperation> Add([JetBrains.Annotations.NotNull] IForeignKey target, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		IEntityType rootType = target.DeclaringEntityType.GetRootType();
		IEntityType rootType2 = target.PrincipalEntityType.GetRootType();
		AddForeignKeyOperation addForeignKeyOperation = new AddForeignKeyOperation
		{
			Schema = rootType.GetSchema(),
			Table = rootType.GetTableName(),
			Name = target.GetConstraintName(),
			Columns = GetColumns(target.Properties),
			PrincipalSchema = rootType2.GetSchema(),
			PrincipalTable = rootType2.GetTableName(),
			PrincipalColumns = GetColumns(target.PrincipalKey.Properties),
			OnDelete = ToReferentialAction(target.DeleteBehavior)
		};
		addForeignKeyOperation.AddAnnotations(MigrationsAnnotations.For(target));
		CreateTableOperation createTableOperation = diffContext.FindCreate(rootType);
		if (createTableOperation != null)
		{
			createTableOperation.ForeignKeys.Add(addForeignKeyOperation);
		}
		else
		{
			yield return addForeignKeyOperation;
		}
	}

	protected virtual IEnumerable<MigrationOperation> Remove([JetBrains.Annotations.NotNull] IForeignKey source, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		IEntityType rootType = source.DeclaringEntityType.GetRootType();
		if (diffContext.FindDrop(rootType) == null)
		{
			DropForeignKeyOperation dropForeignKeyOperation = new DropForeignKeyOperation
			{
				Schema = rootType.GetSchema(),
				Table = rootType.GetTableName(),
				Name = source.GetConstraintName()
			};
			dropForeignKeyOperation.AddAnnotations(MigrationsAnnotations.ForRemove(source));
			yield return dropForeignKeyOperation;
		}
	}

	protected virtual IEnumerable<MigrationOperation> Diff([JetBrains.Annotations.NotNull] IEnumerable<IIndex> source, [JetBrains.Annotations.NotNull] IEnumerable<IIndex> target, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		return DiffCollection(source, target, diffContext, Diff, Add, Remove, (IIndex s, IIndex t, DiffContext c) => string.Equals(s.GetName(), t.GetName(), StringComparison.OrdinalIgnoreCase) && IndexStructureEquals(s, t, c), (IIndex s, IIndex t, DiffContext c) => IndexStructureEquals(s, t, c));
	}

	private bool IndexStructureEquals(IIndex source, IIndex target, DiffContext diffContext)
	{
		if (source.IsUnique == target.IsUnique && source.GetFilter() == target.GetFilter() && !HasDifferences(MigrationsAnnotations.For(source), MigrationsAnnotations.For(target)))
		{
			return source.Properties.Select((IProperty p) => p.GetColumnName()).SequenceEqual(target.Properties.Select((IProperty p) => diffContext.FindSource(p)?.GetColumnName()));
		}
		return false;
	}

	protected virtual IEnumerable<MigrationOperation> Diff([JetBrains.Annotations.NotNull] IIndex source, [JetBrains.Annotations.NotNull] IIndex target, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		IEntityType rootType = target.DeclaringEntityType.GetRootType();
		string name = source.GetName();
		string name2 = target.GetName();
		if (name != name2)
		{
			yield return new RenameIndexOperation
			{
				Schema = rootType.GetSchema(),
				Table = rootType.GetTableName(),
				Name = name,
				NewName = name2
			};
		}
	}

	protected virtual IEnumerable<MigrationOperation> Add([JetBrains.Annotations.NotNull] IIndex target, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		IEntityType rootType = target.DeclaringEntityType.GetRootType();
		CreateIndexOperation createIndexOperation = new CreateIndexOperation
		{
			Name = target.GetName(),
			Schema = rootType.GetSchema(),
			Table = rootType.GetTableName(),
			Columns = GetColumns(target.Properties),
			IsUnique = target.IsUnique,
			Filter = target.GetFilter()
		};
		createIndexOperation.AddAnnotations(MigrationsAnnotations.For(target));
		yield return createIndexOperation;
	}

	protected virtual IEnumerable<MigrationOperation> Remove([JetBrains.Annotations.NotNull] IIndex source, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		IEntityType rootType = source.DeclaringEntityType.GetRootType();
		DropIndexOperation dropIndexOperation = new DropIndexOperation
		{
			Name = source.GetName(),
			Schema = rootType.GetSchema(),
			Table = rootType.GetTableName()
		};
		dropIndexOperation.AddAnnotations(MigrationsAnnotations.ForRemove(source));
		yield return dropIndexOperation;
	}

	protected virtual IEnumerable<MigrationOperation> Diff([JetBrains.Annotations.NotNull] IEnumerable<ICheckConstraint> source, [JetBrains.Annotations.NotNull] IEnumerable<ICheckConstraint> target, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		return DiffCollection(source, target, diffContext, Diff, Add, Remove, (ICheckConstraint s, ICheckConstraint t, DiffContext c) => c.FindSourceTable(s.EntityType) == c.FindSource(c.FindTargetTable(t.EntityType)) && string.Equals(s.Name, t.Name, StringComparison.OrdinalIgnoreCase) && string.Equals(s.Sql, t.Sql, StringComparison.OrdinalIgnoreCase));
	}

	protected virtual IEnumerable<MigrationOperation> Diff([JetBrains.Annotations.NotNull] ICheckConstraint source, [JetBrains.Annotations.NotNull] ICheckConstraint target, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		return Enumerable.Empty<MigrationOperation>();
	}

	protected virtual IEnumerable<MigrationOperation> Add([JetBrains.Annotations.NotNull] ICheckConstraint target, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		IEntityType rootType = target.EntityType.GetRootType();
		CreateCheckConstraintOperation createCheckConstraintOperation = new CreateCheckConstraintOperation
		{
			Name = target.Name,
			Sql = target.Sql,
			Schema = rootType.GetSchema(),
			Table = rootType.GetTableName()
		};
		createCheckConstraintOperation.Sql = target.Sql;
		createCheckConstraintOperation.AddAnnotations(MigrationsAnnotations.For(target));
		yield return createCheckConstraintOperation;
	}

	protected virtual IEnumerable<MigrationOperation> Remove([JetBrains.Annotations.NotNull] ICheckConstraint source, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		IEntityType rootType = source.EntityType.GetRootType();
		DropCheckConstraintOperation dropCheckConstraintOperation = new DropCheckConstraintOperation
		{
			Name = source.Name,
			Schema = rootType.GetSchema(),
			Table = rootType.GetTableName()
		};
		dropCheckConstraintOperation.AddAnnotations(MigrationsAnnotations.ForRemove(source));
		yield return dropCheckConstraintOperation;
	}

	protected virtual IEnumerable<MigrationOperation> Diff([JetBrains.Annotations.NotNull] IEnumerable<ISequence> source, [JetBrains.Annotations.NotNull] IEnumerable<ISequence> target, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		return DiffCollection(source, target, diffContext, Diff, Add, Remove, (ISequence s, ISequence t, DiffContext c) => string.Equals(s.Schema, t.Schema, StringComparison.OrdinalIgnoreCase) && string.Equals(s.Name, t.Name, StringComparison.OrdinalIgnoreCase) && s.ClrType == t.ClrType, (ISequence s, ISequence t, DiffContext c) => string.Equals(s.Name, t.Name, StringComparison.OrdinalIgnoreCase) && s.ClrType == t.ClrType);
	}

	protected virtual IEnumerable<MigrationOperation> Diff([JetBrains.Annotations.NotNull] ISequence source, [JetBrains.Annotations.NotNull] ISequence target, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		if (source.Schema != target.Schema || source.Name != target.Name)
		{
			yield return new RenameSequenceOperation
			{
				Schema = source.Schema,
				Name = source.Name,
				NewSchema = target.Schema,
				NewName = target.Name
			};
		}
		if (source.StartValue != target.StartValue)
		{
			yield return new RestartSequenceOperation
			{
				Schema = target.Schema,
				Name = target.Name,
				StartValue = target.StartValue
			};
		}
		List<IAnnotation> list = MigrationsAnnotations.For(source).ToList();
		List<IAnnotation> list2 = MigrationsAnnotations.For(target).ToList();
		if (source.IncrementBy != target.IncrementBy || source.MaxValue != target.MaxValue || source.MinValue != target.MinValue || source.IsCyclic != target.IsCyclic || HasDifferences(list, list2))
		{
			AlterSequenceOperation alterSequenceOperation = new AlterSequenceOperation
			{
				Schema = target.Schema,
				Name = target.Name
			};
			Initialize(alterSequenceOperation, target, list2);
			Initialize(alterSequenceOperation.OldSequence, source, list);
			yield return alterSequenceOperation;
		}
	}

	protected virtual IEnumerable<MigrationOperation> Add([JetBrains.Annotations.NotNull] ISequence target, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		CreateSequenceOperation sequenceOperation = new CreateSequenceOperation
		{
			Schema = target.Schema,
			Name = target.Name,
			ClrType = target.ClrType,
			StartValue = target.StartValue
		};
		yield return Initialize(sequenceOperation, target, MigrationsAnnotations.For(target));
	}

	protected virtual IEnumerable<MigrationOperation> Remove([JetBrains.Annotations.NotNull] ISequence source, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		DropSequenceOperation dropSequenceOperation = new DropSequenceOperation
		{
			Schema = source.Schema,
			Name = source.Name
		};
		dropSequenceOperation.AddAnnotations(MigrationsAnnotations.ForRemove(source));
		yield return dropSequenceOperation;
	}

	private static SequenceOperation Initialize(SequenceOperation sequenceOperation, ISequence sequence, IEnumerable<IAnnotation> migrationsAnnotations)
	{
		sequenceOperation.IncrementBy = sequence.IncrementBy;
		sequenceOperation.MinValue = sequence.MinValue;
		sequenceOperation.MaxValue = sequence.MaxValue;
		sequenceOperation.IsCyclic = sequence.IsCyclic;
		sequenceOperation.AddAnnotations(migrationsAnnotations);
		return sequenceOperation;
	}

	protected virtual void TrackData([JetBrains.Annotations.CanBeNull] IModel source, [JetBrains.Annotations.CanBeNull] IModel target)
	{
		if (target == null)
		{
			_targetUpdateAdapter = null;
			return;
		}
		_targetUpdateAdapter = UpdateAdapterFactory.CreateStandalone(target);
		_targetUpdateAdapter.CascadeDeleteTiming = CascadeTiming.Never;
		foreach (IEntityType entityType in target.GetEntityTypes())
		{
			foreach (IDictionary<string, object> seedDatum in entityType.GetSeedData())
			{
				_targetUpdateAdapter.CreateEntry(seedDatum, entityType).EntityState = EntityState.Added;
			}
		}
		if (source == null)
		{
			_sourceUpdateAdapter = null;
			return;
		}
		_sourceUpdateAdapter = UpdateAdapterFactory.CreateStandalone(source);
		_sourceUpdateAdapter.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;
		foreach (IEntityType entityType2 in source.GetEntityTypes())
		{
			foreach (IDictionary<string, object> seedDatum2 in entityType2.GetSeedData())
			{
				IUpdateEntry updateEntry = _sourceUpdateAdapter.CreateEntry(seedDatum2, entityType2);
				updateEntry.EntityState = EntityState.Added;
				updateEntry.EntityState = EntityState.Unchanged;
			}
		}
	}

	protected virtual void DiffData([JetBrains.Annotations.NotNull] TableMapping source, [JetBrains.Annotations.NotNull] TableMapping target, [JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(target, "target");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(diffContext, "diffContext");
		SharedTableEntryMap<List<IUpdateEntry>> sharedTableEntryMap = SharedTableEntryMap<List<IUpdateEntry>>.CreateSharedTableEntryMapFactory(target.EntityTypes, _targetUpdateAdapter, target.Name, target.Schema)((string t, string s, IComparer<IUpdateEntry> c) => new List<IUpdateEntry>());
		foreach (IEntityType entityType in target.EntityTypes)
		{
			foreach (IDictionary<string, object> seedDatum in entityType.GetSeedData())
			{
				IUpdateEntry entry = GetEntry(seedDatum, entityType, _targetUpdateAdapter);
				sharedTableEntryMap.GetOrAddValue(entry).Add(entry);
			}
		}
		List<IKey> list = (from k in target.EntityTypes.SelectMany(EntityTypeExtensions.GetDeclaredKeys)
			where k.IsPrimaryKey()
			select k).ToList();
		Dictionary<IEntityType, Dictionary<IKey, List<(IProperty, ValueConverter, ValueConverter)>>> dictionary = new Dictionary<IEntityType, Dictionary<IKey, List<(IProperty, ValueConverter, ValueConverter)>>>();
		foreach (IEntityType entityType2 in source.EntityTypes)
		{
			foreach (IKey item4 in list)
			{
				List<(IProperty, ValueConverter, ValueConverter)> list2 = new List<(IProperty, ValueConverter, ValueConverter)>();
				foreach (IProperty property3 in item4.Properties)
				{
					IProperty property = diffContext.FindSource(property3);
					if (property == null)
					{
						break;
					}
					foreach (IProperty property4 in entityType2.GetProperties())
					{
						if (property4.GetColumnName() == property.GetColumnName())
						{
							ValueConverter valueConverter = GetValueConverter(property);
							ValueConverter valueConverter2 = GetValueConverter(property3);
							if (!(property4.ClrType != property3.ClrType) || (valueConverter != null && !(valueConverter.ProviderClrType != property3.ClrType)) || (valueConverter2 != null && !(valueConverter2.ProviderClrType != property4.ClrType)))
							{
								list2.Add((property4, valueConverter, valueConverter2));
								break;
							}
						}
					}
				}
				if (list2.Count == item4.Properties.Count)
				{
					dictionary.GetOrAddNew(entityType2)[item4] = list2;
				}
			}
		}
		SharedTableEntryMap<EntryMapping> sharedTableEntryMap2 = SharedTableEntryMap<EntryMapping>.CreateSharedTableEntryMapFactory(source.EntityTypes, _sourceUpdateAdapter, source.Name, source.Schema)((string t, string s, IComparer<IUpdateEntry> c) => new EntryMapping());
		_sharedTableEntryMaps.Add(sharedTableEntryMap2);
		foreach (IEntityType entityType3 in source.EntityTypes)
		{
			foreach (IDictionary<string, object> seedDatum2 in entityType3.GetSeedData())
			{
				IUpdateEntry entry2 = GetEntry(seedDatum2, entityType3, _sourceUpdateAdapter);
				EntryMapping orAddValue = sharedTableEntryMap2.GetOrAddValue(entry2);
				orAddValue.SourceEntries.Add(entry2);
				if (!dictionary.TryGetValue(entityType3, out var value))
				{
					orAddValue.RecreateRow = true;
					continue;
				}
				foreach (IKey item5 in list)
				{
					if (!value.TryGetValue(item5, out var value2))
					{
						continue;
					}
					object[] array = new object[value2.Count];
					for (int i = 0; i < value2.Count; i++)
					{
						(IProperty, ValueConverter, ValueConverter) tuple = value2[i];
						IProperty item = tuple.Item1;
						ValueConverter item2 = tuple.Item2;
						ValueConverter item3 = tuple.Item3;
						object currentValue = entry2.GetCurrentValue(item);
						array[i] = ((item5.Properties[i].ClrType != item.ClrType) ? ((item2 != null) ? item2.ConvertToProvider(currentValue) : item3.ConvertFromProvider(currentValue)) : currentValue);
					}
					IUpdateEntry updateEntry = _targetUpdateAdapter.TryGetEntry(item5, array);
					if (updateEntry == null)
					{
						continue;
					}
					foreach (IUpdateEntry item6 in sharedTableEntryMap.GetOrAddValue(updateEntry))
					{
						if (!orAddValue.TargetEntries.Add(item6))
						{
							continue;
						}
						foreach (IProperty property5 in item6.EntityType.GetProperties())
						{
							if (property5.GetAfterSaveBehavior() == PropertySaveBehavior.Save)
							{
								item6.SetOriginalValue(property5, property5.ClrType.GetDefaultValue());
							}
						}
						item6.EntityState = EntityState.Unchanged;
					}
					if (entry2.EntityState == EntityState.Deleted || orAddValue.RecreateRow)
					{
						orAddValue.RecreateRow = true;
						continue;
					}
					foreach (IProperty property6 in updateEntry.EntityType.GetProperties())
					{
						if (property6.ValueGenerated != 0 && property6.ValueGenerated != ValueGenerated.OnAdd)
						{
							continue;
						}
						IProperty property2 = diffContext.FindSource(property6);
						if (property2 == null || !entityType3.GetProperties().Contains(property2))
						{
							continue;
						}
						object currentValue2 = entry2.GetCurrentValue(property2);
						object currentValue3 = updateEntry.GetCurrentValue(property6);
						ValueComparer valueComparer = property6.GetValueComparer() ?? property2.GetValueComparer() ?? property6.FindTypeMapping()?.Comparer ?? property2.FindTypeMapping()?.Comparer;
						if (!(property2.ClrType.UnwrapNullableType() == property6.ClrType.UnwrapNullableType()) || valueComparer == null || valueComparer.Equals(currentValue2, currentValue3))
						{
							ValueConverter valueConverter3 = GetValueConverter(property2);
							ValueConverter valueConverter4 = GetValueConverter(property6);
							object obj = ((valueConverter3 == null) ? currentValue2 : valueConverter3.ConvertToProvider(currentValue2));
							object obj2 = ((valueConverter4 == null) ? currentValue3 : valueConverter4.ConvertToProvider(currentValue3));
							Type type = valueConverter3?.ProviderClrType ?? valueConverter4?.ProviderClrType;
							if (type != null && !type.IsNullableType())
							{
								object defaultValue = type.GetDefaultValue();
								if (obj == null)
								{
									obj = defaultValue;
								}
								if (obj2 == null)
								{
									obj2 = defaultValue;
								}
							}
							bool flag = obj?.GetType().UnwrapNullableType() != obj2?.GetType().UnwrapNullableType();
							if (!flag && type != null)
							{
								valueComparer = TypeMappingSource.FindMapping(type)?.Comparer;
								flag = ((valueComparer != null) ? (!valueComparer.Equals(obj, obj2)) : (!object.Equals(obj, obj2)));
							}
							if (!flag)
							{
								updateEntry.SetOriginalValue(property6, updateEntry.GetCurrentValue(property6));
								continue;
							}
						}
						if (property6.GetAfterSaveBehavior() != 0)
						{
							orAddValue.RecreateRow = true;
							break;
						}
						updateEntry.SetPropertyModified(property6);
					}
				}
			}
		}
	}

	private static IUpdateEntry GetEntry(IDictionary<string, object> sourceSeed, IEntityType sourceEntityType, IUpdateAdapter updateAdapter)
	{
		IKey key = sourceEntityType.FindPrimaryKey();
		object[] array = new object[key.Properties.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = sourceSeed[key.Properties[i].Name];
		}
		return updateAdapter.TryGetEntry(key, array);
	}

	protected virtual IEnumerable<MigrationOperation> GetDataOperations([JetBrains.Annotations.NotNull] DiffContext diffContext)
	{
		foreach (SharedTableEntryMap<EntryMapping> sharedTableEntryMap in _sharedTableEntryMaps)
		{
			foreach (EntryMapping value in sharedTableEntryMap.Values)
			{
				if (!value.RecreateRow && value.TargetEntries.Count != 0)
				{
					continue;
				}
				foreach (IUpdateEntry sourceEntry in value.SourceEntries)
				{
					sourceEntry.EntityState = EntityState.Deleted;
					_sourceUpdateAdapter.CascadeDelete(sourceEntry, sourceEntry.EntityType.GetReferencingForeignKeys().Where(delegate(IForeignKey fk)
					{
						DeleteBehavior? deleteBehavior = diffContext.FindTarget(fk)?.DeleteBehavior;
						return deleteBehavior.HasValue && deleteBehavior != DeleteBehavior.ClientNoAction;
					}));
				}
			}
		}
		foreach (SharedTableEntryMap<EntryMapping> sharedTableEntryMap2 in _sharedTableEntryMaps)
		{
			foreach (EntryMapping value2 in sharedTableEntryMap2.Values)
			{
				if (!value2.SourceEntries.Any((IUpdateEntry e) => e.EntityState == EntityState.Deleted))
				{
					continue;
				}
				foreach (IUpdateEntry targetEntry in value2.TargetEntries)
				{
					targetEntry.EntityState = EntityState.Added;
				}
				foreach (IUpdateEntry sourceEntry2 in value2.SourceEntries)
				{
					sourceEntry2.EntityState = EntityState.Deleted;
				}
			}
		}
		_sharedTableEntryMaps.Clear();
		IUpdateAdapter[] array = new IUpdateAdapter[2] { _sourceUpdateAdapter, _targetUpdateAdapter };
		foreach (IUpdateAdapter updateAdapter in array)
		{
			if (updateAdapter == null)
			{
				continue;
			}
			updateAdapter.DetectChanges();
			IList<IUpdateEntry> entriesToSave = updateAdapter.GetEntriesToSave();
			if (entriesToSave == null || entriesToSave.Count == 0)
			{
				continue;
			}
			IEnumerable<ModificationCommandBatch> enumerable = new CommandBatchPreparer(CommandBatchPreparerDependencies).BatchCommands(entriesToSave, updateAdapter);
			foreach (ModificationCommandBatch item in enumerable)
			{
				InsertDataOperation batchInsertOperation = null;
				foreach (ModificationCommand c in item.ModificationCommands)
				{
					if (c.EntityState == EntityState.Added)
					{
						if (batchInsertOperation != null)
						{
							if (batchInsertOperation.Table == c.TableName && batchInsertOperation.Schema == c.Schema && batchInsertOperation.Columns.SequenceEqual(from col in c.ColumnModifications
								where col.IsKey || col.IsWrite
								select col.ColumnName))
							{
								batchInsertOperation.Values = AddToMultidimensionalArray(c.ColumnModifications.Where((ColumnModification col) => col.IsKey || col.IsWrite).Select(GetValue).ToList(), batchInsertOperation.Values);
								continue;
							}
							yield return batchInsertOperation;
						}
						batchInsertOperation = new InsertDataOperation
						{
							Schema = c.Schema,
							Table = c.TableName,
							Columns = (from col in c.ColumnModifications
								where col.IsKey || col.IsWrite
								select col.ColumnName).ToArray(),
							Values = ToMultidimensionalArray(c.ColumnModifications.Where((ColumnModification col) => col.IsKey || col.IsWrite).Select(GetValue).ToList())
						};
						continue;
					}
					if (c.EntityState == EntityState.Modified)
					{
						if (batchInsertOperation != null)
						{
							yield return batchInsertOperation;
							batchInsertOperation = null;
						}
						yield return new UpdateDataOperation
						{
							Schema = c.Schema,
							Table = c.TableName,
							KeyColumns = (from col in c.ColumnModifications
								where col.IsKey
								select col.ColumnName).ToArray(),
							KeyValues = ToMultidimensionalArray(c.ColumnModifications.Where((ColumnModification col) => col.IsKey).Select(GetValue).ToList()),
							Columns = (from col in c.ColumnModifications
								where col.IsWrite
								select col.ColumnName).ToArray(),
							Values = ToMultidimensionalArray(c.ColumnModifications.Where((ColumnModification col) => col.IsWrite).Select(GetValue).ToList())
						};
						continue;
					}
					if (batchInsertOperation != null)
					{
						yield return batchInsertOperation;
						batchInsertOperation = null;
					}
					if (c.Entries.All((IUpdateEntry e) => diffContext.FindDrop(e.EntityType.GetRootType()) == null))
					{
						yield return new DeleteDataOperation
						{
							Schema = c.Schema,
							Table = c.TableName,
							KeyColumns = (from col in c.ColumnModifications
								where col.IsKey
								select col.ColumnName).ToArray(),
							KeyValues = ToMultidimensionalArray(c.ColumnModifications.Where((ColumnModification col) => col.IsKey).Select(GetValue).ToArray())
						};
					}
				}
				if (batchInsertOperation != null)
				{
					yield return batchInsertOperation;
				}
			}
		}
	}

	private object GetValue(ColumnModification columnModification)
	{
		ValueConverter valueConverter = GetValueConverter(columnModification.Property);
		object obj = (columnModification.UseCurrentValueParameter ? columnModification.Value : columnModification.OriginalValue);
		if (valueConverter == null)
		{
			return obj;
		}
		return valueConverter.ConvertToProvider(obj);
	}

	protected virtual IEnumerable<MigrationOperation> DiffCollection<T>([JetBrains.Annotations.NotNull] IEnumerable<T> sources, [JetBrains.Annotations.NotNull] IEnumerable<T> targets, [JetBrains.Annotations.NotNull] DiffContext diffContext, [JetBrains.Annotations.NotNull] Func<T, T, DiffContext, IEnumerable<MigrationOperation>> diff, [JetBrains.Annotations.NotNull] Func<T, DiffContext, IEnumerable<MigrationOperation>> add, [JetBrains.Annotations.NotNull] Func<T, DiffContext, IEnumerable<MigrationOperation>> remove, [JetBrains.Annotations.NotNull] params Func<T, T, DiffContext, bool>[] predicates)
	{
		List<T> sourceList = sources.ToList();
		List<T> targetList = targets.ToList();
		List<(T, T)> list = new List<(T, T)>();
		foreach (Func<T, T, DiffContext, bool> func in predicates)
		{
			for (int num = sourceList.Count - 1; num >= 0; num--)
			{
				T val = sourceList[num];
				for (int num2 = targetList.Count - 1; num2 >= 0; num2--)
				{
					T val2 = targetList[num2];
					if (func(val, val2, diffContext))
					{
						sourceList.RemoveAt(num);
						targetList.RemoveAt(num2);
						list.Add((val, val2));
						diffContext.AddMapping(val, val2);
						break;
					}
				}
			}
		}
		foreach (var (arg, arg2) in list)
		{
			foreach (MigrationOperation item in diff(arg, arg2, diffContext))
			{
				yield return item;
			}
		}
		foreach (T item2 in sourceList)
		{
			foreach (MigrationOperation item3 in remove(item2, diffContext))
			{
				yield return item3;
			}
		}
		foreach (T item4 in targetList)
		{
			foreach (MigrationOperation item5 in add(item4, diffContext))
			{
				yield return item5;
			}
		}
	}

	protected virtual string[] GetColumns([JetBrains.Annotations.NotNull] IEnumerable<IProperty> properties)
	{
		return properties.Select((IProperty p) => p.GetColumnName()).ToArray();
	}

	protected virtual bool HasDifferences([JetBrains.Annotations.NotNull] IEnumerable<IAnnotation> source, [JetBrains.Annotations.NotNull] IEnumerable<IAnnotation> target)
	{
		List<IAnnotation> list = new List<IAnnotation>(target);
		foreach (IAnnotation annotation in source)
		{
			int num = list.FindIndex((IAnnotation a) => a.Name == annotation.Name && StructuralComparisons.StructuralEqualityComparer.Equals(a.Value, annotation.Value));
			if (num == -1)
			{
				return true;
			}
			list.RemoveAt(num);
		}
		return list.Count != 0;
	}

	protected virtual IEnumerable<string> GetSchemas([JetBrains.Annotations.NotNull] IModel model)
	{
		return (from s in (from t in model.GetRootEntityTypes()
				where !t.IsIgnoredByMigrations()
				select t.GetSchema()).Concat(from s in model.GetSequences()
				select s.Schema)
			where !string.IsNullOrEmpty(s)
			select s).Distinct();
	}

	protected virtual object GetDefaultValue([JetBrains.Annotations.NotNull] Type type)
	{
		if (!(type == typeof(string)))
		{
			if (!type.IsArray)
			{
				return type.UnwrapNullableType().GetDefaultValue();
			}
			return Array.CreateInstance(type.GetElementType(), 0);
		}
		return string.Empty;
	}

	private object GetDefaultValue(IProperty property)
	{
		object defaultValue = property.GetDefaultValue();
		ValueConverter valueConverter = GetValueConverter(property);
		if (valueConverter == null)
		{
			return defaultValue;
		}
		return valueConverter.ConvertToProvider(defaultValue);
	}

	private ValueConverter GetValueConverter(IProperty property)
	{
		return TypeMappingSource.GetMapping(property).Converter;
	}

	private static ReferentialAction ToReferentialAction(DeleteBehavior deleteBehavior)
	{
		switch (deleteBehavior)
		{
		case DeleteBehavior.SetNull:
			return ReferentialAction.SetNull;
		case DeleteBehavior.Cascade:
			return ReferentialAction.Cascade;
		case DeleteBehavior.NoAction:
		case DeleteBehavior.ClientNoAction:
			return ReferentialAction.NoAction;
		default:
			return ReferentialAction.Restrict;
		}
	}

	private static object[,] ToMultidimensionalArray(IReadOnlyList<object> values)
	{
		object[,] array = new object[1, values.Count];
		for (int i = 0; i < values.Count; i++)
		{
			array[0, i] = values[i];
		}
		return array;
	}

	private static object[,] AddToMultidimensionalArray(IReadOnlyList<object> values, object[,] array)
	{
		int length = array.GetLength(0);
		int length2 = array.GetLength(1);
		object[,] array2 = new object[length + 1, length2];
		for (int i = 0; i < length; i++)
		{
			Array.Copy(array, i * length2, array2, i * length2, length2);
		}
		for (int j = 0; j < values.Count; j++)
		{
			array2[length, j] = values[j];
		}
		return array2;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Update;

public class ModificationCommand
{
	private class ColumnValuePropagator
	{
		private bool _write;

		private object _originalValue;

		private object _currentValue;

		public ColumnModification ColumnModification { get; set; }

		public void RecordValue(IProperty property, IUpdateEntry entry)
		{
			switch (entry.EntityState)
			{
			case EntityState.Modified:
				if (!_write && entry.IsModified(property))
				{
					_write = true;
					_currentValue = entry.GetCurrentValue(property);
				}
				break;
			case EntityState.Added:
			{
				_currentValue = entry.GetCurrentValue(property);
				ValueComparer valueComparer = property.GetValueComparer() ?? property.FindTypeMapping()?.Comparer;
				if (valueComparer == null)
				{
					_write = !object.Equals(_originalValue, _currentValue);
				}
				else
				{
					_write = !valueComparer.Equals(_originalValue, _currentValue);
				}
				break;
			}
			case EntityState.Deleted:
				_originalValue = entry.GetOriginalValue(property);
				if (!_write && !property.IsPrimaryKey())
				{
					_write = true;
					_currentValue = null;
				}
				break;
			}
		}

		public bool TryPropagate(IProperty property, InternalEntityEntry entry)
		{
			if (_write && (entry.EntityState == EntityState.Unchanged || (entry.EntityState == EntityState.Modified && !entry.IsModified(property)) || (entry.EntityState == EntityState.Added && object.Equals(_originalValue, entry.GetCurrentValue(property)))))
			{
				entry[property] = _currentValue;
				return false;
			}
			return _write;
		}
	}

	private readonly Func<string> _generateParameterName;

	private readonly bool _sensitiveLoggingEnabled;

	private readonly IComparer<IUpdateEntry> _comparer;

	private readonly List<IUpdateEntry> _entries = new List<IUpdateEntry>();

	private IReadOnlyList<ColumnModification> _columnModifications;

	private bool _requiresResultPropagation;

	private bool _mainEntryAdded;

	public virtual string TableName { get; }

	public virtual string Schema { get; }

	public virtual IReadOnlyList<IUpdateEntry> Entries => _entries;

	public virtual EntityState EntityState
	{
		get
		{
			if (_mainEntryAdded)
			{
				IUpdateEntry updateEntry = _entries[0];
				if (updateEntry.SharedIdentityEntry == null)
				{
					return updateEntry.EntityState;
				}
				return EntityState.Modified;
			}
			return EntityState.Modified;
		}
	}

	public virtual IReadOnlyList<ColumnModification> ColumnModifications => NonCapturingLazyInitializer.EnsureInitialized(ref _columnModifications, this, (ModificationCommand command) => command.GenerateColumnModifications());

	public virtual bool RequiresResultPropagation
	{
		get
		{
			_ = ColumnModifications;
			return _requiresResultPropagation;
		}
	}

	public ModificationCommand([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema, [JetBrains.Annotations.NotNull] Func<string> generateParameterName, bool sensitiveLoggingEnabled, [JetBrains.Annotations.CanBeNull] IComparer<IUpdateEntry> comparer)
		: this(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name"), schema, null, sensitiveLoggingEnabled)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(generateParameterName, "generateParameterName");
		_generateParameterName = generateParameterName;
		_comparer = comparer;
	}

	public ModificationCommand([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema, [JetBrains.Annotations.CanBeNull] IReadOnlyList<ColumnModification> columnModifications, bool sensitiveLoggingEnabled)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(name, "name");
		TableName = name;
		Schema = schema;
		_columnModifications = columnModifications;
		_sensitiveLoggingEnabled = sensitiveLoggingEnabled;
	}

	[Obsolete("Use AddEntry with most parameters")]
	public virtual void AddEntry([JetBrains.Annotations.NotNull] IUpdateEntry entry)
	{
		AddEntry(entry, mainEntry: false);
	}

	public virtual void AddEntry([JetBrains.Annotations.NotNull] IUpdateEntry entry, bool mainEntry)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entry, "entry");
		EntityState entityState = entry.EntityState;
		if ((uint)(entityState - 2) > 2u)
		{
			throw new ArgumentException(RelationalStrings.ModificationCommandInvalidEntityState(entry.EntityState));
		}
		if (mainEntry)
		{
			for (int i = 0; i < _entries.Count; i++)
			{
				ValidateState(entry, _entries[i]);
			}
			_mainEntryAdded = true;
			_entries.Insert(0, entry);
		}
		else
		{
			if (_mainEntryAdded)
			{
				ValidateState(_entries[0], entry);
			}
			_entries.Add(entry);
		}
		_columnModifications = null;
	}

	private void ValidateState(IUpdateEntry mainEntry, IUpdateEntry entry)
	{
		EntityState entityState = ((mainEntry.SharedIdentityEntry == null) ? mainEntry.EntityState : EntityState.Modified);
		if (entityState == EntityState.Modified)
		{
			return;
		}
		EntityState entityState2 = ((entry.SharedIdentityEntry == null) ? entry.EntityState : EntityState.Modified);
		if (entityState == entityState2)
		{
			return;
		}
		if (_sensitiveLoggingEnabled)
		{
			throw new InvalidOperationException(RelationalStrings.ConflictingRowUpdateTypesSensitive(entry.EntityType.DisplayName(), entry.BuildCurrentValuesString(entry.EntityType.FindPrimaryKey().Properties), entityState2, mainEntry.EntityType.DisplayName(), mainEntry.BuildCurrentValuesString(mainEntry.EntityType.FindPrimaryKey().Properties), entityState));
		}
		throw new InvalidOperationException(RelationalStrings.ConflictingRowUpdateTypes(entry.EntityType.DisplayName(), entityState2, mainEntry.EntityType.DisplayName(), entityState));
	}

	private IReadOnlyList<ColumnModification> GenerateColumnModifications()
	{
		EntityState entityState = EntityState;
		bool flag = entityState == EntityState.Added;
		bool flag2 = entityState == EntityState.Modified;
		List<ColumnModification> list = new List<ColumnModification>();
		Dictionary<string, ColumnValuePropagator> dictionary = null;
		if (_entries.Count > 1 || (_entries.Count == 1 && _entries[0].SharedIdentityEntry != null))
		{
			dictionary = new Dictionary<string, ColumnValuePropagator>();
			if (_comparer != null)
			{
				_entries.Sort(_comparer);
			}
			foreach (IUpdateEntry entry in _entries)
			{
				if (entry.SharedIdentityEntry != null)
				{
					InitializeSharedColumns(entry.SharedIdentityEntry, flag2, dictionary);
				}
				InitializeSharedColumns(entry, flag2, dictionary);
			}
		}
		foreach (IUpdateEntry entry2 in _entries)
		{
			bool flag3 = flag2 && (entry2.EntityState == EntityState.Deleted || entry2.EntityState == EntityState.Added);
			foreach (IProperty property in entry2.EntityType.GetProperties())
			{
				bool flag4 = property.IsPrimaryKey();
				bool isConcurrencyToken = property.IsConcurrencyToken;
				bool flag5 = !flag && (flag4 || isConcurrencyToken);
				bool flag6 = entry2.IsStoreGenerated(property);
				string columnName = property.GetColumnName();
				ColumnValuePropagator columnValuePropagator = dictionary?[columnName];
				bool flag7 = false;
				if (!flag6)
				{
					if (flag)
					{
						flag7 = property.GetBeforeSaveBehavior() == PropertySaveBehavior.Save;
					}
					else if ((flag2 && property.GetAfterSaveBehavior() == PropertySaveBehavior.Save) || (!flag4 && flag3))
					{
						flag7 = columnValuePropagator?.TryPropagate(property, (InternalEntityEntry)entry2) ?? entry2.IsModified(property);
					}
				}
				if (!(flag6 || flag7 || flag5))
				{
					continue;
				}
				if (flag6)
				{
					_requiresResultPropagation = true;
				}
				ColumnModification columnModification = new ColumnModification(entry2, property, _generateParameterName, flag6, flag7, flag4, flag5, isConcurrencyToken, _sensitiveLoggingEnabled);
				if (columnValuePropagator != null)
				{
					if (columnValuePropagator.ColumnModification != null)
					{
						columnValuePropagator.ColumnModification.AddSharedColumnModification(columnModification);
						continue;
					}
					columnValuePropagator.ColumnModification = columnModification;
				}
				list.Add(columnModification);
			}
		}
		return list;
	}

	private static void InitializeSharedColumns(IUpdateEntry entry, bool updating, Dictionary<string, ColumnValuePropagator> columnMap)
	{
		foreach (IProperty property in entry.EntityType.GetProperties())
		{
			string columnName = property.GetColumnName();
			if (!columnMap.TryGetValue(columnName, out var value))
			{
				value = new ColumnValuePropagator();
				columnMap.Add(columnName, value);
			}
			if (updating)
			{
				value.RecordValue(property, entry);
			}
		}
	}

	public virtual void PropagateResults(ValueBuffer valueBuffer)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(valueBuffer, "valueBuffer");
		int num = 0;
		foreach (ColumnModification item in ColumnModifications.Where((ColumnModification o) => o.IsRead))
		{
			item.Value = valueBuffer[num++];
		}
	}
}

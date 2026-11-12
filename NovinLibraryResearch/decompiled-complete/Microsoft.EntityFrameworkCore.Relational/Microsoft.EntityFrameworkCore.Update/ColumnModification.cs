using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Update;

public class ColumnModification
{
	private string _parameterName;

	private string _originalParameterName;

	private readonly Func<string> _generateParameterName;

	private readonly object _originalValue;

	private object _value;

	private readonly bool _useParameters;

	private readonly bool _sensitiveLoggingEnabled;

	private List<ColumnModification> _sharedColumnModifications;

	public virtual IUpdateEntry Entry { get; }

	public virtual IProperty Property { get; }

	public virtual bool IsRead { get; }

	public virtual bool IsWrite { get; }

	public virtual bool IsCondition { get; }

	public virtual bool IsConcurrencyToken { get; }

	public virtual bool IsKey { get; }

	public virtual bool UseOriginalValueParameter
	{
		get
		{
			if (_useParameters)
			{
				return IsCondition;
			}
			return false;
		}
	}

	public virtual bool UseCurrentValueParameter
	{
		get
		{
			if (_useParameters)
			{
				return IsWrite;
			}
			return false;
		}
	}

	public virtual string ParameterName => _parameterName ?? (_parameterName = (UseCurrentValueParameter ? _generateParameterName() : null));

	public virtual string OriginalParameterName => _originalParameterName ?? (_originalParameterName = (UseOriginalValueParameter ? _generateParameterName() : null));

	public virtual string ColumnName { get; }

	public virtual object OriginalValue
	{
		get
		{
			if (Entry != null)
			{
				if (Entry.SharedIdentityEntry != null)
				{
					return Entry.SharedIdentityEntry.GetOriginalValue(Property);
				}
				return Entry.GetOriginalValue(Property);
			}
			return _originalValue;
		}
	}

	public virtual object Value
	{
		get
		{
			if (Entry != null)
			{
				if (Entry.EntityState != EntityState.Deleted)
				{
					return Entry.GetCurrentValue(Property);
				}
				return null;
			}
			return _value;
		}
		[param: JetBrains.Annotations.CanBeNull]
		set
		{
			if (Entry == null)
			{
				_value = value;
				return;
			}
			Entry.SetStoreGeneratedValue(Property, value);
			if (_sharedColumnModifications == null)
			{
				return;
			}
			foreach (ColumnModification sharedColumnModification in _sharedColumnModifications)
			{
				sharedColumnModification.Value = value;
			}
		}
	}

	public ColumnModification([JetBrains.Annotations.NotNull] IUpdateEntry entry, [JetBrains.Annotations.NotNull] IProperty property, [JetBrains.Annotations.NotNull] Func<string> generateParameterName, bool isRead, bool isWrite, bool isKey, bool isCondition, bool isConcurrencyToken, bool sensitiveLoggingEnabled)
		: this(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property").GetColumnName(), null, null, property, isRead, isWrite, isKey, isCondition, sensitiveLoggingEnabled)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entry, "entry");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(generateParameterName, "generateParameterName");
		Entry = entry;
		IsConcurrencyToken = isConcurrencyToken;
		_generateParameterName = generateParameterName;
		_useParameters = true;
	}

	public ColumnModification([JetBrains.Annotations.NotNull] string columnName, [JetBrains.Annotations.CanBeNull] object originalValue, [JetBrains.Annotations.CanBeNull] object value, [JetBrains.Annotations.CanBeNull] IProperty property, bool isRead, bool isWrite, bool isKey, bool isCondition, bool sensitiveLoggingEnabled)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(columnName, "columnName");
		ColumnName = columnName;
		_originalValue = originalValue;
		_value = value;
		Property = property;
		IsRead = isRead;
		IsWrite = isWrite;
		IsKey = isKey;
		IsCondition = isCondition;
		_sensitiveLoggingEnabled = sensitiveLoggingEnabled;
	}

	public virtual void AddSharedColumnModification([JetBrains.Annotations.NotNull] ColumnModification modification)
	{
		if (_sharedColumnModifications == null)
		{
			_sharedColumnModifications = new List<ColumnModification>();
		}
		if (UseCurrentValueParameter && !StructuralComparisons.StructuralEqualityComparer.Equals(Value, modification.Value))
		{
			if (_sensitiveLoggingEnabled)
			{
				throw new InvalidOperationException(RelationalStrings.ConflictingRowValuesSensitive(Entry.EntityType.DisplayName(), modification.Entry.EntityType.DisplayName(), Entry.BuildCurrentValuesString(Entry.EntityType.FindPrimaryKey().Properties), Entry.BuildCurrentValuesString(new IProperty[1] { Property }), modification.Entry.BuildCurrentValuesString(new IProperty[1] { modification.Property }), new IProperty[1] { Property }.FormatColumns()));
			}
			throw new InvalidOperationException(RelationalStrings.ConflictingRowValues(Entry.EntityType.DisplayName(), modification.Entry.EntityType.DisplayName(), new IProperty[1] { Property }.Format(), new IProperty[1] { modification.Property }.Format(), new IProperty[1] { Property }.FormatColumns()));
		}
		if (UseOriginalValueParameter && !StructuralComparisons.StructuralEqualityComparer.Equals(OriginalValue, modification.OriginalValue))
		{
			if (_sensitiveLoggingEnabled)
			{
				throw new InvalidOperationException(RelationalStrings.ConflictingOriginalRowValuesSensitive(Entry.EntityType.DisplayName(), modification.Entry.EntityType.DisplayName(), Entry.BuildCurrentValuesString(Entry.EntityType.FindPrimaryKey().Properties), Entry.BuildOriginalValuesString(new IProperty[1] { Property }), modification.Entry.BuildOriginalValuesString(new IProperty[1] { modification.Property }), new IProperty[1] { Property }.FormatColumns()));
			}
			throw new InvalidOperationException(RelationalStrings.ConflictingOriginalRowValues(Entry.EntityType.DisplayName(), modification.Entry.EntityType.DisplayName(), new IProperty[1] { Property }.Format(), new IProperty[1] { modification.Property }.Format(), new IProperty[1] { Property }.FormatColumns()));
		}
		_sharedColumnModifications.Add(modification);
	}
}

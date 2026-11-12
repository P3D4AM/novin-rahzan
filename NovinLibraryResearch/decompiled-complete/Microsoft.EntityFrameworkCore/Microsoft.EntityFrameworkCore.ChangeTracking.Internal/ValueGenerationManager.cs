using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class ValueGenerationManager : IValueGenerationManager
{
	private readonly IValueGeneratorSelector _valueGeneratorSelector;

	private readonly IKeyPropagator _keyPropagator;

	private readonly IDiagnosticsLogger<DbLoggerCategory.ChangeTracking> _logger;

	private readonly ILoggingOptions _loggingOptions;

	public ValueGenerationManager([JetBrains.Annotations.NotNull] IValueGeneratorSelector valueGeneratorSelector, [JetBrains.Annotations.NotNull] IKeyPropagator keyPropagator, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.ChangeTracking> logger, [JetBrains.Annotations.NotNull] ILoggingOptions loggingOptions)
	{
		_valueGeneratorSelector = valueGeneratorSelector;
		_keyPropagator = keyPropagator;
		_logger = logger;
		_loggingOptions = loggingOptions;
	}

	public virtual InternalEntityEntry Propagate(InternalEntityEntry entry)
	{
		InternalEntityEntry internalEntityEntry = null;
		foreach (IProperty item in FindPropagatingProperties(entry))
		{
			InternalEntityEntry internalEntityEntry2 = _keyPropagator.PropagateValue(entry, item);
			if (internalEntityEntry == null)
			{
				internalEntityEntry = internalEntityEntry2;
			}
		}
		return internalEntityEntry;
	}

	public virtual void Generate(InternalEntityEntry entry)
	{
		EntityEntry entry2 = new EntityEntry(entry);
		foreach (IProperty item in FindGeneratingProperties(entry))
		{
			ValueGenerator valueGenerator = GetValueGenerator(entry, item);
			object generatedValue = valueGenerator.Next(entry2);
			bool generatesTemporaryValues = valueGenerator.GeneratesTemporaryValues;
			Log(entry, item, generatedValue, generatesTemporaryValues);
			SetGeneratedValue(entry, item, generatedValue, generatesTemporaryValues);
		}
	}

	private void Log(InternalEntityEntry entry, IProperty property, object generatedValue, bool temporary)
	{
		if (_loggingOptions.IsSensitiveDataLoggingEnabled)
		{
			_logger.ValueGeneratedSensitive(entry, property, generatedValue, temporary);
		}
		else
		{
			_logger.ValueGenerated(entry, property, generatedValue, temporary);
		}
	}

	public virtual async Task GenerateAsync(InternalEntityEntry entry, CancellationToken cancellationToken = default(CancellationToken))
	{
		EntityEntry entityEntry = new EntityEntry(entry);
		foreach (IProperty property in FindGeneratingProperties(entry))
		{
			ValueGenerator valueGenerator = GetValueGenerator(entry, property);
			object generatedValue = await valueGenerator.NextAsync(entityEntry, cancellationToken);
			bool generatesTemporaryValues = valueGenerator.GeneratesTemporaryValues;
			Log(entry, property, generatedValue, generatesTemporaryValues);
			SetGeneratedValue(entry, property, generatedValue, generatesTemporaryValues);
		}
	}

	private static IEnumerable<IProperty> FindPropagatingProperties(InternalEntityEntry entry)
	{
		foreach (Property property in ((EntityType)entry.EntityType).GetProperties())
		{
			if (property.IsForeignKey() && entry.HasDefaultValue(property))
			{
				yield return property;
			}
		}
	}

	private static IEnumerable<IProperty> FindGeneratingProperties(InternalEntityEntry entry)
	{
		foreach (Property property in ((EntityType)entry.EntityType).GetProperties())
		{
			if (property.RequiresValueGenerator() && entry.HasDefaultValue(property))
			{
				yield return property;
			}
		}
	}

	private ValueGenerator GetValueGenerator(InternalEntityEntry entry, IProperty property)
	{
		return _valueGeneratorSelector.Select(property, property.IsKey() ? property.DeclaringEntityType : entry.EntityType);
	}

	public virtual bool MayGetTemporaryValue(IProperty property, IEntityType entityType)
	{
		if (property.RequiresValueGenerator())
		{
			return _valueGeneratorSelector.Select(property, entityType).GeneratesTemporaryValues;
		}
		return false;
	}

	private static void SetGeneratedValue(InternalEntityEntry entry, IProperty property, object generatedValue, bool isTemporary)
	{
		if (generatedValue != null)
		{
			if (isTemporary)
			{
				entry.SetTemporaryValue(property, generatedValue);
			}
			else
			{
				entry[property] = generatedValue;
			}
		}
	}
}

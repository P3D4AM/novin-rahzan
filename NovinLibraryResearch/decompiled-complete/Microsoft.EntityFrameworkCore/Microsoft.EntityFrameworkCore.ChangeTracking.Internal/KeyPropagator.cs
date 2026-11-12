using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class KeyPropagator : IKeyPropagator
{
	private readonly IValueGeneratorSelector _valueGeneratorSelector;

	public KeyPropagator([JetBrains.Annotations.NotNull] IValueGeneratorSelector valueGeneratorSelector)
	{
		_valueGeneratorSelector = valueGeneratorSelector;
	}

	public virtual InternalEntityEntry PropagateValue(InternalEntityEntry entry, IProperty property)
	{
		InternalEntityEntry internalEntityEntry = TryPropagateValue(entry, property);
		if (internalEntityEntry == null && property.IsKey())
		{
			ValueGenerator valueGenerator = TryGetValueGenerator(property);
			if (valueGenerator != null)
			{
				object value = valueGenerator.Next(new EntityEntry(entry));
				if (valueGenerator.GeneratesTemporaryValues)
				{
					entry.SetTemporaryValue(property, value);
					return internalEntityEntry;
				}
				entry[property] = value;
			}
		}
		return internalEntityEntry;
	}

	public virtual async Task<InternalEntityEntry> PropagateValueAsync(InternalEntityEntry entry, IProperty property, CancellationToken cancellationToken = default(CancellationToken))
	{
		InternalEntityEntry principalEntry = TryPropagateValue(entry, property);
		if (principalEntry == null && property.IsKey())
		{
			ValueGenerator valueGenerator = TryGetValueGenerator(property);
			if (valueGenerator != null)
			{
				object value = await valueGenerator.NextAsync(new EntityEntry(entry), cancellationToken);
				if (valueGenerator.GeneratesTemporaryValues)
				{
					entry.SetTemporaryValue(property, value);
				}
				else
				{
					entry[property] = value;
				}
			}
		}
		return principalEntry;
	}

	private static InternalEntityEntry TryPropagateValue(InternalEntityEntry entry, IProperty property)
	{
		IEntityType entityType = entry.EntityType;
		IStateManager stateManager = entry.StateManager;
		foreach (IForeignKey foreignKey in entityType.GetForeignKeys())
		{
			for (int i = 0; i < foreignKey.Properties.Count; i++)
			{
				if (property != foreignKey.Properties[i])
				{
					continue;
				}
				object obj = ((foreignKey.DependentToPrincipal == null) ? null : entry[foreignKey.DependentToPrincipal]);
				InternalEntityEntry internalEntityEntry = null;
				if (obj != null)
				{
					internalEntityEntry = stateManager.GetOrCreateEntry(obj, foreignKey.PrincipalEntityType);
				}
				else if (foreignKey.PrincipalToDependent != null)
				{
					foreach (Tuple<INavigation, InternalEntityEntry> recordedReferrer in stateManager.GetRecordedReferrers(entry.Entity, clear: false))
					{
						if (recordedReferrer.Item1 == foreignKey.PrincipalToDependent)
						{
							internalEntityEntry = recordedReferrer.Item2;
							break;
						}
					}
				}
				if (internalEntityEntry == null)
				{
					break;
				}
				IProperty property2 = foreignKey.PrincipalKey.Properties[i];
				object value = internalEntityEntry[property2];
				if (property2.ClrType.IsDefaultValue(value))
				{
					break;
				}
				if (internalEntityEntry.HasTemporaryValue(property2))
				{
					entry.SetTemporaryValue(property, value);
				}
				else
				{
					entry[property] = value;
				}
				return internalEntityEntry;
			}
		}
		return null;
	}

	private ValueGenerator TryGetValueGenerator(IProperty property)
	{
		IProperty generationProperty = property.GetGenerationProperty();
		if (generationProperty == null)
		{
			return null;
		}
		return _valueGeneratorSelector.Select(generationProperty, generationProperty.DeclaringEntityType);
	}
}

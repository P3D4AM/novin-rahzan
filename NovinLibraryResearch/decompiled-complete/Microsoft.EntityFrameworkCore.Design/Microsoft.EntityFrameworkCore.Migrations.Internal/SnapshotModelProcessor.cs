using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Migrations.Internal;

public class SnapshotModelProcessor : ISnapshotModelProcessor
{
	private readonly IOperationReporter _operationReporter;

	private readonly HashSet<string> _relationalNames;

	public SnapshotModelProcessor([JetBrains.Annotations.NotNull] IOperationReporter operationReporter)
	{
		_operationReporter = operationReporter;
		_relationalNames = new HashSet<string>(from p in typeof(RelationalAnnotationNames).GetTypeInfo().GetRuntimeFields()
			where p.Name != "Prefix"
			select ((string)p.GetValue(null)).Substring("Relational:".Length - 1));
	}

	public virtual IModel Process(IModel model)
	{
		if (model == null)
		{
			return null;
		}
		string productVersion = model.GetProductVersion();
		if (productVersion != null)
		{
			ProcessElement(model, productVersion);
			foreach (IEntityType entityType in model.GetEntityTypes())
			{
				ProcessElement(entityType, productVersion);
				ProcessCollection(entityType.GetProperties(), productVersion);
				ProcessCollection(entityType.GetKeys(), productVersion);
				ProcessCollection(entityType.GetIndexes(), productVersion);
				foreach (IForeignKey foreignKey in entityType.GetForeignKeys())
				{
					ProcessElement(foreignKey, productVersion);
					ProcessElement(foreignKey.DependentToPrincipal, productVersion);
					ProcessElement(foreignKey.PrincipalToDependent, productVersion);
				}
			}
		}
		if (!(model is IMutableModel model2))
		{
			return model;
		}
		return model2.FinalizeModel();
	}

	private void ProcessCollection(IEnumerable<IAnnotatable> metadata, string version)
	{
		foreach (IAnnotatable metadatum in metadata)
		{
			ProcessElement(metadatum, version);
		}
	}

	private void ProcessElement(IEntityType entityType, string version)
	{
		ProcessElement((IAnnotatable)entityType, version);
		if ((version.StartsWith("2.0", StringComparison.Ordinal) || version.StartsWith("2.1", StringComparison.Ordinal)) && entityType is IMutableEntityType entityType2 && !entityType.IsOwned())
		{
			UpdateOwnedTypes(entityType2);
		}
	}

	private void ProcessElement(IAnnotatable metadata, string version)
	{
		if (!version.StartsWith("1.", StringComparison.Ordinal) || !(metadata is IMutableAnnotatable mutableAnnotatable))
		{
			return;
		}
		foreach (IAnnotation item in mutableAnnotatable.GetAnnotations().ToList())
		{
			int num = item.Name.IndexOf(':');
			if (num <= 0)
			{
				continue;
			}
			string text = item.Name.Substring(num);
			if (_relationalNames.Contains(text))
			{
				mutableAnnotatable.RemoveAnnotation(item.Name);
				string name = "Relational" + text;
				IAnnotation annotation = mutableAnnotatable.FindAnnotation(name);
				if (annotation == null)
				{
					mutableAnnotatable[name] = item.Value;
				}
				else if (!object.Equals(annotation.Value, item.Value))
				{
					_operationReporter.WriteWarning(DesignStrings.MultipleAnnotationConflict(text.Substring(1)));
				}
			}
		}
	}

	private void UpdateOwnedTypes(IMutableEntityType entityType)
	{
		foreach (IMutableForeignKey item in (from fk in entityType.GetDeclaredReferencingForeignKeys()
			where fk.IsOwnership && fk.IsUnique
			select fk).ToList())
		{
			IMutableEntityType declaringEntityType = item.DeclaringEntityType;
			IMutableKey principalKey = item.PrincipalKey;
			if (!principalKey.IsPrimaryKey())
			{
				item.SetProperties((IReadOnlyList<Property>)item.Properties, item.PrincipalEntityType.FindPrimaryKey());
				if (principalKey is IConventionKey conventionKey && conventionKey.GetConfigurationSource() == ConfigurationSource.Convention)
				{
					principalKey.DeclaringEntityType.RemoveKey(principalKey);
				}
				foreach (IMutableProperty property in principalKey.Properties)
				{
					if (property is IConventionProperty conventionProperty && conventionProperty.GetConfigurationSource() == ConfigurationSource.Convention)
					{
						property.DeclaringEntityType.RemoveProperty(property);
					}
				}
			}
			if (declaringEntityType.FindPrimaryKey() == null)
			{
				foreach (IMutableProperty property2 in item.Properties)
				{
					if (property2.IsNullable)
					{
						property2.IsNullable = false;
					}
				}
				declaringEntityType.SetPrimaryKey(item.Properties);
			}
			UpdateOwnedTypes(declaringEntityType);
		}
	}
}

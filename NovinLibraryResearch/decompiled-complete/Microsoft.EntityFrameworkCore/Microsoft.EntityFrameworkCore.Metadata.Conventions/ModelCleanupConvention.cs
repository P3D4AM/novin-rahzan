using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class ModelCleanupConvention : IModelFinalizedConvention, IConvention
{
	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	public ModelCleanupConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
	{
		Dependencies = dependencies;
	}

	public virtual void ProcessModelFinalized(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
	{
		RemoveEntityTypesUnreachableByNavigations(modelBuilder, context);
		RemoveNavigationlessForeignKeys(modelBuilder);
		RemoveModelBuildingAnnotations(modelBuilder);
	}

	private void RemoveEntityTypesUnreachableByNavigations(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
	{
		IConventionModel metadata = modelBuilder.Metadata;
		IReadOnlyList<IConventionEntityType> roots = GetRoots(metadata, ConfigurationSource.DataAnnotation);
		using (context.DelayConventions())
		{
			foreach (IConventionEntityType unreachableVertex in new ModelNavigationsGraphAdapter(metadata).GetUnreachableVertices(roots))
			{
				modelBuilder.HasNoEntityType(unreachableVertex, fromDataAnnotation: true);
			}
		}
	}

	private IReadOnlyList<IConventionEntityType> GetRoots(IConventionModel model, ConfigurationSource configurationSource)
	{
		List<IConventionEntityType> list = new List<IConventionEntityType>();
		foreach (IConventionEntityType entityType in model.GetEntityTypes())
		{
			if (entityType.GetConfigurationSource().Overrides(configurationSource))
			{
				list.Add(entityType);
			}
		}
		return list;
	}

	private void RemoveNavigationlessForeignKeys(IConventionModelBuilder modelBuilder)
	{
		foreach (IConventionEntityType entityType in modelBuilder.Metadata.GetEntityTypes())
		{
			foreach (IConventionForeignKey item in entityType.GetDeclaredForeignKeys().ToList())
			{
				if (item.PrincipalToDependent == null && item.DependentToPrincipal == null)
				{
					entityType.Builder.HasNoRelationship(item, fromDataAnnotation: true);
				}
			}
		}
	}

	private void RemoveModelBuildingAnnotations(IConventionModelBuilder modelBuilder)
	{
		foreach (IConventionEntityType entityType in modelBuilder.Metadata.GetEntityTypes())
		{
			entityType.RemoveAnnotation("RelationshipDiscoveryConvention:AmbiguousNavigations");
			entityType.RemoveAnnotation("RelationshipDiscoveryConvention:NavigationCandidates");
		}
	}
}

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

public abstract class RelationalConventionSetBuilder : ProviderConventionSetBuilder
{
	protected virtual RelationalConventionSetBuilderDependencies RelationalDependencies { get; }

	protected RelationalConventionSetBuilder([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalConventionSetBuilderDependencies relationalDependencies)
		: base(dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(relationalDependencies, "relationalDependencies");
		RelationalDependencies = relationalDependencies;
	}

	public override ConventionSet CreateConventionSet()
	{
		ConventionSet conventionSet = base.CreateConventionSet();
		RelationalColumnAttributeConvention item = new RelationalColumnAttributeConvention(Dependencies, RelationalDependencies);
		conventionSet.PropertyAddedConventions.Add(item);
		TableNameFromDbSetConvention item2 = new TableNameFromDbSetConvention(Dependencies, RelationalDependencies);
		conventionSet.EntityTypeAddedConventions.Add(new RelationalTableAttributeConvention(Dependencies, RelationalDependencies));
		conventionSet.EntityTypeAddedConventions.Add(item2);
		ValueGenerationConvention valueGenerationConvention = new RelationalValueGenerationConvention(Dependencies, RelationalDependencies);
		ReplaceConvention(conventionSet.EntityTypeBaseTypeChangedConventions, valueGenerationConvention);
		conventionSet.EntityTypeBaseTypeChangedConventions.Add(item2);
		conventionSet.EntityTypeAnnotationChangedConventions.Add((RelationalValueGenerationConvention)valueGenerationConvention);
		ReplaceConvention(conventionSet.EntityTypePrimaryKeyChangedConventions, valueGenerationConvention);
		ReplaceConvention(conventionSet.ForeignKeyAddedConventions, valueGenerationConvention);
		ReplaceConvention(conventionSet.ForeignKeyRemovedConventions, valueGenerationConvention);
		conventionSet.PropertyFieldChangedConventions.Add(item);
		StoreGenerationConvention storeGenerationConvention = new StoreGenerationConvention(Dependencies, RelationalDependencies);
		conventionSet.PropertyAnnotationChangedConventions.Add(storeGenerationConvention);
		conventionSet.PropertyAnnotationChangedConventions.Add((RelationalValueGenerationConvention)valueGenerationConvention);
		RelationalDbFunctionAttributeConvention item3 = new RelationalDbFunctionAttributeConvention(Dependencies, RelationalDependencies);
		conventionSet.ModelInitializedConventions.Add(item3);
		conventionSet.ModelAnnotationChangedConventions.Add(item3);
		ConventionSet.AddBefore(conventionSet.ModelFinalizedConventions, storeGenerationConvention, typeof(ValidatingConvention));
		ConventionSet.AddBefore(conventionSet.ModelFinalizedConventions, new SharedTableConvention(Dependencies, RelationalDependencies), typeof(ValidatingConvention));
		ConventionSet.AddBefore(conventionSet.ModelFinalizedConventions, new DbFunctionTypeMappingConvention(Dependencies, RelationalDependencies), typeof(ValidatingConvention));
		ReplaceConvention(conventionSet.ModelFinalizedConventions, (QueryFilterDefiningQueryRewritingConvention)new RelationalQueryFilterDefiningQueryRewritingConvention(Dependencies, RelationalDependencies));
		return conventionSet;
	}
}

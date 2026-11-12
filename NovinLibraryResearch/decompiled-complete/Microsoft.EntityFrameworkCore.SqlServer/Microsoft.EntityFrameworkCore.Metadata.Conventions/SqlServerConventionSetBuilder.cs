using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class SqlServerConventionSetBuilder : RelationalConventionSetBuilder
{
	private readonly ISqlGenerationHelper _sqlGenerationHelper;

	public SqlServerConventionSetBuilder([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalConventionSetBuilderDependencies relationalDependencies, [JetBrains.Annotations.NotNull] ISqlGenerationHelper sqlGenerationHelper)
		: base(dependencies, relationalDependencies)
	{
		_sqlGenerationHelper = sqlGenerationHelper;
	}

	public override ConventionSet CreateConventionSet()
	{
		ConventionSet conventionSet = base.CreateConventionSet();
		SqlServerValueGenerationStrategyConvention sqlServerValueGenerationStrategyConvention = new SqlServerValueGenerationStrategyConvention(Dependencies, RelationalDependencies);
		conventionSet.ModelInitializedConventions.Add(sqlServerValueGenerationStrategyConvention);
		conventionSet.ModelInitializedConventions.Add(new RelationalMaxIdentifierLengthConvention(128, Dependencies, RelationalDependencies));
		ValueGenerationConvention valueGenerationConvention = new SqlServerValueGenerationConvention(Dependencies, RelationalDependencies);
		SqlServerIndexConvention item = new SqlServerIndexConvention(Dependencies, RelationalDependencies, _sqlGenerationHelper);
		ReplaceConvention(conventionSet.EntityTypeBaseTypeChangedConventions, valueGenerationConvention);
		conventionSet.EntityTypeBaseTypeChangedConventions.Add(item);
		SqlServerMemoryOptimizedTablesConvention item2 = new SqlServerMemoryOptimizedTablesConvention(Dependencies, RelationalDependencies);
		conventionSet.EntityTypeAnnotationChangedConventions.Add(item2);
		ReplaceConvention(conventionSet.EntityTypeAnnotationChangedConventions, (RelationalValueGenerationConvention)valueGenerationConvention);
		ReplaceConvention(conventionSet.EntityTypePrimaryKeyChangedConventions, valueGenerationConvention);
		conventionSet.KeyAddedConventions.Add(item2);
		ReplaceConvention(conventionSet.ForeignKeyAddedConventions, valueGenerationConvention);
		ReplaceConvention(conventionSet.ForeignKeyRemovedConventions, valueGenerationConvention);
		conventionSet.IndexAddedConventions.Add(item2);
		conventionSet.IndexAddedConventions.Add(item);
		conventionSet.IndexUniquenessChangedConventions.Add(item);
		conventionSet.IndexAnnotationChangedConventions.Add(item);
		conventionSet.PropertyNullabilityChangedConventions.Add(item);
		StoreGenerationConvention newConvention = new SqlServerStoreGenerationConvention(Dependencies, RelationalDependencies);
		conventionSet.PropertyAnnotationChangedConventions.Add(item);
		ReplaceConvention(conventionSet.PropertyAnnotationChangedConventions, newConvention);
		ReplaceConvention(conventionSet.PropertyAnnotationChangedConventions, (RelationalValueGenerationConvention)valueGenerationConvention);
		ConventionSet.AddBefore(conventionSet.ModelFinalizedConventions, sqlServerValueGenerationStrategyConvention, typeof(ValidatingConvention));
		ReplaceConvention(conventionSet.ModelFinalizedConventions, newConvention);
		return conventionSet;
	}

	public static ConventionSet Build()
	{
		using IServiceScope serviceScope = ServiceProviderServiceExtensions.GetRequiredService<IServiceScopeFactory>(new ServiceCollection().AddEntityFrameworkSqlServer().AddDbContext<DbContext>(delegate(IServiceProvider p, DbContextOptionsBuilder o)
		{
			o.UseSqlServer("Server=.").UseInternalServiceProvider(p);
		}).BuildServiceProvider()).CreateScope();
		using DbContext context = ServiceProviderServiceExtensions.GetService<DbContext>(serviceScope.ServiceProvider);
		return ConventionSet.CreateConventionSet(context);
	}
}

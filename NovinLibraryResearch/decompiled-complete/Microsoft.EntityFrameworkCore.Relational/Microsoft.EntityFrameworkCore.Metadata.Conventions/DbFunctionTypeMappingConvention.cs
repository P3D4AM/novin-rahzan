using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class DbFunctionTypeMappingConvention : IModelFinalizedConvention, IConvention
{
	private readonly IRelationalTypeMappingSource _relationalTypeMappingSource;

	public DbFunctionTypeMappingConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalConventionSetBuilderDependencies relationalDependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(relationalDependencies, "relationalDependencies");
		_relationalTypeMappingSource = (IRelationalTypeMappingSource)dependencies.TypeMappingSource;
	}

	public virtual void ProcessModelFinalized(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(context, "context");
		foreach (IConventionDbFunction dbFunction in modelBuilder.Metadata.GetDbFunctions())
		{
			RelationalTypeMapping typeMapping = ((!string.IsNullOrEmpty(dbFunction.StoreType)) ? _relationalTypeMappingSource.FindMapping(dbFunction.StoreType) : _relationalTypeMappingSource.FindMapping(dbFunction.MethodInfo.ReturnType));
			dbFunction.Builder.HasTypeMapping(typeMapping);
			foreach (IConventionDbFunctionParameter parameter in dbFunction.Parameters)
			{
				typeMapping = ((!string.IsNullOrEmpty(parameter.StoreType)) ? _relationalTypeMappingSource.FindMapping(parameter.StoreType) : _relationalTypeMappingSource.FindMapping(parameter.ClrType));
				parameter.Builder.HasTypeMapping(typeMapping);
			}
		}
	}
}

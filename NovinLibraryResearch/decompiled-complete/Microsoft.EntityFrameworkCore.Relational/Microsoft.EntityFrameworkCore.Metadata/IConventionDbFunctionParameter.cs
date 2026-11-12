using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IConventionDbFunctionParameter : IDbFunctionParameter
{
	new IConventionDbFunction Function { get; }

	IConventionDbFunctionParameterBuilder Builder { get; }

	void SetStoreType([JetBrains.Annotations.CanBeNull] string storeType, bool fromDataAnnotation = false);

	ConfigurationSource? GetStoreTypeConfigurationSource();

	void SetTypeMapping([JetBrains.Annotations.CanBeNull] RelationalTypeMapping typeMapping, bool fromDataAnnotation = false);

	ConfigurationSource? GetTypeMappingConfigurationSource();
}

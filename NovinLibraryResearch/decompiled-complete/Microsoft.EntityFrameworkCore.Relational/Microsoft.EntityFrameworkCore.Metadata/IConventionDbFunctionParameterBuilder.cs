using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IConventionDbFunctionParameterBuilder
{
	IConventionDbFunctionParameter Metadata { get; }

	IConventionDbFunctionParameterBuilder HasStoreType([JetBrains.Annotations.CanBeNull] string storeType, bool fromDataAnnotation = false);

	bool CanSetStoreType([JetBrains.Annotations.CanBeNull] string storeType, bool fromDataAnnotation = false);

	IConventionDbFunctionParameterBuilder HasTypeMapping([JetBrains.Annotations.CanBeNull] RelationalTypeMapping typeMapping, bool fromDataAnnotation = false);

	bool CanSetTypeMapping([JetBrains.Annotations.CanBeNull] RelationalTypeMapping typeMapping, bool fromDataAnnotation = false);
}

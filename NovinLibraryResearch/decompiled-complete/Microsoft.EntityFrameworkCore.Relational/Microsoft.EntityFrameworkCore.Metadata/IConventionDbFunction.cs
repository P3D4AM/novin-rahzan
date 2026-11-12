using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IConventionDbFunction : IDbFunction
{
	new IConventionModel Model { get; }

	IConventionDbFunctionBuilder Builder { get; }

	new IReadOnlyList<IConventionDbFunctionParameter> Parameters { get; }

	ConfigurationSource GetConfigurationSource();

	void SetName([JetBrains.Annotations.CanBeNull] string name, bool fromDataAnnotation = false);

	ConfigurationSource? GetNameConfigurationSource();

	void SetSchema([JetBrains.Annotations.CanBeNull] string schema, bool fromDataAnnotation = false);

	ConfigurationSource? GetSchemaConfigurationSource();

	void SetStoreType([JetBrains.Annotations.CanBeNull] string storeType, bool fromDataAnnotation = false);

	ConfigurationSource? GetStoreTypeConfigurationSource();

	void SetTypeMapping([JetBrains.Annotations.CanBeNull] RelationalTypeMapping typeMapping, bool fromDataAnnotation = false);

	ConfigurationSource? GetTypeMappingConfigurationSource();

	void SetTranslation([JetBrains.Annotations.CanBeNull] Func<IReadOnlyCollection<SqlExpression>, SqlExpression> translation, bool fromDataAnnotation = false);

	ConfigurationSource? GetTranslationConfigurationSource();
}

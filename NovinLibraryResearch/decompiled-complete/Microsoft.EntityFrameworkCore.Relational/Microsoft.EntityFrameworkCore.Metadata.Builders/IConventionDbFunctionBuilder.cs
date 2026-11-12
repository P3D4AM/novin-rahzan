using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

public interface IConventionDbFunctionBuilder
{
	IConventionDbFunction Metadata { get; }

	IConventionDbFunctionBuilder HasName([JetBrains.Annotations.CanBeNull] string name, bool fromDataAnnotation = false);

	bool CanSetName([JetBrains.Annotations.CanBeNull] string name, bool fromDataAnnotation = false);

	IConventionDbFunctionBuilder HasSchema([JetBrains.Annotations.CanBeNull] string schema, bool fromDataAnnotation = false);

	bool CanSetSchema([JetBrains.Annotations.CanBeNull] string schema, bool fromDataAnnotation = false);

	IConventionDbFunctionBuilder HasStoreType([JetBrains.Annotations.CanBeNull] string storeType, bool fromDataAnnotation = false);

	bool CanSetStoreType([JetBrains.Annotations.CanBeNull] string storeType, bool fromDataAnnotation = false);

	IConventionDbFunctionBuilder HasTypeMapping([JetBrains.Annotations.CanBeNull] RelationalTypeMapping typeMapping, bool fromDataAnnotation = false);

	bool CanSetTypeMapping([JetBrains.Annotations.CanBeNull] RelationalTypeMapping typeMapping, bool fromDataAnnotation = false);

	IConventionDbFunctionBuilder HasTranslation([JetBrains.Annotations.CanBeNull] Func<IReadOnlyCollection<SqlExpression>, SqlExpression> translation, bool fromDataAnnotation = false);

	bool CanSetTranslation([JetBrains.Annotations.CanBeNull] Func<IReadOnlyCollection<SqlExpression>, SqlExpression> translation, bool fromDataAnnotation = false);
}

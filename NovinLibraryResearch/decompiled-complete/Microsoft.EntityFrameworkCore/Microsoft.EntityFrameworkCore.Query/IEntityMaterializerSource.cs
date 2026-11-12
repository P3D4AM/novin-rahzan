using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Query;

public interface IEntityMaterializerSource
{
	Expression CreateReadValueExpression([JetBrains.Annotations.NotNull] Expression valueBuffer, [JetBrains.Annotations.NotNull] Type type, int index, [JetBrains.Annotations.CanBeNull] IPropertyBase property);

	Expression CreateMaterializeExpression([JetBrains.Annotations.NotNull] IEntityType entityType, [JetBrains.Annotations.NotNull] string entityInstanceName, [JetBrains.Annotations.NotNull] Expression materializationExpression);

	Func<MaterializationContext, object> GetMaterializer([JetBrains.Annotations.NotNull] IEntityType entityType);
}

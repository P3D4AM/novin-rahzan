using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

public class AlterOperationBuilder<TOperation> : OperationBuilder<TOperation> where TOperation : MigrationOperation, IAlterMigrationOperation
{
	public AlterOperationBuilder([JetBrains.Annotations.NotNull] TOperation operation)
		: base(operation)
	{
	}

	public new virtual AlterOperationBuilder<TOperation> Annotation([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] object value)
	{
		return (AlterOperationBuilder<TOperation>)base.Annotation(name, value);
	}

	public virtual AlterOperationBuilder<TOperation> OldAnnotation([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] object value)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(value, "value");
		Operation.OldAnnotations.AddAnnotation(name, value);
		return this;
	}
}

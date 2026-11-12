using System.ComponentModel;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

public class OperationBuilder<TOperation> : IInfrastructure<TOperation> where TOperation : MigrationOperation
{
	protected virtual TOperation Operation { get; }

	TOperation IInfrastructure<TOperation>.Instance => Operation;

	public OperationBuilder([JetBrains.Annotations.NotNull] TOperation operation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
		Operation = operation;
	}

	public virtual OperationBuilder<TOperation> Annotation([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] object value)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(value, "value");
		Operation.AddAnnotation(name, value);
		return this;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override string ToString()
	{
		return base.ToString();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}

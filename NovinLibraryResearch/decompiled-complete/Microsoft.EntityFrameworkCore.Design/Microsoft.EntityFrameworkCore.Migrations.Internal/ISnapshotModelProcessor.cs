using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Migrations.Internal;

public interface ISnapshotModelProcessor
{
	IModel Process([JetBrains.Annotations.CanBeNull] IModel model);
}

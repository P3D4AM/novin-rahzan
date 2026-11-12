using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Migrations.Design;

public interface ICSharpSnapshotGenerator
{
	void Generate([JetBrains.Annotations.NotNull] string builderName, [JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IndentedStringBuilder stringBuilder);
}

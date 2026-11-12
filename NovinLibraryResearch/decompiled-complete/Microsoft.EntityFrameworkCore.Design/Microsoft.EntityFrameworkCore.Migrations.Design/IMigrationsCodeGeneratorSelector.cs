using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Migrations.Design;

public interface IMigrationsCodeGeneratorSelector
{
	IMigrationsCodeGenerator Select([JetBrains.Annotations.CanBeNull] string language);
}

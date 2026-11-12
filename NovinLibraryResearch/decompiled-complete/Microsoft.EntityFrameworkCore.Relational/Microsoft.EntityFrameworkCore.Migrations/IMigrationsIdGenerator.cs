using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Migrations;

public interface IMigrationsIdGenerator
{
	string GenerateId([JetBrains.Annotations.NotNull] string name);

	string GetName([JetBrains.Annotations.NotNull] string id);

	bool IsValidId([JetBrains.Annotations.NotNull] string value);
}

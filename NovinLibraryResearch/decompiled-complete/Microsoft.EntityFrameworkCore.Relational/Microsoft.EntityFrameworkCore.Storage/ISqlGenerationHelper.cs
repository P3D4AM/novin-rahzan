using System.Text;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage;

public interface ISqlGenerationHelper
{
	string StatementTerminator { get; }

	string BatchTerminator { get; }

	string SingleLineCommentToken { get; }

	string GenerateParameterName([JetBrains.Annotations.NotNull] string name);

	void GenerateParameterName([JetBrains.Annotations.NotNull] StringBuilder builder, [JetBrains.Annotations.NotNull] string name);

	string GenerateParameterNamePlaceholder([JetBrains.Annotations.NotNull] string name);

	void GenerateParameterNamePlaceholder([JetBrains.Annotations.NotNull] StringBuilder builder, [JetBrains.Annotations.NotNull] string name);

	string DelimitIdentifier([JetBrains.Annotations.NotNull] string identifier);

	void DelimitIdentifier([JetBrains.Annotations.NotNull] StringBuilder builder, [JetBrains.Annotations.NotNull] string identifier);

	string DelimitIdentifier([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema);

	void DelimitIdentifier([JetBrains.Annotations.NotNull] StringBuilder builder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema);

	string GenerateComment([JetBrains.Annotations.NotNull] string text);
}

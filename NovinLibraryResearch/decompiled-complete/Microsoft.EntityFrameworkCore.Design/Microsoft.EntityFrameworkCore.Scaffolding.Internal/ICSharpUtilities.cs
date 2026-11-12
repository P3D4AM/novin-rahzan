using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Scaffolding.Internal;

public interface ICSharpUtilities
{
	string GenerateCSharpIdentifier([JetBrains.Annotations.NotNull] string identifier, [JetBrains.Annotations.CanBeNull] ICollection<string> existingIdentifiers, [JetBrains.Annotations.CanBeNull] Func<string, string> singularizePluralizer);

	string GenerateCSharpIdentifier([JetBrains.Annotations.NotNull] string identifier, [JetBrains.Annotations.CanBeNull] ICollection<string> existingIdentifiers, [JetBrains.Annotations.CanBeNull] Func<string, string> singularizePluralizer, [JetBrains.Annotations.NotNull] Func<string, ICollection<string>, string> uniquifier);

	bool IsCSharpKeyword([JetBrains.Annotations.NotNull] string identifier);

	bool IsValidIdentifier([JetBrains.Annotations.CanBeNull] string name);
}

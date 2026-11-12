using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace Microsoft.EntityFrameworkCore.Scaffolding.Internal;

public interface ICandidateNamingService
{
	string GenerateCandidateIdentifier([JetBrains.Annotations.NotNull] DatabaseTable originalTable);

	string GenerateCandidateIdentifier([JetBrains.Annotations.NotNull] DatabaseColumn originalColumn);

	string GetDependentEndCandidateNavigationPropertyName([JetBrains.Annotations.NotNull] IForeignKey foreignKey);

	string GetPrincipalEndCandidateNavigationPropertyName([JetBrains.Annotations.NotNull] IForeignKey foreignKey, [JetBrains.Annotations.NotNull] string dependentEndNavigationPropertyName);
}

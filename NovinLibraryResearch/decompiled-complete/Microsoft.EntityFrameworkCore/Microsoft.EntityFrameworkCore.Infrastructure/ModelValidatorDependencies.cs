using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public sealed class ModelValidatorDependencies
{
	public ITypeMappingSource TypeMappingSource { get; }

	public IMemberClassifier MemberClassifier { get; }

	[EntityFrameworkInternal]
	public ModelValidatorDependencies([JetBrains.Annotations.NotNull] ITypeMappingSource typeMappingSource, [JetBrains.Annotations.NotNull] IMemberClassifier memberClassifier)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(typeMappingSource, "typeMappingSource");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(memberClassifier, "memberClassifier");
		TypeMappingSource = typeMappingSource;
		MemberClassifier = memberClassifier;
	}

	public ModelValidatorDependencies With([JetBrains.Annotations.NotNull] ITypeMappingSource typeMappingSource)
	{
		return new ModelValidatorDependencies(typeMappingSource, MemberClassifier);
	}

	public ModelValidatorDependencies With([JetBrains.Annotations.NotNull] IMemberClassifier memberClassifier)
	{
		return new ModelValidatorDependencies(TypeMappingSource, memberClassifier);
	}
}

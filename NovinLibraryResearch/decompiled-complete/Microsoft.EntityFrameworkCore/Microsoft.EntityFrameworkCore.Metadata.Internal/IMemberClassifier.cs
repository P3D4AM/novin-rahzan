using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public interface IMemberClassifier
{
	Type FindCandidateNavigationPropertyType([JetBrains.Annotations.NotNull] MemberInfo memberInfo);
}

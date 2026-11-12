using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IConventionTypeBase : ITypeBase, IAnnotatable, IConventionAnnotatable
{
	new IConventionModel Model { get; }

	void AddIgnored([JetBrains.Annotations.NotNull] string memberName, bool fromDataAnnotation = false);

	void RemoveIgnored([JetBrains.Annotations.NotNull] string memberName);

	ConfigurationSource? FindIgnoredConfigurationSource([JetBrains.Annotations.NotNull] string memberName);

	IReadOnlyList<string> GetIgnoredMembers();
}

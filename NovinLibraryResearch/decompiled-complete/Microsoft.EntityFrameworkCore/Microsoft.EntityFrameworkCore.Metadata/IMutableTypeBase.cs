using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IMutableTypeBase : ITypeBase, IAnnotatable, IMutableAnnotatable
{
	new IMutableModel Model { get; }

	void AddIgnored([JetBrains.Annotations.NotNull] string memberName);

	void RemoveIgnored([JetBrains.Annotations.NotNull] string memberName);

	bool IsIgnored([JetBrains.Annotations.NotNull] string memberName);

	IReadOnlyList<string> GetIgnoredMembers();
}

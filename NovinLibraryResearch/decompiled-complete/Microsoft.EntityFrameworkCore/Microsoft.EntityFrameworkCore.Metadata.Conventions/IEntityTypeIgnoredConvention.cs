using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public interface IEntityTypeIgnoredConvention : IConvention
{
	void ProcessEntityTypeIgnored([JetBrains.Annotations.NotNull] IConventionModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] Type type, [JetBrains.Annotations.NotNull] IConventionContext<string> context);
}

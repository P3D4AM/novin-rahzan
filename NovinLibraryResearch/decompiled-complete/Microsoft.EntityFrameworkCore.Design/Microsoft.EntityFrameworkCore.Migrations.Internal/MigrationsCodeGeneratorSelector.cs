using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Design;

namespace Microsoft.EntityFrameworkCore.Migrations.Internal;

public class MigrationsCodeGeneratorSelector : LanguageBasedSelector<IMigrationsCodeGenerator>, IMigrationsCodeGeneratorSelector
{
	public virtual IMigrationsCodeGenerator Override
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public MigrationsCodeGeneratorSelector([JetBrains.Annotations.NotNull] IEnumerable<IMigrationsCodeGenerator> services)
		: base(services)
	{
	}

	public override IMigrationsCodeGenerator Select(string language)
	{
		return Override ?? base.Select(language);
	}
}

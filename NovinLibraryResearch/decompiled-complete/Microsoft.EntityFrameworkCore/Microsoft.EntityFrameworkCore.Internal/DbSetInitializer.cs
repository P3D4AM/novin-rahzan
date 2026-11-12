using System.Linq;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Internal;

public class DbSetInitializer : IDbSetInitializer
{
	private readonly IDbSetFinder _setFinder;

	private readonly IDbSetSource _setSource;

	public DbSetInitializer([JetBrains.Annotations.NotNull] IDbSetFinder setFinder, [JetBrains.Annotations.NotNull] IDbSetSource setSource)
	{
		_setFinder = setFinder;
		_setSource = setSource;
	}

	public virtual void InitializeSets(DbContext context)
	{
		foreach (DbSetProperty item in from p in _setFinder.FindSets(context.GetType())
			where p.Setter != null
			select p)
		{
			item.Setter.SetClrValue(context, ((IDbSetCache)context).GetOrAddSet(_setSource, item.ClrType));
		}
	}
}

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Scaffolding.Internal;

public class CSharpNamer<T>
{
	private readonly Func<T, string> _nameGetter;

	private readonly ICSharpUtilities _cSharpUtilities;

	private readonly Func<string, string> _singularizePluralizer;

	protected readonly Dictionary<T, string> NameCache = new Dictionary<T, string>();

	public CSharpNamer([JetBrains.Annotations.NotNull] Func<T, string> nameGetter, [JetBrains.Annotations.NotNull] ICSharpUtilities cSharpUtilities, [JetBrains.Annotations.CanBeNull] Func<string, string> singularizePluralizer)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(nameGetter, "nameGetter");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(cSharpUtilities, "cSharpUtilities");
		_nameGetter = nameGetter;
		_cSharpUtilities = cSharpUtilities;
		_singularizePluralizer = singularizePluralizer;
	}

	public virtual string GetName([JetBrains.Annotations.NotNull] T item)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(item, "item");
		if (NameCache.ContainsKey(item))
		{
			return NameCache[item];
		}
		string text = _cSharpUtilities.GenerateCSharpIdentifier(_nameGetter(item), null, _singularizePluralizer);
		NameCache.Add(item, text);
		return text;
	}
}

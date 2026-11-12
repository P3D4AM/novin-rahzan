using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Scaffolding.Internal;

public class CSharpUniqueNamer<T> : CSharpNamer<T>
{
	private readonly HashSet<string> _usedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

	public CSharpUniqueNamer([JetBrains.Annotations.NotNull] Func<T, string> nameGetter, [JetBrains.Annotations.NotNull] ICSharpUtilities cSharpUtilities, [JetBrains.Annotations.CanBeNull] Func<string, string> singularizePluralizer)
		: this(nameGetter, (IEnumerable<string>)null, cSharpUtilities, singularizePluralizer)
	{
	}

	public CSharpUniqueNamer([JetBrains.Annotations.NotNull] Func<T, string> nameGetter, [JetBrains.Annotations.CanBeNull] IEnumerable<string> usedNames, [JetBrains.Annotations.NotNull] ICSharpUtilities cSharpUtilities, [JetBrains.Annotations.CanBeNull] Func<string, string> singularizePluralizer)
		: base(nameGetter, cSharpUtilities, singularizePluralizer)
	{
		if (usedNames == null)
		{
			return;
		}
		foreach (string usedName in usedNames)
		{
			_usedNames.Add(usedName);
		}
	}

	public override string GetName(T item)
	{
		if (NameCache.ContainsKey(item))
		{
			return base.GetName(item);
		}
		string name = base.GetName(item);
		string text = name;
		int num = 1;
		while (_usedNames.Contains(text))
		{
			text = name + num++;
		}
		_usedNames.Add(text);
		NameCache[item] = text;
		return text;
	}
}

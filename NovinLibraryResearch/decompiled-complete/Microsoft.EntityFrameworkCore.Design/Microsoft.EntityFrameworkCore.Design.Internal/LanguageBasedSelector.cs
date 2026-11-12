using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;

namespace Microsoft.EntityFrameworkCore.Design.Internal;

public abstract class LanguageBasedSelector<T> where T : ILanguageBasedService
{
	protected virtual IEnumerable<T> Services { get; }

	protected LanguageBasedSelector(IEnumerable<T> services)
	{
		Services = services;
	}

	public virtual T Select([JetBrains.Annotations.CanBeNull] string language)
	{
		if (string.IsNullOrEmpty(language))
		{
			language = "C#";
		}
		T val = Services.LastOrDefault((T s) => s.Language == null);
		if (val != null)
		{
			return val;
		}
		List<T> list = Services.Where((T s) => string.Equals(s.Language, language, StringComparison.OrdinalIgnoreCase)).ToList();
		if (list.Count == 0)
		{
			throw new OperationException(DesignStrings.NoLanguageService(language, typeof(T).ShortDisplayName()));
		}
		return list.Last();
	}
}

using System.Collections.Generic;

namespace Microsoft.EntityFrameworkCore.Query;

public interface IMethodCallTranslatorPlugin
{
	IEnumerable<IMethodCallTranslator> Translators { get; }
}

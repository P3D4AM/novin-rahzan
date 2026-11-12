using System.Collections.Generic;

namespace Microsoft.EntityFrameworkCore.Query;

public interface IMemberTranslatorPlugin
{
	IEnumerable<IMemberTranslator> Translators { get; }
}

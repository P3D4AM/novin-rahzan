using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Scaffolding;

public class SavedModelFiles
{
	public virtual string ContextFile { get; }

	public virtual IList<string> AdditionalFiles { get; }

	public SavedModelFiles([JetBrains.Annotations.NotNull] string contextFile, [JetBrains.Annotations.NotNull] IEnumerable<string> additionalFiles)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(contextFile, "contextFile");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(additionalFiles, "additionalFiles");
		ContextFile = contextFile;
		AdditionalFiles = new List<string>(additionalFiles);
	}
}

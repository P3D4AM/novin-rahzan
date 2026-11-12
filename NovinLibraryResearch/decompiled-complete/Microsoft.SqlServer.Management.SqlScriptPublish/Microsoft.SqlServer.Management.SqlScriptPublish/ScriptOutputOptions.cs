using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.SqlScriptPublish;

public class ScriptOutputOptions
{
	public ScriptDestination ScriptDestination { get; set; }

	public ScriptFileType SaveFileType { get; set; }

	public ScriptFileMode SaveFileMode { get; set; }

	public string SaveFileName { get; set; }

	public bool Indented { get; set; }

	public ISmoScriptWriter CustomSmoScriptWriter { get; set; }
}

namespace Microsoft.SqlServer.Management.SqlScriptPublish;

public enum ScriptDestination
{
	ToSingleFile,
	ToClipboard,
	ToEditor,
	ToFilePerObject,
	ToNotebook,
	ToCustomWriter
}

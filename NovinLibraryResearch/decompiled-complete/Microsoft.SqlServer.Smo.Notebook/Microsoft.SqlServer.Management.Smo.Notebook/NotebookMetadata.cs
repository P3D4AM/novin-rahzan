namespace Microsoft.SqlServer.Management.Smo.Notebook;

internal class NotebookMetadata
{
	private static readonly KernelSpec kernelSpec = new KernelSpec();

	private static readonly LanguageInfo languageInfo = new LanguageInfo();

	public KernelSpec kernel_spec => kernelSpec;

	public LanguageInfo language_info => languageInfo;
}

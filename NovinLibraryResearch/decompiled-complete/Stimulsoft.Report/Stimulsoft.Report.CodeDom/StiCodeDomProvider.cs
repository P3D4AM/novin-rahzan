using System.CodeDom.Compiler;

namespace Stimulsoft.Report.CodeDom;

public abstract class StiCodeDomProvider
{
	public abstract string FileExtension { get; }

	public abstract ICodeGenerator CreateGenerator();
}

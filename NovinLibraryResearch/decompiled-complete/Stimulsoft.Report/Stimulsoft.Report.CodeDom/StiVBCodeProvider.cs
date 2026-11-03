using System.CodeDom.Compiler;

namespace Stimulsoft.Report.CodeDom;

public class StiVBCodeProvider : StiCodeDomProvider
{
	public override string FileExtension => "vb";

	public override ICodeGenerator CreateGenerator()
	{
		return (ICodeGenerator)(object)new StiVBCodeGenerator();
	}
}

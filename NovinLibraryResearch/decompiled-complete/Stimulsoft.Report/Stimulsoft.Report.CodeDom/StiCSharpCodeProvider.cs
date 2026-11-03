using System.CodeDom.Compiler;

namespace Stimulsoft.Report.CodeDom;

public class StiCSharpCodeProvider : StiCodeDomProvider
{
	public override string FileExtension => "cs";

	public override ICodeGenerator CreateGenerator()
	{
		return (ICodeGenerator)(object)new StiCSharpCodeGenerator();
	}
}

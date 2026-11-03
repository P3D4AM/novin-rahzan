using System.Collections;

namespace Stimulsoft.Report.Engine;

public class StiParserParameters
{
	public bool StoreToPrint { get; set; }

	public bool ExecuteIfStoreToPrint { get; set; } = true;


	public bool ReturnAsmList { get; set; }

	public bool CheckSyntaxMode { get; set; }

	public bool? SyntaxCaseSensitive { get; set; }

	public StiParser Parser { get; set; }

	public Hashtable ConversionStore { get; set; }

	public string GlobalizedNameExt { get; set; }

	public bool IgnoreGlobalizedName { get; set; }

	public Hashtable Constants { get; set; }

	public StiParser.StiParserGetDataFieldValueDelegate GetDataFieldValue { get; set; }

	public bool UseAliases { get; set; }

	internal Hashtable VariablesRecursionCheck { get; set; }
}

using System.Windows.Forms.Design;

namespace Stimulsoft.Report.Design;

internal class StiDesignerControlDesigner : ParentControlDesigner
{
	protected override bool DrawGrid => false;
}

using System.Windows.Forms;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Editors;

namespace Stimulsoft.Report.Components.Design;

public class StiGroupHeaderBandDesigner : StiBandDesigner
{
	public override DialogResult Design(StiComponent component)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return StiConditionEditor.EditCondition((IStiGroup)component);
	}

	public StiGroupHeaderBandDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}

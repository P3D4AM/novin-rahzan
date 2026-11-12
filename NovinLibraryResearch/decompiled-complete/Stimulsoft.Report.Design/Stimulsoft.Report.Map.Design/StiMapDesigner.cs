using System.Windows.Forms;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Maps;

namespace Stimulsoft.Report.Map.Design;

public class StiMapDesigner : StiComponentDesigner
{
	public override DialogResult Design(StiComponent component)
	{
		StiMapDesignerForm stiMapDesignerForm = new StiMapDesignerForm((StiMap)component);
		component?.Report?.Designer?.AddOwnedForm((Form)(object)stiMapDesignerForm);
		((Control)stiMapDesignerForm).Show();
		return (DialogResult)1;
	}

	public StiMapDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}

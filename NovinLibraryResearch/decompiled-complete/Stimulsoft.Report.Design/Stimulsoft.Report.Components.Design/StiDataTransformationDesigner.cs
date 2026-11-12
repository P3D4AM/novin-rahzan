using System.Windows.Forms;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.DataTransformation;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Components.Design;

public class StiDataTransformationDesigner : StiInplaceDesigner
{
	protected override StiInplaceEditorForm CreateEditorForm(object component)
	{
		StiDataTransformationEditorForm stiDataTransformationEditorForm = new StiDataTransformationEditorForm(component as StiDataTransformation);
		if (base.Designer != null && base.Designer.DesignerControl != null)
		{
			base.Designer.DesignerControl.AddOwnedForm((Form)(object)stiDataTransformationEditorForm);
		}
		return stiDataTransformationEditorForm;
	}

	public StiDataTransformationDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}

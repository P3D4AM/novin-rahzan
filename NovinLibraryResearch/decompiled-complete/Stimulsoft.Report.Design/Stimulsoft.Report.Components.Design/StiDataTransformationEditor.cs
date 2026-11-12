using System;
using System.ComponentModel;
using System.Drawing.Design;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Components.Design;

public class StiDataTransformationEditor : UITypeEditor
{
	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (context == null || context.Instance == null)
		{
			return ((UITypeEditor)this).GetEditStyle(context);
		}
		return (UITypeEditorEditStyle)2;
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		StiComponent stiComponent = context.Instance as StiComponent;
		if (!(stiComponent is IStiTransformActions) && !(stiComponent is IStiTransformFilters) && !(stiComponent is IStiTransformSorts))
		{
			return null;
		}
		StiComponentDesigner transformationDesigner = Stimulsoft.Report.Dashboard.StiDataTransformationDesigner.GetTransformationDesigner(stiComponent.Report.Designer, stiComponent.GetType());
		if (transformationDesigner == null || !StiRestrictionsHelper.IsAllowChange(stiComponent))
		{
			return null;
		}
		stiComponent.Report.Designer.UndoRedoSave(stiComponent.LocalizedName, onlyCurrentPage: true);
		transformationDesigner.OnDoubleClick(stiComponent);
		return null;
	}
}

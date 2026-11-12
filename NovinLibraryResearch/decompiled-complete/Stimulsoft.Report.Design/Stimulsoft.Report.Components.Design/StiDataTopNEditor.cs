using System;
using System.ComponentModel;
using System.Drawing.Design;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Components.Design;

public class StiDataTopNEditor : UITypeEditor
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
			return value;
		}
		StiComponent stiComponent = context.Instance as StiComponent;
		if (!(stiComponent is IStiDataTopN))
		{
			return null;
		}
		if (((IStiDataTopN)stiComponent).TopN == null)
		{
			((IStiDataTopN)stiComponent).TopN = new StiDataTopN();
		}
		StiComponentDesigner designer = StiDataTopNDesigner.GetDesigner(stiComponent.Report.Designer, stiComponent.GetType());
		if (designer == null || !StiRestrictionsHelper.IsAllowChange(stiComponent))
		{
			return null;
		}
		stiComponent.Report.Designer.UndoRedoSave(stiComponent.LocalizedName, onlyCurrentPage: true);
		designer.OnDoubleClick(stiComponent);
		return ((IStiDataTopN)stiComponent).TopN;
	}
}

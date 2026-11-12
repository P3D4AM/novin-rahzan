using System;
using System.ComponentModel;
using System.Drawing.Design;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Design.Components;

public class StiElementEditor : UITypeEditor
{
	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null)
		{
			return (UITypeEditorEditStyle)2;
		}
		return ((UITypeEditor)this).GetEditStyle(context);
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		Array array = context.Instance as Array;
		IStiElement stiElement = context.Instance as IStiElement;
		if (array != null && array.GetLength(0) > 0)
		{
			stiElement = array.GetValue(0) as IStiElement;
		}
		if (stiElement == null)
		{
			return null;
		}
		StiComponentDesigner componentDesigner = StiComponentDesigner.GetComponentDesigner(stiElement.Report.Designer, stiElement.GetType());
		componentDesigner.Design(stiElement as StiComponent);
		return context.PropertyDescriptor.GetValue(stiElement);
	}
}

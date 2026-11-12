using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Design;

public class TokenEditorDesigner : ControlDesigner
{
	public override void Initialize(IComponent component)
	{
		((ControlDesigner)this).Initialize(component);
		if (component != null && component.Site != null && component.Site.DesignMode && ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) is IDesignerHost designerHost)
		{
			designerHost.LoadComplete += DesignerLoadComplete;
		}
	}

	private void DesignerLoadComplete(object sender, EventArgs e)
	{
		if (((ComponentDesigner)this).GetService(typeof(IDesignerHost)) is IDesignerHost designerHost)
		{
			designerHost.LoadComplete -= DesignerLoadComplete;
		}
		Control control = ((ControlDesigner)this).Control;
		if (control != null && !string.IsNullOrEmpty(control.Text))
		{
			TypeDescriptor.GetProperties(control)["Text"].SetValue(control, "");
		}
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		if (((ControlDesigner)this).Control is TokenEditor tokenEditor)
		{
			tokenEditor.BackgroundStyle.Class = ElementStyleClassKeys.DateTimeInputBackgroundKey;
			tokenEditor.Separators.Add(";");
			tokenEditor.Separators.Add(",");
		}
		((ControlDesigner)this).InitializeNewComponent(defaultValues);
	}
}

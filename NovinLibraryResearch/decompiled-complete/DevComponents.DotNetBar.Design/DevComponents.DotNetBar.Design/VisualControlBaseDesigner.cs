using System.Collections;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Design;

public class VisualControlBaseDesigner : ControlDesigner
{
	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		if (((ControlDesigner)this).Control is VisualControlBase visualControlBase)
		{
			((Control)visualControlBase).Height = visualControlBase.PreferredHeight;
		}
		((ControlDesigner)this).InitializeNewComponent(defaultValues);
	}
}

using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using DevComponents.DotNetBar.Validator;

namespace DevComponents.DotNetBar.Design;

internal class HighlighterDesigner : ComponentDesigner
{
	public override void Initialize(IComponent component)
	{
		((ComponentDesigner)this).Initialize(component);
		_ = component.Site.DesignMode;
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ComponentDesigner)this).InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	private void SetDesignTimeDefaults()
	{
		if (((ComponentDesigner)this).Component is Highlighter highlighter && ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) is IDesignerHost designerHost)
		{
			IComponent rootComponent = designerHost.RootComponent;
			highlighter.ContainerControl = (Control)((rootComponent is Control) ? rootComponent : null);
		}
	}
}

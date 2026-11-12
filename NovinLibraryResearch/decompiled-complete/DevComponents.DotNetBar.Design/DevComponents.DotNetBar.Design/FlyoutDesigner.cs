using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar.Design;

public class FlyoutDesigner : ComponentDesigner
{
	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		SetDefaults();
		((ComponentDesigner)this).InitializeNewComponent(defaultValues);
	}

	private void SetDefaults()
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		if (designerHost.Loading)
		{
			designerHost.LoadComplete += DesignerLoadComplete;
		}
		else if (((ComponentDesigner)this).Component is Flyout flyout)
		{
			IComponent rootComponent = designerHost.RootComponent;
			Control val = (Control)((rootComponent is Control) ? rootComponent : null);
			if (val != null)
			{
				flyout.Parent = val;
			}
		}
	}

	private void DesignerLoadComplete(object sender, EventArgs e)
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost != null)
		{
			designerHost.LoadComplete -= DesignerLoadComplete;
		}
		SetDefaults();
	}
}

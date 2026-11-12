using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class RibbonBarMergeContainerDesigner : ParentControlDesigner
{
	public override DesignerVerbCollection Verbs => new DesignerVerbCollection(new DesignerVerb[1]
	{
		new DesignerVerb("Layout Ribbons", LayoutRibbons)
	});

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ParentControlDesigner)this).InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	private void SetDesignTimeDefaults()
	{
		if (((ControlDesigner)this).Control is RibbonBarMergeContainer component)
		{
			TypeDescriptor.GetProperties(component)["Visible"].SetValue(component, false);
			TypeDescriptor.GetProperties(component)["ColorSchemeStyle"].SetValue(component, eDotNetBarStyle.Office2007);
		}
	}

	private void LayoutRibbons(object sender, EventArgs e)
	{
		if (!(((ControlDesigner)this).Control is RibbonPanel ribbonPanel))
		{
			return;
		}
		IDesignerHost designerHost = ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) as IDesignerHost;
		DesignerTransaction designerTransaction = null;
		if (designerHost != null)
		{
			designerTransaction = designerHost.CreateTransaction("Rendering Layout");
		}
		try
		{
			ribbonPanel.LayoutRibbons();
		}
		finally
		{
			designerTransaction?.Commit();
		}
	}
}

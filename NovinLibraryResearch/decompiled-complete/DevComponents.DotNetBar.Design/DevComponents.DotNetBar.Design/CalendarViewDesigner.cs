using System.Collections;
using System.ComponentModel;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class CalendarViewDesigner : ControlDesigner
{
	public override void Initialize(IComponent component)
	{
		((ControlDesigner)this).Initialize(component);
		if (component != null && component.Site != null)
		{
			_ = component.Site.DesignMode;
		}
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ControlDesigner)this).InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	private void SetDesignTimeDefaults()
	{
		_ = ((ControlDesigner)this).Control;
	}
}

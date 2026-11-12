using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using DevComponents.DotNetBar.Validator;

namespace DevComponents.DotNetBar.Design;

internal class SuperValidatorDesigner : ComponentDesigner
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
		if (!(((ComponentDesigner)this).Component is SuperValidator superValidator) || !(((ComponentDesigner)this).GetService(typeof(IDesignerHost)) is IDesignerHost designerHost))
		{
			return;
		}
		IComponent component = designerHost.CreateComponent(typeof(ErrorProvider));
		ErrorProvider val = (ErrorProvider)((component is ErrorProvider) ? component : null);
		if (val != null && designerHost.CreateComponent(typeof(Highlighter)) is Highlighter highlighter)
		{
			if (highlighter.ContainerControl == null)
			{
				IComponent rootComponent = designerHost.RootComponent;
				highlighter.ContainerControl = (Control)((rootComponent is Control) ? rootComponent : null);
			}
			TypeDescriptor.GetProperties(superValidator)["Highlighter"].SetValue(superValidator, highlighter);
			TypeDescriptor.GetProperties(superValidator)["ErrorProvider"].SetValue(superValidator, val);
			val.Icon = Helpers.LoadIcon("Validator.SuperValidator.ico");
			IComponent rootComponent2 = designerHost.RootComponent;
			superValidator.ContainerControl = (Control)((rootComponent2 is Control) ? rootComponent2 : null);
		}
	}
}

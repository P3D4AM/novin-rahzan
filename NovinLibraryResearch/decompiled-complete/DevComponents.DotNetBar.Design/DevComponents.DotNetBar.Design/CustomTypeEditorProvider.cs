using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class CustomTypeEditorProvider : ITypeDescriptorContext, IServiceProvider, IWindowsFormsEditorService
{
	private IContainer m_Container;

	private object m_Instance;

	private IServiceProvider m_Provider;

	private PropertyDescriptor m_PropertyDescriptor;

	public IContainer Container => m_Container;

	public object Instance => m_Instance;

	public PropertyDescriptor PropertyDescriptor => m_PropertyDescriptor;

	public CustomTypeEditorProvider(IContainer container, IServiceProvider provider)
	{
		m_Container = container;
		m_Provider = provider;
	}

	public void SetInstance(object instance, PropertyDescriptor desc)
	{
		m_Instance = instance;
		m_PropertyDescriptor = desc;
	}

	public void OnComponentChanged()
	{
		if (m_Provider.GetService(typeof(IComponentChangeService)) is IComponentChangeService componentChangeService)
		{
			componentChangeService.OnComponentChanged(m_Instance, m_PropertyDescriptor, null, null);
		}
	}

	public bool OnComponentChanging()
	{
		return true;
	}

	public object GetService(Type serviceType)
	{
		return m_Provider.GetService(serviceType);
	}

	public void CloseDropDown()
	{
		throw new Exception("The method or operation is not implemented.");
	}

	public void DropDownControl(Control control)
	{
		throw new Exception("The method or operation is not implemented.");
	}

	public DialogResult ShowDialog(Form dialog)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		IntPtr focus = WinApi.GetFocus();
		IUIService val = (IUIService)GetService(typeof(IUIService));
		DialogResult result = ((val == null) ? dialog.ShowDialog() : val.ShowDialog(dialog));
		if (focus != IntPtr.Zero)
		{
			WinApi.SetFocus(focus);
		}
		return result;
	}
}

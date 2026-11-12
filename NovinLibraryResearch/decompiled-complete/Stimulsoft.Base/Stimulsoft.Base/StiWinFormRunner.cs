using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace Stimulsoft.Base;

public class StiWinFormRunner : IStiFormRunner, IDisposable
{
	private Form form;

	public object OwnerWindow { get; set; }

	public object this[string key]
	{
		get
		{
			Type type = ((object)form).GetType();
			PropertyInfo property = type.GetProperty(key);
			return property.GetValue(form, new object[0]);
		}
		set
		{
			Type type = ((object)form).GetType();
			PropertyInfo property = type.GetProperty(key);
			property.SetValue(form, value, new object[0]);
		}
	}

	public event StiShowDialogCompleteEventHandler Complete;

	public void ShowDialog()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Invalid comparison between Unknown and I4
		_003F val = form;
		object ownerWindow = OwnerWindow;
		((Form)val).Owner = (Form)((ownerWindow is Form) ? ownerWindow : null);
		InvokeComplete((int)form.ShowDialog() == 1);
	}

	public void Create(string formType, string assemblyName)
	{
		Create(formType, assemblyName, new object[0]);
	}

	public void Create(string formType, string assemblyName, params object[] args)
	{
		Type type = Type.GetType(formType + ", " + assemblyName + ", Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
		if (type == null)
		{
			throw new Exception("Assembly '" + assemblyName + "' is not found!");
		}
		ref Form reference = ref form;
		object obj = StiActivator.CreateObject(type, args);
		reference = (Form)((obj is Form) ? obj : null);
	}

	public void Dispose()
	{
		if (form != null)
		{
			((Component)(object)form).Dispose();
		}
	}

	private void InvokeComplete(bool dialogResult)
	{
		this.Complete?.Invoke(this, new StiShowDialogCompleteEvetArgs(dialogResult));
	}
}

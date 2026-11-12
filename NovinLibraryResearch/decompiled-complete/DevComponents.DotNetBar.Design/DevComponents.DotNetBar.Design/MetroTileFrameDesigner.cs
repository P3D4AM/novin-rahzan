using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;

namespace DevComponents.DotNetBar.Design;

public class MetroTileFrameDesigner : ComponentDesigner
{
	private static bool SetGenerateMemberProperty(IExtenderListService service, IComponent component, bool value)
	{
		IExtenderProvider extenderProvider = null;
		IExtenderProvider[] extenderProviders = service.GetExtenderProviders();
		IExtenderProvider[] array = extenderProviders;
		foreach (IExtenderProvider extenderProvider2 in array)
		{
			if (extenderProvider2.GetType().FullName == "System.ComponentModel.Design.Serialization.CodeDomDesignerLoader+ModifiersExtenderProvider")
			{
				extenderProvider = extenderProvider2;
				break;
			}
		}
		if (extenderProvider == null)
		{
			return false;
		}
		MethodInfo method = extenderProvider.GetType().GetMethod("SetGenerateMember", BindingFlags.Instance | BindingFlags.Public);
		if ((object)method != null)
		{
			method.Invoke(extenderProvider, new object[2] { component, value });
			return true;
		}
		return false;
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		SetGenerateMemberProperty((IExtenderListService)((ComponentDesigner)this).GetService(typeof(IExtenderListService)), ((ComponentDesigner)this).Component, value: false);
		((ComponentDesigner)this).InitializeNewComponent(defaultValues);
	}
}

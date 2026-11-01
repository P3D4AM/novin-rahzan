using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace TimeAttendanceUI.Dictionary;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class ResourceUI
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				resourceMan = new ResourceManager("TimeAttendanceUI.Dictionary.ResourceUI", typeof(ResourceUI).Assembly);
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	public static string HelpCircleShift => ResourceManager.GetString("HelpCircleShift", resourceCulture);

	public static string HelpFloatingShift => ResourceManager.GetString("HelpFloatingShift", resourceCulture);

	public static string HelpNormalShift => ResourceManager.GetString("HelpNormalShift", resourceCulture);

	public static string HelpProgrammingShift => ResourceManager.GetString("HelpProgrammingShift", resourceCulture);

	public static Bitmap PayamakUnknown => (Bitmap)ResourceManager.GetObject("PayamakUnknown", resourceCulture);

	internal ResourceUI()
	{
	}
}

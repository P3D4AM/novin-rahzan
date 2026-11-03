using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Resources;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Resources
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				resourceMan = new ResourceManager("LK.Properties.Resources", typeof(Resources).Assembly);
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
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

	internal static Bitmap _stop => (Bitmap)ResourceManager.GetObject("_stop", resourceCulture);

	internal static Bitmap accept1 => (Bitmap)ResourceManager.GetObject("accept1", resourceCulture);

	internal static byte[] AccTinyX64 => (byte[])ResourceManager.GetObject("AccTinyX64", resourceCulture);

	internal static byte[] AccTinyX86 => (byte[])ResourceManager.GetObject("AccTinyX86", resourceCulture);

	internal static Bitmap Setting => (Bitmap)ResourceManager.GetObject("Setting", resourceCulture);

	internal static string TinyError => ResourceManager.GetString("TinyError", resourceCulture);

	internal Resources()
	{
	}
}

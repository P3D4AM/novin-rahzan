using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Serilog.Exceptions;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
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
				resourceMan = new ResourceManager("Serilog.Exceptions.Resources", typeof(Resources).GetTypeInfo().Assembly);
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

	internal static string CannotBeEmpty => ResourceManager.GetString("CannotBeEmpty", resourceCulture);

	internal static string DestructuringDepthMustBeMoreThanZero => ResourceManager.GetString("DestructuringDepthMustBeMoreThanZero", resourceCulture);

	internal static string FilterAlreadySet => ResourceManager.GetString("FilterAlreadySet", resourceCulture);

	internal Resources()
	{
	}
}

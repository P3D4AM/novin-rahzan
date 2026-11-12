using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Stimulsoft.Base.Drawing;

public class StiIconUtils
{
	public static Icon GetIcon(Type type, string iconName)
	{
		return GetIcon(type.Module.Assembly, iconName);
	}

	public static Icon GetIcon(string assemblyName, string iconName)
	{
		Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault((Assembly a) => a.GetName().Name == assemblyName);
		if (assembly != null)
		{
			return GetIcon(assembly, iconName);
		}
		throw new Exception("Can't find assembly '" + assemblyName + "'");
	}

	public static Icon GetIcon(Assembly cursorAssembly, string iconName)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		Stream manifestResourceStream = cursorAssembly.GetManifestResourceStream(iconName);
		if (manifestResourceStream != null)
		{
			return new Icon(manifestResourceStream);
		}
		throw new Exception("Can't find icon '" + iconName + "' in resources");
	}

	public static Icon ToIcon(Image image)
	{
		Bitmap val = (Bitmap)(object)((image is Bitmap) ? image : null);
		if (val == null)
		{
			return null;
		}
		IntPtr hicon = val.GetHicon();
		return Icon.FromHandle(hicon);
	}

	public static Icon ResizeIcon(Icon icon, int width, int height)
	{
		if (icon.Width == width && icon.Height == height)
		{
			return icon;
		}
		return ToIcon((Image)(object)StiImageUtils.ResizeImage((Image)(object)icon.ToBitmap(), width, height));
	}
}

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Stimulsoft.Base.Drawing;

public class StiCursorUtils
{
	public static Cursor GetCursor(Type type, string cursorName)
	{
		return GetCursor(type.Module.Assembly, cursorName);
	}

	public static Cursor GetCursor(string assemblyName, string cursorName)
	{
		Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault((Assembly a) => a.GetName().Name == assemblyName);
		if (assembly != null)
		{
			return GetCursor(assembly, cursorName);
		}
		throw new Exception("Can't find assembly '" + assemblyName + "'");
	}

	public static Cursor GetCursor(Assembly cursorAssembly, string cursorName)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		Stream manifestResourceStream = cursorAssembly.GetManifestResourceStream(cursorName);
		if (manifestResourceStream != null)
		{
			return new Cursor(manifestResourceStream);
		}
		throw new Exception("Can't find cursor '" + cursorName + "' in resources");
	}
}

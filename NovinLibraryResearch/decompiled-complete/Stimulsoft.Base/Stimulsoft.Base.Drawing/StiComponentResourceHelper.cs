using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Base.Drawing;

public class StiComponentResourceHelper
{
	public static object GetObject(ComponentResourceManager resources, string name)
	{
		string stepName = StiScale.StepName;
		object @object = resources.GetObject(name + stepName);
		if (@object != null)
		{
			return @object;
		}
		@object = resources.GetObject(name ?? "");
		if (@object != null && @object is Image)
		{
			Image val = (Image)((@object is Image) ? @object : null);
			Bitmap result = StiImageUtils.ResizeImage(val, StiScale.XXI(val.Width), StiScale.YYI(val.Height));
			val.Dispose();
			return result;
		}
		return @object;
	}

	public static Icon GetIcon(ComponentResourceManager resources, string name)
	{
		object @object = GetObject(resources, name);
		return (Icon)((@object is Icon) ? @object : null);
	}

	public static Image GetImage(ComponentResourceManager resources, string name)
	{
		object @object = GetObject(resources, name);
		return (Image)((@object is Image) ? @object : null);
	}
}

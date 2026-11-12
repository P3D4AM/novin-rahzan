using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Base.Drawing.Design;

public class StiStyleColorEditor : StiColorEditor
{
	public override bool GetPaintValueSupported(ITypeDescriptorContext context)
	{
		object obj = context.PropertyDescriptor?.GetValue(context.Instance);
		if (obj is Color)
		{
			return (Color)obj != Color.Transparent;
		}
		return true;
	}
}

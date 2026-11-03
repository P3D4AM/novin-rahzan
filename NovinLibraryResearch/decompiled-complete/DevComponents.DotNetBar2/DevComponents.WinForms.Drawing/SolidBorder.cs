using System.ComponentModel;
using System.Drawing;

namespace DevComponents.WinForms.Drawing;

public class SolidBorder : Border
{
	private Color __1360 = Color.Empty;

	[Description("Indicates the fill color.")]
	public Color Color
	{
		get
		{
			return __1360;
		}
		set
		{
			__1360 = value;
		}
	}

	public SolidBorder(Color color, int width)
	{
		__1360 = color;
		_ች = width;
	}

	public SolidBorder(Color color)
	{
		__1360 = color;
	}

	public SolidBorder()
	{
	}

	public override Pen CreatePen()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		if (!_1362())
		{
			return null;
		}
		return new Pen(__1360, (float)_ች);
	}

	private bool _1362()
	{
		if (!__1360.IsEmpty)
		{
			return _ች > 0;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeColor()
	{
		return !__1360.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetColor()
	{
		Color = Color.Empty;
	}
}

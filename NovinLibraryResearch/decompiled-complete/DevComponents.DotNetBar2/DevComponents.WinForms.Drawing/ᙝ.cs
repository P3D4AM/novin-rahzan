using System.ComponentModel;
using System.Drawing;

namespace DevComponents.WinForms.Drawing;

internal abstract class ᙝ
{
	private ᙝ __2781;

	private bool __2782;

	[DefaultValue(null)]
	public ᙝ _277F
	{
		get
		{
			return __2781;
		}
		set
		{
			__2781 = value;
		}
	}

	[DefaultValue(false)]
	public bool _2780
	{
		get
		{
			return __2782;
		}
		set
		{
			__2782 = value;
		}
	}

	public abstract void Paint(Graphics g, Rectangle bounds);
}

using System.ComponentModel;
using System.Drawing;

namespace DevComponents.DotNetBar;

[TypeConverter(typeof(DocumentBarContainerConverter))]
[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class DocumentBarContainer : DocumentBaseContainer
{
	private Bar Ⴭ;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Bar Bar
	{
		get
		{
			return Ⴭ;
		}
		set
		{
			Ⴭ = value;
		}
	}

	public override bool Visible
	{
		get
		{
			if (Ⴭ == null)
			{
				return false;
			}
			if (Ⴭ.ఛ())
			{
				return true;
			}
			return Ⴭ.ଇ;
		}
	}

	protected internal override Size MinimumSize => Ⴭ._౮(Ⴭ.MinimumDockSize(eOrientation.Horizontal));

	public DocumentBarContainer()
	{
	}

	public DocumentBarContainer(Bar bar)
	{
		Ⴭ = bar;
		if (Ⴭ._0B01 > 0 || Ⴭ._0B02 > 0)
		{
			_10C9(new Rectangle(0, 0, Ⴭ._0B02, Ⴭ._0B01));
		}
	}

	public DocumentBarContainer(Bar bar, int proposedWidth, int proposedHeight)
	{
		Ⴭ = bar;
		_10C9(new Rectangle(0, 0, proposedWidth, proposedHeight));
	}

	public override void Layout(Rectangle bounds)
	{
		if (Ⴭ != null)
		{
			Ⴭ._0B01 = 0;
			Ⴭ._0B02 = 0;
			Ⴭ._0B03 = 0;
			Ⴭ._0BE5(bounds.Size);
			Ⴭ.Location = bounds.Location;
		}
		_10C8(bounds);
	}
}

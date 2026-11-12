using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
public class Separator : BaseItem
{
	private Padding _Padding = new Padding(2, 2, 2, 2);

	private Size _FixedSize = new Size(3, 16);

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets separator padding.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public Padding Padding => _Padding;

	public Size FixedSize
	{
		get
		{
			return _FixedSize;
		}
		set
		{
			_FixedSize = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	public Separator()
		: this("")
	{
	}

	public Separator(string sItemName)
		: base(sItemName)
	{
	}

	public override void Paint(ItemPaintArgs p)
	{
		Rectangle bounds = Bounds;
		if (bounds.Width < 1 || bounds.Height < 1)
		{
			return;
		}
		Color itemSeparator = p.Colors.ItemSeparator;
		Color itemSeparatorShade = p.Colors.ItemSeparatorShade;
		Graphics graphics = p.Graphics;
		Rectangle r;
		if (Orientation == eOrientation.Vertical)
		{
			r = new Rectangle(bounds.X + _Padding.Left, bounds.Y + _Padding.Top + bounds.Height / 2, bounds.Width - _Padding.Right, 1);
			if (!itemSeparator.IsEmpty)
			{
				DisplayHelp.DrawLine(graphics, r.X, r.Y, r.Right, r.Y, itemSeparator, 1);
			}
		}
		else
		{
			r = new Rectangle(bounds.X + _Padding.Left + bounds.Width / 2, bounds.Y + _Padding.Top + (bounds.Height - _FixedSize.Height) / 2, 1, _FixedSize.Height);
			if (!itemSeparator.IsEmpty)
			{
				DisplayHelp.DrawLine(graphics, r.X, r.Y, r.X, r.Bottom, itemSeparator, 1);
			}
		}
		if (!itemSeparatorShade.IsEmpty)
		{
			r.Inflate(1, 1);
			DisplayHelp.DrawRectangle(graphics, itemSeparatorShade, r);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializePadding()
	{
		if (_Padding.Bottom == 2 && _Padding.Top == 2 && _Padding.Left == 2)
		{
			return _Padding.Right != 2;
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	private void ResetPadding()
	{
		_Padding = new Padding(2, 2, 2, 2);
	}

	public override void RecalcSize()
	{
		if (Orientation == eOrientation.Horizontal)
		{
			m_Rect.Size = new Size(_FixedSize.Width + _Padding.Horizontal, _FixedSize.Height + _Padding.Vertical);
		}
		else
		{
			m_Rect.Size = new Size(_FixedSize.Height + Padding.Horizontal, _FixedSize.Width + _Padding.Vertical);
		}
		base.RecalcSize();
	}

	public override BaseItem Copy()
	{
		Separator separator = new Separator(m_Name);
		CopyToItem(separator);
		return separator;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		Separator separator = copy as Separator;
		base.CopyToItem(separator);
		separator.FixedSize = _FixedSize;
		separator.Padding.Left = _Padding.Left;
		separator.Padding.Right = _Padding.Right;
		separator.Padding.Top = _Padding.Top;
		separator.Padding.Bottom = _Padding.Bottom;
	}
}

using System.Drawing;
using System.Xml;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar.TextMarkup;

internal class ሯ : _0CBC
{
	private ሴ ሱ;

	private ሶ ሲ;

	private int ܘ;

	private int ܗ;

	private Padding ሳ = new Padding(0, 0, 0, 0);

	private Color ފ = Color.Empty;

	public ሴ ሰ
	{
		get
		{
			return ሱ;
		}
		set
		{
			ሱ = value;
		}
	}

	public override void Render(MarkupDrawContext d)
	{
		Rectangle bounds = base.Bounds;
		bounds.Offset(d.Offset);
		if (!ފ.IsEmpty)
		{
			DisplayHelp.FillRectangle(d.Graphics, bounds, ފ);
		}
		base.Render(d);
	}

	protected override SerialContentLayoutManager GetLayoutManager(bool multiLine)
	{
		SerialContentLayoutManager layoutManager = base.GetLayoutManager(multiLine);
		if (ሱ == ሴ._0749)
		{
			layoutManager.ContentAlignment = eContentAlignment.Left;
		}
		else if (ሱ == ሴ.ሪ)
		{
			layoutManager.ContentAlignment = eContentAlignment.Right;
		}
		else if (ሱ == ሴ.ስ)
		{
			layoutManager.ContentAlignment = eContentAlignment.Center;
		}
		if (ሲ != 0)
		{
			layoutManager.EvenHeight = false;
			layoutManager.ContentVerticalAlignment = ((ሲ == ሶ.ራ) ? eContentVerticalAlignment.Bottom : eContentVerticalAlignment.Middle);
		}
		return layoutManager;
	}

	protected override Point GetContainerOffset()
	{
		if (ሳ.Left == 0 && ሳ.Top == 0)
		{
			return base.GetContainerOffset();
		}
		Point containerOffset = base.GetContainerOffset();
		containerOffset.X += ሳ.Left;
		containerOffset.Y += ሳ.Top;
		return containerOffset;
	}

	protected override void ArrangeInternal(Rectangle bounds, MarkupDrawContext d)
	{
		Rectangle bounds2 = bounds;
		int num = Dpi.Width(ܘ);
		if (num > 0)
		{
			bounds2.Width = num;
		}
		int num2 = Dpi.Height(ܗ);
		if (num2 > 0)
		{
			bounds2.Height = num2;
		}
		Padding padding = Dpi.Size(ሳ);
		if (padding.IsEmpty)
		{
			base.ArrangeInternal(bounds2, d);
			if (num > 0)
			{
				base.Bounds = new Rectangle(base.Bounds.X, base.Bounds.Y, num, (num2 > 0) ? num2 : (base.Bounds.Height + padding.Bottom));
			}
			else if (num2 > 0)
			{
				base.Bounds = new Rectangle(base.Bounds.X, base.Bounds.Y, base.Bounds.Width, num2);
			}
			return;
		}
		bounds2.X += padding.Left;
		bounds2.Y += padding.Top;
		bounds2.Width -= padding.Horizontal;
		bounds2.Height -= padding.Vertical;
		base.ArrangeInternal(bounds2, d);
		bounds2 = new Rectangle(bounds.X, bounds.Y, base.Bounds.Width + padding.Horizontal, base.Bounds.Height + padding.Vertical);
		if (num > 0)
		{
			bounds2.Width = num;
		}
		if (num2 > 0)
		{
			bounds2.Height = num2;
		}
		base.Bounds = bounds2;
	}

	public override void ReadAttributes(XmlTextReader reader)
	{
		for (int i = 0; i < reader.AttributeCount; i++)
		{
			reader.MoveToAttribute(i);
			if (reader.Name.ToLower() == "align")
			{
				switch (reader.Value.ToLower())
				{
				case "left":
					ሱ = ሴ._0749;
					break;
				case "right":
					ሱ = ሴ.ሪ;
					break;
				case "center":
					ሱ = ሴ.ስ;
					break;
				}
			}
			else if (reader.Name.ToLower() == "valign")
			{
				switch (reader.Value.ToLower())
				{
				case "top":
					ሲ = ሶ._074A;
					break;
				case "middle":
					ሲ = ሶ.ሷ;
					break;
				case "bottom":
					ሲ = ሶ.ራ;
					break;
				}
			}
			else if (reader.Name.ToLower() == "width")
			{
				try
				{
					ܘ = int.Parse(reader.Value);
				}
				catch
				{
					ܘ = 0;
				}
			}
			else if (reader.Name.ToLower() == "height")
			{
				try
				{
					ܗ = int.Parse(reader.Value);
				}
				catch
				{
					ܗ = 0;
				}
			}
			else if (reader.Name.ToLower() == "padding")
			{
				try
				{
					string[] array = reader.Value.Split(new char[1] { ',' });
					if (array.Length > 0)
					{
						ሳ.Left = int.Parse(array[0]);
					}
					if (array.Length > 1)
					{
						ሳ.Right = int.Parse(array[1]);
					}
					if (array.Length > 2)
					{
						ሳ.Top = int.Parse(array[2]);
					}
					if (array.Length > 3)
					{
						ሳ.Bottom = int.Parse(array[3]);
					}
				}
				catch
				{
					ሳ = new Padding(0, 0, 0, 0);
				}
			}
			else
			{
				if (!(reader.Name.ToLower() == "bgcolor"))
				{
					continue;
				}
				try
				{
					string value = reader.Value;
					if (value.StartsWith("#"))
					{
						if (value.Length == 7)
						{
							ފ = ColorScheme.GetColor(value.Substring(1));
						}
					}
					else
					{
						ފ = Color.FromName(value);
					}
				}
				catch
				{
					ފ = Color.Empty;
				}
			}
		}
	}
}

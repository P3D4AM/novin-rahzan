using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.Editors;

public class VisualButton : VisualButtonBase
{
	private bool _IsMouseOver;

	private bool _IsMouseDown;

	private bool _Checked;

	private string _Text = "";

	private Image _DisabledImage;

	private Image _Image;

	public bool IsMouseOver
	{
		get
		{
			return _IsMouseOver;
		}
		internal set
		{
			if (_IsMouseOver != value)
			{
				_IsMouseOver = value;
				InvalidateRender();
			}
		}
	}

	public bool IsMouseDown
	{
		get
		{
			return _IsMouseDown;
		}
		internal set
		{
			if (_IsMouseDown != value)
			{
				_IsMouseDown = value;
				InvalidateRender();
			}
		}
	}

	public bool Checked
	{
		get
		{
			return _Checked;
		}
		set
		{
			if (_Checked != value)
			{
				_Checked = value;
				InvalidateRender();
			}
		}
	}

	[DefaultValue("")]
	public string Text
	{
		get
		{
			return _Text;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			if (_Text != value)
			{
				_Text = value;
				InvalidateArrange();
			}
		}
	}

	[DefaultValue(null)]
	public Image Image
	{
		get
		{
			return _Image;
		}
		set
		{
			if (_Image != value)
			{
				_Image = value;
				if (_DisabledImage != null)
				{
					_DisabledImage.Dispose();
					_DisabledImage = null;
				}
				InvalidateArrange();
			}
		}
	}

	public override void PerformLayout(PaintInfo pi)
	{
		Size size = new Size(0, pi.AvailableSize.Height);
		Graphics graphics = pi.Graphics;
		size.Width += 6;
		if (_Text.Length > 0)
		{
			Size size2 = TextDrawing.MeasureString(graphics, _Text, pi.DefaultFont);
			size.Width += size2.Width;
			if (_Image != null)
			{
				size.Width += 4;
			}
		}
		if (_Image != null)
		{
			size.Width += _Image.Width;
		}
		if (_Text.Length == 0 && _Image == null)
		{
			size.Width += 9;
		}
		base.Size = size;
		base.CalculatedSize = size;
		base.PerformLayout(pi);
	}

	protected override void OnPaint(PaintInfo p)
	{
		Graphics graphics = p.Graphics;
		Rectangle renderBounds = base.RenderBounds;
		PaintButtonBackground(p);
		Rectangle bounds = renderBounds;
		bounds.Inflate(-3, -3);
		if (_Text.Length > 0)
		{
			TextDrawing.DrawString(graphics, _Text, p.DefaultFont, GetIsEnabled(p) ? p.ForeColor : p.DisabledForeColor, bounds, eTextFormat.VerticalCenter);
		}
		if (_Image != null)
		{
			Image val = (GetIsEnabled(p) ? _Image : GetDisabledImage());
			graphics.DrawImage(val, new Rectangle(bounds.Right - _Image.Width, bounds.Y + (bounds.Height - _Image.Height) / 2, _Image.Width, _Image.Height));
		}
		base.OnPaint(p);
	}

	protected virtual void PaintButtonBackground(PaintInfo p)
	{
		PaintButtonBackground(p, GetOffice2007StateColorTable(p));
	}

	protected virtual void PaintButtonBackground(PaintInfo p, Office2007ButtonItemStateColorTable ct)
	{
		Graphics graphics = p.Graphics;
		Rectangle renderBounds = base.RenderBounds;
		if (RenderBackground(p))
		{
			Office2007ButtonItemPainter.PaintBackground(graphics, ct, renderBounds, RoundRectangleShapeDescriptor.RectangleShape);
		}
	}

	private bool RenderBackground(PaintInfo p)
	{
		if (base.RenderDefaultBackground)
		{
			return true;
		}
		if ((!p.MouseOver && !IsMouseDown && !IsMouseOver && !Checked) || !GetIsEnabled())
		{
			return false;
		}
		return true;
	}

	protected Office2007ButtonItemStateColorTable GetOffice2007StateColorTable(PaintInfo p)
	{
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			Office2007ColorTable colorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
			Office2007ButtonItemColorTable office2007ButtonItemColorTable = colorTable.ButtonItemColors[Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground)];
			if (!GetIsEnabled(p))
			{
				return office2007ButtonItemColorTable.Disabled;
			}
			if (IsMouseDown)
			{
				return office2007ButtonItemColorTable.Pressed;
			}
			if (IsMouseOver)
			{
				return office2007ButtonItemColorTable.MouseOver;
			}
			if (Checked)
			{
				return office2007ButtonItemColorTable.Checked;
			}
			return office2007ButtonItemColorTable.Default;
		}
		return null;
	}

	protected override void OnMouseEnter()
	{
		if (GetIsEnabled())
		{
			IsMouseOver = true;
		}
		base.OnMouseEnter();
	}

	protected override void OnMouseLeave()
	{
		IsMouseOver = false;
		base.OnMouseLeave();
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576)
		{
			IsMouseDown = true;
		}
		base.OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576)
		{
			IsMouseDown = false;
		}
		base.OnMouseUp(e);
	}

	private Image GetDisabledImage()
	{
		if (_DisabledImage == null && _Image != null)
		{
			ref Image disabledImage = ref _DisabledImage;
			Image image = _Image;
			disabledImage = (Image)(object)ImageHelper.CreateGrayScaleImage((image is Bitmap) ? image : null);
		}
		return _DisabledImage;
	}
}

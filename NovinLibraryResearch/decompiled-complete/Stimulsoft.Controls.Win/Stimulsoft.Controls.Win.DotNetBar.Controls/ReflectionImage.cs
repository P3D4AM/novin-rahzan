using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxBitmap(typeof(ReflectionImage), "Controls.ReflectionImage.ico")]
[ToolboxItem(false)]
public class ReflectionImage : ControlWithBackgroundStyle
{
	private Bitmap _ReflectionBitmap;

	private Bitmap _DisabledBitmap;

	private Image _Image;

	private bool _ReflectionEnabled = true;

	[DefaultValue(null)]
	[Description("Indicates image displayed on the control.")]
	[Category("Appearance")]
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
				OnImageChanged();
			}
		}
	}

	protected override Size DefaultSize => new Size(128, 128);

	[DefaultValue(true)]
	[Description("Indicates whether reflection effect is enabled.")]
	public bool ReflectionEnabled
	{
		get
		{
			return _ReflectionEnabled;
		}
		set
		{
			if (_ReflectionEnabled != value)
			{
				_ReflectionEnabled = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(false)]
	public override Image BackgroundImage
	{
		get
		{
			return ((Control)this).BackgroundImage;
		}
		set
		{
			((Control)this).BackgroundImage = value;
		}
	}

	[Browsable(false)]
	public override Font Font
	{
		get
		{
			return ((Control)this).Font;
		}
		set
		{
			((Control)this).Font = value;
		}
	}

	[Browsable(false)]
	public override Color ForeColor
	{
		get
		{
			return ((Control)this).ForeColor;
		}
		set
		{
			((Control)this).ForeColor = value;
		}
	}

	[Browsable(false)]
	public override ImageLayout BackgroundImageLayout
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).BackgroundImageLayout;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).BackgroundImageLayout = value;
		}
	}

	[Browsable(false)]
	public Padding Padding
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).Padding;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Padding = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override string Text
	{
		get
		{
			return ((Control)this).Text;
		}
		set
		{
			((Control)this).Text = value;
		}
	}

	protected override void Dispose(bool disposing)
	{
		DisposeReflectionImage();
		DisposeDisabledImage();
		if (BarUtilities.DisposeItemImages && !((Component)(object)this).DesignMode)
		{
			BarUtilities.DisposeImage(ref _Image);
		}
		((Control)this).Dispose(disposing);
	}

	private void OnImageChanged()
	{
		DisposeDisabledImage();
		CreateReflectionImage();
	}

	private void CreateReflectionImage()
	{
		DisposeReflectionImage();
		Image image = GetImage();
		if (image != null)
		{
			_ReflectionBitmap = ImageHelper.CreateReflectionImage(image);
		}
		((Control)this).Invalidate();
	}

	private Image GetImage()
	{
		if (((Control)this).Enabled)
		{
			return _Image;
		}
		if (_DisabledBitmap == null)
		{
			_DisabledBitmap = ImageHelper.CreateGrayScaleImage(_Image);
		}
		return (Image)(object)_DisabledBitmap;
	}

	private void DisposeReflectionImage()
	{
		if (_ReflectionBitmap != null)
		{
			((Image)_ReflectionBitmap).Dispose();
			_ReflectionBitmap = null;
		}
	}

	private void DisposeDisabledImage()
	{
		if (_DisabledBitmap != null)
		{
			((Image)_DisabledBitmap).Dispose();
			_DisabledBitmap = null;
		}
	}

	protected override void PaintContent(PaintEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		Image image = GetImage();
		if (image != null)
		{
			Graphics graphics = e.Graphics;
			SmoothingMode smoothingMode = graphics.SmoothingMode;
			if (AntiAlias)
			{
				graphics.SmoothingMode = (SmoothingMode)2;
			}
			bool disposeStyle = false;
			ElementStyle backgroundStyle = GetBackgroundStyle(out disposeStyle);
			Rectangle contentRectangle = GetContentRectangle();
			Point location = contentRectangle.Location;
			if (backgroundStyle.TextAlignment == eStyleTextAlignment.Center && contentRectangle.Width > image.Width)
			{
				location.X += (contentRectangle.Width - image.Width) / 2;
			}
			else if (backgroundStyle.TextAlignment == eStyleTextAlignment.Far && contentRectangle.Width > image.Width)
			{
				location.X += contentRectangle.Width - image.Width;
			}
			float num = 0.52f;
			if (backgroundStyle.TextLineAlignment == eStyleTextAlignment.Center && (float)contentRectangle.Height > (float)image.Height + (float)image.Height * num)
			{
				location.Y += (int)(((float)contentRectangle.Height - ((float)image.Height + (float)image.Height * num)) / 2f);
			}
			else if (backgroundStyle.TextLineAlignment == eStyleTextAlignment.Far && (float)contentRectangle.Height > (float)image.Height + (float)image.Height * num)
			{
				location.Y += (int)((float)contentRectangle.Height - ((float)image.Height + (float)image.Height * num));
			}
			graphics.DrawImage(image, location.X, location.Y, image.Width, image.Height);
			if (_ReflectionBitmap != null && _ReflectionEnabled)
			{
				location.Y += image.Height;
				graphics.DrawImage((Image)(object)_ReflectionBitmap, location);
			}
			if (disposeStyle)
			{
				backgroundStyle.Dispose();
			}
			if (AntiAlias)
			{
				graphics.SmoothingMode = smoothingMode;
			}
		}
	}

	protected override void OnEnabledChanged(EventArgs e)
	{
		CreateReflectionImage();
		((Control)this).OnEnabledChanged(e);
	}
}

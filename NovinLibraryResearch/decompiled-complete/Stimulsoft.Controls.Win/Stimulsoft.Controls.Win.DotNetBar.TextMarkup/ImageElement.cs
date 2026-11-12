using System;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Xml;

namespace Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

internal class ImageElement : MarkupElement
{
	private Size m_ImageSize = Size.Empty;

	private string m_ImageSource = "";

	private Image m_Image;

	public override bool IsBlockElement => false;

	public override void Measure(Size availableSize, MarkupDrawContext d)
	{
		if (!m_ImageSize.IsEmpty)
		{
			base.Bounds = new Rectangle(Point.Empty, m_ImageSize);
			return;
		}
		if (m_ImageSource.Length == 0)
		{
			base.Bounds = Rectangle.Empty;
			return;
		}
		Image image = GetImage();
		if (image != null)
		{
			base.Bounds = new Rectangle(Point.Empty, image.Size);
		}
		else
		{
			base.Bounds = new Rectangle(Point.Empty, new Size(16, 16));
		}
	}

	private Image GetImage()
	{
		if (m_Image != null)
		{
			return m_Image;
		}
		Assembly assembly = null;
		ResolveImageEventArgs resolveImageEventArgs = new ResolveImageEventArgs();
		resolveImageEventArgs.Key = m_ImageSource;
		resolveImageEventArgs.ResolvedImage = null;
		MarkupSettings.InvokeResolveImage(resolveImageEventArgs);
		if (resolveImageEventArgs.Handled)
		{
			return resolveImageEventArgs.ResolvedImage;
		}
		if (m_ImageSource.IndexOf('/') >= 0)
		{
			string[] array = m_ImageSource.Split(new char[1] { '/' });
			assembly = Assembly.Load(array[0]);
			string text = array[1];
			if (assembly != null)
			{
				m_Image = LoadImageGlobalResource(array[1], assembly);
				if (m_Image == null)
				{
					m_Image = LoadImageResource(array[1], assembly);
				}
				if (m_Image != null)
				{
					return m_Image;
				}
			}
		}
		assembly = Assembly.GetExecutingAssembly();
		m_Image = LoadImageGlobalResource(m_ImageSource, assembly);
		if (m_Image == null)
		{
			m_Image = LoadImageResource(m_ImageSource, assembly);
		}
		if (m_Image == null)
		{
			assembly = Assembly.GetEntryAssembly();
			m_Image = LoadImageGlobalResource(m_ImageSource, assembly);
			if (m_Image == null)
			{
				m_Image = LoadImageResource(m_ImageSource, assembly);
			}
		}
		return m_Image;
	}

	private Image LoadImageGlobalResource(string imageSource, Assembly a)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		Image result = null;
		if (imageSource.StartsWith("global::"))
		{
			string text = imageSource.Substring(8);
			if (text.Length > 0)
			{
				try
				{
					int num = text.LastIndexOf('.');
					string baseName = text.Substring(0, num);
					text = text.Substring(num + 1);
					ResourceManager resourceManager = new ResourceManager(baseName, a);
					object @object = resourceManager.GetObject(text);
					result = (Image)(Bitmap)@object;
				}
				catch
				{
					result = null;
				}
			}
		}
		return result;
	}

	private Image LoadImageResource(string imageSource, Assembly a)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected O, but got Unknown
		Image result = null;
		try
		{
			result = (Image)new Bitmap(a.GetManifestResourceStream(imageSource));
		}
		catch
		{
		}
		return result;
	}

	public override void Render(MarkupDrawContext d)
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		Rectangle bounds = base.Bounds;
		bounds.Offset(d.Offset);
		if (!d.ClipRectangle.IsEmpty && !bounds.IntersectsWith(d.ClipRectangle))
		{
			return;
		}
		Image image = GetImage();
		if (image != null)
		{
			Rectangle rectangle = bounds;
			if (m_ImageSize.IsEmpty)
			{
				rectangle.Size = image.Size;
			}
			else
			{
				rectangle.Size = m_ImageSize;
			}
			d.Graphics.DrawImage(image, rectangle);
		}
		else
		{
			SolidBrush val = new SolidBrush(Color.White);
			try
			{
				d.Graphics.FillRectangle((Brush)(object)val, bounds);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			Pen val2 = new Pen(Color.DarkGray, 1f);
			try
			{
				d.Graphics.DrawRectangle(val2, bounds);
				d.Graphics.DrawLine(val2, bounds.X, bounds.Y, bounds.Right, bounds.Bottom);
				d.Graphics.DrawLine(val2, bounds.Right, bounds.Y, bounds.X, bounds.Bottom);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		base.RenderBounds = bounds;
	}

	protected override void ArrangeCore(Rectangle finalRect, MarkupDrawContext d)
	{
	}

	public override void ReadAttributes(XmlTextReader reader)
	{
		m_ImageSize = Size.Empty;
		m_ImageSource = "";
		m_Image = null;
		for (int i = 0; i < reader.AttributeCount; i++)
		{
			reader.MoveToAttribute(i);
			if (reader.Name.ToLower() == "width")
			{
				string value = reader.Value;
				m_ImageSize.Width = int.Parse(value);
			}
			else if (reader.Name.ToLower() == "height")
			{
				string value2 = reader.Value;
				m_ImageSize.Height = int.Parse(value2);
			}
			else if (reader.Name.ToLower() == "src")
			{
				m_ImageSource = reader.Value;
			}
		}
	}
}

using System;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Xml;

namespace DevComponents.DotNetBar.TextMarkup;

internal class ሹ : _0CBF
{
	private Size ਉ = Size.Empty;

	private string ሺ = "";

	private Image _0E66;

	public override bool IsBlockElement => false;

	public override void Measure(Size availableSize, MarkupDrawContext d)
	{
		if (!ਉ.IsEmpty)
		{
			base.Bounds = new Rectangle(Point.Empty, ਉ);
			return;
		}
		if (ሺ.Length == 0)
		{
			base.Bounds = Rectangle.Empty;
			return;
		}
		Image val = _0EC5();
		if (val != null)
		{
			base.Bounds = new Rectangle(Point.Empty, val.Size);
		}
		else
		{
			base.Bounds = new Rectangle(Point.Empty, new Size(16, 16));
		}
	}

	private Image _0EC5()
	{
		if (_0E66 != null)
		{
			return _0E66;
		}
		Assembly assembly = null;
		ResolveImageEventArgs resolveImageEventArgs = new ResolveImageEventArgs();
		resolveImageEventArgs.Key = ሺ;
		resolveImageEventArgs.ResolvedImage = null;
		MarkupSettings.ഏ(resolveImageEventArgs);
		if (resolveImageEventArgs.Handled)
		{
			return resolveImageEventArgs.ResolvedImage;
		}
		if (ሺ.IndexOf('/') >= 0)
		{
			string[] array = ሺ.Split(new char[1] { '/' });
			assembly = Assembly.Load(array[0]);
			_ = array[1];
			if ((object)assembly != null)
			{
				_0E66 = ሻ(array[1], assembly);
				if (_0E66 == null)
				{
					_0E66 = ሾ(array[1], assembly);
				}
				if (_0E66 != null)
				{
					return _0E66;
				}
			}
		}
		assembly = Assembly.GetExecutingAssembly();
		_0E66 = ሻ(ሺ, assembly);
		if (_0E66 == null)
		{
			_0E66 = ሾ(ሺ, assembly);
		}
		if (_0E66 == null)
		{
			assembly = Assembly.GetEntryAssembly();
			_0E66 = ሻ(ሺ, assembly);
			if (_0E66 == null)
			{
				_0E66 = ሾ(ሺ, assembly);
			}
		}
		return _0E66;
	}

	private Image ሻ(string ሼ, Assembly ሽ)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		Image result = null;
		if (ሼ.StartsWith("global::"))
		{
			string text = ሼ.Substring(8);
			if (text.Length > 0)
			{
				try
				{
					int num = text.LastIndexOf('.');
					string baseName = text.Substring(0, num);
					text = text.Substring(num + 1);
					ResourceManager resourceManager = new ResourceManager(baseName, ሽ);
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

	private Image ሾ(string ሼ, Assembly ሽ)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected O, but got Unknown
		Image result = null;
		try
		{
			result = (Image)new Bitmap(ሽ.GetManifestResourceStream(ሼ));
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
		Image val = _0EC5();
		if (val != null)
		{
			Rectangle rectangle = bounds;
			if (ਉ.IsEmpty)
			{
				rectangle.Size = val.Size;
			}
			else
			{
				rectangle.Size = ਉ;
			}
			d.Graphics.DrawImage(val, rectangle);
		}
		else
		{
			SolidBrush val2 = new SolidBrush(Color.White);
			try
			{
				d.Graphics.FillRectangle((Brush)(object)val2, bounds);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			Pen val3 = new Pen(Color.DarkGray, 1f);
			try
			{
				d.Graphics.DrawRectangle(val3, bounds);
				d.Graphics.DrawLine(val3, bounds.X, bounds.Y, bounds.Right, bounds.Bottom);
				d.Graphics.DrawLine(val3, bounds.Right, bounds.Y, bounds.X, bounds.Bottom);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		base._0CC0 = bounds;
	}

	protected override void ArrangeCore(Rectangle finalRect, MarkupDrawContext d)
	{
	}

	public override void ReadAttributes(XmlTextReader reader)
	{
		ਉ = Size.Empty;
		ሺ = "";
		_0E66 = null;
		for (int i = 0; i < reader.AttributeCount; i++)
		{
			reader.MoveToAttribute(i);
			if (reader.Name.ToLower() == "width")
			{
				string value = reader.Value;
				ਉ.Width = int.Parse(value);
			}
			else if (reader.Name.ToLower() == "height")
			{
				string value2 = reader.Value;
				ਉ.Height = int.Parse(value2);
			}
			else if (reader.Name.ToLower() == "src")
			{
				ሺ = reader.Value;
			}
		}
	}
}

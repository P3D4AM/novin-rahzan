using System;
using System.ComponentModel;
using System.Drawing;

namespace DevComponents.DotNetBar;

[DesignTimeVisible(false)]
[TypeConverter(typeof(DocumentDockContainerConverter))]
[ToolboxItem(false)]
public class DocumentDockContainer : DocumentBaseContainer
{
	private struct _10CA
	{
		public int _10CB;

		public int _10CC;
	}

	private bool _Ⴏ;

	private eOrientation Ⴐ;

	private DocumentBaseContainerCollection Ⴑ = new DocumentBaseContainerCollection();

	private int Ⴒ = 3;

	private Size Ⴓ = Size.Empty;

	private bool Ⴔ;

	private Point _Ⴕ = Point.Empty;

	internal Point Ⴎ
	{
		get
		{
			return _Ⴕ;
		}
		set
		{
			_Ⴕ = value;
		}
	}

	public override bool Visible
	{
		get
		{
			foreach (DocumentBaseContainer item in Ⴑ)
			{
				if (item.Visible)
				{
					return true;
				}
			}
			return false;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public eOrientation Orientation
	{
		get
		{
			return Ⴐ;
		}
		set
		{
			if (Ⴐ != value)
			{
				Ⴐ = value;
				Ⴞ();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public DocumentBaseContainerCollection Documents => Ⴑ;

	protected internal override Size MinimumSize => Ⴓ;

	[DefaultValue(3)]
	[Browsable(true)]
	[Category("Layout")]
	[Description("Indicates the splitter size between the documents docking inside the container.")]
	public virtual int SplitterSize
	{
		get
		{
			return Ⴒ;
		}
		set
		{
			Ⴒ = value;
			foreach (DocumentBaseContainer item in Ⴑ)
			{
				if (item is DocumentDockContainer documentDockContainer)
				{
					documentDockContainer.SplitterSize = value;
				}
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool RecordDocumentSize
	{
		get
		{
			return Ⴔ;
		}
		set
		{
			Ⴔ = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool OversizeEnabled
	{
		get
		{
			return _Ⴏ;
		}
		set
		{
			_Ⴏ = value;
		}
	}

	public DocumentDockContainer(DocumentBaseContainer[] documents, eOrientation orientation)
	{
		Ⴐ = orientation;
		Ⴑ._၉ = this;
		Ⴑ.DocumentAdded += Ⴛ;
		Ⴑ.DocumentRemoved += Ⴜ;
		if (documents != null)
		{
			foreach (DocumentBaseContainer tab in documents)
			{
				Ⴑ.Add(tab);
			}
		}
	}

	public DocumentDockContainer()
		: this(null, eOrientation.Horizontal)
	{
	}

	public override void Layout(Rectangle bounds)
	{
		if (Ⴚ(bounds))
		{
			if (!_Ⴏ)
			{
				_10C8(bounds);
			}
		}
		else
		{
			_10C8(Rectangle.Empty);
		}
	}

	private _10CA Ⴖ(Rectangle ശ)
	{
		_10CA result = default(_10CA);
		if (Ⴑ.Count == 0)
		{
			return result;
		}
		int num = ((Ⴐ == eOrientation.Horizontal) ? ശ.Width : ശ.Height);
		int num2 = 0;
		int[] array = new int[Ⴑ.Count];
		bool flag = false;
		int num3 = Ⴑ.Count;
		int num4 = Dpi.Width(Ⴒ);
		do
		{
			num2 = (num - (num3 - 1) * num4) / num3;
			flag = false;
			for (int i = 0; i < Ⴑ.Count; i++)
			{
				if (array[i] == 0)
				{
					DocumentBaseContainer documentBaseContainer = Ⴑ[i];
					if (Ⴐ == eOrientation.Horizontal && documentBaseContainer.LayoutBounds.Width == 0 && documentBaseContainer.MinimumSize.Width > num2)
					{
						array[i] = documentBaseContainer.MinimumSize.Width;
						flag = true;
						num -= array[i];
						num3--;
						break;
					}
					if (Ⴐ == eOrientation.Vertical && documentBaseContainer.LayoutBounds.Height == 0 && documentBaseContainer.MinimumSize.Height > num2)
					{
						array[i] = documentBaseContainer.MinimumSize.Height;
						flag = true;
						num -= array[i];
						num3--;
						break;
					}
				}
			}
		}
		while (flag && num3 > 0);
		int num5 = 0;
		int num6 = 0;
		for (int j = 0; j < Ⴑ.Count; j++)
		{
			DocumentBaseContainer documentBaseContainer2 = Ⴑ[j];
			if (Ⴐ == eOrientation.Horizontal && documentBaseContainer2.LayoutBounds.Width == 0)
			{
				documentBaseContainer2._10C9(new Rectangle(0, 0, (array[j] > 0) ? array[j] : num2, ശ.Height));
			}
			else if (Ⴐ == eOrientation.Vertical && documentBaseContainer2.LayoutBounds.Height == 0)
			{
				documentBaseContainer2._10C9(new Rectangle(0, 0, ശ.Width, (array[j] > 0) ? array[j] : num2));
			}
			if (documentBaseContainer2.Visible)
			{
				num5 += ((Ⴐ == eOrientation.Horizontal) ? documentBaseContainer2.LayoutBounds.Width : documentBaseContainer2.LayoutBounds.Height) + num4;
				num6++;
			}
		}
		if (num6 > 0)
		{
			num5 -= num4;
		}
		result._10CB = num5;
		result._10CC = num6;
		return result;
	}

	private bool Ⴗ(float Ⴘ, Rectangle Ⴙ)
	{
		bool flag = false;
		foreach (DocumentBaseContainer item in Ⴑ)
		{
			if (!item.Visible)
			{
				continue;
			}
			Rectangle rectangle;
			if (Ⴐ == eOrientation.Horizontal)
			{
				rectangle = new Rectangle(Ⴙ.X, Ⴙ.Y, (int)((float)item.LayoutBounds.Width * Ⴘ), Ⴙ.Height);
				if (rectangle.Width < item.MinimumSize.Width)
				{
					flag = true;
					break;
				}
			}
			else
			{
				rectangle = new Rectangle(Ⴙ.X, Ⴙ.Y, Ⴙ.Width, (int)((float)item.LayoutBounds.Height * Ⴘ));
				if (rectangle.Height < item.MinimumSize.Height)
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			foreach (DocumentBaseContainer item2 in Ⴑ)
			{
				item2._10C9(Rectangle.Empty);
			}
		}
		return flag;
	}

	private bool Ⴚ(Rectangle ശ)
	{
		if (Ⴑ.Count == 0)
		{
			return false;
		}
		int num = ((Ⴐ == eOrientation.Horizontal) ? ശ.Width : ശ.Height);
		_10CA obj = Ⴖ(ശ);
		if (obj._10CC == 0)
		{
			return false;
		}
		int num2 = obj._10CB;
		int num3 = obj._10CC;
		float num4 = (float)num / (float)num2;
		if (Ⴗ(num4, ശ))
		{
			obj = Ⴖ(ശ);
		}
		Rectangle rectangle = ശ;
		int num5 = 0;
		bool flag = true;
		if (!Ⴔ && Math.Abs(num2 - num) / Ⴑ.Count != Math.Abs(num2 - num) / Ⴑ.Count * Ⴑ.Count)
		{
			flag = false;
		}
		bool flag2 = num2 != num;
		Rectangle rectangle2 = Rectangle.Empty;
		for (int i = 0; i < Ⴑ.Count; i++)
		{
			DocumentBaseContainer documentBaseContainer = Ⴑ[i];
			if (documentBaseContainer.Visible)
			{
				num5++;
				Rectangle rectangle3 = (flag2 ? ((Ⴐ == eOrientation.Horizontal) ? ((!_Ⴏ) ? new Rectangle(rectangle.X, rectangle.Y, (int)((float)documentBaseContainer.LayoutBounds.Width * num4), rectangle.Height) : new Rectangle(rectangle.X, rectangle.Y, (int)Math.Max(documentBaseContainer.MinimumSize.Width, (float)documentBaseContainer.LayoutBounds.Width * num4), Math.Max(rectangle.Height, documentBaseContainer.MinimumSize.Height))) : ((!_Ⴏ) ? new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, (int)((float)documentBaseContainer.LayoutBounds.Height * num4)) : new Rectangle(rectangle.X, rectangle.Y, Math.Max(rectangle.Width, documentBaseContainer.MinimumSize.Width), (int)Math.Max(documentBaseContainer.MinimumSize.Height, (float)documentBaseContainer.LayoutBounds.Height * num4)))) : ((Ⴐ == eOrientation.Horizontal) ? ((!_Ⴏ) ? new Rectangle(rectangle.X, rectangle.Y, documentBaseContainer.LayoutBounds.Width, rectangle.Height) : new Rectangle(rectangle.X, rectangle.Y, documentBaseContainer.LayoutBounds.Width, Math.Max(rectangle.Height, documentBaseContainer.MinimumSize.Height))) : ((!_Ⴏ) ? new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, documentBaseContainer.LayoutBounds.Height) : new Rectangle(rectangle.X, rectangle.Y, Math.Max(rectangle.Width, documentBaseContainer.MinimumSize.Width), documentBaseContainer.LayoutBounds.Height))));
				if (num5 == num3 && (!_Ⴏ || (rectangle3.Width < rectangle.Width && Ⴐ == eOrientation.Horizontal) || (rectangle3.Height < rectangle.Height && Ⴐ == eOrientation.Vertical)))
				{
					rectangle3 = rectangle;
				}
				Rectangle rectangle4 = rectangle3;
				rectangle4.Offset(_Ⴕ);
				documentBaseContainer.Layout(rectangle4);
				if (flag)
				{
					documentBaseContainer._10C9(rectangle4);
				}
				rectangle2 = ((!rectangle2.IsEmpty) ? Rectangle.Union(rectangle2, rectangle4) : rectangle4);
				int num6 = Dpi.Width(Ⴒ);
				if (Ⴐ == eOrientation.Horizontal)
				{
					rectangle.Width -= rectangle3.Width + num6;
					rectangle.X += rectangle3.Width + num6;
				}
				else
				{
					rectangle.Height -= rectangle3.Height + num6;
					rectangle.Y += rectangle3.Height + num6;
				}
			}
		}
		if (_Ⴏ)
		{
			_10C8(rectangle2);
		}
		return true;
	}

	protected internal override bool OnSetWidth(DocumentBaseContainer doc, int width)
	{
		bool result = false;
		if (Ⴐ != 0 || (width < doc.MinimumSize.Width && doc.MinimumSize.Width > 0))
		{
			return result;
		}
		DocumentBaseContainer documentBaseContainer = null;
		DocumentBaseContainer documentBaseContainer2 = null;
		int num = Ⴑ.IndexOf(doc);
		for (int i = 0; i < Ⴑ.Count; i++)
		{
			DocumentBaseContainer documentBaseContainer3 = Ⴑ[i];
			if (documentBaseContainer3.Visible)
			{
				if (documentBaseContainer3.DisplayBounds.Width > 0)
				{
					documentBaseContainer3._10C9(documentBaseContainer3.DisplayBounds);
				}
				if (i > num && documentBaseContainer3.Visible && documentBaseContainer == null)
				{
					documentBaseContainer = documentBaseContainer3;
				}
				else if (i < num && documentBaseContainer3.Visible)
				{
					documentBaseContainer2 = documentBaseContainer3;
				}
			}
		}
		int num2 = doc.LayoutBounds.Width - width;
		if (documentBaseContainer != null && documentBaseContainer.LayoutBounds.Width > 0 && documentBaseContainer.LayoutBounds.Width + num2 > 0 && (documentBaseContainer.LayoutBounds.Width + num2 >= documentBaseContainer.MinimumSize.Width || documentBaseContainer.MinimumSize.Width == 0))
		{
			documentBaseContainer._10C9(new Rectangle(documentBaseContainer.LayoutBounds.X, documentBaseContainer.LayoutBounds.Y, documentBaseContainer.LayoutBounds.Width + num2, documentBaseContainer.LayoutBounds.Height));
			result = true;
		}
		else if (documentBaseContainer == null && documentBaseContainer2 != null && documentBaseContainer2.LayoutBounds.Width > 0 && documentBaseContainer2.LayoutBounds.Width + num2 > 0 && (documentBaseContainer2.LayoutBounds.Width + num2 >= documentBaseContainer2.MinimumSize.Width || documentBaseContainer2.MinimumSize.Width == 0))
		{
			doc._10C9(new Rectangle(doc.LayoutBounds.X, doc.LayoutBounds.Y, width, doc.LayoutBounds.Height));
			result = true;
		}
		return result;
	}

	protected internal override bool OnSetHeight(DocumentBaseContainer doc, int height)
	{
		bool result = false;
		if (Ⴐ != eOrientation.Vertical || (height < doc.MinimumSize.Height && doc.MinimumSize.Height > 0))
		{
			return result;
		}
		DocumentBaseContainer documentBaseContainer = null;
		DocumentBaseContainer documentBaseContainer2 = null;
		int num = Ⴑ.IndexOf(doc);
		for (int i = 0; i < Ⴑ.Count; i++)
		{
			DocumentBaseContainer documentBaseContainer3 = Ⴑ[i];
			if (documentBaseContainer3.Visible)
			{
				if (documentBaseContainer3.DisplayBounds.Height > 0)
				{
					documentBaseContainer3._10C9(documentBaseContainer3.DisplayBounds);
				}
				if (i > num && documentBaseContainer3.Visible && documentBaseContainer == null)
				{
					documentBaseContainer = documentBaseContainer3;
				}
				else if (i < num && documentBaseContainer3.Visible)
				{
					documentBaseContainer2 = documentBaseContainer3;
				}
			}
		}
		if (documentBaseContainer == null)
		{
			documentBaseContainer = documentBaseContainer2;
		}
		int num2 = doc.LayoutBounds.Height - height;
		if (documentBaseContainer != null && documentBaseContainer.LayoutBounds.Height > 0 && documentBaseContainer.LayoutBounds.Height + num2 > 0 && (documentBaseContainer.LayoutBounds.Height + num2 >= documentBaseContainer.MinimumSize.Height || documentBaseContainer.MinimumSize.Height == 0))
		{
			documentBaseContainer._10C9(new Rectangle(documentBaseContainer.LayoutBounds.X, documentBaseContainer.LayoutBounds.Y, documentBaseContainer.LayoutBounds.Width, documentBaseContainer.LayoutBounds.Height + num2));
			result = true;
		}
		else if (documentBaseContainer == null && documentBaseContainer2 != null && documentBaseContainer2.LayoutBounds.Height > 0 && documentBaseContainer2.LayoutBounds.Height + num2 > 0 && (documentBaseContainer2.LayoutBounds.Height + num2 >= documentBaseContainer2.MinimumSize.Height || documentBaseContainer2.MinimumSize.Height == 0))
		{
			doc._10C9(new Rectangle(doc.LayoutBounds.X, doc.LayoutBounds.Y, doc.LayoutBounds.Width, height));
			documentBaseContainer2._10C9(new Rectangle(documentBaseContainer2.LayoutBounds.X, documentBaseContainer2.LayoutBounds.Y, documentBaseContainer2.LayoutBounds.Width, 0));
			result = true;
		}
		return result;
	}

	public DocumentBarContainer GetBarDocumentContainer(Bar bar)
	{
		foreach (DocumentBaseContainer item in Ⴑ)
		{
			if (item is DocumentBarContainer && ((DocumentBarContainer)item).Bar == bar)
			{
				return (DocumentBarContainer)item;
			}
			if (item is DocumentDockContainer)
			{
				DocumentBarContainer barDocumentContainer = ((DocumentDockContainer)item).GetBarDocumentContainer(bar);
				if (barDocumentContainer != null)
				{
					return barDocumentContainer;
				}
			}
		}
		return null;
	}

	private void Ⴛ(object _٠, EventArgs ؿ)
	{
		DocumentBaseContainer documentBaseContainer = _٠ as DocumentBaseContainer;
		Size minimumSize = documentBaseContainer.MinimumSize;
		int num = Dpi.Width(Ⴒ);
		if (Ⴐ == eOrientation.Horizontal)
		{
			Ⴓ.Width += minimumSize.Width + num;
			if (minimumSize.Height > Ⴓ.Height)
			{
				Ⴓ.Height = minimumSize.Height;
			}
		}
		else
		{
			Ⴓ.Height += minimumSize.Height + num;
			if (minimumSize.Width > Ⴓ.Width)
			{
				Ⴓ.Width = minimumSize.Width;
			}
		}
	}

	internal void Ⴜ(object _٠, EventArgs ؿ)
	{
		Ⴝ();
	}

	internal void Ⴝ()
	{
		Ⴓ = Size.Empty;
		foreach (DocumentBaseContainer item in Ⴑ)
		{
			Size minimumSize = item.MinimumSize;
			int num = Dpi.Width(Ⴒ);
			if (Ⴐ == eOrientation.Horizontal)
			{
				Ⴓ.Width += minimumSize.Width + num;
				if (minimumSize.Height > Ⴓ.Height)
				{
					Ⴓ.Height = minimumSize.Height;
				}
			}
			else
			{
				Ⴓ.Height += minimumSize.Height + num;
				if (minimumSize.Width > Ⴓ.Width)
				{
					Ⴓ.Width = minimumSize.Width;
				}
			}
		}
	}

	private void Ⴞ()
	{
		Ⴟ();
	}

	private void Ⴟ()
	{
		foreach (DocumentBaseContainer item in Ⴑ)
		{
			item._10C9(Rectangle.Empty);
		}
	}

	internal override void Ⴠ(int Ⴡ, int Ⴢ, bool Ⴣ)
	{
		base.Ⴠ(Ⴡ, Ⴢ, Ⴣ);
		foreach (DocumentBaseContainer item in Ⴑ)
		{
			item.Ⴠ(Ⴡ, Ⴢ, Ⴣ);
		}
	}
}

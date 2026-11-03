using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar.TextMarkup;

internal interface _0CFC
{
	bool _0CFD(int و, int ى);

	void બ(Control ԟ);

	void ભ(Control ԟ);

	void ग़(Control ԟ, MouseEventArgs ؿ);

	void ફ(Control ԟ, MouseEventArgs ؿ);

	void દ(Control ԟ);
}
internal class _0CB4 : _0CBC
{
	private _0CC9 ಸ;

	private _0CFC ಹ;

	internal bool _0CBA;

	internal string _0CBB = "";

	public _0CC9 ಶ => ಸ;

	public _0CFC ಷ => ಹ;

	public event EventHandler ವ;

	public _0CB4()
	{
		ಸ = new _0CC9(null);
	}

	public void ભ(Control ԟ)
	{
		if (ಹ != null)
		{
			ಹ.ભ(ԟ);
		}
		ಹ = null;
	}

	public void મ(Control ԟ, MouseEventArgs ؿ)
	{
		if (ಹ != null && ಹ._0CFD(ؿ.X, ؿ.Y))
		{
			return;
		}
		if (ಹ != null)
		{
			ಹ.ભ(ԟ);
		}
		ಹ = null;
		foreach (_0CFC item in ಸ)
		{
			if (item._0CFD(ؿ.X, ؿ.Y))
			{
				ಹ = item;
				ಹ.બ(ԟ);
			}
		}
	}

	public void ग़(Control ԟ, MouseEventArgs ؿ)
	{
		if (ಹ != null)
		{
			ಹ.ग़(ԟ, ؿ);
		}
	}

	public void ફ(Control ԟ, MouseEventArgs ؿ)
	{
		if (ಹ != null)
		{
			ಹ.ફ(ԟ, ؿ);
		}
	}

	public void દ(Control ԟ)
	{
		if (ಹ != null)
		{
			ಹ.દ(ԟ);
			if (ಹ is _0CFE && this.ವ != null)
			{
				this.ವ(ಹ, new EventArgs());
			}
		}
	}
}
internal class _0CBC : _0CBF
{
	private SerialContentLayoutManager _0594;

	private _0CF9 ಽ;

	public override bool IsBlockElement => true;

	public override void Measure(Size availableSize, MarkupDrawContext d)
	{
		ArrangeInternal(new Rectangle(Point.Empty, availableSize), d);
	}

	protected virtual SerialContentLayoutManager GetLayoutManager(bool mutliLine)
	{
		if (_0594 == null)
		{
			_0594 = new SerialContentLayoutManager();
			_0594.MultiLine = mutliLine;
			_0594.ContentVerticalAlignment = eContentVerticalAlignment.Top;
			_0594.BlockLineAlignment = eContentVerticalAlignment.Top;
			_0594.BlockSpacing = 0;
		}
		_0594.EvenHeight = true;
		return _0594;
	}

	private _0CF9 _0CBE()
	{
		if (ಽ == null)
		{
			ಽ = new _0CF9();
		}
		return ಽ;
	}

	public override void Render(MarkupDrawContext d)
	{
		Point containerOffset = GetContainerOffset();
		d.Offset.Offset(containerOffset.X, containerOffset.Y);
		try
		{
			foreach (_0CBF element in Elements)
			{
				element.Render(d);
			}
		}
		finally
		{
			d.Offset.Offset(-containerOffset.X, -containerOffset.Y);
		}
	}

	protected virtual Point GetContainerOffset()
	{
		return base.Bounds.Location;
	}

	protected override void ArrangeCore(Rectangle finalRect, MarkupDrawContext d)
	{
		base.Bounds = finalRect;
		ArrangeInternal(finalRect, d);
	}

	protected virtual void ArrangeInternal(Rectangle bounds, MarkupDrawContext d)
	{
		SerialContentLayoutManager layoutManager = GetLayoutManager(d.AllowMultiLine);
		layoutManager.RightToLeft = d.RightToLeft;
		_0CF9 obj = _0CBE();
		obj._0CFA = d;
		try
		{
			_0CBF[] array = new _0CBF[Elements.Count];
			Elements._0CD0(array);
			Rectangle rectangle = layoutManager.Layout(new Rectangle(Point.Empty, bounds.Size), array, obj);
			base.Bounds = new Rectangle(bounds.Location, rectangle.Size);
		}
		finally
		{
			obj._0CFA = null;
		}
	}
}
internal abstract class _0CBF : IBlock, IBlockExtended
{
	private _0CC9 _0CC1;

	private _0CBF ӭ;

	private Rectangle ݗ = Rectangle.Empty;

	private bool Ӻ = true;

	private bool _0CC2;

	private Rectangle _0CC3 = Rectangle.Empty;

	private Padding __0AE5 = new Padding(0);

	public virtual bool IsBlockContainer => false;

	public virtual bool IsBlockElement => false;

	public virtual bool IsNewLineAfterElement => false;

	public virtual bool CanStartNewLine => true;

	public virtual _0CC9 Elements
	{
		get
		{
			if (_0CC1 == null)
			{
				_0CC1 = new _0CC9(this);
			}
			return _0CC1;
		}
	}

	public virtual bool IsSizeValid
	{
		get
		{
			return _0CC2;
		}
		set
		{
			_0CC2 = value;
		}
	}

	public virtual _0CBF Parent => ӭ;

	public Rectangle Bounds
	{
		get
		{
			return ݗ;
		}
		set
		{
			ݗ = value;
		}
	}

	public bool Visible
	{
		get
		{
			return Ӻ;
		}
		set
		{
			Ӻ = value;
		}
	}

	public Padding Margin
	{
		get
		{
			return __0AE5;
		}
		set
		{
			__0AE5 = value;
		}
	}

	public Rectangle _0CC0
	{
		get
		{
			return _0CC3;
		}
		set
		{
			_0CC3 = value;
		}
	}

	public _0CBF()
	{
		_0CC1 = new _0CC9(this);
	}

	internal void _0CC4()
	{
		IsSizeValid = false;
		if (_0CC1 == null || _0CC1.Count == 0)
		{
			return;
		}
		foreach (_0CBF item in _0CC1)
		{
			item._0CC4();
			item.IsSizeValid = false;
		}
	}

	internal void _0CC5(_0CBF ԟ)
	{
		ӭ = ԟ;
	}

	public abstract void Measure(Size availableSize, MarkupDrawContext d);

	public virtual void MeasureEnd(Size availableSize, MarkupDrawContext d)
	{
	}

	public abstract void Render(MarkupDrawContext d);

	public virtual void RenderEnd(MarkupDrawContext d)
	{
	}

	protected abstract void ArrangeCore(Rectangle finalRect, MarkupDrawContext d);

	public void _0CC6(Rectangle _0CC7, MarkupDrawContext _0CC8)
	{
		ArrangeCore(_0CC7, _0CC8);
	}

	public virtual void ReadAttributes(XmlTextReader reader)
	{
	}
}
[DefaultMember("Item")]
internal class _0CC9 : CollectionBase
{
	private _0CBF ӭ;

	public _0CBF ކ
	{
		get
		{
			return ӭ;
		}
		set
		{
			ӭ = value;
		}
	}

	public _0CBF _0CCA
	{
		get
		{
			return (_0CBF)base.List[ԡ];
		}
		set
		{
			base.List[ԡ] = value;
		}
	}

	public _0CC9(_0CBF parent)
	{
		ӭ = parent;
	}

	public int _0CCB(_0CBF _0CBF)
	{
		return base.List.Add(_0CBF);
	}

	public void _0CCC(int ԡ, _0CBF ԝ)
	{
		base.List.Insert(ԡ, ԝ);
	}

	public int _0CCD(_0CBF ԝ)
	{
		return base.List.IndexOf(ԝ);
	}

	public bool _0CCE(_0CBF ԝ)
	{
		return base.List.Contains(ԝ);
	}

	public void _0CCF(_0CBF ԝ)
	{
		base.List.Remove(ԝ);
	}

	protected override void OnRemoveComplete(int index, object value)
	{
		base.OnRemoveComplete(index, value);
		_0CBF obj = value as _0CBF;
		if (ӭ != null)
		{
			obj._0CC5(null);
			ӭ.IsSizeValid = false;
		}
	}

	protected override void OnInsertComplete(int index, object value)
	{
		base.OnInsertComplete(index, value);
		_0CBF obj = value as _0CBF;
		if (ӭ != null)
		{
			obj._0CC5(ӭ);
			ӭ.IsSizeValid = false;
		}
	}

	public void _0CD0(_0CBF[] ঀ, int ԡ)
	{
		base.List.CopyTo(ঀ, ԡ);
	}

	internal void _0CD0(_0CBF[] ঀ)
	{
		base.List.CopyTo(ঀ, 0);
	}

	protected override void OnClear()
	{
		base.OnClear();
	}
}
internal class _0CF9 : BlockLayoutManager
{
	private MarkupDrawContext _0CFB;

	public MarkupDrawContext _0CFA
	{
		get
		{
			return _0CFB;
		}
		set
		{
			_0CFB = value;
		}
	}

	public override void Layout(IBlock block, Size availableSize)
	{
		if (block is _0CBF)
		{
			_0CBF obj = block as _0CBF;
			if (!obj.IsSizeValid)
			{
				obj.Measure(availableSize, _0CFB);
			}
		}
	}

	public override Rectangle FinalizeLayout(Rectangle containerBounds, Rectangle blocksBounds, ArrayList lines)
	{
		return blocksBounds;
	}
}
internal class _0CFE : _0CBF, _0CFC
{
	private Color _0D02 = Color.Empty;

	private Color _0D03 = Color.Empty;

	private string ഄ = "";

	private string Ӿ = "";

	private Cursor അ;

	private bool ആ;

	private bool ഇ;

	public Color _0CFF
	{
		get
		{
			return _0D02;
		}
		set
		{
			_0D02 = value;
		}
	}

	public string _0D00
	{
		get
		{
			return ഄ;
		}
		set
		{
			ഄ = value;
		}
	}

	public string _0D01
	{
		get
		{
			return Ӿ;
		}
		set
		{
			Ӿ = value;
		}
	}

	public override void Measure(Size availableSize, MarkupDrawContext d)
	{
		base.Bounds = Rectangle.Empty;
		SetForeColor(d);
	}

	public override void Render(MarkupDrawContext d)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		d.HyperLink = true;
		d.HyperlinkStyle = ഈ();
		if (!d.HyperlinkStyle.BackColor.IsEmpty)
		{
			GraphicsPath val = new GraphicsPath();
			try
			{
				_0CC9 elements = Parent.Elements;
				int num = elements._0CCD(this) + 1;
				for (int i = num; i < elements.Count; i++)
				{
					_0CBF obj = elements.get__0CCA(i);
					if (obj.Visible)
					{
						if (obj is ഐ && ((ഐ)obj)._0D11 == this)
						{
							break;
						}
						val.AddRectangle(obj._0CC0);
					}
				}
				SolidBrush val2 = new SolidBrush(d.HyperlinkStyle.BackColor);
				try
				{
					d.Graphics.FillPath((Brush)(object)val2, val);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		SetForeColor(d);
	}

	private HyperlinkStyle ഈ()
	{
		if (ആ && MarkupSettings.MouseOverHyperlink.IsChanged)
		{
			return MarkupSettings.MouseOverHyperlink;
		}
		if (ഇ && MarkupSettings.VisitedHyperlink.IsChanged)
		{
			return MarkupSettings.VisitedHyperlink;
		}
		return MarkupSettings.NormalHyperlink;
	}

	protected virtual void SetForeColor(MarkupDrawContext d)
	{
		Color currentForeColor = Color.Empty;
		HyperlinkStyle hyperlinkStyle = ഈ();
		if (hyperlinkStyle != null && !hyperlinkStyle.TextColor.IsEmpty)
		{
			currentForeColor = hyperlinkStyle.TextColor;
		}
		if (!_0D02.IsEmpty)
		{
			currentForeColor = _0D02;
		}
		if (!currentForeColor.IsEmpty)
		{
			_0D03 = d.CurrentForeColor;
			d.CurrentForeColor = currentForeColor;
		}
	}

	public override void RenderEnd(MarkupDrawContext d)
	{
		RestoreForeColor(d);
		d.HyperLink = false;
		d.HyperlinkStyle = null;
		base.RenderEnd(d);
	}

	public override void MeasureEnd(Size availableSize, MarkupDrawContext d)
	{
		RestoreForeColor(d);
		base.MeasureEnd(availableSize, d);
	}

	protected override void ArrangeCore(Rectangle finalRect, MarkupDrawContext d)
	{
	}

	protected virtual void RestoreForeColor(MarkupDrawContext d)
	{
		if (d != null)
		{
			if (!_0D03.IsEmpty)
			{
				d.CurrentForeColor = _0D03;
			}
			_0D03 = Color.Empty;
		}
	}

	public override void ReadAttributes(XmlTextReader reader)
	{
		for (int i = 0; i < reader.AttributeCount; i++)
		{
			reader.MoveToAttribute(i);
			if (reader.Name.ToLower() == "href")
			{
				ഄ = reader.Value;
			}
			else if (reader.Name.ToLower() == "name")
			{
				Ӿ = reader.Value;
			}
			else
			{
				if (!(reader.Name.ToLower() == "color"))
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
							_0D02 = ColorScheme.GetColor(value.Substring(1));
						}
					}
					else
					{
						_0D02 = ഉ(value);
					}
				}
				catch
				{
					_0D02 = Color.Empty;
				}
			}
		}
	}

	private Color ഉ(string _0956)
	{
		return Color.FromName(_0956);
	}

	public bool _0CFD(int و, int ى)
	{
		if (Parent == null)
		{
			return false;
		}
		_0CC9 elements = Parent.Elements;
		int num = elements._0CCD(this) + 1;
		for (int i = num; i < elements.Count; i++)
		{
			_0CBF obj = elements.get__0CCA(i);
			if (obj is ഐ && ((ഐ)obj)._0D11 == this)
			{
				break;
			}
			if (elements.get__0CCA(i)._0CC0.Contains(و, ى))
			{
				return true;
			}
		}
		return false;
	}

	public void બ(Control ԟ)
	{
		അ = ԟ.Cursor;
		ԟ.Cursor = Cursors.Hand;
		ആ = true;
		if (MarkupSettings.MouseOverHyperlink.IsChanged)
		{
			ഊ(ԟ);
		}
	}

	public void ભ(Control ԟ)
	{
		if (അ != (Cursor)null && ԟ != null)
		{
			ԟ.Cursor = അ;
		}
		അ = null;
		ആ = false;
		if (MarkupSettings.MouseOverHyperlink.IsChanged)
		{
			ഊ(ԟ);
		}
	}

	public void ग़(Control ԟ, MouseEventArgs ؿ)
	{
	}

	public void ફ(Control ԟ, MouseEventArgs ؿ)
	{
	}

	public void દ(Control ԟ)
	{
		ഇ = true;
		if (MarkupSettings.VisitedHyperlink.IsChanged)
		{
			ഊ(ԟ);
		}
	}

	private void ഊ(Control ԟ)
	{
		if (Parent == null)
		{
			return;
		}
		_0CC9 elements = Parent.Elements;
		int num = elements._0CCD(this) + 1;
		for (int i = num; i < elements.Count; i++)
		{
			_0CBF obj = elements.get__0CCA(i);
			if (obj.Visible)
			{
				if (obj is ഐ && ((ഐ)obj)._0D11 == this)
				{
					break;
				}
				ԟ.Invalidate(obj._0CC0);
			}
		}
	}
}
internal class _28E0 : _0CBF
{
	public override void Measure(Size availableSize, MarkupDrawContext d)
	{
		base.Bounds = Rectangle.Empty;
	}

	public override void Render(MarkupDrawContext d)
	{
		d.StrikeOut = true;
	}

	public override void RenderEnd(MarkupDrawContext d)
	{
		d.StrikeOut = false;
		base.RenderEnd(d);
	}

	protected override void ArrangeCore(Rectangle finalRect, MarkupDrawContext d)
	{
	}
}
internal class _28E1 : _0CBF
{
	private enum _28E2
	{
		_28E3
	}

	private Size _ᢌ = new Size(16, 16);

	private _28E2 _ລ;

	public override bool CanStartNewLine => false;

	public override void Measure(Size availableSize, MarkupDrawContext d)
	{
		base.Bounds = new Rectangle(Point.Empty, Dpi.Size(_ᢌ));
	}

	protected override void ArrangeCore(Rectangle finalRect, MarkupDrawContext d)
	{
	}

	public override void Render(MarkupDrawContext d)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		Rectangle bounds = base.Bounds;
		bounds.Offset(d.Offset);
		if (!d.ClipRectangle.IsEmpty && !bounds.IntersectsWith(d.ClipRectangle))
		{
			return;
		}
		Graphics graphics = d.Graphics;
		Color currentForeColor = d.CurrentForeColor;
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		graphics.SmoothingMode = (SmoothingMode)4;
		if (_ລ == _28E2._28E3)
		{
			Rectangle rectangle = new Rectangle(bounds.X + (bounds.Width - Dpi.Width14) / 2, bounds.Y + (bounds.Height - Dpi.Height14) / 2, Dpi.Width14, Dpi.Height14);
			Pen val = new Pen(currentForeColor, 1.51f);
			try
			{
				graphics.DrawEllipse(val, rectangle);
				graphics.DrawLine(val, rectangle.X + Dpi.Width4, rectangle.Y + Dpi.Height4, rectangle.X + Dpi.Width10, rectangle.Y + Dpi.Height10);
				graphics.DrawLine(val, rectangle.X + Dpi.Width10, rectangle.Y + Dpi.Height4, rectangle.X + Dpi.Width4, rectangle.Y + Dpi.Height10);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		graphics.SmoothingMode = smoothingMode;
		base._0CC0 = bounds;
	}

	public override void ReadAttributes(XmlTextReader reader)
	{
		_ລ = _28E2._28E3;
		for (int i = 0; i < reader.AttributeCount; i++)
		{
			reader.MoveToAttribute(i);
			if (reader.Name.ToLower() == "type")
			{
				string text = reader.Value.ToLower();
				if (text == "xincircle")
				{
					_ລ = _28E2._28E3;
				}
				break;
			}
		}
	}
}

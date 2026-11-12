using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.Ribbon;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;
using Stimulsoft.Controls.Win.Editors;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
[DefaultEvent("RatingChanged")]
public class RatingItem : PopupItem
{
	private struct RatingImage
	{
		public Image Image;

		public Rectangle SourceBounds;

		public RatingImage(Image image, Rectangle sourceBounds)
		{
			Image = image;
			SourceBounds = sourceBounds;
		}
	}

	private struct RatingInfo
	{
		public Rectangle Bounds;

		public bool IsMouseOver;
	}

	private double _AverageRating;

	private int _Rating;

	private bool _IsEditable = true;

	private bool _Initialized;

	private RatingInfo[] _RatingInfo = new RatingInfo[5];

	private Size _DefaultImageSize = new Size(13, 13);

	private Image _DefaultImage;

	private int _MouseOverRatingIndex = -1;

	private int _MenuIndent = 8;

	private Size _TextSize = Size.Empty;

	private bool _RenderMenuSide;

	private Size _CalcSize = Size.Empty;

	private ElementStyle _BackgroundStyle = new ElementStyle();

	private RatingImages _CustomImages;

	private bool _TextVisible = true;

	private Color _TextColor = Color.Empty;

	private int _TextSpacing;

	private eOrientation _RatingOrientation;

	private int _NumberOfStars = 5;

	private bool _EnableMarkup = true;

	[DefaultValue(0.0)]
	[Category("Data")]
	[Description("Indicates average rating shown by control.")]
	public double AverageRating
	{
		get
		{
			return _AverageRating;
		}
		set
		{
			if (value < 0.0)
			{
				value = 0.0;
			}
			if (value > (double)_NumberOfStars)
			{
				value = _NumberOfStars;
			}
			if (value != _AverageRating)
			{
				_AverageRating = value;
				OnAverageRatingChanged(EventArgs.Empty);
				Refresh();
			}
		}
	}

	[Bindable(true)]
	[Browsable(false)]
	[RefreshProperties(RefreshProperties.All)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[TypeConverter(typeof(StringConverter))]
	public object AverageRatingValue
	{
		get
		{
			return AverageRating;
		}
		set
		{
			if (AcceptCustomAverageRatingValue(value))
			{
				return;
			}
			if (IsNull(value))
			{
				AverageRating = 0.0;
				return;
			}
			if (value is double || value is int || value is float)
			{
				AverageRating = (double)value;
				return;
			}
			if (value is long)
			{
				string s = value.ToString();
				AverageRating = double.Parse(s);
				return;
			}
			if (value is string)
			{
				double result = 0.0;
				if (double.TryParse(value.ToString(), out result))
				{
					AverageRating = result;
					return;
				}
				throw new ArgumentException("AverageRatingValue property expects either null/nothing value or double type.");
			}
			throw new ArgumentException("AverageRatingValue property expects either null/nothing value or double type.");
		}
	}

	[DefaultValue(0)]
	[Category("Data")]
	[Description("Indicates rating value represented by the control.")]
	public int Rating
	{
		get
		{
			return _Rating;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			if (_Initialized && value > _NumberOfStars)
			{
				value = _NumberOfStars;
			}
			if (_Rating != value)
			{
				SetRating(value, eEventSource.Code);
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[RefreshProperties(RefreshProperties.All)]
	[Bindable(true)]
	public object RatingValue
	{
		get
		{
			return _Rating;
		}
		set
		{
			if (AcceptCustomRatingValue(value))
			{
				return;
			}
			if (IsNull(value))
			{
				Rating = 0;
				return;
			}
			if (value is int)
			{
				Rating = (int)value;
				return;
			}
			if (value is string)
			{
				int result = 0;
				if (int.TryParse(value.ToString(), out result))
				{
					Rating = result;
					return;
				}
				throw new ArgumentException("RatingValue property expects either null/nothing value or int type.");
			}
			throw new ArgumentException("RatingValue property expects either null/nothing value or int type.");
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether rating can be edited.")]
	public bool IsEditable
	{
		get
		{
			return _IsEditable;
		}
		set
		{
			if (_IsEditable != value)
			{
				_IsEditable = value;
				SetMouseOverIndex(-1);
			}
		}
	}

	internal Size CalcSize => _CalcSize;

	public override Rectangle Bounds
	{
		get
		{
			return base.Bounds;
		}
		set
		{
			if (!base.Bounds.Equals((object?)value))
			{
				base.Bounds = value;
				UpdateRatingBounds();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Gets or sets control background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle BackgroundStyle => _BackgroundStyle;

	[Browsable(true)]
	[Category("Images")]
	[Description("Gets the reference to custom rating images.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public RatingImages CustomImages => _CustomImages;

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text assigned to the check box is visible.")]
	public bool TextVisible
	{
		get
		{
			return _TextVisible;
		}
		set
		{
			_TextVisible = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates text color.")]
	public Color TextColor
	{
		get
		{
			return _TextColor;
		}
		set
		{
			_TextColor = value;
			OnAppearanceChanged();
		}
	}

	[DefaultValue(0)]
	[Category("Appearance")]
	[Description("Gets or sets the spacing between optional text and the rating.")]
	public int TextSpacing
	{
		get
		{
			return _TextSpacing;
		}
		set
		{
			if (_TextSpacing != value)
			{
				_TextSpacing = value;
				NeedRecalcSize = true;
				Refresh();
			}
		}
	}

	[DefaultValue(eOrientation.Horizontal)]
	[Category("Appearance")]
	[Description("Gets or sets the orientation of rating control.")]
	public eOrientation RatingOrientation
	{
		get
		{
			return _RatingOrientation;
		}
		set
		{
			if (_RatingOrientation != value)
			{
				_RatingOrientation = value;
				NeedRecalcSize = true;
				Refresh();
			}
		}
	}

	[DefaultValue(5)]
	[Category("Appearance")]
	[Description("Indicates number of stars used for the rating.")]
	public int NumberOfStars
	{
		get
		{
			return _NumberOfStars;
		}
		set
		{
			if (_NumberOfStars < 2)
			{
				_NumberOfStars = 2;
			}
			if (_NumberOfStars != value)
			{
				_NumberOfStars = value;
				if (_Rating > _NumberOfStars)
				{
					_Rating = _NumberOfStars;
				}
				_RatingInfo = new RatingInfo[_NumberOfStars];
				NeedRecalcSize = true;
				Refresh();
			}
		}
	}

	protected override bool IsMarkupSupported => _EnableMarkup;

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text-markup support is enabled for items Text property.")]
	public bool EnableMarkup
	{
		get
		{
			return _EnableMarkup;
		}
		set
		{
			if (_EnableMarkup != value)
			{
				_EnableMarkup = value;
				NeedRecalcSize = true;
				OnTextChanged();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override SubItemsCollection SubItems => base.SubItems;

	[Browsable(false)]
	public override bool ClickAutoRepeat
	{
		get
		{
			return base.ClickAutoRepeat;
		}
		set
		{
			base.ClickAutoRepeat = value;
		}
	}

	[Browsable(false)]
	public override int ClickRepeatInterval
	{
		get
		{
			return base.ClickRepeatInterval;
		}
		set
		{
			base.ClickRepeatInterval = value;
		}
	}

	[Browsable(false)]
	public override ePersonalizedMenus PersonalizedMenus
	{
		get
		{
			return base.PersonalizedMenus;
		}
		set
		{
			base.PersonalizedMenus = value;
		}
	}

	[Browsable(false)]
	public override ePopupAnimation PopupAnimation
	{
		get
		{
			return base.PopupAnimation;
		}
		set
		{
			base.PopupAnimation = value;
		}
	}

	[Browsable(false)]
	public override Font PopupFont
	{
		get
		{
			return base.PopupFont;
		}
		set
		{
			base.PopupFont = value;
		}
	}

	[Browsable(false)]
	public override ePopupSide PopupSide
	{
		get
		{
			return base.PopupSide;
		}
		set
		{
			base.PopupSide = value;
		}
	}

	[Browsable(false)]
	public override ePopupType PopupType
	{
		get
		{
			return base.PopupType;
		}
		set
		{
			base.PopupType = value;
		}
	}

	[Browsable(false)]
	public override int PopupWidth
	{
		get
		{
			return base.PopupWidth;
		}
		set
		{
			base.PopupWidth = value;
		}
	}

	[Browsable(false)]
	public override ShortcutsCollection Shortcuts
	{
		get
		{
			return base.Shortcuts;
		}
		set
		{
			base.Shortcuts = value;
		}
	}

	[Browsable(false)]
	public override bool ShowSubItems
	{
		get
		{
			return base.ShowSubItems;
		}
		set
		{
			base.ShowSubItems = value;
		}
	}

	[Browsable(false)]
	public override bool Stretch
	{
		get
		{
			return base.Stretch;
		}
		set
		{
			base.Stretch = value;
		}
	}

	[Browsable(false)]
	public override bool ThemeAware
	{
		get
		{
			return base.ThemeAware;
		}
		set
		{
			base.ThemeAware = value;
		}
	}

	[DefaultValue(false)]
	public override bool AutoCollapseOnClick
	{
		get
		{
			return base.AutoCollapseOnClick;
		}
		set
		{
			base.AutoCollapseOnClick = value;
		}
	}

	[Description("Occurs when Rating property has changed.")]
	public event EventHandler RatingChanged;

	[Description("Occurs when Rating property has changed.")]
	public event EventHandler RatingValueChanged;

	[Description("Occurs when Rating property has changed.")]
	public event RatingChangeEventHandler RatingChanging;

	[Description("Occurs when AverageRating property has changed.")]
	public event EventHandler AverageRatingChanged;

	[Description("Occurs when AverageRatingValue property has changed.")]
	public event EventHandler AverageRatingValueChanged;

	public event MarkupLinkClickEventHandler MarkupLinkClick;

	public event ParseIntegerValueEventHandler ParseRatingValue;

	public event ParseDoubleValueEventHandler ParseAverageRatingValue;

	public RatingItem()
	{
		_BackgroundStyle.StyleChanged += BackgroundStyleChanged;
		_CustomImages = new RatingImages(this);
		AutoCollapseOnClick = false;
	}

	protected override void Dispose(bool disposing)
	{
		if (BarUtilities.DisposeItemImages && !DesignMode)
		{
			BarUtilities.DisposeImage(ref _DefaultImage);
			_CustomImages.Dispose();
		}
		base.Dispose(disposing);
	}

	public override BaseItem Copy()
	{
		RatingItem ratingItem = new RatingItem();
		ratingItem.Name = Name;
		CopyToItem(ratingItem);
		return ratingItem;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		RatingItem ratingItem = copy as RatingItem;
		ratingItem.AverageRating = AverageRating;
		ratingItem.Rating = Rating;
		ratingItem.RatingOrientation = RatingOrientation;
		ratingItem.IsEditable = IsEditable;
		ratingItem.TextSpacing = TextSpacing;
		ratingItem.TextColor = TextColor;
		ratingItem.TextVisible = TextVisible;
		base.CopyToItem((BaseItem)ratingItem);
	}

	protected virtual void OnAverageRatingChanged(EventArgs eventArgs)
	{
		if (this.AverageRatingChanged != null)
		{
			this.AverageRatingChanged(this, eventArgs);
		}
		if (this.AverageRatingValueChanged != null)
		{
			this.AverageRatingValueChanged(this, eventArgs);
		}
	}

	private bool AcceptCustomAverageRatingValue(object value)
	{
		ParseDoubleValueEventArgs parseDoubleValueEventArgs = new ParseDoubleValueEventArgs(value);
		OnParseAverageRatingValue(parseDoubleValueEventArgs);
		if (parseDoubleValueEventArgs.IsParsed)
		{
			AverageRating = parseDoubleValueEventArgs.ParsedValue;
		}
		return parseDoubleValueEventArgs.IsParsed;
	}

	protected virtual void OnParseAverageRatingValue(ParseDoubleValueEventArgs e)
	{
		if (this.ParseAverageRatingValue != null)
		{
			this.ParseAverageRatingValue(this, e);
		}
	}

	public void SetRating(int newRating, eEventSource source)
	{
		if (newRating != _Rating)
		{
			RatingChangeEventArgs ratingChangeEventArgs = new RatingChangeEventArgs(newRating, _Rating, source);
			OnRatingChanging(ratingChangeEventArgs);
			if (!ratingChangeEventArgs.Cancel)
			{
				newRating = ratingChangeEventArgs.NewRating;
				_Rating = newRating;
				OnRatingChanged(ratingChangeEventArgs);
				ExecuteCommand();
				Refresh();
			}
		}
	}

	protected virtual void OnRatingChanging(RatingChangeEventArgs e)
	{
		if (this.RatingChanging != null)
		{
			this.RatingChanging(this, e);
		}
	}

	protected virtual void OnRatingChanged(EventArgs eventArgs)
	{
		this.RatingChanged?.Invoke(this, eventArgs);
		this.RatingValueChanged?.Invoke(this, eventArgs);
	}

	protected virtual bool IsNull(object value)
	{
		if (value == null || value is DBNull)
		{
			return true;
		}
		return false;
	}

	private bool AcceptCustomRatingValue(object value)
	{
		ParseIntegerValueEventArgs parseIntegerValueEventArgs = new ParseIntegerValueEventArgs(value);
		OnParseRatingValue(parseIntegerValueEventArgs);
		if (parseIntegerValueEventArgs.IsParsed)
		{
			Rating = parseIntegerValueEventArgs.ParsedValue;
		}
		return parseIntegerValueEventArgs.IsParsed;
	}

	protected virtual void OnParseRatingValue(ParseIntegerValueEventArgs e)
	{
		if (this.ParseRatingValue != null)
		{
			this.ParseRatingValue(this, e);
		}
	}

	public override void Paint(ItemPaintArgs p)
	{
		_Initialized = true;
		PaintBackground(p);
		Rectangle r = DisplayRectangle;
		if (_RenderMenuSide)
		{
			PaintMenuItemSide(p, ref r);
		}
		r = GetClientRectangle(r);
		if (_TextVisible && !_TextSize.IsEmpty)
		{
			PaintText(p, r);
		}
		Graphics graphics = p.Graphics;
		if (_MouseOverRatingIndex >= 0)
		{
			RatingImage ratedMouseOverImage = GetRatedMouseOverImage();
			RatingImage unratedMouseOverImage = GetUnratedMouseOverImage();
			for (int i = 0; i < _NumberOfStars; i++)
			{
				if (_RatingInfo[i].IsMouseOver)
				{
					graphics.DrawImage(ratedMouseOverImage.Image, _RatingInfo[i].Bounds, ratedMouseOverImage.SourceBounds, (GraphicsUnit)2);
				}
				else
				{
					graphics.DrawImage(unratedMouseOverImage.Image, _RatingInfo[i].Bounds, unratedMouseOverImage.SourceBounds, (GraphicsUnit)2);
				}
			}
		}
		else
		{
			int num = Math.Min(_Rating, _NumberOfStars);
			if (_AverageRating > 0.0 && num == 0)
			{
				RatingImage averageRatedImage = GetAverageRatedImage();
				RatingImage unratedImage = GetUnratedImage();
				for (int j = 0; j < _NumberOfStars; j++)
				{
					if ((double)(j + 1) <= _AverageRating)
					{
						graphics.DrawImage(averageRatedImage.Image, _RatingInfo[j].Bounds, averageRatedImage.SourceBounds, (GraphicsUnit)2);
					}
					else if ((double)(j + 1) == Math.Ceiling(_AverageRating))
					{
						int num2 = (int)((double)((RatingOrientation == eOrientation.Horizontal) ? _RatingInfo[j].Bounds.Width : _RatingInfo[j].Bounds.Height) * (_AverageRating - Math.Floor(_AverageRating)));
						graphics.DrawImage(unratedImage.Image, _RatingInfo[j].Bounds, unratedImage.SourceBounds, (GraphicsUnit)2);
						if (num2 > 0)
						{
							Region clip = graphics.Clip;
							if (RatingOrientation == eOrientation.Horizontal)
							{
								graphics.SetClip(new Rectangle(_RatingInfo[j].Bounds.X, _RatingInfo[j].Bounds.Y, num2, _RatingInfo[j].Bounds.Height), (CombineMode)1);
							}
							else
							{
								graphics.SetClip(new Rectangle(_RatingInfo[j].Bounds.X + (_RatingInfo[j].Bounds.Height - num2), _RatingInfo[j].Bounds.Y, _RatingInfo[j].Bounds.Width, num2), (CombineMode)1);
							}
							graphics.DrawImage(averageRatedImage.Image, _RatingInfo[j].Bounds, averageRatedImage.SourceBounds, (GraphicsUnit)2);
							graphics.Clip = clip;
						}
					}
					else
					{
						graphics.DrawImage(unratedImage.Image, _RatingInfo[j].Bounds, unratedImage.SourceBounds, (GraphicsUnit)2);
					}
				}
			}
			else
			{
				RatingImage ratedImage = GetRatedImage();
				RatingImage unratedImage2 = GetUnratedImage();
				for (int k = 0; k < _NumberOfStars; k++)
				{
					if (num > 0 && k + 1 <= num)
					{
						graphics.DrawImage(ratedImage.Image, _RatingInfo[k].Bounds, ratedImage.SourceBounds, (GraphicsUnit)2);
					}
					else
					{
						graphics.DrawImage(unratedImage2.Image, _RatingInfo[k].Bounds, unratedImage2.SourceBounds, (GraphicsUnit)2);
					}
				}
			}
		}
		if (Focused && DesignMode)
		{
			Rectangle displayRectangle = DisplayRectangle;
			displayRectangle.Inflate(-1, -1);
			DesignTime.DrawDesignTimeSelection(graphics, displayRectangle, p.Colors.ItemDesignTimeBorder);
		}
	}

	private void PaintText(ItemPaintArgs p, Rectangle bounds)
	{
		Color empty = Color.Empty;
		empty = (_TextColor.IsEmpty ? GetTextColor(p) : _TextColor);
		bool rightToLeft = p.RightToLeft;
		Rectangle rectangle = bounds;
		if ((_BackgroundStyle.TextAlignment == eStyleTextAlignment.Far && !rightToLeft) || (rightToLeft && _BackgroundStyle.TextAlignment == eStyleTextAlignment.Near))
		{
			rectangle.X = rectangle.Right - _TextSize.Width;
			rectangle.Width = _TextSize.Width;
		}
		else
		{
			rectangle.Width = _TextSize.Width;
		}
		Graphics graphics = p.Graphics;
		Font font = p.Font;
		eTextFormat textFormat = _BackgroundStyle.TextFormat;
		if (!_TextVisible || !(Text != "") || rectangle.IsEmpty || empty.IsEmpty)
		{
			return;
		}
		if (base.TextMarkupBody != null)
		{
			MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, font, empty, rightToLeft);
			markupDrawContext.HotKeyPrefixVisible = (textFormat & eTextFormat.HidePrefix) != eTextFormat.HidePrefix;
			if ((textFormat & eTextFormat.VerticalCenter) == eTextFormat.VerticalCenter)
			{
				rectangle.Y = TopInternal + (Bounds.Height - rectangle.Height) / 2;
			}
			else if ((textFormat & eTextFormat.Bottom) == eTextFormat.Bottom)
			{
				rectangle.Y += Bounds.Height - rectangle.Height + 1;
			}
			base.TextMarkupBody.Bounds = rectangle;
			base.TextMarkupBody.Render(markupDrawContext);
		}
		else if (p.GlassEnabled && Parent is CaptionItemContainer && !(p.ContainerControl is QatToolbar))
		{
			if (!p.CachedPaint)
			{
				Office2007RibbonControlPainter.PaintTextOnGlass(graphics, Text, font, rectangle, TextDrawing.GetTextFormat(textFormat));
			}
		}
		else
		{
			TextDrawing.DrawString(graphics, Text, font, empty, rectangle, textFormat);
		}
	}

	private Color GetTextColor(ItemPaintArgs pa)
	{
		if (BarFunctions.IsOffice2007Style(EffectiveStyle) && pa.Renderer is Office2007Renderer)
		{
			if ((pa.IsOnMenu || pa.IsOnPopupBar) && ((Office2007Renderer)pa.Renderer).ColorTable.ButtonItemColors.Count > 0)
			{
				if (!GetEnabled(pa.ContainerControl))
				{
					return pa.Colors.ItemDisabledText;
				}
				return ((Office2007Renderer)pa.Renderer).ColorTable.ButtonItemColors[0].Default.Text;
			}
			if ((pa.ContainerControl is RibbonStrip || pa.ContainerControl is Bar) && ((Office2007Renderer)pa.Renderer).ColorTable.RibbonTabItemColors.Count > 0)
			{
				return ((Office2007Renderer)pa.Renderer).ColorTable.RibbonTabItemColors[0].Default.Text;
			}
			Office2007ButtonItemColorTable office2007ButtonItemColorTable = ((Office2007Renderer)pa.Renderer).ColorTable.ButtonItemColors[Enum.GetName(typeof(eButtonColor), eButtonColor.Orange)];
			if (office2007ButtonItemColorTable != null && !office2007ButtonItemColorTable.Default.Text.IsEmpty)
			{
				if (!GetEnabled(pa.ContainerControl))
				{
					return pa.Colors.ItemDisabledText;
				}
				return office2007ButtonItemColorTable.Default.Text;
			}
			return _TextColor;
		}
		return pa.Colors.ItemText;
	}

	private void PaintMenuItemSide(ItemPaintArgs pa, ref Rectangle r)
	{
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		bool flag = base.IsOnMenu && !(Parent is ItemContainer);
		Size maxImageSize = GetMaxImageSize();
		if (flag && (EffectiveStyle == eDotNetBarStyle.OfficeXP || EffectiveStyle == eDotNetBarStyle.Office2003 || EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(EffectiveStyle)))
		{
			Graphics graphics = pa.Graphics;
			maxImageSize.Width += 7;
			r.Width -= maxImageSize.Width + _MenuIndent;
			if (!pa.RightToLeft)
			{
				r.X += maxImageSize.Width + _MenuIndent;
			}
			if (base.IsOnCustomizeMenu)
			{
				maxImageSize.Width += maxImageSize.Height + 8;
			}
			Rectangle rectangle = new Rectangle(m_Rect.Left, m_Rect.Top, maxImageSize.Width, m_Rect.Height);
			if (pa.RightToLeft)
			{
				rectangle.X = m_Rect.Right - rectangle.Width;
			}
			if (!pa.Colors.MenuSide2.IsEmpty)
			{
				LinearGradientBrush val = BarFunctions.CreateLinearGradientBrush(rectangle, pa.Colors.MenuSide, pa.Colors.MenuSide2, pa.Colors.MenuSideGradientAngle);
				graphics.FillRectangle((Brush)(object)val, rectangle);
				((Brush)val).Dispose();
			}
			else
			{
				graphics.FillRectangle((Brush)new SolidBrush(pa.Colors.MenuSide), rectangle);
			}
			if (BarFunctions.IsOffice2007Style(EffectiveStyle) && GlobalManager.Renderer is Office2007Renderer)
			{
				Office2007MenuColorTable menu = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Menu;
				if (menu != null && !menu.SideBorder.IsEmpty)
				{
					if (pa.RightToLeft)
					{
						DisplayHelp.DrawGradientLine(graphics, rectangle.X, rectangle.Y, rectangle.X, rectangle.Bottom - 1, menu.SideBorder, 1);
					}
					else
					{
						DisplayHelp.DrawGradientLine(graphics, rectangle.Right - 2, rectangle.Y, rectangle.Right - 2, rectangle.Bottom - 1, menu.SideBorder, 1);
					}
				}
				if (menu != null && !menu.SideBorderLight.IsEmpty)
				{
					if (pa.RightToLeft)
					{
						DisplayHelp.DrawGradientLine(graphics, rectangle.X + 1, rectangle.Y, rectangle.X + 1, rectangle.Bottom - 1, menu.SideBorderLight, 1);
					}
					else
					{
						DisplayHelp.DrawGradientLine(graphics, rectangle.Right - 1, rectangle.Y, rectangle.Right - 1, rectangle.Bottom - 1, menu.SideBorderLight, 1);
					}
				}
			}
		}
		if (base.IsOnCustomizeMenu)
		{
			if (EffectiveStyle == eDotNetBarStyle.OfficeXP || EffectiveStyle == eDotNetBarStyle.Office2003 || EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(EffectiveStyle))
			{
				r.X += maxImageSize.Height + 8;
				r.Width -= maxImageSize.Height + 8;
			}
			else
			{
				r.X += maxImageSize.Height + 4;
				r.Width -= maxImageSize.Height + 4;
			}
		}
	}

	private RatingImage GetAverageRatedImage()
	{
		if (_CustomImages.AverageRated != null)
		{
			return new RatingImage(_CustomImages.AverageRated, new Rectangle(Point.Empty, _CustomImages.AverageRated.Size));
		}
		EnsureDefaultImage();
		return new RatingImage(_DefaultImage, new Rectangle(0, 52, _DefaultImageSize.Width, _DefaultImageSize.Height));
	}

	private RatingImage GetRatedImage()
	{
		if (_CustomImages.Rated != null)
		{
			return new RatingImage(_CustomImages.Rated, new Rectangle(Point.Empty, _CustomImages.Rated.Size));
		}
		EnsureDefaultImage();
		return new RatingImage(_DefaultImage, new Rectangle(0, 26, _DefaultImageSize.Width, _DefaultImageSize.Height));
	}

	private RatingImage GetUnratedImage()
	{
		if (_CustomImages.Unrated != null)
		{
			return new RatingImage(_CustomImages.Unrated, new Rectangle(Point.Empty, _CustomImages.Unrated.Size));
		}
		EnsureDefaultImage();
		return new RatingImage(_DefaultImage, new Rectangle(0, 0, _DefaultImageSize.Width, _DefaultImageSize.Height));
	}

	private RatingImage GetUnratedMouseOverImage()
	{
		if (_CustomImages.UnratedMouseOver != null)
		{
			return new RatingImage(_CustomImages.UnratedMouseOver, new Rectangle(Point.Empty, _CustomImages.UnratedMouseOver.Size));
		}
		EnsureDefaultImage();
		return new RatingImage(_DefaultImage, new Rectangle(0, 13, _DefaultImageSize.Width, _DefaultImageSize.Height));
	}

	private RatingImage GetRatedMouseOverImage()
	{
		if (_CustomImages.RatedMouseOver != null)
		{
			return new RatingImage(_CustomImages.RatedMouseOver, new Rectangle(Point.Empty, _CustomImages.RatedMouseOver.Size));
		}
		EnsureDefaultImage();
		return new RatingImage(_DefaultImage, new Rectangle(0, 39, _DefaultImageSize.Width, _DefaultImageSize.Height));
	}

	private void EnsureDefaultImage()
	{
		if (_DefaultImage == null)
		{
			_DefaultImage = (Image)(object)BarFunctions.LoadBitmap("SystemImages.Rating.png");
		}
	}

	public override void InternalMouseMove(MouseEventArgs e)
	{
		if (_IsEditable)
		{
			int mouseOverIndex = -1;
			if (RatingOrientation == eOrientation.Horizontal)
			{
				for (int i = 0; i < _NumberOfStars; i++)
				{
					if (e.X >= _RatingInfo[i].Bounds.X && e.X <= _RatingInfo[i].Bounds.Right)
					{
						mouseOverIndex = i;
						break;
					}
				}
			}
			else
			{
				for (int j = 0; j < _NumberOfStars; j++)
				{
					if ((e.Y >= _RatingInfo[j].Bounds.Y && e.Y <= _RatingInfo[j].Bounds.Bottom && (!_TextVisible || _TextSize.IsEmpty)) || _RatingInfo[j].Bounds.Contains(e.X, e.Y))
					{
						mouseOverIndex = j;
						break;
					}
				}
			}
			SetMouseOverIndex(mouseOverIndex);
		}
		else
		{
			SetMouseOverIndex(-1);
		}
		base.InternalMouseMove(e);
	}

	public override void InternalClick(MouseButtons buttons, Point position)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		if (_MouseOverRatingIndex >= 0)
		{
			SetRating(_MouseOverRatingIndex + 1, eEventSource.Mouse);
		}
		base.InternalClick(buttons, position);
	}

	private void SetMouseOverIndex(int mouseOverIndex)
	{
		if (mouseOverIndex != _MouseOverRatingIndex)
		{
			_MouseOverRatingIndex = mouseOverIndex;
			for (int i = 0; i < _NumberOfStars; i++)
			{
				_RatingInfo[i].IsMouseOver = mouseOverIndex >= 0 && i <= mouseOverIndex;
			}
			Refresh();
		}
	}

	public override void InternalMouseLeave()
	{
		if (_IsEditable)
		{
			SetMouseOverIndex(-1);
		}
		base.InternalMouseLeave();
	}

	private Rectangle GetClientRectangle()
	{
		return GetClientRectangle(DisplayRectangle);
	}

	private Rectangle GetClientRectangle(Rectangle bounds)
	{
		bounds.X += ElementStyleLayout.LeftWhiteSpace(_BackgroundStyle);
		bounds.Y += ElementStyleLayout.TopWhiteSpace(_BackgroundStyle);
		bounds.Width -= ElementStyleLayout.HorizontalStyleWhiteSpace(_BackgroundStyle);
		bounds.Height -= ElementStyleLayout.VerticalStyleWhiteSpace(_BackgroundStyle);
		return bounds;
	}

	private void UpdateRatingBounds()
	{
		Rectangle clientRectangle = GetClientRectangle();
		if (_TextVisible && !_TextSize.IsEmpty && ((_BackgroundStyle.TextAlignment == eStyleTextAlignment.Far && IsRightToLeft) || (!IsRightToLeft && _BackgroundStyle.TextAlignment == eStyleTextAlignment.Near)))
		{
			clientRectangle.X += _TextSpacing + _TextSize.Width + (_RenderMenuSide ? _MenuIndent : 0);
		}
		if (_RenderMenuSide && ((_BackgroundStyle.TextAlignment == eStyleTextAlignment.Far && IsRightToLeft) || (!IsRightToLeft && _BackgroundStyle.TextAlignment == eStyleTextAlignment.Near)))
		{
			Size maxImageSize = GetMaxImageSize();
			clientRectangle.X += maxImageSize.Width + 7;
			if (base.IsOnCustomizeMenu)
			{
				clientRectangle.X += clientRectangle.Height + 2;
			}
		}
		Size size = DisplayHelp.MaxSize(_DefaultImageSize, _CustomImages.LargestImageSize);
		if (clientRectangle.Height > size.Height && RatingOrientation == eOrientation.Horizontal)
		{
			clientRectangle.Y += (clientRectangle.Height - size.Height) / 2;
		}
		if (RatingOrientation == eOrientation.Horizontal)
		{
			Point location = clientRectangle.Location;
			for (int i = 0; i < _NumberOfStars; i++)
			{
				_RatingInfo[i].Bounds = new Rectangle(location, size);
				location.X += size.Width;
			}
		}
		else
		{
			Point location2 = new Point(clientRectangle.X, clientRectangle.Bottom - size.Height);
			for (int j = 0; j < _NumberOfStars; j++)
			{
				_RatingInfo[j].Bounds = new Rectangle(location2, size);
				location2.Y -= size.Height;
			}
		}
	}

	public override void RecalcSize()
	{
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Invalid comparison between Unknown and I4
		Rectangle displayRectangle = DisplayRectangle;
		Size size = DisplayHelp.MaxSize(_DefaultImageSize, _CustomImages.LargestImageSize);
		if (RatingOrientation == eOrientation.Horizontal)
		{
			size.Width *= _NumberOfStars;
		}
		else
		{
			size.Height *= _NumberOfStars;
		}
		size.Width += ElementStyleLayout.HorizontalStyleWhiteSpace(_BackgroundStyle);
		size.Height += ElementStyleLayout.VerticalStyleWhiteSpace(_BackgroundStyle);
		_RenderMenuSide = false;
		if (base.IsOnMenu && !(Parent is ItemContainer) && (EffectiveStyle == eDotNetBarStyle.OfficeXP || EffectiveStyle == eDotNetBarStyle.Office2003 || EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(EffectiveStyle)))
		{
			Size maxImageSize = GetMaxImageSize();
			size.Width += maxImageSize.Width + 7;
			if (base.IsOnCustomizeMenu)
			{
				size.Width += size.Height + 2;
			}
			size.Width += _MenuIndent;
			_RenderMenuSide = true;
		}
		_TextSize = Size.Empty;
		if (_TextVisible)
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val == null || val.Disposing || val.IsDisposed)
			{
				return;
			}
			Graphics val2 = BarFunctions.CreateGraphics(val);
			if (val2 == null)
			{
				return;
			}
			try
			{
				Size textSize = ButtonItemLayout.MeasureItemText(this, val2, 0, val.Font, eTextFormat.Default, (int)val.RightToLeft == 1);
				textSize.Width++;
				size.Width += _TextSpacing + textSize.Width;
				size.Height = Math.Max(size.Height, textSize.Height);
				_TextSize = textSize;
			}
			finally
			{
				val2.Dispose();
			}
		}
		_CalcSize = size;
		displayRectangle.Size = size;
		if (!(ContainerControl is RatingStar))
		{
			m_Rect = displayRectangle;
		}
		UpdateRatingBounds();
		base.RecalcSize();
	}

	private Size GetMaxImageSize()
	{
		if (m_Parent != null)
		{
			if (m_Parent is ImageItem imageItem)
			{
				return imageItem.SubItemsImageSize;
			}
			return ImageSize;
		}
		return ImageSize;
	}

	protected override void OnLeftLocationChanged(int oldValue)
	{
		UpdateRatingBounds();
		base.OnLeftLocationChanged(oldValue);
	}

	protected override void OnTopLocationChanged(int oldValue)
	{
		UpdateRatingBounds();
		base.OnTopLocationChanged(oldValue);
	}

	protected virtual void PaintBackground(ItemPaintArgs p)
	{
		_BackgroundStyle.SetColorScheme(p.Colors);
		Graphics graphics = p.Graphics;
		ElementStyleDisplay.Paint(new ElementStyleDisplayInfo(_BackgroundStyle, graphics, DisplayRectangle));
	}

	internal void SetBackgroundStyle(ElementStyle elementStyle)
	{
		_BackgroundStyle.StyleChanged -= BackgroundStyleChanged;
		_BackgroundStyle = elementStyle;
		_BackgroundStyle.StyleChanged += BackgroundStyleChanged;
	}

	private void BackgroundStyleChanged(object sender, EventArgs e)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		return !_TextColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		TextColor = Color.Empty;
	}

	protected override void TextMarkupLinkClick(object sender, EventArgs e)
	{
		if (sender is HyperLink hyperLink)
		{
			OnMarkupLinkClick(new MarkupLinkClickEventArgs(hyperLink.Name, hyperLink.HRef));
		}
		base.TextMarkupLinkClick(sender, e);
	}

	protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
	{
		if (this.MarkupLinkClick != null)
		{
			this.MarkupLinkClick(this, e);
		}
	}
}

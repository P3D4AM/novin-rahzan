using System.ComponentModel;
using System.Drawing;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[TypeConverter(typeof(ExpandableObjectConverter))]
public class RatingImages
{
	private RatingItem _ᗚ;

	private Image _Ɫ;

	private Image _Ᵽ;

	private Image _Ɽ;

	private Image _ⱥ;

	private Image _ⱦ;

	private Size _Ⱨ = Size.Empty;

	[DefaultValue(null)]
	[Description("Gets or sets the image used for unrated rating part.")]
	public Image Unrated
	{
		get
		{
			return _Ɫ;
		}
		set
		{
			if (_Ɫ != value)
			{
				_Ɫ = value;
				ᡁ();
			}
		}
	}

	[Description("Gets or sets the image used for unrated rating part when mouse is over the control.")]
	[DefaultValue(null)]
	public Image UnratedMouseOver
	{
		get
		{
			return _Ᵽ;
		}
		set
		{
			if (_Ᵽ != value)
			{
				_Ᵽ = value;
				ᡁ();
			}
		}
	}

	[Description("Gets or sets the image used for rated part of the control.")]
	[DefaultValue(null)]
	public Image Rated
	{
		get
		{
			return _Ɽ;
		}
		set
		{
			if (_Ɽ != value)
			{
				_Ɽ = value;
				ᡁ();
			}
		}
	}

	[Description("Gets or sets the image used for Average Rated part of the control.")]
	[DefaultValue(null)]
	public Image AverageRated
	{
		get
		{
			return _ⱥ;
		}
		set
		{
			if (_ⱥ != value)
			{
				_ⱥ = value;
				ᡁ();
			}
		}
	}

	[DefaultValue(null)]
	[Description("Gets or sets the image used for rated part of the control when mouse is over the control.")]
	public Image RatedMouseOver
	{
		get
		{
			return _ⱦ;
		}
		set
		{
			if (_ⱦ != value)
			{
				_ⱦ = value;
				ᡁ();
			}
		}
	}

	internal Size _26EE => _Ⱨ;

	internal RatingImages(RatingItem parentItem)
	{
		_ᗚ = parentItem;
	}

	public void Dispose()
	{
		BarUtilities.ᆟ(ref _ⱥ);
		BarUtilities.ᆟ(ref _Ɽ);
		BarUtilities.ᆟ(ref _ⱦ);
		BarUtilities.ᆟ(ref _Ɫ);
		BarUtilities.ᆟ(ref _Ᵽ);
	}

	private void ᡁ()
	{
		_Ⱨ = default(Size);
		if (_Ɫ != null)
		{
			_Ⱨ = DisplayHelp.ഝ(_Ⱨ, _Ɫ.Size);
		}
		if (_Ᵽ != null)
		{
			_Ⱨ = DisplayHelp.ഝ(_Ⱨ, _Ᵽ.Size);
		}
		if (_Ɽ != null)
		{
			_Ⱨ = DisplayHelp.ഝ(_Ⱨ, _Ɽ.Size);
		}
		if (_ⱦ != null)
		{
			_Ⱨ = DisplayHelp.ഝ(_Ⱨ, _ⱦ.Size);
		}
		if (_ⱥ != null)
		{
			_Ⱨ = DisplayHelp.ഝ(_Ⱨ, _ⱥ.Size);
		}
		if (_ᗚ != null)
		{
			_ᗚ.NeedRecalcSize = true;
			_ᗚ.OnAppearanceChanged();
		}
	}
}

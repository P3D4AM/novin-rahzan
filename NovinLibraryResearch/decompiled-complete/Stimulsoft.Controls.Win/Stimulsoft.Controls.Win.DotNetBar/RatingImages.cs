using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[TypeConverter(typeof(ExpandableObjectConverter))]
public class RatingImages
{
	private RatingItem _Parent;

	private Image _Unrated;

	private Image _UnratedMouseOver;

	private Image _Rated;

	private Image _AverageRated;

	private Image _RatedMouseOver;

	private Size _LargestImageSize = Size.Empty;

	[DefaultValue(null)]
	[Description("Gets or sets the image used for unrated rating part.")]
	public Image Unrated
	{
		get
		{
			return _Unrated;
		}
		set
		{
			if (_Unrated != value)
			{
				_Unrated = value;
				OnImageChanged();
			}
		}
	}

	[DefaultValue(null)]
	[Description("Gets or sets the image used for unrated rating part when mouse is over the control.")]
	public Image UnratedMouseOver
	{
		get
		{
			return _UnratedMouseOver;
		}
		set
		{
			if (_UnratedMouseOver != value)
			{
				_UnratedMouseOver = value;
				OnImageChanged();
			}
		}
	}

	[DefaultValue(null)]
	[Description("Gets or sets the image used for rated part of the control.")]
	public Image Rated
	{
		get
		{
			return _Rated;
		}
		set
		{
			if (_Rated != value)
			{
				_Rated = value;
				OnImageChanged();
			}
		}
	}

	[DefaultValue(null)]
	[Description("Gets or sets the image used for Average Rated part of the control.")]
	public Image AverageRated
	{
		get
		{
			return _AverageRated;
		}
		set
		{
			if (_AverageRated != value)
			{
				_AverageRated = value;
				OnImageChanged();
			}
		}
	}

	[DefaultValue(null)]
	[Description("Gets or sets the image used for rated part of the control when mouse is over the control.")]
	public Image RatedMouseOver
	{
		get
		{
			return _RatedMouseOver;
		}
		set
		{
			if (_RatedMouseOver != value)
			{
				_RatedMouseOver = value;
				OnImageChanged();
			}
		}
	}

	internal Size LargestImageSize => _LargestImageSize;

	internal RatingImages(RatingItem parentItem)
	{
		_Parent = parentItem;
	}

	public void Dispose()
	{
		BarUtilities.DisposeImage(ref _AverageRated);
		BarUtilities.DisposeImage(ref _Rated);
		BarUtilities.DisposeImage(ref _RatedMouseOver);
		BarUtilities.DisposeImage(ref _Unrated);
		BarUtilities.DisposeImage(ref _UnratedMouseOver);
	}

	private void OnImageChanged()
	{
		_LargestImageSize = default(Size);
		if (_Unrated != null)
		{
			_LargestImageSize = DisplayHelp.MaxSize(_LargestImageSize, _Unrated.Size);
		}
		if (_UnratedMouseOver != null)
		{
			_LargestImageSize = DisplayHelp.MaxSize(_LargestImageSize, _UnratedMouseOver.Size);
		}
		if (_Rated != null)
		{
			_LargestImageSize = DisplayHelp.MaxSize(_LargestImageSize, _Rated.Size);
		}
		if (_RatedMouseOver != null)
		{
			_LargestImageSize = DisplayHelp.MaxSize(_LargestImageSize, _RatedMouseOver.Size);
		}
		if (_AverageRated != null)
		{
			_LargestImageSize = DisplayHelp.MaxSize(_LargestImageSize, _AverageRated.Size);
		}
		if (_Parent != null)
		{
			_Parent.NeedRecalcSize = true;
			_Parent.OnAppearanceChanged();
		}
	}
}

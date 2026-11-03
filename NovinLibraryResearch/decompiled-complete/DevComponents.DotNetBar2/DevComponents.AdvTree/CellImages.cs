using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree;

[ToolboxItem(false)]
[TypeConverter(typeof(ExpandableObjectConverter))]
public class CellImages
{
	private Image _0E66;

	private Image _26EF;

	private int դ = -1;

	private Image _26F0;

	private int _26F1 = -1;

	private Image _26F2;

	private int _26F3 = -1;

	private Image _26F4;

	private int _26F5 = -1;

	private Cell _26F6;

	private Size _26F7 = Size.Empty;

	private string __26F8 = "";

	private string __26F9 = "";

	private string __26FA = "";

	private string __26FB = "";

	private bool __1A1E;

	internal Image _26EC
	{
		get
		{
			return _26EF;
		}
		set
		{
			_26EF = value;
		}
	}

	[Category("Images")]
	[DevCoSerialize]
	[Description("Indicates default cell image")]
	[DefaultValue(null)]
	[Browsable(true)]
	public Image Image
	{
		get
		{
			return _0E66;
		}
		set
		{
			_26FC(ref _0E66, value);
		}
	}

	[Description("Indicates cell image when mouse is over the cell")]
	[DevCoSerialize]
	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Images")]
	public Image ImageMouseOver
	{
		get
		{
			return _26F0;
		}
		set
		{
			_26FC(ref _26F0, value);
		}
	}

	[Description("Indicates disabled cell image")]
	[Category("Images")]
	[Browsable(true)]
	[DefaultValue(null)]
	public Image ImageDisabled
	{
		get
		{
			return _26F2;
		}
		set
		{
			_26FC(ref _26F2, value);
		}
	}

	[Category("Images")]
	[Description("Indicates cell image when node associtaed with this cell is expanded")]
	[DevCoSerialize]
	[Browsable(true)]
	[DefaultValue(null)]
	public Image ImageExpanded
	{
		get
		{
			return _26F4;
		}
		set
		{
			_26FC(ref _26F4, value);
		}
	}

	[Description("Indicates default cell image")]
	[DevCoSerialize]
	[Category("ImageList Images")]
	[DefaultValue(-1)]
	[Browsable(true)]
	public int ImageIndex
	{
		get
		{
			return դ;
		}
		set
		{
			դ = value;
			ᡁ();
		}
	}

	[DevCoSerialize]
	[DefaultValue("")]
	[Description("Indicates the default cell image key")]
	[Category("ImageList Images")]
	public string ImageKey
	{
		get
		{
			return __26F8;
		}
		set
		{
			__26F8 = value;
			ᡁ();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public ImageList ImageList
	{
		get
		{
			if (Parent != null)
			{
				AdvTree treeControl = Parent.TreeControl;
				if (treeControl != null)
				{
					return treeControl.ImageList;
				}
			}
			return null;
		}
	}

	[DefaultValue(-1)]
	[Category("ImageList Images")]
	[Browsable(true)]
	[Description("Indicates cell image when mouse is over the cell")]
	[DevCoSerialize]
	public int ImageMouseOverIndex
	{
		get
		{
			return _26F1;
		}
		set
		{
			_26F1 = value;
			ᡁ();
		}
	}

	[DevCoSerialize]
	[Category("ImageList Images")]
	[Browsable(true)]
	[DefaultValue("")]
	[Description("Indicates cell image when mouse is over the cell")]
	public string ImageMouseOverKey
	{
		get
		{
			return __26F9;
		}
		set
		{
			__26F9 = value;
			ᡁ();
		}
	}

	[DefaultValue(-1)]
	[Browsable(true)]
	[Description("Indicates disabled cell image")]
	[Category("ImageList Images")]
	public int ImageDisabledIndex
	{
		get
		{
			return _26F3;
		}
		set
		{
			_26F3 = value;
			ᡁ();
		}
	}

	[DefaultValue("")]
	[Browsable(true)]
	[Description("Indicates disabled cell image")]
	[Category("ImageList Images")]
	public string ImageDisabledKey
	{
		get
		{
			return __26FA;
		}
		set
		{
			__26FA = value;
			ᡁ();
		}
	}

	[Category("ImageList Images")]
	[Description("Indicates expanded cell image")]
	[Browsable(true)]
	[DevCoSerialize]
	[DefaultValue(-1)]
	public int ImageExpandedIndex
	{
		get
		{
			return _26F5;
		}
		set
		{
			_26F5 = value;
			ᡁ();
		}
	}

	[DevCoSerialize]
	[Description("Indicates expanded cell image")]
	[Category("ImageList Images")]
	[Browsable(true)]
	[DefaultValue("")]
	public string ImageExpandedKey
	{
		get
		{
			return __26FB;
		}
		set
		{
			__26FB = value;
			ᡁ();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Cell Parent
	{
		get
		{
			return _26F6;
		}
		set
		{
			_26F6 = value;
		}
	}

	internal bool _26ED
	{
		get
		{
			if (_0E66 == null && _26F2 == null && _26F3 == -1 && _26F4 == null && _26F5 == -1 && դ == -1 && _26F0 == null && _26F1 == -1)
			{
				return false;
			}
			return true;
		}
	}

	internal Size _26EE => _26F7;

	[DefaultValue(false)]
	[Browsable(false)]
	public bool AutoDispose
	{
		get
		{
			return __1A1E;
		}
		set
		{
			__1A1E = value;
		}
	}

	public CellImages(Cell parentCell)
	{
		_26F6 = parentCell;
	}

	public void ResetImage()
	{
		TypeDescriptor.GetProperties(this)["Image"].SetValue(this, null);
	}

	public void ResetImageMouseOver()
	{
		TypeDescriptor.GetProperties(this)["ImageMouseOver"].SetValue(this, null);
	}

	public void ResetImageDisabled()
	{
		ImageDisabled = null;
	}

	public void ResetImageExpanded()
	{
		TypeDescriptor.GetProperties(this)["ImageExpanded"].SetValue(this, null);
	}

	public virtual CellImages Copy()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		CellImages cellImages = new CellImages(null);
		cellImages.Image = Image;
		cellImages.ImageExpanded = ((ImageExpanded == null) ? ((Image)null) : ((Image)ImageExpanded.Clone()));
		cellImages.ImageExpandedIndex = ImageExpandedIndex;
		cellImages.ImageIndex = ImageIndex;
		cellImages.ImageMouseOver = ((ImageMouseOver == null) ? ((Image)null) : ((Image)ImageMouseOver.Clone()));
		cellImages.ImageMouseOverIndex = ImageMouseOverIndex;
		cellImages.ImageDisabledKey = ImageDisabledKey;
		cellImages.ImageExpandedKey = ImageExpandedKey;
		cellImages.ImageKey = ImageKey;
		cellImages.ImageMouseOverKey = ImageMouseOverKey;
		return cellImages;
	}

	private void _26FC(ref Image _26FD, Image _26FE)
	{
		if (_26FD == _26FE)
		{
			return;
		}
		Image val = _26FD;
		_26FD = _26FE;
		if (val == null || _26FD == null || _26FF(val) != _26FF(_26FD))
		{
			_2701();
			if (Parent != null)
			{
				Parent.OnImageChanged();
			}
		}
		_2700();
	}

	private Size _26FF(Image ແ)
	{
		try
		{
			return ແ.Size;
		}
		catch
		{
			return Size.Empty;
		}
	}

	private void ᡁ()
	{
		_2701();
		if (Parent != null)
		{
			Parent.OnImageChanged();
		}
		_2700();
	}

	public void Dispose()
	{
		_2700();
		if (BarUtilities.DisposeItemImages || __1A1E)
		{
			BarUtilities.ᆟ(ref _0E66);
			BarUtilities.ᆟ(ref _26F2);
			BarUtilities.ᆟ(ref _26F4);
			BarUtilities.ᆟ(ref _26F0);
		}
	}

	internal void _2700()
	{
		if (_26EF != null)
		{
			_26EF.Dispose();
			_26EF = null;
		}
	}

	internal void _2701()
	{
		_26F7 = Size.Empty;
		_21DF(_0E66, ref _26F7);
		_21DF(_26F2, ref _26F7);
		_21DF(_26F4, ref _26F7);
		_21DF(_26F0, ref _26F7);
		_21DF(_2702(դ), ref _26F7);
		_21DF(_2702(_26F3), ref _26F7);
		_21DF(_2702(_26F5), ref _26F7);
		_21DF(_2702(_26F1), ref _26F7);
		_21DF(_2704(__26F8), ref _26F7);
		_21DF(_2704(__26FA), ref _26F7);
		_21DF(_2704(__26FB), ref _26F7);
		_21DF(_2704(__26F9), ref _26F7);
	}

	private void _21DF(Image ແ, ref Size ߒ)
	{
		if (ແ != null)
		{
			if (ແ.Width > ߒ.Width)
			{
				ߒ.Width = ແ.Width;
			}
			if (ແ.Height > ߒ.Height)
			{
				ߒ.Height = ແ.Height;
			}
		}
	}

	internal Image _2702(int _2703)
	{
		if (_2703 >= 0 && Parent != null && Parent.TreeControl != null && Parent.TreeControl.ImageList != null && Parent.TreeControl.ImageList.Images.Count > 0)
		{
			try
			{
				return Parent.TreeControl.ImageList.Images[_2703];
			}
			catch
			{
				return null;
			}
		}
		return null;
	}

	internal Image _2704(string _੬)
	{
		if (string.IsNullOrEmpty(_੬))
		{
			return null;
		}
		Cell parent = Parent;
		if (parent == null)
		{
			return null;
		}
		AdvTree treeControl = parent.TreeControl;
		if (treeControl == null || treeControl.ImageList == null || !treeControl.ImageList.Images.ContainsKey(_੬))
		{
			return null;
		}
		return treeControl.ImageList.Images[_੬];
	}
}

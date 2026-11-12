using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace Stimulsoft.Editor;

public class Braces : IBraceMatchingEx, IBraceMatching
{
	private Color foreColor = EditConsts.DefaultBracesForeColor;

	private Color backColor = EditConsts.DefaultBracesBackColor;

	private bool useRoundRect;

	private FontStyle style = EditConsts.DefaultBracesFontStyle;

	private ISyntaxEdit owner;

	[Description("Gets or sets an array of characters each one representing an open brace.")]
	public char[] OpenBraces
	{
		get
		{
			if (owner == null)
			{
				return null;
			}
			return owner.Source.OpenBraces;
		}
		set
		{
			if (owner != null)
			{
				owner.Source.OpenBraces = value;
			}
		}
	}

	[Description("Gets or sets an array of characters each one representing a closing brace.")]
	public char[] ClosingBraces
	{
		get
		{
			if (owner == null)
			{
				return null;
			}
			return owner.Source.ClosingBraces;
		}
		set
		{
			if (owner != null)
			{
				owner.Source.ClosingBraces = value;
			}
		}
	}

	[DefaultValue(BracesOptions.None)]
	[Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor))]
	[Description("Gets or sets options specifying appearance and behaviour of matching braces within Edit control.")]
	public BracesOptions BracesOptions
	{
		get
		{
			if (owner == null)
			{
				return BracesOptions.None;
			}
			return owner.Source.BracesOptions;
		}
		set
		{
			if (owner != null)
			{
				owner.Source.BracesOptions = value;
			}
		}
	}

	[Description("Gets or sets a value that represents foreground color to draw matching braces.")]
	public Color ForeColor
	{
		get
		{
			return foreColor;
		}
		set
		{
			if (foreColor != value)
			{
				foreColor = value;
				Update();
			}
		}
	}

	[Description("Gets or sets a value that represents background color to draw matching braces.")]
	public Color BackColor
	{
		get
		{
			return backColor;
		}
		set
		{
			if (backColor != value)
			{
				backColor = value;
				Update();
			}
		}
	}

	[DefaultValue(false)]
	[Description("ets or sets a boolean value that indicates whether Edit control should draw rectangle around matching braces.")]
	public bool UseRoundRect
	{
		get
		{
			return useRoundRect;
		}
		set
		{
			if (useRoundRect != value)
			{
				useRoundRect = value;
				Update();
			}
		}
	}

	[Description("Gets or sets a FontStyle value that is used to draw matching braces.")]
	public FontStyle Style
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return style;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			if (style != value)
			{
				style = value;
				Update();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public object XmlInfo
	{
		get
		{
			return new XmlBracesInfo(this);
		}
		set
		{
			((XmlBracesInfo)value).FixupReferences(this);
		}
	}

	public Braces(ISyntaxEdit owner)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		this.owner = owner;
	}

	protected void Update()
	{
		if (BracesOptions != 0 && owner != null)
		{
			owner.Invalidate();
		}
	}

	protected bool IsEqualArrays(char[] array1, char[] array2)
	{
		bool flag = array1.Length == array2.Length;
		if (flag)
		{
			for (int i = 0; i < array1.Length; i++)
			{
				flag = array1[i] == array2[i];
				if (!flag)
				{
					break;
				}
			}
		}
		return flag;
	}

	public void Assign(IBraceMatchingEx source)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		ForeColor = source.ForeColor;
		BackColor = source.BackColor;
		Style = source.Style;
		OpenBraces = source.OpenBraces;
		ClosingBraces = source.ClosingBraces;
		BracesOptions = source.BracesOptions;
		UseRoundRect = source.UseRoundRect;
	}

	public virtual void ResetBracesOptions()
	{
		BracesOptions = BracesOptions.None;
	}

	public virtual void ResetClosingBraces()
	{
		ClosingBraces = EditConsts.DefaultClosingBraces;
	}

	public virtual void ResetOpenBraces()
	{
		OpenBraces = EditConsts.DefaultOpenBraces;
	}

	public virtual void ResetForeColor()
	{
		ForeColor = EditConsts.DefaultBracesForeColor;
	}

	public virtual void ResetBackColor()
	{
		BackColor = EditConsts.DefaultBracesBackColor;
	}

	public virtual void ResetUseRoundRect()
	{
		UseRoundRect = false;
	}

	public virtual void ResetStyle()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		Style = EditConsts.DefaultBracesFontStyle;
	}

	public bool ShouldSerializeForeColor()
	{
		return foreColor != EditConsts.DefaultBracesForeColor;
	}

	public bool ShouldSerializeBackColor()
	{
		return backColor != EditConsts.DefaultBracesBackColor;
	}

	public bool ShouldSerializeStyle()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return style != EditConsts.DefaultBracesFontStyle;
	}

	public bool FindClosingBrace(ref Point position)
	{
		if (owner == null)
		{
			return false;
		}
		return owner.Source.FindClosingBrace(ref position);
	}

	public bool FindClosingBrace(ref int x, ref int y)
	{
		if (owner == null)
		{
			return false;
		}
		return owner.Source.FindClosingBrace(ref x, ref y);
	}

	public bool FindOpenBrace(ref Point position)
	{
		if (owner == null)
		{
			return false;
		}
		return owner.Source.FindOpenBrace(ref position);
	}

	public bool FindOpenBrace(ref int x, ref int y)
	{
		if (owner == null)
		{
			return false;
		}
		return owner.Source.FindOpenBrace(ref x, ref y);
	}

	public void TempHighlightBraces(Rectangle[] rects)
	{
		if (owner != null)
		{
			owner.Source.TempHighlightBraces(rects);
		}
	}

	public void TempUnhighlightBraces()
	{
		if (owner != null)
		{
			owner.Source.TempUnhighlightBraces();
		}
	}

	public bool ShouldSerializeOpenBraces()
	{
		if (((SyntaxEdit)owner).ShouldSerializeSourceProps())
		{
			return !IsEqualArrays(OpenBraces, EditConsts.DefaultOpenBraces);
		}
		return false;
	}

	public bool ShouldSerializeClosingBraces()
	{
		if (((SyntaxEdit)owner).ShouldSerializeSourceProps())
		{
			return !IsEqualArrays(ClosingBraces, EditConsts.DefaultClosingBraces);
		}
		return false;
	}
}

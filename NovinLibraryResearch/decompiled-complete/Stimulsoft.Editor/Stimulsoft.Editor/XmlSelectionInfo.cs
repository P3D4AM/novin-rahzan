using System.Drawing;

namespace Stimulsoft.Editor;

public class XmlSelectionInfo
{
	private Selection owner;

	private Rectangle selectionRect;

	private SelectionType selectionType;

	private SelectionOptions options = EditConsts.DefaultSelectionOptions;

	private string foreColor = EditConsts.DefaultHighlightForeColor.Name;

	private string backColor = EditConsts.DefaultHighlightBackColor.Name;

	private string inActiveForeColor = EditConsts.DefaultInactiveHighlightForeColor.Name;

	private string inActiveBackColor = EditConsts.DefaultInactiveHighlightBackColor.Name;

	public Rectangle SelectionRect
	{
		get
		{
			if (owner == null)
			{
				return selectionRect;
			}
			return owner.SelectionRect;
		}
		set
		{
			selectionRect = value;
			if (owner != null)
			{
				owner.SelectionRect = value;
			}
		}
	}

	public SelectionType SelectionType
	{
		get
		{
			if (owner == null)
			{
				return selectionType;
			}
			return owner.SelectionType;
		}
		set
		{
			selectionType = value;
			if (owner != null)
			{
				owner.SelectionType = value;
			}
		}
	}

	public SelectionOptions Options
	{
		get
		{
			if (owner == null)
			{
				return options;
			}
			return owner.Options;
		}
		set
		{
			options = value;
			if (owner != null)
			{
				owner.Options = value;
			}
		}
	}

	public string ForeColor
	{
		get
		{
			if (owner == null)
			{
				return foreColor;
			}
			return XmlHelper.SerializeColor(owner.ForeColor);
		}
		set
		{
			foreColor = value;
			if (owner != null)
			{
				owner.ForeColor = XmlHelper.DeserializeColor(value);
			}
		}
	}

	public string BackColor
	{
		get
		{
			if (owner == null)
			{
				return backColor;
			}
			return XmlHelper.SerializeColor(owner.BackColor);
		}
		set
		{
			backColor = value;
			if (owner != null)
			{
				owner.BackColor = XmlHelper.DeserializeColor(value);
			}
		}
	}

	public string InActiveForeColor
	{
		get
		{
			if (owner == null)
			{
				return inActiveForeColor;
			}
			return XmlHelper.SerializeColor(owner.InActiveForeColor);
		}
		set
		{
			inActiveForeColor = value;
			if (owner != null)
			{
				owner.InActiveForeColor = XmlHelper.DeserializeColor(value);
			}
		}
	}

	public string InActiveBackColor
	{
		get
		{
			if (owner == null)
			{
				return inActiveBackColor;
			}
			return XmlHelper.SerializeColor(owner.InActiveBackColor);
		}
		set
		{
			inActiveBackColor = value;
			if (owner != null)
			{
				owner.InActiveBackColor = XmlHelper.DeserializeColor(value);
			}
		}
	}

	public XmlSelectionInfo()
	{
	}

	public XmlSelectionInfo(Selection owner)
		: this()
	{
		this.owner = owner;
	}

	public virtual void Load()
	{
		if (owner != null)
		{
			selectionRect = SelectionRect;
			selectionType = SelectionType;
			options = Options;
			foreColor = ForeColor;
			backColor = BackColor;
			inActiveForeColor = InActiveForeColor;
			inActiveBackColor = InActiveBackColor;
		}
	}

	public virtual void FixupReferences(Selection owner)
	{
		this.owner = owner;
		SelectionRect = selectionRect;
		SelectionType = selectionType;
		Options = options;
		ForeColor = foreColor;
		BackColor = backColor;
		InActiveForeColor = inActiveForeColor;
		InActiveBackColor = inActiveBackColor;
	}
}

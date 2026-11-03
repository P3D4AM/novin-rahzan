using System.ComponentModel;

namespace DevComponents.DotNetBar;

[TypeConverter(typeof(ExpandableObjectConverter))]
[ToolboxItem(false)]
public class RibbonLocalization
{
	private string Შ = "&Remove from Quick Access Toolbar";

	private string Ჩ = "&Add to Quick Access Toolbar";

	private string Ც = "&Customize Quick Access Toolbar...";

	private string Ძ = "&Place Quick Access Toolbar below the Ribbon";

	private string Წ = "&Place Quick Access Toolbar above the Ribbon";

	private string Ჭ = "OK";

	private string Ხ = "Cancel";

	private string Ჯ = "&Add >>";

	private string Ჰ = "&Remove";

	private string Ჱ = "&Choose commands from:";

	private string Ჲ = "&Place Quick Access Toolbar below the Ribbon";

	private string Ჳ = "Customize Quick Access Toolbar";

	private string Ჴ = "Mi&nimize the Ribbon";

	private string Ჵ = "&Maximize the Ribbon";

	private string Ჶ = "<b>Customize Quick Access Toolbar</b>";

	[Category("QAT Customize Dialog")]
	[Description("Indicates the the title text of the Quick Access Toolbar Customize dialog form.")]
	[Localizable(true)]
	public string QatDialogCaption
	{
		get
		{
			return Ჳ;
		}
		set
		{
			Ჳ = value;
		}
	}

	[Category("QAT Customize Dialog")]
	[Description("Indicates the text of the 'Place Quick Access Toolbar below the Ribbon' check-box on the Quick Access Toolbar Customize dialog form.")]
	[Localizable(true)]
	public string QatDialogPlacementCheckbox
	{
		get
		{
			return Ჲ;
		}
		set
		{
			Ჲ = value;
		}
	}

	[Description("Indicates the text of the Choose commands from label on the Quick Access Toolbar Customize dialog form.")]
	[Localizable(true)]
	[Category("QAT Customize Dialog")]
	public string QatDialogCategoriesLabel
	{
		get
		{
			return Ჱ;
		}
		set
		{
			Ჱ = value;
		}
	}

	[Description("Indicates the text of the Remove button on the Quick Access Toolbar Customize dialog form.")]
	[Localizable(true)]
	[Category("QAT Customize Dialog")]
	public string QatDialogRemoveButton
	{
		get
		{
			return Ჰ;
		}
		set
		{
			Ჰ = value;
		}
	}

	[Category("QAT Customize Dialog")]
	[Description("Indicates the text of the Add button on the Quick Access Toolbar Customize dialog form.")]
	[Localizable(true)]
	public string QatDialogAddButton
	{
		get
		{
			return Ჯ;
		}
		set
		{
			Ჯ = value;
		}
	}

	[Category("QAT Customize Dialog")]
	[Description("Indicates the text of the OK button on the Quick Access Toolbar Customize dialog form.")]
	[Localizable(true)]
	public string QatDialogOkButton
	{
		get
		{
			return Ჭ;
		}
		set
		{
			Ჭ = value;
		}
	}

	[Localizable(true)]
	[Category("QAT Customize Dialog")]
	[Description("Indicates the text of the OK button on the Quick Access Toolbar Customize dialog form.")]
	public string QatDialogCancelButton
	{
		get
		{
			return Ხ;
		}
		set
		{
			Ხ = value;
		}
	}

	[Category("Quick Access Toolbar")]
	[Localizable(true)]
	[Description("Indicates the text that is used on context menu used to customize Quick Access Toolbar.")]
	public string QatRemoveItemText
	{
		get
		{
			return Შ;
		}
		set
		{
			Შ = value;
		}
	}

	[Localizable(true)]
	[Description("Indicates the text that is used on context menu used to customize Quick Access Toolbar.")]
	[Category("Quick Access Toolbar")]
	public string QatAddItemText
	{
		get
		{
			return Ჩ;
		}
		set
		{
			Ჩ = value;
		}
	}

	[Description("Indicates the text that is used on context menu used to customize Quick Access Toolbar.")]
	[Localizable(true)]
	[Category("Quick Access Toolbar")]
	public string QatCustomizeText
	{
		get
		{
			return Ც;
		}
		set
		{
			Ც = value;
		}
	}

	[Description("Indicates text that is used on Quick Access Toolbar customize menu label.")]
	[Localizable(true)]
	[Category("Quick Access Toolbar")]
	public string QatCustomizeMenuLabel
	{
		get
		{
			return Ჶ;
		}
		set
		{
			Ჶ = value;
		}
	}

	[Description("Indicates the text that is used on context menu used to change placement of the Quick Access Toolbar.")]
	[Category("Quick Access Toolbar")]
	[Localizable(true)]
	public string QatPlaceBelowRibbonText
	{
		get
		{
			return Ძ;
		}
		set
		{
			Ძ = value;
		}
	}

	[Category("Quick Access Toolbar")]
	[Description("Indicates the text that is used on context menu used to change placement of the Quick Access Toolbar.")]
	[Localizable(true)]
	public string QatPlaceAboveRibbonText
	{
		get
		{
			return Წ;
		}
		set
		{
			Წ = value;
		}
	}

	[Localizable(true)]
	[Description("Indicates text that is used on context menu item used to minimize the Ribbon.")]
	[Category("Quick Access Toolbar")]
	public string MinimizeRibbonText
	{
		get
		{
			return Ჴ;
		}
		set
		{
			Ჴ = value;
		}
	}

	[Category("Quick Access Toolbar")]
	[Description("Indicates text that is used on context menu item used to maximize the Ribbon.")]
	[Localizable(true)]
	public string MaximizeRibbonText
	{
		get
		{
			return Ჵ;
		}
		set
		{
			Ჵ = value;
		}
	}
}

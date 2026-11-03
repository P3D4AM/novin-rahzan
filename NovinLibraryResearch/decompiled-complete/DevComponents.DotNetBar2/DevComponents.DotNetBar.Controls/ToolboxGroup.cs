using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar.Controls;

[Designer("DevComponents.DotNetBar.Design.ToolboxGroupDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(false)]
[DefaultEvent("Click")]
[DesignTimeVisible(false)]
public class ToolboxGroup : ItemContainer
{
	private List<BaseItem> _Ȿ = new List<BaseItem>();

	private ElementStyle _Ⲛ;

	private bool _ⲛ = true;

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Style")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Specifies the title background style when toolbox group is expanded.")]
	public ElementStyle TitleGroupExpandedStyle => _Ⲛ;

	[Category("Behavior")]
	[Description("Indicates whether items in vertical layout orientation with MultiLine=false are stretched to fill group width. Default value is true.")]
	[DefaultValue(true)]
	public bool StretchItems
	{
		get
		{
			return _ⲛ;
		}
		set
		{
			_ⲛ = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[DefaultValue(false)]
	public override bool Expanded
	{
		get
		{
			return base.Expanded;
		}
		set
		{
			base.Expanded = value;
		}
	}

	public override bool CanExpand => true;

	[DefaultValue(true)]
	[Category("Layout")]
	[Browsable(true)]
	[Description("Indicates whether items in horizontal layout are wrapped into the new line when they cannot fit allotted container size.")]
	public override bool MultiLine
	{
		get
		{
			return base.MultiLine;
		}
		set
		{
			base.MultiLine = value;
		}
	}

	[Description("Indicates whether items contained by container are resized to fit the container bounds. When container is in horizontal layout mode then all items will have the same height. When container is in vertical layout mode then all items will have the same width.")]
	[Browsable(true)]
	[Category("Layout")]
	[DefaultValue(false)]
	[DevCoBrowsable(true)]
	public override bool ResizeItemsToFit
	{
		get
		{
			return base.ResizeItemsToFit;
		}
		set
		{
			base.ResizeItemsToFit = value;
		}
	}

	[Description("Indicates whether all items are equally sized based on the size of the largest item in the list.")]
	[DefaultValue(false)]
	[Category("Layout")]
	public override bool EqualItemSize
	{
		get
		{
			return base.EqualItemSize;
		}
		set
		{
			base.EqualItemSize = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override bool Visible
	{
		get
		{
			return base.Visible;
		}
		set
		{
			base.Visible = value;
		}
	}

	public ToolboxGroup()
	{
		MultiLine = true;
		ResizeItemsToFit = false;
		base.TitleStyle.Class = ElementStyleClassKeys.ToolboxGroupTitleKey;
		base.TitleMouseOverStyle.Class = ElementStyleClassKeys.ToolboxGroupTitleMouseOverKey;
		GlobalItem = false;
		EqualItemSize = true;
		_Ⲛ = new ElementStyle();
		_Ⲛ.StyleChanged += ⲝ;
		_Ⲛ.Class = ElementStyleClassKeys.ToolboxGroupExpandedTitleKey;
	}

	public ToolboxGroup(string groupText)
		: this()
	{
		base.TitleText = groupText;
	}

	public override BaseItem Copy()
	{
		ToolboxGroup toolboxGroup = new ToolboxGroup(Name);
		CopyToItem(toolboxGroup);
		return toolboxGroup;
	}

	internal void _0EBA(ButtonItem _0EBB)
	{
		CopyToItem(_0EBB);
	}

	protected override void CopyToItem(BaseItem copy)
	{
		ToolboxGroup toolboxGroup = copy as ToolboxGroup;
		base.CopyToItem((BaseItem)toolboxGroup);
		if (toolboxGroup != null)
		{
			toolboxGroup.ResizeItemsToFit = ResizeItemsToFit;
		}
	}

	protected internal override void OnExpandChange()
	{
		Ⲝ();
		base.OnExpandChange();
	}

	private void Ⲝ()
	{
		ToolboxControl toolboxControl = GetToolboxControl();
		bool flag = false;
		if (toolboxControl != null && toolboxControl.IsSearching)
		{
			flag = true;
		}
		if (Expanded)
		{
			foreach (BaseItem item in _Ȿ)
			{
				item.Visible = true;
			}
			_Ȿ.Clear();
			if (toolboxControl != null)
			{
				if (toolboxControl.ExpandSingleGroupOnly && !toolboxControl.IsSearching)
				{
					foreach (ToolboxGroup group in toolboxControl.Groups)
					{
						if (group is ToolboxGroup toolboxGroup && toolboxGroup != this && toolboxGroup.Expanded)
						{
							toolboxGroup.Expanded = false;
						}
					}
				}
				if (!flag)
				{
					ⁱ();
				}
				toolboxControl.Ⲑ(this);
			}
		}
		else
		{
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem.Visible)
				{
					subItem.Visible = false;
					_Ȿ.Add(subItem);
				}
			}
		}
		NeedRecalcSize = true;
		if (!flag)
		{
			ⁱ();
		}
	}

	public ToolboxControl GetToolboxControl()
	{
		if (ContainerControl is ItemPanel itemPanel)
		{
			return ((Control)itemPanel).Parent as ToolboxControl;
		}
		return null;
	}

	private void ⁱ()
	{
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val != null)
		{
			BarFunctions.InvokeRecalcLayout(val, invalidate: true);
		}
	}

	protected override void OnSubItemsChanged(CollectionChangeAction action, BaseItem item)
	{
		switch (action)
		{
		case CollectionChangeAction.Add:
			if (!Expanded && item != null && item.Visible)
			{
				item.Visible = false;
				_Ȿ.Add(item);
			}
			else if (Expanded && item != null && !item.Visible)
			{
				item.Visible = true;
			}
			break;
		case CollectionChangeAction.Remove:
			if (!Expanded && item != null && _Ȿ.Contains(item))
			{
				_Ȿ.Remove(item);
			}
			break;
		case CollectionChangeAction.Refresh:
			_Ȿ.Clear();
			if (!Expanded)
			{
				Ⲝ();
			}
			break;
		}
		base.OnSubItemsChanged(action, item);
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		if (base.TitleRectangle.Contains(objArg.Location))
		{
			Expanded = !Expanded;
		}
		base.InternalMouseDown(objArg);
	}

	protected override ElementStyle GetActiveTitleStyle()
	{
		if (Expanded && _Ⲛ.Custom)
		{
			return _Ⲛ;
		}
		return base.GetActiveTitleStyle();
	}

	private void ⲝ(object _٠, EventArgs ؿ)
	{
		OnAppearanceChanged();
	}

	protected override IContentLayout GetContentLayout()
	{
		SerialContentLayoutManager serialContentLayoutManager = (SerialContentLayoutManager)base.GetContentLayout();
		ToolboxControl toolboxControl = GetToolboxControl();
		if (_ⲛ && LayoutOrientation == eOrientation.Vertical && !MultiLine && toolboxControl != null && toolboxControl.Expanded)
		{
			serialContentLayoutManager.VerticalFitContainerWidth = true;
		}
		return serialContentLayoutManager;
	}
}

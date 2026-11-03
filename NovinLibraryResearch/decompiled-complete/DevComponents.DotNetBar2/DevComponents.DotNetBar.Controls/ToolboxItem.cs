using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

[DesignTimeVisible(false)]
[DefaultEvent("Click")]
[Designer("DevComponents.DotNetBar.Design.ToolboxItemDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(false)]
public class ToolboxItem : ButtonItem
{
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

	public ToolboxItem()
	{
	}

	public ToolboxItem(string text)
	{
		Text = text;
		ButtonStyle = eButtonStyle.ImageAndText;
	}

	public ToolboxItem(string text, Image image)
	{
		Text = text;
		base.Image = image;
		ButtonStyle = eButtonStyle.ImageAndText;
	}

	public ToolboxItem(string text, string symbol, eSymbolSet symbolSet)
	{
		Text = text;
		base.SymbolSet = symbolSet;
		base.Symbol = symbol;
		ButtonStyle = eButtonStyle.ImageAndText;
	}

	public override BaseItem Copy()
	{
		ToolboxItem toolboxItem = new ToolboxItem(Name);
		CopyToItem(toolboxItem);
		return toolboxItem;
	}

	internal void _0EBA(ToolboxItem _0EBB)
	{
		CopyToItem(_0EBB);
	}

	protected override void OnCheckedChanged()
	{
		base.OnCheckedChanged();
		GetToolboxControl()?.ⲓ(this);
	}

	public ToolboxControl GetToolboxControl()
	{
		if (ContainerControl is ItemPanel itemPanel)
		{
			return ((Control)itemPanel).Parent as ToolboxControl;
		}
		return null;
	}

	protected override bool ShouldDrawInsertMarker()
	{
		if (base.DesignInsertMarker != 0 && Visible && Displayed)
		{
			return !DesignMode;
		}
		return false;
	}
}

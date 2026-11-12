using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Design.Toolbars;

[StiServiceBitmap(typeof(StiToolbarService), "Stimulsoft.Report.Design.Images.Designer.ToolbarService.png")]
[StiServiceCategoryBitmap(typeof(StiDesignerInitService), "Stimulsoft.Report.Design.Images.Designer.ToolbarService.png")]
public abstract class StiToolbarService : StiDesignerInitService
{
	public sealed override string ServiceCategory => StiLocalization.Get("MainMenu", "menuViewToolbars");

	public sealed override Type ServiceType => typeof(StiToolbarService);

	[Browsable(false)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public virtual int DefaultDockLine => 1;

	[Browsable(false)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public virtual bool DefaultVisible => true;

	[Browsable(false)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public virtual DockStyle DefaultDock => (DockStyle)1;

	public abstract int Position { get; }

	protected bool AddSeparatorToNextItem { get; set; }

	public Bar ToolBar { get; set; }

	protected List<StiToolButtonInfo> ToolButtons { get; set; }

	public static StiService[] GetToolbarServices(StiDesignerControl designer)
	{
		return designer.Services.GetServices(typeof(StiToolbarService)).ToArray();
	}

	protected void AddToolButtonSeparator()
	{
		AddSeparator();
	}

	protected void Add(ref ButtonItem button, string hint, EventHandler handler)
	{
		Add(ref button, null, -1, hint, handler);
	}

	protected void Add(ref ButtonItem button, Image image, string hint, EventHandler handler)
	{
		button = new ButtonItem
		{
			Image = image,
			BeginGroup = AddSeparatorToNextItem
		};
		button.Click += handler;
		AddSeparatorToNextItem = false;
		ToolBar.Items.Add(button);
		button.Text = StiHint.CreateHint(hint);
	}

	protected void AddSub(ref ButtonItem button, ButtonItem parentButton, Image image, string hint, EventHandler handler, MouseEventHandler mouseEventHandler)
	{
		button = new ButtonItem
		{
			Image = image,
			BeginGroup = AddSeparatorToNextItem
		};
		button.Click += handler;
		button.MouseMove += mouseEventHandler;
		AddSeparatorToNextItem = false;
		parentButton.SubItems.Add(button);
		button.Text = StiHint.CreateHint(hint);
	}

	protected void Add(ref ButtonItem button, Image image, string hint, EventHandler handler, int buttonIndex)
	{
		button = new ButtonItem
		{
			Image = image,
			BeginGroup = AddSeparatorToNextItem
		};
		button.Click += handler;
		AddSeparatorToNextItem = false;
		ToolBar.Items.Insert(buttonIndex, button);
		button.Text = StiHint.CreateHint(hint);
	}

	protected void AddColorItem(ref ColorPickerDropDown button, Image image, string hint, EventHandler selectedColorChangedHandler)
	{
		button = new ColorPickerDropDown
		{
			Image = image,
			BeginGroup = AddSeparatorToNextItem,
			AutoExpandOnClick = true,
			SelectedColorImageRectangle = new Rectangle(StiScale.I1, StiScale.I14, StiScale.XXI(15.0), StiScale.I3)
		};
		button.SelectedColorChanged += selectedColorChangedHandler;
		button.Localize();
		AddSeparatorToNextItem = false;
		ToolBar.Items.Add(button);
		button.Text = StiHint.CreateHint(hint);
	}

	protected void Add(ref ButtonItem button, ImageList imageList, int imageIndex, string hint, EventHandler handler)
	{
		if (imageList != null && imageList.Images.Count > imageIndex && imageIndex >= 0)
		{
			Add(ref button, imageList.Images[imageIndex], hint, handler);
		}
		else
		{
			Add(ref button, null, hint, handler);
		}
	}

	protected void Add(ref ButtonItem button, ImageList imageList, int imageIndex, string hint, EventHandler handler, int buttonIndex)
	{
		if (imageList != null && imageList.Images.Count > imageIndex && imageIndex >= 0)
		{
			Add(ref button, imageList.Images[imageIndex], hint, handler, buttonIndex);
		}
		else
		{
			Add(ref button, null, hint, handler, buttonIndex);
		}
	}

	public void AddToolButton(ImageList imageList, int imageIndex, EventHandler eventHandler)
	{
		AddToolButton(imageList, imageIndex, string.Empty, eventHandler);
	}

	public void AddSeparator()
	{
		AddSeparatorToNextItem = true;
	}

	public void AddToolButton(string text, EventHandler eventHandler)
	{
		AddToolButton(null, -1, text, eventHandler);
	}

	public void AddToolButton(ImageList imageList, int imageIndex, string text, EventHandler eventHandler)
	{
		AddToolButton(imageList, imageIndex, text, eventHandler, -1);
	}

	public void AddToolButton(ImageList imageList, int imageIndex, string text, EventHandler eventHandler, int buttonIndex)
	{
		if (this is StiToolboxToolbarService)
		{
			throw new Exception("You can't add Tool Button to StiToolboxToolbarService!");
		}
		if (ToolButtons == null)
		{
			ToolButtons = new List<StiToolButtonInfo>();
		}
		ToolButtons.Add(new StiToolButtonInfo(imageList, imageIndex, text, eventHandler, buttonIndex));
	}

	protected void CreateToolButtons()
	{
		if (ToolButtons == null || ToolButtons.Count <= 0)
		{
			return;
		}
		foreach (StiToolButtonInfo toolButton in ToolButtons)
		{
			if (toolButton.Text == "-")
			{
				AddToolButtonSeparator();
			}
			else if (toolButton.ButtonIndex == -1)
			{
				ButtonItem button = null;
				Add(ref button, toolButton.ImageList, toolButton.ImageIndex, toolButton.Text, toolButton.EventHandler);
				button.Text = toolButton.Text;
			}
			else
			{
				ButtonItem button2 = null;
				Add(ref button2, toolButton.ImageList, toolButton.ImageIndex, toolButton.Text, toolButton.EventHandler, toolButton.ButtonIndex);
				button2.Text = toolButton.Text;
			}
		}
	}

	protected void DisposeComponent(Component component)
	{
		try
		{
			component?.Dispose();
		}
		catch
		{
		}
	}

	protected void CreateToolbar()
	{
		Bar bar = new Bar();
		((Control)bar).AccessibleRole = (AccessibleRole)22;
		bar.GrabHandleStyle = eGrabHandleStyle.Office2003;
		bar.PaddingTop = 2;
		bar.PaddingRight = 2;
		bar.PaddingBottom = 2;
		bar.ItemSpacing = 1;
		((Control)bar).Text = ServiceName;
		bar.CanHide = true;
		bar.CanCustomize = false;
		bar.Name = GetType().ToString();
		bar.DockLine = DefaultDockLine;
		ToolBar = bar;
	}

	protected virtual bool GetDesignerToolbarVisibility()
	{
		if (Designer == null)
		{
			return true;
		}
		if (this is StiBordersToolbarService)
		{
			return Designer.ShowToolbarBorders;
		}
		if (this is StiDockStyleToolbarService)
		{
			return Designer.ShowToolbarDockStyle;
		}
		if (this is StiFormattingToolbarService)
		{
			return Designer.ShowToolbarFormatting;
		}
		if (this is StiLayoutToolbarService)
		{
			return Designer.ShowToolbarLayout;
		}
		if (this is StiStandardToolbarService)
		{
			return Designer.ShowToolbarStandard;
		}
		if (this is StiStatusBarService)
		{
			return Designer.ShowToolbarStatusBar;
		}
		if (this is StiStyleToolbarService)
		{
			return Designer.ShowToolbarStyle;
		}
		if (this is StiTextFormatToolbarService)
		{
			return Designer.ShowToolbarTextFormat;
		}
		if (this is StiToolsToolbarService)
		{
			return Designer.ShowToolbarTools;
		}
		return true;
	}

	protected void AddToolbarToDockSite()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Invalid comparison between Unknown and I4
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Invalid comparison between Unknown and I4
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Invalid comparison between Unknown and I4
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Invalid comparison between Unknown and I4
		CreateToolButtons();
		if (!(this is StiStatusBarService))
		{
			AddCustomizeItem();
		}
		ToolBar.Visible = DefaultVisible && base.ServiceEnabled && GetDesignerToolbarVisibility();
		if ((int)DefaultDock == 1)
		{
			((Control)Designer.DockSiteTopBars).Controls.Add((Control)(object)ToolBar);
		}
		if ((int)DefaultDock == 2)
		{
			((Control)Designer.DockSiteBottomBars).Controls.Add((Control)(object)ToolBar);
		}
		if ((int)DefaultDock == 3)
		{
			ToolBar.DockOrientation = eOrientation.Vertical;
			((Control)Designer.DockSiteLeftBars).Controls.Add((Control)(object)ToolBar);
		}
		if ((int)DefaultDock == 4)
		{
			ToolBar.DockOrientation = eOrientation.Vertical;
			((Control)Designer.DockSiteRightBars).Controls.Add((Control)(object)ToolBar);
		}
	}

	protected void AddCustomizeItem()
	{
		ToolBar.Items.Add(new CustomizeItem
		{
			CustomizeItemVisible = false
		});
	}

	protected bool CheckPropertyEnabled(StiReport report, string propName)
	{
		if (report == null)
		{
			return false;
		}
		StiPage currentPage = report.GetCurrentPage();
		if (currentPage == null)
		{
			return false;
		}
		StiComponentsCollection selectedComponents = currentPage.GetSelectedComponents();
		if (currentPage.IsSelected)
		{
			selectedComponents.Add(currentPage);
		}
		foreach (StiComponent item in selectedComponents)
		{
			PropertyDescriptorCollection propertiesFromComponent = StiPropertiesTab.GetPropertiesFromComponent(item, null, (PropertySort)1, StiGuiMode.Gdi);
			if (propertiesFromComponent.Find(propName, ignoreCase: false) == null)
			{
				return false;
			}
		}
		return true;
	}

	protected void SetSuperToolTip(BaseItem item, string header, string localizeName, Graphics g)
	{
		string bodyText = StiLocalization.Get("HelpDesigner", localizeName);
		SuperTooltipInfo superTooltipInfo = new SuperTooltipInfo(header, null, bodyText, null, null, Designer.CurrentTooltipColor);
		superTooltipInfo.FooterVisible = false;
		SizeF sizeF = Designer.SuperToolTip.MeasureHeaderText(g, header);
		SizeF sizeF2 = Designer.SuperToolTip.MeasureBodyText(g, bodyText);
		int width = ((sizeF.Width > sizeF2.Width) ? ((int)Math.Round((double)sizeF.Width + 0.5) + 15) : ((int)Math.Round((double)sizeF2.Width + 0.5) + 25));
		superTooltipInfo.CustomSize = new Size(width, (int)Math.Round(sizeF2.Height + sizeF.Height) + 15);
		Designer.SuperToolTip.SetSuperTooltip(item, superTooltipInfo);
	}

	public override void Dispose()
	{
		base.Dispose();
		DisposeComponent((Component)(object)ToolBar);
		ToolBar = null;
	}
}

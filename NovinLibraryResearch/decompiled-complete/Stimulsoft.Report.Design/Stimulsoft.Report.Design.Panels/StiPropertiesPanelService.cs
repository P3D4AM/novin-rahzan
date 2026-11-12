using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Base.Server;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Images.Office2013;

namespace Stimulsoft.Report.Design.Panels;

[StiServiceBitmap(typeof(StiPropertiesPanelService), "Stimulsoft.Report.Design.Images.Panels.Properties.png")]
public class StiPropertiesPanelService : StiPanelService
{
	public class StiSortComponentComparer : IComparer<StiComponent>
	{
		public int Compare(StiComponent x, StiComponent y)
		{
			if (x.Name == null)
			{
				if (y.Name == null)
				{
					return 0;
				}
				return -1;
			}
			if (y.Name == null)
			{
				return 1;
			}
			return x.Name.CompareTo(y.Name);
		}
	}

	public class StiToolStripDropDownButton : ToolStripDropDownButton
	{
		protected override void OnPaint(PaintEventArgs e)
		{
			((ToolStripDropDownButton)this).OnPaint(e);
			int verticalScrollBarWidth = SystemInformation.VerticalScrollBarWidth;
			StiButtonHelper.DrawDownArrow(e.Graphics, new Rectangle(((ToolStripItem)this).ContentRectangle.Right - verticalScrollBarWidth, ((ToolStripItem)this).ContentRectangle.Top, verticalScrollBarWidth, ((ToolStripItem)this).ContentRectangle.Height), ((ToolStripItem)this).Enabled);
		}

		public StiToolStripDropDownButton()
		{
			((ToolStripDropDownButton)this).ShowDropDownArrow = false;
		}
	}

	private bool lockComponents;

	private bool lockRefreshComponents;

	private bool propertyValueChangedLock;

	private Hashtable components;

	private Panel panelConfiguration;

	private StiComboBox comboBoxComponents;

	private ButtonItem itemSortItems;

	private ToolStripDropDownButton itemSettings;

	private ToolStripMenuItem itemLocalizePropertyGrid;

	private ToolStripMenuItem itemShowDescription;

	private ToolStripSeparator itemSeparator;

	private ToolStripMenuItem itemBasic;

	private ToolStripMenuItem itemStandard;

	private ToolStripMenuItem itemProfessional;

	public override string ServiceName => StiLocalization.Get("Panels", "Properties");

	public override DockStyle DockStyle => (DockStyle)4;

	public override eShortcut Shortcut => eShortcut.F4;

	public PropertySort PropertySort
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((PropertyGrid)PropertyGrid).PropertySort;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((PropertyGrid)PropertyGrid).PropertySort = value;
		}
	}

	internal StiReportPropertyGrid PropertyGrid { get; private set; }

	public bool RefreshLock { get; set; }

	private void ComboBoxComponents_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (lockComponents)
			{
				return;
			}
			object obj = components[((ComboBox)comboBoxComponents).SelectedItem.ToString()];
			lockComponents = true;
			if (obj is StiReport)
			{
				Report.ResetSelection();
				Report.Select();
				((PropertyGrid)PropertyGrid).SelectedObject = Report;
				((Control)Designer).Refresh();
			}
			else
			{
				StiComponent stiComponent = obj as StiComponent;
				Report.ResetSelection();
				stiComponent.Select();
				((PropertyGrid)PropertyGrid).SelectedObject = stiComponent;
				if (stiComponent.Page != Report.GetCurrentPage())
				{
					Designer.SelectTabPage(stiComponent.Page);
				}
				((Control)Designer).Refresh();
			}
			lockComponents = false;
		}
		catch
		{
			lockComponents = false;
		}
	}

	private void Designer_RefreshDesigner(object sender, EventArgs e)
	{
		((Control)comboBoxComponents).Enabled = Designer.TabControl.SelectedTabIndex < Designer.PageCodeIndex;
		if (Report == null || Designer.Report.IsPageDesigner)
		{
			((Control)comboBoxComponents).Enabled = false;
		}
		if (((Control)comboBoxComponents).Enabled)
		{
			RefreshComponent();
		}
		((PropertyGrid)PropertyGrid).HelpVisible = StiOptions.Designer.PropertyGrid.ShowDescription;
	}

	private void Designer_SelectedObjectsChanged(object sender, EventArgs e)
	{
		if (RefreshLock || PropertyGrid == null || Designer == null)
		{
			return;
		}
		try
		{
			((Control)PropertyGrid).BeginInvoke((Delegate)(Action)delegate
			{
				bool flag = true;
				object[] array = ((((PropertyGrid)PropertyGrid).SelectedObjects != null && ((PropertyGrid)PropertyGrid).SelectedObjects.Length != 0 && ((PropertyGrid)PropertyGrid).SelectedObjects.Any((object c) => c is StiSeries)) ? ((PropertyGrid)PropertyGrid).SelectedObjects : Designer.SelectedObjects);
				object[] array2 = array;
				foreach (object obj2 in array2)
				{
					if (obj2 is StiComponent comp && !StiRestrictionsHelper.IsAllowChange(comp))
					{
						flag = false;
					}
				}
				int scrollOffset = PropertyGrid.ScrollOffset;
				if (StiOptions.Designer.PropertyGrid.PropertyLevel.HasValue)
				{
					((PropertyGrid)PropertyGrid).SelectedObjects = array;
				}
				PropertyGrid.ScrollOffset = scrollOffset;
				if (((Control)PropertyGrid).Enabled != flag)
				{
					((Control)PropertyGrid).Enabled = flag;
				}
			});
		}
		catch
		{
		}
	}

	private void PropertyGrid_Enter(object sender, EventArgs e)
	{
		if (Report != null)
		{
			Designer.CreateUndoRedoProperty();
		}
	}

	private void PropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
	{
		if (!propertyValueChangedLock)
		{
			if (CheckRestrictions(e))
			{
				return;
			}
			if (Designer.UndoRedo.UndoLimit > 0 && Designer.UndoRedo.ServiceEnabled)
			{
				Designer.UndoRedoProperty.Name = string.Format(StiLocalization.Get("FormDesigner", "PropertyChange"), e.ChangedItem.Label);
				if (Designer.UndoRedo != null)
				{
					Designer.UndoRedoProperty.Value = StiGZipHelper.Pack(Encoding.UTF8.GetBytes((string)Designer.UndoRedoProperty.Value));
					Designer.UndoRedo.UndoStack.Push(Designer.UndoRedoProperty);
				}
				if (Designer.UndoRedo != null)
				{
					Designer.UndoRedo.RedoStack.Clear();
				}
			}
			Designer.Report.IsModified = true;
			Designer.Report.ReportChanged = DateTime.Now;
			Designer.CreateUndoRedoProperty();
			switch (e.ChangedItem.PropertyDescriptor.Name)
			{
			case "EngineVersion":
				Designer.InvokeBuidingToolbox();
				Designer.InvokeRefreshDictionary();
				break;
			case "AllowApplyStyle":
			case "AllowApplyBrushNegative":
			case "AllowApplyColorNegative":
			case "AllowApplyBorderColor":
			case "AllowApplyBrush":
			case "Summary":
			case "ComponentStyle":
			case "GlobalizedName":
			case "Wrap":
				Designer.InvokeSelectedObjectsChanged();
				break;
			case "CalculationMode":
				if (((PropertyGrid)PropertyGrid).SelectedObject is IStiGaugeElement)
				{
					Designer.InvokeSelectedObjectsChanged();
				}
				break;
			case "Group":
				if (((PropertyGrid)PropertyGrid).SelectedObject is IStiGroupElement)
				{
					StiCacheCleaner.Clean(Designer.Report);
				}
				break;
			case "CrossFiltering":
				if (((PropertyGrid)PropertyGrid).SelectedObject is IStiCrossFiltering)
				{
					StiCacheCleaner.Clean(Designer.Report);
				}
				break;
			case "SegmentPerWidth":
			case "SegmentPerHeight":
			case "LargeHeight":
			case "LargeHeightFactor":
			case "Icon":
				Designer.Builder.BuildReport();
				break;
			case "Name":
			case "ReportName":
			{
				string text = (string)e.ChangedItem.Value;
				if (((PropertyGrid)PropertyGrid).SelectedObject == Report)
				{
					text = StiNameValidator.CorrectName(text, Designer.Report);
					string classKeyword = ((StiCodeGenerator)(object)Report.Language.GetProvider().CreateGenerator()).GetClassKeyword();
					Designer.Report.Script = StiLexer.ReplaceWithPrefix(Designer.Report.Script, classKeyword, (string)e.OldValue, text);
					Designer.Report.Script = StiLexer.ReplaceWithPrefix(Designer.Report.Script, "public", (string)e.OldValue, text);
				}
				else if (((PropertyGrid)PropertyGrid).SelectedObject is StiComponent)
				{
					StiComponent comp = ((PropertyGrid)PropertyGrid).SelectedObject as StiComponent;
					Report.GlobalizationStrings.RenameComponent(comp, e.OldValue as string, text);
				}
				break;
			}
			case "ScriptLanguage":
				Designer.Report.ScriptNew();
				break;
			case "Border":
				Designer.InvokeSelectedObjectsChanged();
				break;
			}
			if (((PropertyGrid)PropertyGrid).SelectedObject is StiReport || ((PropertyGrid)PropertyGrid).SelectedObject is StiComponent || ((PropertyGrid)PropertyGrid).SelectedObject is IStiMeter)
			{
				((Control)Designer).Refresh();
			}
			if (((PropertyGrid)PropertyGrid).SelectedObject is IStiMeterRules)
			{
				(((PropertyGrid)PropertyGrid).SelectedObject as IStiMeterRules).CheckRules();
			}
		}
		Report.Designer.TextAliasesHash.Clear();
	}

	private void PropertyGrid_SelectedObjectsChanged(object s, EventArgs e)
	{
		RefreshLock = true;
		try
		{
			Designer.SelectedObjects = ((PropertyGrid)PropertyGrid).SelectedObjects;
			Designer.InvokeSelectedObjectsChanged();
		}
		catch
		{
		}
		RefreshLock = false;
	}

	private void User_DesignerSpecificationChanged(object sender, EventArgs e)
	{
		if (Designer != null)
		{
			object[] selectedObjects = Designer.SelectedObjects;
			Designer.SelectedObject = null;
			Designer.SelectedObjects = selectedObjects;
			Designer.InvokeSelectedObjectsChanged();
			if (panelConfiguration != null)
			{
				((Control)panelConfiguration).Visible = false;
			}
		}
	}

	private void ItemBasic_Click(object sender, EventArgs e)
	{
		if (StiAccountSettings.CurrentAccount?.User != null)
		{
			StiAccountSettings.CurrentAccount.User.DesignerSpecification = StiDesignerSpecification.Beginner;
			StiAccountSettings.CurrentAccount.User.SaveToFile();
		}
		else
		{
			StiOptions.Designer.PropertyGrid.PropertyLevel = StiLevel.Basic;
		}
		object[] selectedObjects = Designer.SelectedObjects;
		Designer.SelectedObject = null;
		Designer.SelectedObjects = selectedObjects;
		Designer.InvokeSelectedObjectsChanged();
		if (panelConfiguration != null)
		{
			((Control)panelConfiguration).Visible = false;
		}
	}

	private void ItemStandard_Click(object sender, EventArgs e)
	{
		if (StiAccountSettings.CurrentAccount?.User != null)
		{
			StiAccountSettings.CurrentAccount.User.DesignerSpecification = StiDesignerSpecification.BICreator;
			StiAccountSettings.CurrentAccount.User.SaveToFile();
		}
		else
		{
			StiOptions.Designer.PropertyGrid.PropertyLevel = StiLevel.Standard;
		}
		object[] selectedObjects = Designer.SelectedObjects;
		Designer.SelectedObject = null;
		Designer.SelectedObjects = selectedObjects;
		Designer.InvokeSelectedObjectsChanged();
		if (panelConfiguration != null)
		{
			((Control)panelConfiguration).Visible = false;
		}
	}

	private void ItemProfessional_Click(object sender, EventArgs e)
	{
		if (StiAccountSettings.CurrentAccount?.User != null)
		{
			StiAccountSettings.CurrentAccount.User.DesignerSpecification = StiDesignerSpecification.Developer;
			StiAccountSettings.CurrentAccount.User.SaveToFile();
		}
		else
		{
			StiOptions.Designer.PropertyGrid.PropertyLevel = StiLevel.Professional;
		}
		object[] selectedObjects = Designer.SelectedObjects;
		Designer.SelectedObject = null;
		Designer.SelectedObjects = selectedObjects;
		Designer.InvokeSelectedObjectsChanged();
		if (panelConfiguration != null)
		{
			((Control)panelConfiguration).Visible = false;
		}
	}

	private void ItemSettings_DropDownOpening(object sender, EventArgs e)
	{
		itemLocalizePropertyGrid.Checked = StiOptions.Designer.PropertyGrid.Localizable;
		itemShowDescription.Checked = StiOptions.Designer.PropertyGrid.ShowDescription;
		itemBasic.Checked = StiOptions.Designer.PropertyGrid.PropertyLevel == StiLevel.Basic;
		itemStandard.Checked = StiOptions.Designer.PropertyGrid.PropertyLevel == StiLevel.Standard;
		itemProfessional.Checked = StiOptions.Designer.PropertyGrid.PropertyLevel == StiLevel.Professional;
	}

	public override void Localize()
	{
		base.Localize();
		object[] selectedObjects = ((PropertyGrid)PropertyGrid).SelectedObjects;
		((PropertyGrid)PropertyGrid).SelectedObjects = null;
		((PropertyGrid)PropertyGrid).SelectedObjects = selectedObjects;
		if (PropertyGrid != null)
		{
			PropertyGrid.Localize();
		}
		if (((Control)comboBoxComponents).Enabled)
		{
			RefreshComponent();
		}
		if (itemSortItems != null)
		{
			Graphics globalMeasureGraphics = StiReport.GlobalMeasureGraphics;
			Designer.SetToolTipOnlyHeaderEx(itemSortItems, globalMeasureGraphics, StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "SortItems")), "user-manual/reports_designer_panels_properties.htm");
		}
		((ToolStripItem)itemLocalizePropertyGrid).Text = StiLocalization.Get("FormDesigner", "LocalizePropertyGrid");
		((ToolStripItem)itemShowDescription).Text = StiLocalization.Get("FormDesigner", "ShowDescription");
		((ToolStripItem)itemBasic).Text = StiLocalization.Get("Report", "Basic");
		((ToolStripItem)itemStandard).Text = StiLocalization.Get("Report", "Standard");
		((ToolStripItem)itemProfessional).Text = StiLocalization.Get("Report", "Professional");
		if (itemSettings != null)
		{
			((ToolStripItem)itemSettings).ToolTipText = StiHint.CreateHint(StiLocalization.Get("Export", "Settings"));
		}
	}

	public void UpdateComponent()
	{
		object[] selectedObjects = ((PropertyGrid)PropertyGrid).SelectedObjects;
		((PropertyGrid)PropertyGrid).SelectedObjects = null;
		((PropertyGrid)PropertyGrid).SelectedObjects = selectedObjects;
	}

	private string GetName(object value)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		if (value is StiComponent stiComponent)
		{
			text = stiComponent.Name;
			text2 = stiComponent.LocalizedName;
		}
		if (value is StiReport stiReport)
		{
			text = stiReport.ReportName;
			text2 = StiLocalization.Get("Components", "StiReport");
		}
		return text + " : " + text2;
	}

	public void RefreshComponent()
	{
		if (Report != null && Designer.ComponentsOnReport != null)
		{
			try
			{
				if (!lockRefreshComponents)
				{
					lockRefreshComponents = true;
					components = new Hashtable();
					List<object> list = new List<object>();
					((ComboBox)comboBoxComponents).Items.Clear();
					components[GetName(Designer.Report)] = Designer.Report;
					list.Add(Designer.Report);
					foreach (StiComponent item2 in Designer.ComponentsOnReport)
					{
						if (!(item2 is IStiHideFromReportTree))
						{
							components[GetName(item2)] = item2;
							list.Add(item2);
						}
					}
					if (itemSortItems.Checked)
					{
						list = list.OrderBy((object x) => (!(x is StiComponent)) ? ((StiReport)x).ReportName : ((StiComponent)x).Name).ToList();
					}
					foreach (object item3 in list)
					{
						((ComboBox)comboBoxComponents).Items.Add((object)GetName(item3));
					}
					lockComponents = true;
					object[] selected = Report.GetSelected();
					if (selected.Length > 1)
					{
						((ListControl)comboBoxComponents).SelectedIndex = -1;
					}
					else
					{
						object item = selected.FirstOrDefault((object c) => c is IStiSelect && ((IStiSelect)c).IsSelected);
						((ListControl)comboBoxComponents).SelectedIndex = list.IndexOf(item);
					}
					lockComponents = false;
					((Control)comboBoxComponents).Invalidate();
					lockRefreshComponents = false;
				}
			}
			catch
			{
			}
			((Control)comboBoxComponents).Enabled = true;
			bool flag = true;
			object[] selectedObjects = Designer.SelectedObjects;
			foreach (object obj2 in selectedObjects)
			{
				if (obj2 is StiComponent comp && !StiRestrictionsHelper.IsAllowChange(comp))
				{
					flag = false;
				}
			}
			if (((Control)PropertyGrid).Enabled != flag)
			{
				((Control)PropertyGrid).Enabled = flag;
			}
		}
		else
		{
			StiComboBox stiComboBox = comboBoxComponents;
			bool enabled = (((Control)PropertyGrid).Enabled = Report != null);
			((Control)stiComboBox).Enabled = enabled;
		}
	}

	private bool CheckRestrictions(PropertyValueChangedEventArgs e)
	{
		if (((PropertyGrid)PropertyGrid).SelectedObjects == null || ((PropertyGrid)PropertyGrid).SelectedObjects.Length != 1)
		{
			return false;
		}
		switch (e.ChangedItem.PropertyDescriptor.Name)
		{
		case "Width":
			if (((PropertyGrid)PropertyGrid).SelectedObject is StiComponent stiComponent3 && (stiComponent3.Restrictions & StiRestrictions.AllowResize) == 0)
			{
				stiComponent3.Width = (double)e.OldValue;
				return true;
			}
			break;
		case "Height":
			if (((PropertyGrid)PropertyGrid).SelectedObject is StiComponent stiComponent2 && (stiComponent2.Restrictions & StiRestrictions.AllowResize) == 0)
			{
				stiComponent2.Height = (double)e.OldValue;
				return true;
			}
			break;
		case "Left":
			if (((PropertyGrid)PropertyGrid).SelectedObject is StiComponent stiComponent4 && (stiComponent4.Restrictions & StiRestrictions.AllowMove) == 0)
			{
				stiComponent4.Left = (double)e.OldValue;
				return true;
			}
			break;
		case "Top":
			if (((PropertyGrid)PropertyGrid).SelectedObject is StiComponent stiComponent && (stiComponent.Restrictions & StiRestrictions.AllowMove) == 0)
			{
				stiComponent.Top = (double)e.OldValue;
				return true;
			}
			break;
		}
		return false;
	}

	private void SetSuperToolTip(Button item, string header, string bodyText, Graphics g)
	{
		SizeF sizeF = Designer.SuperToolTip.MeasureHeaderText(g, header);
		SizeF sizeF2 = Designer.SuperToolTip.MeasureBodyText(g, bodyText, StiScale.XXI(200.0));
		int width = ((sizeF.Width > sizeF2.Width) ? ((int)Math.Round((double)sizeF.Width + 0.5) + StiScale.XXI(15.0)) : ((int)Math.Round((double)sizeF2.Width + 0.5) + StiScale.XXI(25.0)));
		SuperTooltipInfo info = new SuperTooltipInfo(header, null, bodyText, null, null, Designer.CurrentTooltipColor)
		{
			FooterVisible = false,
			CustomSize = new Size(width, (int)Math.Round(sizeF2.Height + sizeF.Height) + StiScale.YYI(15.0))
		};
		Designer.SuperToolTip.SetSuperTooltip((IComponent)item, info);
	}

	public static StiPropertiesPanelService GetService()
	{
		return GetService(null);
	}

	public static StiPropertiesPanelService GetService(StiDesignerControl designer)
	{
		if (designer == null)
		{
			return StiDesignerOptions.Panels.FirstOrDefault((StiPanelService c) => c.GetType() == typeof(StiPropertiesPanelService)) as StiPropertiesPanelService;
		}
		return designer.Services.GetServices(typeof(StiPanelService)).OfType<StiPropertiesPanelService>().FirstOrDefault();
	}

	public override void ApplyStyle()
	{
		StiGuiHelper.ApplyStyle((Control)(object)base.DockingContainer);
		((Control)base.DockingContainer).BackColor = StiGuiHelper.GetControlColor();
	}

	public override void Init(StiDesignerControl designer)
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Expected O, but got Unknown
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_044f: Expected O, but got Unknown
		//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Expected O, but got Unknown
		//IL_050d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0517: Expected O, but got Unknown
		//IL_0524: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Expected O, but got Unknown
		//IL_053b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0545: Expected O, but got Unknown
		//IL_0552: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Expected O, but got Unknown
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Expected O, but got Unknown
		//IL_06a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06df: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0714: Expected O, but got Unknown
		//IL_0731: Unknown result type (might be due to invalid IL or missing references)
		//IL_0736: Unknown result type (might be due to invalid IL or missing references)
		//IL_073d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0751: Unknown result type (might be due to invalid IL or missing references)
		//IL_075c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0773: Expected O, but got Unknown
		//IL_0784: Unknown result type (might be due to invalid IL or missing references)
		//IL_0789: Unknown result type (might be due to invalid IL or missing references)
		//IL_079d: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b5: Expected O, but got Unknown
		//IL_07f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_080d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0815: Unknown result type (might be due to invalid IL or missing references)
		//IL_081c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0825: Expected O, but got Unknown
		//IL_0864: Unknown result type (might be due to invalid IL or missing references)
		//IL_0869: Unknown result type (might be due to invalid IL or missing references)
		//IL_087d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0885: Unknown result type (might be due to invalid IL or missing references)
		//IL_088c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0895: Expected O, but got Unknown
		//IL_08d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08da: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f7: Expected O, but got Unknown
		//IL_091b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0920: Unknown result type (might be due to invalid IL or missing references)
		//IL_0927: Unknown result type (might be due to invalid IL or missing references)
		//IL_093d: Expected O, but got Unknown
		//IL_0961: Unknown result type (might be due to invalid IL or missing references)
		//IL_0966: Unknown result type (might be due to invalid IL or missing references)
		//IL_096d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0983: Expected O, but got Unknown
		if (StiAccountSettings.CurrentAccount != null)
		{
			StiAccountSettings.CurrentAccount.DesignerSpecificationChanged += User_DesignerSpecificationChanged;
		}
		base.Init(designer);
		StiReportPropertyGrid stiReportPropertyGrid = new StiReportPropertyGrid();
		((PropertyGrid)stiReportPropertyGrid).CommandsVisibleIfAvailable = true;
		((Control)stiReportPropertyGrid).Font = StiOptions.Designer.DefaultPropertyGridFont;
		((PropertyGrid)stiReportPropertyGrid).LargeButtons = false;
		((Control)stiReportPropertyGrid).Dock = (DockStyle)5;
		PropertyGrid = stiReportPropertyGrid;
		PropertyGrid.SetStandardPropertyTabs();
		Bitmap image = ((StiScale.Id == Stimulsoft.Base.SystemScaleID.x1) ? ((Bitmap)null) : new Bitmap(StiScale.XXI(16.0), StiScale.XXI(16.0)));
		StiScaledToolStripMenuItem stiScaledToolStripMenuItem = new StiScaledToolStripMenuItem();
		((ToolStripItem)stiScaledToolStripMenuItem).Name = "itemLocalizePropertyGrid";
		((ToolStripItem)stiScaledToolStripMenuItem).Size = new Size(339, 38);
		((ToolStripItem)stiScaledToolStripMenuItem).Text = "Localize PropertyGrid";
		((ToolStripItem)stiScaledToolStripMenuItem).ImageScaling = (ToolStripItemImageScaling)0;
		((ToolStripItem)stiScaledToolStripMenuItem).Image = (Image)(object)image;
		itemLocalizePropertyGrid = (ToolStripMenuItem)(object)stiScaledToolStripMenuItem;
		((ToolStripItem)itemLocalizePropertyGrid).Click += PropertyGrid.ItemLocalization_Click;
		StiScaledToolStripMenuItem stiScaledToolStripMenuItem2 = new StiScaledToolStripMenuItem();
		((ToolStripItem)stiScaledToolStripMenuItem2).Name = "itemShowDescription";
		((ToolStripItem)stiScaledToolStripMenuItem2).Size = new Size(339, 38);
		((ToolStripItem)stiScaledToolStripMenuItem2).Text = "Show Description";
		((ToolStripItem)stiScaledToolStripMenuItem2).ImageScaling = (ToolStripItemImageScaling)0;
		((ToolStripItem)stiScaledToolStripMenuItem2).Image = (Image)(object)image;
		itemShowDescription = (ToolStripMenuItem)(object)stiScaledToolStripMenuItem2;
		((ToolStripItem)itemShowDescription).Click += PropertyGrid.ItemShowDescription_Click;
		itemSeparator = new ToolStripSeparator
		{
			Name = "itemSeparator",
			Size = new Size(336, 6),
			ImageScaling = (ToolStripItemImageScaling)0,
			Image = (Image)(object)image
		};
		StiScaledToolStripMenuItem stiScaledToolStripMenuItem3 = new StiScaledToolStripMenuItem();
		((ToolStripItem)stiScaledToolStripMenuItem3).Name = "itemBasic";
		((ToolStripItem)stiScaledToolStripMenuItem3).Size = new Size(339, 38);
		((ToolStripItem)stiScaledToolStripMenuItem3).Text = "Basic";
		((ToolStripItem)stiScaledToolStripMenuItem3).ImageScaling = (ToolStripItemImageScaling)0;
		((ToolStripItem)stiScaledToolStripMenuItem3).Image = (Image)(object)image;
		itemBasic = (ToolStripMenuItem)(object)stiScaledToolStripMenuItem3;
		((ToolStripItem)itemBasic).Click += ItemBasic_Click;
		StiScaledToolStripMenuItem stiScaledToolStripMenuItem4 = new StiScaledToolStripMenuItem();
		((ToolStripItem)stiScaledToolStripMenuItem4).Name = "itemStandard";
		((ToolStripItem)stiScaledToolStripMenuItem4).Size = new Size(339, 38);
		((ToolStripItem)stiScaledToolStripMenuItem4).Text = "Standard";
		((ToolStripItem)stiScaledToolStripMenuItem4).ImageScaling = (ToolStripItemImageScaling)0;
		((ToolStripItem)stiScaledToolStripMenuItem4).Image = (Image)(object)image;
		itemStandard = (ToolStripMenuItem)(object)stiScaledToolStripMenuItem4;
		((ToolStripItem)itemStandard).Click += ItemStandard_Click;
		StiScaledToolStripMenuItem stiScaledToolStripMenuItem5 = new StiScaledToolStripMenuItem();
		((ToolStripItem)stiScaledToolStripMenuItem5).Name = "itemPro";
		((ToolStripItem)stiScaledToolStripMenuItem5).Size = new Size(339, 38);
		((ToolStripItem)stiScaledToolStripMenuItem5).Text = "Pro";
		((ToolStripItem)stiScaledToolStripMenuItem5).ImageScaling = (ToolStripItemImageScaling)0;
		((ToolStripItem)stiScaledToolStripMenuItem5).Image = (Image)(object)image;
		itemProfessional = (ToolStripMenuItem)(object)stiScaledToolStripMenuItem5;
		((ToolStripItem)itemProfessional).Click += ItemProfessional_Click;
		StiToolStripDropDownButton stiToolStripDropDownButton = new StiToolStripDropDownButton();
		((ToolStripItem)stiToolStripDropDownButton).DisplayStyle = (ToolStripItemDisplayStyle)3;
		((ToolStripItem)stiToolStripDropDownButton).Image = StiReportDesignImages.Actions.Settings();
		((ToolStripItem)stiToolStripDropDownButton).ImageTransparentColor = Color.Magenta;
		((ToolStripItem)stiToolStripDropDownButton).Name = "itemSettings";
		((ToolStripItem)stiToolStripDropDownButton).Text = "  ";
		itemSettings = (ToolStripDropDownButton)(object)stiToolStripDropDownButton;
		((ToolStripDropDownItem)itemSettings).DropDownItems.AddRange((ToolStripItem[])(object)new ToolStripItem[6]
		{
			(ToolStripItem)itemLocalizePropertyGrid,
			(ToolStripItem)itemShowDescription,
			(ToolStripItem)itemSeparator,
			(ToolStripItem)itemBasic,
			(ToolStripItem)itemStandard,
			(ToolStripItem)itemProfessional
		});
		((ToolStripDropDownItem)itemSettings).DropDownOpening += ItemSettings_DropDownOpening;
		ToolStrip val = null;
		foreach (Control item in (ArrangedElementCollection)((PropertyGrid)PropertyGrid).Controls)
		{
			Control val2 = item;
			val = (ToolStrip)(object)((val2 is ToolStrip) ? val2 : null);
			if (val != null)
			{
				break;
			}
		}
		if (val != null)
		{
			if (((ArrangedElementCollection)val.Items).Count > 5 && val.Items[((ArrangedElementCollection)val.Items).Count - 1] is ToolStripButton && val.Items[((ArrangedElementCollection)val.Items).Count - 2] is ToolStripSeparator)
			{
				val.Items.RemoveAt(((ArrangedElementCollection)val.Items).Count - 1);
				val.Items.RemoveAt(((ArrangedElementCollection)val.Items).Count - 1);
			}
			val.Items.Add((ToolStripItem)new ToolStripSeparator());
			val.Items.Add((ToolStripItem)(object)itemSettings);
			val.Renderer = (ToolStripRenderer)(object)new StiScaledToolStripRenderer();
		}
		((Control)PropertyGrid).Enter += PropertyGrid_Enter;
		((PropertyGrid)PropertyGrid).PropertyValueChanged += new PropertyValueChangedEventHandler(PropertyGrid_PropertyValueChanged);
		((PropertyGrid)PropertyGrid).SelectedObjectsChanged += PropertyGrid_SelectedObjectsChanged;
		ContextMenuBar contextMenuBar = PropertyGrid.ContextMenuBar;
		if (((Control)designer).Controls.Contains((Control)(object)contextMenuBar))
		{
			((Control)designer).Controls.Remove((Control)(object)contextMenuBar);
		}
		((Control)designer).Controls.Add((Control)(object)contextMenuBar);
		Designer.RefreshDesigner += Designer_RefreshDesigner;
		Designer.SelectedObjectsChanged += Designer_SelectedObjectsChanged;
		base.DockingItem.Image = StiReportDesignImages.Actions.Properties();
		TableLayoutPanel val3 = new TableLayoutPanel
		{
			AutoSize = true,
			AutoSizeMode = (AutoSizeMode)0,
			Dock = (DockStyle)1,
			ColumnCount = 2,
			RowCount = 2
		};
		val3.ColumnStyles.Add(new ColumnStyle((SizeType)2, 100f));
		val3.ColumnStyles.Add(new ColumnStyle((SizeType)0, 0f));
		val3.RowStyles.Add(new RowStyle((SizeType)0, 0f));
		val3.RowStyles.Add(new RowStyle((SizeType)1, 3f));
		StiComboBox obj = new StiComboBox
		{
			DropDownStyle = (ComboBoxStyle)2
		};
		((Control)obj).Dock = (DockStyle)1;
		((ComboBox)obj).MaxDropDownItems = 20;
		comboBoxComponents = obj;
		((ComboBox)comboBoxComponents).SelectedIndexChanged += ComboBoxComponents_SelectedIndexChanged;
		ContextMenuBar contextMenuBar2 = new ContextMenuBar
		{
			Style = eDotNetBarStyle.StyleManagerControlled
		};
		ButtonItem buttonItem = new ButtonItem
		{
			AutoExpandOnClick = true
		};
		contextMenuBar2.Items.Add(buttonItem);
		itemSortItems = new ButtonItem
		{
			AutoCheckOnClick = true,
			Text = StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "SortItems")),
			Image = StiImageResizer.GetImage((Image)(object)PanelDictionaryOffice2013Resource.SortAZ)
		};
		itemSortItems.Click += delegate
		{
			RefreshComponent();
			StiSettings.Load();
			StiSettings.Set("Designer", "PropertiesSortItems", itemSortItems.Checked);
			StiSettings.Save();
		};
		buttonItem.SubItems.Add(itemSortItems);
		contextMenuBar2.SetContextMenuEx((Control)(object)comboBoxComponents, buttonItem);
		StiSettings.Load();
		itemSortItems.Checked = StiSettings.GetBool("Designer", "PropertiesSortItems", nullValue: false);
		((Control)base.DockingContainer).Controls.Add((Control)(object)PropertyGrid);
		((Control)base.DockingContainer).Controls.Add((Control)(object)comboBoxComponents);
		if (!StiOptions.Designer.PropertyGrid.PropertyLevel.HasValue)
		{
			panelConfiguration = new Panel
			{
				Padding = new Padding(StiScale.I5, 0, StiScale.I5, 0),
				BackColor = Color.White,
				Margin = new Padding(StiScale.I10),
				Anchor = (AnchorStyles)15,
				Left = 0,
				Top = 0,
				Width = ((Control)base.DockingContainer).Width,
				Height = ((Control)base.DockingContainer).Height
			};
			((Control)base.DockingContainer).Controls.Add((Control)(object)panelConfiguration);
			Graphics globalMeasureGraphics = StiReport.GlobalMeasureGraphics;
			Label val4 = new Label
			{
				Dock = (DockStyle)1,
				Height = StiScale.YYI(100.0),
				TextAlign = (ContentAlignment)256,
				Text = StiLocalization.Get("Questions", "qnConfiguration")
			};
			string text = StiLocalization.Get("Report", "Basic");
			Button val5 = new Button
			{
				Height = StiScale.YYI(32.0),
				Text = text,
				Dock = (DockStyle)1,
				UseVisualStyleBackColor = true
			};
			((Control)val5).Click += ItemBasic_Click;
			SetSuperToolTip(val5, text, StiLocalization.Get("Report", "BasicConfiguration"), globalMeasureGraphics);
			text = StiLocalization.Get("Report", "Standard");
			Button val6 = new Button
			{
				Height = StiScale.YYI(32.0),
				Text = text,
				Dock = (DockStyle)1,
				UseVisualStyleBackColor = true
			};
			((Control)val6).Click += ItemStandard_Click;
			SetSuperToolTip(val6, text, StiLocalization.Get("Report", "StandardConfiguration"), globalMeasureGraphics);
			text = StiLocalization.Get("Report", "Professional");
			Button val7 = new Button
			{
				Height = StiScale.YYI(32.0),
				Text = text,
				Dock = (DockStyle)1,
				UseVisualStyleBackColor = true
			};
			((Control)val7).Click += ItemProfessional_Click;
			SetSuperToolTip(val7, text, StiLocalization.Get("Report", "ProfessionalConfiguration"), globalMeasureGraphics);
			((Control)panelConfiguration).Controls.Add((Control)(object)val7);
			Panel val8 = new Panel
			{
				Dock = (DockStyle)1,
				Height = StiScale.YYI(10.0)
			};
			((Control)panelConfiguration).Controls.Add((Control)(object)val8);
			((Control)panelConfiguration).Controls.Add((Control)(object)val6);
			val8 = new Panel
			{
				Dock = (DockStyle)1,
				Height = StiScale.YYI(10.0)
			};
			((Control)panelConfiguration).Controls.Add((Control)(object)val8);
			((Control)panelConfiguration).Controls.Add((Control)(object)val5);
			val8 = new Panel
			{
				Dock = (DockStyle)1,
				Height = StiScale.YYI(10.0)
			};
			((Control)panelConfiguration).Controls.Add((Control)(object)val8);
			((Control)panelConfiguration).Controls.Add((Control)(object)val4);
			((Control)panelConfiguration).BringToFront();
		}
	}

	public override void Dispose()
	{
		base.Dispose();
		if (PropertyGrid != null)
		{
			RefreshLock = true;
			propertyValueChangedLock = true;
			PropertyGrid = null;
		}
		DisposeComponent((Component)(object)comboBoxComponents);
		comboBoxComponents = null;
		if (components != null)
		{
			components.Clear();
			components = null;
		}
	}
}

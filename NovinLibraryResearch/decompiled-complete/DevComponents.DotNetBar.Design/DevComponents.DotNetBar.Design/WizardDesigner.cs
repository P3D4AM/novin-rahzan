using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.Win32;

namespace DevComponents.DotNetBar.Design;

public class WizardDesigner : ControlDesigner
{
	private enum eWizardImages
	{
		WelcomeDefault,
		BackgroundOffice2007
	}

	private WizardPageNavigationControl m_NavigationControl;

	private eWizardStyle m_WizardStyle;

	public override DesignerVerbCollection Verbs
	{
		get
		{
			DesignerVerb[] array = null;
			array = new DesignerVerb[7]
			{
				new DesignerVerb("Create Inner Page", CreatePage),
				new DesignerVerb("Create Welcome Page", CreateWelcomePage),
				new DesignerVerb("Create Outer Page", CreateOuterPage),
				new DesignerVerb("Delete Page", DeletePage),
				new DesignerVerb("Next Page", NextPage),
				new DesignerVerb("Previous Page", PreviousPage),
				new DesignerVerb("Goto Page/Change Order", GotoPage)
			};
			return new DesignerVerbCollection(array);
		}
	}

	public eWizardStyle WizardStyle
	{
		get
		{
			return m_WizardStyle;
		}
		set
		{
			if (m_WizardStyle != value)
			{
				m_WizardStyle = value;
				((Wizard)(object)((ControlDesigner)this).Control).ButtonStyle = m_WizardStyle;
				if (((ComponentDesigner)this).GetService(typeof(IDesignerHost)) is IDesignerHost { Loading: false })
				{
					ChangeWizardStyle(m_WizardStyle);
				}
			}
		}
	}

	public override void Initialize(IComponent component)
	{
		((ControlDesigner)this).Initialize(component);
		if (component.Site.DesignMode)
		{
			IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentRemoved += OnComponentRemoved;
			}
			if (component is Wizard wizard)
			{
				WizardPageNavigationControl wizardPageNavigationControl = new WizardPageNavigationControl();
				((Control)wizardPageNavigationControl).Location = new Point(4, ((Control)wizard.panelFooter).Height - ((Control)wizardPageNavigationControl).Height - 4);
				((Control)wizard.panelFooter).Controls.Add((Control)(object)wizardPageNavigationControl);
				((Control)wizardPageNavigationControl).BackColor = SystemColors.Control;
				m_NavigationControl = wizardPageNavigationControl;
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
		if (componentChangeService != null)
		{
			componentChangeService.ComponentRemoved -= OnComponentRemoved;
		}
		((ControlDesigner)this).Dispose(disposing);
	}

	private void OnComponentRemoved(object sender, ComponentEventArgs e)
	{
		if (e.Component is WizardPage)
		{
			WizardPage wizardPage = e.Component as WizardPage;
			Wizard wizard = ((ControlDesigner)this).Control as Wizard;
			if (wizardPage != null && wizard.WizardPages.Contains(wizardPage))
			{
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				componentChangeService?.OnComponentChanging(wizard, TypeDescriptor.GetProperties(wizard)["WizardPages"]);
				wizard.WizardPages.Remove(wizardPage);
				componentChangeService?.OnComponentChanged(wizard, TypeDescriptor.GetProperties(wizard)["WizardPages"], null, null);
			}
		}
	}

	public override void DoDefaultAction()
	{
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ControlDesigner)this).InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	private void SetDesignTimeDefaults()
	{
		if (((ControlDesigner)this).Control is Wizard wizard)
		{
			TypeDescriptor.GetProperties(wizard)["HeaderImageVisible"].SetValue(wizard, true);
			TypeDescriptor.GetProperties(wizard)["Dock"].SetValue(wizard, (object)(DockStyle)5);
			SetDefaultFooterStyle(wizard);
		}
	}

	private void GotoPage(object sender, EventArgs e)
	{
		Wizard w = ((ControlDesigner)this).Control as Wizard;
		IComponentChangeService cc = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		ISelectionService ss = ((ComponentDesigner)this).GetService(typeof(ISelectionService)) as ISelectionService;
		GotoPage(w, cc, ss);
	}

	internal static void GotoPage(Wizard w, IComponentChangeService cc, ISelectionService ss)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		if (w == null)
		{
			return;
		}
		WizardPageOrderDialog wizardPageOrderDialog = new WizardPageOrderDialog();
		wizardPageOrderDialog.SetWizard(w);
		((Form)wizardPageOrderDialog).StartPosition = (FormStartPosition)1;
		if ((int)((Form)wizardPageOrderDialog).ShowDialog() == 1)
		{
			string selectedPageName = wizardPageOrderDialog.SelectedPageName;
			if (wizardPageOrderDialog.OrderChanged)
			{
				cc?.OnComponentChanging(w, TypeDescriptor.GetProperties(w)["WizardPages"]);
				string[] orderedPageNames = wizardPageOrderDialog.OrderedPageNames;
				w.WizardPages.IgnoreEvents = true;
				try
				{
					WizardPageCollection wizardPageCollection = new WizardPageCollection();
					w.WizardPages.CopyTo(wizardPageCollection);
					w.WizardPages.Clear();
					string[] array = orderedPageNames;
					foreach (string name in array)
					{
						w.WizardPages.Add(wizardPageCollection[name]);
					}
				}
				finally
				{
					w.WizardPages.IgnoreEvents = false;
				}
				cc?.OnComponentChanged(w, TypeDescriptor.GetProperties(w)["WizardPages"], null, null);
			}
			if (selectedPageName != "")
			{
				w.SelectedPage = w.WizardPages[selectedPageName];
			}
			else if (wizardPageOrderDialog.OrderChanged)
			{
				w.SelectedPageIndex = 0;
			}
			if (ss != null && (selectedPageName != "" || wizardPageOrderDialog.OrderChanged))
			{
				ss.SetSelectedComponents(new WizardPage[1] { w.SelectedPage }, SelectionTypes.Replace);
			}
		}
		((Component)(object)wizardPageOrderDialog).Dispose();
	}

	private void CreatePage(object sender, EventArgs e)
	{
		IDesignerHost dh = ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) as IDesignerHost;
		IComponentChangeService cc = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		ISelectionService ss = ((ComponentDesigner)this).GetService(typeof(ISelectionService)) as ISelectionService;
		CreatePage(((ControlDesigner)this).Control as Wizard, innerPage: true, dh, cc, ss, m_WizardStyle);
	}

	private void CreateWelcomePage(object sender, EventArgs e)
	{
		IDesignerHost dh = ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) as IDesignerHost;
		IComponentChangeService cc = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		ISelectionService ss = ((ComponentDesigner)this).GetService(typeof(ISelectionService)) as ISelectionService;
		CreateWelcomePage(((ControlDesigner)this).Control as Wizard, dh, cc, ss, WizardStyle);
	}

	private void CreateOuterPage(object sender, EventArgs e)
	{
		IDesignerHost dh = ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) as IDesignerHost;
		IComponentChangeService cc = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		ISelectionService ss = ((ComponentDesigner)this).GetService(typeof(ISelectionService)) as ISelectionService;
		CreatePage(((ControlDesigner)this).Control as Wizard, innerPage: false, dh, cc, ss, m_WizardStyle);
	}

	private void DeletePage(object sender, EventArgs e)
	{
		IDesignerHost dh = ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) as IDesignerHost;
		IComponentChangeService cc = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		if (((ControlDesigner)this).Control is Wizard wizard)
		{
			DeletePage(wizard.SelectedPage, dh, cc);
		}
	}

	internal static void DeletePage(WizardPage page, IDesignerHost dh, IComponentChangeService cc)
	{
		if (page == null || !(((Control)page).Parent is Wizard))
		{
			return;
		}
		Wizard wizard = ((Control)page).Parent as Wizard;
		DesignerTransaction designerTransaction = dh.CreateTransaction();
		try
		{
			cc?.OnComponentChanging(wizard, TypeDescriptor.GetProperties(wizard)["WizardPages"]);
			wizard.WizardPages.Remove(page);
			cc?.OnComponentChanged(wizard, TypeDescriptor.GetProperties(wizard)["WizardPages"], null, null);
			dh.DestroyComponent((IComponent)page);
		}
		catch
		{
			designerTransaction.Cancel();
		}
		finally
		{
			if (!designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
		}
	}

	private void NextPage(object sender, EventArgs e)
	{
		SelectNextPage(((ControlDesigner)this).Control as Wizard);
	}

	private void PreviousPage(object sender, EventArgs e)
	{
		SelectPreviousPage(((ControlDesigner)this).Control as Wizard);
	}

	internal static bool SelectNextPage(Wizard w)
	{
		if (w == null)
		{
			return false;
		}
		WizardPage nextPage = w.GetNextPage();
		if (nextPage != null)
		{
			w.SelectedPage = nextPage;
			return true;
		}
		return false;
	}

	internal static bool SelectPreviousPage(Wizard w)
	{
		if (w == null)
		{
			return false;
		}
		WizardPage backPage = w.GetBackPage();
		if (backPage != null)
		{
			w.SelectedPage = backPage;
			return true;
		}
		return false;
	}

	private static Image LoadWizardImage(eWizardImages wizardImage)
	{
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		string text = "WizardWelcomeImage.png";
		if (wizardImage == eWizardImages.BackgroundOffice2007)
		{
			text = "WizardOffice2007Background.png";
		}
		try
		{
			RegistryKey val = Registry.LocalMachine;
			string text2 = "";
			try
			{
				if (val != null)
				{
					val = val.OpenSubKey("Software\\DevComponents\\DotNetBar");
				}
				if (val != null)
				{
					text2 = val.GetValue("InstallationFolder", (object)"").ToString();
				}
			}
			finally
			{
				if (val != null)
				{
					val.Close();
				}
			}
			if (text2 != "")
			{
				if (text2.Substring(text2.Length - 1, 1) != "\\")
				{
					text2 += "\\";
				}
				text2 = ((!File.Exists(text2 + text)) ? "" : (text2 + text));
			}
			if (text2 != "")
			{
				return (Image)new Bitmap(text2);
			}
		}
		catch (Exception)
		{
		}
		return null;
	}

	internal static void ApplyOffice2007WelcomePageStyle(WizardPage page, IDesignerHost dh, IComponentChangeService cc)
	{
		DesignerTransaction designerTransaction = null;
		if (dh != null)
		{
			designerTransaction = dh.CreateTransaction();
		}
		try
		{
			cc?.OnComponentChanging(page, null);
			((Control)page).BackColor = Color.Transparent;
			page.Style.Reset();
			cc?.OnComponentChanged(page, null, null, null);
		}
		catch
		{
			designerTransaction.Cancel();
			throw;
		}
		finally
		{
			if (designerTransaction != null && !designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
		}
	}

	internal static void ApplyOffice2007InnerPageStyle(WizardPage page, IDesignerHost dh, IComponentChangeService cc)
	{
		cc?.OnComponentChanging(page, null);
		((Control)page).BackColor = Color.Transparent;
		page.Style.Reset();
		cc?.OnComponentChanged(page, null, null, null);
	}

	internal static void ApplyDefaultInnerPageStyle(WizardPage page, IDesignerHost dh, IComponentChangeService cc)
	{
		cc?.OnComponentChanging(page, null);
		((Control)page).BackColor = SystemColors.Control;
		if (!page.InteriorPage)
		{
			TypeDescriptor.GetProperties(page.Style)["BackColor"].SetValue(page.Style, Color.White);
		}
		cc?.OnComponentChanged(page, null, null, null);
	}

	internal static void ApplyDefaultWelcomePageStyle(WizardPage page, IDesignerHost dh, IComponentChangeService cc)
	{
		DesignerTransaction designerTransaction = null;
		if (dh != null)
		{
			designerTransaction = dh.CreateTransaction();
		}
		try
		{
			((Control)page).BackColor = Color.White;
			page.Style.BackColorBlend.Clear();
			TypeDescriptor.GetProperties(page)["InteriorPage"].SetValue(page, false);
			TypeDescriptor.GetProperties(page)["BackColor"].SetValue(page, Color.White);
			TypeDescriptor.GetProperties(page)["CanvasColor"].SetValue(page, Color.White);
			TypeDescriptor.GetProperties(page.Style)["BackColor"].SetValue(page.Style, Color.White);
			TypeDescriptor.GetProperties(page.Style)["BackColor2"].SetValue(page.Style, Color.Empty);
			TypeDescriptor.GetProperties(page.Style)["BackgroundImage"].SetValue(page.Style, LoadWizardImage(eWizardImages.WelcomeDefault));
			TypeDescriptor.GetProperties(page.Style)["BackgroundImagePosition"].SetValue(page.Style, eStyleBackgroundImage.TopLeft);
		}
		catch
		{
			designerTransaction.Cancel();
			throw;
		}
		finally
		{
			if (designerTransaction != null && !designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
		}
	}

	internal static WizardPage CreateWelcomePage(Wizard parent, IDesignerHost dh, IComponentChangeService cc, ISelectionService ss, eWizardStyle style)
	{
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		DesignerTransaction designerTransaction = dh.CreateTransaction();
		WizardPage wizardPage = null;
		try
		{
			wizardPage = dh.CreateComponent(typeof(WizardPage)) as WizardPage;
			if (style == eWizardStyle.Default)
			{
				ApplyDefaultWelcomePageStyle(wizardPage, null, null);
			}
			else
			{
				ApplyOffice2007WelcomePageStyle(wizardPage, null, null);
			}
			((Control)wizardPage).Size = new Size(534, 289);
			IComponent component = dh.CreateComponent(typeof(Label));
			Label val = (Label)((component is Label) ? component : null);
			((Control)wizardPage).Controls.Add((Control)(object)val);
			((Control)val).Location = new Point(210, 18);
			((Control)val).Text = "Welcome to the <Wizard Name> Wizard";
			((Control)val).BackColor = Color.Transparent;
			try
			{
				((Control)val).Font = new Font("Tahoma", 16f);
				TypeDescriptor.GetProperties(val)["AutoSize"].SetValue(val, false);
			}
			catch
			{
			}
			((Control)val).Size = new Size(310, 66);
			((Control)val).Anchor = (AnchorStyles)13;
			IComponent component2 = dh.CreateComponent(typeof(Label));
			val = (Label)((component2 is Label) ? component2 : null);
			((Control)wizardPage).Controls.Add((Control)(object)val);
			((Control)val).Location = new Point(210, 100);
			((Control)val).Text = "This wizard will guide you through the <Enter Process Name>.\r\n\r\n<Enter brief description of the process wizard is covering.>";
			((Control)val).BackColor = Color.Transparent;
			try
			{
				TypeDescriptor.GetProperties(val)["AutoSize"].SetValue(val, false);
			}
			catch
			{
			}
			((Control)val).Size = new Size(309, 157);
			((Control)val).Anchor = (AnchorStyles)15;
			IComponent component3 = dh.CreateComponent(typeof(Label));
			val = (Label)((component3 is Label) ? component3 : null);
			((Control)wizardPage).Controls.Add((Control)(object)val);
			((Control)val).Location = new Point(210, 266);
			((Control)val).Text = "To continue, click Next.";
			((Control)val).Size = new Size(120, 13);
			((Control)val).Anchor = (AnchorStyles)6;
			((Control)val).BackColor = Color.Transparent;
			cc?.OnComponentChanging(parent, TypeDescriptor.GetProperties(parent)["WizardPages"]);
			parent.WizardPages.Add(wizardPage);
			cc?.OnComponentChanged(parent, TypeDescriptor.GetProperties(parent)["WizardPages"], null, null);
			ss?.SetSelectedComponents(new WizardPage[1] { wizardPage }, SelectionTypes.Replace);
			TypeDescriptor.GetProperties(parent)["SelectedPageIndex"].SetValue(parent, parent.WizardPages.IndexOf(wizardPage));
		}
		catch
		{
			designerTransaction.Cancel();
		}
		finally
		{
			if (!designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
		}
		return wizardPage;
	}

	internal static WizardPage CreatePage(Wizard parent, bool innerPage, IDesignerHost dh, IComponentChangeService cc, ISelectionService ss, eWizardStyle wizardStyle)
	{
		DesignerTransaction designerTransaction = dh.CreateTransaction();
		WizardPage wizardPage = null;
		try
		{
			wizardPage = dh.CreateComponent(typeof(WizardPage)) as WizardPage;
			wizardPage.AntiAlias = false;
			wizardPage.InteriorPage = innerPage;
			if (innerPage)
			{
				wizardPage.PageTitle = "< Wizard step title >";
				wizardPage.PageDescription = "< Wizard step description >";
			}
			switch (wizardStyle)
			{
			case eWizardStyle.Default:
				ApplyDefaultInnerPageStyle(wizardPage, dh, cc);
				break;
			case eWizardStyle.Office2007:
				ApplyOffice2007InnerPageStyle(wizardPage, dh, cc);
				break;
			}
			cc?.OnComponentChanging(parent, TypeDescriptor.GetProperties(parent)["WizardPages"]);
			parent.WizardPages.Add(wizardPage);
			cc?.OnComponentChanged(parent, TypeDescriptor.GetProperties(parent)["WizardPages"], null, null);
			ss?.SetSelectedComponents(new WizardPage[1] { wizardPage }, SelectionTypes.Replace);
			TypeDescriptor.GetProperties(parent)["SelectedPageIndex"].SetValue(parent, parent.WizardPages.IndexOf(wizardPage));
		}
		catch
		{
			designerTransaction.Cancel();
		}
		finally
		{
			if (!designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
		}
		return wizardPage;
	}

	protected override void OnMouseDragBegin(int x, int y)
	{
		if (((ControlDesigner)this).Control is Wizard wizard)
		{
			Point pt = ((Control)wizard.NextButtonControl).Parent.PointToClient(new Point(x, y));
			if (((Control)wizard.NextButtonControl).Enabled && ((Control)wizard.NextButtonControl).Visible && ((Control)wizard.NextButtonControl).Bounds.Contains(pt))
			{
				SelectNextPage(wizard);
				return;
			}
			if (((Control)wizard.BackButtonControl).Enabled && ((Control)wizard.BackButtonControl).Visible && ((Control)wizard.BackButtonControl).Bounds.Contains(pt))
			{
				SelectPreviousPage(wizard);
				return;
			}
			if (m_NavigationControl != null)
			{
				pt = ((Control)m_NavigationControl).PointToClient(new Point(x, y));
				if (((Control)m_NavigationControl.LinkNext).Bounds.Contains(pt))
				{
					SelectNextPage(wizard);
					return;
				}
				if (((Control)m_NavigationControl.LinkBack).Bounds.Contains(pt))
				{
					SelectPreviousPage(wizard);
					return;
				}
			}
		}
		((ControlDesigner)this).OnMouseDragBegin(x, y);
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		((ControlDesigner)this).PreFilterProperties(properties);
		properties["WizardStyle"] = TypeDescriptor.CreateProperty(((object)this).GetType(), "WizardStyle", typeof(eWizardStyle), new BrowsableAttribute(browsable: true), new DesignOnlyAttribute(isDesignOnly: true), new DefaultValueAttribute(eWizardStyle.Default), new DescriptionAttribute("Indicates the visual appearance style of the Wizard"));
	}

	private void SetDefaultFooterStyle(Wizard w)
	{
		w.FooterStyle.BackColor = SystemColors.Control;
		w.FooterStyle.BackColorGradientAngle = 90;
		w.FooterStyle.BorderBottomWidth = 1;
		w.FooterStyle.BorderColor = SystemColors.Control;
		w.FooterStyle.BorderLeftWidth = 1;
		w.FooterStyle.BorderRightWidth = 1;
		w.FooterStyle.BorderTop = eStyleBorderType.Etched;
		w.FooterStyle.BorderTopColor = SystemColors.Control;
		w.FooterStyle.BorderTopWidth = 1;
		w.FooterStyle.TextAlignment = eStyleTextAlignment.Center;
		w.FooterStyle.TextColorSchemePart = eColorSchemePart.PanelText;
	}

	private void ChangeWizardStyle(eWizardStyle style)
	{
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected O, but got Unknown
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		IComponentChangeService cc = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		IDesignerHost designerHost = ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) as IDesignerHost;
		DesignerTransaction designerTransaction = designerHost.CreateTransaction();
		try
		{
			Wizard wizard = ((ControlDesigner)this).Control as Wizard;
			wizard.FooterStyle.Reset();
			switch (style)
			{
			case eWizardStyle.Office2007:
				wizard.FooterStyle.BackColor = Color.Transparent;
				((Control)wizard).BackColor = Color.FromArgb(205, 229, 253);
				((Control)wizard).ForeColor = ColorScheme.GetColor(997761);
				((Control)wizard).BackgroundImage = LoadWizardImage(eWizardImages.BackgroundOffice2007);
				wizard.HeaderStyle.Reset();
				wizard.HeaderStyle.BackColor = Color.Transparent;
				wizard.HeaderStyle.BackColor2 = Color.Empty;
				wizard.HeaderStyle.BackColorGradientAngle = 90;
				wizard.HeaderStyle.BorderBottomColor = ColorScheme.GetColor(7970230);
				wizard.HeaderStyle.BorderBottomWidth = 1;
				wizard.HeaderStyle.BorderBottom = eStyleBorderType.Solid;
				wizard.HeaderHeight = 90;
				wizard.HeaderDescriptionVisible = false;
				wizard.HeaderImageAlignment = eWizardTitleImageAlignment.Left;
				wizard.HeaderCaptionFont = new Font(((Control)wizard).Font.FontFamily, 12f, (FontStyle)1);
				break;
			case eWizardStyle.Default:
				SetDefaultFooterStyle(wizard);
				((Control)wizard).BackgroundImage = null;
				((Control)wizard).BackColor = SystemColors.Control;
				((Control)wizard).ForeColor = SystemColors.ControlText;
				wizard.HeaderStyle.Reset();
				wizard.HeaderStyle.BackColor = Color.FromArgb(255, 255, 255);
				wizard.HeaderStyle.BackColorGradientAngle = 90;
				wizard.HeaderStyle.BorderBottom = eStyleBorderType.Etched;
				wizard.HeaderStyle.BorderBottomWidth = 1;
				wizard.HeaderStyle.BorderColor = SystemColors.Control;
				wizard.HeaderStyle.BorderLeftWidth = 1;
				wizard.HeaderStyle.BorderRightWidth = 1;
				wizard.HeaderStyle.BorderTopWidth = 1;
				wizard.HeaderStyle.TextAlignment = eStyleTextAlignment.Center;
				wizard.HeaderStyle.TextColorSchemePart = eColorSchemePart.PanelText;
				wizard.HeaderHeight = 60;
				wizard.HeaderDescriptionVisible = true;
				wizard.HeaderCaptionFont = new Font(((Control)wizard).Font, (FontStyle)1);
				wizard.HeaderImageAlignment = eWizardTitleImageAlignment.Right;
				break;
			}
			for (int i = 0; i < wizard.WizardPages.Count; i++)
			{
				WizardPage wizardPage = wizard.WizardPages[i];
				if (!wizardPage.InteriorPage && i == 0)
				{
					switch (style)
					{
					case eWizardStyle.Default:
						ApplyDefaultWelcomePageStyle(wizardPage, null, cc);
						break;
					case eWizardStyle.Office2007:
						ApplyOffice2007WelcomePageStyle(wizardPage, null, cc);
						break;
					}
				}
				else
				{
					switch (style)
					{
					case eWizardStyle.Default:
						ApplyDefaultInnerPageStyle(wizardPage, null, cc);
						break;
					case eWizardStyle.Office2007:
						ApplyOffice2007InnerPageStyle(wizardPage, null, cc);
						break;
					}
				}
			}
		}
		catch
		{
			designerTransaction.Cancel();
		}
		finally
		{
			if (!designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
		}
	}
}

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
public class WizardPage : PanelControl
{
	private eWizardButtonState m_BackButtonEnabled = eWizardButtonState.Auto;

	private eWizardButtonState m_BackButtonVisible = eWizardButtonState.Auto;

	private eWizardButtonState m_NextButtonEnabled = eWizardButtonState.Auto;

	private eWizardButtonState m_NextButtonVisible = eWizardButtonState.Auto;

	private eWizardButtonState m_FinishButtonEnabled = eWizardButtonState.Auto;

	private eWizardButtonState m_CancelButtonEnabled = eWizardButtonState.Auto;

	private eWizardButtonState m_CancelButtonVisible = eWizardButtonState.Auto;

	private eWizardButtonState m_HelpButtonEnabled = eWizardButtonState.Auto;

	private eWizardButtonState m_HelpButtonVisible = eWizardButtonState.Auto;

	private string m_PageTitle = "";

	private string m_PageDescription = "";

	private Image m_PageHeaderImage;

	private string m_FormCaption = "";

	private bool m_InteriorPage = true;

	[Browsable(false)]
	public bool IsSelected
	{
		get
		{
			if (((Control)this).Parent is Wizard wizard)
			{
				return wizard.SelectedPage == this;
			}
			return false;
		}
	}

	[Browsable(true)]
	[DefaultValue(eWizardButtonState.Auto)]
	[Category("Page Options")]
	[Description("Indicates whether back button is enabled when page is active.")]
	public eWizardButtonState BackButtonEnabled
	{
		get
		{
			return m_BackButtonEnabled;
		}
		set
		{
			m_BackButtonEnabled = value;
			UpdatePageState();
		}
	}

	[Browsable(true)]
	[DefaultValue(eWizardButtonState.Auto)]
	[Category("Page Options")]
	[Description("Indicates whether back button is visible when page is active.")]
	public eWizardButtonState BackButtonVisible
	{
		get
		{
			return m_BackButtonVisible;
		}
		set
		{
			m_BackButtonVisible = value;
			UpdatePageState();
		}
	}

	[Browsable(true)]
	[DefaultValue(eWizardButtonState.Auto)]
	[Category("Page Options")]
	[Description("Indicates whether next button is enabled when page is active.")]
	public eWizardButtonState NextButtonEnabled
	{
		get
		{
			return m_NextButtonEnabled;
		}
		set
		{
			m_NextButtonEnabled = value;
			UpdatePageState();
		}
	}

	[Browsable(true)]
	[DefaultValue(eWizardButtonState.Auto)]
	[Category("Page Options")]
	[Description("Indicates whether next button is visible when page is active.")]
	public eWizardButtonState NextButtonVisible
	{
		get
		{
			return m_NextButtonVisible;
		}
		set
		{
			m_NextButtonVisible = value;
			UpdatePageState();
		}
	}

	[Browsable(true)]
	[DefaultValue(eWizardButtonState.Auto)]
	[Category("Page Options")]
	[Description("Indicates whether finish button is enabled when page is active.")]
	public eWizardButtonState FinishButtonEnabled
	{
		get
		{
			return m_FinishButtonEnabled;
		}
		set
		{
			m_FinishButtonEnabled = value;
			UpdatePageState();
		}
	}

	[Browsable(true)]
	[DefaultValue(eWizardButtonState.Auto)]
	[Category("Page Options")]
	[Description("Indicates whether cancel button is enabled when page is active.")]
	public eWizardButtonState CancelButtonEnabled
	{
		get
		{
			return m_CancelButtonEnabled;
		}
		set
		{
			m_CancelButtonEnabled = value;
			UpdatePageState();
		}
	}

	[Browsable(true)]
	[DefaultValue(eWizardButtonState.Auto)]
	[Category("Page Options")]
	[Description("Indicates whether cancel button is visible when page is active.")]
	public eWizardButtonState CancelButtonVisible
	{
		get
		{
			return m_CancelButtonVisible;
		}
		set
		{
			m_CancelButtonVisible = value;
			UpdatePageState();
		}
	}

	[Browsable(true)]
	[DefaultValue(eWizardButtonState.Auto)]
	[Category("Page Options")]
	[Description("Indicates whether help button is enabled when page is active.")]
	public eWizardButtonState HelpButtonEnabled
	{
		get
		{
			return m_HelpButtonEnabled;
		}
		set
		{
			m_HelpButtonEnabled = value;
			UpdatePageState();
		}
	}

	[Browsable(true)]
	[DefaultValue(eWizardButtonState.Auto)]
	[Category("Page Options")]
	[Description("Indicates whether help button is visible when page is active.")]
	public eWizardButtonState HelpButtonVisible
	{
		get
		{
			return m_HelpButtonVisible;
		}
		set
		{
			m_HelpButtonVisible = value;
			UpdatePageState();
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Page Options")]
	[Description("Indicates the page header image when page is an interior page.")]
	public Image PageHeaderImage
	{
		get
		{
			return m_PageHeaderImage;
		}
		set
		{
			m_PageHeaderImage = value;
			UpdatePageState();
		}
	}

	[Browsable(true)]
	[Localizable(true)]
	[DefaultValue("")]
	[Category("Page Options")]
	[Description("Indicates text that is displayed as title in wizard header.")]
	public string PageTitle
	{
		get
		{
			return m_PageTitle;
		}
		set
		{
			m_PageTitle = value;
			UpdatePageState();
		}
	}

	[Browsable(true)]
	[Localizable(true)]
	[DefaultValue("")]
	[Category("Page Options")]
	[Description("Indicates text that is displayed as description in wizard header when page is active.")]
	public string PageDescription
	{
		get
		{
			return m_PageDescription;
		}
		set
		{
			m_PageDescription = value;
			UpdatePageState();
		}
	}

	[Browsable(true)]
	[Localizable(true)]
	[DefaultValue("")]
	[Category("Page Options")]
	[Description("Indicates text that is displayed on form caption when page is active.")]
	public string FormCaption
	{
		get
		{
			return m_FormCaption;
		}
		set
		{
			m_FormCaption = value;
			UpdatePageState();
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Page Options")]
	[Description("Indicates whether page is inner page.")]
	public bool InteriorPage
	{
		get
		{
			return m_InteriorPage;
		}
		set
		{
			if (m_InteriorPage != value)
			{
				m_InteriorPage = value;
				OnInnerPageChanged();
			}
		}
	}

	[Browsable(true)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public override Color BackColor
	{
		get
		{
			return base.BackColor;
		}
		set
		{
			base.BackColor = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool Visible
	{
		get
		{
			return ((Control)this).Visible;
		}
		set
		{
			((Control)this).Visible = value;
		}
	}

	[Description("Occurs before page is displayed")]
	public event WizardCancelPageChangeEventHandler BeforePageDisplayed;

	[Description("Occurs before page is displayed")]
	public event WizardPageChangeEventHandler AfterPageDisplayed;

	public event WizardPageChangeEventHandler AfterPageHidden;

	[Description("Occurs when Back button is clicked.")]
	public event CancelEventHandler BackButtonClick;

	[Description("Occurs when Next button is clicked.")]
	public event CancelEventHandler NextButtonClick;

	[Description("Occurs when Finish button is clicked.")]
	public event CancelEventHandler FinishButtonClick;

	[Description("Occurs when Cancel button is clicked.")]
	public event CancelEventHandler CancelButtonClick;

	[Description("Occurs when Help button is clicked.")]
	public event CancelEventHandler HelpButtonClick;

	public WizardPage()
	{
		((Control)this).BackColor = SystemColors.Control;
	}

	protected override void Dispose(bool disposing)
	{
		if (BarUtilities.DisposeItemImages && !((Component)(object)this).DesignMode)
		{
			BarUtilities.DisposeImage(ref m_PageHeaderImage);
		}
		base.Dispose(disposing);
	}

	public bool ShouldSerializeBackColor()
	{
		return ((Control)this).BackColor != SystemColors.Control;
	}

	private void UpdatePageState()
	{
		if (((Control)this).Parent is Wizard && ((Wizard)(object)((Control)this).Parent).SelectedPage == this)
		{
			((Wizard)(object)((Control)this).Parent).UpdatePageDisplay();
			((Wizard)(object)((Control)this).Parent).SetupButtons(canSetFocus: false);
		}
	}

	private void OnInnerPageChanged()
	{
		if (((Control)this).Parent is Wizard)
		{
			Wizard wizard = ((Control)this).Parent as Wizard;
			wizard.SetupPage(this);
			if (wizard.SelectedPage == this)
			{
				wizard.UpdatePageDisplay();
			}
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		base.OnPaint(e);
		if (((Component)(object)this).DesignMode && InteriorPage)
		{
			Graphics graphics = e.Graphics;
			Rectangle displayRectangle = ((Control)this).DisplayRectangle;
			Pen val = new Pen(Color.FromArgb(100, SystemColors.ControlDarkDark), 1f);
			try
			{
				val.DashStyle = (DashStyle)2;
				SmoothingMode smoothingMode = graphics.SmoothingMode;
				graphics.SmoothingMode = (SmoothingMode)0;
				displayRectangle.Inflate(-12, 0);
				graphics.DrawLine(val, displayRectangle.X, displayRectangle.Y, displayRectangle.X, displayRectangle.Bottom);
				graphics.DrawLine(val, displayRectangle.Right, displayRectangle.Y, displayRectangle.Right, displayRectangle.Bottom);
				displayRectangle.Inflate(-22, 0);
				graphics.DrawLine(val, displayRectangle.X, displayRectangle.Y, displayRectangle.X, displayRectangle.Bottom);
				graphics.DrawLine(val, displayRectangle.Right, displayRectangle.Y, displayRectangle.Right, displayRectangle.Bottom);
				displayRectangle.Inflate(-22, 0);
				graphics.DrawLine(val, displayRectangle.X, displayRectangle.Y, displayRectangle.X, displayRectangle.Bottom);
				graphics.DrawLine(val, displayRectangle.Right, displayRectangle.Y, displayRectangle.Right, displayRectangle.Bottom);
				graphics.SmoothingMode = smoothingMode;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	protected virtual void OnBeforePageDisplayed(WizardCancelPageChangeEventArgs e)
	{
		if (this.BeforePageDisplayed != null)
		{
			this.BeforePageDisplayed(this, e);
		}
	}

	internal void InvokeBeforePageDisplayed(WizardCancelPageChangeEventArgs e)
	{
		OnBeforePageDisplayed(e);
	}

	protected virtual void OnAfterPageDisplayed(WizardPageChangeEventArgs e)
	{
		if (this.AfterPageDisplayed != null)
		{
			this.AfterPageDisplayed(this, e);
		}
	}

	internal void InvokeAfterPageDisplayed(WizardPageChangeEventArgs e)
	{
		OnAfterPageDisplayed(e);
	}

	protected virtual void OnAfterPageHidden(WizardPageChangeEventArgs e)
	{
		if (this.AfterPageHidden != null)
		{
			this.AfterPageHidden(this, e);
		}
	}

	internal void InvokeAfterPageHidden(WizardPageChangeEventArgs e)
	{
		OnAfterPageHidden(e);
	}

	protected virtual void OnBackButtonClick(CancelEventArgs e)
	{
		if (this.BackButtonClick != null)
		{
			this.BackButtonClick(this, e);
		}
	}

	internal void InvokeBackButtonClick(CancelEventArgs e)
	{
		OnBackButtonClick(e);
	}

	protected virtual void OnNextButtonClick(CancelEventArgs e)
	{
		if (this.NextButtonClick != null)
		{
			this.NextButtonClick(this, e);
		}
	}

	internal void InvokeNextButtonClick(CancelEventArgs e)
	{
		OnNextButtonClick(e);
	}

	protected virtual void OnFinishButtonClick(CancelEventArgs e)
	{
		if (this.FinishButtonClick != null)
		{
			this.FinishButtonClick(this, e);
		}
	}

	internal void InvokeFinishButtonClick(CancelEventArgs e)
	{
		OnFinishButtonClick(e);
	}

	protected virtual void OnCancelButtonClick(CancelEventArgs e)
	{
		if (this.CancelButtonClick != null)
		{
			this.CancelButtonClick(this, e);
		}
	}

	internal void InvokeCancelButtonClick(CancelEventArgs e)
	{
		OnCancelButtonClick(e);
	}

	protected virtual void OnHelpButtonClick(CancelEventArgs e)
	{
		if (this.HelpButtonClick != null)
		{
			this.HelpButtonClick(this, e);
		}
	}

	internal void InvokeHelpButtonClick(CancelEventArgs e)
	{
		OnHelpButtonClick(e);
	}
}

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[Designer("DevComponents.DotNetBar.Design.WizardPageDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class WizardPage : PanelControl
{
	private eWizardButtonState げ = eWizardButtonState.Auto;

	private eWizardButtonState こ = eWizardButtonState.Auto;

	private eWizardButtonState ご = eWizardButtonState.Auto;

	private eWizardButtonState さ = eWizardButtonState.Auto;

	private eWizardButtonState ざ = eWizardButtonState.Auto;

	private eWizardButtonState し = eWizardButtonState.Auto;

	private eWizardButtonState じ = eWizardButtonState.Auto;

	private eWizardButtonState す = eWizardButtonState.Auto;

	private eWizardButtonState 〳 = eWizardButtonState.Auto;

	private string ず = "";

	private string せ = "";

	private Image ぜ;

	private string そ = "";

	private bool ぞ = true;

	private WizardCancelPageChangeEventHandler た;

	private WizardPageChangeEventHandler だ;

	private WizardPageChangeEventHandler ち;

	private CancelEventHandler _1BFE;

	private CancelEventHandler 〴;

	private CancelEventHandler 〵;

	private CancelEventHandler _3036;

	private CancelEventHandler Ꮹ;

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

	[DefaultValue(eWizardButtonState.Auto)]
	[Browsable(true)]
	[Category("Page Options")]
	[Description("Indicates whether back button is enabled when page is active.")]
	public eWizardButtonState BackButtonEnabled
	{
		get
		{
			return げ;
		}
		set
		{
			げ = value;
			ぢ();
		}
	}

	[Description("Indicates whether back button is visible when page is active.")]
	[Browsable(true)]
	[Category("Page Options")]
	[DefaultValue(eWizardButtonState.Auto)]
	public eWizardButtonState BackButtonVisible
	{
		get
		{
			return こ;
		}
		set
		{
			こ = value;
			ぢ();
		}
	}

	[DefaultValue(eWizardButtonState.Auto)]
	[Category("Page Options")]
	[Description("Indicates whether next button is enabled when page is active.")]
	[Browsable(true)]
	public eWizardButtonState NextButtonEnabled
	{
		get
		{
			return ご;
		}
		set
		{
			ご = value;
			ぢ();
		}
	}

	[Browsable(true)]
	[Category("Page Options")]
	[Description("Indicates whether next button is visible when page is active.")]
	[DefaultValue(eWizardButtonState.Auto)]
	public eWizardButtonState NextButtonVisible
	{
		get
		{
			return さ;
		}
		set
		{
			さ = value;
			ぢ();
		}
	}

	[Browsable(true)]
	[Category("Page Options")]
	[DefaultValue(eWizardButtonState.Auto)]
	[Description("Indicates whether finish button is enabled when page is active.")]
	public eWizardButtonState FinishButtonEnabled
	{
		get
		{
			return ざ;
		}
		set
		{
			ざ = value;
			ぢ();
		}
	}

	[Category("Page Options")]
	[Description("Indicates whether cancel button is enabled when page is active.")]
	[DefaultValue(eWizardButtonState.Auto)]
	[Browsable(true)]
	public eWizardButtonState CancelButtonEnabled
	{
		get
		{
			return し;
		}
		set
		{
			し = value;
			ぢ();
		}
	}

	[Browsable(true)]
	[Category("Page Options")]
	[Description("Indicates whether cancel button is visible when page is active.")]
	[DefaultValue(eWizardButtonState.Auto)]
	public eWizardButtonState CancelButtonVisible
	{
		get
		{
			return じ;
		}
		set
		{
			じ = value;
			ぢ();
		}
	}

	[Category("Page Options")]
	[Browsable(true)]
	[DefaultValue(eWizardButtonState.Auto)]
	[Description("Indicates whether help button is enabled when page is active.")]
	public eWizardButtonState HelpButtonEnabled
	{
		get
		{
			return す;
		}
		set
		{
			す = value;
			ぢ();
		}
	}

	[DefaultValue(eWizardButtonState.Auto)]
	[Category("Page Options")]
	[Browsable(true)]
	[Description("Indicates whether help button is visible when page is active.")]
	public eWizardButtonState HelpButtonVisible
	{
		get
		{
			return 〳;
		}
		set
		{
			〳 = value;
			ぢ();
		}
	}

	[DefaultValue(null)]
	[Category("Page Options")]
	[Browsable(true)]
	[Description("Indicates the page header image when page is an interior page.")]
	public Image PageHeaderImage
	{
		get
		{
			return ぜ;
		}
		set
		{
			ぜ = value;
			ぢ();
		}
	}

	[Browsable(true)]
	[Localizable(true)]
	[Category("Page Options")]
	[DefaultValue("")]
	[Description("Indicates text that is displayed as title in wizard header.")]
	public string PageTitle
	{
		get
		{
			return ず;
		}
		set
		{
			ず = value;
			ぢ();
		}
	}

	[Category("Page Options")]
	[Browsable(true)]
	[DefaultValue("")]
	[Localizable(true)]
	[Description("Indicates text that is displayed as description in wizard header when page is active.")]
	public string PageDescription
	{
		get
		{
			return せ;
		}
		set
		{
			せ = value;
			ぢ();
		}
	}

	[Description("Indicates text that is displayed on form caption when page is active.")]
	[Browsable(true)]
	[DefaultValue("")]
	[Localizable(true)]
	[Category("Page Options")]
	public string FormCaption
	{
		get
		{
			return そ;
		}
		set
		{
			そ = value;
			ぢ();
		}
	}

	[Category("Page Options")]
	[Browsable(true)]
	[Description("Indicates whether page is inner page.")]
	[DefaultValue(true)]
	public bool InteriorPage
	{
		get
		{
			return ぞ;
		}
		set
		{
			if (ぞ != value)
			{
				ぞ = value;
				っ();
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
	public event WizardCancelPageChangeEventHandler BeforePageDisplayed
	{
		add
		{
			WizardCancelPageChangeEventHandler wizardCancelPageChangeEventHandler = た;
			WizardCancelPageChangeEventHandler wizardCancelPageChangeEventHandler2;
			do
			{
				wizardCancelPageChangeEventHandler2 = wizardCancelPageChangeEventHandler;
				WizardCancelPageChangeEventHandler value2 = (WizardCancelPageChangeEventHandler)Delegate.Combine(wizardCancelPageChangeEventHandler2, value);
				wizardCancelPageChangeEventHandler = Interlocked.CompareExchange(ref た, value2, wizardCancelPageChangeEventHandler2);
			}
			while ((object)wizardCancelPageChangeEventHandler != wizardCancelPageChangeEventHandler2);
		}
		remove
		{
			WizardCancelPageChangeEventHandler wizardCancelPageChangeEventHandler = た;
			WizardCancelPageChangeEventHandler wizardCancelPageChangeEventHandler2;
			do
			{
				wizardCancelPageChangeEventHandler2 = wizardCancelPageChangeEventHandler;
				WizardCancelPageChangeEventHandler value2 = (WizardCancelPageChangeEventHandler)Delegate.Remove(wizardCancelPageChangeEventHandler2, value);
				wizardCancelPageChangeEventHandler = Interlocked.CompareExchange(ref た, value2, wizardCancelPageChangeEventHandler2);
			}
			while ((object)wizardCancelPageChangeEventHandler != wizardCancelPageChangeEventHandler2);
		}
	}

	[Description("Occurs before page is displayed")]
	public event WizardPageChangeEventHandler AfterPageDisplayed
	{
		add
		{
			WizardPageChangeEventHandler wizardPageChangeEventHandler = だ;
			WizardPageChangeEventHandler wizardPageChangeEventHandler2;
			do
			{
				wizardPageChangeEventHandler2 = wizardPageChangeEventHandler;
				WizardPageChangeEventHandler value2 = (WizardPageChangeEventHandler)Delegate.Combine(wizardPageChangeEventHandler2, value);
				wizardPageChangeEventHandler = Interlocked.CompareExchange(ref だ, value2, wizardPageChangeEventHandler2);
			}
			while ((object)wizardPageChangeEventHandler != wizardPageChangeEventHandler2);
		}
		remove
		{
			WizardPageChangeEventHandler wizardPageChangeEventHandler = だ;
			WizardPageChangeEventHandler wizardPageChangeEventHandler2;
			do
			{
				wizardPageChangeEventHandler2 = wizardPageChangeEventHandler;
				WizardPageChangeEventHandler value2 = (WizardPageChangeEventHandler)Delegate.Remove(wizardPageChangeEventHandler2, value);
				wizardPageChangeEventHandler = Interlocked.CompareExchange(ref だ, value2, wizardPageChangeEventHandler2);
			}
			while ((object)wizardPageChangeEventHandler != wizardPageChangeEventHandler2);
		}
	}

	public event WizardPageChangeEventHandler AfterPageHidden
	{
		add
		{
			WizardPageChangeEventHandler wizardPageChangeEventHandler = ち;
			WizardPageChangeEventHandler wizardPageChangeEventHandler2;
			do
			{
				wizardPageChangeEventHandler2 = wizardPageChangeEventHandler;
				WizardPageChangeEventHandler value2 = (WizardPageChangeEventHandler)Delegate.Combine(wizardPageChangeEventHandler2, value);
				wizardPageChangeEventHandler = Interlocked.CompareExchange(ref ち, value2, wizardPageChangeEventHandler2);
			}
			while ((object)wizardPageChangeEventHandler != wizardPageChangeEventHandler2);
		}
		remove
		{
			WizardPageChangeEventHandler wizardPageChangeEventHandler = ち;
			WizardPageChangeEventHandler wizardPageChangeEventHandler2;
			do
			{
				wizardPageChangeEventHandler2 = wizardPageChangeEventHandler;
				WizardPageChangeEventHandler value2 = (WizardPageChangeEventHandler)Delegate.Remove(wizardPageChangeEventHandler2, value);
				wizardPageChangeEventHandler = Interlocked.CompareExchange(ref ち, value2, wizardPageChangeEventHandler2);
			}
			while ((object)wizardPageChangeEventHandler != wizardPageChangeEventHandler2);
		}
	}

	[Description("Occurs when Back button is clicked.")]
	public event CancelEventHandler BackButtonClick
	{
		add
		{
			CancelEventHandler cancelEventHandler = _1BFE;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _1BFE, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = _1BFE;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _1BFE, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	[Description("Occurs when Next button is clicked.")]
	public event CancelEventHandler NextButtonClick
	{
		add
		{
			CancelEventHandler cancelEventHandler = 〴;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref 〴, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = 〴;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref 〴, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	[Description("Occurs when Finish button is clicked.")]
	public event CancelEventHandler FinishButtonClick
	{
		add
		{
			CancelEventHandler cancelEventHandler = 〵;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref 〵, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = 〵;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref 〵, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	[Description("Occurs when Cancel button is clicked.")]
	public event CancelEventHandler CancelButtonClick
	{
		add
		{
			CancelEventHandler cancelEventHandler = _3036;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _3036, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = _3036;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _3036, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	[Description("Occurs when Help button is clicked.")]
	public event CancelEventHandler HelpButtonClick
	{
		add
		{
			CancelEventHandler cancelEventHandler = Ꮹ;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref Ꮹ, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = Ꮹ;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref Ꮹ, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	public WizardPage()
	{
		((Control)this).BackColor = SystemColors.Control;
	}

	protected override void Dispose(bool disposing)
	{
		if (BarUtilities.DisposeItemImages && !((Component)(object)this).DesignMode)
		{
			BarUtilities.ᆟ(ref ぜ);
		}
		base.Dispose(disposing);
	}

	public bool ShouldSerializeBackColor()
	{
		return ((Control)this).BackColor != SystemColors.Control;
	}

	private void ぢ()
	{
		if (((Control)this).Parent is Wizard && ((Wizard)(object)((Control)this).Parent).SelectedPage == this)
		{
			((Wizard)(object)((Control)this).Parent).か();
			((Wizard)(object)((Control)this).Parent).が(き: false);
		}
	}

	private void っ()
	{
		if (((Control)this).Parent is Wizard)
		{
			Wizard wizard = ((Control)this).Parent as Wizard;
			wizard.ぐ(this);
			if (wizard.SelectedPage == this)
			{
				wizard.か();
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
		if (た != null)
		{
			た(this, e);
		}
	}

	internal void つ(WizardCancelPageChangeEventArgs ؿ)
	{
		OnBeforePageDisplayed(ؿ);
	}

	protected virtual void OnAfterPageDisplayed(WizardPageChangeEventArgs e)
	{
		if (だ != null)
		{
			だ(this, e);
		}
	}

	internal void づ(WizardPageChangeEventArgs ؿ)
	{
		OnAfterPageDisplayed(ؿ);
	}

	protected virtual void OnAfterPageHidden(WizardPageChangeEventArgs e)
	{
		if (ち != null)
		{
			ち(this, e);
		}
	}

	internal void て(WizardPageChangeEventArgs ؿ)
	{
		OnAfterPageHidden(ؿ);
	}

	protected virtual void OnBackButtonClick(CancelEventArgs e)
	{
		if (_1BFE != null)
		{
			_1BFE(this, e);
		}
	}

	internal void で(CancelEventArgs ؿ)
	{
		OnBackButtonClick(ؿ);
	}

	protected virtual void OnNextButtonClick(CancelEventArgs e)
	{
		if (〴 != null)
		{
			〴(this, e);
		}
	}

	internal void と(CancelEventArgs ؿ)
	{
		OnNextButtonClick(ؿ);
	}

	protected virtual void OnFinishButtonClick(CancelEventArgs e)
	{
		if (〵 != null)
		{
			〵(this, e);
		}
	}

	internal void ど(CancelEventArgs ؿ)
	{
		OnFinishButtonClick(ؿ);
	}

	protected virtual void OnCancelButtonClick(CancelEventArgs e)
	{
		if (_3036 != null)
		{
			_3036(this, e);
		}
	}

	internal void な(CancelEventArgs ؿ)
	{
		OnCancelButtonClick(ؿ);
	}

	protected virtual void OnHelpButtonClick(CancelEventArgs e)
	{
		if (Ꮹ != null)
		{
			Ꮹ(this, e);
		}
	}

	internal void ᏻ(CancelEventArgs ؿ)
	{
		OnHelpButtonClick(ؿ);
	}
}

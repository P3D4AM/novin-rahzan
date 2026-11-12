using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

[ToolboxItem(false)]
public class Printing : IPrinting
{
	private ISyntaxEdit owner;

	private EditorPrintDocument printDocument;

	private PrintOptions options;

	private PrintOptions allowedOptions;

	private PageHeader header;

	private PageHeader footer;

	private PrinterSettings printerSettings;

	private PrintDialog printDialog;

	private PageSetupDialog pageSetupDialog;

	private PrintPreviewDialog printPreviewDialog;

	private IPrintOptionsDialog printOptionsDialog;

	private HookHandler hookProc;

	private IntPtr hookHandle = IntPtr.Zero;

	private IntPtr lastWnd = IntPtr.Zero;

	private bool dialogCalled;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public PrinterSettings PrinterSettings => printerSettings;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public PrintDocument PrintDocument => (PrintDocument)(object)printDocument;

	[Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor))]
	[Description("Gets or sets \"PrintOptions\" that determines printing behaviour.")]
	public PrintOptions Options
	{
		get
		{
			return options;
		}
		set
		{
			options = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IPageHeader Header
	{
		get
		{
			return header;
		}
		set
		{
			header.Assign(value);
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IPageHeader Footer
	{
		get
		{
			return footer;
		}
		set
		{
			footer.Assign(value);
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public PrintDialog PrintDialog
	{
		get
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			if (printDialog == null)
			{
				printDialog = new PrintDialog();
				printDialog.Document = (PrintDocument)(object)printDocument;
				printDialog.PrinterSettings = printerSettings;
				printDialog.AllowSomePages = true;
				printDialog.ShowHelp = true;
				((CommonDialog)printDialog).HelpRequest += DoHelpRequest;
			}
			return printDialog;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public PageSetupDialog PageSetupDialog
	{
		get
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Expected O, but got Unknown
			if (pageSetupDialog == null)
			{
				pageSetupDialog = new PageSetupDialog();
				pageSetupDialog.Document = (PrintDocument)(object)printDocument;
				pageSetupDialog.PrinterSettings = printerSettings;
				pageSetupDialog.PageSettings = new PageSettings();
			}
			return pageSetupDialog;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public PrintPreviewDialog PrintPreviewDialog
	{
		get
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			if (printPreviewDialog == null)
			{
				printPreviewDialog = new PrintPreviewDialog();
				printPreviewDialog.Document = (PrintDocument)(object)printDocument;
			}
			return printPreviewDialog;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IPrintOptionsDialog PrintOptionsDialog
	{
		get
		{
			if (printOptionsDialog == null)
			{
				printOptionsDialog = new DlgPrintOptions();
			}
			return printOptionsDialog;
		}
		set
		{
			printOptionsDialog = value;
		}
	}

	[Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor))]
	[Description("Gets or sets Print Options dialog options, that can be changed by user.")]
	public PrintOptions AllowedOptions
	{
		get
		{
			if (!owner.Selection.IsEmpty)
			{
				return allowedOptions | PrintOptions.PrintSelection;
			}
			return allowedOptions;
		}
		set
		{
			allowedOptions = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public object XmlInfo
	{
		get
		{
			return new XmlPrintingInfo(this);
		}
		set
		{
			((XmlPrintingInfo)value).FixupReferences(this);
		}
	}

	[Browsable(false)]
	public event CreatePrintEditEvent CreatePrintEdit;

	[Browsable(false)]
	public event EventHandler Initialized;

	public Printing(ISyntaxEdit owner)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		this.owner = owner;
		header = new PageHeader(owner.Pages[0]);
		footer = new PageHeader(owner.Pages[0]);
		printerSettings = new PrinterSettings();
		printDocument = new EditorPrintDocument(this, printerSettings);
		((PrintDocument)printDocument).PrinterSettings = printerSettings;
		options = EditConsts.DefaultPrintOptions;
		allowedOptions = EditConsts.DefaultPrintOptions;
	}

	private bool EnumDialogChilds(IntPtr hwnd, IntPtr lParam)
	{
		if (string.Compare(OSUtils.GetText(hwnd), "&Help", ignoreCase: true) == 0)
		{
			OSUtils.SetText(hwnd, StringConsts.PrintOptionsButtonText);
			lastWnd = IntPtr.Zero;
			return false;
		}
		if (string.Compare(OSUtils.GetClassName(hwnd), "Button", ignoreCase: true) == 0)
		{
			lastWnd = hwnd;
		}
		return true;
	}

	private IntPtr DialogHook(int ncode, IntPtr wParam, IntPtr lParam)
	{
		if (ncode == 0 && !dialogCalled)
		{
			CWPSTRUCT cWPSTRUCT = (CWPSTRUCT)Marshal.PtrToStructure(lParam, typeof(CWPSTRUCT));
			if (cWPSTRUCT.message == 272)
			{
				lastWnd = IntPtr.Zero;
				OSUtils.EnumChildWindows(cWPSTRUCT.hwnd, EnumDialogChilds, IntPtr.Zero);
				if (lastWnd != IntPtr.Zero)
				{
					OSUtils.SetText(lastWnd, "&Options");
				}
				dialogCalled = true;
			}
		}
		return OSUtils.CallNextHook(hookHandle, ncode, wParam, lParam);
	}

	private void HookPrintDialog()
	{
		dialogCalled = false;
		hookProc = DialogHook;
		hookHandle = OSUtils.SetWndProcHook(hookProc);
	}

	private void UnhookPrintDialog()
	{
		if (hookHandle != IntPtr.Zero)
		{
			OSUtils.ReleaseHook(hookHandle);
			hookHandle = IntPtr.Zero;
			hookProc = null;
		}
	}

	private void DoHelpRequest(object sender, EventArgs e)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		if (PrintOptionsDialog != null && (int)ExecutePrintOptionsDialog() == 1)
		{
			Options = PrintOptionsDialog.Options;
		}
	}

	public SyntaxEdit OnCreatePrintEdit()
	{
		if (this.CreatePrintEdit != null)
		{
			CreatePrintEditEventArgs createPrintEditEventArgs = new CreatePrintEditEventArgs();
			this.CreatePrintEdit(this, createPrintEditEventArgs);
			if (createPrintEditEventArgs.PrintEdit != null)
			{
				return createPrintEditEventArgs.PrintEdit;
			}
		}
		return new SyntaxEdit();
	}

	public void OnInitialized()
	{
		if (this.Initialized != null)
		{
			this.Initialized(this, EventArgs.Empty);
		}
	}

	public void Print()
	{
		printDocument.Init(owner);
		((PrintDocument)printDocument).Print();
	}

	public DialogResult ExecutePrintDialog()
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		printDocument.Init(owner, calcPages: true);
		try
		{
			DialogResult result;
			if (PrintDialog.ShowHelp)
			{
				HookPrintDialog();
				try
				{
					result = ((CommonDialog)PrintDialog).ShowDialog();
				}
				finally
				{
					UnhookPrintDialog();
				}
			}
			else
			{
				result = ((CommonDialog)PrintDialog).ShowDialog();
			}
			return result;
		}
		catch
		{
			return (DialogResult)2;
		}
	}

	public DialogResult ExecutePageSetupDialog()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		printDocument.Init(owner);
		try
		{
			return ((CommonDialog)PageSetupDialog).ShowDialog();
		}
		catch
		{
			return (DialogResult)2;
		}
	}

	public DialogResult ExecutePrintPreviewDialog()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		printDocument.Init(owner);
		try
		{
			return ((Form)PrintPreviewDialog).ShowDialog();
		}
		catch
		{
			return (DialogResult)2;
		}
	}

	public DialogResult ExecutePrintOptionsDialog()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		if (PrintOptionsDialog == null)
		{
			return (DialogResult)0;
		}
		PrintOptionsDialog.Options = Options;
		PrintOptionsDialog.AllowedOptions = AllowedOptions;
		PrintOptionsDialog.FileName = owner.Source.FileName;
		return PrintOptionsDialog.ShowDialog();
	}

	public void Assign(IPrinting source)
	{
		Footer = source.Footer;
		Header = source.Header;
		Options = source.Options;
		AllowedOptions = source.AllowedOptions;
	}

	public virtual void ResetOptions()
	{
		Options = EditConsts.DefaultPrintOptions;
	}

	public virtual void ResetAllowedOptions()
	{
		AllowedOptions = EditConsts.DefaultPrintOptions;
	}

	public bool ShouldSerializeOptions()
	{
		return options != EditConsts.DefaultPrintOptions;
	}

	public bool ShouldSerializeAllowedOptions()
	{
		return allowedOptions != EditConsts.DefaultPrintOptions;
	}
}

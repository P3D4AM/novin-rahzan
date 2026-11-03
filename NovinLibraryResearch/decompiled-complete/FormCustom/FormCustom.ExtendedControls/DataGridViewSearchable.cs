using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PacketParser.Entities;

namespace FormCustom.ExtendedControls;

public class DataGridViewSearchable : DataGridView
{
	private PictureBox searchIcon = new PictureBox();

	private Label seachText = new Label();

	private CancellationTokenSource cancellation = new CancellationTokenSource();

	private bool _isDisposed;

	public TextBox SearchBox { get; set; } = new TextBox();


	public Panel FooterPanel { get; set; } = new Panel();


	public Panel HeaderPanel { get; set; } = new Panel();


	public Label RowsCountBox { get; set; } = new Label();


	public Label ColumnSumBox { get; set; } = new Label();


	public bool ShowSearchBox { get; set; } = true;


	public bool SearchBoxInheritFont { get; set; } = true;


	public bool ShowSearchLabel { get; set; } = true;


	public bool ShowFooterPanel { get; set; } = true;


	public int RowCount
	{
		get
		{
			return ((DataGridView)this).RowCount;
		}
		set
		{
			((DataGridView)this).RowCount = value;
			SetSearchBoxWidth();
		}
	}

	public HorizontalAlignment SeachBoxTextAligmnet { get; set; } = (HorizontalAlignment)1;


	public DataGridViewSearchable()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Expected O, but got Unknown
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Expected O, but got Unknown
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Expected O, but got Unknown
		((DataGridView)this).BorderStyle = (BorderStyle)1;
		Panel footerPanel = FooterPanel;
		int width = (((Control)HeaderPanel).Width = ((Control)this).Width - ((Control)((DataGridView)this).VerticalScrollBar).Width);
		((Control)footerPanel).Width = width;
		((Control)FooterPanel).Visible = ShowFooterPanel;
		((Control)((DataGridView)this).VerticalScrollBar).Height = 80;
		((Control)HeaderPanel).Visible = ShowSearchBox;
		((Control)((DataGridView)this).VerticalScrollBar).Dock = (DockStyle)0;
		((Control)((DataGridView)this).VerticalScrollBar).Padding = new Padding(0, ((Control)HeaderPanel).Height, 0, 0);
		((Control)seachText).Text = "جستجو:";
		((Control)seachText).Dock = (DockStyle)4;
		seachText.BorderStyle = (BorderStyle)0;
		((Control)seachText).RightToLeft = (RightToLeft)1;
		((Control)seachText).AutoSize = false;
		((Control)seachText).Height = ((Control)HeaderPanel).Height;
		if (ShowSearchLabel)
		{
			((Control)seachText).Width = TextRenderer.MeasureText(((Control)seachText).Text, ((Control)this).Font).Width;
		}
		else
		{
			((Control)seachText).Width = 0;
		}
		((Control)searchIcon).Size = new Size(((Control)SearchBox).Height, ((Control)SearchBox).Height - 1);
		searchIcon.SizeMode = (PictureBoxSizeMode)1;
		((Control)searchIcon).Dock = (DockStyle)3;
		((Control)SearchBox).Font = ((Control)this).Font;
		SearchBox.TextAlign = SeachBoxTextAligmnet;
		if (ShowSearchLabel)
		{
			((Control)SearchBox).Dock = (DockStyle)3;
		}
		else
		{
			((Control)SearchBox).Dock = (DockStyle)5;
		}
		((TextBoxBase)SearchBox).MaxLength = 100;
		((Control)SearchBox).Width = ((Control)HeaderPanel).Width - ((Control)seachText).Width;
		((TextBoxBase)SearchBox).BorderStyle = (BorderStyle)2;
		((Control)SearchBox).BackColor = Color.White;
		((Control)SearchBox).RightToLeft = ((Control)this).RightToLeft;
		((Control)SearchBox).Anchor = (AnchorStyles)5;
		((Control)SearchBox).Controls.Add((Control)(object)searchIcon);
		((Control)HeaderPanel).Controls.Add((Control)(object)seachText);
		((Control)HeaderPanel).Controls.Add((Control)(object)SearchBox);
		((Control)FooterPanel).Dock = (DockStyle)2;
		FooterPanel.BorderStyle = (BorderStyle)1;
		((Control)FooterPanel).BackColor = Color.White;
		((Control)FooterPanel).Height = ((Control)RowsCountBox).Height;
		((Control)FooterPanel).Width = ((Control)this).Width;
		HeaderPanel.BorderStyle = ((DataGridView)this).BorderStyle;
		((Control)HeaderPanel).Visible = ShowSearchBox;
		((Control)HeaderPanel).Dock = (DockStyle)1;
		((Control)HeaderPanel).Height = ((Control)SearchBox).Height;
		((Control)this).Controls.Add((Control)(object)HeaderPanel);
		((Control)this).Controls.Add((Control)(object)FooterPanel);
		DataGridViewCellStyle val = ((DataGridView)this).ColumnHeadersDefaultCellStyle.Clone();
		val.Alignment = (DataGridViewContentAlignment)512;
		val.Padding = new Padding(0, ((Control)HeaderPanel).Height, 0, 0);
		((DataGridView)this).RowHeadersVisible = false;
		((DataGridView)this).ColumnHeadersDefaultCellStyle = val;
		((DataGridView)this).AllowUserToAddRows = false;
		((DataGridView)this).AllowUserToDeleteRows = false;
		((Control)this).AllowDrop = false;
		((DataGridView)this).AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
		{
			BackColor = Color.White
		};
		((DataGridView)this).RowHeadersVisible = false;
		((DataGridView)this).SelectionMode = (DataGridViewSelectionMode)1;
		((DataGridView)this).FontChanged += NpGridView_FontChanged;
		((Control)this).Resize += delegate
		{
			SetSeatchBoxWidthResize();
		};
		((DataGridView)this).RowsAdded += (DataGridViewRowsAddedEventHandler)delegate
		{
			SetSearchBoxWidth();
		};
		((DataGridView)this).RowsRemoved += (DataGridViewRowsRemovedEventHandler)delegate
		{
			SetSearchBoxWidth();
		};
		((DataGridView)this).DataSourceChanged += delegate
		{
			SetSearchBoxWidth();
		};
		((Control)((DataGridView)this).VerticalScrollBar).VisibleChanged += delegate
		{
			SetSearchBoxWidth();
		};
	}

	private void NpGridView_FontChanged(object sender, EventArgs e)
	{
		if (ShowSearchLabel)
		{
			((Control)seachText).Width = TextRenderer.MeasureText(((Control)seachText).Text, ((Control)this).Font).Width;
			((Control)SearchBox).Width = ((Control)this).Width - ((Control)seachText).Width;
		}
	}

	private void SetSearchBoxWidth()
	{
		if (!_isDisposed)
		{
			cancellation?.Cancel();
			cancellation = new CancellationTokenSource(60000);
			CancellationToken token = cancellation.Token;
			SetSeatchBoxWidth(token);
			if (ShowFooterPanel)
			{
				SetSeatchFooteridth(token);
			}
		}
	}

	private async void SetSeatchBoxWidth(CancellationToken token)
	{
		await Task.Delay(50);
		if (!token.IsCancellationRequested && !_isDisposed)
		{
			if (((DataGridView)this).VerticalScrollBar.Maximum > 1 && ((Control)HeaderPanel).Width == ((Control)this).Width)
			{
				((Control)SearchBox).Width = ((Control)HeaderPanel).Width - ((Control)seachText).Width;
			}
			else if (((Control)SearchBox).Width == ((Control)HeaderPanel).Width - ((Control)seachText).Width)
			{
				((Control)SearchBox).Width = ((Control)HeaderPanel).Width - ((Control)seachText).Width;
			}
		}
	}

	private async void SetSeatchFooteridth(CancellationToken token)
	{
		await Task.Delay(50);
		if (!token.IsCancellationRequested && !_isDisposed)
		{
			if (((DataGridView)this).VerticalScrollBar.Maximum > 1 && ((Control)HeaderPanel).Width == ((Control)this).Width)
			{
				Panel footerPanel = FooterPanel;
				int width = (((Control)HeaderPanel).Width = ((Control)this).Width - ((Control)((DataGridView)this).VerticalScrollBar).Width);
				((Control)footerPanel).Width = width;
			}
			else if (((Control)SearchBox).Width == ((Control)HeaderPanel).Width - ((Control)seachText).Width)
			{
				Panel footerPanel2 = FooterPanel;
				int width = (((Control)HeaderPanel).Width = ((Control)this).Width);
				((Control)footerPanel2).Width = width;
			}
		}
	}

	private void SetSeatchBoxWidthResize()
	{
		if (((DataGridView)this).VerticalScrollBar.Maximum > 1)
		{
			Panel footerPanel = FooterPanel;
			int width = (((Control)HeaderPanel).Width = ((Control)this).Width - ((Control)((DataGridView)this).VerticalScrollBar).Width);
			((Control)footerPanel).Width = width;
			((Control)SearchBox).Width = ((Control)HeaderPanel).Width - ((Control)seachText).Width;
		}
		else
		{
			Panel footerPanel2 = FooterPanel;
			int width = (((Control)HeaderPanel).Width = ((Control)this).Width);
			((Control)footerPanel2).Width = width;
			((Control)SearchBox).Width = ((Control)HeaderPanel).Width - ((Control)seachText).Width;
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				((DataGridView)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ExtendedControls\\DataGridViewSearchable.cs", 196);
		}
	}
}

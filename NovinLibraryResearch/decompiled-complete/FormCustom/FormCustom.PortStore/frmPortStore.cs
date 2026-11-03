using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom.Cache;
using FormCustom.Properties;
using FormCustom.UC;
using PacketParser;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;

namespace FormCustom.PortStore;

public class frmPortStore : AccountingBaseForm
{
	private bool _isDisposed;

	private CancellationTokenSource _searchToken;

	private IEnumerable<PortStoreModel> _filteredList;

	private IEnumerable<PortStoreModel> _listData;

	private IContainer components;

	private Panel panel1;

	private Panel panel3;

	private PictureBox pictureBox8;

	private PictureBox pictureBoxDropDone;

	private PictureBox pictureBox4;

	private PictureBox pictureBox3;

	private Label lblService;

	private Label lblPcPoss;

	private PictureBox pictureBox1;

	private PictureBox pictureBox5;

	private PictureBox pictureBox2;

	private Label lblScale;

	private BindingSource PortStoreBindingSource;

	private Panel pnlService;

	private UcGroupBox ucGroupBox1;

	private UcSearchBox ucSearchBox1;

	private DataGridView DGrid;

	private DataGridViewTextBoxColumn typeDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn portDataGridViewTextBoxColumn;

	public int Port { get; private set; }

	public EnPortType Type { get; private set; }

	public string SearchString { get; private set; }

	public new bool IsDisposed
	{
		get
		{
			if (!base.IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	private Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			if (IsDisposed || token.IsCancellationRequested)
			{
				return Task.CompletedTask;
			}
			_listData = PortStoreModel.GetAll();
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource(60000);
			CancellationToken seToken = CancellationTokenSource.CreateLinkedTokenSource(_searchToken.Token, token).Token;
			Task.Run(() => SearchAsync(seToken));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\PortStore\\frmPortStore.cs", 40);
		}
		return Task.CompletedTask;
	}

	private async Task SearchAsync(CancellationToken token)
	{
		try
		{
			_filteredList = _listData?.Where((PortStoreModel q) => q.Type == Type);
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			List<string> list = SearchString.SplitString();
			if (list != null && list.Count > 0)
			{
				foreach (string item in list)
				{
					if (token.IsCancellationRequested || IsDisposed)
					{
						return;
					}
					if (!string.IsNullOrEmpty(item))
					{
						_filteredList = _filteredList?.Where((PortStoreModel x) => x.Name.StringComparisons(item) || x.Port.StringComparisons(item) || x.TypeForDisplay.StringComparisons(item));
					}
				}
			}
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (token.IsCancellationRequested || IsDisposed)
				{
					return;
				}
			}
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				if (token.IsCancellationRequested || IsDisposed)
				{
					return;
				}
				try
				{
					PortStoreBindingSource.DataSource = _filteredList?.OrderBy((PortStoreModel q) => q.Port)?.ToSortableBindingList(token);
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "SearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\PortStore\\frmPortStore.cs", 73);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\PortStore\\frmPortStore.cs", 76);
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_searchToken?.Cancel();
				_searchToken?.Dispose();
				_searchToken = null;
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\PortStore\\frmPortStore.cs", 92);
		}
	}

	public frmPortStore(string contextGuid, EnPortType type, Font font)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
			DGrid.Initialize(font);
			Type = type;
			Task.Run(() => LoadDataAsync(default(CancellationToken)));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\PortStore\\frmPortStore.cs", 104);
		}
	}

	private void PortStoreBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		try
		{
			Port = ((PortStoreModel)PortStoreBindingSource.Current)?.Port ?? 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "PortStoreBindingSource_CurrentChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\PortStore\\frmPortStore.cs", 114);
		}
	}

	private void lblService_Click(object sender, EventArgs e)
	{
		try
		{
			Type = EnPortType.Service;
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource(60000);
			CancellationToken token = _searchToken.Token;
			Task.Run(() => SearchAsync(token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "lblService_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\PortStore\\frmPortStore.cs", 126);
		}
	}

	private void lblPcPoss_Click(object sender, EventArgs e)
	{
		try
		{
			Type = EnPortType.PcPoss;
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource(60000);
			CancellationToken token = _searchToken.Token;
			Task.Run(() => SearchAsync(token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "lblPcPoss_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\PortStore\\frmPortStore.cs", 138);
		}
	}

	private void lblScale_Click(object sender, EventArgs e)
	{
		try
		{
			Type = EnPortType.Scale;
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource(60000);
			CancellationToken token = _searchToken.Token;
			Task.Run(() => SearchAsync(token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "lblScale_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\PortStore\\frmPortStore.cs", 150);
		}
	}

	private void ucSearchBox1_OnTextChanged(object sender, EventArgs e)
	{
		try
		{
			SearchString = ((Control)ucSearchBox1).Text;
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource(60000);
			CancellationToken token = _searchToken.Token;
			Task.Run(() => SearchAsync(token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ucSearchBox1_OnTextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\PortStore\\frmPortStore.cs", 162);
		}
	}

	private void DGrid_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if (Text.IsStringOrNumber(e.KeyChar))
			{
				((Control)ucSearchBox1).Focus();
				((Control)ucSearchBox1).Text = e.KeyChar.ToString();
				ucSearchBox1.SelectionStart = 9999;
				ucSearchBox1.SelectionLength = 0;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DGrid_KeyPress", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\PortStore\\frmPortStore.cs", 174);
		}
	}

	private void frmPortStore_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 13)
			{
				if ((int)keyCode == 27)
				{
					if (!string.IsNullOrEmpty(((Control)ucSearchBox1).Text))
					{
						((Control)ucSearchBox1).Text = "";
						return;
					}
					((Form)this).DialogResult = (DialogResult)2;
					((Form)this).Close();
				}
			}
			else
			{
				e.Handled = true;
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "frmPortStore_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\PortStore\\frmPortStore.cs", 198);
		}
	}

	private void ucSearchBox1_OnKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 40 || (int)e.KeyCode == 13)
			{
				((Control)DGrid).Focus();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ucSearchBox1_OnKeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\PortStore\\frmPortStore.cs", 207);
		}
	}

	private void lblService_Enter(object sender, EventArgs e)
	{
		try
		{
			Label val = (Label)((sender is Label) ? sender : null);
			if (val != null)
			{
				((Control)val).ForeColor = Color.FromArgb(65, 81, 220);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "lblService_Enter", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\PortStore\\frmPortStore.cs", 216);
		}
	}

	private void lblService_Leave(object sender, EventArgs e)
	{
		try
		{
			Label val = (Label)((sender is Label) ? sender : null);
			if (val != null)
			{
				((Control)val).ForeColor = Color.FromArgb(98, 116, 125);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "lblService_Leave", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\PortStore\\frmPortStore.cs", 225);
		}
	}

	private void frmPortStore_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private void InitializeComponent()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Expected O, but got Unknown
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_078e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0798: Expected O, but got Unknown
		//IL_0885: Unknown result type (might be due to invalid IL or missing references)
		//IL_088f: Expected O, but got Unknown
		//IL_097c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0986: Expected O, but got Unknown
		//IL_0c13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1d: Expected O, but got Unknown
		//IL_0cd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cdd: Expected O, but got Unknown
		//IL_0d93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d9d: Expected O, but got Unknown
		//IL_0e51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5b: Expected O, but got Unknown
		//IL_0f91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f9b: Expected O, but got Unknown
		//IL_0fc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_107b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1085: Expected O, but got Unknown
		//IL_10a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b2: Expected O, but got Unknown
		//IL_10e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b2: Expected O, but got Unknown
		//IL_11bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c9: Expected O, but got Unknown
		//IL_11fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1206: Expected O, but got Unknown
		//IL_120e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1218: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DataGridViewCellStyle val3 = new DataGridViewCellStyle();
		DataGridViewCellStyle val4 = new DataGridViewCellStyle();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmPortStore));
		panel1 = new Panel();
		panel3 = new Panel();
		pictureBox5 = new PictureBox();
		pictureBox8 = new PictureBox();
		pictureBoxDropDone = new PictureBox();
		pictureBox2 = new PictureBox();
		pictureBox4 = new PictureBox();
		pictureBox3 = new PictureBox();
		lblScale = new Label();
		lblService = new Label();
		lblPcPoss = new Label();
		pictureBox1 = new PictureBox();
		PortStoreBindingSource = new BindingSource(components);
		pnlService = new Panel();
		DGrid = new DataGridView();
		typeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		nameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		portDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		ucSearchBox1 = new UcSearchBox();
		ucGroupBox1 = new UcGroupBox();
		((Control)panel1).SuspendLayout();
		((Control)panel3).SuspendLayout();
		((ISupportInitialize)pictureBox5).BeginInit();
		((ISupportInitialize)pictureBox8).BeginInit();
		((ISupportInitialize)pictureBoxDropDone).BeginInit();
		((ISupportInitialize)pictureBox2).BeginInit();
		((ISupportInitialize)pictureBox4).BeginInit();
		((ISupportInitialize)pictureBox3).BeginInit();
		((ISupportInitialize)pictureBox1).BeginInit();
		((ISupportInitialize)PortStoreBindingSource).BeginInit();
		((Control)pnlService).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((Control)this).SuspendLayout();
		((Control)panel1).BackgroundImage = (Image)(object)FormCustom.Properties.Resources._41;
		((Control)panel1).Controls.Add((Control)(object)panel3);
		((Control)panel1).Controls.Add((Control)(object)pictureBox1);
		((Control)panel1).Dock = (DockStyle)4;
		((Control)panel1).Location = new Point(472, 0);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(236, 450);
		((Control)panel1).TabIndex = 193;
		((Control)panel3).Anchor = (AnchorStyles)15;
		((Control)panel3).BackColor = Color.Transparent;
		((Control)panel3).Controls.Add((Control)(object)pictureBox5);
		((Control)panel3).Controls.Add((Control)(object)pictureBox8);
		((Control)panel3).Controls.Add((Control)(object)pictureBoxDropDone);
		((Control)panel3).Controls.Add((Control)(object)pictureBox2);
		((Control)panel3).Controls.Add((Control)(object)pictureBox4);
		((Control)panel3).Controls.Add((Control)(object)pictureBox3);
		((Control)panel3).Controls.Add((Control)(object)lblScale);
		((Control)panel3).Controls.Add((Control)(object)lblService);
		((Control)panel3).Controls.Add((Control)(object)lblPcPoss);
		((Control)panel3).Location = new Point(4, 134);
		((Control)panel3).Name = "panel3";
		((Control)panel3).Size = new Size(227, 304);
		((Control)panel3).TabIndex = 1;
		((Control)pictureBox5).BackgroundImageLayout = (ImageLayout)0;
		((Control)pictureBox5).Cursor = Cursors.Hand;
		pictureBox5.Image = (Image)(object)FormCustom.Properties.Resources.Untitled_11;
		((Control)pictureBox5).Location = new Point(7, 113);
		((Control)pictureBox5).Margin = new Padding(15);
		((Control)pictureBox5).Name = "pictureBox5";
		((Control)pictureBox5).Size = new Size(17, 17);
		pictureBox5.SizeMode = (PictureBoxSizeMode)1;
		pictureBox5.TabIndex = 4;
		pictureBox5.TabStop = false;
		((Control)pictureBox5).Click += lblScale_Click;
		((Control)pictureBox8).BackgroundImageLayout = (ImageLayout)0;
		((Control)pictureBox8).Cursor = Cursors.Hand;
		pictureBox8.Image = (Image)(object)FormCustom.Properties.Resources.Untitled_11;
		((Control)pictureBox8).Location = new Point(7, 73);
		((Control)pictureBox8).Margin = new Padding(15);
		((Control)pictureBox8).Name = "pictureBox8";
		((Control)pictureBox8).Size = new Size(17, 17);
		pictureBox8.SizeMode = (PictureBoxSizeMode)1;
		pictureBox8.TabIndex = 4;
		pictureBox8.TabStop = false;
		((Control)pictureBox8).Click += lblPcPoss_Click;
		((Control)pictureBoxDropDone).BackgroundImageLayout = (ImageLayout)0;
		((Control)pictureBoxDropDone).Cursor = Cursors.Hand;
		pictureBoxDropDone.Image = (Image)(object)FormCustom.Properties.Resources.Untitled_11;
		((Control)pictureBoxDropDone).Location = new Point(7, 32);
		((Control)pictureBoxDropDone).Margin = new Padding(15);
		((Control)pictureBoxDropDone).Name = "pictureBoxDropDone";
		((Control)pictureBoxDropDone).Size = new Size(17, 17);
		pictureBoxDropDone.SizeMode = (PictureBoxSizeMode)1;
		pictureBoxDropDone.TabIndex = 3;
		pictureBoxDropDone.TabStop = false;
		((Control)pictureBoxDropDone).Click += lblService_Click;
		((Control)pictureBox2).Anchor = (AnchorStyles)9;
		((Control)pictureBox2).Cursor = Cursors.Hand;
		pictureBox2.Image = (Image)(object)FormCustom.Properties.Resources.Tell;
		((Control)pictureBox2).Location = new Point(182, 100);
		((Control)pictureBox2).Name = "pictureBox2";
		((Control)pictureBox2).Size = new Size(31, 40);
		pictureBox2.SizeMode = (PictureBoxSizeMode)4;
		pictureBox2.TabIndex = 1;
		pictureBox2.TabStop = false;
		((Control)pictureBox2).Click += lblScale_Click;
		((Control)pictureBox4).Anchor = (AnchorStyles)9;
		((Control)pictureBox4).Cursor = Cursors.Hand;
		pictureBox4.Image = (Image)(object)FormCustom.Properties.Resources.Tell;
		((Control)pictureBox4).Location = new Point(182, 60);
		((Control)pictureBox4).Name = "pictureBox4";
		((Control)pictureBox4).Size = new Size(31, 40);
		pictureBox4.SizeMode = (PictureBoxSizeMode)4;
		pictureBox4.TabIndex = 1;
		pictureBox4.TabStop = false;
		((Control)pictureBox4).Click += lblPcPoss_Click;
		((Control)pictureBox3).Anchor = (AnchorStyles)9;
		((Control)pictureBox3).Cursor = Cursors.Hand;
		pictureBox3.Image = (Image)(object)FormCustom.Properties.Resources.PublicInfoAccess;
		((Control)pictureBox3).Location = new Point(182, 20);
		((Control)pictureBox3).Name = "pictureBox3";
		((Control)pictureBox3).Size = new Size(31, 40);
		pictureBox3.SizeMode = (PictureBoxSizeMode)4;
		pictureBox3.TabIndex = 1;
		pictureBox3.TabStop = false;
		((Control)pictureBox3).Click += lblService_Click;
		((Control)lblScale).Anchor = (AnchorStyles)13;
		((Control)lblScale).Cursor = Cursors.Hand;
		((Control)lblScale).Font = new Font("B Yekan", 12.25f, (FontStyle)1);
		((Control)lblScale).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)lblScale).Location = new Point(22, 110);
		((Control)lblScale).Name = "lblScale";
		((Control)lblScale).Size = new Size(154, 26);
		((Control)lblScale).TabIndex = 0;
		((Control)lblScale).Text = "[ترازو]";
		lblScale.TextAlign = (ContentAlignment)16;
		((Control)lblScale).Click += lblScale_Click;
		((Control)lblScale).Enter += lblService_Enter;
		((Control)lblScale).Leave += lblService_Leave;
		((Control)lblService).Anchor = (AnchorStyles)13;
		((Control)lblService).Cursor = Cursors.Hand;
		((Control)lblService).Font = new Font("B Yekan", 12.25f, (FontStyle)1);
		((Control)lblService).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)lblService).Location = new Point(22, 29);
		((Control)lblService).Name = "lblService";
		((Control)lblService).Size = new Size(154, 26);
		((Control)lblService).TabIndex = 0;
		((Control)lblService).Text = "[سرویس ها]";
		lblService.TextAlign = (ContentAlignment)16;
		((Control)lblService).Click += lblService_Click;
		((Control)lblService).Enter += lblService_Enter;
		((Control)lblService).Leave += lblService_Leave;
		((Control)lblPcPoss).Anchor = (AnchorStyles)13;
		((Control)lblPcPoss).Cursor = Cursors.Hand;
		((Control)lblPcPoss).Font = new Font("B Yekan", 12.25f, (FontStyle)1);
		((Control)lblPcPoss).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)lblPcPoss).Location = new Point(22, 70);
		((Control)lblPcPoss).Name = "lblPcPoss";
		((Control)lblPcPoss).Size = new Size(154, 26);
		((Control)lblPcPoss).TabIndex = 0;
		((Control)lblPcPoss).Text = "[PcPoss]";
		lblPcPoss.TextAlign = (ContentAlignment)16;
		((Control)lblPcPoss).Click += lblPcPoss_Click;
		((Control)lblPcPoss).Enter += lblService_Enter;
		((Control)lblPcPoss).Leave += lblService_Leave;
		((Control)pictureBox1).Anchor = (AnchorStyles)13;
		((Control)pictureBox1).BackColor = Color.Transparent;
		pictureBox1.Image = (Image)(object)FormCustom.Properties.Resources.Logo55;
		((Control)pictureBox1).Location = new Point(4, 22);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(226, 88);
		pictureBox1.SizeMode = (PictureBoxSizeMode)4;
		pictureBox1.TabIndex = 0;
		pictureBox1.TabStop = false;
		PortStoreBindingSource.DataSource = typeof(PortStoreModel);
		PortStoreBindingSource.CurrentChanged += PortStoreBindingSource_CurrentChanged;
		((Control)pnlService).Controls.Add((Control)(object)DGrid);
		((Control)pnlService).Controls.Add((Control)(object)ucSearchBox1);
		((Control)pnlService).Controls.Add((Control)(object)ucGroupBox1);
		((Control)pnlService).Dock = (DockStyle)5;
		((Control)pnlService).Location = new Point(0, 0);
		((Control)pnlService).Name = "pnlService";
		((Control)pnlService).Size = new Size(472, 450);
		((Control)pnlService).TabIndex = 194;
		DGrid.AllowUserToAddRows = false;
		DGrid.AllowUserToDeleteRows = false;
		DGrid.AllowUserToResizeRows = false;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		DGrid.AutoGenerateColumns = false;
		DGrid.CellBorderStyle = (DataGridViewCellBorderStyle)4;
		val.Alignment = (DataGridViewContentAlignment)16;
		val.BackColor = Color.FromArgb(192, 255, 255);
		val.Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		val.ForeColor = SystemColors.WindowText;
		val.SelectionBackColor = Color.DodgerBlue;
		val.SelectionForeColor = SystemColors.HighlightText;
		val.WrapMode = (DataGridViewTriState)1;
		DGrid.ColumnHeadersDefaultCellStyle = val;
		DGrid.ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		DGrid.Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[3]
		{
			(DataGridViewColumn)typeDataGridViewTextBoxColumn,
			(DataGridViewColumn)nameDataGridViewTextBoxColumn,
			(DataGridViewColumn)portDataGridViewTextBoxColumn
		});
		DGrid.DataSource = PortStoreBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = Color.FromArgb(192, 255, 255);
		val2.Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = Color.DodgerBlue;
		val2.SelectionForeColor = SystemColors.HighlightText;
		val2.WrapMode = (DataGridViewTriState)2;
		DGrid.DefaultCellStyle = val2;
		DGrid.GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(4, 74);
		((Control)DGrid).Name = "DGrid";
		((Control)DGrid).RightToLeft = (RightToLeft)1;
		val3.Alignment = (DataGridViewContentAlignment)16;
		val3.BackColor = Color.FromArgb(192, 255, 255);
		val3.Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		val3.ForeColor = SystemColors.WindowText;
		val3.SelectionBackColor = Color.DodgerBlue;
		val3.SelectionForeColor = SystemColors.HighlightText;
		val3.WrapMode = (DataGridViewTriState)1;
		DGrid.RowHeadersDefaultCellStyle = val3;
		DGrid.RowHeadersVisible = false;
		val4.BackColor = Color.FromArgb(192, 255, 255);
		val4.SelectionBackColor = Color.DodgerBlue;
		DGrid.RowsDefaultCellStyle = val4;
		DGrid.SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(462, 364);
		((Control)DGrid).TabIndex = 168;
		((Control)DGrid).KeyPress += new KeyPressEventHandler(DGrid_KeyPress);
		((DataGridViewColumn)typeDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)typeDataGridViewTextBoxColumn).DataPropertyName = "TypeForDisplay";
		((DataGridViewColumn)typeDataGridViewTextBoxColumn).HeaderText = "[نوع]";
		((DataGridViewColumn)typeDataGridViewTextBoxColumn).Name = "typeDataGridViewTextBoxColumn";
		((DataGridViewBand)typeDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)typeDataGridViewTextBoxColumn).Width = 61;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).DataPropertyName = "Name";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).HeaderText = "[عنوان]";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).Name = "nameDataGridViewTextBoxColumn";
		((DataGridViewBand)nameDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)portDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)portDataGridViewTextBoxColumn).DataPropertyName = "Port";
		((DataGridViewColumn)portDataGridViewTextBoxColumn).HeaderText = "[پورت]";
		((DataGridViewColumn)portDataGridViewTextBoxColumn).Name = "portDataGridViewTextBoxColumn";
		((DataGridViewBand)portDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)portDataGridViewTextBoxColumn).Width = 70;
		((Control)ucSearchBox1).Anchor = (AnchorStyles)13;
		((Control)ucSearchBox1).BackColor = Color.FromArgb(249, 250, 255);
		((Control)ucSearchBox1).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucSearchBox1.IsSaerchBox = true;
		((Control)ucSearchBox1).Location = new Point(36, 22);
		((Control)ucSearchBox1).Margin = new Padding(4, 5, 4, 5);
		((Control)ucSearchBox1).Name = "ucSearchBox1";
		((Control)ucSearchBox1).RightToLeft = (RightToLeft)1;
		ucSearchBox1.SelectionLength = 0;
		ucSearchBox1.SelectionStart = 0;
		((Control)ucSearchBox1).Size = new Size(382, 44);
		((Control)ucSearchBox1).TabIndex = 167;
		ucSearchBox1.OnTextChanged += ucSearchBox1_OnTextChanged;
		ucSearchBox1.OnKeyDown += ucSearchBox1_OnKeyDown;
		((Control)ucGroupBox1).BackColor = Color.Transparent;
		ucGroupBox1.DisplayFont = new Font("PeydaWeb Black", 14.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)ucGroupBox1).Dock = (DockStyle)5;
		((Control)ucGroupBox1).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucGroupBox1.HeadHeight = 40;
		ucGroupBox1.IsShowHeader = false;
		((Control)ucGroupBox1).Location = new Point(0, 0);
		((Control)ucGroupBox1).Margin = new Padding(4, 5, 4, 5);
		((Control)ucGroupBox1).Name = "ucGroupBox1";
		((Control)ucGroupBox1).RightToLeft = (RightToLeft)1;
		((Control)ucGroupBox1).Size = new Size(472, 450);
		((Control)ucGroupBox1).TabIndex = 0;
		ucGroupBox1.Title = "        عنوان";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(708, 450);
		((Control)this).Controls.Add((Control)(object)pnlService);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmPortStore";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[لیست پورت ها]";
		((Form)this).FormClosed += new FormClosedEventHandler(frmPortStore_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(frmPortStore_KeyDown);
		((Control)panel1).ResumeLayout(false);
		((Control)panel3).ResumeLayout(false);
		((ISupportInitialize)pictureBox5).EndInit();
		((ISupportInitialize)pictureBox8).EndInit();
		((ISupportInitialize)pictureBoxDropDone).EndInit();
		((ISupportInitialize)pictureBox2).EndInit();
		((ISupportInitialize)pictureBox4).EndInit();
		((ISupportInitialize)pictureBox3).EndInit();
		((ISupportInitialize)pictureBox1).EndInit();
		((ISupportInitialize)PortStoreBindingSource).EndInit();
		((Control)pnlService).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((Control)this).ResumeLayout(false);
	}
}

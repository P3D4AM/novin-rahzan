using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class CompletionListBox : ListBox
{
	private bool filtered;

	private string filter = string.Empty;

	private ICodeCompletionProvider provider;

	private ICodeCompletionColumns columns;

	private ArrayList columnWidths;

	private int itemWidth;

	private IPainter painter;

	private HtmlSyntaxPaint syntaxPaint;

	private EventHandler updateSize;

	private ImageList images;

	private Keys[] navKeys;

	protected internal ICodeCompletionColumns Columns => columns;

	public bool Filtered
	{
		get
		{
			return filtered;
		}
		set
		{
			if (filtered != value)
			{
				filtered = value;
				ResetContent(0);
			}
		}
	}

	public string Filter
	{
		get
		{
			return filter;
		}
		set
		{
			if (filter != value)
			{
				filter = value;
				if (filtered)
				{
					ResetContent(0);
				}
			}
		}
	}

	public int ColumnCount => columns.Count;

	public ICodeCompletionProvider Provider
	{
		get
		{
			return provider;
		}
		set
		{
			if (provider != value)
			{
				provider = value;
				ProviderChanged();
			}
		}
	}

	public int ItemWidth => itemWidth;

	public ImageList Images
	{
		get
		{
			return images;
		}
		set
		{
			if (images != value)
			{
				images = value;
				UpdateControlSize();
			}
		}
	}

	public Keys[] NavKeys => navKeys;

	public EventHandler UpdateSize
	{
		get
		{
			return updateSize;
		}
		set
		{
			updateSize = value;
		}
	}

	public CompletionListBox()
	{
		Keys[] array = new Keys[4];
		RuntimeHelpers.InitializeArray(array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
		navKeys = (Keys[])(object)array;
		((ListBox)this)._002Ector();
		columns = new CodeCompletionColumns();
		columnWidths = new ArrayList();
		painter = new GdiPainter();
		painter.Font = ((Control)this).Font;
		syntaxPaint = new HtmlSyntaxPaint(painter, null);
		((ListBox)this).DrawMode = (DrawMode)2;
		((ListBox)this).BorderStyle = (BorderStyle)0;
		((ListBox)this).HorizontalScrollbar = true;
		((ListBox)this).Sorted = true;
	}

	private int MeasureWidth(string text, FontStyle style)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		if (text != null)
		{
			painter.FontStyle = style;
			short[] colorData = null;
			syntaxPaint.PrepareData(0, style, ref text, ref colorData);
			return syntaxPaint.MeasureLine(text, colorData, 0, text.Length);
		}
		return 0;
	}

	private void DrawColumn(Rectangle rect, string text, Color foreColor, FontStyle style)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		if (text != null)
		{
			painter.TextColor = foreColor;
			painter.Opaque = false;
			painter.FontStyle = style;
			short[] colorData = null;
			syntaxPaint.PrepareData(0, style, ref text, ref colorData);
			syntaxPaint.DrawLine(0, text, colorData, rect.Location, rect);
		}
	}

	protected internal int GetIndex()
	{
		return GetIndex(((ListControl)this).SelectedIndex);
	}

	protected internal int GetIndex(int index)
	{
		if ((!filtered && !((ListBox)this).Sorted) || index < 0 || index >= ((ListBox)this).Items.Count)
		{
			return index;
		}
		return ((ListboxItem)((ListBox)this).Items[index]).Index;
	}

	protected virtual void ProviderChanged()
	{
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		((ListBox)this).BeginUpdate();
		try
		{
			((ListBox)this).Items.Clear();
			columnWidths.Clear();
			itemWidth = 0;
			syntaxPaint.Provider = provider;
			if (provider != null)
			{
				for (int i = 0; i < provider.Count; i++)
				{
					string name = provider.GetName(i);
					if (IsFiltered(name))
					{
						((ListBox)this).Items.Add((object)new ListboxItem(name, i));
					}
				}
				for (int j = 0; j < provider.ColumnCount; j++)
				{
					int num = 0;
					if (provider.ColumnVisible(j))
					{
						ICodeCompletionColumn codeCompletionColumn = ((j < columns.Count) ? GetColumn(j) : null);
						for (int k = 0; k < provider.Count; k++)
						{
							if (IsFiltered(provider.Strings[k]))
							{
								num = Math.Max(num, MeasureWidth(provider.GetColumnText(k, j), (FontStyle)((codeCompletionColumn != null) ? ((int)codeCompletionColumn.FontStyle) : 0)));
							}
						}
						num += EditConsts.DefaultColumnSeparator;
					}
					itemWidth += num;
					columnWidths.Add(num);
				}
			}
			UpdateControlSize();
		}
		finally
		{
			((ListBox)this).EndUpdate();
		}
	}

	protected override void OnFontChanged(EventArgs e)
	{
		((ListBox)this).OnFontChanged(e);
		painter.Clear();
		painter.Font = ((Control)this).Font;
		UpdateControlSize();
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		if (Array.IndexOf(EditConsts.NavKeys, e.KeyCode) >= 0 && Array.IndexOf(navKeys, e.KeyCode) < 0)
		{
			e.Handled = (int)e.KeyCode != 9;
		}
		((Control)this).OnKeyDown(e);
	}

	protected virtual bool IsFiltered(string s)
	{
		if (filtered && filter != null && !(filter == string.Empty))
		{
			return s.ToLower(CultureInfo.InvariantCulture).StartsWith(filter.ToLower(CultureInfo.InvariantCulture));
		}
		return true;
	}

	protected internal void UpdateControlSize()
	{
		int num = painter.FontHeight + EditConsts.DefaultRowSeparator;
		if (images != null)
		{
			num = Math.Max(num, images.ImageSize.Height);
		}
		((ListBox)this).ItemHeight = num;
		((ListBox)this).HorizontalExtent = ((provider != null && provider.ColumnCount != 0) ? itemWidth : ((images != null) ? (images.ImageSize.Width + EditConsts.DefaultColumnSeparator) : 0));
		if (UpdateSize != null)
		{
			UpdateSize(this, EventArgs.Empty);
		}
	}

	protected override void OnMeasureItem(MeasureItemEventArgs e)
	{
		((ListBox)this).OnMeasureItem(e);
		if (images != null)
		{
			e.ItemHeight = Math.Max(e.ItemHeight, images.ImageSize.Height);
		}
	}

	protected override void OnDrawItem(DrawItemEventArgs e)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Invalid comparison between Unknown and I4
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Invalid comparison between Unknown and I4
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Invalid comparison between Unknown and I4
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		int index = GetIndex(e.Index);
		if (provider == null || index < 0 || index >= provider.Count)
		{
			((ListBox)this).OnDrawItem(e);
			return;
		}
		bool flag = (e.State & 4) > 0;
		bool flag2 = (e.State & 0x10) != 0 && (e.State & 0x200) == 0;
		bool flag3 = (e.State & 1) > 0;
		Size size = ((images != null) ? new Size(images.ImageSize.Width + EditConsts.DefaultColumnSeparator, images.ImageSize.Height) : new Size(0, 0));
		if (images != null)
		{
			int imageIndex = ((CodeCompletionProvider)provider).GetImageIndex(index);
			if (imageIndex >= 0 && imageIndex < images.Images.Count)
			{
				images.Draw(e.Graphics, e.Bounds.Left, e.Bounds.Top + (e.Bounds.Height - size.Height) / 2, imageIndex);
			}
			e.Graphics.TranslateTransform((float)size.Width, 0f);
		}
		e.DrawBackground();
		if (flag2)
		{
			e.DrawFocusRectangle();
		}
		int num = e.Bounds.Left + size.Width;
		painter.BeginPaint(e.Graphics);
		try
		{
			for (int i = 0; i < provider.ColumnCount; i++)
			{
				ICodeCompletionColumn codeCompletionColumn = ((i < columns.Count) ? GetColumn(i) : null);
				if ((codeCompletionColumn == null || codeCompletionColumn.Visible) && provider.ColumnVisible(i))
				{
					Color foreColor = codeCompletionColumn?.ForeColor ?? ((Control)this).ForeColor;
					int num2 = (int)columnWidths[i];
					Rectangle rect = new Rectangle(num, e.Bounds.Top, num2, e.Bounds.Height);
					if (flag)
					{
						foreColor = EditConsts.DefaultDisabledForeColor;
					}
					else if (flag3)
					{
						foreColor = EditConsts.DefaultHighlightForeColor;
					}
					DrawColumn(rect, provider.GetColumnText(index, i), foreColor, (FontStyle)((codeCompletionColumn != null) ? ((int)codeCompletionColumn.FontStyle) : 0));
					num += num2;
				}
			}
		}
		finally
		{
			painter.EndPaint();
		}
	}

	public void ResetContent(int index)
	{
		((ListBox)this).BeginUpdate();
		try
		{
			ResetContent();
			if (index >= 0 && index < ((ListBox)this).Items.Count)
			{
				((ListControl)this).SelectedIndex = index;
			}
		}
		finally
		{
			((ListBox)this).EndUpdate();
		}
	}

	public void ResetContent()
	{
		ProviderChanged();
	}

	public ICodeCompletionColumn GetColumn(int index)
	{
		return columns[index];
	}

	public void ClearColumns()
	{
		columns.Clear();
	}

	public ICodeCompletionColumn AddColumn()
	{
		ICodeCompletionColumn codeCompletionColumn = new CodeCompletionColumn();
		columns.Add(codeCompletionColumn);
		return codeCompletionColumn;
	}

	public ICodeCompletionColumn InsertColumn(int index)
	{
		ICodeCompletionColumn codeCompletionColumn = new CodeCompletionColumn();
		columns.Insert(index, codeCompletionColumn);
		return codeCompletionColumn;
	}

	public void RemoveColumnAt(int index)
	{
		columns.RemoveAt(index);
	}
}

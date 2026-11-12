using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

public class DataGridViewComboBoxExCell : DataGridViewTextBoxCell
{
	private BindingContext _BindingContext = new BindingContext();

	public override Type EditType => typeof(DataGridViewComboBoxExEditingControl);

	public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Expected O, but got Unknown
		((DataGridViewCell)this).DetachEditingControl();
		((DataGridViewTextBoxCell)this).InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
		DataGridViewComboBoxExEditingControl dataGridViewComboBoxExEditingControl = (DataGridViewComboBoxExEditingControl)(object)((DataGridViewElement)this).DataGridView.EditingControl;
		DataGridViewComboBoxExColumn dataGridViewComboBoxExColumn = (DataGridViewComboBoxExColumn)(object)((DataGridViewCell)this).OwningColumn;
		ComboBoxEx comboBoxEx = dataGridViewComboBoxExColumn.ComboBoxEx;
		((ComboBox)dataGridViewComboBoxExEditingControl).DropDownStyle = ((ComboBox)comboBoxEx).DropDownStyle;
		((ComboBox)dataGridViewComboBoxExEditingControl).AutoCompleteCustomSource = ((ComboBox)comboBoxEx).AutoCompleteCustomSource;
		((ComboBox)dataGridViewComboBoxExEditingControl).AutoCompleteMode = ((ComboBox)comboBoxEx).AutoCompleteMode;
		((ComboBox)dataGridViewComboBoxExEditingControl).AutoCompleteSource = ((ComboBox)comboBoxEx).AutoCompleteSource;
		((ComboBox)dataGridViewComboBoxExEditingControl).DrawMode = ((ComboBox)comboBoxEx).DrawMode;
		((ComboBox)dataGridViewComboBoxExEditingControl).DropDownHeight = ((ComboBox)comboBoxEx).DropDownHeight;
		((Control)dataGridViewComboBoxExEditingControl).Enabled = ((Control)comboBoxEx).Enabled;
		((ComboBox)dataGridViewComboBoxExEditingControl).FlatStyle = ((ComboBox)comboBoxEx).FlatStyle;
		dataGridViewComboBoxExEditingControl.FocusCuesEnabled = comboBoxEx.FocusCuesEnabled;
		((ListControl)dataGridViewComboBoxExEditingControl).FormatString = ((ListControl)comboBoxEx).FormatString;
		((ListControl)dataGridViewComboBoxExEditingControl).FormattingEnabled = ((ListControl)comboBoxEx).FormattingEnabled;
		dataGridViewComboBoxExEditingControl.Images = comboBoxEx.Images;
		((Control)dataGridViewComboBoxExEditingControl).ImeMode = ((Control)comboBoxEx).ImeMode;
		((ComboBox)dataGridViewComboBoxExEditingControl).IntegralHeight = ((ComboBox)comboBoxEx).IntegralHeight;
		dataGridViewComboBoxExEditingControl.IsStandalone = comboBoxEx.IsStandalone;
		((ComboBox)dataGridViewComboBoxExEditingControl).ItemHeight = ((ComboBox)comboBoxEx).ItemHeight;
		((ComboBox)dataGridViewComboBoxExEditingControl).MaxDropDownItems = ((ComboBox)comboBoxEx).MaxDropDownItems;
		((ComboBox)dataGridViewComboBoxExEditingControl).MaxLength = ((ComboBox)comboBoxEx).MaxLength;
		dataGridViewComboBoxExEditingControl.RenderBorder = comboBoxEx.RenderBorder;
		((Control)dataGridViewComboBoxExEditingControl).RightToLeft = ((Control)comboBoxEx).RightToLeft;
		dataGridViewComboBoxExEditingControl.Style = comboBoxEx.Style;
		dataGridViewComboBoxExEditingControl.WatermarkBehavior = comboBoxEx.WatermarkBehavior;
		dataGridViewComboBoxExEditingControl.WatermarkColor = comboBoxEx.WatermarkColor;
		dataGridViewComboBoxExEditingControl.WatermarkEnabled = comboBoxEx.WatermarkEnabled;
		dataGridViewComboBoxExEditingControl.WatermarkFont = comboBoxEx.WatermarkFont;
		dataGridViewComboBoxExEditingControl.WatermarkText = comboBoxEx.WatermarkText;
		((ComboBox)dataGridViewComboBoxExEditingControl).DataSource = ((ComboBox)comboBoxEx).DataSource;
		if (!(((ComboBox)dataGridViewComboBoxExEditingControl).DataSource is List<string>))
		{
			((ListControl)dataGridViewComboBoxExEditingControl).ValueMember = ((ListControl)comboBoxEx).ValueMember;
		}
		((ListControl)dataGridViewComboBoxExEditingControl).DisplayMember = ((ListControl)comboBoxEx).DisplayMember;
		if (((ComboBox)dataGridViewComboBoxExEditingControl).DataSource == null)
		{
			object[] array = new object[comboBoxEx.Items.Count];
			comboBoxEx.Items.CopyTo(array, 0);
			dataGridViewComboBoxExEditingControl.Items.Clear();
			dataGridViewComboBoxExEditingControl.Items.AddRange(array);
			((Control)dataGridViewComboBoxExEditingControl).Text = Convert.ToString(initialFormattedValue);
		}
		else
		{
			object dataSourceDisplayValue = GetDataSourceDisplayValue(dataGridViewComboBoxExColumn, comboBoxEx, initialFormattedValue);
			((Control)dataGridViewComboBoxExEditingControl).Text = Convert.ToString(dataSourceDisplayValue);
		}
		((Control)dataGridViewComboBoxExEditingControl).Click += Click;
		((ComboBox)dataGridViewComboBoxExEditingControl).DrawItem += new DrawItemEventHandler(DrawItem);
		dataGridViewComboBoxExEditingControl.DropDownChange += DropDownChange;
		((ComboBox)dataGridViewComboBoxExEditingControl).MeasureItem += new MeasureItemEventHandler(MeasureItem);
	}

	public override void DetachEditingControl()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		if (((DataGridViewElement)this).DataGridView != null && ((DataGridViewElement)this).DataGridView.EditingControl != null && ((DataGridViewElement)this).DataGridView.EditingControl is DataGridViewComboBoxExEditingControl dataGridViewComboBoxExEditingControl)
		{
			((Control)dataGridViewComboBoxExEditingControl).Click -= Click;
			((ComboBox)dataGridViewComboBoxExEditingControl).DrawItem -= new DrawItemEventHandler(DrawItem);
			dataGridViewComboBoxExEditingControl.DropDownChange -= DropDownChange;
			((ComboBox)dataGridViewComboBoxExEditingControl).MeasureItem -= new MeasureItemEventHandler(MeasureItem);
		}
		((DataGridViewTextBoxCell)this).DetachEditingControl();
	}

	private void Click(object sender, EventArgs e)
	{
		((DataGridViewComboBoxExColumn)(object)((DataGridViewCell)this).OwningColumn).DoComboBoxEx_Click(sender, e);
	}

	private void DrawItem(object sender, DrawItemEventArgs e)
	{
		((DataGridViewComboBoxExColumn)(object)((DataGridViewCell)this).OwningColumn).DoComboBoxEx_DrawItem(sender, e);
	}

	private void DropDownChange(object sender, bool expanded)
	{
		((DataGridViewComboBoxExColumn)(object)((DataGridViewCell)this).OwningColumn).DoComboBoxEx_DropDownChange(sender, new DropDownChangeEventArgs(((DataGridViewCell)this).RowIndex, ((DataGridViewCell)this).ColumnIndex, expanded));
	}

	private void MeasureItem(object sender, MeasureItemEventArgs e)
	{
		((DataGridViewComboBoxExColumn)(object)((DataGridViewCell)this).OwningColumn).DoComboBoxEx_MeasureItem(sender, e);
	}

	public override void PositionEditingControl(bool setLocation, bool setSize, Rectangle cellBounds, Rectangle cellClip, DataGridViewCellStyle cellStyle, bool singleVerticalBorderAdded, bool singleHorizontalBorderAdded, bool isFirstDisplayedColumn, bool isFirstDisplayedRow)
	{
		Rectangle editingControlBounds = ((DataGridViewCell)this).PositionEditingPanel(cellBounds, cellClip, cellStyle, singleVerticalBorderAdded, singleHorizontalBorderAdded, isFirstDisplayedColumn, isFirstDisplayedRow);
		editingControlBounds = GetAdjustedEditingControlBounds(editingControlBounds, cellStyle);
		((DataGridViewElement)this).DataGridView.EditingControl.Location = new Point(editingControlBounds.X, editingControlBounds.Y);
		((DataGridViewElement)this).DataGridView.EditingControl.Size = new Size(editingControlBounds.Width, editingControlBounds.Height);
	}

	private Rectangle GetAdjustedEditingControlBounds(Rectangle editingControlBounds, DataGridViewCellStyle cellStyle)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Invalid comparison between Unknown and I4
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Invalid comparison between Unknown and I4
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Invalid comparison between Unknown and I4
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Invalid comparison between Unknown and I4
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Invalid comparison between Unknown and I4
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Invalid comparison between Unknown and I4
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Invalid comparison between Unknown and I4
		editingControlBounds.X++;
		editingControlBounds.Y++;
		editingControlBounds.Width = Math.Max(0, editingControlBounds.Width - 2);
		editingControlBounds.Height = Math.Max(0, editingControlBounds.Height - 2);
		Rectangle cellBounds = GetCellBounds(editingControlBounds);
		if (cellBounds.Height < editingControlBounds.Height)
		{
			DataGridViewContentAlignment alignment = cellStyle.Alignment;
			if ((int)alignment <= 64)
			{
				if ((int)alignment == 16 || (int)alignment == 32 || (int)alignment == 64)
				{
					editingControlBounds.Y += (editingControlBounds.Height - cellBounds.Height) / 2;
				}
			}
			else if ((int)alignment == 256 || (int)alignment == 512 || (int)alignment == 1024)
			{
				editingControlBounds.Y += editingControlBounds.Height - cellBounds.Height;
			}
		}
		editingControlBounds.Width = Math.Max(1, editingControlBounds.Width);
		editingControlBounds.Height = Math.Max(1, cellBounds.Height);
		return editingControlBounds;
	}

	protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
	{
		if (((DataGridViewElement)this).DataGridView == null)
		{
			return new Size(-1, -1);
		}
		Size preferredSize = ((DataGridViewTextBoxCell)this).GetPreferredSize(graphics, cellStyle, rowIndex, constraintSize);
		if (((DataGridViewCell)this).OwningColumn is DataGridViewComboBoxExColumn { ComboBoxEx: var comboBoxEx })
		{
			preferredSize.Height = ((Control)comboBoxEx).Height;
			if (constraintSize.Width == 0)
			{
				Rectangle cellDisplayRectangle = ((DataGridViewElement)this).DataGridView.GetCellDisplayRectangle(((DataGridViewCell)this).ColumnIndex, rowIndex, true);
				preferredSize.Width += comboBoxEx.GetThumbRect(cellDisplayRectangle).Width;
			}
		}
		return preferredSize;
	}

	public override object ParseFormattedValue(object formattedValue, DataGridViewCellStyle cellStyle, TypeConverter formattedValueTypeConverter, TypeConverter valueTypeConverter)
	{
		DataGridViewComboBoxExColumn dataGridViewComboBoxExColumn = (DataGridViewComboBoxExColumn)(object)((DataGridViewCell)this).OwningColumn;
		ComboBoxEx comboBoxEx = dataGridViewComboBoxExColumn.ComboBoxEx;
		if (((ComboBox)comboBoxEx).DataSource != null)
		{
			return GetDataSourceValue(dataGridViewComboBoxExColumn, comboBoxEx, formattedValue);
		}
		return ((DataGridViewCell)this).ParseFormattedValue(formattedValue, cellStyle, formattedValueTypeConverter, valueTypeConverter);
	}

	protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Invalid comparison between Unknown and I4
		if (((DataGridViewElement)this).DataGridView == null)
		{
			return;
		}
		if (PartsSet(paintParts, (DataGridViewPaintParts)2))
		{
			((DataGridViewCell)this).PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
		}
		if (!PartsSet(paintParts, (DataGridViewPaintParts)1))
		{
			return;
		}
		Rectangle backBounds = GetBackBounds(cellBounds, advancedBorderStyle);
		if (backBounds.Height <= 0 || backBounds.Width <= 0)
		{
			return;
		}
		DataGridViewComboBoxExColumn dataGridViewComboBoxExColumn = (DataGridViewComboBoxExColumn)(object)((DataGridViewCell)this).OwningColumn;
		Bitmap cellBitmap = dataGridViewComboBoxExColumn.GetCellBitmap(cellBounds);
		if (cellBitmap != null)
		{
			Graphics val = Graphics.FromImage((Image)(object)cellBitmap);
			try
			{
				PaintCellBackground(val, cellStyle, backBounds);
				PaintCellContent(val, cellBounds, rowIndex, value, cellStyle, paintParts, cellBitmap);
				graphics.DrawImageUnscaledAndClipped((Image)(object)cellBitmap, backBounds);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			if (((DataGridViewElement)this).DataGridView.ShowCellErrors && (paintParts & 0x10) == 16)
			{
				((DataGridViewCell)this).PaintErrorIcon(graphics, clipBounds, cellBounds, errorText);
			}
		}
	}

	private void PaintCellBackground(Graphics g, DataGridViewCellStyle cellStyle, Rectangle rBack)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		Rectangle rectangle = rBack;
		rectangle.Location = new Point(0, 0);
		if (((DataGridViewElement)this).DataGridView is DataGridViewX dataGridViewX && ((Control)dataGridViewX).Enabled && ((DataGridViewCell)this).Selected)
		{
			Office2007ButtonItemPainter.PaintBackground(g, dataGridViewX.ButtonStateColorTable, rectangle, RoundRectangleShapeDescriptor.RectangleShape, isDefault: false, paintBorder: false);
			return;
		}
		Color color = (((DataGridViewCell)this).Selected ? cellStyle.SelectionBackColor : cellStyle.BackColor);
		Brush val = (Brush)new SolidBrush(color);
		try
		{
			g.FillRectangle(val, rectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void PaintCellContent(Graphics g, Rectangle cellBounds, int rowIndex, object value, DataGridViewCellStyle cellStyle, DataGridViewPaintParts paintParts, Bitmap bm)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		DataGridViewComboBoxExColumn dataGridViewComboBoxExColumn = (DataGridViewComboBoxExColumn)(object)((DataGridViewCell)this).OwningColumn;
		ComboBoxEx comboBoxEx = dataGridViewComboBoxExColumn.ComboBoxEx;
		Point currentCellAddress = ((DataGridViewElement)this).DataGridView.CurrentCellAddress;
		if ((currentCellAddress.X != ((DataGridViewCell)this).ColumnIndex || currentCellAddress.Y != rowIndex || ((DataGridViewElement)this).DataGridView.EditingControl == null) && rowIndex < ((DataGridViewElement)this).DataGridView.RowCount && PartsSet(paintParts, (DataGridViewPaintParts)8))
		{
			cellBounds.X = 0;
			cellBounds.Y = 0;
			cellBounds.Width -= ((DataGridViewColumn)dataGridViewComboBoxExColumn).DividerWidth + 1;
			cellBounds.Height--;
			if (((ComboBox)comboBoxEx).DataSource == null)
			{
				((ListControl)comboBoxEx).DisplayMember = "Text";
				((ListControl)comboBoxEx).ValueMember = null;
			}
			else
			{
				value = GetDataSourceDisplayValue(dataGridViewComboBoxExColumn, comboBoxEx, value);
				((Control)comboBoxEx).BindingContext = _BindingContext;
			}
			((Control)comboBoxEx).Font = cellStyle.Font;
			((Control)comboBoxEx).ForeColor = cellStyle.ForeColor;
			((Control)comboBoxEx).BackColor = cellStyle.BackColor;
			string s = (((Control)comboBoxEx).Text = ((value == Convert.DBNull || string.IsNullOrEmpty(Convert.ToString(value))) ? null : Convert.ToString(value)));
			dataGridViewComboBoxExColumn.OnBeforeCellPaint(rowIndex, ((DataGridViewCell)this).ColumnIndex);
			Rectangle adjustedEditingControlBounds = GetAdjustedEditingControlBounds(cellBounds, cellStyle);
			if (!dataGridViewComboBoxExColumn.DisplayControlForCurrentCellOnly)
			{
				DrawControl(comboBoxEx, adjustedEditingControlBounds, bm, g, dataGridViewComboBoxExColumn, s);
			}
			else
			{
				DrawText(comboBoxEx, adjustedEditingControlBounds, g, s);
			}
		}
	}

	private void DrawControl(ComboBoxEx di, Rectangle r, Bitmap bm, Graphics g, DataGridViewComboBoxExColumn oc, string s)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		Rectangle thumbRect = di.GetThumbRect(new Rectangle(0, 0, r.Width, r.Height));
		if (!thumbRect.IsEmpty)
		{
			if (MustRenderVisibleControl(((ComboBox)di).DropDownStyle))
			{
				((Control)di).Location = ((Control)((DataGridViewElement)oc).DataGridView).Location;
				if (((Control)di).Parent == null)
				{
					Form val = ((Control)((DataGridViewElement)oc).DataGridView).FindForm();
					if (val != null)
					{
						((Control)di).Parent = (Control)(object)val;
					}
				}
				((Control)di).SendToBack();
				((Control)di).Visible = true;
			}
			Bitmap val2 = new Bitmap((Image)(object)bm);
			try
			{
				((Control)di).Bounds = r;
				((Control)di).DrawToBitmap(val2, r);
				thumbRect.X += r.X;
				thumbRect.Y += r.Y;
				g.DrawImage((Image)(object)val2, thumbRect, thumbRect, (GraphicsUnit)2);
				if (thumbRect.Left < r.Right)
				{
					r.Width -= r.Right - thumbRect.Left - 3;
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			((Control)di).Visible = false;
		}
		DrawText(di, r, g, s);
	}

	private void DrawText(ComboBoxEx di, Rectangle r, Graphics g, string s)
	{
		r.Inflate(-2, 0);
		TextDrawing.DrawString(g, s, ((Control)di).Font, ((Control)di).ForeColor, r, eTextFormat.EndEllipsis | eTextFormat.NoPrefix | eTextFormat.VerticalCenter);
	}

	private object GetDataSourceDisplayValue(DataGridViewComboBoxExColumn oc, ComboBoxEx di, object key)
	{
		if (key != null && !string.IsNullOrEmpty(((ListControl)di).ValueMember) && !string.IsNullOrEmpty(((ListControl)di).DisplayMember))
		{
			CurrencyManager currencyManager = GetCurrencyManager(oc, di);
			if (currencyManager != null)
			{
				string text = key.ToString();
				for (int i = 0; i < currencyManager.List.Count; i++)
				{
					if (currencyManager.List[i] is DataRowView)
					{
						DataRowView dataRowView = (DataRowView)currencyManager.List[i];
						string value = dataRowView[((ListControl)di).ValueMember].ToString();
						if (text.Equals(value))
						{
							return dataRowView[((ListControl)di).DisplayMember];
						}
					}
					else if (currencyManager.List[i] is string)
					{
						if (text.Equals(currencyManager.List[i]))
						{
							return key;
						}
					}
					else
					{
						object obj = currencyManager.List[i];
						object value2 = obj.GetType().GetProperty(((ListControl)di).ValueMember).GetValue(obj, null);
						if (key.Equals(value2))
						{
							return obj.GetType().GetProperty(((ListControl)di).DisplayMember).GetValue(obj, null);
						}
					}
				}
			}
		}
		if (key != Convert.DBNull)
		{
			return key;
		}
		return "";
	}

	private object GetDataSourceValue(DataGridViewComboBoxExColumn oc, ComboBoxEx di, object key)
	{
		if (key != null && !string.IsNullOrEmpty(((ListControl)di).ValueMember) && !string.IsNullOrEmpty(((ListControl)di).DisplayMember))
		{
			CurrencyManager currencyManager = GetCurrencyManager(oc, di);
			if (currencyManager != null)
			{
				string text = key.ToString();
				for (int i = 0; i < currencyManager.List.Count; i++)
				{
					if (currencyManager.List[i] is DataRowView)
					{
						DataRowView dataRowView = (DataRowView)currencyManager.List[i];
						string value = dataRowView[((ListControl)di).DisplayMember].ToString();
						if (text.Equals(value))
						{
							return dataRowView[((ListControl)di).ValueMember];
						}
					}
					else if (currencyManager.List[i] is string)
					{
						if (text.Equals(currencyManager.List[i]))
						{
							return key;
						}
					}
					else
					{
						object obj = currencyManager.List[i];
						object value2 = obj.GetType().GetProperty(((ListControl)di).DisplayMember).GetValue(obj, null);
						if (value2 is string && text.Equals(value2))
						{
							return obj.GetType().GetProperty(((ListControl)di).ValueMember).GetValue(obj, null);
						}
					}
				}
			}
		}
		return key;
	}

	private CurrencyManager GetCurrencyManager(DataGridViewComboBoxExColumn oc, ComboBoxEx di)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		if (oc.CurrencyManager == null && ((DataGridViewElement)this).DataGridView != null && ((Control)((DataGridViewElement)this).DataGridView).BindingContext != null && ((ComboBox)di).DataSource != null && ((ComboBox)di).DataSource != Convert.DBNull)
		{
			oc.CurrencyManager = (CurrencyManager)((Control)((DataGridViewElement)this).DataGridView).BindingContext[((ComboBox)di).DataSource];
		}
		return oc.CurrencyManager;
	}

	private bool MustRenderVisibleControl(ComboBoxStyle style)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		if ((int)style == 2)
		{
			return true;
		}
		OperatingSystem oSVersion = Environment.OSVersion;
		if (oSVersion.Platform != PlatformID.Win32Windows)
		{
			if (oSVersion.Platform == PlatformID.Win32NT)
			{
				return oSVersion.Version.Major < 6;
			}
			return false;
		}
		return true;
	}

	protected override void OnMouseEnter(int rowIndex)
	{
		DataGridViewComboBoxExColumn dataGridViewComboBoxExColumn = (DataGridViewComboBoxExColumn)(object)((DataGridViewCell)this).OwningColumn;
		ComboBoxEx comboBoxEx = dataGridViewComboBoxExColumn.ComboBoxEx;
		if ((uint)((DataGridViewCell)this).RowIndex < ((DataGridViewElement)this).DataGridView.Rows.Count)
		{
			if (((ComboBox)comboBoxEx).DataSource != null)
			{
				((DataGridViewCell)this).ToolTipText = Convert.ToString(GetDataSourceDisplayValue(dataGridViewComboBoxExColumn, comboBoxEx, ((DataGridViewCell)this).Value));
			}
			else
			{
				((DataGridViewCell)this).ToolTipText = Convert.ToString(((DataGridViewCell)this).Value);
			}
		}
		((DataGridViewCell)this).OnMouseEnter(rowIndex);
	}

	private Rectangle GetBackBounds(Rectangle cellBounds, DataGridViewAdvancedBorderStyle advancedBorderStyle)
	{
		DataGridViewComboBoxExColumn dataGridViewComboBoxExColumn = (DataGridViewComboBoxExColumn)(object)((DataGridViewCell)this).OwningColumn;
		Rectangle rectangle = ((DataGridViewCell)this).BorderWidths(advancedBorderStyle);
		cellBounds.Offset(rectangle.X, rectangle.Y);
		cellBounds.Width -= rectangle.Right;
		cellBounds.Height -= rectangle.Bottom;
		if (((DataGridViewCell)this).Selected)
		{
			cellBounds.Width += ((DataGridViewColumn)dataGridViewComboBoxExColumn).DividerWidth;
		}
		return cellBounds;
	}

	private Rectangle GetCellBounds(Rectangle cellBounds)
	{
		DataGridViewComboBoxExColumn dataGridViewComboBoxExColumn = (DataGridViewComboBoxExColumn)(object)((DataGridViewCell)this).OwningColumn;
		Size size = ((Control)dataGridViewComboBoxExColumn.ComboBoxEx).Size;
		cellBounds.Location = new Point(1, 1);
		cellBounds.Width -= ((DataGridViewColumn)dataGridViewComboBoxExColumn).DividerWidth;
		cellBounds.Height = Math.Min(size.Height, cellBounds.Height);
		return cellBounds;
	}

	private bool PartsSet(DataGridViewPaintParts paintParts, DataGridViewPaintParts parts)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return (DataGridViewPaintParts)(paintParts & parts) == parts;
	}
}

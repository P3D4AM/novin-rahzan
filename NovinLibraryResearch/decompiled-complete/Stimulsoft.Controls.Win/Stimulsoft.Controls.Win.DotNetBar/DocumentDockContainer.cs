using System;
using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
[TypeConverter(typeof(DocumentDockContainerConverter))]
public class DocumentDockContainer : DocumentBaseContainer
{
	private struct DocumentMeasureInfo
	{
		public int TotalSize;

		public int VisibleCount;
	}

	private eOrientation m_Orientation;

	private DocumentBaseContainerCollection m_Documents = new DocumentBaseContainerCollection();

	private int m_SplitterSize = 3;

	private Size m_MinimumSize = Size.Empty;

	private bool m_RecordDocumentSize;

	public override bool Visible
	{
		get
		{
			foreach (DocumentBaseContainer document in m_Documents)
			{
				if (document.Visible)
				{
					return true;
				}
			}
			return false;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public eOrientation Orientation
	{
		get
		{
			return m_Orientation;
		}
		set
		{
			if (m_Orientation != value)
			{
				m_Orientation = value;
				OnOrientationChanged();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DocumentBaseContainerCollection Documents => m_Documents;

	protected internal override Size MinimumSize => m_MinimumSize;

	[Browsable(true)]
	[DefaultValue(3)]
	[Description("Indicates the splitter size between the documents docking inside the container.")]
	[Category("Layout")]
	public virtual int SplitterSize
	{
		get
		{
			return m_SplitterSize;
		}
		set
		{
			m_SplitterSize = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool RecordDocumentSize
	{
		get
		{
			return m_RecordDocumentSize;
		}
		set
		{
			m_RecordDocumentSize = value;
		}
	}

	public DocumentDockContainer(DocumentBaseContainer[] documents, eOrientation orientation)
	{
		m_Orientation = orientation;
		m_Documents.Owner = this;
		m_Documents.DocumentAdded += DocumentAdded;
		m_Documents.DocumentRemoved += DocumentRemoved;
		if (documents != null)
		{
			foreach (DocumentBaseContainer tab in documents)
			{
				m_Documents.Add(tab);
			}
		}
	}

	public DocumentDockContainer()
		: this(null, eOrientation.Horizontal)
	{
	}

	public override void Layout(Rectangle bounds)
	{
		if (LayoutInternal(bounds))
		{
			SetDisplayBounds(bounds);
		}
		else
		{
			SetDisplayBounds(Rectangle.Empty);
		}
	}

	private DocumentMeasureInfo MeasureDocuments(Rectangle bounds)
	{
		DocumentMeasureInfo result = default(DocumentMeasureInfo);
		if (m_Documents.Count == 0)
		{
			return result;
		}
		int num = ((m_Orientation == eOrientation.Horizontal) ? bounds.Width : bounds.Height);
		int num2 = 0;
		int[] array = new int[m_Documents.Count];
		bool flag = false;
		int num3 = m_Documents.Count;
		do
		{
			num2 = (num - (num3 - 1) * m_SplitterSize) / num3;
			flag = false;
			for (int i = 0; i < m_Documents.Count; i++)
			{
				if (array[i] == 0)
				{
					DocumentBaseContainer documentBaseContainer = m_Documents[i];
					if (m_Orientation == eOrientation.Horizontal && documentBaseContainer.LayoutBounds.Width == 0 && documentBaseContainer.MinimumSize.Width > num2)
					{
						array[i] = documentBaseContainer.MinimumSize.Width;
						flag = true;
						num -= array[i];
						num3--;
						break;
					}
					if (m_Orientation == eOrientation.Vertical && documentBaseContainer.LayoutBounds.Height == 0 && documentBaseContainer.MinimumSize.Height > num2)
					{
						array[i] = documentBaseContainer.MinimumSize.Height;
						flag = true;
						num -= array[i];
						num3--;
						break;
					}
				}
			}
		}
		while (flag && num3 > 0);
		int num4 = 0;
		int num5 = 0;
		for (int j = 0; j < m_Documents.Count; j++)
		{
			DocumentBaseContainer documentBaseContainer2 = m_Documents[j];
			if (m_Orientation == eOrientation.Horizontal && documentBaseContainer2.LayoutBounds.Width == 0)
			{
				documentBaseContainer2.SetLayoutBounds(new Rectangle(0, 0, (array[j] > 0) ? array[j] : num2, bounds.Height));
			}
			else if (m_Orientation == eOrientation.Vertical && documentBaseContainer2.LayoutBounds.Height == 0)
			{
				documentBaseContainer2.SetLayoutBounds(new Rectangle(0, 0, bounds.Width, (array[j] > 0) ? array[j] : num2));
			}
			if (documentBaseContainer2.Visible)
			{
				num4 += ((m_Orientation == eOrientation.Horizontal) ? documentBaseContainer2.LayoutBounds.Width : documentBaseContainer2.LayoutBounds.Height) + m_SplitterSize;
				num5++;
			}
		}
		if (num5 > 0)
		{
			num4 -= m_SplitterSize;
		}
		result.TotalSize = num4;
		result.VisibleCount = num5;
		return result;
	}

	private bool AdjustLayoutBoundsForMinimumSize(float m, Rectangle layoutBounds)
	{
		bool flag = false;
		foreach (DocumentBaseContainer document in m_Documents)
		{
			if (!document.Visible)
			{
				continue;
			}
			Rectangle rectangle;
			if (m_Orientation == eOrientation.Horizontal)
			{
				rectangle = new Rectangle(layoutBounds.X, layoutBounds.Y, (int)((float)document.LayoutBounds.Width * m), layoutBounds.Height);
				if (rectangle.Width < document.MinimumSize.Width)
				{
					flag = true;
					break;
				}
			}
			else
			{
				rectangle = new Rectangle(layoutBounds.X, layoutBounds.Y, layoutBounds.Width, (int)((float)document.LayoutBounds.Height * m));
				if (rectangle.Height < document.MinimumSize.Height)
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			foreach (DocumentBaseContainer document2 in m_Documents)
			{
				document2.SetLayoutBounds(Rectangle.Empty);
			}
		}
		return flag;
	}

	private bool LayoutInternal(Rectangle bounds)
	{
		if (m_Documents.Count == 0)
		{
			return false;
		}
		int num = ((m_Orientation == eOrientation.Horizontal) ? bounds.Width : bounds.Height);
		DocumentMeasureInfo documentMeasureInfo = MeasureDocuments(bounds);
		if (documentMeasureInfo.VisibleCount == 0)
		{
			return false;
		}
		int totalSize = documentMeasureInfo.TotalSize;
		int visibleCount = documentMeasureInfo.VisibleCount;
		float num2 = (float)num / (float)totalSize;
		if (AdjustLayoutBoundsForMinimumSize(num2, bounds))
		{
			documentMeasureInfo = MeasureDocuments(bounds);
		}
		Rectangle rectangle = bounds;
		int num3 = 0;
		bool flag = true;
		if (!m_RecordDocumentSize && Math.Abs(totalSize - num) / m_Documents.Count != Math.Abs(totalSize - num) / m_Documents.Count * m_Documents.Count)
		{
			flag = false;
		}
		bool flag2 = totalSize != num;
		for (int i = 0; i < m_Documents.Count; i++)
		{
			DocumentBaseContainer documentBaseContainer = m_Documents[i];
			if (documentBaseContainer.Visible)
			{
				num3++;
				Rectangle rectangle2 = (flag2 ? ((m_Orientation != 0) ? new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, (int)((float)documentBaseContainer.LayoutBounds.Height * num2)) : new Rectangle(rectangle.X, rectangle.Y, (int)((float)documentBaseContainer.LayoutBounds.Width * num2), rectangle.Height)) : ((m_Orientation != 0) ? new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, documentBaseContainer.LayoutBounds.Height) : new Rectangle(rectangle.X, rectangle.Y, documentBaseContainer.LayoutBounds.Width, rectangle.Height)));
				if (num3 == visibleCount)
				{
					rectangle2 = rectangle;
				}
				documentBaseContainer.Layout(rectangle2);
				if (flag)
				{
					documentBaseContainer.SetLayoutBounds(rectangle2);
				}
				if (m_Orientation == eOrientation.Horizontal)
				{
					rectangle.Width -= rectangle2.Width + m_SplitterSize;
					rectangle.X += rectangle2.Width + m_SplitterSize;
				}
				else
				{
					rectangle.Height -= rectangle2.Height + m_SplitterSize;
					rectangle.Y += rectangle2.Height + m_SplitterSize;
				}
			}
		}
		return true;
	}

	protected internal override bool OnSetWidth(DocumentBaseContainer doc, int width)
	{
		bool result = false;
		if (m_Orientation != 0 || (width < doc.MinimumSize.Width && doc.MinimumSize.Width > 0))
		{
			return result;
		}
		DocumentBaseContainer documentBaseContainer = null;
		DocumentBaseContainer documentBaseContainer2 = null;
		int num = m_Documents.IndexOf(doc);
		for (int i = 0; i < m_Documents.Count; i++)
		{
			DocumentBaseContainer documentBaseContainer3 = m_Documents[i];
			if (documentBaseContainer3.Visible)
			{
				if (documentBaseContainer3.DisplayBounds.Width > 0)
				{
					documentBaseContainer3.SetLayoutBounds(documentBaseContainer3.DisplayBounds);
				}
				if (i > num && documentBaseContainer3.Visible && documentBaseContainer == null)
				{
					documentBaseContainer = documentBaseContainer3;
				}
				else if (i < num && documentBaseContainer3.Visible)
				{
					documentBaseContainer2 = documentBaseContainer3;
				}
			}
		}
		int num2 = doc.LayoutBounds.Width - width;
		if (documentBaseContainer != null && documentBaseContainer.LayoutBounds.Width > 0 && documentBaseContainer.LayoutBounds.Width + num2 > 0 && (documentBaseContainer.LayoutBounds.Width + num2 >= documentBaseContainer.MinimumSize.Width || documentBaseContainer.MinimumSize.Width == 0))
		{
			documentBaseContainer.SetLayoutBounds(new Rectangle(documentBaseContainer.LayoutBounds.X, documentBaseContainer.LayoutBounds.Y, documentBaseContainer.LayoutBounds.Width + num2, documentBaseContainer.LayoutBounds.Height));
			result = true;
		}
		else if (documentBaseContainer == null && documentBaseContainer2 != null && documentBaseContainer2.LayoutBounds.Width > 0 && documentBaseContainer2.LayoutBounds.Width + num2 > 0 && (documentBaseContainer2.LayoutBounds.Width + num2 >= documentBaseContainer2.MinimumSize.Width || documentBaseContainer2.MinimumSize.Width == 0))
		{
			doc.SetLayoutBounds(new Rectangle(doc.LayoutBounds.X, doc.LayoutBounds.Y, width, doc.LayoutBounds.Height));
			result = true;
		}
		return result;
	}

	protected internal override bool OnSetHeight(DocumentBaseContainer doc, int height)
	{
		bool result = false;
		if (m_Orientation != eOrientation.Vertical || (height < doc.MinimumSize.Height && doc.MinimumSize.Height > 0))
		{
			return result;
		}
		DocumentBaseContainer documentBaseContainer = null;
		DocumentBaseContainer documentBaseContainer2 = null;
		int num = m_Documents.IndexOf(doc);
		for (int i = 0; i < m_Documents.Count; i++)
		{
			DocumentBaseContainer documentBaseContainer3 = m_Documents[i];
			if (documentBaseContainer3.Visible)
			{
				if (documentBaseContainer3.DisplayBounds.Height > 0)
				{
					documentBaseContainer3.SetLayoutBounds(documentBaseContainer3.DisplayBounds);
				}
				if (i > num && documentBaseContainer3.Visible && documentBaseContainer == null)
				{
					documentBaseContainer = documentBaseContainer3;
				}
				else if (i < num && documentBaseContainer3.Visible)
				{
					documentBaseContainer2 = documentBaseContainer3;
				}
			}
		}
		if (documentBaseContainer == null)
		{
			documentBaseContainer = documentBaseContainer2;
		}
		int num2 = doc.LayoutBounds.Height - height;
		if (documentBaseContainer != null && documentBaseContainer.LayoutBounds.Height > 0 && documentBaseContainer.LayoutBounds.Height + num2 > 0 && (documentBaseContainer.LayoutBounds.Height + num2 >= documentBaseContainer.MinimumSize.Height || documentBaseContainer.MinimumSize.Height == 0))
		{
			documentBaseContainer.SetLayoutBounds(new Rectangle(documentBaseContainer.LayoutBounds.X, documentBaseContainer.LayoutBounds.Y, documentBaseContainer.LayoutBounds.Width, documentBaseContainer.LayoutBounds.Height + num2));
			result = true;
		}
		else if (documentBaseContainer == null && documentBaseContainer2 != null && documentBaseContainer2.LayoutBounds.Height > 0 && documentBaseContainer2.LayoutBounds.Height + num2 > 0 && (documentBaseContainer2.LayoutBounds.Height + num2 >= documentBaseContainer2.MinimumSize.Height || documentBaseContainer2.MinimumSize.Height == 0))
		{
			doc.SetLayoutBounds(new Rectangle(doc.LayoutBounds.X, doc.LayoutBounds.Y, doc.LayoutBounds.Width, height));
			documentBaseContainer2.SetLayoutBounds(new Rectangle(documentBaseContainer2.LayoutBounds.X, documentBaseContainer2.LayoutBounds.Y, documentBaseContainer2.LayoutBounds.Width, 0));
			result = true;
		}
		return result;
	}

	public DocumentBarContainer GetBarDocumentContainer(Bar bar)
	{
		foreach (DocumentBaseContainer document in m_Documents)
		{
			if (document is DocumentBarContainer && ((DocumentBarContainer)document).Bar == bar)
			{
				return (DocumentBarContainer)document;
			}
			if (document is DocumentDockContainer)
			{
				DocumentBarContainer barDocumentContainer = ((DocumentDockContainer)document).GetBarDocumentContainer(bar);
				if (barDocumentContainer != null)
				{
					return barDocumentContainer;
				}
			}
		}
		return null;
	}

	private void DocumentAdded(object sender, EventArgs e)
	{
		DocumentBaseContainer documentBaseContainer = sender as DocumentBaseContainer;
		Size minimumSize = documentBaseContainer.MinimumSize;
		if (m_Orientation == eOrientation.Horizontal)
		{
			m_MinimumSize.Width += minimumSize.Width + m_SplitterSize;
			if (minimumSize.Height > m_MinimumSize.Height)
			{
				m_MinimumSize.Height = minimumSize.Height;
			}
		}
		else
		{
			m_MinimumSize.Height += minimumSize.Height + m_SplitterSize;
			if (minimumSize.Width > m_MinimumSize.Width)
			{
				m_MinimumSize.Width = minimumSize.Width;
			}
		}
	}

	internal void DocumentRemoved(object sender, EventArgs e)
	{
		RefreshMinimumSize();
	}

	internal void RefreshMinimumSize()
	{
		m_MinimumSize = Size.Empty;
		foreach (DocumentBaseContainer document in m_Documents)
		{
			Size minimumSize = document.MinimumSize;
			if (m_Orientation == eOrientation.Horizontal)
			{
				m_MinimumSize.Width += minimumSize.Width + m_SplitterSize;
				if (minimumSize.Height > m_MinimumSize.Height)
				{
					m_MinimumSize.Height = minimumSize.Height;
				}
			}
			else
			{
				m_MinimumSize.Height += minimumSize.Height + m_SplitterSize;
				if (minimumSize.Width > m_MinimumSize.Width)
				{
					m_MinimumSize.Width = minimumSize.Width;
				}
			}
		}
	}

	private void OnOrientationChanged()
	{
		ResetDocumentsLayout();
	}

	private void ResetDocumentsLayout()
	{
		foreach (DocumentBaseContainer document in m_Documents)
		{
			document.SetLayoutBounds(Rectangle.Empty);
		}
	}
}

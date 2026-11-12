using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class MessageFilterWrapper : IMessageFilter
{
	public bool PreFilterMessage(ref Message m)
	{
		switch (((Message)(ref m)).Msg)
		{
		case 260:
			return MessageHandler.OnSysKeyDown(((Message)(ref m)).HWnd, ((Message)(ref m)).WParam, ((Message)(ref m)).LParam);
		case 261:
			return MessageHandler.OnSysKeyUp(((Message)(ref m)).HWnd, ((Message)(ref m)).WParam, ((Message)(ref m)).LParam);
		case 256:
			return MessageHandler.OnKeyDown(((Message)(ref m)).HWnd, ((Message)(ref m)).WParam, ((Message)(ref m)).LParam);
		case 161:
		case 164:
		case 167:
		case 513:
		case 516:
		case 519:
			return MessageHandler.OnMouseDown(((Message)(ref m)).HWnd, ((Message)(ref m)).WParam, ((Message)(ref m)).LParam);
		case 512:
			return MessageHandler.OnMouseMove(((Message)(ref m)).HWnd, ((Message)(ref m)).WParam, ((Message)(ref m)).LParam);
		case 522:
			return MessageHandler.OnMouseWheel(((Message)(ref m)).HWnd, ((Message)(ref m)).WParam, ((Message)(ref m)).LParam);
		default:
			return false;
		}
	}

	public void PostFilterMessage(ref Message m)
	{
	}
}

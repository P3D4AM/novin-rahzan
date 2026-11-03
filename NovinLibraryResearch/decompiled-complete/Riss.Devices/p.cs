using System.Net;
using System.Net.Sockets;
using System.Threading;

internal class p
{
	private byte[] m_a;

	private IPEndPoint b;

	private UdpClient c;

	public p(byte[] A_0, ref UdpClient A_1, IPEndPoint A_2)
	{
		this.m_a = A_0;
		b = A_2;
		c = A_1;
	}

	public void a()
	{
		Thread.Sleep(1280);
		c.Send(this.m_a, this.m_a.Length, b);
	}
}

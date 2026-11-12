using System;
using System.Collections;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace mCore;

public class Inbox : IInbox, IEnumerable
{
	[Obsolete("mCore.Inbox.EnumMessageType enumerator was obsoleted in version 1.1. Use mCore.MessageType instead", true)]
	public enum EnumMessageType
	{
		ReceivedUnreadMessages,
		ReceivedReadMessages,
		ReceivedAllMessages
	}

	private class a
	{
		internal bool a;

		internal MessageType b;

		internal bool c;

		internal bool d;

		internal bool e;

		public a()
		{
			a = true;
			c = false;
			d = false;
			e = false;
		}
	}

	public class InboxEnumerator : IEnumerator
	{
		private Inbox a;

		public object Current => a.Message(checked(a.c + 1));

		public InboxEnumerator(Inbox Ibox)
		{
			a = Ibox;
			Reset();
		}

		public bool MoveNext()
		{
			checked
			{
				if (a.c == a.Count - 1)
				{
					return false;
				}
				Inbox inbox = a;
				inbox.c++;
				return true;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		public void Reset()
		{
			a.c = -1;
		}

		void IEnumerator.Reset()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Reset
			this.Reset();
		}
	}

	private SMS m_a;

	private f m_b;

	private int c;

	private const int d = 1;

	public bool Concatenate
	{
		get
		{
			this.m_b.aw();
			return this.m_b.b1;
		}
		set
		{
			this.m_b.aw();
			this.m_b.b1 = value;
		}
	}

	public bool HasMessages
	{
		get
		{
			bool result = default(bool);
			try
			{
				this.m_b.aw();
				result = false;
				if (this.m_b.c0.Count > 0)
				{
					result = true;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				this.m_b.a.b("[Inbox:HasMessages:1] " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			a();
			return result;
		}
	}

	public bool IsEmpty
	{
		get
		{
			bool result = default(bool);
			try
			{
				this.m_b.aw();
				result = false;
				if (this.m_b.c0.Count <= 0)
				{
					result = true;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				this.m_b.a.b("[Inbox:IsEmpty:1] " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			a();
			return result;
		}
	}

	public int Count
	{
		get
		{
			int count = default(int);
			try
			{
				this.m_b.aw();
				count = this.m_b.c0.Count;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				this.m_b.a.b("[Inbox:Count:1] " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			a();
			return count;
		}
	}

	internal Inbox(ref SMS A_0)
	{
		c = -1;
		this.m_a = A_0;
		this.m_b = this.m_a.gm;
	}

	public Message Message(int Index)
	{
		Message result = null;
		try
		{
			this.m_b.aw();
			if (Index <= 0 || Index > this.m_b.c0.Count)
			{
				this.m_b.ae = 0;
				this.m_b.a(5002L, "Invalid message item index", "SMSREAD");
				result = null;
			}
			else
			{
				this.m_b.ae = Index;
				result = (Message)this.m_b.c0[Index.ToString()];
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.m_b.a.b("[Inbox:Message:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			a();
		}
		return result;
	}

	Message IInbox.Message(int Index)
	{
		//ILSpy generated this explicit interface implementation from .override directive in Message
		return this.Message(Index);
	}

	public bool Refresh()
	{
		a a = new a();
		a.b = MessageType.ReceivedAllMessages;
		a.e = true;
		ThreadPool.RegisterWaitForSingleObject(this.m_b.dz, this.a, a, -1, executeOnlyOnce: true);
		while (a.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		bool flag = a.d;
		if (a != null)
		{
			a = null;
		}
		bool result = flag;
		this.a();
		return result;
	}

	bool IInbox.Refresh()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Refresh
		return this.Refresh();
	}

	public bool Refresh(MessageType ReadMessageType)
	{
		a a = new a();
		a.b = ReadMessageType;
		a.e = true;
		ThreadPool.RegisterWaitForSingleObject(this.m_b.dz, this.a, a, -1, executeOnlyOnce: true);
		while (a.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		bool flag = a.d;
		if (a != null)
		{
			a = null;
		}
		bool result = flag;
		this.a();
		return result;
	}

	bool IInbox.Refresh(MessageType ReadMessageType)
	{
		//ILSpy generated this explicit interface implementation from .override directive in Refresh
		return this.Refresh(ReadMessageType);
	}

	public bool Refresh(MessageType ReadMessageType, bool CleanUpMemory)
	{
		a a = new a();
		a.b = ReadMessageType;
		a.e = CleanUpMemory;
		ThreadPool.RegisterWaitForSingleObject(this.m_b.dz, this.a, a, -1, executeOnlyOnce: true);
		while (a.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		bool flag = a.d;
		if (a != null)
		{
			a = null;
		}
		bool result = flag;
		this.a();
		return result;
	}

	bool IInbox.Refresh(MessageType ReadMessageType, bool CleanUpMemory)
	{
		//ILSpy generated this explicit interface implementation from .override directive in Refresh
		return this.Refresh(ReadMessageType, CleanUpMemory);
	}

	public bool Clear()
	{
		a a = new a();
		ThreadPool.RegisterWaitForSingleObject(this.m_b.dz, b, a, -1, executeOnlyOnce: true);
		while (a.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		bool result = a.c;
		this.a();
		return result;
	}

	bool IInbox.Clear()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Clear
		return this.Clear();
	}

	public IEnumerator GetEnumerator()
	{
		return new InboxEnumerator(this);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		//ILSpy generated this explicit interface implementation from .override directive in GetEnumerator
		return this.GetEnumerator();
	}

	private void b(object A_0, bool A_1)
	{
		a a = (a)A_0;
		try
		{
			this.m_b.aw();
			a.c = false;
			this.m_b.cy = false;
			bool flag = true;
			if (this.m_b.c0.Count > 0)
			{
				for (int i = this.m_b.c0.Count; i >= 1; i = checked(i + -1))
				{
					if (this.m_b.c0.Count <= 0)
					{
						break;
					}
					this.m_b.ae = i;
					if (!Message(i).a())
					{
						flag = false;
					}
				}
				if (!flag)
				{
					this.m_b.a(6002L, "Unable to delete at least one message", "SMSDELETE");
					a.c = flag;
				}
				a.c = flag;
				if (this.m_b.cz.Count > 0)
				{
					this.m_b.ap();
				}
			}
			else
			{
				this.m_b.a(6001L, "Inbox is already empty", "SMSDELETE");
				a.c = flag;
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.m_b.a.b("[Inbox:Clear_Method:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			a.a = false;
			this.m_b.dz.Set();
		}
	}

	private void a(object A_0, bool A_1)
	{
		a a = (a)A_0;
		try
		{
			MessageType messageType = a.b;
			bool e = a.e;
			bool flag = false;
			int a2 = this.m_b.a3;
			this.m_b.a3 = checked(a2 * 3);
			try
			{
				this.m_b.aw();
				if (messageType >= MessageType.ReceivedUnreadMessages && messageType <= MessageType.ReceivedAllMessages)
				{
					flag = this.m_b.a((int)messageType, e);
					if (!flag)
					{
						if (this.m_b.b6 == 0)
						{
							this.m_b.b6 = 5010L;
							this.m_b.b5 = "Read message error";
						}
						this.m_b.a(this.m_b.b6, this.m_b.b5, "SMSREAD");
					}
				}
				else
				{
					flag = false;
					this.m_b.a(5011L, "Invalid read message type option", "SMSREAD");
				}
				a.d = flag;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				this.m_b.a.b("[Inbox:Refresh_Method:1] " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			finally
			{
				this.m_b.a3 = a2;
			}
		}
		catch (Exception ex3)
		{
			ProjectData.SetProjectError(ex3);
			Exception ex4 = ex3;
			this.m_b.a.b("[Inbox:Refresh_Method:2] " + ex4.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			a.a = false;
			this.m_b.dz.Set();
		}
	}

	private void a()
	{
		if (this.m_b.ac.Length > 0)
		{
			switch (this.m_b.ac.ToUpper())
			{
			case "GENERAL":
				throw new GeneralException("ERROR " + this.m_b.z + ": " + this.m_b.y);
			case "SMSSEND":
				throw new SMSSendException("ERROR " + this.m_b.z + ": " + this.m_b.y);
			case "SMSREAD":
				throw new SMSReadException("ERROR " + this.m_b.z + ": " + this.m_b.y);
			case "SMSDELETE":
				throw new SMSDeleteException("ERROR " + this.m_b.z + ": " + this.m_b.y);
			case "WAPPUSH":
				throw new WapPushException("ERROR " + this.m_b.z + ": " + this.m_b.y);
			case "VCARD":
				throw new vCardException("ERROR " + this.m_b.z + ": " + this.m_b.y);
			case "VCALENDAR":
				throw new vCalendarException("ERROR " + this.m_b.z + ": " + this.m_b.y);
			}
		}
	}
}

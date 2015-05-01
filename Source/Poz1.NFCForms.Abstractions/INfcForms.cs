using System;
using System.Collections;
using NdefLibrary.Ndef;

namespace NFCForms
{
	public interface INfcForms
	{
		bool IsAvailable { get; }

	    void WriteTag(NdefMessage message);

		event EventHandler<XTag> NewTag;

		event EventHandler<XTag> TagConnected;

		event EventHandler<XTag> TagDisconnected;
	}

	public class XTag
	{
		public XTag()
		{
			
		}

		public NdefMessage NdefMessage;			
		public IList TechList;
		public bool IsNdefSupported;
		public bool IsWriteable;
		public bool IsConnected;
		public byte[] Id;
        public int MaxSize;
	}
}


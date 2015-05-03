using NdefLibrary.Ndef;
using System.Collections;

namespace Poz1.NFCForms.Abstract
{
    public class NfcFormsTag
    {
        public NfcFormsTag()
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

using NdefLibrary.Ndef;
using NFCForms.WinPhone;
using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Networking.Proximity;

[assembly: Xamarin.Forms.Dependency(typeof(NfcForms))]
namespace NFCForms.WinPhone
{
    public class NfcForms : INfcForms
    {
        #region Private Variables

        private ProximityDevice nfcDevice;
        private XTag xtag;
        private bool isTagPresent;

        #endregion

        #region Properties
        public bool IsAvailable
        {
            get
            {
                if (ProximityDevice.GetDefault() != null)
                    return true;
                else
                    return false;
            }
        }

        #endregion

        #region Constructors
        public NfcForms() 
        {
            xtag = new XTag();
            if (ProximityDevice.GetDefault() != null)
            {
                nfcDevice = ProximityDevice.GetDefault();
                nfcDevice.SubscribeForMessage("NDEF", MessageReceivedHandler);
                xtag.IsWriteable = false;
                xtag.MaxSize = 0;
                nfcDevice.DeviceArrived += nfcDevice_DeviceArrived;
                nfcDevice.DeviceDeparted += nfcDevice_DeviceDeparted;
            }
        }

        #endregion

        #region Private Methods
        private int GetMaxSize() 
        {
            return xtag.MaxSize;
        }

        private bool GetIsWriteable() 
        {
            return xtag.IsWriteable;
        }

        private void nfcDevice_DeviceDeparted(ProximityDevice sender)
        {
            isTagPresent = false;
        }

        private void nfcDevice_DeviceArrived(ProximityDevice sender)
        {
            isTagPresent = true;
        }

        private void WriteableTagHandler(ProximityDevice sender, ProximityMessage message)
        {
            xtag.IsWriteable = true;
            xtag.MaxSize = System.BitConverter.ToInt32(message.Data.ToArray(), 0);
            RaiseNewTag(xtag);
        }

        private void MessageReceivedHandler(ProximityDevice device, ProximityMessage message)
        {
            xtag.IsNdefSupported = true;
            xtag.Id = new byte[0];
            xtag.TechList = new System.Collections.ObjectModel.ObservableCollection<string>();
            var rawMsg = message.Data.ToArray();
            xtag.NdefMessage = NdefMessage.FromByteArray(rawMsg);
            if (message.MessageType == "WriteableTag")
                nfcDevice.SubscribeForMessage("WriteableTag", WriteableTagHandler);
            else
                RaiseNewTag(xtag);
        }

        #endregion

        #region Public Methods
        public void WriteTag(NdefLibrary.Ndef.NdefMessage message)
        {
            int messageSize = 0;

            foreach (NdefRecord record in message)
            {
                messageSize += record.Payload.Length;
                if (!record.CheckIfValid())
                    throw new Exception("A record on NDEFMessage is not valid");
            }

            if (!isTagPresent)
                throw new Exception("No Tag present or Tag is incompatible ");

            if (!xtag.IsWriteable)
                throw new Exception("Tag is write locked ");

            if (xtag.MaxSize<messageSize)
                throw new Exception("Tag is too small for this message");

            RaiseTagConnected(xtag);

            nfcDevice.PublishBinaryMessage("NDEF:WriteTag", message.ToByteArray().AsBuffer(), writerHandler);
        }
        #endregion

        #region Events
        private void writerHandler(ProximityDevice sender, long messageId)
        {
            nfcDevice.StopPublishingMessage(messageId);
            RaiseTagDisconnected(xtag);
        }

        public event EventHandler<XTag> NewTag;

        private void RaiseNewTag(XTag tag)
        {
            if (NewTag != null)
            {
                NewTag(this, tag);
            }
        }

        public event EventHandler<XTag> TagConnected;
        private void RaiseTagConnected(XTag tag)
        {
            xtag.IsConnected = true;
            if (TagConnected != null)
            {
                TagConnected(this, tag);
            }
        }

        public event EventHandler<XTag> TagDisconnected;
        private void RaiseTagDisconnected(XTag tag)
        {
            xtag.IsConnected = false;
            if (TagDisconnected != null)
            {
                TagDisconnected(this, tag);
            }
        }

        #endregion
    }
}

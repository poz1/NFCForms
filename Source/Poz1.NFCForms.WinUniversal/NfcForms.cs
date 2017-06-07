using NdefLibrary.Ndef;
using Poz1.NfcForms.WinUniversal;
using Poz1.NFCForms.Abstract;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Networking.Proximity;

[assembly: Xamarin.Forms.Dependency(typeof(NfcForms))]
namespace Poz1.NfcForms.WinUniversal
{
    public class NfcForms : INfcForms
    {
        #region Private Variables

        private ProximityDevice nfcDevice;
        private NfcFormsTag nfcTag;
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
            nfcTag = new NfcFormsTag();
            if (ProximityDevice.GetDefault() != null)
            {
                nfcDevice = ProximityDevice.GetDefault();
                nfcDevice.SubscribeForMessage("NDEF", MessageReceivedHandler);
                nfcTag.IsWriteable = false;
                nfcTag.MaxSize = 0;
                nfcDevice.DeviceArrived += nfcDevice_DeviceArrived;
                nfcDevice.DeviceDeparted += nfcDevice_DeviceDeparted;
            }
        }

        #endregion

        #region Private Methods
        private int GetMaxSize()
        {
            return nfcTag.MaxSize;
        }

        private bool GetIsWriteable()
        {
            return nfcTag.IsWriteable;
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
            nfcTag.IsWriteable = true;
            nfcTag.MaxSize = System.BitConverter.ToInt32(message.Data.ToArray(), 0);
            RaiseNewTag(nfcTag);
        }

        private void MessageReceivedHandler(ProximityDevice device, ProximityMessage message)
        {
            nfcTag.IsNdefSupported = true;
            nfcTag.Id = new byte[0];
            nfcTag.TechList = new System.Collections.ObjectModel.ObservableCollection<string>();
            var rawMsg = message.Data.ToArray();
            nfcTag.NdefMessage = NdefMessage.FromByteArray(rawMsg);
            if (message.MessageType == "WriteableTag")
                nfcDevice.SubscribeForMessage("WriteableTag", WriteableTagHandler);
            else
                RaiseNewTag(nfcTag);
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

            if (!nfcTag.IsWriteable)
                throw new Exception("Tag is write locked ");

            if (nfcTag.MaxSize < messageSize)
                throw new Exception("Tag is too small for this message");

            RaiseTagConnected(nfcTag);

            nfcDevice.PublishBinaryMessage("NDEF:WriteTag", message.ToByteArray().AsBuffer(), writerHandler);
        }
        #endregion

        #region Events
        private void writerHandler(ProximityDevice sender, long messageId)
        {
            nfcDevice.StopPublishingMessage(messageId);
            RaiseTagDisconnected(nfcTag);
        }

        public event EventHandler<NfcFormsTag> NewTag;

        private void RaiseNewTag(NfcFormsTag tag)
        {
            if (NewTag != null)
            {
                NewTag(this, tag);
            }
        }

        public event EventHandler<NfcFormsTag> TagConnected;
        private void RaiseTagConnected(NfcFormsTag tag)
        {
            nfcTag.IsConnected = true;
            if (TagConnected != null)
            {
                TagConnected(this, tag);
            }
        }

        public event EventHandler<NfcFormsTag> TagDisconnected;
        private void RaiseTagDisconnected(NfcFormsTag tag)
        {
            nfcTag.IsConnected = false;
            if (TagDisconnected != null)
            {
                TagDisconnected(this, tag);
            }
        }

        #endregion
    }
}

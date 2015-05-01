using Android.Nfc;
using Android.Content;
using System.Collections.Generic;
using Android.Nfc.Tech;
using System;
using System.IO;
using NFCForms.Droid;

[assembly: Xamarin.Forms.Dependency (typeof (NfcForms))]
namespace NFCForms.Droid
{
	#region Android NFC Techs
	public sealed class NFCTechs
	{
		public const string IsoDep = "android.nfc.tech.IsoDep";
		public const string NfcA = "android.nfc.tech.NfcA";
		public const string NfcB = "android.nfc.tech.NfcB";
		public const string NfcF = "android.nfc.tech.NfcF";
		public const string NfcV = "android.nfc.tech.NfcV";
		public const string Ndef = "android.nfc.tech.Ndef";
		public const string NdefFormatable = "android.nfc.tech.NdefFormatable";
		public const string MifareClassic = "android.nfc.tech.MifareClassic";
		public const string MifareUltralight = "android.nfc.tech.MifareUltralight";
	}
	#endregion

	public class NfcForms : INfcForms
	{
		#region Private Variables

		private NfcAdapter nfcDevice;
		private XTag xtag;
		private Tag tag;

		#endregion

		#region Properties

		public bool IsAvailable
		{
			get
			{
				return nfcDevice.IsEnabled;
			}
		} 

		#endregion

		#region Constructors

		public NfcForms ()
		{
			NfcManager NfcManager =	(NfcManager)Android.App.Application.Context.GetSystemService(Context.NfcService);		
			nfcDevice = NfcManager.DefaultAdapter;
		    xtag = new XTag ();
		}

		#endregion

		#region Private Methods

		private Ndef GetNdef(Tag tag)
		{
			Ndef ndef = Ndef.Get(tag);
			if (ndef == null)
				return null;
			else
				return ndef;
		}

		private NdefLibrary.Ndef.NdefMessage ReadNdef(Ndef ndef)
		{	
			try
			{
				return NdefLibrary.Ndef.NdefMessage.FromByteArray(ndef.CachedNdefMessage.ToByteArray());
			}

			catch
			{
				throw new Exception("Tag Error: No NDEF message found o NDEF not supported");
			}
		}

		#endregion

		#region Public Methods

		public void OnNewIntent (object sender, Intent e)
		{
		    tag = e.GetParcelableExtra(NfcAdapter.ExtraTag) as Tag;

			xtag.TechList = new List<string>(tag.GetTechList());
			xtag.Id = tag.GetId();

			if (GetNdef (tag) == null) 
			{
				xtag.IsNdefSupported = false;
			}
			else 
			{
				xtag.IsNdefSupported = true;
				Ndef ndef = GetNdef (tag);
				xtag.NdefMessage = ReadNdef (ndef);
				xtag.IsWriteable = ndef.IsWritable;
				xtag.MaxSize = ndef.MaxSize;
			}

			RaiseNewTag(xtag);
		}

		public void WriteTag (NdefLibrary.Ndef.NdefMessage message)
		{
			if (tag == null) 
			{
				throw new Exception("Tag Error: No Tag to write, register to NewTag event before calling WriteTag()");
			}

			Ndef ndef = GetNdef (tag);

			if (ndef == null) 
			{
				throw new Exception("Tag Error: NDEF not supported");
			}


			try
			{
				ndef.Connect();
				RaiseTagConnected (xtag);
			}

			catch
			{
				throw new Exception("Tag Error: No Tag nearby");
			}

			if(!ndef.IsWritable) 
			{				
				ndef.Close ();
				throw new Exception("Tag Error: Tag is write locked");
			}

			int size = message.ToByteArray ().Length;

			if(ndef.MaxSize < size) 
			{
				ndef.Close ();
				throw new Exception("Tag Error: Tag is too small");
			}

			try 
			{
				List<Android.Nfc.NdefRecord> records = new List<Android.Nfc.NdefRecord>();
				for(int i = 0; i< message.Count;i++)
				{
					if(message[i].CheckIfValid())
						records.Add(new Android.Nfc.NdefRecord(Android.Nfc.NdefRecord.TnfWellKnown,message[i].Type,message[i].Id,message[i].Payload));
					else
					{
						throw new Exception("NDEFRecord number " + i + "is not valid");
					}
				};
				Android.Nfc.NdefMessage msg = new Android.Nfc.NdefMessage(records.ToArray());
				ndef.WriteNdefMessage(msg);
			}

			catch (TagLostException tle) 
			{
				throw new Exception("Tag Lost Error: " + tle.Message);
			} 

			catch (IOException ioe) 
			{
				throw new Exception("Tag IO Error: " +  ioe.ToString());
			}

			catch (Android.Nfc.FormatException fe) 
			{
				throw new Exception("Tag Format Error: " + fe.Message);
			}

			catch (Exception e) 
			{
				throw new Exception("Tag Error: " + e.ToString());
			}

			finally
			{
				ndef.Close ();
				RaiseTagTagDisconnected (xtag);
			}

		}

		#endregion

		#region Events

		public	event EventHandler<XTag> TagConnected;

		public  void RaiseTagConnected(XTag tag)
		{
			xtag.IsConnected = true;
	
			if (TagConnected != null)
			{
				TagConnected(this, tag);
			}
		}

		public	event EventHandler<XTag> TagDisconnected;

		public  void RaiseTagTagDisconnected(XTag tag)
		{
			xtag.IsConnected = false;

			if (TagDisconnected != null)
			{
				TagDisconnected(this, tag);
			}
		}

			
		public	event EventHandler<XTag> NewTag;

		public  void RaiseNewTag(XTag tag)
		{
			if (NewTag != null)
			{
				NewTag(this, tag);
			}
		}

		#endregion
	}
}


package xlabs.platform.services;


public class NfcDevice
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.nfc.NfcAdapter.CreateNdefMessageCallback
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_createNdefMessage:(Landroid/nfc/NfcEvent;)Landroid/nfc/NdefMessage;:GetCreateNdefMessage_Landroid_nfc_NfcEvent_Handler:Android.Nfc.NfcAdapter/ICreateNdefMessageCallbackInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("XLabs.Platform.Services.NfcDevice, XLabs.Platform.Droid, Version=2.0.5520.36779, Culture=neutral, PublicKeyToken=null", NfcDevice.class, __md_methods);
	}


	public NfcDevice () throws java.lang.Throwable
	{
		super ();
		if (getClass () == NfcDevice.class)
			mono.android.TypeManager.Activate ("XLabs.Platform.Services.NfcDevice, XLabs.Platform.Droid, Version=2.0.5520.36779, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public NfcDevice (android.nfc.NfcAdapter p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == NfcDevice.class)
			mono.android.TypeManager.Activate ("XLabs.Platform.Services.NfcDevice, XLabs.Platform.Droid, Version=2.0.5520.36779, Culture=neutral, PublicKeyToken=null", "Android.Nfc.NfcAdapter, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public android.nfc.NdefMessage createNdefMessage (android.nfc.NfcEvent p0)
	{
		return n_createNdefMessage (p0);
	}

	private native android.nfc.NdefMessage n_createNdefMessage (android.nfc.NfcEvent p0);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}

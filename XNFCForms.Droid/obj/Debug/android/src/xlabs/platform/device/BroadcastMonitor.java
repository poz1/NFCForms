package xlabs.platform.device;


public abstract class BroadcastMonitor
	extends android.content.BroadcastReceiver
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("XLabs.Platform.Device.BroadcastMonitor, XLabs.Platform.Droid, Version=2.0.5520.36779, Culture=neutral, PublicKeyToken=null", BroadcastMonitor.class, __md_methods);
	}


	public BroadcastMonitor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == BroadcastMonitor.class)
			mono.android.TypeManager.Activate ("XLabs.Platform.Device.BroadcastMonitor, XLabs.Platform.Droid, Version=2.0.5520.36779, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

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

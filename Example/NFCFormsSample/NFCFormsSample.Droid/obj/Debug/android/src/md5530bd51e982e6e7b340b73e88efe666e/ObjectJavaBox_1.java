package md5530bd51e982e6e7b340b73e88efe666e;


public class ObjectJavaBox_1
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Xamarin.Forms.Platform.Android.ObjectJavaBox`1, Xamarin.Forms.Platform.Android, Version=1.4.0.0, Culture=neutral, PublicKeyToken=null", ObjectJavaBox_1.class, __md_methods);
	}


	public ObjectJavaBox_1 () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ObjectJavaBox_1.class)
			mono.android.TypeManager.Activate ("Xamarin.Forms.Platform.Android.ObjectJavaBox`1, Xamarin.Forms.Platform.Android, Version=1.4.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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

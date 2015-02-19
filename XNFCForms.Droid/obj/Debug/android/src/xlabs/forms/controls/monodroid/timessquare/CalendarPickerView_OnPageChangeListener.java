package xlabs.forms.controls.monodroid.timessquare;


public class CalendarPickerView_OnPageChangeListener
	extends android.support.v4.view.ViewPager.SimpleOnPageChangeListener
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onPageSelected:(I)V:GetOnPageSelected_IHandler\n" +
			"";
		mono.android.Runtime.register ("XLabs.Forms.Controls.MonoDroid.TimesSquare.CalendarPickerView/OnPageChangeListener, XLabs.Forms.Droid, Version=2.0.5522.38682, Culture=neutral, PublicKeyToken=null", CalendarPickerView_OnPageChangeListener.class, __md_methods);
	}


	public CalendarPickerView_OnPageChangeListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == CalendarPickerView_OnPageChangeListener.class)
			mono.android.TypeManager.Activate ("XLabs.Forms.Controls.MonoDroid.TimesSquare.CalendarPickerView/OnPageChangeListener, XLabs.Forms.Droid, Version=2.0.5522.38682, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public CalendarPickerView_OnPageChangeListener (xlabs.forms.controls.monodroid.timessquare.CalendarPickerView p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == CalendarPickerView_OnPageChangeListener.class)
			mono.android.TypeManager.Activate ("XLabs.Forms.Controls.MonoDroid.TimesSquare.CalendarPickerView/OnPageChangeListener, XLabs.Forms.Droid, Version=2.0.5522.38682, Culture=neutral, PublicKeyToken=null", "XLabs.Forms.Controls.MonoDroid.TimesSquare.CalendarPickerView, XLabs.Forms.Droid, Version=2.0.5522.38682, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}


	public void onPageSelected (int p0)
	{
		n_onPageSelected (p0);
	}

	private native void n_onPageSelected (int p0);

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

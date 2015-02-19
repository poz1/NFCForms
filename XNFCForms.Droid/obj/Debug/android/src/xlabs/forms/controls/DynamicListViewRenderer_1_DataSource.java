package xlabs.forms.controls;


public class DynamicListViewRenderer_1_DataSource
	extends android.widget.BaseAdapter
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_getItem:(I)Ljava/lang/Object;:GetGetItem_IHandler\n" +
			"n_getItemId:(I)J:GetGetItemId_IHandler\n" +
			"n_getView:(ILandroid/view/View;Landroid/view/ViewGroup;)Landroid/view/View;:GetGetView_ILandroid_view_View_Landroid_view_ViewGroup_Handler\n" +
			"n_getCount:()I:GetGetCountHandler\n" +
			"";
		mono.android.Runtime.register ("XLabs.Forms.Controls.DynamicListViewRenderer`1/DataSource, XLabs.Forms.Droid, Version=2.0.5522.38682, Culture=neutral, PublicKeyToken=null", DynamicListViewRenderer_1_DataSource.class, __md_methods);
	}


	public DynamicListViewRenderer_1_DataSource () throws java.lang.Throwable
	{
		super ();
		if (getClass () == DynamicListViewRenderer_1_DataSource.class)
			mono.android.TypeManager.Activate ("XLabs.Forms.Controls.DynamicListViewRenderer`1/DataSource, XLabs.Forms.Droid, Version=2.0.5522.38682, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public DynamicListViewRenderer_1_DataSource (xlabs.forms.controls.DynamicListViewRenderer_1 p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == DynamicListViewRenderer_1_DataSource.class)
			mono.android.TypeManager.Activate ("XLabs.Forms.Controls.DynamicListViewRenderer`1/DataSource, XLabs.Forms.Droid, Version=2.0.5522.38682, Culture=neutral, PublicKeyToken=null", "XLabs.Forms.Controls.DynamicListViewRenderer`1<T>, XLabs.Forms.Droid, Version=2.0.5522.38682, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}


	public java.lang.Object getItem (int p0)
	{
		return n_getItem (p0);
	}

	private native java.lang.Object n_getItem (int p0);


	public long getItemId (int p0)
	{
		return n_getItemId (p0);
	}

	private native long n_getItemId (int p0);


	public android.view.View getView (int p0, android.view.View p1, android.view.ViewGroup p2)
	{
		return n_getView (p0, p1, p2);
	}

	private native android.view.View n_getView (int p0, android.view.View p1, android.view.ViewGroup p2);


	public int getCount ()
	{
		return n_getCount ();
	}

	private native int n_getCount ();

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

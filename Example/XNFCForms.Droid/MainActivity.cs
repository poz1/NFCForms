using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using XLabs.Ioc;
using XLabs.Platform.Device;
using Android.Nfc;
using NFCForms;
using NFCForms.Droid;
using Android.Content;

namespace XNFCForms.Droid
{
	[Activity (Label = "NFCForms.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	[IntentFilter (new[]{NfcAdapter.ActionTechDiscovered})]
	[MetaData (NfcAdapter.ActionTechDiscovered, Resource="@xml/nfc")]

	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		public NfcAdapter NFCdevice;
		public XNFC x;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			var resolverContainer = new SimpleContainer();
			resolverContainer.Register<IDevice>(t => AndroidDevice.CurrentDevice);
			Resolver.SetResolver(resolverContainer.GetResolver());

			global::Xamarin.Forms.Forms.Init (this, bundle);

			NfcManager NfcManager =	(NfcManager)Android.App.Application.Context.GetSystemService(Context.NfcService);		
			NFCdevice = NfcManager.DefaultAdapter;

			Xamarin.Forms.DependencyService.Register<IXNFC,XNFC>();
			x = Xamarin.Forms.DependencyService.Get<IXNFC>() as XNFC;

			LoadApplication (new App ());
		}

		protected override void OnResume ()
		{
			base.OnResume ();
			if (NFCdevice != null) {
				var intent = new Intent (this, GetType ()).AddFlags (ActivityFlags.SingleTop);
				NFCdevice.EnableForegroundDispatch
				(
					this,
					PendingIntent.GetActivity (this, 0, intent, 0),
					new[] { new IntentFilter (NfcAdapter.ActionTechDiscovered) },
					new String[][] {new string[] {
							NFCTechs.Ndef,
						},
						new string[] {
							NFCTechs.MifareClassic,
						},
					}
				);
			}
		}

		protected override void OnPause ()
		{
			base.OnPause ();
			NFCdevice.DisableForegroundDispatch (this);
		}

		protected override void OnNewIntent (Intent intent)
		{
			base.OnNewIntent (intent);
			x.OnNewIntent(this, intent);
		}
	}
}


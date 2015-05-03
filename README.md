# NFCForms

Easily read and write NFC tags in your Xamarin.Forms application. XNFCForms is an example app that you can download to see how it works.

##Installation

Install from Nuget NFCForms ( http://www.nuget.org/packages/NFCForms/ )

###Android
1: Add the NFC capability in the manifest.
2: In MainActivity.cs add

```
public NfcAdapter NFCdevice;
public NfcForms x;
```

3: In the OnCreate method, before LoadApplication(), add

```
NfcManager NfcManager =	(NfcManager)Android.App.Application.Context.GetSystemService(Context.NfcService);		
NFCdevice = NfcManager.DefaultAdapter;

Xamarin.Forms.DependencyService.Register<INfcForms,NfcForms>();
x = Xamarin.Forms.DependencyService.Get<INfcForms>() as NfcForms;
```

4: after OnCreate() add

```
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
```

Here we are asking Android to notify us when an NFC tag is available. We can choose wich kind of tags we want to be notified setting the parameeter
of the IntentFilter. In this case we will receive tags supporting NDEF or MifareClassic. More info here ( http://developer.android.com/guide/topics/connectivity/nfc/nfc.html ).

5: If we want our app to be lauched when a compatible tag is scanned add

```
[Activity (Label = "NFCForms.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
[IntentFilter (new[]{NfcAdapter.ActionTechDiscovered})]
[MetaData (NfcAdapter.ActionTechDiscovered, Resource="@xml/nfc")]
```

and an XML file where you set the kind of NFC tag you want to open your app (there is one in the example in Resources/xml/nfc.xml).
This code is the equivalent of adding these lines 

```
<activity>
...
<intent-filter>
    <action android:name="android.nfc.action.TECH_DISCOVERED"/>
</intent-filter>

<meta-data android:name="android.nfc.action.TECH_DISCOVERED"
    android:resource="@xml/nfc_tech_filter" />
...
</activity>
```

in the manifest file. Again, more info here ( http://developer.android.com/guide/topics/connectivity/nfc/nfc.html )

###Windows Phone

1: Add NFC capability to the manifest

2: In MainPage.xaml.cs add

```
public NfcForms x;
```
3: before LoadApplication() add
```
Xamarin.Forms.DependencyService.Register<INfcForms, NfcForms>();
x = Xamarin.Forms.DependencyService.Get<INfcForms>() as NfcForms;
```

On WindowsPhone only NDEF tags are supported so there is no need to specify technologies.

##Usage
###Read Tag
We call the DependecyService to get the NFC Reader and we register to the NewTag event:
```
INfcForms device = DependencyService.Get<INfcForms>();
device.NewTag += HandleNewTag;
```
then when a compatible tag is scanned the event is fired and we receive an NfcFormsTag.
In NfcFormsTag.NdefMessage we have the NDEF message. Using NDEF Library ( http://github.com/mopius/ndef-nfc ) we can read the content.

###Write Tag
we use 
```
device.WriteTag(msg);
``` 
where msg is an NDEFMessage that, again, you can create using NDEF Library.
Please note that this method is assuming the tag to be in place when it is called. 

##Credits

Written by Alessandro Pozone - www.POZ1.com thanks to the guys behind NDEF Library, without their project this would have been much more limited.

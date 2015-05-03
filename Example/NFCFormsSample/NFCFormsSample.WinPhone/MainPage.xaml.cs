using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Poz1.NFCForms.Abstract;
using Poz1.NfcForms.WinPhone;

namespace NFCFormsSample.WinPhone
{
	public partial class MainPage : global::Xamarin.Forms.Platform.WinPhone.FormsApplicationPage
	{
        public NfcForms x;

		public MainPage ()
		{
			InitializeComponent ();
			SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;

			global::Xamarin.Forms.Forms.Init ();

            Xamarin.Forms.DependencyService.Register<INfcForms, NfcForms>();
            x = Xamarin.Forms.DependencyService.Get<INfcForms>() as NfcForms;

			LoadApplication (new NFCFormsSample.App ());
		}
	}
}

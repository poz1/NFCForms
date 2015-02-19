using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using XLabs.Platform.Device;
using XLabs.Ioc;
using NFCForms.WinPhone;
using NFCForms;

namespace XNFCForms.WinPhone
{
    public partial class MainPage : global::Xamarin.Forms.Platform.WinPhone.FormsApplicationPage
    {
        public XNFC x; 
        public MainPage()
        {
            InitializeComponent();
            SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;

            global::Xamarin.Forms.Forms.Init();

            Xamarin.Forms.DependencyService.Register<IXNFC, XNFC>();
            x = Xamarin.Forms.DependencyService.Get<IXNFC>() as XNFC;


            var container = new SimpleContainer();
            container.Register<IDevice>(t => WindowsPhoneDevice.CurrentDevice);
            container.Register<IDisplay>(t => t.Resolve<IDevice>().Display);

            Resolver.SetResolver(container.GetResolver());

            LoadApplication(new XNFCForms.App());
        }
    }
}

using System;
using Xamarin.Forms;
using XLabs.Platform.Services;
using XLabs.Ioc;
using XLabs.Platform.Device;
using System.Collections.ObjectModel;
using NFCForms;
using NdefLibrary.Ndef;

namespace XNFCForms
{
	public class NFCPage : ContentPage
	{
		private readonly IDisplay display;
        private readonly IXNFC device;
		private StackLayout welcomePanel;

		private Switch IsWriteable;
		private Switch IsConnected;
		private Switch IsNDEFSupported;

		private ListView TechList;
		private ListView NDEFMessage;

		public NFCPage ()
		{
            display = Resolver.Resolve<IDevice>().Display;

            device = DependencyService.Get<IXNFC>();
            device.NewTag += HandleNewTag;
            device.TagConnected += device_TagConnected;
            device.TagDisconnected += device_TagDisconnected;

			Grid mainGrid = new Grid (){
				RowDefinitions = 
				{
					new RowDefinition { Height = display.Height * 0.1 },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = display.Height * 0.05 },
				}
			};

			Grid boolInfo = new Grid () { 
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				WidthRequest = display.Width
			};

			IsWriteable = new Switch (){ HorizontalOptions = LayoutOptions.Center, IsEnabled = false};
			Label IsWriteableLabel = new Label (){ Text = "Write Unlocked", HorizontalOptions = LayoutOptions.Center };

			IsConnected = new Switch (){ HorizontalOptions = LayoutOptions.Center, IsEnabled = false };
			Label IsConnectedLabel = new Label (){ Text = "Tag Connected", HorizontalOptions = LayoutOptions.Center };

			IsNDEFSupported = new Switch (){ HorizontalOptions = LayoutOptions.Center, IsEnabled = false };
			Label IsNDEFSupportedLabel = new Label (){ Text = "NDEF Support", HorizontalOptions = LayoutOptions.Center };

			boolInfo.Children.Add (IsWriteable);
			boolInfo.Children.Add (IsWriteableLabel, 0, 1);

			boolInfo.Children.Add (IsConnected,1,0);
			boolInfo.Children.Add (IsConnectedLabel, 1, 1);

			boolInfo.Children.Add (IsNDEFSupported,2,0);
			boolInfo.Children.Add (IsNDEFSupportedLabel, 2, 1);

			TechList = new ListView ();

			NDEFMessage = new ListView ();

			Button writeButton = new Button (){ Text = "Write Tag" };
            writeButton.Clicked += HandleClicked;

			Label welcomeLabel = new Label {
				Text = "Hello, XForms!" + System.Environment.NewLine + "Scan a tag!",
				XAlign = TextAlignment.Center,
				TextColor = Color.Black,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};

			welcomePanel = new StackLayout (){ 
				Children = { welcomeLabel } ,
				BackgroundColor = Color.White 
			};

			mainGrid.Children.Add (boolInfo);
			mainGrid.Children.Add (TechList, 0, 1);
			mainGrid.Children.Add (NDEFMessage, 0, 2);
			mainGrid.Children.Add (writeButton, 0, 3);
			mainGrid.Children.Add (welcomePanel, 0, 1, 0, 4);

	    	Content = mainGrid;	

		}

        void device_TagDisconnected(object sender, XTag e)
        {
#if SILVERLIGHT
            System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                IsConnected.IsToggled = false;
            });
#else
            IsConnected.IsToggled = false;
#endif

        }

        void device_TagConnected(object sender, XTag e)
        {
#if SILVERLIGHT
            System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                IsConnected.IsToggled = true;
            });
#else
            IsConnected.IsToggled = true;
#endif

        }

        void HandleClicked(object sender, EventArgs e)
        {
            var spRecord = new NdefSpRecord
            {
                Uri = "www.poz1.com",
                NfcAction = NdefSpActRecord.NfcActionType.DoAction,
            };
            spRecord.AddTitle(new NdefTextRecord
            {
                Text = "XNFC - XamarinForms - Poz1.com",
                LanguageCode = "en"
            });
            // Add record to NDEF message
            var msg = new NdefMessage { spRecord };
            try
            {
                device.WriteTag(msg);
            }
            catch (Exception excp)
            {
                DisplayAlert("Error", excp.Message, "OK");
            }
        }

        private ObservableCollection<string> readNDEFMEssage(NdefMessage message)
        {

            ObservableCollection<string> collection = new ObservableCollection<string>();
            foreach (NdefRecord record in message)
            {
                // Go through each record, check if it's a Smart Poster
                if (record.CheckSpecializedType(false) == typeof(NdefSpRecord))
                {
                    // Convert and extract Smart Poster info
                    var spRecord = new NdefSpRecord(record);
                    collection.Add("URI: " + spRecord.Uri);
                    collection.Add("Titles: " + spRecord.TitleCount());
                    collection.Add("1. Title: " + spRecord.Titles[0].Text);
                    collection.Add("Action set: " + spRecord.ActionInUse());
                }

                if (record.CheckSpecializedType(false) == typeof(NdefUriRecord))
                {
                    // Convert and extract Smart Poster info
                    var spRecord = new NdefUriRecord(record);
                    collection.Add("Text: " + spRecord.Uri);
                }
            }
            return collection;
        }

        void HandleNewTag(object sender, XTag e)
        {
#if SILVERLIGHT
            System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
            {

                welcomePanel.IsVisible = false;

                IsWriteable.IsToggled = e.IsWriteable;
                IsNDEFSupported.IsToggled = e.IsNDEFSupported;

                if (TechList != null)
                    TechList.ItemsSource = e.TechList;

                if (e.IsNDEFSupported)
                    NDEFMessage.ItemsSource = readNDEFMEssage(e.NDEFMessage);
            });
#else
            welcomePanel.IsVisible = false;

            IsWriteable.IsToggled = e.IsWriteable;
            IsNDEFSupported.IsToggled = e.IsNDEFSupported;

            if(TechList != null)
                TechList.ItemsSource = e.TechList;

            if (e.IsNDEFSupported)
                NDEFMessage.ItemsSource = readNDEFMEssage(e.NDEFMessage);
#endif
        }

	}
}


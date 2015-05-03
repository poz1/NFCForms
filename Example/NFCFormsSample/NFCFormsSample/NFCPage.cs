using NdefLibrary.Ndef;
using Poz1.NFCForms.Abstract;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace NFCFormsSample
{
    public class NFCPage : ContentPage
    {
        private readonly INfcForms device;
        private StackLayout welcomePanel;

        private Switch IsWriteable;
        private Switch IsConnected;
        private Switch IsNDEFSupported;

        private ListView TechList;
        private ListView NDEFMessage;

        public NFCPage()
        {
            device = DependencyService.Get<INfcForms>();
            device.NewTag += HandleNewTag;
            device.TagConnected += device_TagConnected;
            device.TagDisconnected += device_TagDisconnected;

            Grid mainGrid = new Grid()
            {
                RowDefinitions = 
				{
					new RowDefinition { Height = new GridLength(10,GridUnitType.Star)},
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height =  new GridLength(5,GridUnitType.Star)}
				}
            };

            Grid boolInfo = new Grid()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            IsWriteable = new Switch() { HorizontalOptions = LayoutOptions.Center, IsEnabled = false };
            Label IsWriteableLabel = new Label() { Text = "Write Unlocked", HorizontalOptions = LayoutOptions.Center };

            IsConnected = new Switch() { HorizontalOptions = LayoutOptions.Center, IsEnabled = false };
            Label IsConnectedLabel = new Label() { Text = "Tag Connected", HorizontalOptions = LayoutOptions.Center };

            IsNDEFSupported = new Switch() { HorizontalOptions = LayoutOptions.Center, IsEnabled = false };
            Label IsNDEFSupportedLabel = new Label() { Text = "NDEF Support", HorizontalOptions = LayoutOptions.Center };

            boolInfo.Children.Add(IsWriteable);
            boolInfo.Children.Add(IsWriteableLabel, 0, 1);

            boolInfo.Children.Add(IsConnected, 1, 0);
            boolInfo.Children.Add(IsConnectedLabel, 1, 1);

            boolInfo.Children.Add(IsNDEFSupported, 2, 0);
            boolInfo.Children.Add(IsNDEFSupportedLabel, 2, 1);

            TechList = new ListView();

            NDEFMessage = new ListView();

            Button writeButton = new Button() { Text = "Write Tag" };
            writeButton.Clicked += HandleClicked;

            Label welcomeLabel = new Label
            {
                Text = "Hello, XForms!" + System.Environment.NewLine + "Scan a tag!",
                XAlign = TextAlignment.Center,
                TextColor = Color.Black,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            welcomePanel = new StackLayout()
            {
                Children = { welcomeLabel },
                BackgroundColor = Color.White
            };

            mainGrid.Children.Add(boolInfo);
            mainGrid.Children.Add(TechList, 0, 1);
            mainGrid.Children.Add(NDEFMessage, 0, 2);
            mainGrid.Children.Add(writeButton, 0, 3);
            mainGrid.Children.Add(welcomePanel, 0, 1, 0, 4);

            Content = mainGrid;

        }

        void device_TagDisconnected(object sender, NfcFormsTag e)
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

        void device_TagConnected(object sender, NfcFormsTag e)
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
                Text = "NFCForms - XamarinForms - Poz1.com",
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

        void HandleNewTag(object sender, NfcFormsTag e)
        {

#if SILVERLIGHT
            System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
            {

                welcomePanel.IsVisible = false;

                IsWriteable.IsToggled = e.IsWriteable;
                IsNDEFSupported.IsToggled = e.IsNdefSupported;

                if (TechList != null)
                    TechList.ItemsSource = e.TechList;

                if (e.IsNdefSupported)
                    NDEFMessage.ItemsSource = readNDEFMEssage(e.NdefMessage);
            });
#else
            welcomePanel.IsVisible = false;

            IsWriteable.IsToggled = e.IsWriteable;
            IsNDEFSupported.IsToggled = e.IsNdefSupported;

            if(TechList != null)
                TechList.ItemsSource = e.TechList;

            if (e.IsNdefSupported)
                NDEFMessage.ItemsSource = readNDEFMEssage(e.NdefMessage);
#endif
        }

    }
}
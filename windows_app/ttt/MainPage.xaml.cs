using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ttt.Resources;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using System.IO;
using System.Runtime.Serialization;
using System.IO.IsolatedStorage;
//using Windows.Storage.Streams;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Phone;

namespace ttt
{
    public partial class MainPage : PhoneApplicationPage
    {
        //WebClient wc = new WebClient();
        //HttpWebRequest webRequest;
        byte[] sbytedata;
        IsolatedStorageFile myIsolatedStorage;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
             

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }


       

        private void but(object sender, EventArgs e)
        {
            PhotoChooserTask task = new PhotoChooserTask();
            task.Completed += task_Completed;
            task.Show();
           // wc.OpenReadCompleted += OnOpenReadCompleted;
        }



        private void task_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
               
                sbytedata = ReadToEnd(e.ChosenPhoto);
                string s = sbytedata.ToString();
                Uri u = new Uri("http://169.254.80.80:5000/image/");
                HttpWebRequest webRequest;
                webRequest = (HttpWebRequest)HttpWebRequest.CreateHttp(u);
                webRequest.Method = "POST";
                webRequest.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), webRequest);
              

            }
            
        }
        void GetRequestStreamCallback(IAsyncResult callbackResult)
        {
            HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;
            // End the stream request operation
            Stream postStream = myRequest.EndGetRequestStream(callbackResult);
            postStream.Write(sbytedata, 0, sbytedata.Length);
            postStream.Close();

            // Start the web request
            myRequest.BeginGetResponse(new AsyncCallback(GetResponsetStreamCallback), myRequest);
        }
        void GetResponsetStreamCallback(IAsyncResult callbackResult)
        {

            HttpWebRequest request = (HttpWebRequest)callbackResult.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(callbackResult);
            Stream receiveStream = response.GetResponseStream();
            byte[] b = null;
            using (BinaryReader br = new BinaryReader(receiveStream))
            {
                b = br.ReadBytes(500000);
                br.Close();
            }
            MemoryStream memoryStream = new MemoryStream(b);
            memoryStream.Position = 0;

            BitmapImage bitmapImage = null;
            this.Dispatcher.BeginInvoke(() =>
            {
              
                bitmapImage = new BitmapImage { CreateOptions = BitmapCreateOptions.None };
                bitmapImage.SetSource(memoryStream);
                image1.Source = bitmapImage;


            });

          
           
        }
        
       

   
    public static byte[] ReadToEnd(System.IO.Stream stream)
    {
        long originalPosition = stream.Position;
        stream.Position = 0;

        try
        {
            byte[] readBuffer = new byte[4096];

            int totalBytesRead = 0;
            int bytesRead;

            while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
            {
                totalBytesRead += bytesRead;

                if (totalBytesRead == readBuffer.Length)
                {
                    int nextByte = stream.ReadByte();
                    if (nextByte != -1)
                    {
                        byte[] temp = new byte[readBuffer.Length * 2];
                        Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                        Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                        readBuffer = temp;
                        totalBytesRead++;
                    }
                }
            }

            byte[] buffer = readBuffer;
            if (readBuffer.Length != totalBytesRead)
            {
                buffer = new byte[totalBytesRead];
                Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
            }
            return buffer;
        }
        finally
        {
            stream.Position = originalPosition;
        }
    
        }


   

        
    }
}
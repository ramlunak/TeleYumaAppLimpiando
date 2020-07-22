using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using TeleYumaApp;
using TeleYumaApp.Droid;
using TeleYumaApp.Droid.Renderers;
using Xamarin.Forms;

[assembly: Dependency(typeof(PicturePickerImplementation))]
namespace TeleYumaApp.Droid.Renderers
{
    public class PicturePickerImplementation : IPicturePicker
    {
      
        public async Task<Stream> GetImageStreamAsync()
        {

            // Define the Intent for getting images
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);

            // Get the MainActivity instance
            MainActivity activity = Forms.Context as MainActivity;

            // Start the picture-picker activity (resumes in MainActivity.cs)
            activity.StartActivityForResult(
                Intent.CreateChooser(intent, "Select Picture"),
                MainActivity.PickImageId);

            // Save the TaskCompletionSource object as a MainActivity property
            activity.PickImageTaskCompletionSource = new TaskCompletionSource<Stream>();

            // Return Task object
            return await activity.PickImageTaskCompletionSource.Task;

            //Bitmap bitmap = BitmapFactory.DecodeStream(stream);
            //MemoryStream MemoStream = new MemoryStream();
            //bitmap.Compress(Bitmap.CompressFormat.Webp, 100, MemoStream);
            //byte[] bytes = MemoStream.ToArray();
            //return bytes;
            //  return activity.ImageByteArray;

        }
    }
}
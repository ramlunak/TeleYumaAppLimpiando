using System;
using System.Collections.Generic;
using System.Text;

namespace TeleYumaApp
{
    public interface IMediaService
    {
        byte[] ResizeImage(byte[] imageData, float width, float height);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using TeleYumaApp.Class;

namespace TeleYumaApp
{
    interface IPushNotification
    {       
        void SetNotification(Esms sms);
    }
}

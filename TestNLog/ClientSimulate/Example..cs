﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientSimulate
{

    public class ClientResult
    {

        public List<Example> list { get; set; }
        public int count { get; set; }
    }
    public class Example
    {
        public int Index { get; set; }
        public int TransId { get; set; }
        public int UserId { get; set; }
        public string UserToken { get; set; }
        public int MessageType { get; set; }
        public string TopicName { get; set; }
        public string LandingURL { get; set; }
        public int ThemeId { get; set; }
        public string FontNotificationColor { get; set; }
        public int TopRangeNotification { get; set; }
        public int DisplayPersent { get; set; }
        public int AutoRedirect { get; set; }
        public int ShowCloseBtn { get; set; }
        public int RemoveRead { get; set; }
        public int DispSender { get; set; }
        public object URLIcon { get; set; }
        public string DisplayTime { get; set; }
        public object NewsLoop { get; set; }
        public int Speed { get; set; }
        public string SenderName { get; set; }
        public string OSName { get; set; }
        public string DeviceName { get; set; }
        public string DeviceId { get; set; }
        public string FontColor { get; set; }
        public string AlertTime { get; set; }
        public string BGColor { get; set; }
        public string DetailColor { get; set; }
        public int CompanyId { get; set; }
        public string IPAddress { get; set; }
        public string DeviceType { get; set; }
        public bool ReadFlag { get; set; }
        public bool FCMFlag { get; set; }
        public object FCMToken { get; set; }
    }

    public class Stock
    { 

        public string Symbol { get; set; }

        public decimal DayOpen { get; set; }

        public decimal DayLow { get; set; }

        public decimal DayHigh { get; set; }

        public decimal LastChange { get; set; }

        public decimal Change { get; set; }

        public double PercentChange { get; set; }

        public decimal Price { get; set; }
    }
}

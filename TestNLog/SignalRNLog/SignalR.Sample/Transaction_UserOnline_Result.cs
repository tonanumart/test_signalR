using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRNLog.SignalR.Sample
{
    public class Transaction_UserOnline_Result
    {
         public Nullable<long> Index { get; set; }
        public decimal TransId { get; set; }
        public decimal UserId { get; set; }
        public string UserToken { get; set; }
        public Nullable<int> MessageType { get; set; }

        private string _topicName;
        public string TopicName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_topicName))
                    return string.Empty;
                return _topicName.Replace("&", "&&");
            }
            set
            {
                _topicName = value;
            }
        }

        public string LandingURL { get; set; }
        public Nullable<int> ThemeId { get; set; }
        public string FontNotificationColor { get; set; }
        public Nullable<int> TopRangeNotification { get; set; }
        public Nullable<int> DisplayPersent { get; set; }
        public Nullable<bool> AutoRedirect { get; set; }
        public Nullable<bool> ShowCloseBtn { get; set; }
        public Nullable<bool> RemoveRead { get; set; }
        public Nullable<bool> DispSender { get; set; }
        public string URLIcon { get; set; }
        public Nullable<int> DisplayTime { get; set; }
        public Nullable<int> NewsLoop { get; set; }
        public Nullable<decimal> Speed { get; set; }
        public string SenderName { get; set; }
        public string OSName { get; set; }
        public string DeviceName { get; set; }
        public string DeviceId { get; set; }
        public string FontColor { get; set; }
        public Nullable<System.DateTime> AlertTime { get; set; }
        public string BGColor { get; set; }
        public string DetailColor { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public string IPAddress { get; set; }
        public decimal? RequestId { get; set; }
        public decimal? ViewId { get; set; }
        public decimal? OpenId { get; set; }
        public bool IsView { get; set; }

        public Transaction_UserOnline_Result()
        {
            IsView = false;
        }
    }
}
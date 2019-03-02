using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestNLog.Background
{

    public class MyUserState<T>
    {
        public double Percent { get; set; }
        public T EnumValue { get; set; }
    }

    public class SignalRSResult : MyUserState<SignalRStatus>
    { 
    }

    public class NetworkResult : MyUserState<SignalRStatus>
    {
    }

    public class UpdateResult : MyUserState<SignalRStatus>
    {
    }

    public class BackgroundStatus
    {
        public BackgroundStatus()
        {
            SignalR = SignalRStatus.Waiting;
            Network = NetworkStatus.Waiting;
            Update = UpdateStatus.Waiting;
        }

        public SignalRStatus SignalR { get; set; }
        public NetworkStatus Network { get; set; }
        public UpdateStatus Update { get; set; }
    }


    public enum SignalRStatus
    {
        Waiting = 0, Normal = 1
    }

    public enum NetworkStatus
    {
        Waiting = 0, Normal = 1
    }

    public enum UpdateStatus
    {
        Waiting = 0, Normal = 1
    }
}

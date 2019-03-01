using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRNLog.SignalR.Sample
{
    public class Example
    {
        public int Index { get; set; }
        public decimal TransId { get; set; }
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


    public class MessageFactory
    {
        private static string Mess_4kb = "[{\"TransId\":7979.0,\"TopicName\":\"[โปรโมชั่น] แจ้งกดจบโปร Instore พิเศษ เคซีเอฟไข่ไก่สดแพ็ค 10 วันที่ 25 ม.ค.-31 มี.ค. 62\",\"LandingURL\":\"http://smartkorp.com/Client/Questionaire?transId=7979&qry=hcnQxMTMxfDE1Nw==\",\"URLIcon\":null,\"AlertTime\":\"2019-01-31T11:00:00\",\"ReadFlag\":false},{\"TransId\":7998.0,\"TopicName\":\"[แจ้งปฏิบัติ] แจ้งเกณฑ์มาตรฐานการตรวจสอบ ประจำปี 2562\",\"LandingURL\":\"http://smartkorp.com/Client/Questionaire?transId=7998&qry=hcnQxMDkwfDE1Nw==\",\"URLIcon\":null,\"AlertTime\":\"2019-01-31T11:00:00\",\"ReadFlag\":false},{\"TransId\":7996.0,\"TopicName\":\"[แจ้งปฏิบัติ] แจ้งทีม Set up เข้าสาขาเพื่อดำเนินการตามแผนปรับร้าน โปรเจค Space Allocation วันที่ 6-8 ก.พ. 62 เฉพาะสาขาในคลองบางปลากด\",\"LandingURL\":\"http://smartkorp.com/Client/Questionaire?transId=7996&qry=hcnQxMDkwfDE1Nw==\",\"URLIcon\":null,\"AlertTime\":\"2019-01-31T11:00:00\",\"ReadFlag\":false},{\"TransId\":7993.0,\"TopicName\":\"[แจ้งปฏิบัติ] แจ้งทีม Set up เข้าสาขาเพื่อดำเนินการตามแผนปรับร้าน โปรเจค Space Allocation วันที่ 5-7 ก.พ. 62 เฉพาะสาขาท่าข้าม\",\"LandingURL\":\"http://smartkorp.com/Client/Questionaire?transId=7993&qry=hcnQxMDkwfDE1Nw==\",\"URLIcon\":null,\"AlertTime\":\"2019-01-31T11:00:00\",\"ReadFlag\":false},{\"TransId\":7989.0,\"TopicName\":\"[แจ้งปฏิบัติ] แจ้งจัดเรียงสินค้า, ติดตั้งตู้ SASHA ประจำเดือน ก.พ. 62 เฉพาะ 150 สาขา\",\"LandingURL\":\"http://smartkorp.com/Client/Questionaire?transId=7989&qry=hcnQxMDkwfDE1Nw==\",\"URLIcon\":null,\"AlertTime\":\"2019-01-31T11:00:00\",\"ReadFlag\":false},{\"TransId\":7988.0,\"TopicName\":\"[แจ้งปฏิบัติ] แจ้งรายละเอียด “กิจกรรมซิมเพนกวิน เฉพาะลูกค้าสบายการ์ด” วันที่ 1-28 ก.พ. 62\",\"LandingURL\":\"http://smartkorp.com/Client/Questionaire?transId=7988&qry=hcnQxMDkwfDE1Nw==\",\"URLIcon\":null,\"AlertTime\":\"2019-01-31T11:00:00\",\"ReadFlag\":false},{\"TransId\":7987.0,\"TopicName\":\"[แจ้งปฎิบัติ] แจ้งตรวจสอบลังพลาสติกเปล่าทุกประเภท ก่อนส่งกลับคืนคลัง จะต้องไม่มีสินค้า หรือขยะติดลังพลาสติกอยู่ภายในลัง\",\"LandingURL\":\"http://smartkorp.com/Client/Questionaire?transId=7987&qry=hcnQxMDkwfDE1Nw==\",\"URLIcon\":null,\"AlertTime\":\"2019-01-31T11:00:00\",\"ReadFlag\":false},{\"TransId\":7986.0,\"TopicName\":\"[แจ้งปฎิบัติ] แจ้งอัพเดทตารางจัด-ส่งสินค้า คลังปกติและคลังเย็น ประจำวันที่ 31 ม.ค. 62\",\"LandingURL\":\"http://smartkorp.com/Client/Questionaire?transId=7986&qry=hcnQxMDkwfDE1Nw==\",\"URLIcon\":null,\"AlertTime\":\"2019-01-31T11:00:00\",\"ReadFlag\":false},{\"TransId\":7999.0,\"TopicName\":\"[โปรโมชั่น] แจ้งติดสื่อประชาสัมพันธ์รับชำระ e-money เฉพาะ 60 สาขา\",\"LandingURL\":\"http://smartkorp.com/Client/Questionaire?transId=7999&qry=hcnQxMDkwfDE1Nw==\",\"URLIcon\":null,\"AlertTime\":\"2019-01-31T11:00:00\",\"ReadFlag\":false},{\"TransId\":7990.0,\"TopicName\":\"[โปรโมชั่น] แจ้งติดสื่อตกแต่งชั้น Mini Lifestyle วันที่ 2 ก.พ. 62 เป็นต้นไป เฉพาะ 222 สาขา\",\"LandingURL\":\"http://smartkorp.com/Client/Questionaire?transId=7990&qry=hcnQxMDkwfDE1Nw==\",\"URLIcon\":null,\"AlertTime\":\"2019-01-31T11:00:00\",\"ReadFlag\":false},{\"TransId\":7984.0,\"TopicName\":\"[โปรโมชั่น] แจ้งพิมพ์ป้ายราคา เนสกาแฟ, เอส, คลอเร็ท,ทิคกี้บิสกิต, เซ็นซี่ผ้าอ้อมผู้ใหญ่, Heroถุงขยะ, เกสรน้ำมันปาล์ม, กลุ่มคุ้กกี้, ลูกอม เริ่มวันที่ 1 ก.พ. 62 (27 รายการ)\",\"LandingURL\":\"http://smartkorp.com/Client/Questionaire?transId=7984&qry=hcnQxMDkwfDE1Nw==\",\"URLIcon\":null,\"AlertTime\":\"2019-01-31T11:00:00\",\"ReadFlag\":false},{\"TransId\":7983.0,\"TopicName\":\"[โปรโมชั่น] แจ้งพิมพ์ป้ายราคา  กลุ่มอาหารเสริม (NINE9) วันที่ 1 ก.พ.-30 เม.ย.62 (6 รายการ)\",\"LandingURL\":\"http://smartkorp.com/Client/Questionaire?transId=7983&qry=hcnQxMDkwfDE1Nw==\",\"URLIcon\":null,\"AlertTime\":\"2019-01-31T11:00:00\",\"ReadFlag\":false},{\"TransId\":7982.0,\"TopicName\":\"[โปรโมชั่น] แจ้งพิมพ์ป้ายราคา เซนส์แอนด์ซอฟท์แผ่นรองซับ วันที่ 1-15 ก.พ. 62 (2 รายการ)\",\"LandingURL\":\"http://smartkorp.com/Client/Questionaire?transId=7982&qry=hcnQxMDkwfDE1Nw==\",\"URLIcon\":null,\"AlertTime\":\"2019-01-31T11:00:00\",\"ReadFlag\":false}]";
        public static List<Example> MockOver_4Kb()
        {
            return JsonConvert.DeserializeObject<List<Example>>(Mess_4kb);
        }
    }
}
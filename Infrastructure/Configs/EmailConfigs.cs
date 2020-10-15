using System;

namespace CourseStudio.Lib.Configs
{
    public class EmailConfigs
    {
        public string SendGridApiKey { get; set; }
        public string SenderGeneral { get; set; }
        public string SenderDoNotReply { get; set; }
        public string RecieverGeneral { get; set; }
        public string RequestCode { get; set; }
        public string RegistrationConfirmUrl { get; set; }
        public string ForgetPasswordUrl { get; set; }
        public string CouponRedeemUrl { get; set; }
    }
}

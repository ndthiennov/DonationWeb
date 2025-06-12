using Twilio.Rest.Verify.V2.Service;
using Twilio;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Security.Cryptography;
using DonationAppDemo.DTOs;
using DonationAppDemo.Helper;
using System;
using Twilio.TwiML.Voice;
using Twilio.Http;
//using DonationAppDemo.Models;
using Twilio.Jwt.AccessToken;
using DonationAppDemo.Services.Interfaces;
using Newtonsoft.Json;
using ZaloPay.Helper.Crypto;
using ZaloPay.Helper;
using System.Globalization;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin.Messaging;
using Newtonsoft.Json.Linq;

namespace DonationAppDemo.Services
{
    public class UtilitiesService : IUtilitiesService
    {
        private readonly IConfiguration _config;
        private readonly Cloudinary _cloudinary;

        private static FirebaseApp? firebaseApp;

        public UtilitiesService(IConfiguration config)
        {
            _config = config;

            // Cloudinary
            Account account = new Account(
                _config.GetValue<string>("CloudinarySettings:CloudName"),
                _config.GetValue<string>("CloudinarySettings:ApiKey"),
                _config.GetValue<string>("CloudinarySettings:ApiSecret"));

            _cloudinary = new Cloudinary(account);

            // Firebase cloud messaging
            if (firebaseApp == null)
            {
                try
                {
                    // This will only create a new FirebaseApp if it doesn't already exist.
                    if (FirebaseApp.DefaultInstance == null)
                    {
                        FirebaseApp.Create(new AppOptions()
                        {
                            Credential = GoogleCredential.FromFile(_config["FirebaseSetting:ServiceAccountPath"])
                        });
                    }
                }
                catch (System.Exception ex)
                {
                    // Handle the exception if necessary
                    Console.WriteLine($"Error initializing Firebase: {ex.Message}");
                }
            }


            /*if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(_config["FirebaseSetting:ServiceAccountPath"])
                });
            }*/
        }
        public async Task<string> TwilioSendCodeSms(string phoneNum)
        {
            string accountSid = _config.GetValue<string>("TwilioSettings:AccountSid");
            string authToken = _config.GetValue<string>("TwilioSettings:AuthToken");

            TwilioClient.Init(accountSid, authToken);

            var verification = await VerificationResource.CreateAsync(
                to: phoneNum,
                channel: "sms",
                pathServiceSid: _config.GetValue<string>("TwilioSettings:PathServiceSid"));

            return verification.Sid;
        }
        public async Task<bool?> TwilioVerifyCodeSms(string? code, string? phoneNum)
        {
            string accountSid = _config.GetValue<string>("TwilioSettings:AccountSid");
            string authToken = _config.GetValue<string>("TwilioSettings:AuthToken");

            TwilioClient.Init(accountSid, authToken);

            var verificationCheck = await VerificationCheckResource.CreateAsync(
                to: phoneNum, code: code, pathServiceSid: _config.GetValue<string>("TwilioSettings:PathServiceSid"));

            return verificationCheck.Valid;
        }
        public async Task<ImageUploadResult> CloudinaryUploadPhotoAsync(IFormFile photo)
        {
            var uploadResult = new ImageUploadResult();
            if (photo.Length > 0)
            {
                using var stream = photo.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(photo.FileName, stream),
                    /*Transformation = new Transformation()
                        .Height(1517).Width(1024)*/
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }
        public async Task<DeletionResult> CloudinaryDeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var deleteResult = await _cloudinary.DestroyAsync(deleteParams);
            return deleteResult;
        }
        public async Task<string> VnPayCreatePaymentUrl(HttpContext context, PaymentRequestDto requestDto)
        {
            // Get Config Info
            string vnp_Returnurl = _config.GetValue<string>("VnPaySettings:PaymentReturnUrl");
            string vnp_Url = _config.GetValue<string>("VnPaySettings:BaseUrl");
            string vnp_TmnCode = _config.GetValue<string>("VnPaySettings:TmnCode");
            string vnp_HashSecret = _config.GetValue<string>("VnPaySettings:HashSecret");

            // Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ.Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần(khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            vnpay.AddRequestData("vnp_Amount", (requestDto.Amount * 100).ToString());
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Helper.Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", $"{requestDto.UserId} {requestDto.UserRole} donated campaign id {requestDto.CampaignId}");
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);

            int roleNum = 0;
            switch (requestDto.UserRole)
            {
                case "admin": roleNum = 1; break;
                case "organiser": roleNum = 2; break;
                case "donor": roleNum = 3; break;
                default: roleNum = 0; break;
            }
            // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY.Không được trùng lặp trong ngày
            vnpay.AddRequestData("vnp_TxnRef", roleNum.ToString() + requestDto.UserId.ToString() + DateTime.Now.Ticks.ToString());

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);

            return paymentUrl;
        }
        public async Task<PaymentResponseDto> VnPayPaymentExecute(IQueryCollection collections)
        {
            VnPayLibrary vnpay = new VnPayLibrary();
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            //Lay danh sach tham so tra ve tu VNPAY
            //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
            //vnp_TransactionNo: Ma GD tai he thong VNPAY
            //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
            //vnp_SecureHash: HmacSHA512 cua du lieu tra ve

            string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            string vnp_date = vnpay.GetResponseData("vnp_PayDate");
            //long vnp_OrderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            string vnp_OrderId = vnpay.GetResponseData("vnp_TxnRef");
            decimal vnp_Amount = Decimal.Parse(vnpay.GetResponseData("vnp_Amount")) / 100;
            long vnpay_TranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            //string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
            string vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            string vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _config.GetValue<string>("VnPaySettings:HashSecret"));
            if (!checkSignature)
            {
                throw new Exception("VnPay execution failed");
            }

            // Get user id and campaign id
            var id = vnp_OrderInfo.Split(' ');

            return new PaymentResponseDto
            {
                PaymentResponse = true,
                PaymentMethodId = 1,
                PaymentDescription = vnp_OrderInfo,
                PaymentOrderId = vnp_OrderId.ToString(),
                PaymentTransactionId = vnpay_TranId.ToString(),
                PaymentToken = vnp_SecureHash,
                PaymentDate = DateTime.ParseExact(vnp_date, "yyyyMMddHHmmss", CultureInfo.InvariantCulture),
                Amount = vnp_Amount,
                UserId = Int32.Parse(id[0]),
                CampaignId = Int32.Parse(id[id.Length - 1])
            };
        }
        public async Task<string> ZaloPayCreatePaymentUrl(PaymentRequestDto requestDto)
        {
            // Get Config Info
            string zalopay_AppId = _config.GetValue<string>("ZaloPaySettings:AppId");
            string zalopay_Key1 = _config.GetValue<string>("ZaloPaySettings:Key1");
            string zalopay_BaseUrl = _config.GetValue<string>("ZaloPaySettings:BaseUrl");
            string zalopay_PaymentReturnUrl = _config.GetValue<string>("ZaloPaySettings:PaymentReturnUrl");

            int roleNum = 0;
            switch (requestDto.UserRole)
            {
                case "admin": roleNum = 1; break;
                case "organiser": roleNum = 2; break;
                case "donor": roleNum = 3; break;
                default: roleNum = 0; break;
            }
            var transid = DateTime.Now.Ticks.ToString() + "_" + $"{roleNum}_{requestDto.UserId}_{requestDto.CampaignId}";
            var embeddata = new { redirecturl = zalopay_PaymentReturnUrl };
            var items = new[] { new { itemid = "", itemname = "", itemprice = 0, itemquantity = 0 } };

            var param = new Dictionary<string, string>();

            param.Add("appid", zalopay_AppId);
            param.Add("appuser", $"{roleNum}{requestDto.UserId}"); // role + user id
            param.Add("apptime", ZaloPay.Helper.Utils.GetTimeStamp().ToString());
            param.Add("amount", $"{requestDto.Amount}");
            param.Add("apptransid", DateTime.Now.ToString("yyMMdd") + "_" + transid); // mã giao dich có định dạng yyMMdd_xxxx
            param.Add("embeddata", JsonConvert.SerializeObject(embeddata));
            param.Add("item", JsonConvert.SerializeObject(items));
            param.Add("description", $"{requestDto.UserId} {requestDto.UserRole} donated campaign id {requestDto.CampaignId}");
            param.Add("bankcode", "zalopayapp");

            var data = zalopay_AppId + "|" + param["apptransid"] + "|" + param["appuser"] + "|" + param["amount"] + "|"
                + param["apptime"] + "|" + param["embeddata"] + "|" + param["item"];
            param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, zalopay_Key1, data));

            var result = await HttpHelper.PostFormAsync(zalopay_BaseUrl, param);

            string paymentUrl = "";
            foreach (var (key, value) in result)
            {
                if (!string.IsNullOrEmpty(key) && key.Equals("orderurl"))
                {
                    paymentUrl = value.ToString();
                    break;
                }
            }
            return paymentUrl;
        }
        public async Task<PaymentResponseDto> ZaloPayPaymentExecute(IQueryCollection collections)
        {
            // Get Config Info
            string zalopay_AppId = _config.GetValue<string>("ZaloPaySettings:AppId");
            string zalopay_Key1 = _config.GetValue<string>("ZaloPaySettings:Key1");
            string zalopay_CallBackUrl = _config.GetValue<string>("ZaloPaySettings:CallBackUrl");

            var app_trans_id = "";
            decimal amount = 0;

            int i = 0;
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.Equals("apptransid"))
                {
                    app_trans_id = value.ToString();
                    i++;
                }

                if (!string.IsNullOrEmpty(key) && key.Equals("amount"))
                {
                    amount = Decimal.Parse(value);
                    i++;
                }

                if (i == 2)
                {
                    break;
                }
            }

            var id = app_trans_id.Split("_");
            int userId = Int32.Parse(id[id.Length - 2]);
            int campaignId = Int32.Parse(id[id.Length - 1]);
            int roleNum = Int32.Parse(id[id.Length - 3]);
            string role = "";
            switch (roleNum)
            {
                case 1: role = "amin"; break;
                case 2: role = "organiser"; break;
                case 3: role = "donor"; break;
                default: role = ""; break;
            }

            var param = new Dictionary<string, string>();
            param.Add("app_id", zalopay_AppId);
            param.Add("app_trans_id", app_trans_id);
            var data = zalopay_AppId + "|" + app_trans_id + "|" + zalopay_Key1;

            param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, zalopay_Key1, data));

            var result = await HttpHelper.PostFormAsync(zalopay_CallBackUrl, param);

            var zp_trans_id = "";
            foreach (var (key, value) in result)
            {
                if (!string.IsNullOrEmpty(key) && key.Equals("zp_trans_id"))
                {
                    zp_trans_id = value.ToString();
                }
            }

            return new PaymentResponseDto
            {
                PaymentResponse = true,
                PaymentMethodId = 2,
                PaymentDescription = $"{userId} {role} donated campaign id {campaignId}",
                PaymentOrderId = zp_trans_id,
                PaymentTransactionId = app_trans_id,
                PaymentToken = null,
                PaymentDate = DateTime.Now,
                Amount = amount,
                UserId = userId,
                CampaignId = campaignId
            };

        }
        public async Task<string> SendNotification(string? token, string title, string body)
        {
            if (token == null)
            {
                return "No tokens found for sending";
            }

            var message = new FirebaseAdmin.Messaging.Message()
            {
                Token = token,
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = title,
                    Body = body
                }
            };

            // Send a message to the device corresponding to the provided registration token
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return response;
        }
        public async Task<BatchResponse?> SendMultipleNotifications(List<string>? tokens, string title, string body)
        {
            if (tokens == null)
            {
                return null;
            }

            var message = new FirebaseAdmin.Messaging.MulticastMessage()
            {
                Tokens = tokens,
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = title,
                    Body = body
                }
            };

            // Send a message to the device corresponding to the provided registration token
            //var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
            var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);
            return response;
        }
    }
}

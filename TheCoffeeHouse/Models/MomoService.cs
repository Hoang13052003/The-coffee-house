using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using RestSharp;
using System.Threading.Tasks;

namespace TheCoffeeHouse.Models
{
    public class MomoService
    {
        private readonly string _momoApiUrl = ConfigurationManager.AppSettings["MomoApiUrl"];
        private readonly string _secretKey = ConfigurationManager.AppSettings["MomoSecretKey"];
        private readonly string _accessKey = ConfigurationManager.AppSettings["MomoAccessKey"];
        private readonly string _returnUrl = ConfigurationManager.AppSettings["MomoReturnUrl"];
        private readonly string _notifyUrl = ConfigurationManager.AppSettings["MomoNotifyUrl"];
        private readonly string _partnerCode = ConfigurationManager.AppSettings["MomoPartnerCode"];
        private readonly string _requestType = ConfigurationManager.AppSettings["MomoRequestType"]; // Thêm requestType từ config

        public async Task<string> CreatePaymentUrlAsync(long orderId, decimal amount)
        {
            var requestId = Guid.NewGuid().ToString();
            var orderInfo = "Thanh toán đơn hàng " + orderId;

            // Dữ liệu để tạo chữ ký
            var rawData = $"partnerCode={_partnerCode}&accessKey={_accessKey}&requestId={requestId}&amount={amount}&orderId={orderId}&orderInfo={orderInfo}&returnUrl={_returnUrl}&notifyUrl={_notifyUrl}&extraData=";
            var signature = ComputeHmacSha256(rawData, _secretKey);

            var requestBody = new
            {
                partnerCode = _partnerCode,
                accessKey = _accessKey,
                requestId = requestId,
                orderId = orderId,
                amount = amount,
                orderInfo = orderInfo,
                returnUrl = _returnUrl,
                notifyUrl = _notifyUrl,
                requestType = _requestType, // Sử dụng requestType từ config
                extraData = "",
                signature = signature
            };

            var client = new RestClient(_momoApiUrl);
            var request = new RestRequest() { Method = Method.Post };
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(requestBody);

            var response = await client.ExecuteAsync(request);
            // Thêm log để xem response chi tiết khi có lỗi
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine($"Momo API Error - Status Code: {response.StatusCode}");
                Console.WriteLine($"Momo API Error - Content: {response.Content}");
                return "";
            }

            var responseJson = JsonConvert.DeserializeObject<dynamic>(response.Content);
            return responseJson?.payUrl ?? "";
        }

        private string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);
            using (var hmac = new HMACSHA256(keyBytes))
            {
                return BitConverter.ToString(hmac.ComputeHash(messageBytes)).Replace("-", "").ToLower();
            }
        }
    }
}
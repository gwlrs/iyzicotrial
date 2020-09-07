using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IyzicoTrial.Controllers
{
    public class Card : Controller
    {
        public Sepet Sepet { get; set; }
        public Options Options { get; set; }
        public new CreatePaymentRequest Request { get; set; }
        public Card()
        {
            Sepet = new Sepet();
            Options = new Options
            {
                ApiKey = "sandbox-d7MGhkU17icKVq4sPSuPAIkL5pHv9tXm",
                SecretKey = "sandbox-AjSdHujxmCk2AH1JbY2kOlE9ZqYyrD6u",
                BaseUrl = "https://sandbox-api.iyzipay.com"
            };
            Request = new CreatePaymentRequest
            {
                BasketId = "1",
                BasketItems = new List<Iyzipay.Model.BasketItem>()
                {
                    new Iyzipay.Model.BasketItem
                    {
                        Category1 = "Giyim",
                        Category2 = "Ayakkabi",
                        Id = "123321",
                        ItemType = BasketItemType.PHYSICAL.ToString(),
                        Name = "Adidas Spor Ayakkabi",
                        Price = "112.5",
                    }
                },
                BillingAddress = new Address
                {
                    City = "Konya",
                    ContactName = "Mehmet",
                    Country = "Turkey",
                    Description = "Yazır Mah. Aslan Sk. No: 2/6",
                    ZipCode = "42090"
                },
                Buyer = new Buyer
                {
                    Id = "BY789",
                    Name = "John",
                    Surname = "Doe",
                    GsmNumber = "+905350000000",
                    Email = "faceofface_42@hotmail.com",
                    IdentityNumber = "37615834546",
                    LastLoginDate = "2015-10-05 12:43:35",
                    RegistrationDate = "2013-04-21 15:12:09",
                    RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1",
                    Ip = "94.55.173.253",
                    City = "Konya",
                    Country = "Turkey",
                    ZipCode = "42090"
                },
                CallbackUrl = "/Home/Index",
                Locale = Locale.TR.ToString(),
                ConversationId = "123456789",
                Price = "1",
                PaidPrice = "1.2",
                Currency = Currency.TRY.ToString(),
                Installment = 1,
                PaymentChannel = PaymentChannel.WEB.ToString(),
                PaymentGroup = PaymentGroup.PRODUCT.ToString(),
                ShippingAddress = new Address
                {
                    City = "Konya",
                    ContactName = "Mehmet",
                    Country = "Turkey",
                    Description = "Yazır Mah. Aslan Sk. No: 2/6",
                    ZipCode = "42090"
                },
                PaymentCard = new PaymentCard
                {
                    CardHolderName = "John Doe",
                    CardNumber = "5528790000000008",
                    ExpireMonth = "12",
                    ExpireYear = "2030",
                    Cvc = "123",
                    RegisterCard = 0
                },
            };
        }

        public IActionResult MyPayment()
        {

            Payment payment = Payment.Create(Request, Options);


            return View();
        }

        public IActionResult RetriveInstallment()
        {
            RetrieveInstallmentInfoRequest request = new RetrieveInstallmentInfoRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = "123456789";
            request.BinNumber = "540668";
            request.Price = "100";
            InstallmentInfo installmentInfo = InstallmentInfo.Retrieve(request, Options);
            return Json(new object { });
        }

       
    }


    public class Sepet
    {
        public decimal Tutar { get; set; }
    }
}

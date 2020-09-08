using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Iyzipay.Samples
{
    public class CheckoutFormSample : Controller
    {
        public Options options;
        public string _token;

        public CheckoutFormSample()
        {
            options = new Options
            {
                ApiKey = "sandbox-d7MGhkU17icKVq4sPSuPAIkL5pHv9tXm",
                SecretKey = "sandbox-AjSdHujxmCk2AH1JbY2kOlE9ZqYyrD6u",
                BaseUrl = "https://sandbox-api.iyzipay.com"
            };
        }

        public IActionResult Should_Initialize_Checkout_Form()
        {
            CreateCheckoutFormInitializeRequest request = new CreateCheckoutFormInitializeRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = "123456789";
            request.Price = "1";
            request.PaidPrice = "1.2";
            request.Currency = Currency.TRY.ToString();
            request.BasketId = "B67832";
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();
            request.CallbackUrl = "https://localhost:44320/CheckoutFormSample/Should_Retrieve_Checkout_Form_Result";

            List<int> enabledInstallments = new List<int>();
            enabledInstallments.Add(2);
            enabledInstallments.Add(3);
            enabledInstallments.Add(6);
            enabledInstallments.Add(9);
            request.EnabledInstallments = enabledInstallments;

            Buyer buyer = new Buyer();
            buyer.Id = "BY789";
            buyer.Name = "John";
            buyer.Surname = "Doe";
            buyer.GsmNumber = "+905350000000";
            buyer.Email = "email@email.com";
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = "85.34.78.112";
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Jane Doe";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Jane Doe";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem firstBasketItem = new BasketItem();
            firstBasketItem.Id = "BI101";
            firstBasketItem.Name = "Binocular";
            firstBasketItem.Category1 = "Collectibles";
            firstBasketItem.Category2 = "Accessories";
            firstBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            firstBasketItem.Price = "0.3";
            basketItems.Add(firstBasketItem);

            BasketItem secondBasketItem = new BasketItem();
            secondBasketItem.Id = "BI102";
            secondBasketItem.Name = "Game code";
            secondBasketItem.Category1 = "Game";
            secondBasketItem.Category2 = "Online Game Items";
            secondBasketItem.ItemType = BasketItemType.VIRTUAL.ToString();
            secondBasketItem.Price = "0.5";
            basketItems.Add(secondBasketItem);

            BasketItem thirdBasketItem = new BasketItem();
            thirdBasketItem.Id = "BI103";
            thirdBasketItem.Name = "Usb";
            thirdBasketItem.Category1 = "Electronics";
            thirdBasketItem.Category2 = "Usb / Cable";
            thirdBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            thirdBasketItem.Price = "0.2";
            basketItems.Add(thirdBasketItem);
            request.BasketItems = basketItems;

            CheckoutFormInitialize checkoutFormInitialize = CheckoutFormInitialize.Create(request, options);
            _token = checkoutFormInitialize.Token;
            ViewBag.Content = checkoutFormInitialize.CheckoutFormContent;

            return View();
        }

        public IActionResult Should_Retrieve_Checkout_Form_Result(string token)
        {

            RetrieveCheckoutFormRequest request = new RetrieveCheckoutFormRequest();
            request.ConversationId = "123456789";
            request.Token = token;

            CheckoutForm checkoutForm = CheckoutForm.Retrieve(request, options);

            PrintResponse<CheckoutForm>(checkoutForm);

            if (checkoutForm.Status.ToLower() == "success")
            {

                // return Redirect(Uri.EscapeUriString("https://localhost:44320/CheckoutFormSample/OdemeSonucu?message=Ödeme işleminiz başarılı bir şekilde tamamlandı?orderNumber=Siparişiniz Numarınız : 12134352"));
                return RedirectToAction("OdemeSonucu", new { message = "Odeme işleminiz başarılı bir şekilde tamamlandı", orderNumber = "Siparişiniz Numarınız : 12134352" });
            }
            else
            {
                return RedirectToAction("OdemeSonucu", new { message = "Odeme işlemin Başarısız  !!!!!!!!!" });
            }

        }

        //public void Should_Retrieve_Checkout_Form_Result(string token)
        //{
        //    RetrieveCheckoutFormRequest request = new RetrieveCheckoutFormRequest();
        //    request.ConversationId = "123456789";
        //    request.Token = token;

        //    CheckoutForm checkoutForm = CheckoutForm.Retrieve(request, options);


        //    PrintResponse<CheckoutForm>(checkoutForm);
        //}

        public IActionResult OdemeSonucu(string message, string orderNumber)
        {
            ViewBag.MessageInfo = message;
            ViewBag.OrderNumber = orderNumber;
            return View();
        }




        protected void PrintResponse<T>(T resource)
        {
#if NETCORE1 || NETCORE2
            TraceListener consoleListener = new TextWriterTraceListener(System.Console.Out);
#else
            TraceListener consoleListener = new ConsoleTraceListener();
#endif

            Trace.Listeners.Add(consoleListener);
            Trace.WriteLine(JsonConvert.SerializeObject(resource, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));
        }
    }

}

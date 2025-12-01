using CityStoreAPIWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace CityStoreAPIWeb.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {

            TrnOrderMain Obj = new TrnOrderMain();
            Obj.Usercode =4; Obj.Paymode = "Bank"; Obj.TrnId = "TrnId"; Obj.TrnStatus = "Success"; Obj.ShippingName = "Nehaa";
            Obj.ShippingAddress = "Address"; Obj.ShippingMobileNo = "7737361448"; Obj.ShippingEmailId = "satya18031987@gmail.com";
            Obj.ShippingCity = "Bhilwara"; Obj.ShippingZipcode = "311001"; Obj.ShippingStats = "Rajasthan"; 
            Obj.DeliveryFees = 25;  Obj.PromoCodeAmount = 0; Obj.PromoCodeId = 0; Obj.OrderThrough = "App";
            Obj.WalletAmount = 0;Obj.IsWalletDeduct = false;


         
           
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:9080/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.PostAsJsonAsync("api/Order/GenerateOrder", Obj).Result;
            response.EnsureSuccessStatusCode();
            return View();
        }

       
        public ActionResult Orderup()
        {
            TrnOrderDetail Obj = new TrnOrderDetail();
            Obj.OrderId = 88; Obj.AID = 458; Obj.ProductId = 183; Obj.RequestReason = "galat saman"; Obj.Cancelled = true; Obj.Delivered = false; Obj.Dispatched = false; Obj.Returned = false;
         
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:9099/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.PostAsJsonAsync("api/Order/OrderSettlementStatusEdit1", Obj).Result;
            response.EnsureSuccessStatusCode();
            return View();
        }
        public ActionResult Mail1()
        {
            mail Obj = new mail();
            Obj.name = "kjjjjj"; Obj.email = "vvvvvv"; Obj.message = "119";
            Obj.mobile = "9214472437";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:9099/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.PostAsJsonAsync("api/Slider/Mail", Obj).Result;
            response.EnsureSuccessStatusCode();
            return View();
        }

        public ActionResult reg()
        {

      


            register Obj = new register();
            Obj.Name = "arpit ttttt"; Obj.EmailId = "satyachawla1@gmail.com"; Obj.MobileNumber = "7737361449";
            Obj.Address = "9214472437";
             Obj.State = "9214472437";
             Obj.password = "123456"; Obj.gst = "9214472437";
             Obj.City = "1";
             Obj.District = "1";
             Obj.rcode = "frim18";
             Obj.Groupid = 1;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:9080/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.PostAsJsonAsync("api/Slider/CreateNewUser", Obj).Result;
            
            response.EnsureSuccessStatusCode();
            return View();
        }


        public ActionResult reg1()
        {




            ForgotPassword Obj = new ForgotPassword();
            Obj.Username = "7737361445";
           
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:9080/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.PostAsJsonAsync("api/Slider/ForgotPassword", Obj).Result;
            response.EnsureSuccessStatusCode();
            return View();
        }

        
        public ActionResult login()
        {
            getlogin Obj = new getlogin ();

            Obj.username = "7737361448";
            Obj.password = "123456";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:9099/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.PostAsJsonAsync("api/Slider/Login", Obj).Result;
            response.EnsureSuccessStatusCode();
            return View();
        }

        public ActionResult Cart()
        {
            TrnCart Obj = new TrnCart();

            Obj.UserId = 4;
            Obj.pid = "1,2,3";
            Obj.Size = "1,2,3";
            Obj.Color = "1,2,3";
            Obj.pqty = "1,2,3";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:9080/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.PostAsJsonAsync("api/Slider/InsertCart", Obj).Result;
            response.EnsureSuccessStatusCode();
            return View();
        }


        public ActionResult Cart1()
        {
            TrnCart Obj = new TrnCart();
            Obj.UserId = 4;
            Obj.pid = "1";
            Obj.Size = "1";
            Obj.Color = "1";
            Obj.pqty = "1";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:9080/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.PostAsJsonAsync("api/Slider/InsertCart1", Obj).Result;
            response.EnsureSuccessStatusCode();
            return View();
        }

        public ActionResult Token()
        {
            Token Obj = new Token();
            Obj.tokend = "qn9SaBpurLcuUzbPa3YtNGUzLWJT5VyZh2vYfDNa59xbHpY7wvUwecSURSb2LLhRRaSHLUWcsKf9JkUfwxxWKuaEjPGvHdTWfCb5e6yrEvkjWtM9vnhusbHQD55KCxh3ERD452bkeN33xqBY3jWRK9mQckBCwaXB68aARUumnwexYwdGq7cDteaz3W4C7sCtg54Vfw63AAhuQ4ZFrMQPtBG7rgBYeXVdcJN7x2hyxqHs4EnsBzwZW488qEcGdV9bv5qY7pbexZz7DqcgPqVg8BnsLxHuyYGLndEx2SynNTCtksTkEn3MGwVY2Gz8rKtb78VBY9V7u69NsVh2rgtnsvJPKfz7DAPCxr29k36w4CfSgDAZpw2nHMWwq7FekkFHujgCvmCsHb6HfBa6Vsx2U7fwuWeWk8nM9ebUkUxSPaBGDz77LLEJNBzPPBPt9xYmHBWnuz2rY85Gc3xb8PmqLgPyJY4588vKTGjqsTjyEF7xVD3f3js8VU4B6theFYrTbkE9j5Jpj8bWRhKdxWGE6gHky9EyUJzduzLRc4AXmUWNKUwDmFVNBQy4nsc3hgftBvP8ngJYRm7tUEKB9JFF54dQabNKHPALmHfGjNk98TsWU23Ru7j7Hqu792h2gzZc2rB7bt7hsHMUNs5VAnB7DJdx7Q2nVg76ZkAjtsygWmrDCXDhZ9TBshdLEsaMgsjbCRWxPAUEkpscmuauwxjZndfNBF9yRE3PsjhLyCvWhxf5bEHTfY3MPmUz3zwQgJ69LuhMh3fdWVmhCuPrH2FQkhygztCDu9Hn4rt";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://friendsmobile.virtualappstore.in/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.PostAsJsonAsync("api/Slider/Token", Obj).Result;
            response.EnsureSuccessStatusCode();
            return View();
        }



    }
}


 //{"EmailId":"satyachawla1@gmail.com","MobileNumber":"7737361445","Address":"shahpura","ZipCode":"311001","State":"1","password ":"123456","gst":"","City":"Bhilwara","District":"1","Name":"satya narayan khatik","Groupid":"1"}
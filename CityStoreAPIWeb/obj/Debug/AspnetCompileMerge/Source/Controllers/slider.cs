using CityStoreAPIWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Transactions;
using System.Data.Common;
using System.Data.Objects.SqlClient;
using System.Data.Objects;

namespace CityStoreAPIWeb.Controllers
{
    public class sliderController : ApiController
    {
        friendsmobileEntities db = new friendsmobileEntities();
        SqlConnection sqlCon = new SqlConnection(@"Data Source= 103.71.99.32; Database=virtualelectronics;initial catalog=virtualelectronics;User id=virtualelectronics;Password=electronics@55;Max Pool Size=2000;");
        //   ClsQuantity objQty = new ClsQuantity();
        ClsConnection Cnn = new ClsConnection();
        public HttpResponseMessage GetSlider()
        {

            try
            {
                DataTable ds = new DataTable();
                sqlCon.Open();
                string querystring = "select * from appslider order by priority asc ";  // 3521 "            
                SqlCommand cmd = new SqlCommand(querystring, sqlCon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                sqlCon.Close();

                return Request.CreateResponse(HttpStatusCode.OK, ds);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage GetCategory()
        {
            try
            {
                DataTable ds = new DataTable();
                sqlCon.Open();
                string querystring = "select * from M_CategoryMaster";  // 3521 "              
                SqlCommand cmd = new SqlCommand(querystring, sqlCon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                sqlCon.Close();
                return Request.CreateResponse(HttpStatusCode.OK, ds);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage GetBrand()
        {
            try
            {
                DataTable ds = new DataTable();
                sqlCon.Open();
                string querystring = "select * from brand";  // 3521 "           
                SqlCommand cmd = new SqlCommand(querystring, sqlCon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                sqlCon.Close();
                return Request.CreateResponse(HttpStatusCode.OK, ds);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        public HttpResponseMessage GetState()
        {
            try
            {
                DataTable ds = new DataTable();
                sqlCon.Open();
                string querystring = "Select State_Code,State_Name From Loc_State Where State_Active=1";  // 3521 "          
                SqlCommand cmd = new SqlCommand(querystring, sqlCon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                sqlCon.Close();
                return Request.CreateResponse(HttpStatusCode.OK, ds);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        public HttpResponseMessage GetDistrict(string id)
        {
            try
            {
                DataTable ds = new DataTable();
                sqlCon.Open();
                string querystring = "Select District_Name,District_Code from Loc_District WHERE State_Code='" + id + "'";  // 3521 "               
                SqlCommand cmd = new SqlCommand(querystring, sqlCon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                sqlCon.Close();
                return Request.CreateResponse(HttpStatusCode.OK, ds);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        public string GetVersionNo()
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(@"Data Source= 103.71.99.32; Database= friendsmobile;initial catalog=friendsmobile;User id=friendsmobile;Password=friendsmobile@55");
                sqlCon.Open();
                SqlCommand NewSql = new SqlCommand("Select versionNo From M_AppVersionMaster", sqlCon);
                var balanceObj = NewSql.ExecuteScalar();
                sqlCon.Close();
                return balanceObj.ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public HttpResponseMessage CreateNewUser(register register)
        {
            try
            {
               
                Cnn.Open();
                int Firstime = 0, Firstuser = 0, seconduser = 0;
                int Id = Convert.ToInt32(Cnn.ExecuteScalar("Select IsNull(Max(UserId)+1,1) From [register]"));
              //  Firstime = Convert.ToInt32(Cnn.ExecuteScalar("Select Firstime From [Walletamount]"));
                Firstuser = Convert.ToInt32(Cnn.ExecuteScalar("Select Firstuser From [Walletamount]"));
                seconduser = Convert.ToInt32(Cnn.ExecuteScalar("Select seconduser From [Walletamount]"));

                String Sql = "Select Count(*) From register Where rcode='" + register.rcode + "'";
                int Count = Convert.ToInt32(Cnn.ExecuteScalar(Sql).ToString());
                if (Count> 0)
                {
                    Firstime += seconduser;
                }
             
                string code = "frim" + Id;
                Cnn.ExecuteNonQuery("insert into [register] (UserId,Name,EmailId,Address,State,District,City,ZipCode,MobileNumber,password,rts,active,ip,Wallet,OrderCount,Groupid,gst,rcode)  Values ('" + Id + "','" + register.Name + "','" + register.EmailId + "','" + register.Address + "','" + register.State + "','" + register.District + "','" + register.City + "','" + register.ZipCode + "','" + register.MobileNumber + "','" + register.password + "',GetDate(),'1','App','0','1','1','','" + code + "')");
                // Cnn.ExecuteNonQuery("insert into TrnWallet (UserId,OrderId,TrnType,PromoCodeAmount,Amount,IsDeducted,RTS,OrderType,Type,ReMark) values ('" + Id + "','0','Cr','0','" + Firstime + "',0,Getdate(),'','','First time Register Wallet Added') ");
                 if (Count > 0)
                {
                    int Firstuserid = Convert.ToInt32(Cnn.ExecuteScalar("Select userid From [register] where rcode='" + register.rcode + "'"));
                    Cnn.ExecuteNonQuery("update register set Wallet=Wallet+" + seconduser + " where userid=" + Firstuserid + "");
                    Cnn.ExecuteNonQuery("insert into TrnWallet (UserId,OrderId,TrnType,PromoCodeAmount,Amount,IsDeducted,RTS,OrderType,Type,ReMark) values ('" + Firstuserid + "','0','Cr','0','" + seconduser + "',0,Getdate(),'','','Your  Refer Code used by "+register.Name+"  Wallet Amount credited') ");
                 
                }

               
                // grant parent
                 int checkrcode = Convert.ToInt32(Cnn.ExecuteScalar("Select count(*) From [trnrefer] where Fristuser='" + register.rcode + "'"));
                 if (checkrcode == 1)
                 {
                     string rcode = Cnn.ExecuteScalar("Select seconduser From [trnrefer] where Fristuser='" + register.rcode + "'").ToString();
                     int Firstuseidd = Convert.ToInt32(Cnn.ExecuteScalar("Select userid From [register] where rcode='" + rcode + "'"));
                     Cnn.ExecuteNonQuery("insert into TrnWallet (UserId,OrderId,TrnType,PromoCodeAmount,Amount,IsDeducted,RTS,OrderType,Type,ReMark) values ('" + Firstuseidd + "','0','Cr','0','" + Firstuser + "',0,Getdate(),'','','Earn From Child Refer Code Wallet Amount credited') ");
                     Cnn.ExecuteNonQuery("update register set Wallet=Wallet+" + Firstuser + " where userid=" + Firstuseidd + "");
                 }

                 // parent
                 if (register.rcode.Length > 0)
                 {
                     Cnn.ExecuteNonQuery("insert into trnrefer (Fristuser,seconduser,rts) values ('" + code + "','" + register.rcode + "',Getdate()) ");
                 }
               
                Cnn.Close();
                Msg.SendEmail(register.EmailId, "Registration Successful", "<html><div style='height:520px;width:521px;border:solid 1px #d7d6d6'><div style='height:10px;background-color:#d7d6d6'> </div> <br /><br /><br /><div style='margin-left:30px'><p>Hello " + register.Name + "</p><p>Your Registration is Successful on friendsmobile.co.in<br />This Email Contains Your Details</p><p>Username : " + register.MobileNumber + "</p><p> Password  &nbsp; :" + register.password + " </p><br /><p>Visit Website <a href='http://friendsmobile.co.in/' target='_blank'>friendsmobile.co.in</a></p><p>Best Regards <br />Friends Mobile Team </p></div><br /><div style='height:60px;background-color:#F4D13D'><p style='margin-left:130px;color:black;'><br /><a href='http://friendsmobile.co.in' target='_blank' style='text-decoration:none;color:black'>friendsmobile.co.in</a> © 2020  All Rights Reserved</p></div></div></html>");
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }

        }


        public HttpResponseMessage SendOTP(MsgClass obj)
        {
            Msg.SendSMS1(obj.MobileNo, obj.Message);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        public string GetMobileNumberStatus11()
        {
            try
            {
                Msg.SendSMS1("9214472437","HELLO");
               
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public HttpResponseMessage ForgotPassword(ForgotPassword fpas)
        {
            try
            {
                Cnn.Open();
                string password = Cnn.ExecuteScalar("select password from register where  MobileNumber='"+fpas.Username+"'").ToString();
                string email = Cnn.ExecuteScalar("select emailid from register where  MobileNumber='" + fpas.Username + "'").ToString();
                Cnn.Close();

                Msg.SendEmail(email, "Forgot Password", "<html><div style='height:520px;width:521px;border:solid 1px #d7d6d6'><div style='height:100px;background-color:#d7d6d6'></div> <br /><br /><br /><div style='margin-left:30px'>This Email Contains Your Details</p><p>UserName :  " + fpas.Username + "</p><p> Password  &nbsp; :" + password + " </p><br /><p>Visit Website <a href='http://friendsmobile.co.in' target='_blank'>friendsmobile.co.in</a></p><p>Best Regards <br />friendsmobile Team </p></div><br /><div style='height:60px;background-color:#F4D13D'><p style='margin-left:130px;color:black;'><br /><a href='http://friendsmobile.co.in' target='_blank' style='text-decoration:none;color:black'>friendsmobile.co.in</a> © 2020  All Rights Reserved</p></div></div></html>");
           

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

        }

        public string GetMobileNumberStatus(string id)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(@"Data Source= 103.71.99.32; Database= friendsmobile;initial catalog=friendsmobile;User id=friendsmobile;Password=friendsmobile@55");
                sqlCon.Open();
                SqlCommand NewSql = new SqlCommand("Select count(*) From register where MobileNumber=" + id + "", sqlCon);
                var balanceObj = NewSql.ExecuteScalar();
                sqlCon.Close();
                string st = "";
                if (balanceObj == "0")
                {
                    st = "true";
                }
                else
                {
                    st = "false";
                }
                return balanceObj.ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }


        public IEnumerable<Productinfolist> GetMainPageProducts(int id, int Exid)
        {

            if (Exid == 0)
            {
                var Results = (from p in db.products select new Productinfolist { productid = p.productid, productname = p.productname, categoryid = p.categoryid }).OrderByDescending(x => x.productid).Take(16);
                List<Productinfolist> list_cart = Results.ToList<Productinfolist>();

                Cnn.Open();
                foreach (var item in list_cart)
                {
                    if (id == 1)
                    {
                        string cid = Cnn.ExecuteScalar("select  distinct  a.colorid,a.colorimg from color as a inner join ProductSizeQuantity as b on a.colorid=b.colorid where ProductID=" + item.productid + " order by a.colorimg desc").ToString();
                        string fimage = Cnn.ExecuteScalar("DECLARE @temp VARCHAR(MAX) SELECT top 1 @temp = COALESCE(@temp+'/' ,'') + ImageCode  FROM [dbo].dtl_ProductGallery where  product_id=" + item.productid + " and colorid="+cid+" SELECT @temp ").ToString();
                        DataRow dr = Cnn.FillRow("select  top 1 coalesce(urate,0) as price,coalesce(udiscount,0) as discount from ProductSizeQuantity where ProductSizeQuantity.ProductID=" + item.productid + " ");
                        int price = Convert.ToInt32(dr["price"]);
                        item.price = price;
                        int discount = Convert.ToInt32(dr["discount"]);
                        item.discount = discount;
                        item.Image = fimage;

                    }
                    else if (id == 2)
                    {
                        string cid = Cnn.ExecuteScalar("select  distinct  a.colorid,a.colorimg from color as a inner join ProductSizeQuantity as b on a.colorid=b.colorid where ProductID=" + item.productid + " order by a.colorimg desc").ToString();
                        string fimage = Cnn.ExecuteScalar("DECLARE @temp VARCHAR(MAX) SELECT top 1 @temp = COALESCE(@temp+'/' ,'') + ImageCode  FROM [dbo].dtl_ProductGallery where  product_id=" + item.productid + " and colorid=" + cid + " SELECT @temp ").ToString();
                      
                        DataRow dr = Cnn.FillRow("select top 1 coalesce(arate,0) as price,coalesce(adiscount,0) as discount from ProductSizeQuantity where ProductSizeQuantity.ProductID=" + item.productid + " ");
                        int price = Convert.ToInt32(dr["price"]);
                        item.price = price;
                        int discount = Convert.ToInt32(dr["discount"]);
                        item.discount = discount;
                        item.Image = fimage;
                    }


                }

                Cnn.Close();
                return list_cart;
            }
            else
            {
                var Results = (from p in db.products select new Productinfolist { productid = p.productid, productname = p.productname, categoryid = p.categoryid, Image = p.image }).OrderByDescending(x => x.productid).Skip(Exid * 16).Take(16);
                List<Productinfolist> list_cart = Results.ToList<Productinfolist>();

                Cnn.Open();
                foreach (var item in list_cart)
                {
                    if (id == 1)
                    {
                        string cid = Cnn.ExecuteScalar("select  distinct  a.colorid,a.colorimg from color as a inner join ProductSizeQuantity as b on a.colorid=b.colorid where ProductID=" + item.productid + " order by a.colorimg desc").ToString();
                        string fimage = Cnn.ExecuteScalar("DECLARE @temp VARCHAR(MAX) SELECT top 1 @temp = COALESCE(@temp+'/' ,'') + ImageCode  FROM [dbo].dtl_ProductGallery where  product_id=" + item.productid + " and colorid=" + cid + " SELECT @temp ").ToString();
                      
                        DataRow dr = Cnn.FillRow("select  top 1 coalesce(urate,0) as price,coalesce(udiscount,0) as discount from ProductSizeQuantity where ProductSizeQuantity.ProductID=" + item.productid + " ");
                        int price = Convert.ToInt32(dr["price"]);
                        item.price = price;
                        int discount = Convert.ToInt32(dr["discount"]);
                        item.discount = discount;
                        item.Image = fimage;

                    }
                    else if (id == 2)
                    {
                        string cid = Cnn.ExecuteScalar("select  distinct  a.colorid,a.colorimg from color as a inner join ProductSizeQuantity as b on a.colorid=b.colorid where ProductID=" + item.productid + " order by a.colorimg desc").ToString();
                        string fimage = Cnn.ExecuteScalar("DECLARE @temp VARCHAR(MAX) SELECT top 1 @temp = COALESCE(@temp+'/' ,'') + ImageCode  FROM [dbo].dtl_ProductGallery where  product_id=" + item.productid + " and colorid=" + cid + " SELECT @temp ").ToString();
                      
                        DataRow dr = Cnn.FillRow("select top 1 coalesce(arate,0) as price,coalesce(adiscount,0) as discount from ProductSizeQuantity where ProductSizeQuantity.ProductID=" + item.productid + " ");
                        int price = Convert.ToInt32(dr["price"]);
                        item.price = price;
                        int discount = Convert.ToInt32(dr["discount"]);
                        item.discount = discount;
                        item.Image = fimage;
                    }


                }

                Cnn.Close();
                return list_cart;
            }

        }


        public IEnumerable<Productinfolist> GetBrandWiseProducts(int id, int Exid, int Exxid = 0)
        {


            if (Exxid == 0)
            {
                var Results = (from p in db.products
                               where p.Brandid == Exid
                               select new Productinfolist { productid = p.productid, productname = p.productname, categoryid = p.categoryid, sale = p.sale }).Take(16);
                List<Productinfolist> list_cart = Results.ToList<Productinfolist>();
                Cnn.Open();
                foreach (var item in list_cart)
                {
                    if (id == 1)
                    {
                        string cid = Cnn.ExecuteScalar("select  distinct  a.colorid,a.colorimg from color as a inner join ProductSizeQuantity as b on a.colorid=b.colorid where ProductID=" + item.productid + " order by a.colorimg desc").ToString();
                        string fimage = Cnn.ExecuteScalar("DECLARE @temp VARCHAR(MAX) SELECT top 1 @temp = COALESCE(@temp+'/' ,'') + ImageCode  FROM [dbo].dtl_ProductGallery where  product_id=" + item.productid + " and colorid=" + cid + " SELECT @temp ").ToString();
                      
                        DataRow dr = Cnn.FillRow("select  top 1 coalesce(urate,0) as price,coalesce(udiscount,0) as discount from ProductSizeQuantity where ProductSizeQuantity.ProductID=" + item.productid + " ");
                        int price = Convert.ToInt32(dr["price"]);
                        item.price = price;
                        int discount = Convert.ToInt32(dr["discount"]);
                        item.discount = discount; item.Image = fimage;
                    }
                    else if (id == 2)
                    {
                        string cid = Cnn.ExecuteScalar("select  distinct  a.colorid,a.colorimg from color as a inner join ProductSizeQuantity as b on a.colorid=b.colorid where ProductID=" + item.productid + " order by a.colorimg desc").ToString();
                        string fimage = Cnn.ExecuteScalar("DECLARE @temp VARCHAR(MAX) SELECT top 1 @temp = COALESCE(@temp+'/' ,'') + ImageCode  FROM [dbo].dtl_ProductGallery where  product_id=" + item.productid + " and colorid=" + cid + " SELECT @temp ").ToString();
                      
                        DataRow dr = Cnn.FillRow("select top 1 coalesce(arate,0) as price,coalesce(adiscount,0) as discount from ProductSizeQuantity where ProductSizeQuantity.ProductID=" + item.productid + " ");
                        int price = Convert.ToInt32(dr["price"]);
                        item.price = price;
                        int discount = Convert.ToInt32(dr["discount"]);
                        item.discount = discount; item.Image = fimage;
                    }
                }
                Cnn.Close();
                return list_cart;
            }
            else
            {
                //   //.Skip(Exid * 20).Take(20);
                var Results = (from p in db.products
                               where p.Brandid == Exid
                               select new Productinfolist { productid = p.productid, productname = p.productname, categoryid = p.categoryid, sale = p.sale }).OrderByDescending(x => x.productid).Skip(Exxid * 16).Take(16);
                List<Productinfolist> list_cart = Results.ToList<Productinfolist>();
                Cnn.Open();
                foreach (var item in list_cart)
                {
                    if (id == 1)
                    {
                        string cid = Cnn.ExecuteScalar("select  distinct  a.colorid,a.colorimg from color as a inner join ProductSizeQuantity as b on a.colorid=b.colorid where ProductID=" + item.productid + " order by a.colorimg desc").ToString();
                        string fimage = Cnn.ExecuteScalar("DECLARE @temp VARCHAR(MAX) SELECT top 1 @temp = COALESCE(@temp+'/' ,'') + ImageCode  FROM [dbo].dtl_ProductGallery where  product_id=" + item.productid + " and colorid=" + cid + " SELECT @temp ").ToString();
                      
                        DataRow dr = Cnn.FillRow("select  top 1 coalesce(urate,0) as price,coalesce(udiscount,0) as discount from ProductSizeQuantity where ProductSizeQuantity.ProductID=" + item.productid + " ");
                        int price = Convert.ToInt32(dr["price"]);
                        item.price = price;
                        int discount = Convert.ToInt32(dr["discount"]);
                        item.discount = discount; item.Image = fimage;


                    }
                    else if (id == 2)
                    {
                        string cid = Cnn.ExecuteScalar("select  distinct  a.colorid,a.colorimg from color as a inner join ProductSizeQuantity as b on a.colorid=b.colorid where ProductID=" + item.productid + " order by a.colorimg desc").ToString();
                        string fimage = Cnn.ExecuteScalar("DECLARE @temp VARCHAR(MAX) SELECT top 1 @temp = COALESCE(@temp+'/' ,'') + ImageCode  FROM [dbo].dtl_ProductGallery where  product_id=" + item.productid + " and colorid=" + cid + " SELECT @temp ").ToString();
                      
                        DataRow dr = Cnn.FillRow("select top 1 coalesce(arate,0) as price,coalesce(adiscount,0) as discount from ProductSizeQuantity where ProductSizeQuantity.ProductID=" + item.productid + " ");
                        int price = Convert.ToInt32(dr["price"]);
                        item.price = price;
                        int discount = Convert.ToInt32(dr["discount"]);
                        item.discount = discount; item.Image = fimage;
                    }
                }
                Cnn.Close();
                return list_cart;
            }

        }

        
        public IEnumerable<Productinfolist> GetCategoryWiseProducts(int id, int Exid, int Exxid = 0)
        {

            if (Exxid == 0)
            {
                var Results = (from p in db.products
                               where p.categoryid == Exid
                               select new Productinfolist { productid = p.productid, productname = p.productname, categoryid = p.categoryid, sale = p.sale }).Take(16);
                List<Productinfolist> list_cart = Results.ToList<Productinfolist>();

                Cnn.Open();
                foreach (var item in list_cart)
                {
                    if (id == 1)
                    {
                        string cid = Cnn.ExecuteScalar("select  distinct  a.colorid,a.colorimg from color as a inner join ProductSizeQuantity as b on a.colorid=b.colorid where ProductID=" + item.productid + " order by a.colorimg desc").ToString();
                        string fimage = Cnn.ExecuteScalar("DECLARE @temp VARCHAR(MAX) SELECT top 1 @temp = COALESCE(@temp+'/' ,'') + ImageCode  FROM [dbo].dtl_ProductGallery where  product_id=" + item.productid + " and colorid=" + cid + " SELECT @temp ").ToString();
                    
                        DataRow dr = Cnn.FillRow("select top 1 coalesce(urate,0) as price,coalesce(udiscount,0) as discount from ProductSizeQuantity where ProductSizeQuantity.ProductID=" + item.productid + " ");
                        int price = Convert.ToInt32(dr["price"]);
                        item.price = price;
                        int discount = Convert.ToInt32(dr["discount"]);
                        item.discount = discount; item.Image = fimage;


                    }
                    else if (id == 2)
                    {
                        string cid = Cnn.ExecuteScalar("select  distinct  a.colorid,a.colorimg from color as a inner join ProductSizeQuantity as b on a.colorid=b.colorid where ProductID=" + item.productid + " order by a.colorimg desc").ToString();
                        string fimage = Cnn.ExecuteScalar("DECLARE @temp VARCHAR(MAX) SELECT top 1 @temp = COALESCE(@temp+'/' ,'') + ImageCode  FROM [dbo].dtl_ProductGallery where  product_id=" + item.productid + " and colorid=" + cid + " SELECT @temp ").ToString();
                    
                        DataRow dr = Cnn.FillRow("select top 1 coalesce(arate,0) as price,coalesce(adiscount,0) as discount from ProductSizeQuantity where ProductSizeQuantity.ProductID=" + item.productid + " ");
                        int price = Convert.ToInt32(dr["price"]);
                        item.price = price;
                        int discount = Convert.ToInt32(dr["discount"]);
                        item.discount = discount; item.Image = fimage;
                    }


                }

                Cnn.Close();
                return list_cart;
            }
            else
            {
                //.Skip(Exid * 20).Take(20);
                var Results = (from p in db.products
                               where p.categoryid == Exid
                               select new Productinfolist { productid = p.productid, productname = p.productname, categoryid = p.categoryid, sale = p.sale}).OrderByDescending(x => x.productid).Skip(Exxid * 16).Take(16);
                List<Productinfolist> list_cart = Results.ToList<Productinfolist>();

                Cnn.Open();
                foreach (var item in list_cart)
                {
                    if (id == 1)
                    {
                        string cid = Cnn.ExecuteScalar("select  distinct  a.colorid,a.colorimg from color as a inner join ProductSizeQuantity as b on a.colorid=b.colorid where ProductID=" + item.productid + " order by a.colorimg desc").ToString();
                        string fimage = Cnn.ExecuteScalar("DECLARE @temp VARCHAR(MAX) SELECT top 1 @temp = COALESCE(@temp+'/' ,'') + ImageCode  FROM [dbo].dtl_ProductGallery where  product_id=" + item.productid + " and colorid=" + cid + " SELECT @temp ").ToString();
                    
                        DataRow dr = Cnn.FillRow("select top 1 coalesce(urate,0) as price,coalesce(udiscount,0) as discount from ProductSizeQuantity where ProductSizeQuantity.ProductID=" + item.productid + " ");
                        int price = Convert.ToInt32(dr["price"]);
                        item.price = price;
                        int discount = Convert.ToInt32(dr["discount"]);
                        item.discount = discount; item.Image = fimage;


                    }
                    else if (id == 2)
                    {
                        string cid = Cnn.ExecuteScalar("select  distinct  a.colorid,a.colorimg from color as a inner join ProductSizeQuantity as b on a.colorid=b.colorid where ProductID=" + item.productid + " order by a.colorimg desc").ToString();
                        string fimage = Cnn.ExecuteScalar("DECLARE @temp VARCHAR(MAX) SELECT top 1 @temp = COALESCE(@temp+'/' ,'') + ImageCode  FROM [dbo].dtl_ProductGallery where  product_id=" + item.productid + " and colorid=" + cid + " SELECT @temp ").ToString();
                    
                        DataRow dr = Cnn.FillRow("select top 1 coalesce(arate,0) as price,coalesce(adiscount,0) as discount from ProductSizeQuantity where ProductSizeQuantity.ProductID=" + item.productid + " ");
                        int price = Convert.ToInt32(dr["price"]);
                        item.price = price;
                        int discount = Convert.ToInt32(dr["discount"]);
                        item.discount = discount; item.Image = fimage;
                    }


                }

                Cnn.Close();
                return list_cart;

            }

        }
        public HttpResponseMessage Mail(mail obj)
        {
            try
            {
                string strEmail_Admin = "<html>" + DateTime.Now + "<br/><br/><br />Hello, &nbsp;Admin";
                strEmail_Admin += "<html><div style='height:520px;width:521px;border:solid 1px #d7d6d6'><div style='height:10px;background-color:#d7d6d6'></div> <br /><br /><br /><div style='margin-left:30px'><p>Hello Admin</p><p>New enquiry on friendsmobile.co.in<br />This Email Contains </p><p>Name : " + obj.name + "</p><p> Mobile No.  &nbsp; :" + obj.mobile + " </p><br /><p> Email Id  &nbsp; :" + obj.email + " </p><br /><p> Message No.  &nbsp; :" + obj.message + " </p><br /><p>Visit Website <a href='http://www.friendsmobile.co.in/' target='_blank'>friendsmobile.co.in</a></div><br /><div style='height:60px;background-color:#F4D13D'><p style='margin-left:130px;color:black;'><br /><a href='http://friendsmobile.co.in' target='_blank' style='text-decoration:none;color:black'>friendsmobile.co.in</a> © 2020  All Rights Reserved</p></div></div></html>";
                Msg.SendEmailtoAll("info@friendsmobile.co.in", "New enquiry: friendsmobile.co.in", strEmail_Admin);//send email to user
                return Request.CreateResponse(HttpStatusCode.Created, "OK");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            //   finally { db.Dispose(); }
        }

        public HttpResponseMessage GetLogin(string id, string Exid)
        {
            DataTable ds = new DataTable();
            sqlCon.Open();
            string querystring = "select * from register  where MobileNumber='" + id + "'  and password='" + Exid + "'";
            SqlCommand cmd = new SqlCommand(querystring, sqlCon);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlCon.Close();
            return Request.CreateResponse(HttpStatusCode.OK, ds);
        }


        public string GetWallet(int id)
        {
            string Wallet = "";
            Cnn.Open();
            Wallet = Cnn.ExecuteScalar("select Wallet from register  where UserId="+id+"").ToString();
            Cnn.Close();
            return Wallet;
        }

        public colorload GetColor(int id)
        {
            SqlConnection sqlCon = new SqlConnection(@"Data Source= 103.71.99.32; Database= friendsmobile;initial catalog=friendsmobile;User id=friendsmobile;Password=friendsmobile@55");
            sqlCon.Open();
            SqlDataAdapter sqlDa = new SqlDataAdapter("select distinct  a.colorid,a.colorimg from color as a inner join ProductSizeQuantity as b on a.colorid=b.colorid where ProductID=" + id + " order by a.colorimg desc", sqlCon);
            DataTable Dt = new DataTable();
            sqlDa.Fill(Dt);
            sqlCon.Close();
            string cid = "", cnmae = "";
            for (int i = 0; i < Dt.Rows.Count; i++)
            {

                cid += Dt.Rows[i]["colorid"].ToString()+ "/" ;
                cnmae += Dt.Rows[i]["colorimg"].ToString()+ "/";
            }
            cid = cid.Remove(cid.Length - 1);
            cnmae = cnmae.Remove(cnmae.Length - 1);
            var Results = new colorload();
            //Results.ColorId = string.Join("/", (from p in db.ProductSizeQuantities join q in db.Colors on p.ColorId equals q.ColorId where p.ProductID == id orderby p.ID descending select q).Select(x => x.ColorId).Distinct().ToList());
            //Results.Colorimg = string.Join("/", (from p in db.ProductSizeQuantities join q in db.Colors on p.ColorId equals q.ColorId where p.ProductID == id orderby p.ID descending select q).Select(x => x.Colorimg).Distinct().ToList());
                Results.ColorId = cid;
                Results.Colorimg = cnmae;
               return Results;
        }

        public HttpResponseMessage GetColor1(int id)
        {
            DataTable ds = new DataTable();
            sqlCon.Open();
            string querystring = "select distinct  a.colorid,a.colorimg from color as a inner join ProductSizeQuantity as b on a.colorid=b.colorid where ProductID=" + id + " order by a.colorimg desc";  // 3521 "            
            SqlCommand cmd = new SqlCommand(querystring, sqlCon);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlCon.Close();
            return Request.CreateResponse(HttpStatusCode.OK, ds);
        }
        public JoinColorDetails GetColorDetail(int id, int Exid)
        {

            var Results = new JoinColorDetails();
            string sid = "", sname = "", sizeid = "";
            Cnn.Open();
            sname = Cnn.ExecuteScalar("DECLARE @temp VARCHAR(MAX) SELECT @temp = COALESCE(@temp+'/' ,'') + varient FROM [dbo].varient as a  inner join ProductSizeQuantity as b on a.id=b.size where b.productid=" + id + " and b.colorid=" + Exid + " SELECT @temp ").ToString();
            sizeid = Cnn.ExecuteScalar("DECLARE @temp VARCHAR(MAX) SELECT @temp = COALESCE(@temp+'/' ,'') + size FROM  ProductSizeQuantity  where productid=" + id + " and colorid=" + Exid + " SELECT @temp ").ToString();
            sid = Cnn.ExecuteScalar(" DECLARE @temp VARCHAR(MAX) SELECT @temp = COALESCE(@temp+'/' ,'') + ImageCode FROM [dbo].dtl_ProductGallery where  product_id=" + id + " and colorid=" + Exid + " SELECT @temp ").ToString();
            Cnn.Close();
            Results.Images = sid;
            Results.Size = sname;
            Results.Sizeid = sizeid;
            Results.Quantity = string.Join("/", db.ProductSizeQuantities.Where(x => x.ProductID == id && x.ColorId == Exid).ToList().Select(x => x.Quantity));
            Results.ProductColorRate = string.Join("/", db.ProductSizeQuantities.Where(x => x.ProductID == id && x.ColorId == Exid).Select(x => x.urate).ToList());
            Results.ProductColorDiscount = string.Join("/", db.ProductSizeQuantities.Where(x => x.ProductID == id && x.ColorId == Exid).Select(x => x.udiscount).ToList());
            Results.Description = string.Join("/", db.products.Where(x => x.productid == id).ToList().Select(x => x.Description));
            return Results;
        }
        public JoinColorDetails GetRateDetail(int id, string Exid)
        {

            var Results = new JoinColorDetails();
            Results.Quantity = string.Join("/", db.ProductSizeQuantities.Where(x => x.ProductID == id && x.Size == Exid).ToList().Select(x => x.Quantity));
            Results.ProductColorRate = string.Join("/", db.ProductSizeQuantities.Where(x => x.ProductID == id && x.Size == Exid).Select(x => x.urate).ToList());
            Results.ProductColorDiscount = string.Join("/", db.ProductSizeQuantities.Where(x => x.ProductID == id && x.Size == Exid).Select(x => x.udiscount).ToList());
            return Results;
        }
        public Colorclass GetProductinfo(int id)
        {

            var Results = new Colorclass();
            string sid = "", descr = "";
            Cnn.Open();
            sid = Cnn.ExecuteScalar("DECLARE @temp VARCHAR(MAX) SELECT @temp = COALESCE(@temp+'/' ,'') + varient FROM [dbo].varient as a  inner join ProductSizeQuantity as b on a.id=b.size where b.productid=" + id + " SELECT @temp ").ToString();
            descr = Cnn.ExecuteScalar("select Description from Product where productid=" + id + " ").ToString();
            Cnn.Close();
            Results.Sizeid = string.Join("/", (from p in db.ProductSizeQuantities where p.ProductID == id select p.Size).Distinct().ToList());
            Results.Sizename = sid;
            Results.Image = string.Join("/", db.dtl_ProductGallery.Where(x => x.Product_Id == id).ToList().Select(x => x.ImageCode.ToString()));
            Results.PColorId = string.Join("/", (from p in db.ProductSizeQuantities join q in db.Colors on p.ColorId equals q.ColorId where p.ProductID == id select q).Select(x => x.ColorId).Distinct().ToList());
            Results.PColor = string.Join("/", (from p in db.ProductSizeQuantities join q in db.Colors on p.ColorId equals q.ColorId where p.ProductID == id select q).Select(x => x.Colorimg).Distinct().ToList());
            Results.Quantity = string.Join("/", (from p in db.ProductSizeQuantities where p.ProductID == id select p).Select(x => x.Quantity).ToList());
            Results.Description = descr;
            return Results;
        }

        public string GetCourier(int id, string Exid)
        {
            Cnn.Open();
            int Couriergm = 0, Courierkg = 0;
            SqlDataAdapter sqlDa = new SqlDataAdapter("select a.id,a.productid,a.color,a.Size,b.productname,a.Quantity,b.image,a.id as rate,a.id as discount  from trncart as a inner join  product as b on a.productid=b.productid Where a.UserId=" + id + "", sqlCon);
            DataTable Dt = new DataTable();
            sqlDa.Fill(Dt);
            String CourierState = Convert.ToString(Cnn.ExecuteScalar("Select State_Name from Loc_State where State_Name='" + Exid + "'"));
            String CourierW;
            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                int ProductId = Convert.ToInt32(Dt.Rows[i]["ProductId"]);
                CourierW = Convert.ToString(Cnn.ExecuteScalar("Select Unit from Product where ProductId='" + ProductId + "'"));
                if (CourierW == "gm")
                {
                    String Courier = Convert.ToString(Cnn.ExecuteScalar("Select ((cast(Weight as varchar(50)))) as CourierWeight from Product where ProductId='" + ProductId + "'"));
                    Couriergm += (Convert.ToInt32(Courier) * Convert.ToInt32(Dt.Rows[i]["Quantity"]));
                }
                else
                {
                    String Courier = Convert.ToString(Cnn.ExecuteScalar("Select ((cast(Weight as varchar(50)))) as CourierWeight from Product where ProductId='" + ProductId + "'"));
                    Courierkg += (Convert.ToInt32(Courier) * Convert.ToInt32(Dt.Rows[i]["Quantity"]));
                }
            }
            string TotalWeight = (Convert.ToString(Courierkg + Couriergm)).ToString();
            double CW = (Convert.ToDouble(TotalWeight) / Convert.ToDouble(1000));
            string RajCourier_Fees;
            if (CourierState == "Rajasthan")
            {
                RajCourier_Fees = Convert.ToString(Cnn.ExecuteScalar("select RajCourier_Fees,id from Courier_Detail where RajCourier_Name='" + CourierState + "' and '" + CW + "' between FromWeight and ToWeight"));
            }
            else if (CourierState == "North India")
            {
                RajCourier_Fees = Convert.ToString(Cnn.ExecuteScalar("select NICourier_fees,id from Courier_Detail where NICourier_Name='" + CourierState + "' and '" + CW + "' between FromWeight and ToWeight"));
            }
            else if (CourierState == "Rest of India")
            {
                RajCourier_Fees = Convert.ToString(Cnn.ExecuteScalar("select ROICourier_fees,id from Courier_Detail where ROICourier_Name='" + CourierState + "' and '" + CW + "' between FromWeight and ToWeight"));
            }
            else
            {
                RajCourier_Fees = Convert.ToString(Cnn.ExecuteScalar("select NECourier_fees,id from Courier_Detail where NECourier_Name='" + CourierState + "' and '" + CW + "' between FromWeight and ToWeight"));
            }

            Cnn.Close();
            return RajCourier_Fees.ToString();
        }

        public HttpResponseMessage InsertCart(TrnCart obj)
        {
            try
            {
                var pid1 = obj.pid.Split(new[] { ',' });
                var pqty1 = obj.pqty.Split(new[] { ',' });
                var Size1 = obj.Size.Split(new[] { ',' });
                var Color1 = obj.Color.Split(new[] { ',' });
                for (int i = 0; i < pid1.Length; i++)
                {

                    Cnn.Open();
                    Cnn.ExecuteNonQuery("delete from trncart where userid=" + obj.UserId + " and productid=" + Convert.ToInt32(pid1[i].ToString()) + "");
                    Cnn.Close();
                    obj.Id = db.TrnCarts.Any() ? db.TrnCarts.Max(x => x.Id + 1) : 1;
                    obj.UserId = obj.UserId;
                    obj.ProductId = Convert.ToInt32(pid1[i].ToString());
                    obj.Size = Size1[i].ToString();
                    obj.Quantity = Convert.ToInt32(pqty1[i].ToString());
                    obj.Color = Color1[i].ToString();
                    obj.CartId = 0; obj.ActiveStatus = true; obj.RTS = DateTime.Now;
                    db.TrnCarts.Add(obj);
                    db.SaveChanges();

                }

                return Request.CreateResponse(HttpStatusCode.Created, "ok");
            }
            catch (Exception Ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Ex);
            }


        }

        public HttpResponseMessage InsertCart1(TrnCart obj)
        {
            try
            {
                int count = 0;

                Cnn.Open();
                count = Convert.ToInt32(Cnn.ExecuteScalar("select count(*) from trncart where userid=" + obj.UserId + " and productid=" + Convert.ToInt32(obj.pid) + " and Size=" + Convert.ToInt32(obj.Size) + " and Color=" + Convert.ToInt32(obj.Color) + ""));
                Cnn.Close();
                if (count > 0)
                {

                    return Request.CreateResponse(HttpStatusCode.Created);
                }

                else
                {
                    obj.Id = db.TrnCarts.Any() ? db.TrnCarts.Max(x => x.Id + 1) : 1;
                    obj.UserId = obj.UserId;
                    obj.ProductId = Convert.ToInt32(obj.pid);
                    obj.Size = obj.Size;
                    obj.Quantity = Convert.ToInt32(obj.pqty);
                    obj.Color = obj.Color;
                    obj.CartId = 0; obj.ActiveStatus = true; obj.RTS = DateTime.Now;
                    db.TrnCarts.Add(obj);
                    db.SaveChanges();


                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch (Exception Ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }


        }
        public HttpResponseMessage DCart(deletecart obj)
        {
            try
            {
          
                Cnn.Open();
                Cnn.ExecuteNonQuery("delete from trncart where userid=" + obj.UserId + " and productid=" + obj.pid + "  and Size=" + obj.Size + " and Color=" + obj.Color + "");
                Cnn.Close();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception Ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Ex);
            }


        }
        public HttpResponseMessage UCart(deletecart obj)
        {
            try
            {
                Cnn.Open();
                Cnn.ExecuteNonQuery("update trncart set Quantity=" + obj.qty + " where userid=" + obj.UserId + " and productid=" + obj.pid + " and Size=" + obj.Size + " and Color=" + obj.Color + "");
                Cnn.Close();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception Ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Ex);
            }


        }
        public IEnumerable<Cart> GetCart(int id)
        {
            var Results = (from p in db.products
                           join q in db.TrnCarts on p.productid equals q.ProductId
                           where q.UserId == id
                           select new Cart { ProductId = p.productid, productname = p.productname, colorid = q.Color, qty = q.Quantity, size = q.Size });
            List<Cart> list_cart = Results.ToList<Cart>();
            Cnn.Open();
            foreach (var item in list_cart)
            {
                string sizename = "";
                if (item.size != "-1")
                {
                    sizename = Cnn.ExecuteScalar("select varient from varient where id=" + item.size + "").ToString();
                }

                DataRow dr = Cnn.FillRow("select coalesce(urate,0) as price,coalesce(udiscount,0) as discount,Quantity from ProductSizeQuantity where ProductID=" + item.ProductId + " and colorid=" + item.colorid + " and size=" + item.size + " group by urate,udiscount,Quantity ");
                DataRow dr1 = Cnn.FillRow("select colorimg from color where  colorid=" + item.colorid + " ");
                DataRow dr2 = Cnn.FillRow("select isnull(ImageCode,'') as ImageCode from dtl_ProductGallery where Product_ID=" + item.ProductId + " and colorid=" + item.colorid + "");
                int price = Convert.ToInt32(dr["price"]);
                item.rate = price;
                int discount = Convert.ToInt32(dr["discount"]);
                item.discount = discount;
                item.stock = Convert.ToInt32(dr["Quantity"]);
                item.color = dr1["colorimg"].ToString();
                item.image = dr2["ImageCode"].ToString();
                item.sizename = sizename;
            }
            Cnn.Close();
            return list_cart;

        }

        public IEnumerable<Cart> GetCartweb(int id)
        {
            var Results = (from p in db.products
                           join q in db.TrnCarts on p.productid equals q.ProductId
                           where q.CartId == id
                           select new Cart { ProductId = p.productid, productname = p.productname, colorid = q.Color, qty = q.Quantity, size = q.Size });
            List<Cart> list_cart = Results.ToList<Cart>();
            Cnn.Open();
            foreach (var item in list_cart)
            {

                DataRow dr = Cnn.FillRow("select coalesce(urate,0) as price,coalesce(udiscount,0) as discount,Quantity from ProductSizeQuantity where ProductID=" + item.ProductId + " and colorid=" + item.colorid + " group by urate,udiscount,Quantity ");
                DataRow dr1 = Cnn.FillRow("select colorimg from color where  colorid=" + item.colorid + " ");
                DataRow dr2 = Cnn.FillRow("select ImageCode from dtl_ProductGallery where Product_ID=" + item.ProductId + " and colorid=" + item.colorid + "");
                int price = Convert.ToInt32(dr["price"]);
                item.rate = price;
                int discount = Convert.ToInt32(dr["discount"]);
                item.discount = discount;
                item.stock = Convert.ToInt32(dr["Quantity"]);
                item.color = dr1["colorimg"].ToString();
                item.image = dr2["ImageCode"].ToString();
            }
            Cnn.Close();
            return list_cart;

        }


        public HttpResponseMessage webtest(varient obj)
        {
            try
            {


                obj.id = db.varients.Any() ? db.varients.Max(x => x.id + 1) : 1;

                obj.varient1 = "twst"; obj.active = true; obj.rts = DateTime.Now;
                db.varients.Add(obj);
                db.SaveChanges();


                return Request.CreateResponse(HttpStatusCode.Created, "ok");
            }
            catch (Exception Ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Ex);
            }


        }



        public HttpResponseMessage Token(Token Token)
        {
            try
            {
                Cnn.Open();
                int check = Convert.ToInt32(Cnn.ExecuteScalar("select count(*) from  M_TokenMaster where TokenID='"+Token.tokend+"' "));
                if (check == 0)
                {
                    Cnn.ExecuteNonQuery("insert into M_TokenMaster (TokenID,RTS) values ('" + Token.tokend + "',GetDate()) ");
                }
                Cnn.Close();
              
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }


        }


        public HttpResponseMessage GetWalletAmount(string id)
        {

            var Results = (from p in db.registers where p.MobileNumber == id select p.Wallet).First();

            return Request.CreateResponse(HttpStatusCode.OK, Results.ToString());

        }
        public IEnumerable<TrnWallet> GetWalletTransaction(string id)
        {
            int userid = 0;
            Cnn.Open();
            userid = Convert.ToInt32(Cnn.ExecuteScalar("select userid from register  where MobileNumber=" + id + ""));
            Cnn.Close();  
            //var Results = (from p in db.TrnWallets where p.TrnType == "Dr" && p.UserId == id select p).AsEnumerable().Union(from p in db.TrnWallets where p.TrnType == "Cr" && p.UserId == id select p).OrderByDescending(x => x.AID);
            var Results = (from p in db.TrnWallets where p.TrnType == "Dr" && p.UserId == userid select p).AsEnumerable().Union(from p in db.TrnWallets where p.TrnType == "Cr" && p.UserId == userid && p.Amount != null && p.PromoCodeAmount != null select p).OrderByDescending(x => x.AID);
            return Results;

        }

        public int GetUserCheck(string id)
        {
            int Wallet=0;
            Cnn.Open();
            Wallet = Convert.ToInt32(Cnn.ExecuteScalar("select count(*) from register  where MobileNumber=" + id + ""));
            Cnn.Close();        
            return Wallet;
        }

        public IEnumerable<ProductListing> GetProductListing(string id)
        {

            var Results = (from q in db.M_CategoryMaster where q.MainCategoryName.Contains(id) select new ProductListing { Name = q.MainCategoryName, Id = q.MainCategoryid, Type = "MainCategoryName", mid = 0 }).Union(from q in db.brands where q.brandName.Contains(id) select new ProductListing { Name = q.brandName, Id = q.brandid, Type = "brandName", mid = 0 }).Union(from p in db.products where p.productname.Contains(id) && p.active == true select new ProductListing { Name = p.productname.Substring(0, 30), Id = p.productid, Type = "Product", mid = p.categoryid }).AsEnumerable().Take(4);
             return Results;

        }

        public IEnumerable<newitem> Getnewproduct()
        {
           
                var Results = (from p in db.products select new newitem { productid = p.productid, productname = p.productname }).OrderByDescending(x => x.productid).Take(20);
                List<newitem> list_cart = Results.ToList<newitem>();

                Cnn.Open();
                //foreach (var item in list_cart)
                //{


                //    DataRow dr = Cnn.FillRow("select top 1 brandname,brandid from  brand  order by NEWID()  ");
                //    string brandid = dr["brandid"].ToString();
                //    item.brandid = brandid;

                //    string brandname = dr["brandname"].ToString();
                //    item.brandname = brandname;
                //}

                Cnn.Close();
                return list_cart;
         
            
            
        }

    }
}


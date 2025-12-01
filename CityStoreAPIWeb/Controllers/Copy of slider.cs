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
        SqlConnection sqlCon = new SqlConnection(@"Data Source= 103.71.99.32; Database= friendsmobile;initial catalog=friendsmobile;User id=friendsmobile;Password=friendsmobile@55");
        //   ClsQuantity objQty = new ClsQuantity();
        ClsConnection Cnn = new ClsConnection();
        public HttpResponseMessage GetSlider()
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

        public HttpResponseMessage GetCategory()
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

        public HttpResponseMessage GetBrand()
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

        public HttpResponseMessage GetState()
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
        public HttpResponseMessage GetDistrict(string id)
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
        public string GetVersionNo()
        {
            SqlConnection sqlCon = new SqlConnection(@"Data Source= 103.71.99.32; Database= friendsmobile;initial catalog=friendsmobile;User id=friendsmobile;Password=friendsmobile@55");
            sqlCon.Open();
            SqlCommand NewSql = new SqlCommand("Select versionNo From M_AppVersionMaster", sqlCon);
            var balanceObj = NewSql.ExecuteScalar();
            sqlCon.Close();
            return balanceObj.ToString();
        }

        public HttpResponseMessage CreateNewUser(register register)
        {
            try
            {
                register.UserId = db.registers.Any() ? db.registers.Max(x => x.UserId + 1) : 1;
                register.active = true; register.Wallet = 10; register.ip = "App"; register.rts = DateTime.Now; register.OrderCount = 0;
                TrnWallet trnwallet = new TrnWallet();
                trnwallet.OrderId = 0;
                trnwallet.ReMark = "Wallet Point Add ";
                trnwallet.PromoCodeAmount = 0; trnwallet.ReMark = "";
                trnwallet.TrnType = "Cr"; trnwallet.Amount = 10; trnwallet.IsDeducted = true; trnwallet.UserId = register.UserId; trnwallet.RTS = DateTime.Now; trnwallet.Type = "Normal"; trnwallet.OrderType = "City";
                db.TrnWallets.Add(trnwallet);
                db.SaveChanges();
                db.registers.Add(register);
                db.SaveChanges();
                Msg.SendEmail(register.EmailId, "Registration Successful", "<html><div style='height:520px;width:521px;border:solid 1px #d7d6d6'><div style='height:10px;background-color:#d7d6d6'> </div> <br /><br /><br /><div style='margin-left:30px'><p>Hello '" + register.Name + "'</p><p>Your Registration is Successful on friendsmobile.co.in<br />This Email Contains Your Details</p><p>UserName : '" + register.MobileNumber + "'</p><p> Password  &nbsp; :'" + register.password + "' </p><br /><p>Visit Website <a href='https://www.citystore.in/' target='_blank'>CityStore.in</a></p><p>Best Regards <br />CityStore Team </p></div><br /><div style='height:60px;background-color:#F4D13D'><p style='margin-left:130px;color:black;'><br /><a href='http://friendsmobile.co.in' target='_blank' style='text-decoration:none;color:black'>friendsmobile.co.in</a> © 2020  All Rights Reserved</p></div></div></html>");
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }


        }

        public string GetMobileNumberStatus(string id)
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


        public IEnumerable<Productinfolist> GetMainPageProducts(int id, int Exid)
        {
            if (Exid == 0)
            {

                var Results = (from p in db.products

                               select new Productinfolist { productid = p.productid, productname = p.productname, categoryid = p.categoryid, sale = p.sale, Description = p.Description }).OrderByDescending(x => x.productid).Take(20);
                List<Productinfolist> list_cart = Results.ToList<Productinfolist>();

                Cnn.Open();
                foreach (var item in list_cart)
                {
                    if (id == 1)
                    {
                        DataRow dr = Cnn.FillRow("select top 1 coalesce(urate,0) as price,coalesce(udiscount,0) as discount from ProductSizeQuantity where ProductSizeQuantity.ProductID=" + item.productid + " ");
                        int price = Convert.ToInt32(dr["price"]);
                        item.price = price;
                        int discount = Convert.ToInt32(dr["discount"]);
                        item.discount = discount;


                    }
                    else if (id == 2)
                    {
                        DataRow dr = Cnn.FillRow("select top 1 coalesce(arate,0) as price,coalesce(adiscount,0) as discount from ProductSizeQuantity where ProductSizeQuantity.ProductID=" + item.productid + " ");
                        int price = Convert.ToInt32(dr["price"]);
                        item.price = price;
                        int discount = Convert.ToInt32(dr["discount"]);
                        item.discount = discount;
                    }

                    item.Image = string.Join("/", db.dtl_ProductGallery.Where(x => x.Product_Id == item.productid).ToList().Select(x => x.ImageCode.ToString()));
                    item.PColorId = string.Join("/", (from p in db.ProductSizeQuantities join q in db.Colors on p.ColorId equals q.ColorId where p.ProductID == item.productid select q).Select(x => x.ColorId).Distinct().ToList());
                    item.PColor = string.Join("/", (from p in db.ProductSizeQuantities join q in db.Colors on p.ColorId equals q.ColorId where p.ProductID == item.productid select q).Select(x => x.Colorimg).Distinct().ToList());
                    item.Quantity = string.Join("/", (from p in db.ProductSizeQuantities where p.ProductID == item.productid select p).Select(x => x.Quantity).ToList());

                }

                Cnn.Close();
                return list_cart;
            }
            else
            {
                var Results = (from p in db.products

                               select new Productinfolist { productid = p.productid, productname = p.productname, categoryid = p.categoryid, sale = p.sale, Description = p.Description }).OrderByDescending(x => x.productid).Skip(Exid * 20).Take(20);
                List<Productinfolist> list_cart = Results.ToList<Productinfolist>();

                Cnn.Open();
                foreach (var item in list_cart)
                {
                    if (id == 1)
                    {
                        DataRow dr = Cnn.FillRow("select top 1 coalesce(urate,0) as price,coalesce(udiscount,0) as discount from ProductSizeQuantity where ProductSizeQuantity.ProductID=" + item.productid + " ");
                        int price = Convert.ToInt32(dr["price"]);
                        item.price = price;
                        int discount = Convert.ToInt32(dr["discount"]);
                        item.discount = discount;


                    }
                    else if (id == 2)
                    {
                        DataRow dr = Cnn.FillRow("select top 1 coalesce(arate,0) as price,coalesce(adiscount,0) as discount from ProductSizeQuantity where ProductSizeQuantity.ProductID=" + item.productid + " ");
                        int price = Convert.ToInt32(dr["price"]);
                        item.price = price;
                        int discount = Convert.ToInt32(dr["discount"]);
                        item.discount = discount;
                    }

                    item.Image = string.Join("/", db.dtl_ProductGallery.Where(x => x.Product_Id == item.productid).ToList().Select(x => x.ImageCode.ToString()));
                    item.PColorId = string.Join("/", (from p in db.ProductSizeQuantities join q in db.Colors on p.ColorId equals q.ColorId where p.ProductID == item.productid select q).Select(x => x.ColorId).Distinct().ToList());
                    item.PColor = string.Join("/", (from p in db.ProductSizeQuantities join q in db.Colors on p.ColorId equals q.ColorId where p.ProductID == item.productid select q).Select(x => x.Colorimg).Distinct().ToList());
                    item.Quantity = string.Join("/", (from p in db.ProductSizeQuantities where p.ProductID == item.productid select p).Select(x => x.Quantity).ToList());

                }

                Cnn.Close();
                return list_cart;
            
            }
           
        }
      

        public IEnumerable<Productinfolist> GetBrandWiseProducts(int id, int Exid , int Exxid = 0)
        {


            if (Exxid == 0)
            {
                var Results = (from p in db.products
                               where p.Brandid == Exid
                               select new Productinfolist { productid = p.productid, productname = p.productname, categoryid = p.categoryid, sale = p.sale, Description = p.Description }).Take(20);
                List<Productinfolist> list_cart = Results.ToList<Productinfolist>();

                Cnn.Open();
                foreach (var item in list_cart)
                {
                    if (id == 1)
                    {
                        DataRow dr = Cnn.FillRow("select  top 1 coalesce(urate,0) as price,coalesce(udiscount,0) as discount from ProductSizeQuantity where ProductSizeQuantity.ProductID=" + item.productid + " ");
                        int price = Convert.ToInt32(dr["price"]);
                        item.price = price;
                        int discount = Convert.ToInt32(dr["discount"]);
                        item.discount = discount;


                    }
                    else if (id == 2)
                    {
                        DataRow dr = Cnn.FillRow("select top 1 coalesce(arate,0) as price,coalesce(adiscount,0) as discount from ProductSizeQuantity where ProductSizeQuantity.ProductID=" + item.productid + " ");
                        int price = Convert.ToInt32(dr["price"]);
                        item.price = price;
                        int discount = Convert.ToInt32(dr["discount"]);
                        item.discount = discount;
                    }

                    item.Image = string.Join("/", db.dtl_ProductGallery.Where(x => x.Product_Id == item.productid).ToList().Select(x => x.ImageCode.ToString()));
                    item.PColorId = string.Join("/", (from p in db.ProductSizeQuantities join q in db.Colors on p.ColorId equals q.ColorId where p.ProductID == item.productid select q).Select(x => x.ColorId).Distinct().ToList());
                    item.PColor = string.Join("/", (from p in db.ProductSizeQuantities join q in db.Colors on p.ColorId equals q.ColorId where p.ProductID == item.productid select q).Select(x => x.Colorimg).Distinct().ToList());
                    item.Quantity = string.Join("/", (from p in db.ProductSizeQuantities where p.ProductID == item.productid select p).Select(x => x.Quantity).ToList());

                }

                Cnn.Close();
                return list_cart;
            }
            else
            {
                //   //.Skip(Exid * 20).Take(20);
                var Results = (from p in db.products
                               where p.Brandid == Exid
                               select new Productinfolist { productid = p.productid, productname = p.productname, categoryid = p.categoryid, sale = p.sale, Description = p.Description }).OrderByDescending(x => x.productid).Skip(Exid * 20).Take(20);
                List<Productinfolist> list_cart = Results.ToList<Productinfolist>();

                Cnn.Open();
                foreach (var item in list_cart)
                {
                    if (id == 1)
                    {
                        DataRow dr = Cnn.FillRow("select  top 1 coalesce(urate,0) as price,coalesce(udiscount,0) as discount from ProductSizeQuantity where ProductSizeQuantity.ProductID=" + item.productid + " ");
                        int price = Convert.ToInt32(dr["price"]);
                        item.price = price;
                        int discount = Convert.ToInt32(dr["discount"]);
                        item.discount = discount;


                    }
                    else if (id == 2)
                    {
                        DataRow dr = Cnn.FillRow("select top 1 coalesce(arate,0) as price,coalesce(adiscount,0) as discount from ProductSizeQuantity where ProductSizeQuantity.ProductID=" + item.productid + " ");
                        int price = Convert.ToInt32(dr["price"]);
                        item.price = price;
                        int discount = Convert.ToInt32(dr["discount"]);
                        item.discount = discount;
                    }

                    item.Image = string.Join("/", db.dtl_ProductGallery.Where(x => x.Product_Id == item.productid).ToList().Select(x => x.ImageCode.ToString()));
                    item.PColorId = string.Join("/", (from p in db.ProductSizeQuantities join q in db.Colors on p.ColorId equals q.ColorId where p.ProductID == item.productid select q).Select(x => x.ColorId).Distinct().ToList());
                    item.PColor = string.Join("/", (from p in db.ProductSizeQuantities join q in db.Colors on p.ColorId equals q.ColorId where p.ProductID == item.productid select q).Select(x => x.Colorimg).Distinct().ToList());
                    item.Quantity = string.Join("/", (from p in db.ProductSizeQuantities where p.ProductID == item.productid select p).Select(x => x.Quantity).ToList());

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
                               select new Productinfolist { productid = p.productid, productname = p.productname, categoryid = p.categoryid, sale = p.sale, Description = p.Description }).Take(20); 
                List<Productinfolist> list_cart = Results.ToList<Productinfolist>();

                Cnn.Open();
                foreach (var item in list_cart)
                {
                    if (id == 1)
                    {
                        DataRow dr = Cnn.FillRow("select top 1 coalesce(urate,0) as price,coalesce(udiscount,0) as discount from ProductSizeQuantity where ProductSizeQuantity.ProductID=" + item.productid + " ");
                        int price = Convert.ToInt32(dr["price"]);
                        item.price = price;
                        int discount = Convert.ToInt32(dr["discount"]);
                        item.discount = discount;


                    }
                    else if (id == 2)
                    {
                        DataRow dr = Cnn.FillRow("select top 1 coalesce(arate,0) as price,coalesce(adiscount,0) as discount from ProductSizeQuantity where ProductSizeQuantity.ProductID=" + item.productid + " ");
                        int price = Convert.ToInt32(dr["price"]);
                        item.price = price;
                        int discount = Convert.ToInt32(dr["discount"]);
                        item.discount = discount;
                    }

                    item.Image = string.Join("/", db.dtl_ProductGallery.Where(x => x.Product_Id == item.productid).ToList().Select(x => x.ImageCode.ToString()));
                    item.PColorId = string.Join("/", (from p in db.ProductSizeQuantities join q in db.Colors on p.ColorId equals q.ColorId where p.ProductID == item.productid select q).Select(x => x.ColorId).Distinct().ToList());
                    item.PColor = string.Join("/", (from p in db.ProductSizeQuantities join q in db.Colors on p.ColorId equals q.ColorId where p.ProductID == item.productid select q).Select(x => x.Colorimg).Distinct().ToList());
                    item.Quantity = string.Join("/", (from p in db.ProductSizeQuantities where p.ProductID == item.productid select p).Select(x => x.Quantity).ToList());

                }

                Cnn.Close();
                return list_cart;
            }
            else
            {
                //.Skip(Exid * 20).Take(20);
                var Results = (from p in db.products
                               where p.categoryid == Exid
                               select new Productinfolist { productid = p.productid, productname = p.productname, categoryid = p.categoryid, sale = p.sale, Description = p.Description }).OrderByDescending(x => x.productid).Skip(Exid * 20).Take(20);
                List<Productinfolist> list_cart = Results.ToList<Productinfolist>();

                Cnn.Open();
                foreach (var item in list_cart)
                {
                    if (id == 1)
                    {
                        DataRow dr = Cnn.FillRow("select top 1 coalesce(urate,0) as price,coalesce(udiscount,0) as discount from ProductSizeQuantity where ProductSizeQuantity.ProductID=" + item.productid + " ");
                        int price = Convert.ToInt32(dr["price"]);
                        item.price = price;
                        int discount = Convert.ToInt32(dr["discount"]);
                        item.discount = discount;


                    }
                    else if (id == 2)
                    {
                        DataRow dr = Cnn.FillRow("select top 1 coalesce(arate,0) as price,coalesce(adiscount,0) as discount from ProductSizeQuantity where ProductSizeQuantity.ProductID=" + item.productid + " ");
                        int price = Convert.ToInt32(dr["price"]);
                        item.price = price;
                        int discount = Convert.ToInt32(dr["discount"]);
                        item.discount = discount;
                    }

                    item.Image = string.Join("/", db.dtl_ProductGallery.Where(x => x.Product_Id == item.productid).ToList().Select(x => x.ImageCode.ToString()));
                    item.PColorId = string.Join("/", (from p in db.ProductSizeQuantities join q in db.Colors on p.ColorId equals q.ColorId where p.ProductID == item.productid select q).Select(x => x.ColorId).Distinct().ToList());
                    item.PColor = string.Join("/", (from p in db.ProductSizeQuantities join q in db.Colors on p.ColorId equals q.ColorId where p.ProductID == item.productid select q).Select(x => x.Colorimg).Distinct().ToList());
                    item.Quantity = string.Join("/", (from p in db.ProductSizeQuantities where p.ProductID == item.productid select p).Select(x => x.Quantity).ToList());

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

        public JoinColorDetails GetColorDetail(int id, int Exid)
        {

            var Results = new JoinColorDetails();
            string sid = "",sname="";
            Cnn.Open();
             sname =Cnn.ExecuteScalar("DECLARE @temp VARCHAR(MAX) SELECT @temp = COALESCE(@temp+'/' ,'') + varient FROM [dbo].varient as a  inner join ProductSizeQuantity as b on a.id=b.size where b.productid="+id+" and b.colorid="+Exid+" SELECT @temp ").ToString();
            sid = Cnn.ExecuteScalar(" DECLARE @temp VARCHAR(MAX) SELECT @temp = COALESCE(@temp+'/' ,'') + ImageCode FROM [dbo].dtl_ProductGallery where  product_id="+id+" and colorid="+Exid+" SELECT @temp ").ToString();
            Cnn.Close();
            Results.Images = sid;
            Results.Size = sname;
            Results.Quantity = string.Join("/", db.ProductSizeQuantities.Where(x => x.ProductID == id && x.ColorId == Exid).ToList().Select(x => x.Quantity));
            Results.ProductColorRate = string.Join("/", db.ProductSizeQuantities.Where(x => x.ProductID == id && x.ColorId == Exid).Select(x => x.urate).ToList());
            Results.ProductColorDiscount = string.Join("/", db.ProductSizeQuantities.Where(x => x.ProductID == id && x.ColorId == Exid).Select(x => x.udiscount).ToList());
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
            string sid = "",descr="";
            Cnn.Open();
            sid =Cnn.ExecuteScalar("DECLARE @temp VARCHAR(MAX) SELECT @temp = COALESCE(@temp+'/' ,'') + varient FROM [dbo].varient as a  inner join ProductSizeQuantity as b on a.id=b.size where b.productid="+id+" SELECT @temp ").ToString();
            descr = Cnn.ExecuteScalar("select Description from Product where productid="+id+" ").ToString();
            Cnn.Close();

            Results.Sizeid = string.Join("/", (from p in db.ProductSizeQuantities  where p.ProductID == id  select p.Size).Distinct().ToList());
            Results.Sizename = sid;
            Results.Image = string.Join("/", db.dtl_ProductGallery.Where(x => x.Product_Id == id).ToList().Select(x => x.ImageCode.ToString()));
            Results.PColorId = string.Join("/", (from p in db.ProductSizeQuantities join q in db.Colors on p.ColorId equals q.ColorId where p.ProductID == id  select q).Select(x => x.ColorId).Distinct().ToList());
            Results.PColor = string.Join("/", (from p in db.ProductSizeQuantities join q in db.Colors on p.ColorId equals q.ColorId where p.ProductID == id select q).Select(x => x.Colorimg).Distinct().ToList());
            Results.Quantity = string.Join("/", (from p in db.ProductSizeQuantities where p.ProductID == id select p).Select(x => x.Quantity).ToList());
            Results.Description = descr;
            return Results;
        }









        public string GetCourier(int id, string Exid)
        {
             
            Cnn.Open();
            int Couriergm=0, Courierkg=0;        
            SqlDataAdapter sqlDa = new SqlDataAdapter("select a.id,a.productid,a.color,a.Size,b.productname,a.Quantity,b.image,a.id as rate,a.id as discount  from trncart as a inner join  product as b on a.productid=b.productid Where a.UserId="+id+"", sqlCon);
            DataTable Dt = new DataTable();
            sqlDa.Fill(Dt);
            String CourierState = Convert.ToString(Cnn.ExecuteScalar("Select State_Name from Loc_State where State_Name='"+Exid+"'"));
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
                        Cnn.ExecuteNonQuery("delete from trncart where userid=" + obj.UserId + " and productid="+Convert.ToInt32(pid1[i].ToString())+"");
                        Cnn.Close();
                        obj.Id = db.TrnCarts.Any() ? db.TrnCarts.Max(x => x.Id + 1) : 1;
                        obj.UserId = obj.UserId;
                        obj.ProductId = Convert.ToInt32(pid1[i].ToString());
                        obj.Size = Size1[i].ToString();
                        obj.Quantity = Convert.ToInt32(pqty1[i].ToString());
                        obj.Color =Color1[i].ToString();
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
               count=Convert.ToInt32(Cnn.ExecuteScalar("select count(*) from trncart where userid=" + obj.UserId + " and productid=" + Convert.ToInt32(obj.pid) + ""));
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
                    Cnn.ExecuteNonQuery("delete from trncart where userid=" + obj.UserId + " and productid=" + obj.pid+ "");
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
                Cnn.ExecuteNonQuery("update trncart set Quantity="+obj.qty+" where userid=" + obj.UserId + " and productid=" + obj.pid + "");
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
            var Results = (from p in db.products join q in db.TrnCarts on p.productid equals q.ProductId
                           where q.UserId==id
                           select new Cart { ProductId = p.productid, productname = p.productname,colorid=q.Color, qty=q.Quantity,size=q.Size});
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

        public varient AjaxMethod(varient person)
        {
            person.varient1 = DateTime.Now.ToString();
            return person;
        }
    }  
}


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
    public class OrderController : ApiController
    {
        friendsmobileEntities db = new friendsmobileEntities();
     //   ClsQuantity objQty = new ClsQuantity();

        ClsConnection Cnn = new ClsConnection();



        public HttpResponseMessage GenerateOrder(TrnOrderMain obj)
        {
            
                try
                {
                    int Amount = 0, PaidAmount = 0, DeliveryCharges = 0, OrderAmount = 0, Quantity = 0, AdditionalCharges=0;
                    string strMsg_Admin = "", strMsg_User = "",  strMsgCart = "", strEmailCart = "",Res_Email = "", FranchiseMobileNo = "", FranchiseEmailId = "";
                    string Admin_Mobile = "9214472437";
                    string Admin_Mail = "obhilwara@gmail.com";
                    string Cashback1 = "";
                    decimal TotalDiscount = 0;
                    int sid = 0,neworderid=0;
                    string colorname = "", varianame = "";
                    if (obj.OrderThrough == "") { obj.OrderThrough = "App"; }
                    Cnn.Open();
                    sid = Convert.ToInt32(Cnn.ExecuteScalar("select sid from sessiontest where active=1"));
                     neworderid = Convert.ToInt32(Cnn.ExecuteScalar("Select isnull( max(OrderId),0)+1 from trnordermain where sid="+sid+""));
                 
                    Cnn.Close();
                    SqlConnection sqlCon = new SqlConnection(@"Data Source= 103.71.99.32; Database=virtualelectronics;initial catalog=virtualelectronics;User id=virtualelectronics;Password=electronics@55;Max Pool Size=2000;");
                //    SqlConnection sqlCon = new SqlConnection(@"Data Source= 103.71.99.32; Database= friendsmobile;initial catalog=friendsmobile;User id=friendsmobile;Password=friendsmobile@55");
                    sqlCon.Open();

                    obj.OrderId = neworderid;
                    SqlDataAdapter sqlDa = new SqlDataAdapter("select a.id,a.productid,a.color,a.Size,b.productname,a.Quantity,b.image,a.id as rate,a.id as discount  from trncart as a inner join  product as b on a.productid=b.productid Where a.UserId=" + obj.Usercode + "", sqlCon);
                    DataTable Dt = new DataTable();
                    sqlDa.Fill(Dt);                 
                    FranchiseMobileNo ="9214472437";
                    FranchiseEmailId = "info@virtualinfosystems.com";
                    int calamt=0;
                    int calamount = 0, caldiscount = 0;
                    // FranchiseMobileNo = "9214472437";
                    Cnn.Open();
                    for (int i = 0; i < Dt.Rows.Count; i++)
                    {
                       
                        DataRow dr = Cnn.FillRow("select urate as rate ,uDiscount as Discount from  ProductSizeQuantity  where    ProductId=" + Dt.Rows[i]["ProductId"] + "  and Colorid=" + Dt.Rows[i]["Color"] + " and Size=" + Dt.Rows[i]["Size"] + "");
                        int Discount = Convert.ToInt32(dr["Discount"]);                           
                            int Rate = Convert.ToInt32(dr["Rate"]);
                            Dt.Rows[i]["rate"] = Rate;
                            Dt.Rows[i]["Discount"] = Discount;
                            calamount = Convert.ToInt32(Dt.Rows[i]["rate"]) * Convert.ToInt32(Dt.Rows[i]["Quantity"]);
                            caldiscount = Convert.ToInt32(Dt.Rows[i]["Discount"]) * Convert.ToInt32(Dt.Rows[i]["Quantity"]);

                            calamt = calamount - caldiscount;
                      
                        var DisInvoiceNo = "PI/0"+sid+"/" + obj.OrderId;
                        Amount += calamt;
                        AdditionalCharges = 0;
                       
                        Quantity += Convert.ToInt32(Dt.Rows[i]["Quantity"]);
                        TotalDiscount += caldiscount;                      
                        Cashback1 = "0";
                        SqlCommand Sql = new SqlCommand("insert into TrnOrderDetail(InvoiceNo,DisInvoiceNo,OrderId,StoreId,ProductId,Unit,Weight,Quantity,Rate,Size,Color,Cashback,Amount,Discount,NetAmount,Cancelled,Delivered,Returned,Dispatched,RTS,DeliveryDate,DeliveryCharges,AdditionalCharges,RequestReason,remark,sid) values (" + obj.OrderId + ",'" + DisInvoiceNo + "'," + obj.OrderId + ",'0'," + Dt.Rows[i]["ProductId"] + ",'','10'," + Dt.Rows[i]["Quantity"] + "," + Dt.Rows[i]["Rate"] + ",'" + Dt.Rows[i]["Size"] + "','" + Dt.Rows[i]["Color"] + "'," + Cashback1 + "," +calamount + "," +caldiscount + "," + calamt + ",0,0,0,0,GetDate(),GetDate(),'"+obj.DeliveryFees+"','0','',''," + sid + ")", sqlCon);
                        string balanceObj = Sql.ExecuteNonQuery().ToString();
                        var ProductImage = Dt.Rows[i]["Image"].ToString();
                        var ProductName = Dt.Rows[i]["ProductName"].ToString();
                        if (Dt.Rows[i]["Color"].ToString() != "1")
                        {
                            colorname = "( "+ Cnn.ExecuteScalar("select colorname from color where colorid=" + Dt.Rows[i]["Color"] + "").ToString() +" )";
                        }
                        if (Dt.Rows[i]["Size"].ToString() != "-1")
                        {
                            varianame = "( " + Cnn.ExecuteScalar("select varient from varient where id=" + Dt.Rows[i]["Size"] + "").ToString() + " )"; 
                        }
                        strMsgCart = strMsgCart + ProductName + "  Qty(" + Dt.Rows[i]["Quantity"].ToString() + ")  ,Amt. " + calamt+ "   ";
                        strEmailCart = strEmailCart + "<tr><td align='center'><img src='http://virtualelectronics.bharatdial.com/img/product/" + ProductImage + "' width='70' height='74' padding-top='10' padding-bottom='10'/></td><td padding='10' align='center'>" + ProductName + colorname + varianame + " <br/> </td><td padding='10' align='center'> " + Dt.Rows[i]["Quantity"] + "</td><td padding='10' align='center'>" + Dt.Rows[i]["rate"] + "</td><td padding='10' align='center'>" + Dt.Rows[i]["Quantity"] + "</td><td padding='10' align='center'>" + calamount + "</td><td padding='10' align='center'>" + caldiscount + "</td><td padding='10' align='center'>" + calamt + "</td></tr>";
                      
                    }
                    Cnn.Close();

                    DeliveryCharges = Convert.ToInt32(obj.DeliveryFees);
                   
                 
                    OrderAmount = DeliveryCharges + Amount;
                    PaidAmount = Convert.ToInt32(OrderAmount) - Convert.ToInt32(obj.WalletAmount);
                    SqlCommand MainSql = new SqlCommand("insert into TrnOrderMain(OrderId,Usercode,OrderAmount,WalletAmount,PaidAmount,TotalQuantity,DeliveryFees,Amount,Paymode,TrnId,TrnStatus,OrderThrough,ShippingName,ShippingAddress,ShippingMobileNo,ShippingEmailId,ShippingCity,ShippingZipcode,ShippingStats,Delivered,Cancelled,Returned,Dispatched,Settlement,RTS,DeliveryDate,PromoCodeId,PromoCodeAmount,DeliveryboyId,FranchiseMessage,remark,sid) values (" + obj.OrderId + "," + obj.Usercode + "," + OrderAmount + "," + obj.WalletAmount + "," + PaidAmount + "," + Quantity + "," + DeliveryCharges + "," + Amount + ",'" + obj.Paymode + "','" + obj.TrnId + "','" + obj.TrnStatus + "','" + obj.OrderThrough + "','" + obj.ShippingName + "','" + obj.ShippingAddress + "','" + obj.ShippingMobileNo + "','" + obj.ShippingEmailId + "','" + obj.ShippingCity + "','" + obj.ShippingZipcode + "','" + obj.ShippingStats + "',0,0,0,0,0,GetDate(),GetDate()," + obj.PromoCodeId + "," + obj.PromoCodeAmount + ",0,'','',"+sid+")", sqlCon);
                    string balanceObj1 = MainSql.ExecuteNonQuery().ToString();
                  
                    if (obj.WalletAmount != null && obj.WalletAmount != 0)
                    {

                        SqlCommand MainSql11 = new SqlCommand("insert into TrnWallet(UserId,OrderId,TrnType,PromoCodeAmount,Amount,IsDeducted,RTS,OrderType,Type,ReMark) values (" + obj.Usercode + "," + obj.OrderId + ",'City','0'," + obj.WalletAmount + ",1,GetDate(),'','','Wallet Point Deducted')", sqlCon);
                        string balanceObj11 = MainSql11.ExecuteNonQuery().ToString();
                        SqlCommand Wallet = new SqlCommand("update register set Wallet=Wallet-" + obj.WalletAmount + " where userid=" + obj.Usercode + "", sqlCon);
                        string Walexcute = Wallet.ExecuteNonQuery().ToString();
                        register register = new register();

                    }
                   

                    //
                    //  mAil & Message  Code Start
                    if (obj.DeliveryFees != null && obj.DeliveryFees != 0)
                    {
                        strMsgCart = strMsgCart + "Discount " + TotalDiscount + ", Delivery Charges Rs." + DeliveryCharges + " , Wallet Amount Rs.:" + Convert.ToInt32(obj.WalletAmount) + "";
                    }
                   
                    string Wallamt = "", wallietmsg = "";

                    if (obj.WalletAmount > 0)
                    {
                        wallietmsg = "Wallet Amount Rs.:" +Convert.ToInt32(obj.WalletAmount) + ",";
                        Wallamt = "<tr><td align='right' style='padding-right:10'><b>Wallet Amount &nbsp;(-)&nbsp;&nbsp;: Rs." + Convert.ToInt32(obj.WalletAmount) + " </b></td></tr>";
                    }
                    else
                    {
                        Wallamt = "";
                        wallietmsg = "";
                    }

                   
                    strMsg_Admin = "You Have Received New Order No." + obj.OrderId + " Amt Rs." + PaidAmount + "  Delivery Address: " + obj.ShippingName + ", " + obj.ShippingMobileNo;
                    strMsg_User = "Thanks for your Order No." + obj.OrderId + " Amt Rs." + PaidAmount + " Order Details: " + strMsgCart + " Del Address: " + obj.ShippingName + ", " + obj.ShippingAddress + ", " + obj.ShippingCity + ". Will be delivered soon Visit: virtualelectronics.bharatdial.com";
                    //strMsg_Restaurant = "Order No." + obj.Order_ID + " Amount Rs." + obj.Order_TotalAmount + " Order Details: " + strMsgCart + " Delivery Address: " + obj.Shipper_Name + ", " + obj.Shipper_Address + ", " + obj.Shipper_City + ". Order by: " + obj.User_Name + " " + obj.User_Mobile + " Visit: www.CityStore.in";
                    ////Send Email
                    string strEmail_Admin = "<html>" + DateTime.Now + "<br/><br/><br />Hello, &nbsp;Admin";
                    strEmail_Admin += "<html><head><body><div style='border-style: double; border-width: 10px; border-color:#cccccc; padding:20px; Font-size:11px; color:black;width:590px'><img src='http://virtualelectronics.bharatdial.com/img/logo/logo.jpg'  alt='logo'    border='0'/><br /><br />";
                    strEmail_Admin += "Order No. : <b>" + obj.OrderId + "</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Order Date : <b>" + DateTime.Now + "</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Order Through : " + obj.OrderThrough + " &nbsp;&nbsp;&nbsp;&nbsp; Payment Mode : " + obj.Paymode + "<br><br/>";
                    strEmail_Admin += "<strong></strong><br/><br/><body><table width='590' height='auto' border='1' cellpadding='0' cellspacing='0'><tr bgcolor='#cbc7c7'  align='center'><td height='30'>Image</td><td color='#ffffff'>Particulars</td><td>Unit</td><td>Rate</td><td>Qty.</td><td>Amount</td><td>Discount</td><td>NetAmount</td></tr>" + strEmailCart + "</table></body>";
                    strEmail_Admin += "<table width='590' Font-size:'11'><tr><td align='right' style='padding-right:10'>Amount &nbsp;&nbsp;&nbsp;<b>Rs." + Amount + "</b></td></tr><td><hr/></td></tr><tr><td align='right' style='padding-right:10'><b>Delivery Charges &nbsp;&nbsp;&nbsp;: Rs." + DeliveryCharges + " </b></td></tr>" + Wallamt + "<tr><td align='right' style='padding-right:10'><b>Net Amount &nbsp;&nbsp;&nbsp;: Rs." + PaidAmount + " </b></td></tr><tr><td><hr/></td></tr></table><br/>";
                    strEmail_Admin += "Subject to T&C on <a href='https://virtualelectronics.bharatdial.com/'>www.virtualelectronics.bharatdial.com</a><br/><br/>";
                    strEmail_Admin += "<b>Delivery Address: </b><br/>";
                    strEmail_Admin += obj.ShippingName + "<br/>";
                    strEmail_Admin += obj.ShippingAddress + "<br/>";
                    strEmail_Admin += obj.ShippingCity + "<br/>";
                    strEmail_Admin += "</div></body></head></html>";
                    string strEmail_User = "<html>" + DateTime.Now + "<br/><br/><br />Hello, &nbsp;&nbsp;<b>" + obj.ShippingName + "</b><br/><br/>";
                    strEmail_User += "<html><head><body><div style='border-style: double; border-width: 10px; border-color:#cccccc; padding:20px; Font-size:11px; color:black;width:590px'><img src='http://virtualelectronics.bharatdial.com/img/logo/logo.jpg'  alt='logo'    border='0'/><br /><br />";
                    strEmail_User += "Order No.: <b>" + obj.OrderId + "</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Order Date : <b>" + DateTime.Now + "</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Order Through : " + obj.OrderThrough + " &nbsp;&nbsp;&nbsp;&nbsp; Payment Mode : " + obj.Paymode + "<br><br/>";
                    strEmail_User += "<strong></strong><br/><br/><body><table width='590' height='auto' border='1' cellpadding='0' cellspacing='0'><tr bgcolor='#cbc7c7'  align='center'><td height='30'>Image</td><td color='#ffffff'>Particulars</td><td>Unit</td><td>Rate</td><td>Qty.</td><td>Amount</td><td>Discount</td><td>NetAmount</td></tr>" + strEmailCart + "</table></body>";
                    strEmail_User += "<table width='590' Font-size:'11'><tr><td align='right' style='padding-right:10'>Amount &nbsp;&nbsp;&nbsp;<b>Rs." + Amount + "</b></td></tr><td><hr/></td></tr><tr><td align='right' style='padding-right:10'><b>Delivery Charges &nbsp;&nbsp;&nbsp;: Rs." + DeliveryCharges + " </b></td></tr>" + Wallamt + "<tr><td align='right' style='padding-right:10'><b>Net Amount &nbsp;&nbsp;&nbsp;: Rs." + PaidAmount + " </b></td></tr><tr><td><hr/></td></tr></table><br/>";
                    strEmail_User += "Subject to T&C on <a href='https://virtualelectronics.bharatdial.com/'>www.virtualelectronics.bharatdial.com</a><br/><br/>";
                    strEmail_User += "<b>Delivery Address: </b><br/>";
                    strEmail_User += obj.ShippingName + "<br/>";
                    strEmail_User += obj.ShippingAddress + "<br/>";
                    strEmail_User += obj.ShippingCity + "<br/>";
                    strEmail_User += "For any query or assistance, feel free to contact<br/>Thanks & Regards <br/><b>virtualelectronics.bharatdial.com</b><br/>Mobile No. 91- ,&nbsp;<br/>virtualelectronics";
                    strEmail_User += "</div></body></head></html>";
                    //************ Send Email **************************************************************/             

                    Msg.SendEmailtoAll(obj.ShippingEmailId, "New Order:virtualelectronics.bharatdial.com", strEmail_User);//send email to user
                    Msg.SendSMS1(obj.ShippingMobileNo, strMsg_User); //send sms to user.                   
                //    Msg.SendSMS1("9468598100", strMsg_Admin);

                //    Msg.SendEmailtoAll("info@virtualinfosystems.com", "New Order: virtualelectronics.bharatdial.com", strEmail_Admin);//send email to user
                    SqlDataAdapter sqlDa1 = new SqlDataAdapter("select * from trncart where userid='" + obj.Usercode + "'", sqlCon);
                    DataTable Dt1 = new DataTable();
                    sqlDa1.Fill(Dt1);
                    SqlCommand Sql5 = new SqlCommand("delete   from  trncart  where userId='" +obj.Usercode + "'", sqlCon);
                    string Del = Sql5.ExecuteNonQuery().ToString();
                    //if (Dt1.Rows.Count > 0)
                    //{ }
                    //{
                    //    for (int i = 0; i < Dt1.Rows.Count; i++)
                    //    {
                    //        if (Dt.Rows[i]["Size"].ToString() == "" && Dt.Rows[i]["Color"].ToString() == "")
                    //        {
                    //            SqlCommand NewSql1 = new SqlCommand("SELECT Quantity from  TrnOrderDetail  where ProductId='" + Dt.Rows[i]["ProductId"] + "' and OrderId='" + obj.OrderId + "'", sqlCon);
                    //            string Excut1 = NewSql1.ExecuteScalar().ToString();
                    //            SqlCommand Sql3 = new SqlCommand("update   store.Product set Quantity=Quantity-'" + Excut1 + "'  where ProductId='" + Dt.Rows[i]["ProductId"] + "'", sqlCon);
                    //            string Excut2 = Sql3.ExecuteNonQuery().ToString();
                    //         //   SqlCommand Sql5 = new SqlCommand("delete   from  trncart  where Id='" + Dt.Rows[i]["Id"] + "'", sqlCon);
                    //          //  string Del = Sql5.ExecuteNonQuery().ToString();
                    //        }
                        
                    //        if (Dt.Rows[i]["Size"].ToString() != "" && Dt.Rows[i]["Color"].ToString() != "")
                    //        {
                    //            SqlCommand NewSql1 = new SqlCommand("SELECT Quantity from  TrnOrderDetail   where ProductId='" + Dt.Rows[i]["ProductId"] + "' and Color='" + Dt.Rows[i]["Color"] + "' and Size='" + Dt.Rows[i]["Size"] + "' and orderid='" + obj.OrderId + "'", sqlCon);
                    //            string Excut1 = NewSql1.ExecuteScalar().ToString();
                    //            SqlCommand qwert = new SqlCommand("update   store.Product set Quantity=Quantity-'" + Excut1 + "'  where ProductId='" + Dt.Rows[i]["ProductId"] + "'", sqlCon);
                    //            string Excut2 = qwert.ExecuteNonQuery().ToString();
                    //            SqlCommand Qty = new SqlCommand("SELECT Quantity from  TrnOrderDetail  where ProductId='" + Dt.Rows[i]["ProductId"] + "' and Color='" + Dt.Rows[i]["Color"] + "' and Size='" + Dt.Rows[i]["Size"] + "' and orderid='" + obj.OrderId + "'", sqlCon);
                    //            string Sql1 = Qty.ExecuteScalar().ToString();
                    //            SqlCommand Sql3 = new SqlCommand("update  ProductSizeQuantity set Quantity=Quantity-'" + Sql1 + "'  where ProductId='" + Dt.Rows[i]["ProductId"] + "' and ColorId='" + Dt.Rows[i]["Color"] + "' and Size='" + Dt.Rows[i]["Size"] + "'", sqlCon);
                    //            string balanceObj22 = Sql3.ExecuteNonQuery().ToString();
                    //            SqlCommand Colorname = new SqlCommand("SELECT Color_Name from   Store.mst_Color  where Color_Id='" + Dt.Rows[i]["Color"] + "'", sqlCon);
                    //            string Sql = Colorname.ExecuteScalar().ToString();
                    //            SqlCommand Sql15 = new SqlCommand("update   store.dtl_ProductColor set Quantity=Quantity-'" + Sql1 + "'  where ProductId='" + Dt.Rows[i]["ProductId"] + "' and ColorName='" + Colorname + "'", sqlCon);
                    //            string balanceObj221 = Sql15.ExecuteNonQuery().ToString();
                    //            SqlCommand Sql5 = new SqlCommand("delete   from  trncart  where Id='" + Dt.Rows[i]["Id"] + "'", sqlCon);
                    //            string Del = Sql5.ExecuteNonQuery().ToString();

                    //        }
                    //    }
                    //}
                    sqlCon.Close();
                    return Request.CreateResponse(HttpStatusCode.Created, "OK");
                }
                catch (Exception ex)
                {

                    return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                }
                //   finally { db.Dispose(); }
            
        }



        public IEnumerable<JoinOrderMain> GetOrder(int id)
        {


            
            var Results = (from p in db.TrnOrderMains
                           where p.Usercode == id
                          
                           select new JoinOrderMain { OrderId = p.OrderId, Usercode = p.Usercode, OrderAmount = p.OrderAmount, TotalQuantity = p.TotalQuantity, DeliveryFees = p.DeliveryFees, Amount = p.Amount, Paymode = p.Paymode, TrnId = p.TrnId, TrnStatus = p.TrnStatus, OrderThrough = p.OrderThrough, ShippingName = p.ShippingName, ShippingAddress = p.ShippingAddress, ShippingMobileNo = p.ShippingMobileNo, ShippingEmailId = p.ShippingEmailId, ShippingCity = p.ShippingCity, ShippingZipcode = p.ShippingZipcode, ShippingStats = p.ShippingStats, Delivered = p.Delivered, Cancelled = p.Cancelled, Returned = p.Returned, Dispatched = p.Dispatched, Settlement = p.Settlement, RTS = p.RTS, WalletAmount = p.WalletAmount, PaidAmount = p.PaidAmount, DeliveryDate = p.DeliveryDate}).OrderByDescending(x => x.OrderId);
            bool breturnable = true, bcancellable = true, bdelivered = false;
            List<JoinOrderMain> orderList = Results.ToList();
            SqlConnection sqlCon = new SqlConnection(@"Data Source= 103.71.99.32; Database=virtualelectronics;initial catalog=virtualelectronics;User id=virtualelectronics;Password=electronics@55;Max Pool Size=2000;");
       
            return orderList;
        }


        public IEnumerable<JoinOrderProduct> GetOrderDetail(int id)
        {

            var Results = (from p in db.TrnOrderDetails
                           join q in db.products on p.ProductId equals q.productid
                          
                           where p.OrderId == id
                           from x in db.Colors.Where(x => SqlFunctions.StringConvert((double)x.ColorId).TrimStart() == p.Color).DefaultIfEmpty()
                           from y in db.dtl_ProductGallery.Where(y => SqlFunctions.StringConvert((double)y.ColorId).TrimStart() == (p.Color == "" ? "-1" : p.Color) && y.Product_Id == p.ProductId).DefaultIfEmpty().Take(1)
                           select new JoinOrderProduct { OrderId = p.OrderId, ProductId = p.ProductId, Image = q.image, ProductName = q.productname, Unit = p.Unit, Weight = p.Weight, Quantity = p.Quantity, Rate = p.Rate, Size = p.Size, Color = p.Color, Cashback = p.Cashback, Amount = p.Amount, Discount = p.Discount, NetAmount = p.NetAmount, Cancelled = p.Cancelled, Delivered = p.Delivered, Returned = p.Returned, RTS = p.RTS, DeliveryDate = p.DeliveryDate, ColorName = x.ColorName == null ? "" : x.ColorName, ColorImage = x.Colorimg == null ? "" : x.Colorimg, AID = p.AID, Dispatched = p.Dispatched, DeliverStatus = DateTime.Now < EntityFunctions.AddDays(p.DeliveryDate, 3) == true ? true : false });
            return Results;

        }


        public HttpResponseMessage OrderCancel(TrnOrderMain Up)
        {
            var NewObj = db.TrnOrderMains.SingleOrDefault(x => x.OrderId == Up.OrderId);
            try
            {
                if (NewObj != null)
                {
                    SqlConnection sqlCon = new SqlConnection(@"Data Source= 103.71.99.32; Database=virtualelectronics;initial catalog=virtualelectronics;User id=virtualelectronics;Password=electronics@55;Max Pool Size=2000;");
                    sqlCon.Open();
                    SqlCommand NewSql = new SqlCommand("update  TrnOrderMain set Cancelled=1,Delivered=0,Returned=0,Dispatched=0,Settlement=0,remark='" + Up.remark.Replace("'", "''").Trim() + "' where   OrderId=" + Up.OrderId + " and  usercode=" + Up.Usercode + "", sqlCon);
                    string balanceObj = NewSql.ExecuteNonQuery().ToString();
                    sqlCon.Close();             
                 
                }
            }
            catch (OptimisticConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage OrderReturn(TrnOrderMain Up)
        {
            var NewObj = db.TrnOrderMains.SingleOrDefault(x => x.OrderId == Up.OrderId);
            try
            {
                if (NewObj != null)
                {
                    SqlConnection sqlCon = new SqlConnection(@"Data Source= 103.71.99.32; Database= friendsmobile;initial catalog=friendsmobile;User id=friendsmobile;Password=friendsmobile@55");
                    sqlCon.Open();
                    SqlCommand NewSql = new SqlCommand("update  TrnOrderMain set Cancelled=0,Delivered=0,Returned=1,Dispatched=0,Settlement=0,remark='" + Up.remark.Replace("'", "''").Trim() + "' where   OrderId=" + Up.OrderId + " and  usercode=" + Up.Usercode + "", sqlCon);
                    string balanceObj = NewSql.ExecuteNonQuery().ToString();
                    sqlCon.Close();

                }
            }
            catch (OptimisticConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}

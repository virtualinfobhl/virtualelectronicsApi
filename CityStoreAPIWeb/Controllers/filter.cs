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
using System.Configuration;

namespace CityStoreAPIWeb.Controllers
{
    public class filterController : ApiController
    {
        friendsmobileEntities db = new friendsmobileEntities();
        SqlConnection sqlCon = new SqlConnection(
ConfigurationManager.ConnectionStrings["VirtualInfoSystems"].ConnectionString
);
        // SqlConnection sqlCon = new SqlConnection(@"Data Source= 103.71.99.32; Database=virtualelectronics;initial catalog=virtualelectronics;User id=virtualelectronics;Password=electronics@55;Max Pool Size=2000;");
        //   ClsQuantity objQty = new ClsQuantity();
        ClsConnection Cnn = new ClsConnection();



        //http://localhost:9080/api/filter/GetBrand/1
        //http://friendsmobile.virtualappstore.in/api/filter/GetBrand/1
        public HttpResponseMessage GetBrand(int id)
        {
            //cateforyid
            try
            {
                DataTable ds = new DataTable();
                sqlCon.Open();
                string querystring = "Select distinct a.brandid,a.brandname from brand as a  INNER JOIN PRODUCT as c on a.brandid=c.brandid where c.categoryid=" + id + "";  // 3521 "           
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

        //http://localhost:9080/api/filter/GetVarient/1/2
        //http://friendsmobile.virtualappstore.in/api/filter/GetVarient/1/2
        public HttpResponseMessage GetVarient(int id, int Exid)
        {
            // cateforyid,brandid
            try
            {
                DataTable ds = new DataTable();
                sqlCon.Open();
                string querystring = "select distinct  a.id,a.varient from varient as a inner join ProductSizeQuantity as b on a.id=b.size INNER JOIN PRODUCT as c on b.ProductID=c.ProductID where c.brandid=" + Exid + " and  c.categoryid=" + id + "";  // 3521 "           
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

        //http://localhost:9080/api/filter/GetColor/1/2/1
        //http://friendsmobile.virtualappstore.in/api/filter/GetColor/1/2/1
        public HttpResponseMessage GetColor(int id, int Exid, int Exxid)
        {
            // cateforyid,brandid,varianid
            try
            {
                DataTable ds = new DataTable();
                sqlCon.Open();
                string querystring = "select distinct  a.ColorId,a.ColorName from color as a inner join ProductSizeQuantity as b on a.ColorId=b.ColorId inner join product as c on b.productid=c.productid where b.size=" +Exxid + "  and c.categoryid=" + id + " and c.brandid=" + Exid + " order by  a.ColorName asc";  // 3521 "           
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

        //http://friendsmobile.virtualappstore.in/api/filter/GetFilterProducts/1/2/1/2/-1/0
              //http://localhost:9080/api/filter/GetFilterProducts/1/2/1/2/-1/0
        public HttpResponseMessage GetFilterProducts(string id, string Exid, string Exxid, string Exxxid, string Exxxxid, string pera6)
        {
            string querystring = "", SkipId = "", variant = "", Rate = "", color = "", brand = "",  MiniCategoryId = "",  orderby = "";

            // 1.categoryid,2.brandid,3.variant,4.color,5.orderby,6.skipid
          
            if (Exid != "-1")
            {
                brand = "AND a.brandid='" +Exid + "'";
            }
            if (Exxid == "0")
            {
                variant = "";
            }
            else
            {
                variant = "AND b.Size='" + Exxid + "'";
            }
            if (Exxxid == "0")
            {
                variant = "";
            }
            else
            {
                variant = "AND b.ColorId='" + Exxxid + "'";
            }


            if (Exxxxid == "-1")
            {
                orderby = "order by ProductName asc";
            }
            else if (Exxxxid == "0")
            {
                orderby = "order by price asc"; ;
            }
            else if (Exxxxid == "1")
            {
                orderby = "order by price desc";
            }


            if (pera6 != "-1")
            {
                SkipId = pera6;
            }
            
            DataTable ds = new DataTable();
            sqlCon.Open();
            querystring = "select distinct  a.productid,SUBSTRING(a.productname, 1,10) as productname,a.image as  Image,a.sale,a.categoryid,(select top 1 coalesce(urate,0) from ProductSizeQuantity where ProductSizeQuantity.ProductID=a.ProductID) as price,(select  top 1 coalesce(udiscount,0) from ProductSizeQuantity where ProductSizeQuantity.ProductID=a.ProductID) as discount from product as a   join  ProductSizeQuantity as b  on a.ProductID=b.ProductID where a.active=1  and a.categoryid=" + id + "  " + variant + " " + brand + " " + color + " " + orderby + " OFFSET " + SkipId + " Row FETCH NEXT 16 ROWS ONLY";  // 3521 "           
            SqlCommand cmd = new SqlCommand(querystring, sqlCon);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlCon.Close();
            return Request.CreateResponse(HttpStatusCode.OK, ds);
        }

      
    }




    
}


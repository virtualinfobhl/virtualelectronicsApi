using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;


namespace CSMerchantWebAPI
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
     
            //using (SqlConnection conn = new SqlConnection(@"Data Source= 111.118.190.141; Database= CityStoreNew;initial catalog=CityStoreNew;User id=CityStoreNew;Password=NewStore@City77"))
            //{
            //    DataSet ds = new DataSet();
            //    conn.Open();
            //    string querystring = "select Color_Name,Color_img from store.Mst_Color";  // 3521 "
            //    SqlCommand cmd = new SqlCommand(querystring, conn);
            //    SqlDataAdapter da = new SqlDataAdapter(cmd);
            //    da.Fill(ds);
            //    //string text = "<p style='color:red'>Color_Name</p>";

            //    //XDocument xd = XDocument.Parse(text);


            //    //DropDownList1.DataTextField = text;
            //    //DropDownList1.DataValueField = "Color_img";
            //    DropDownList1.DataSource = ds;
            //    DropDownList1.DataBind();
               
              
            //    conn.Close();
            //}

            Label1.Text = GetMACAddress();

        }

        public string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            } return sMacAddress;
        }

         private void downloadAnImage(string strImage)
    {
        Response.ContentType = "image/jpeg";

        Response.AppendHeader("Content-Disposition", "attachment; filename=test.gif");
      
        Response.TransmitFile(strImage);
        Response.End();

    }
        protected void Button1_Click(object sender, EventArgs e)
        {
             string[] fileEntries = Directory.GetFiles(Server.MapPath("~/favicon.ico"));
        foreach (string fileName in fileEntries)
        {
            downloadAnImage(fileName);
       }
    }
         
        
    }
}
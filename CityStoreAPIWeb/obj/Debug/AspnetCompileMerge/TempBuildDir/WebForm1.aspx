<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="CSMerchantWebAPI.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:DropDownList ID="DropDownList1" runat="server" DataTextField="Color_img"> </asp:DropDownList>

          <img id="image" src="Null_Image.png" /> <br />
            <select id="imgList">
                <option value="1.png">Select Option</option>
                <option value="2.png">One 1</option>
                <option value="3.png">Two 2</option>
                <option value="4.png">Three 3</option>
            </select>




            <script>
                function setClass() {
                    var img = document.getElementById("image");
                    img.src = this.value;
                    return false;
                }
                document.getElementById("imgList").onchange = setClass;
            </script>

        <select>
       
            <a style="text-align:center !important"></a>
  <option value="volvo"><IMG src="Images/heroAccent.png"HEIGHT="15" WIDTH="15" BORDER="0"align="center">Volvo</option>
  <option value="saab"><IMG src="b.jpg"HEIGHT="15" WIDTH="15" BORDER="0"align="center">Saab</option>
  <option value="mercedes"><IMG src="c.jpg"HEIGHT="15" WIDTH="15" BORDER="0"align="center">Mercedes</option>
  <option value="audi"><IMG src="d.jpg"HEIGHT="15" WIDTH="15" BORDER="0"align="center">Audi</option>
  </select>

        <img src="Images/orderedList3.png" />


        <asp:label ID="Label1" runat="server" ></asp:label>
       


        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
    </form>
</body>


</html>

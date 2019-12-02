<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="orders.aspx.cs" Inherits="NikolXML.orders" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <h1>Orders</h1>
    <form id="form1" runat="server">
        <div>
            <div>
            <asp:Xml ID="xml1" runat="server"
                DocumentSource="~/orders.xml"
                TransformSource="~/orders.xslt"/>
            </div>
        </div>
    </form>
</body>
</html>

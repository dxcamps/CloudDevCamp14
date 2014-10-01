<%@ Page Language="C#" %>
<% HelloWorldLabel.Text = "<h1>Hello World (in ASP.NET Web Forms!)</h1>"; %>
<html>
    <head runat="server">
        <title>Hello World</title>
    </head>
    <body>
        <form id="form1" runat="server">
            <asp:Label runat="server" id="HelloWorldLabel"></asp:Label>
        </form>
    </body>
</html>

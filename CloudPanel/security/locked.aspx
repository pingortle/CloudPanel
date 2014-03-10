<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="locked.aspx.cs" Inherits="CloudPanel.security.locked" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <link rel="shortcut icon" href="../images/favicon.png" type="image/png" />


    <link href="../css/style.default.css" rel="stylesheet" />

    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
      <script src="../js/html5shiv.js"></script>
      <script src="../js/respond.min.js"></script>
    <![endif]-->
</head>
<body class="notfound" style="overflow: visible">
    <form id="form1" runat="server">

        <section>

            <div class="lockedpanel">
                <div class="locked">
                    <i class="fa fa-lock"></i>
                </div>
                <div class="loginuser">
                    <img src="../images/man-icon.png" alt="" />
                </div>
                <div class="logged">
                    <h4>
                        <asp:Label ID="lbDisplayName" runat="server" Text=""></asp:Label></h4>
                    <small class="text-muted">
                        <asp:Label ID="lbLoginName" runat="server" Text=""></asp:Label></small>
                </div>

                <br />
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="Enter your password" TextMode="Password"></asp:TextBox>
                <br />
                <asp:Button ID="btnLogin" runat="server" Text="Unlock" CssClass="btn btn-success btn-block" />

                <br />
                <br />

                <small class="text-muted">
                    <a href="../login.aspx">Sign in as another user</a>
                </small>
            </div>
            <!-- lockedpanel -->

        </section>


        <script src="../js/jquery-1.10.2.min.js"></script>
        <script src="../js/jquery-migrate-1.2.1.min.js"></script>
        <script src="../js/bootstrap.min.js"></script>
        <script src="../js/modernizr.min.js"></script>
        <script src="../js/retina.min.js"></script>
    </form>
</body>
</html>

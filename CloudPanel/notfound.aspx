<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="notfound.aspx.cs" Inherits="CloudPanel.notfound" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <link rel="shortcut icon" href="images/favicon.png" type="image/png" />

    <title></title>

    <link href="css/style.default.css" rel="stylesheet" />

    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
      <script src="js/html5shiv.js"></script>
      <script src="js/respond.min.js"></script>
    <![endif]-->
</head>
<body class="notfound">
    <form id="form1" runat="server">
        <div>
            <div class="notfoundpanel">
                <h1>404!</h1>
                <h3><%= Resources.Pages.NotFound_H3 %></h3>
                <h4><%= Resources.Pages.NotFound_H4 %></h4>
            </div>
            <!-- notfoundpanel -->

            <script src="js/jquery-1.10.2.min.js"></script>
            <script src="js/jquery-migrate-1.2.1.min.js"></script>
            <script src="js/bootstrap.min.js"></script>
            <script src="js/modernizr.min.js"></script>
            <script src="js/retina.min.js"></script>
            <script src="js/custom.js"></script>
        </div>
    </form>
</body>
</html>

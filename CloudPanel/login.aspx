<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="CloudPanel.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <link rel="shortcut icon" href="images/favicon.png" type="image/png" />

    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
      <script src="js/html5shiv.js"></script>
      <script src="js/respond.min.js"></script>
    <![endif]-->
</head>
<body class="signin">
    <form id="form1" runat="server">

        <section>

            <div class="signinpanel">

                <div class="row">

                    <div class="col-md-7">

                        <div class="signin-info">
                            <div class="logopanel">
                                <h1><span>[</span> CloudPanel <span>]</span></h1>
                            </div>
                            <!-- logopanel -->

                            <div class="mb20"></div>

                            <h5><strong>Welcome</strong></h5>
                            <ul>
                                <li><i class="fa fa-arrow-circle-o-right mr5"></i>Low Cost Exchange Hosting</li>
                                <li><i class="fa fa-arrow-circle-o-right mr5"></i>Free instant messaging with select mailbox packages</li>
                                <li><i class="fa fa-arrow-circle-o-right mr5"></i>More Info..</li>
                                <li><i class="fa fa-arrow-circle-o-right mr5"></i>More Info...</li>
                                <li><i class="fa fa-arrow-circle-o-right mr5"></i>and much more...</li>
                            </ul>
                            <div class="mb20"></div>
                            <strong>Not a member? <a href="#">Sign Up</a></strong>
                        </div>
                        <!-- signin0-info -->

                    </div>
                    <!-- col-sm-7 -->

                    <div class="col-md-5">
                        <div class="cppanel">
                            <h4 class="nomargin"><%= Resources.LocalizedText.Login_Signin %></h4>
                            <p class="mt5 mb20"><%= Resources.LocalizedText.Login_LoginToAccess %></p>

                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control uname" placeholder='<%$ Resources:LocalizedText, Login_Username %>'></asp:TextBox>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control pword" placeholder='<%$ Resources:LocalizedText, Login_Password %>' TextMode="Password"></asp:TextBox>
                            
                            <asp:Button ID="btnLogin" runat="server" Text='<%$ Resources:LocalizedText, Login_Signin %>' CssClass="btn btn-success btn-block" OnClick="btnLogin_Click" />

                            <br />

                            <asp:Label ID="lbLoginMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
                            
                        </div>
                    </div>
                    <!-- col-sm-5 -->

                </div>
                <!-- row -->

            </div>
            <!-- signin -->

        </section>


        <script src="js/jquery-1.10.2.min.js"></script>
        <script src="js/jquery-migrate-1.2.1.min.js"></script>
        <script src="js/bootstrap.min.js"></script>
        <script src="js/modernizr.min.js"></script>
        <script src="js/retina.min.js"></script>
        <script src="js/custom.js"></script>
        <script type="text/javascript">
            var $buoop = {};
            $buoop.ol = window.onload;
            window.onload = function () {
                try { if ($buoop.ol) $buoop.ol(); } catch (e) { }
                var e = document.createElement("script");
                e.setAttribute("type", "text/javascript");
                e.setAttribute("src", "//browser-update.org/update.js");
                document.body.appendChild(e);
            }
        </script>
    </form>
</body>
</html>

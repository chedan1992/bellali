<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<body class="single single-post postid-7639 single-format-standard logged-in" style=" background-color:#fff">
        <div class="content-wrap">
            <div class="content">
                <header class="article-header">
                    <h1 class="article-title">
                        <%=ViewData["Name"]%></h1>
                    <div class="article-meta">
                        <span class="item">发布于：
                            <%=ViewData["CreateTime"]%></span> 
                            <span class="item">来源：
                            管理员</span> 
                           <%-- <span class="item">所属类型：
                             <%=ViewData["ActionTypeName"]%></span> --%>
                          <%--  <span class="item post-views">阅读(<span id="countnum"><%=ViewData["ReadNum"]%></span>)</span>--%>
                    </div>
                </header>
                <article class="article-content">
                      <%--<div class="article-meta">
                        <img src=" <%=ViewData["Img"]%>" style="margin: 0 auto;
                            height:200px; width:400px;" />
                    </div>--%>
                    <%=ViewData["Info"]%>
                </article>
                <%--<nav class="article-nav">
                        <span class="article-nav-prev"></span><span class="article-nav-next">下一篇<br>
                            <a href="/funny/101.html" rel="next">摄影技巧：红外摄影下的绿色变成了雪景</a> </span>
                    </nav>--%>
            </div>
        </div>
    </body>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="UrlContent" runat="server">
    <link href="../../Resource/css/document.css" rel="stylesheet" type="text/css" />
</asp:Content>
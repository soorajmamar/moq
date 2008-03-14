<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.ServiceModel.Syndication" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MockFrameworks.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>Mock Frameworks</title>
	<link href="css/layout.css" rel="stylesheet" type="text/css" />
	<link href="css/styles.css" rel="stylesheet" type="text/css" />

	<script type="text/javascript" language="javascript">
		function ShowHideAdd()
		{
			var panel = document.getElementById("addPanel");
			var link = document.getElementById("addLibrary");
			if (panel.style.display == "block")
			{	
				panel.style.display = "none";
				link.innerHTML = "+ Add new library";
			}
			else
			{
				panel.style.display = "block";
				link.innerHTML = "- Add new library";
			}
		}	
		
		function HumanCheckComplete(isHuman)
		{
			if (isHuman)
			{
				return true;
				//return WebForm_OnSubmit();
				//WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions("btnAdd", "", true, "", "", false, false))
				//var form = document.getElementById("home");
				//form.submit();
			}
			else
			{
				alert("Please correctly identify the cats before submitting.");
				return false;
			}
		}		
	</script>

</head>
<body>
	<form id="home" runat="server">
	<div id="container">
		<div id="container-inner">
			<div id="hdr">
				<h1>
					<span class="textYellow">Mock</span> Frameworks</h1>
			</div>
			<div id="content">
				<div class="column">
					<p>
						This site intends to provide an index with short urls (i.e. <a href="http://www.mockframeworks.com/moq"
							title="http://www.mockframeworks.com/moq">Moq</a>) for all existing mocking frameworks,
						regardless of platform. Feel free to <a href="#addLibrary" onclick="javascript:ShowHideAdd()">add your favourite framework
							or library</a> using the form at the bottom of the page.
					</p>
					<%
						foreach (var category in GetItems())
						{
					%>
					<h2>
						<%= category.Key %></h2>
					<ul>
						<% 
							foreach (var item in category.Value)
							{
						%>
								<li>
									<a href="<%= item.Id %>" title="<%= item.Id %>"><%= item.Title.Text %></a>
									<% if (IsNew(item.Id)) { %>
									<span class="textYellow">(New!)</span>
									<% } %>
									&nbsp;&nbsp;
									<a href="<%= Page.ClientScript.GetPostBackClientHyperlink(this, item.Id) %>" style="color: Gray" title="Delete this entry?">[x]</a>
							
						<% } %>
					</ul>
					<%	}	%>
				</div>
				<p>
					&nbsp;</p>
				<div class="column" style="font-size: smaller;">
					<a id="addLibrary" onclick="ShowHideAdd(); return false;">+ Add new library</a>
					<div style="display:none;" id="addPanel">
						<br />
						<table width="400px">
							<tr>
								<td>
									Name:
								</td>
								<td>
									<asp:TextBox ID="txtName" runat="server"></asp:TextBox>
								&nbsp;<asp:RequiredFieldValidator ID="nameValidator" runat="server" 
										ErrorMessage="Name is required" ControlToValidate="txtName" Display="None"></asp:RequiredFieldValidator>
								</td>
							</tr>
							<tr>
								<td>
									Platform:
								</td>
								<td>
									<asp:TextBox ID="txtPlatform" runat="server"></asp:TextBox>
								&nbsp;<asp:RequiredFieldValidator ID="platformValidator" runat="server" 
										ErrorMessage="Platform is required" ControlToValidate="txtPlatform" Display="None"></asp:RequiredFieldValidator>
								</td>
							</tr>
							<tr>
								<td>
									Short Url:
								</td>
								<td>
									<a href="http://www.mockframeworks.com/">http://www.mockframeworks.com/</a><asp:TextBox ID="txtShortUrl" Width="90" runat="server"></asp:TextBox>
									&nbsp;<asp:RequiredFieldValidator ID="shortUrlValidator" 
										runat="server" ErrorMessage="Short Url is required" ControlToValidate="txtShortUrl" Display="None"></asp:RequiredFieldValidator>
								</td>
							</tr>
							<tr>
								<td>
									Redirect to Url:
								</td>
								<td>
									<asp:TextBox ID="txtRedirectUrl" Width="255" runat="server"></asp:TextBox>
								&nbsp;<asp:RequiredFieldValidator ID="redirectUrlValidator" runat="server" 
										ErrorMessage="Redirect Url is required" ControlToValidate="txtRedirectUrl" Display="None"></asp:RequiredFieldValidator>
										<asp:RegularExpressionValidator ID="redirectUrlValidValidator" runat="server" 
										ValidationExpression="(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?"
										ErrorMessage="Redirect Url is invalid" ControlToValidate="txtRedirectUrl" Display="None" />
								</td>
							</tr>
						</table>
						<asp:ValidationSummary ID="formValidation" runat="server" />
						<br />
						<p><script type="text/javascript" src="http://challenge.asirra.com/js/AsirraClientSide.js"></script></p>
						<asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/img/BTNadd.gif" />
						<a href="#" onclick="javascript:Asirra_CheckIfHuman(HumanCheckComplete)">
							<img src="img/BTNadd.gif" />
						</a>
						<asp:Label CssClass="textRed" Visible="false" ID="validationFailed" runat="server" />
					</div>
				</div>
			</div>
		</div>
		<div id="footer">
			<br />
			Powered by <span class="textYellow">Clarius Consulting</span>&nbsp;| <a href="maito:info@clariusconsulting.net">
				info@clariusconsulting.net</a> | <a href="http://www.clariusconsulting.net">www.clariusconsulting.net</a></div>
	</div>
	</form>
	<script language="javascript" type="text/javascript">
	var __WebForm_OnSubmit = WebForm_OnSubmit;
	function WebForm_OnSubmit() 
	{
	  if (!Asirra_CheckIfHuman(HumanCheckComplete)) return false;
	  
	  return __WebForm_OnSubmit();
	}		
	</script>
</body>
</html>

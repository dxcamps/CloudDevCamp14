<a name="Title" />
# Azure Web Sites Demo #
---

<a name="Overview" />
## Overview ##

In this demo we will cover....

- [Create a website using the new portal](#CreateWS)
- [Deploy Code to an Azure website via FTP](#DeployWS)
- [Enable the Visual Studio Online Extension](#EnableVSO)
- [Enable Java Support](#EnableJava)

---

<a name="Setup" />
## Setup and Prerequisites ##

To successfully complete this demo you need:

- An Azure subscription 
- Visual Studio 2013 Update 2+
- [Azure SDK for Visual Studio 2013](http://azure.microsoft.com/en-us/downloads/)

---

<a name="CreateWS" />
## Create a website using the New Portal ##

We will use the new Azure portal to create a website

1. Log in to the [Azure Portal](https://manage.windowsazure.com/)

2. Click on your email address (top right) and choose "Switch to new portal"

	![Switch to new portal](images/switch-to-new-portal.png?raw=true)

3. Click on the green plus new button and choose Website

	![New Website](images/new-website.png?raw=true)

4. A new blade will open with the details for creating a new website:
	- Enter a URL for the website.  This must be unique across all Azure websites. 
	- Choose a Web Hosting Plan
	- Choose an existing Resource Group or create a new one
	- Select the location 
	- Click the Create button

	![Website Details](images/website-details.png?raw=true)

5. Once the website has been created the website management blade will open.  Click the browse button to open the website in a new tab. 

	![Website Management Blade](images/website-management-blade.png?raw=true)

---
<a name="DeployWS" />
## Deploy Code to an Azure website via FTP ##

1. From the website management blade
	- On the left navigation bar choose Browse, select Websites, select the website you want to manage

2. Click on the Properties button to view FTP information

	![Website Properties Button](images/website-properties-button.png?raw=true)

3. This will open the Properties blade with FTP connection information.  

	![Website Properties](images/website-properties.png?raw=true)

	_**Note**: The FTP/Deployment User is a global deployment user for your Azure subscription.  If you do not know your deployment user you can reset the credentials from the Website Management Blade. [More information on FTP Deployment Credentials](http://azure.microsoft.com/en-us/documentation/articles/web-sites-manage/#ftp-credentials)_

4. Use the FTP client of your choice (Windows Explorer used below) and connect to your Azure website. 
	
	![Windows Explorer FTP](images/windows-explorer-ftp.png?raw=true)

5. Navigate into /site/wwwroot/ folder

6. Upload the files from the /Demo/Begin/Hello Azure/ of this repository into the wwwroot. 

7. Browse to your website again and you should now see the Say 'Hello' to Azure Websites page

	![Hello Azure Page](images/hello-azure-page.png?raw=true)

8. Azure Websites supports multiple development languages as you can see from the code you just deployed: 
	- [Learn more about using .NET with Azure Websites](http://azure.microsoft.com/en-us/documentation/services/websites/#net)
	- [Learn more about using Node.Js with Azure Websites](http://azure.microsoft.com/en-us/documentation/services/websites/#node)
	- [Learn more about using PHP with Azure Websites](http://azure.microsoft.com/en-us/documentation/services/websites/#php)
	- [Learn more about using Python with Azure Websites](http://azure.microsoft.com/en-us/documentation/services/websites/#python)

---
<a name="EnableVSO" />
## Enable the Visual Studio Online Extension ##

1. From the website management blade
	- On the left navigation bar choose Browse, select Websites, select the website you want to manage

2. Click Extensions from the Configuration section
	
	![Site Extensions](images/site-extensions.png?raw=true)

3. This will open then Installed site extensions blade.  Click on the Add button

4. Once the Choose site extension blade opens, select Visual Studio Online

	![VSO Site Extension](images/vso-site-extension.png?raw=true)

5. Press Ok to Accept the Legal Terms

6. Press Ok to finishing adding the site extension

7. Once the Visual Studio Online has been added, it will be selectable from the Installed site extensions blade

8. Select Visual Studio Online and click Browse to launch the Visual Studio Online "Monaco" editor 

	![Launch VSO](images/launch-vso.png?raw=true)

9. Explore the source code for the Say Hello application 

---
<a name="EnableJava" />
## Enable Java Support ##

1. From the website management blade  
	- On the left navigation bar choose Browse, select Websites, select the website you want to manage

2. Click Site Settings from the Configuration section
	
	![Website Site Settings](images/website-site-settings.png?raw=true)

3. This will open the Site Settings blade and select a Java Version, then choose either TomCat or Jetty
	
	![Site Settings Java](images/site-settings-java.png?raw=true)

4. Save the site.  
_**Note:** This will cause your site to restart_

5. Browse to your site and you should a new Java splash page
	
	![Java Splash Page](images/java-splash-page.png?raw=true)

6. Browse to /hello.jsp to see the sample HelloWorld in Java  
_**Note:** Java pages are stored in the /site/wwwroot/webapps/ROOT/ directory_


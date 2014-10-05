<a name="Title" />
# Platform as a Service (PaaS) Demo #
---

<a name="Overview" />
## Overview ##

In this demo we will review a number of the Platform as a Service (PaaS) overings available on Microsoft Azure.  We'll start with a brief look Azure Redis Cache and Azure Web Sites.  We'll then dig deeper into Azure Cloud Services and how they can help you as the developer break your dependency on the physical machine your code runs on. 

---

<a name="Setup" />
## Setup and Prerequisites ##

To successfully complete this demo you need:

- [An active Azure Subscription](http://azure.microsoft.com)
- [Visual Studio 2013](http://visualstudio.com/downloads)
	- Visual Studio Express 2013 with Update 3 for Web, or 
	- A full version of Visual Studio
- [Azure SDK 2.4 or later](http://go.microsoft.com/fwlink/p/?linkid=323510&clcid=0x409)

- Some of the tasks create resources in Azure that take time to provision.  
	
	- If you will be presenting this demo to a live audience it is helpful if you have pre-created resources for use through the demo.  You can then show how to create a new resource, but then switch to using your pre-created resource instead. 

	- If you are walking through this demo as a "Hands-on-Lab" the instructions spell out when you can expect a long delay.  Those are good times to take a break if needed.  

---

<a name="Tasks" />
## Tasks ##

This demo is comprised of the following tasks:

- [Review the Fireworks Sample](#Task1)
- [Create Azure Redis Cache](#Task2)
- [Publish to Azure Web Site](#Task3) 
- [Publish to Cloud Service](#Task4)
- [Clean Up](#Task5)

--- 

<a name="Task1" />
## Review the Fireworks Web Site ##

The Fireworks sample web application is part of the **["Windows Azure Samples"](https://github.com/WindowsAzure-Samples)** .  There really isn't much code in the app, yet it creates an impressive interactive multi-user fireworks display using [SignalR](http://signalr.net/) and a [Redis Cache](http://azure.microsoft.com/en-us/services/cache/) up in Azure.  

![01000-FireworksArchitecture](images/01000-fireworksarchitecture.png?raw=true "FireworksArchitecture")

The Website can run on multiple instances.  It uses SignalR to communicate with the clients connected to it.  If there are multiple website insances, each with their own clients, the instances communicate with each other using Redis Cache as a "backplane". This means that if a user in one client causes and action (launching a firework in this case), the action can be communicated through the Redis Cache backplane to all the other website instances, and then in turn to each client connected to those instances.  

1. Open the **Begin\Fireworks\Fireworks.sln** file in Visual Studio.  Then in the Solution Explorer, review the contents of the Web Application project: 

	![01010-FireworkSolution](images/01010-fireworksolution.png?raw=true "Firework Solution")

1. There are really only a few key files in the solution (Hightlighted in the screenshot above, working our way from the bottom up):

	> **Note:** The following code is displayed for review purposes only.  **YOU DO NOT NEED TO DO ANY CODE MODIFICATIONS! (YET)**

	- **Startup.cs** - Sets the SignalR [GlobalHost.DependencyResolver](http://msdn.microsoft.com/en-us/library/microsoft.aspnet.signalr.globalhost.dependencyresolver%28v=vs.118%29.aspx) to use Redis ([learn more](http://www.asp.net/signalr/overview/performance/scaleout-with-redis)) as a backplane. We'll update this code in just a bit to point to a Redis Cache instance we create in Azure.

	<!-- mark:3 -->
	````C#
	public void Configuration(IAppBuilder app)
	{
		 GlobalHost.DependencyResolver.UseRedis("[Redis cache cluster]", 6379, "[access key]", "Fireworks");
		 app.MapSignalR();
	}
	````

	- **FireworkHub.cs** - Defines the [SignalR Hub](http://msdn.microsoft.com/en-us/library/microsoft.aspnet.signalr.hub%28v=vs.118%29.aspx) for the Firework website.  It implements the `Add(int type, double x, double y, string color, int tail)` Method which in turns invokes the `addFirework` method on each client. The `addFirework` method is told what kind of firework to draw (simple or complex), where to draw it (x,y), what color it should be, and what kind of tail the sparks have).  Each client then implements an addFirework handler to draw the firework as specified.  Cool!

	```C#
	public void Add(int type, double x, double y, string color, int tail)
	{
		 Clients.All.addFirework(
			  new
			  {
					Type = type,
					X = x,
					Y = y,
					Color = color,
					TailType = tail
			  }
			  );
	}
	```

	- **Views\Home\Index.cshtml** - Is the only view for the application.  It creates the User Interface and implements the logic for creating and visualizing fireworks on a canvas.  It ties into SignalR to send and receive firework instances to other clients, and draw them on the screen.

	````JavaScript

	//...Classes for the Fireworks, and methods to draw them on the HTML5 canvas are in here as well!

	//...The following code isn't shown in the same order it appears in the file, it has been re-organized to support the discussion about it. 


	//Wire up an event handler so that when the user clicks on the canvas, we send the firework information to all the other connected clients.
	canvas.addEventListener('click', sendFirework, false);

	//...

	//This creates the firework instance and sends details to the hub
	function sendFirework(evt)
	{
		 //User clicks on canvas - request user to broadcast the event
		 if (!enableUI)
			  return;
		 var firework;
		 if (preType == 1)
			  firework = new SimpleFirework(evt.clientX - rect.left + document.documentElement.scrollLeft,
															evt.clientY - rect.top + document.documentElement.scrollTop, $('#c_' + preColor).data('value'), preTrail);
		 else
			  firework = new ComplexFirework(evt.clientX - rect.left,
													  evt.clientY - rect.top, $('#c_' + preColor).data('value'));
			  
		 hub.server.add(preType, firework.BaseX, firework.BaseY, $('#c_' + preColor).data('value'), preTrail);
	}

	//...

	//This method is invoked whenever a firework is added by another client.  The current client then adds that firework to the collection.
	hub.client.addFirework = function (firework) {
		 //Server broadcasted the event. Respond by adding the firework to local state.
		 if (firework.Type == 1)
			  fireworks.push(new SimpleFirework(firework.X, firework.Y, firework.Color, firework.TailType));
		 else
			  fireworks.push(new ComplexFirework(firework.X, firework.Y, firework.Color, firework.TailType));
	};

	//...
	````

1. Before you can actually run the sample web site though, there are a few things we need to fix.  First, you need to restore the NuGet Packages.  

1. From the Visual Studio menu bar, select **"Tools"** | **"NuGet Package Manager"** | **"Package Manager Console"**, then in the **"Package Manager Console"** window, click the **"Restore"** button to restore the missing NuGet packages. 

	![01020-RestoreNuGetPackages](images/01020-restorenugetpackages.png?raw=true "Restore NuGet Packages")

1. We still aren't ready to run it yet.  Continue with the next task

--- 

<a name="Task2" />
## Create Azure Redis Cache ##


In this task we'll create an Azure Redis Cache instance for SignalR to use as it's backplane.  Basically, this just means that SignalR can use the RedisCache to share client information across a collection of website front ends.  It helps keep client connections in sync when there are multiple servers hosting a web site.  

The [Azure Redis Cache](http://msdn.microsoft.com/en-us/library/azure/dn690523.aspx) service is a "Platform as a Service" implementation of Redis.  That means that you don't need to go stand up servers to install redis on.  Azure takes care of that for you and gives you a fully compatible Redis cache to use in the cloud!  Cool! 

1. Open the [**NEW** Azure Portal](https://portal.azure.com) (https://portal.azure.com) (Redis Cache support is only available in the new portal) and login with the Microsoft Account associated with the subscription you want to use. 

1. Click the **+ NEW** button in the lower left corner, and select **"Redis Cache"** from the list of options:

	![02010-RedisCache](images/02010-rediscache.png?raw=true "Redis Cache")

1. In the **"New Redis Cache"** blade, enter a name for your new cache:

	> **Note:** Redis Cache names must be only numbers or lower case characters, can't start with a number, or contain any other symbols (including spaces)

	> **Note:** You are only specifying the DNS host name, the fully qualified DNS name will be **&lt;cachename&gt;.redis.cache.windows.net**

	![02020-RedisCacheName](images/02020-rediscachename.png?raw=true "Redis Cache Name")

1. The pricing tier defaults to a Standard 1GB cache. We don't need that much cache for this demo.  Click on **"Pricing Tier", then in the **"Recommended pricing tiers"** blade, click **"BROWSE ALL PRICING TIERS"**.

	![02030-BrowsePricingTiers](images/02030-browsepricingtiers.png?raw=true "Browse Pricing Tiers")

1. In the **"Choose your pricing tier"** blade, scroll down to find the **"Basic 250MB"** option, and click on it to select it, then click the **"Select"** button:

	![02040-BasicCache](images/02040-basiccache.png?raw=true "Basic Cache")

1. Back in the **"Recommended pricing tiers"** blade, again click the **"Select"** button to confirm the change:

	![02050-ConfirmBasicCache](images/02050-confirmbasiccache.png?raw=true "Confirm Basic Cache")

1. On the **"New Redis Cache"** blade, click the **"RESOURCE GROUP"** option, then in the **"Resource group"** blade, click **"Create a new resource group"**.  In the **"Create resource group"** blade, enter a name (no spaces) for your Resource Group and click **"OK"** to create the new group.  

	![02060-NewResourceGroup](images/02060-newresourcegroup.png?raw=true "New Resource Group")

1. If you have multiple subscriptions, you will  be prompted to select the subscription you want to use.  On the **"New Redis Cache" blade, Click the **"SUBSCRIPTION"** option, and then in the **"Subscription"** blade, select the subscription you want to use, then click the **"x"** button on the **"Subscription"** blade to close it. 

	![02070-ChooseSubscription](images/02070-choosesubscription.png?raw=true "Choose Subscription")

1. Next, we need to pick the location for the cache.  On the **"New Redis Cache"** blade, click the **"LOCATION"** option, and select the desired location from the **"Location"** blade.

	![02080-CacheLocation](images/02080-cachelocation.png?raw=true "Cache Location")

1. With all the options set, you can now create the cache.  Leave the **"Add to Startboard"** checkbox checked (this will add a tile for the cache to the portal start board), and click the **"Create"** button.

	![02090-CreateCache](images/02090-createcache.png?raw=true "Create Cache")

1. **It will take some time to create the cache, perhaps 10-20 minutes**.  You can monitor the status on either the **Startboard**, or on the **Notifications** tab:

	![02100-ProvisioningCache](images/02100-provisioningcache.png?raw=true "Provisioning Cache")

1. Once the cache has been provisioned, you should see it on the Startboard, and blade with the cache properties should open by default (if not, just click on the tile for the cache on the Startboard):

	![02110-CacheTileAndBlade](images/02110-cachetileandblade.png?raw=true "Cache Tile and Blade")

1. We need to collect a few details from our new cache so we can update the **"Startup.cs"** code in our project.  First, Click on the **"Properties"** button to retrieve the host name for the cache:

	![02120-CacheHostName](images/02120-cachehostname.png?raw=true "Cache Host Name")

1. Next, click on the **"Keys"** button and in the **"Manage Keys"** blade, retrieve the **"PRIMARY"** key

	![02130-CacheAccessKey](images/02130-cacheaccesskey.png?raw=true "Cache Access Key")

1. Use the host name and access key values from above to update the code in the **"Startup.cs"** file:

	![02140-StartupCsCodeToReplace](images/02140-startupcscodetoreplace.png?raw=true "Startup.cs Code to Replace")

1. For example:

	![02150-RepalcedCode](images/02150-repalcedcode.png?raw=true "ReplacedCode")

1. Finally, you are ready to build the sample, and run it locally.  You should see the following UI in the browser:

	> **Note:** try clicking in the sky above the skyline to set off fireworks!

	> **Note:** You can also open a second copy of your browser and navigate to the same URL to see SignalR in action. Notice that clicking on either browser instances canvas causes fireworks in both browsers!  Cool!

	![02160-SampleRunningLocally](images/02160-samplerunninglocally.png?raw=true "Sample Running Locally")

1. Close the browsers to stop debugging when you are done testing the sample web application locally.

--- 

<a name="Task3" />
## Publish to Azure Web Site ##

### If you don't want to publish to a website, you can skip this task and go directly to Task 4, **["Publish to Cloud Service"](#Task4)**

So far we have an ASP.NET Web application that runs locally, but uses an Azure Redis Cache in the cloud.  The only clients that can connect to the web site right now though are browser sessions running on the development box.  In this task we'll look at using a simple, but powerful PaaS solution in Azure called "Websites"

1. Open the new Azure Portal in the browser (https://portal.azure.com) and login.  

1. Click the **"+ NEW"** button in the lower left corner, and select **"Website"** from the list of options:

	![03010-Website](images/03010-website.png?raw=true "Website")

1. In the **"Website"** blade, enter a valid and unique hostname for your website.  Note that the fully qualified domain name will be **&lt;hostname&gt;.azurewebsites.net** 

	![03020-WebsiteName](images/03020-websitename.png?raw=true "Website Name")

1. Next, click on the **"WEB HOSTING PLAN"** option, and in the **"Web hosting plan"** blade select an existing Free hosting plan (no need to incur charges for this demo), then click **"OK"**:

	> **Note:** the hosting plan can be used to manage your costs, and to group sites together into the same plan.  Learn more about hosting plans here: http://azure.microsoft.com/en-us/documentation/articles/web-sites-web-hosting-plan-overview/.

	![03030-FreeHostingPlan](images/03030-freehostingplan.png?raw=true "Free Hosting Plan")

1. If you do not have a free hosting plan already, you can create one:

	- In the **"Web hosting plan"** blade, scroll down and click on **"Browse all pricing tiers"**:

	- In the **"Choose your pricing tier"** blade, select the **"F1 Free"** pricing tier, then click **"Select**"

	- Then give your new hosting plan a name, and click **"OK"**:

		![03040-BrowseAllPricingTiers](images/03040-browseallpricingtiers.png?raw=true "Browse All Pricing Tiers")

		![03050-FreePricingTier](images/03050-freepricingtier.png?raw=true "Free Pricing Tier")

		![03060-HostingPlanName](images/03060-hostingplanname.png?raw=true "Hosting Plan Name")

1. Back on the **"Website"** blade, click the **"RESOURCE GROUP"** option, and scroll down to select the Resource Group you created earlier when provisioning the Redis Cache:

	![03070-Select Resource Group](images/03070-select-resource-group.png?raw=true "Select Resource Group")

1. If the **"Website"** blade has a **"Subscription"** option, it should be locked, because the Resource Group you just selected belongs only to a single subscription:

	![03080-SubscriptionLocked](images/03080-subscriptionlocked.png?raw=true "Subscription Locked")

1. Next, click on the **"LOCATION"** option, and select the data center where you want the website to run:

	![03090-Location](images/03090-location.png?raw=true "Location")

1. Finally, at the bottom of the **"Website"** blade, ensure that **"Add to Startboard"** is **CHECKED**, then click **"Create"**

	![03100-CreateWebsite](images/03100-createwebsite.png?raw=true "Create Website")

1. It may take a few minutes to provision the website.  You can monitor the provisioning of your new website back on the startboard:

	![03110-WebsiteProvisioning](images/03110-websiteprovisioning.png?raw=true "Website Provisioning")

1. Once the website is created, you should see a tile for it on the Startboard:

	![03120-WebsiteOnStartboard](images/03120-websiteonstartboard.png?raw=true "Website on Startboard")

1. If you click on the tile for the website, a the website's blade will open.  From there you can perform various monitoring and configuration actions.  Take a moment to investigate the website blade options, then close it when you are done:

	![03130-WebsiteBlade](images/03130-websiteblade.png?raw=true "Website Blad")

1. Back in Visual Studio, in the **"Solution Explorer"**, right-click on the **"Firework"** project, and select **"Publish"** from the pop-up menu:

	![03140-PublishWebsite](images/03140-publishwebsite.png?raw=true "Publish Website")

1. In the **"Publish Web"** window, select **"Microsoft Azure Websites"**:

	![03150-PublishWeb](images/03150-publishweb.png?raw=true "Publish Web")

1. In the **"Select Existing Website"** window, if you aren't already signed into your Azure subscription, click the **"Sign In"** button:

	![03160-SignIn](images/03160-signin.png?raw=true "Sign In")

1. And enter your credentials to login into your Microsoft Azure subscription:

	![03170-AzureCredentials](images/03170-azurecredentials.png?raw=true "Azure Credentials")

1. Then, from the **"Existing Websites"** drop down, select the website we just created in the portal:

	![03180-SelectWebsite](images/03180-selectwebsite.png?raw=true "Select Website")

1. Then click **"OK"** to confirm the selection:

	![03190-ConfirmWebsite](images/03190-confirmwebsite.png?raw=true "Confirm Website")

1. In the **"Publish Web"** window, review the options, **BUT DON'T CHANGE ANYTHING**.  Click **"Publish"** when you are ready to publish the website:

	![03200-Publish](images/03200-publish.png?raw=true "Publish")

1. The site should publish within just a few seconds, when it's done, the Visual Studio **"Output"** window should show you the status, and the URL for the live site:

	![03210-PublishStatus](images/03210-publishstatus.png?raw=true "Publish Status")

1. Finally, the browser should open to the site.  You can open an additional browser to see SignalR in action.  As you click on the Night Skyline canvas in one browser, you should see fireworks in all connected clients.  Close the browsers when you are done:

	![03220-SiteInBrowsers](images/03220-siteinbrowsers.png?raw=true "Site in Browsers")

---

<a name="Task4" />
## Publish to Cloud Service ##

So far, we have seen two different Platform as a Service (PaaS) offerings in Azure: [Redis Cache](http://azure.microsoft.com/en-us/services/cache/) and [Websites](http://azure.microsoft.com/en-us/documentation/services/websites/).  There are other's including [Mobile Services](http://azure.microsoft.com/en-us/documentation/services/mobile-services/), [SQL Database](http://azure.microsoft.com/en-us/documentation/services/sql-database/), and more.  

The original PaaS offering on Azure however was **["Cloud Services"](http://azure.microsoft.com/en-us/documentation/services/cloud-services/)** with on or more **"Web Role"** or **"Worker Role"** instances. 

As Websites become more and more powerful and configurable, the reason to choose Cloud Services over Websites is becoming less and less clear, however, for the time being the major benefits of a Cloud Service over a Web Site include:

- With Cloud Services you have administrative access to the VMs running your application.  This gives you the ability to perform configuration tasks (like installing custom 3rd party software) that isn't currently possible with Websites. 

- Cloud services offer both **
Web Roles"** and **"Worker Roles"** each with extensive configurability making it easier to create multi-tier applications.

- You can use Remote Desktop to remotely monitor the VMs running your Cloud Service applications. 

Similarly, Cloud Services have some benefits over Virtual Machines:

- You aren't responsible for configuring the base operating system, service packs, hotifxes, etc. 

- Because any custom configuration is specified either in the cloud services configuration files (*.csdef and *.cscfg), or custom startup scripts, it can be applied to as many instances of the role as you desire.  This makes scalability simple.  

- Web Role and Worker Role VM instances are **"Stateless"**. As long as you store all persistent data OFF of the instance (in an Azure SQL Databse, Azure Storage, etc), there is nothing to loose if the VM instance becomes corrupt.  The Azure Fabric controller can simply spin up a new instance and re-deploy your application to it.  

In this last task, well add a cloud Project to the Firework solution in Visual Studio, and configure the Firework website as a "Web Role".  Finally, we'll publish that Cloud Service into Azure. 

1. You will continue with the project you used in the previous tasks because the Redis Cache configuration performed earlier will be required here as well.  So you must have at least completed the **["Create Azure Redis Cache"](#Task2)** task, Ensure that the Firework Solution is open in Visual Studio.

1. In the **"Solution Explorer"** window, right click on the **"Firework"** Solution (not the project) and select **"Add"** | **"New Project..."** from the pop-up menu:

	![04010-AddNewProject](images/04010-addnewproject.png?raw=true "AddNewProject")

1. In the **"Add New Project"** window, in the list of **"Installed"** Templates, select **"Visual C#"** | **"Cloud"** | **"Azure Cloud Service"**.  Give the new cloud service project a **"Name"** and click **"OK"** to create the project.

	![04020-AddNewCloudProject](images/04020-addnewcloudproject.png?raw=true "AddNewCloudProject")

1. In the **"New Microsoft Azure Cloud Service"** window, review the list of different roles, but **"DON'T ADD ANY TO THE PROJECT"**.  Click **"OK"** to create the cloud service:

	![04030-NewMicrosoftAzureCloudService](images/04030-newmicrosoftazurecloudservice.png?raw=true "New Microsoft Azure Cloud Service")

1. In the Visual Studio **"Solution Explorer"** review the contents of the new Cloud Service project:

	- **"Roles"** shows you the various roles in the cloud service.  In this case, we don't have any roles yet.

	- **"ServiceConfiguration.Cloud.cscfg"** and **"ServiceConfiguration.Local.cscfg"** store the configuration values for the cloud service.  The **"Cloud"** file contains settings that will be used when you deploy to the cloud, the **"Local"** file uses settings that will be used when running locally in the debugger.  You can create additional configuration files for different scenarios if needed. Because we haven't added any roles to the cloud service yet, the Service Configuration is empty.
	
	- **"ServiceDefinition.csdef"** contains the Service Definition.  It is used by the Azure Fabric Controller in the cloud to determine what kind of role VMs you need, and how many instances of each. It also describes the various endpoints that need to be configured for communication with and between those instances.  Because we haven't added any Roles to the cloud service yet, the Service Definition is empty:

	![04040-CloudServiceProject](images/04040-cloudserviceproject.png?raw=true "Cloud Service Project")

1. Right click on the **"Roles"** node, and select 

	![04050-AddWebRole](images/04050-addwebrole.png?raw=true "Add Web Role")

1. In the **"Associate with Role Project"** window, select the project (there should only be one) for the Firework Website in the same solution, and click **"OK"**:

	![04060-SelectWebProject](images/04060-selectwebproject.png?raw=true "Select Web Project")

1. In the **"Solution Explorer"**, right-click on the new **"Firework"** web role, and select **"Properties"** from the pop-up menu:

	![04070-OpenRoleProperties](images/04070-openroleproperties.png?raw=true "Open Role Properites")
	
1. On the **"Configuration"** tab, review the configuration of the Role.  For instance, notice that by default it will run in a Single instance, using the Small VM size:

	![04080-RoleConfiguration](images/04080-roleconfiguration.png?raw=true "Role Configuration")

1. On the **"Settings"** tab, note that there is a single default setting that points to the Diagnostics storage.  You could add other settings here if you needed to supply values to the role after is was deployed:

	![04090-RoleSettings](images/04090-rolesettings.png?raw=true "Role Settings")

1. On the **"Endpoints"** tab, notice there is a single endpoint that allows http traffic on port 80 to be directed into the role instance.  If you wanted to enable https for example, you wouuld add a second endpoint for port 443:

	![04100-RoleEndpoints](images/04100-roleendpoints.png?raw=true "Role Endpoints")

1. Review the other properties as you wish, but you don't need to make any changes.  Close the *"Firework [Role]"** properties page when you are done.

1. Open the **"ServiceDefinition.csdef"** (**DON'T MAKE ANY CHANGES TO IT**) and note the contents.  You can see the **"definition"** of the Web Role, and it's single endpoint:

	````xml
	<?xml version="1.0" encoding="utf-8"?>
	<ServiceDefinition 
	  name="FireworkCloudServiceBSS" 
	  xmlns="http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition" 
	  schemaVersion="2014-06.2.4">
	  <WebRole name="Firework" vmsize="Small">
		 <Sites>
			<Site name="Web">
			  <Bindings>
				 <Binding name="Endpoint1" endpointName="Endpoint1" />
			  </Bindings>
			</Site>
		 </Sites>
		 <Endpoints>
			<InputEndpoint name="Endpoint1" protocol="http" port="80" />
		 </Endpoints>
		 <Imports>
			<Import moduleName="Diagnostics" />
		 </Imports>
	  </WebRole>
	</ServiceDefinition>
	````
1. Open the **"ServiceConfiguration.Cloud.cscfg"** file and review it's contents.  **AGAIN, DON'T MAKE ANY CHANGES**:

	````XML
	<?xml version="1.0" encoding="utf-8"?>
	<ServiceConfiguration 
	  serviceName="FireworkCloudServiceBSS" 
	  xmlns="http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceConfiguration" 
	  osFamily="4" 
	  osVersion="*" 
	  schemaVersion="2014-06.2.4">
	  <Role name="Firework">
		 <Instances count="1" />
		 <ConfigurationSettings>
			<Setting 
			  name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" 
			  value="UseDevelopmentStorage=true" />
		 </ConfigurationSettings>
	  </Role>
	</ServiceConfiguration>
	````

1. You can see from above that the GUI based **"Properties"** page is really just a managed way to edit the **"ServiceDefinition.csdef"** and **"ServiceConfiguration.&lowast;.cscfg"** files.  You could edit the XML directly, but you run the risk of making mistakes.  The Role **"Properities"** page helps to ensure to only valid values are used, and that the XML is typed correctly.

1. Next, right-click on the Cloud Service project you created, and select **"Properties"** from the pop-up menu:

	![04110-OpenCloudServiceProperties](images/04110-opencloudserviceproperties.png?raw=true "Open Cloud Service Properites")

1. Switch to the **"Development"** tab, and review the options.  You can click on the name of each to get a definition of the property. **DON'T MAKE ANY CHANGES**:

	![04120-DevelopmentProperties](images/04120-developmentproperties.png?raw=true "Development Properties")

1. On the **"Web"** tab notice that Visual Studio will host the Web Site in **"IIS Express"** by default, and that an emulator that mimics the **"Azure Fabric"** will run.  By default the **"Emulator Express"** is used.  The **"Emulator Express" supports multiple roles (web role and worker role), but only a single instance of each.  If you want to test with multiple role instances of any role you can switch to the **"Full Emulator"**. , **DON'T MAKE ANY CHANGES**:

	![04130-WebProperties](images/04130-webproperties.png?raw=true "Web Properties")

1. Close the properties page when you are done looking. 

1. By default, when you added the Cloud Service project to the solution, the new project should have been set as the **"Startup Project"**, you can tell that is the case if the project name in the **"Solution Explorer"** is bold.  If it isn't, you can right click on the project, and select **"Set as StartUp Project"** from the pop-up menu:

	![04140-StartupProject](images/04140-startupproject.png?raw=true "Startup Project")

1. On the Visual Studio Tool Bar, hit the **"Internet Explorer"** button to start a debug session in IE.  You should see a couple of windows appear (if this is the first time you have debugged a Cloud Service during this session) that indicate that the **"Storage Emulator"** and **"Compute Emulator"** are being started. 

	![04150-StartDebug](images/04150-startdebug.png?raw=true "Start Debug Session")

1. Once the emulators have started, the Cloud Service will be deployed, and the browser window should open with the site loaded.  In the System Tray, if you click on the arrow to **"Show hidden icons"**, you should see a new icon that looks like the Windows logo.  That icon represents the Azure emulators.  If you right click on the Icon, you can both view, or shutdown the emulators.  Select **"Show Compute Emulator UI"**

	![04160-ShowComputeEmulatorUI](images/04160-showcomputeemulatorui.png?raw=true "Show Compute Emulator UI")

1. Remember, that by default the **"Emulator Express"** will be used that only allows a single instance of each role.  However, you can still view the status of that instance in the **"Compute Emulator UI"**.  In the **"Microsoft Azure Compute Emulator (Express)"** Window, expand all the nodes to find the single instance of the Web Role, and view it's trace output.  Close the Emulator UI window when you are done.

	![04170-ComputeEmulatorUI](images/04170-computeemulatorui.png?raw=true "Compute Emulator UI")

1. As before you can test the website in one or more browser instances.  Close the browser instances when you are done testing, and the compute emulator instances will be shutdown, and the debug session will end in Visual Studio:

	![04180-SiteInBrowsers](images/04180-siteinbrowsers.png?raw=true "Site in Browsers")

1. Now that we know it works as a Web Role, let's actually deploy it to the cloud.  You can either use a pre-created Cloud Service that you wish to deploy to, or Visual Studio's Azure Tools can help you create a Cloud Service on the fly.  We'll use that approach in this demo.  Right click on the Cloud Service project and select **"Publish..."** from the pop-up menu:

	![04190-PublishMenuItem](images/04190-publishmenuitem.png?raw=true "Publish Menu Item")

1. In the **"Publish Azure Application"** window, if needed, click the **"Sign In"** button and enter the credentials for your Azure Subscription:

	![04196-SignIn](images/04196-signin.png?raw=true "Sign In if Needed")

1. In the **"Publish Azure Application"** window, select your desired subscription (if you have more than one) from the *""Choose your subscription:"** dropdown, and click **"Next"** to continue: 

	![04200-PublishSignIn](images/04200-publishsignin.png?raw=true "Microsoft Azure Publish Sign In")

1. If you already have other Cloud Services in your subscription, create a new one by clicking on the **Cloud Service"** drop down on the **"Microsoft Azure Publish Settings"** page, and selecting **"&lt;Create New...&gt;**.  If you don't already have any cloud services, this option is implied:

	![042005-CreateNewCloudService](images/042005-createnewcloudservice.png?raw=true "Create New Cloud Service")

1. In the **"Create Cloud Service and Storage Account"** window, enter the requested values, and click **"Create"**:

	- **Name:** A valid hostname for your cloud service.  The fully qualified domain name will take the form of **&lt;hostname&gt;.cloudapp.net**

	- **Region or Affinity Group:** Pick the data center where you want the Cloud Service to run.  

	- **Replication:** Leave it at the default value

	![04210-CreateCloudService](images/04210-createcloudservice.png?raw=true "Create Cloud Service")

1. Review, but don't change the settings:

	- **Environment**: Defaults to **Production**.  The other choice is **Staging**.  Staging deployments allow you to verify that a deployment is production ready before making it live.  You can then **"Swap"** between the production and staging slots to make the staged slot go live.  If there is a problem you can always swap back to the original deployment.

	- **Build configuration:** This tells Visual Studio what build configuration (Release, Debug, etc) to use.

	- **Service configuration:** indicates which **ServiceConfiguration.&lowast;.cscfg** file to use.

	![04215-CommonSettings](images/04215-commonsettings.png?raw=true "Common Settings")

1. Next, turn on the Checkbox for **"Enable Remote Desktop for all roles"**.  Then in the **"Remote Desktop Configuration"** window, enter the username and password you want to use to connect to the roles using Remote Desktop then click **"OK"**:

	> **Note:** The Remote Desktop credentials are NEW credentials.  They don't have to match any existing account.  However, you do want to make sure to remember what you use here so that you can use them later. 

	> **Note:** Remote Desktop access is an example of a benefit that Cloud Services have over Web Sites.  You do need to remember though that Role instances are stateless, and that any changes you make will affect only that instance, and only until it is reset.  Remote Desktop sessions should be used more for diagnostic reasons than for configuration.

	![04220-RemoteDesktopConfiguration](images/04220-remotedesktopconfiguration.png?raw=true "Remote Desktop Configuration")

1. Finally, click **"Publish"** to start the publication process:

	![04230-Publish](images/04230-publish.png?raw=true "Publish")

1. The actual publication process can take some time (ten minutes or more) to complete.  You can monitor the progress in Visual Studio's **"Microsoft Azure Activity Log"** window:

	![04240-MicrosoftAzureActivityLog](images/04240-microsoftazureactivitylog.png?raw=true "Microsoft Azure Activity Log")

1. While the Cloud Service is deploying, you can also review it's configuration in the portal.  At the time this is being written, the new Azure portal does NOT have support for managing Cloud Services.  You still need to do that in the old portal.  Login to your subscription on the old portal at https://manage.windowsazure.com, then click on **"Cloud Services"** to see the list of Cloud Services in your subscription. Finally, click on the name of your new cloud service to see it's details:

	![04250-CloudServiceInPortal](images/04250-cloudserviceinportal.png?raw=true "Cloud Service in Portal")

1. Try switching between the different pages of the Cloud Services' configuration in the portal.  For example, on the **"Instances"** page, you can see the status of all role instances in your Cloud Service.  You can close the portal when you are done:

	![04260-RoleInstances](images/04260-roleinstances.png?raw=true "Role Instances")

1. In Visual Studio's **"Microsoft Azure Activity Log"** window, wait until the status shows **"Completed"**.  You can then click on the URL for your cloud service to open it in the browser:

	![04270-Completed](images/04270-completed.png?raw=true "Completed")

1. Verify that the site loads correctly in the browser, then close the browser window when you are done:

	![04280-SiteInBrowser](images/04280-siteinbrowser.png?raw=true "Site In Browser")

1. Back in Visual Studio, from the menu bar, select **"View"** | **"Server Explorer"**, and then under the **"Azure"** node expand your Cloud Service all the way down to the individual instance of the web role.  Right click on the instance, and select **"Connect using Remote Desktop..."** from the pop-up menu:

	![04290-OpenRemoteDesktop](images/04290-openremotedesktop.png?raw=true "Open Remote Desktop")

1. Follow the prompts to connect, and enter the remote desktop credentials you specified earlier when enabling remote desktop:

	![04300-ConfirmPublisher](images/04300-confirmpublisher.png?raw=true "Confirm Publisher")

	![04310-RemoteDesktopCredentials](images/04310-remotedesktopcredentials.png?raw=true "Remote Desktop Credentials")

	![04320-ConfirmCertificate](images/04320-confirmcertificate.png?raw=true "Confirm Certificate")

1. Once you are connected you can do pretty much anything you like in the session.  For example, in the screenshot below, we've opened Internet Information Services Management to view the website in the role:

	![04330-IISManager](images/04330-iismanager.png?raw=true "IIS Manager")

1. When you are done, close the remote desktop session.

---

<a name="Task5" />
## Clean Up ##

When you are done with this demo, you may have some resources in your subscription that are incurring charges.  If you don't want those charges to continue, you should delete the resources as soon as possible. 

1. To clean up the Cloud Service we created in Task 4:

	- Open the old management portal (https://manage.windowsazure.com)
	- Select **Cloud Services**, then clik on the row (not the name) for your Cloud Service
	- Click the **"Delete"** button along the bottom:
	- Choose the **"Delete the cloud service and its deployments"** option
	- Click **"Yes"** to confirm the deletion

	![05010-DeleteCloudService](images/05010-deletecloudservice.png?raw=true "Delete Cloud Service")

	![05020-ConfirmDeletion](images/05020-confirmdeletion.png?raw=true "Confirm Deletion")

1. Deleting a set of related resources is easier in the New Portal if you used **"Resource Groups"**.  Login to the new portal (https://portal.azure.com).  Then, click **"Browse"** | **"Resource Groups"** 

	![05030-BrowseResourceGroups](images/05030-browseresourcegroups.png?raw=true "Browse Resource Groups")

1. Then in the **"Resource groups"** blade, select the Resource Group you created at the beginning of this demo when you provisioned the Redis Cache.

	![05040-SelectResourceGroup](images/05040-selectresourcegroup.png?raw=true "Select Resource Group")

1. Click the **"Delete"** button at the top of the blade for your resource group:

	![05050-DeleteResourceGroup](images/05050-deleteresourcegroup.png?raw=true "Delete Resource Group")


1. Finally, on the **"Are you sure..."** blade, enter the name of the resource group in the box at the top to confirm the deletion.  Then review the list of resources that will be affected, and click the **"Delete"** button:

	![05060-ConfirmDeletion](images/05060-confirmdeletion.png?raw=true "Confirm Deletion")

1. You can monitor the status of the Resource Group deletion on the **"Notifications"** tab:

	![05070-MonitorDeletion](images/05070-monitordeletion.png?raw=true "Monitor Deletion")

	> **Note:** Currently, deleting a resource or resource group does NOT remove any pinned tiles for the afftected resource from the Startboard.  You will need to manually unpin the tiles for the deleted resources. 

---

<a name="Summary" />
## Summary ##

In this demo we took a look at a number of Azure PaaS offerings including: 

- [Review the Fireworks Sample](#Task1)
- [Create Azure Redis Cache](#Task2)
- [Publish to Azure Web Site](#Task3) 
- [Publish to Cloud Service](#Task4)

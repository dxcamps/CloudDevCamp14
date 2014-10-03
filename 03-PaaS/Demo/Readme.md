<a name="Title" />
# Platform as a Service (PaaS) Demo #
---

<a name="Overview" />
## Overview ##

In this demo we will review a number of the PaaS overings available on Microsoft Azure.  We'll start with a brief look at Azure Web Sites, but those are covered in more depth in a lter demo.  We'll then dig deeper into Azure Cloud Services and how they can help you as the developer break your dependency on the physical machine your code runs on. 

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

--- 

<a name="Task1" />
## Review the Fireworks Web Site ##

The Fireworks sample web application is part of the **["Windows Azure Samples"](https://github.com/WindowsAzure-Samples)** .  There really isn't much code in the app, yet it creates an impressive interactive multi-user fireworks display using SignalR and a Redis Cache up in Azure.  

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

1. It will take some time to create the cache.  You can monitor the status on either the **Startboard**, or on the **Notifications** tab:

	![02100-ProvisioningCache](images/02100-provisioningcache.png?raw=true "Provisioning Cache")

--- 

<a name="Task3" />
## Task 3 ##


In this task we'll......

1. One

1. Two

1. Three

---

<a name="Summary" />
## Summary ##

In this demo we completed the following tasks:

- [Task 1](#Task1)
- [Task 2](#Task2) 
- [Task 3](#Task3)


<a name="Title" />
# Azure Portal Demos #
---

<a name="Overview" />
## Overview ##

In this demo we will cover two quick demos of the two current versions of the Azure Portals. 

The **"old"** portal is being phased out as the breadth of Azure Services out grows it.  However, at the time this is being written there is still a fair amount of functionality that has not yet been implemented in the new portal preview. For that reason, you may find your self working with both portals for the near future until the new portal supports all functionality.

---

<a name="Setup" />
## Setup and Prerequisites ##

To successfully complete this demo you need:

- An active Azure Subscription
- A standards compliant browser

---

<a name="Tasks" />
## Tasks ##

This demo is comprised of the following tasks:

- [View the Current Portal](#Task1)
- [View the Preview Portal](#Task2) 

--- 

<a name="Task1" />
## View the current Portal ##

1. In the browser of your choice, open the current portal at https://manage.windowsazure.com and login with the Microsoft Account associated with your subscription.

1. Along the left hand edge of the portal, you should see a scrollable list of Azure services you can configure:

	![01010-ServicesInPortal](images/01010-servicesinportal.png?raw=true "Services in Portal")

1. You can easily create new service instances by clicking the **"+ NEW"** button in the lower left hand corner.  For example, to quickly create a new website you can 

	- Click **"+NEW"** 
	- Select **"COMPUTE"** | **"WEBSITE"** | **"QUICK CREATE"**
	- Give your new website a name
	- Choose a hosting plan for your website
	- Click **"CREATE WEBSITE"**

	![01020-NewWebsite](images/01020-newwebsite.png?raw=true "NewWebsite")

1. There is infact a gallery of pre-configured website templates that you can choose from:

	- Click **"+ NEW"** 
	- Select **"COMPUTE"** | **"WEBSITE"** | **"FROM GALLERY"**
	- Scroll through the list of available templates
	- Close the Gallery when you are done reviewing the list.

	![01030-WebsiteGallery](images/01030-websitegallery.png?raw=true "Website Gallery")

1. Select **"WEBSITES"** from the list of services along the left, and you should see the website you created listed:

	![01040-WebsiteProvisioning](images/01040-websiteprovisioning.png?raw=true "Website Provisioning")

1. When the status reads **"Running"** right click on the URL of the website and select **"Open in new tab"**:

	![01050-OpenInNewTab](images/01050-openinnewtab.png?raw=true "Open Website in New Tab")

1. A placeholder page should appear letting you know that the website has been created and is ready to have content deployed to it. Close the tab.

	![01055-WebsitePlaceholder](images/01055-websiteplaceholder.png?raw=true "Website Placholder Content")

1. Back on the management portal page, click on the name of the Website to open it's dashboard:

	![01057-ClickWebsiteName](images/01057-clickwebsitename.png?raw=true "Click Website Name")

1.  The dashboard for the website gives you a way to configure and monitor your website.  Step through the various pages to review the options:

	![01060-WebsiteDashboard](images/01060-websitedashboard.png?raw=true "Website Dashboard")

1. Next, delete the new website by clicking switching to the **"DASHBOARD"** page, and clicking the **"DELETE"** button along the bottom:

	![01065-DeleteWebSite](images/01065-deletewebsite.png?raw=true "Delete Web Site")

1. On the **"Delete Confirmation"** page, click the checkmark button to confirm the deletion:

	![01070-ConfirmDeletion](images/01070-confirmdeletion.png?raw=true "Confirm Deletion")

1. You should see a confirmation of the deletion along the bottom of the portal. Click **"OK"**:

	![01080-DeletionStatus](images/01080-deletionstatus.png?raw=true "Deletion Status")

1. Spend your time reviewing the other features and services in the portal.

1. Close the browser when you are done.

--- 

<a name="Task2" />
## Task 2 ##


The new **"preview portal"** is an evolution of the previous portal.  As the breadth of services available on the Azure platform, the management tools needed to configure and maintain those services need to evolve as well.  This new portal is designed to make it easier to manage the ever growing array of services.  

1. Open the new portal at https://portal.azure.com and login with the Microsoft Account associated with your subscription.


1. The **"Home"** page of the new portal is called the **"Startboard"**. It shows you a number of things about your subscription, as well as tiles for any resources that you have **"pinned"** to the startboard:

	![02010-Startboard](images/02010-startboard.png?raw=true "Startboard")

1. You can create resources similar to the previous portal.  To create a Website.  Click **"+ NEW"** and pick **"Website"** from the list of resources:

	![02020-NewWebsite](images/02020-newwebsite.png?raw=true "New Website")

1. As you work with resources in the new portal, "panels" or "blades" of UI open to give you the relevant options.  

1. Complete the values in the **"Website"** blade.  Notice that as you click on options (like **"LOCATION"**, new blades open and close when you choose an option.

	![02030-WebsiteBlade](images/02030-websiteblade.png?raw=true "WebsiteBlade")

1. Create a new **"Resource Group"** for your website.  Resource Groups are a new feature in Azure that allow you to create a logical grouping of resources (like a storage account, a database, and a website).  This allows you to perform certain management tasks on the resource group as a whole:

	![02040-NewResourceGroup](images/02040-newresourcegroup.png?raw=true "New Resource Group")

1. Finally, ensure that the **"Add to Startboard"** checkbox is enabled, and click the **"Create"** button.

	![02050-Create](images/02050-create.png?raw=true "Create")

1. You can monitor the status of the website provisioning on both the Startboard, as well as on the **"Notifications" tab:

	![02060-MonitorWebsiteProvisioning](images/02060-monitorwebsiteprovisioning.png?raw=true "Monitor Website Provisioning")

1. When the website is done provisioning, it's blade should open for you to review.  Close the blade when you are done:

	![02070-WebsiteBlade](images/02070-websiteblade.png?raw=true "Website Blade")

1. You can right click on the tile for the Website on the Startboard to either unpin it, or to customize it's location.  Choose **"Customize"**

	![02080-CustomizeStartboard](images/02080-customizestartboard.png?raw=true "Customize Startbord")

1. You can now resize tiles, re-position them, or un-pin them.  Click the **"Done"** button when you are done:

	![02090-CustomizationOptions](images/02090-customizationoptions.png?raw=true "Customization Options")

1. The **"Browse"** tab lets you browse items you have.  For example.  Click **"Browse"** then select **"Resource"** Groups:

	![02100-BrowseResourceGroups](images/02100-browseresourcegroups.png?raw=true "Browse Resource Groups")

1. Then in the **"Resource groups"** blade, select the resource group you created earlier:

	![02110-SelectResourceGroup](images/02110-selectresourcegroup.png?raw=true "Select Resource Group")

1. The blade for your resource groups should appear showing the resources in it.  In reality, often when you are working on one thing, you need to switch to doing something else briefly.  To simulate that **KEEP THE RESOURCE GROUP BLADE OPEN**, but click the **"HOME"** button to return to the **Startboard**.

	![02120-ResourceGroupBlade](images/02120-resourcegroupblade.png?raw=true "ResourceGroupBlade")

1. Assume that you have now performed some other task.  To return to what you were working on before, you can use the **"JOURNEYS"** tab.  Click **"JOURNEYS"** and you should see an entry for the Resource Group you were working on previously.  Select it to return to where you left off:

	![02130-ResourceGroupJourney](images/02130-resourcegroupjourney.png?raw=true "Resource Group Journey")

1. Finally, let's delete the resource group and the website in it. On the blade for your Resource Group, click the **"Delete"** button along the top:

	![02140-Delete](images/02140-delete.png?raw=true "Delete")

1. On the **"Are you sure..."** blade, enter the name of the Resource Group in the box along the top, review the list of affected resources, and click the **"Delete"** button to confirm the deletion:

	![02150-ConfirmDeletion](images/02150-confirmdeletion.png?raw=true "Confirm Deletion")

1. You can monitor the deletion back on the *""NOTIFICATIONs"** tab:
	
	![02160-DeletionStatus](images/02160-deletionstatus.png?raw=true "Deletion Status")

1. Currently deleting a resource group does not remove tiles for affected resources from the Startboard.  For now, you'll need to manually unpin the tile for your website:

	![02170-UnpinTile](images/02170-unpintile.png?raw=true "Unpin Tile")

1. Close the browser window when you are done.

--- 

<a name="Summary" />
## Summary ##

In this demo we completed the following tasks:

- [View the Current Portal](#Task1)
- [View the Preview Portal](#Task2) 


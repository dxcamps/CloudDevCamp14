<a name="CreateDevTestVmMSDN" />
# Create a Dev/Test VM in Azure for MSDN Subscribers #
---
<a name="Overview" />
## Overview ##

In this demo, we'll show you how MSDN subscribers can create a Dev/Test VM using Gallery Images that include **Windows 8.1 Enterprise and Visual Studio 2013** already installed. This demo is specifically for people with MSDN subscriptions.  The Windows 8.1 client images, and Visual Studio 2013 images are not available to non-MSDN subscribers. 

While it is called a "demo", it is really a step-by-step hands on lab that presenters and event attendees alike can follow to create a development environment in Azure.  

<a id="goals" />
### Goals ###
In this demo, you will see how to:

1. Create a Windows Virtual Machine in Azure
1. Configure the operating system security
1. Install additional software and tools as needed

<a name="technologies" />
### Key Technologies ###

- [Azure Virtual Machines](http://azure.microsoft.com/en-us/documentation/services/virtual-machines/)
- [Azure Virtual Machine Windows Client Images for MSDN Subscribers](http://azure.microsoft.com/blog/2014/05/29/windows-client-images-on-azure/)
- [Visual Studio 2013 Gallery Image for MSDN Subscribers](http://visualstudio2013msdngalleryimage.azurewebsites.net/)

<a name="Setup" />
### Setup and Configuration ###

In order to execute this demo you need to do a little prep.

1. ***IMPORTANT - Follow all of the steps in this demo to create a VM ahead of time.  This will allow you to continue show how subsequent steps are done without having to wait for the provisioning times, reboot times, etc.***  
1. ***Startup the VM to make sure it is running and that you can connect to it via RDP, BEFORE you start the demo***
1. As you do the demo live, show them how to create the VM, but rather than waiting for it to be provisioned, switch to the VM you created ahead of time.    

<a name="Demo" />
## Demo ##
This demo is composed of the following segments:

1. [Create an Azure Virtual Machine using an MSDN Gallery Image](#CreateVM)
1. [Configure Security to allow for Windows 8.1 App Development](#Security)
1. [Install Additional Software as Needed](#Software)
1. [Shutting Down or Deleting Your VM](#CleanUp)

---

<a name="CreateVM" />
### Create an Azure Virtual Machine using an MSDN Gallery Image ###

1. Login to the [Azure Managment Portal](https://manage.windowsazure.com) (https://manage.windowsazure.com) using the appropriate Microsoft Account, and switch to the **"VIRTUAL MACHINES"** page, then along the bottom, click **"+NEW"** | **"COMPUTE"** | **"VIRTUAL MACHINE"** | **"FROM GALLERY"**:

	> **Note:** The Microsoft Account you connect as must have access to an Azure Account associated with an MSDN benefit.  Otherwise, the MSDN gallery images will not be available. If you have more than one subscription associated with the same Microsoft Account, use the Subscriptions filter along the top of the portal window to choose the appropriate MSDN subscription.

	![01010-FromGallery](images/01010-fromgallery.png?raw=true "From Gallery")

1. On the **"Chose an Image"**, enable the **"MSDN"** checkbox, and select the **"Visual Studio Ultimate 2013 Update 2 on Windows 8.1 Enterprise (x64)"** (or an appropriate later version), and click the **"Next"** (**Right Arrow**) button to continue:

	![01020-Win81Image](images/01020-win81image.png?raw=true "Windows 8.1 Image with VS Ultimate")

1. On the **"Virtual machine configuration"** page, enter the following, then click the **Next** (**Right Arrow**) button to continue:

	- **VIRTUAL MACHINE NAME**: Enter a valid windows server computer name
	- **TIER**: Choose **STANDARD**.  You can actually choose **BASIC** if you prefer, and the VM will cost slightly less to execute, but you can't do load balancing or virtual networks with BASIC VMs.  I prefer to choose **STANDARD** for demo VMs I will be deleting soon anyhow so I can show all features if asked. 
	- **SIZE**: Choose **A2**. You can go larger if you have available cores and credits.  I don't recommend going smaller for demo though because the VM performancw will suffer, and the demo will run slower.  Use your best judgement, but remember that you can change this in the future if needed.   
	- **NEW USER NAME**: Enter a valid windows user name.  ***This will be the name of the built-in administrator account.***
	- **NEW PASSWORD** / **CONFIRM**: Enter a complex password for the built-in administrator account. 

	![01030-ConfigureVM01](images/01030-configurevm01.png?raw=true "Configure Virtual Machine")

1. On the next **"Virtual Machine Configuration"** page, enter the following, then click the **Next** (**Right Arrow**) button to continue:

	- **CLOUD SERVICE**: Choose **Create a new cloud service**
	- **CLOUD SERVICE DNS NAME**:  Accept the default cloud service host name if it is valid.  If it isn't, edit it until you get a valid, unique, hostname (indicated by the green circle and checkmark).  This is the fully qualified DNS host name you will use to access all VM endpoints exposed on this cloud service.  
	- **REGION / AFFINITY GROUP / VIRTUAL NETWORK**: Choose the data region closest to you to host your VM.
	- **STORAGE ACCOUNT**:  Choose **"Use an automatically generated storage account"** to let the wizard provision an account for you. 
	- **AVAILABILITY SET**": Choose **(None)**
	- **ENDPOINTS**:  Leave the default **Remote Desktop** and **PowerShell** endpoints.  Note that the **Remote Desktop"" endpoint uses a dynamic (AUTO) public port that will be assigned at creating time.  This allows you to run multiple VMs in the cloud service, each with their own custom RDP endpoint.  

	![01040-ConfigureVM02](images/01040-configurevm02.png?raw=true "Configure Virtual Machine")

1. On the final **"Virtual maching configuration"** page, leave the defaults (ensure that **"Install the VM Agent"** is checked), and click the **"Finish"** (**Checkmark**) button to complete the wizard.

	![01050-ConfigureVM03](images/01050-configurevm03.png?raw=true "Configure Virtual Machine")

	> **Note:** The VM Agent allows a few things.  Allows various extensions to be installed in your VM.  These include the VMAccess extension, that can be used to reset your password if needed, the BGInfo extension which configures the BGInfo utility to show useful information on your VM wallpaper, as well as the Puppet Enterprise Agent, and the Chef agent which allow the VM to be managed by a Puppet or Chef server.  We won't mess with Puppet or Chef in this demo.

1. Back in the portal, on the **VIRTUAL MACHINES** page, you should now see your new VM being provisioned.  It will take 5-10 minutes for the VM to be ready to connect to via RDP. ***If you have a previously created VM ready to go, this is a good time to swtich over to it to demonstrate the remaining steps so you don't have to wait for the VM provisioning to complete.  Its ok if all subsequent steps have been completed on the second VM.  You can still how how the steps are completed, even if they are already done.***

	![01060-VMProvisioning](images/01060-vmprovisioning.png?raw=true "VM Provisioning")

1. Once the VM is running, you can select it by clicking on its status, not it's name.  Clicking on it's name opens it's dashboard.  Which is fine if you want to show it, we just aren't going to walk through the dashboard here.  Once you have it selected, you can click on the **"CONNECT"** button along the bottom of the portal.  

	![01070-Connect](images/01070-connect.png?raw=true "Connect")

1. The portal will inform you that a .RDP file will be downloaded.  Click **OK** to acknowlegde.

	![01080-PortalPrompt](images/01080-portalprompt.png?raw=true "Portal Prompt")

1. Your browser should prompt you to save the file.  Save it to your **"Downloads"** folder:

	![01090-SaveRDPFileToDownloads](images/01090-saverdpfiletodownloads.png?raw=true "Save RDP File to Downloads")

1. Open the folder where the .RDP File was downloaded:

	![01100-OpenDownloadsFolder](images/01100-opendownloadsfolder.png?raw=true "Open Downloads Folder")

1. Then double click on the .RDP file to open it. 

	> **Note:** Of course you could have just opened it rather than saving it, but by saving it, you can open it and edit it.  This may be necessary if you need to manage which devices get connected, the screen resolution, as well as other settings.  Later we'll talk about editing the file as a way to enable Microsoft Accounts to login to the remote VM.  So, for that reason we are saving it here now.  

	![01110-OpenRDPFile](images/01110-openrdpfile.png?raw=true "Open RDP File")

1. If prompted to confirm the publisher of the .RDP file, optionally enable the **"Don't ask me again for connections to this computer"** checkbox if you don't want to receive this prompt in the future.  Then click **"Connect"** to confirm. 

	![01120-Confirm](images/01120-confirm.png?raw=true "Confirm")

1. In the **"Windows Security"** dialog, first select **"Use another account"**:

	![01130-UseAnotherAccount](images/01130-useanotheraccount.png?raw=true "User Another Account")

1. Then, in the credential prompt enter:

	- **User Name**: Enter the built-in administrator user name you specified when creating the VM.  You must enter it in the **&lt;VM Name&gt;&#92;&lt;User Name&gt;** format.
	- **Password**: The complex password you specified for the built-in administrator account when creating the VM earlier.

	![01140-AdminCredentials](images/01140-admincredentials.png?raw=true "Admin Credentials")


1. If you receive a prompt stating that **"The identity of the remote computer annot be verified..."**, optionally enable the **"Don't ask me again..."** checkbox if you don't want to receive prompts in the future, then click **"Yes"** to continue:

	![01150-Yes](images/01150-yes.png?raw=true "Yes")

1.  When the connection completes, you should see the remote desktop:

	![01160-RemoteDesktop](images/01160-remotedesktop.png?raw=true "Remote Desktop")

---

<a name="Security" />
### Configure Security to allow for Windows 8.1 App Development ###

If you hope to do Windows App development with this VM (unfortunately, you can't currently do Windows Phone App development in an Azure VM), you will need to login as a separate account, that is not the built-in administrator.  This is because the built-in administrator has policy restrictions set on it that prevent it from deploying store apps.  If you tried yourself, when you tried to debug a Windows Store app in Visual Studio, you would receive an error like this:

![02010-WindowAppError](images/02010-windowapperror.png?raw=true "Windows App Activation Error")

So, what kind of account should you use?  Well, you have two choices:

- A traditional "Local" account: Easiest, but least funcationality
- A Microsoft Account: Requires a few more steps, but has all the benefits of a Microsoft Account

If you choose to use a Microsoft Account, you again have a decision to make:

![02020-MicrosoftAccountOptions](images/02020-microsoftaccountoptions.png?raw=true "Microsoft Account Options")	


- Create a "Local" account first, then associate it with a "Microsoft Account": Messy from an "account" perspective, but all-in-all is probably the easiest.
- Add a "Microsoft Account" Directly. Cleaner from an "account" perspective but requires chaning remote desktop, and RDP file settings to work. 

For this demo, we will use the first option of creating a Local account, then associating it with a Microsoft Account. This doesn't require changing any Remote Desktop or RDP file settings, and as such is the easiest way to work with an Azure VM, since the RDP file would need to be edited each time it was downloaded. If you want to learn about the second option of adding a Microsoft Account directly, and modifying the Remote Desktop and RDP file settings, go to [**Windows Client images for MSDN subscribers**](http://azure.microsoft.com/blog/2014/05/29/windows-client-images-on-azure/) and read the instructions under the **"How do I use a Microsoft account to remote desktop into the VMs?"** heading.

1. In the **Remote Desktop Connection** window for the VM, ensure that you are logged in as the built-in administrator account you specified when creating them VM. Access the system **"Charms"** bar by moving your mouse to the lower right-corner of the remote desktop, or by using the Remote Desktop Connection window's system menu:

	![02030-RemoteCommands](images/02030-remotecommands.png?raw=true "Remote Commands")

	Or

	![02035-RemoteCommands](images/02035-remotecommands.png?raw=true "Remote Commands")

1. Then, click on the **"Settings"** charm:

	![02040-SettingsCharm](images/02040-settingscharm.png?raw=true "Settings Charm")

1. Then click on **"Change PC settings"**

	![02050-ChangePCSettings](images/02050-changepcsettings.png?raw=true "Change PC Settings")

1. Then click on **"Accounts"**

	![02060-Accounts](images/02060-accounts.png?raw=true "Accounts")

1. On the **"Acounts"** Page, click **"Other Accounts"** then **"Add an account"

	![02070-AddAccount](images/02070-addaccount.png?raw=true "Add an Account")

1. On the **"How will this person sign in?"** page, click the *""Sign in without a Microsoft account (not recommended)"** link along the bottom.  Don't worry about that **"not recommended"** part.  We'll associate it with our Microsoft Account in just a bit. 

	![02070-HowSignIn](images/02070-howsignin.png?raw=true "How will this person sign in")

1. On the **"Add a user"** page, click the **"Local account"** button

	![02080-AddAUser](images/02080-addauser.png?raw=true "Add a User")

1. On the next **"Add a user"** page, enter the credentials for the new user and click **"Next"**

	![02090-NewUserCredentials](images/02090-newusercredentials.png?raw=true "New User Credentials")

1. On the final **"Add a user"** page, click **"Finish"**

	![02100-Finish](images/02100-finish.png?raw=true "Finish")

1. Back on the **"Manage other accounts"** page, click the new user, then click **"Edit"** to modify it:

	![02110-EditUser](images/02110-edituser.png?raw=true "Edit User")

1. In the **Edit account, change the user type to **"Administrator"** (You don't have to do this, but it makes sure that your dev user will have all the permissions it needs in the VM).  Then click **"OK"**

	![02120-MakeAdministrator](images/02120-makeadministrator.png?raw=true "Make Administrator")

1. Return to the **Start Screen** in the remote VM (you can use the **Remote Desktop Connection** window commands as described previously).  The click on the built-in administrators user name, and select **"Sign out"** to log out, and disconnect from the VM. The **Remote Desktop Connection** window will close. 

	![02130-SignOut](images/02130-signout.png?raw=true "Sign Out")

1. Back on your machine, re-connect to the VM either by opening the saved **.RDP** file or by selecting the VM in the portal, and choosing **"CONNECT"** again.  

	![01110-OpenRDPFile](images/01110-openrdpfile.png?raw=true "Open RDP File")

	Or

	![01070-Connect](images/01070-connect.png?raw=true "Connect")

1. Again, follow the prompts, but this time login using the credentials of the Local user you just created (Make sure to use the **&lt;VM Name&gt;&#92;&lt;User Name&gt;** format for the user name.

	![02140-ConnectAsNewUser](images/02140-connectasnewuser.png?raw=true "Connect as new Local User")

1. Using the steps described above, return to the *""Accounts"** page in **
PC Settings"**.  Then from the **"Your account"** page, click **"Connect to a Microsoft account"**

	![02150-ConnectMicrosoftAccount](images/02150-connectmicrosoftaccount.png?raw=true "Connect Microsoft Account")

1. Confirm the password of the currently logged in local user, and click **Next**:

	![02160-ConfirmPassword](images/02160-confirmpassword.png?raw=true "Confirm Password")

1. Then, enter the credentials for the Microsoft Account you wish to use:

	![02180-MicrosoftAccountCredentials](images/02180-microsoftaccountcredentials.png?raw=true "Microsoft Account Credentials")

1. On the **"Help us protect your info"** page, follow the prompts to verify your identity:

	![02190-Protection01](images/02190-protection01.png?raw=true "Protection")

	![02195-Protection02](images/02195-protection02.png?raw=true "Protection")

1. Click **"Next"** on the **OneDrive** page

	![02200-OneDrive](images/02200-onedrive.png?raw=true "OneDrive")

1. Finally, click **"Switch"** to confirm the switch:

	![02210-Switch](images/02210-switch.png?raw=true "Switch")

1. Sign out of the Remote Desktop Connection as described before, and re-connect one more time, this time using your Microsoft Account credentials for the connection:

	![02220-ConnectWithMicrosoftAccount](images/02220-connectwithmicrosoftaccount.png?raw=true "Connect With Microsoft Account")

1. You should now be connected with your Microsoft Account, with local Administrator privileges!

	![02230-Connected](images/02230-connected.png?raw=true "Connected")

1. Next, let's open Visual Studio and get signed in.  On the desktop of the VM, click the **"Visual Studio 2013"**  icon, and when prompted, click **"Sign In"** 

	![02240-SignInToVS](images/02240-signintovs.png?raw=true "Sign In to Visual Studio")

1. Enter the credentials for the Microsoft Account associated with your MSDN subscription, and click **"Sign in"**

	![02250-SignIn](images/02250-signin.png?raw=true "Sign In")

1. If any other prompts appear, answer them to the best of your ability

1. When complete you should be signed into Visual Studio with your Microsoft Account

	![02260-Verify](images/02260-verify.png?raw=true "Verify")

1. You can close Visual Studio when you are done.

---

<a name="Software" />
### Install Additional Software as Needed ###

The VM image we used already has **"Visual Studio Ultimate 2013 Update 2"** installed, but we often need additional tools.  If you plan on using this VM for the subsequent demos in this series, you will also want to ensure that you have 

- **"Azure SDK 2.3"** or later
- **"Applicaiton Insights Tools for Visual Studio"** 
- Any Updates to Visual Studio 
- Optionally SQL Server 2014 Express or Developer Editions
- etc.

In this demo we'll walk through the first few installations.

#### Install the Azure SDK 2.3 ####

1. Open a Remote Desktop Connection to your VM, and login as the Microsoft Account we provisioned previously.  ***All subsequent steps are to be performed within the Remote Desktop Connection window unless otherwise specified.***

1. Open **Internet Explorer** and navigate to **"http://azure.microsoft.com"** (If prompted, select to **"Use Recommended Settings"** in IE).  From the **"Downloads"** page, click the **"VS 2013 Install"**

	![03010-AzureSDKDownload](images/03010-azuresdkdownload.png?raw=true "Azure SDK Download")

1. When prompted, click **"Run"** to run the installer after it is downloaded:

	![03020-Run](images/03020-run.png?raw=true "Run")

1. In the **"User Account Control"** window, click **"Yes"**

	![03030-Yes](images/03030-yes.png?raw=true "Yes")

1. Follow the prompts to install the **"Azure SDK 2.3"** 

	![03040-SdkInstall01](images/03040-sdkinstall01.png?raw=true "SDK Install")

	![03042-SDKInstall02](images/03042-sdkinstall02.png?raw=true "SDK Install")

	![03044-SDKInstall03](images/03044-sdkinstall03.png?raw=true "SDK Install")

	![03046-SDKInstall04](images/03046-sdkinstall04.png?raw=true "SDK Install")

1. You may optionally want to install the **"Windows Azure Cross-Platform Command Line Tools"** and the **"Windows Azure PowerShell"** items as well.  When done.  Click **"Exit"**

	![03050-ExitWebPI](images/03050-exitwebpi.png?raw=true "Web the Web Platform Installer")

#### Install "Application Insights Tools for Visual Studio" ####

1. Open Visual Studio 2013, and from the menu bar, select **"Tools"** | **"Extensions and Updates..."**

	![03060-Extensions](images/03060-extensions.png?raw=true "Extensions")

1. In the **"Extensions and Updates"** window, select **"Online"** on the left hand side, then in the search box in the top right corner, enter **"Application Insights"**.  From the search results list, select **"Application Insights Tools for Visual Studio"** and click **"Download"**:

	![03070-InstallAppInsights](images/03070-installappinsights.png?raw=true "Install App Insights")

1. In the **"Download and Install"** window, click **"Install"**

	![03080-Install](images/03080-install.png?raw=true "Install")

1. When the installation is complete, click the **"Restart Now"** button to restart Visual Studio:

	![03090-RestartNow](images/03090-restartnow.png?raw=true "Restart Now")

#### Install Additional Visual Studio Updates ####

1. Back in Visual Studio, again from the menu bar, select **"TOOLS"** | **"Extensions and Updates"**

1. In the **"Extensions and Updates"** window, select **"Updates"** along the left, and proceed to install the various **"Product Updates"** and **"Visual Studio Gallery"** updates.

	![03100-Updates](images/03100-updates.png?raw=true "Updates")

1. Follow the prompts for each.  Some may require you to close Visul Studio to continue, or restart Visual Studio when the installation is complete.  Continue to return to **"Extensions and Updates"** to install the next update until they have all been installed.  However, you may want to **SKIP** then **"Windows Phone 8.1 Emulators"** because you can't run the emulator in an Azure VM and it would just be a waste of time and storage. 

--- 

<a name="CleanUp" />
### Shutting Down or Deleting Your VM ###

At some point you'll be done using your shiny new VM.  It could be that you just want to shut it down for a while to avoid compute costs when you don't need them machine, or it could be that you are done forever and want to delete the VM.  In this last section we'll cover a few scenarios

- Shutting down your VM to Stop Compute Billing
- Deleting a VM and it's Attached Disks
- Deleting the Entire Cloud Service and it's Deployments

#### Shutting down your VM to compute  ####

1. Open the Azure Management Portal and sign in with an account that has permissions to manage your VMs.  Switch to the **"VIRTUAL MACHINES"** page, and select the VM you wish to stop by clicking on it's **Status** (**not its name**), then click the **"SHUTDOWN"** button along the bottom:

	![04010-Shutdown](images/04010-shutdown.png?raw=true "Shutdown")

1. You will  be prompted to verify that you want to shutdown the VM, and that the IP addresses that were allocated to the VM will be released.  Click **"Yes"** to confirm the shutdown

	![04020-Yes](images/04020-yes.png?raw=true "Yes")

1. Wait until the VM's status reads **"Stopped (Deallocated)"**.   Once the machine is **"DEALLOCATED"** you are no longer being bill for compute costs.  However, you are still incurring storage costs for the **&#42;.vhd** files associated with the Virtual Hard Disks.  Of course, you can always start the VM back up by selecting it, and clicking the **"START"** button along the bottom.

	![04030-Deallocated](images/04030-deallocated.png?raw=true "Deallocated")


#### Deleting a VM and it's Attached Disks  ####

1. If you don't plan on using the VM anymore, you can delete it, and stop being billed for the **&#42;.vhd** file storage costs.  To delete a VM and it's attached disks, in the Azure Management Portal, on the **"VIRTUAL MACHINES"** page, select the VM and click the **"DELETE"** button along the bottom.  You can then choose to either **"Delete the attached disks"**, or to **"Keep the attached disks"** (in which case you will still pay for their storage):

	> **Note:** You might decide to keep the attached disks so you can download them to your local network before you delete them, or perhaps to create a new Virtual Machine using those same disks in the future.  Just remember that you will continue to incur storage costs for them. 

	![04040-DeleteVM](images/04040-deletevm.png?raw=true "Delete VM")

1. When prompted, read the information, and if you aggree, click **"Yes"** to delete the VM and it's disks.

	![04050-Confirm](images/04050-confirm.png?raw=true "Confirm")

#### Deleting the Entire Cloud Service and it's Deployments ####

1. The third option we'll cover here is to delete the **Cloud Service** that contains the VM, and all Web Role or Work Role deployments as well as any VMs and their attached disks.  This is an easy (albeit scary) way to delete an entire cloud service and everything in it.  **BE CAREFUL!!!**.

1. In the Azure Management Portal, go to the **"CLOUD SERVICES"** Page, and select the Cloud Service that you want to delete, again by clicking on it's status, then click the **"DELETE"** button along the bottom, and choose from:

	- **"Delete the cloud service and its deployments"** - This is the most complete (and therefore most destructive) option.  This is the one we'll use, but **MAKE SURE YOU MEAN IT!**
	- **"Delete all Virtual Machines"** - Deletes just the VMs but not their attached disks, and leaves Web Role and Worker Role deployments alone.
	- **"Delete all virtual machines and their attached disks"** - Deletes all VMs and their attached disks, but leaves Web Role and Worker Role deployments alone.

	![04060-DeleteCloudService](images/04060-deletecloudservice.png?raw=true "Delete Cloud Service")

1. Read the confirmation prompt, and if you agree, click **"Yes"** to delete the cloud service and all it's deployments, VMs and attached disks.

	![04070-Yes](images/04070-yes.png?raw=true "Yes")

````C#
void Main()
{
  var x = 1;
  var y = 2;
  var z = x + y;
}
````


--- 

<a name="summary" />
## Summary ##

Congraulations!  In this demo you learned the basics of:

- Creating a Virtual Machine in Azure
- Configuring Security for Windows App Development
- Installing Additional Software as Needed
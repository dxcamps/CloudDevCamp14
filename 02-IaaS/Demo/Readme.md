<a name="Title" />
# IaaS Demos #
---

<a name="Overview" />
## Overview ##

In these demos we will cover Virtual Machines, Virtual Machine Extensions and Virtual Networks.

[Demo 1](#Demo1)
[Demo 2](#Demo2)
[Demo 3](#Demo3)
[Demo 4](#Demo4)
---

<a name="Setup" />
## Setup and Prerequisites ##

To successfully complete this demo you need:

- An Active Azure Account

---

<a name="Demo1" />
## Demo 1: Create a New Virtual Machine from the Gallery ##

1. Log into the Azure Portal at http://portal.azure.com.

	![Portal](images/portal.png?raw=true)

1. Click New

	![New](images/new.png?raw=true)

1. Click Everything

	![everything](images/everything.png?raw=true)

1. Click Virtual Machines

1. Select a VM from the Gallery and click create.

	![Create](images/create.png?raw=true)

1. Fill in the information for Host name, user name, and password. Additionally, set any optional configurations for Network, storage, diagnostics and subscription. Be sure to set a location that is geographically located to the majority of your traffic.

	![Info2](images/info2.png?raw=true)

1. Click "Create" to start VM Creation process.

--- 
<a name="Demo2" />
## Demo 2: Add VM Extension to a New Virtual Machine ##

To successfully complete this demo you need:

- An Active Azure Account
- A Windows Server 2012 VM has been provisioned.

---

1. Log into the Azure Portal at http://portal.azure.com.

	![Portal](images/portal.png?raw=true)

1. On the left, click Browse, then click Virtual Machines

	![BrowseVM](images/browsevm.png?raw=true)

1. Select VM

	![VMSelect](images/vmselect.png?raw=true)

1. In the journey that comes up, scroll down to the Extensions and Click the box to see the extensions installed.

	![Extensions](images/extensions.png?raw=true)

1. Click the "+ Add" icon in the header and select the Extension to install.

	![AddChef](images/addchef.png?raw=true)

1. After reading the installation requirements in information, click "Create" to install the extension.

	![Complete](images/complete.png?raw=true)

---

<a name="Demo3" />
## Demo 3: Add Custom PowerShell Script Extension to a New Virtual Machine ##

To successfully complete this demo you need:

- An Active Azure Account
- Azure PowerShell v0.8 or higher has been installed and configured.
- Desired Azure subscription has already been selected. 
- A storage account has been provisioned under the same Azure subscription.
- A scripts container has been created under the storage account with public read access.
- A helloworld.ps1 PowerShell script has been uploaded to the container. The content of the script is a single line: write-output “Hello World!”
- PowerShell environment has been set with large font for easy reading.
- A Windows Server 2012 VM has been provisioned.


---
1. Start PowerShell command prompt

1. Issue the following cmdlet to log in to your Azure account:

    ````PowerShell
    Add-AzureAccount
    ````

1. Issue the following cmdlets to get a reference to the virtual machine instance:

	````PowerShell
	$serviceName = “[cloud service that hosts the VM]”
	$vmName = “[name of the VM]”
	$vm = Get-AzureVM –ServiceName $serviceName –Name $vmName
	````

1. Issue the following cmdlet to see what extensions are already installed:

	````PowerShell
	Get-AzureVMExtension –VM $vm
	````
1. Use the following cmdlet **(all on one line)** to enable Custom Script Extension, and instruct it to download and execute the helloworld.ps1 (this takes about 20-30 seconds):

	````PowerShell
	Set-AzureVMCustomScriptExtension -ContainerName scripts -StorageAccountName
	'[your storage account name]' -VM $vm -FileName ‘helloworld.ps1'
	-Run ‘helloworld.ps1' | Update-AzureVM -Verbose
	````

6. Next, the following cmdlet will retrieve and display the script execution result:

	````PowerShell
	$status = Get-AzureVM -ServiceName $serviceName -Name $vmName
	$result = $status.ResourceExtensionStatusList.ExtensionSettingStatus.SubStatusList | Select Name, @{"Label"="Message";Expression = {$_.FormattedMessage.Message }} 
	$result
	````

--- 

<a name="Demo4" />
## Demo 4: Setup Virtual Networking for Point To Site Network ##

Follow first fifteen steps of the following blog:

	http://haishibai.blogspot.com/2013/05/walkthrough-file-sharing-between-your.html


1. Log on to Windows Azure Management Portal.

1. On command bar, click on **NEW** button, then select **NETWORKS**->**VIRTUAL NETWORK**->**CUSTOM CREATE** menu.
1. On **Virtual Network Details** page, enter network **NAME** as _pointtosite_, create or select a **AFFINITY GROUP**, then click next arrow.
1. On **DNS Servers and VPN Connectivity** page, check **Configure Point-to-Site VPN**, then click next arrow:

	![VPN1](images/vpn1.png?raw=true)

1. On **Point-to-Site Connectivity** page, click next arrow.

1. On **Virtual Network Address Spaces** page, click on **add gateway subnet** button, and then click on check icon to complete network creation.

1.	After the virtual network has been created, open its **DASHBOARD** page, and the click **CREATE GATEWAY** icon to create the dynamic routing gateway.
1.	Add a new Windows Server 2012 virtual machine to the virtual network. Note that when you specify user credential, make sure to use the same user id and password of your local account. This is because the virtual machine on Windows Azure is not under your domain controller, and we are using the same user credential on both local and virtual machines to allow file sharing.
1.	As the virtual machine is cooking, let’s create two self-signed certificates: one for root, and another for client identification. During the following steps we’ll need to upload the root certificate to Windows Azure so Windows Azure can validate the client machine using the certificate chain.
1.	Launch **Developer Command Prompt for VS2012** as administrator. Change current folder to a folder where you want to keep the generated certificates. Here I’ll use folder **c:\books**.
1.	Use command
		
	````
	makecert -sky exchange -r -n "CN=MyFakeRoot" -pe -a sha1 -len 2048 -ss My
	````

	to create root certificate.
1.	Use command

	````
	makecert -n "CN=MyLaptop" -pe -sky exchange -m 96 -ss My -in "MyFakeRoot" -is my -a sha1
	````

	to create client certificate.

1. Launch **certmgr**.

1.	Export the root certificate as a **MyFakeRoot.cer** file (without private key).

1.	[Optional] if you are configuring VPN for another client, you’d need to install the client certificate on target machine.

1.	Go back to Windows Azure Management Portal. On **DASHBOARD** page of the virtual network, click on link **Upload client certificate** to upload the root certificate. Note at this point the gateway should have been created.

	![VPN2](images/vpn2.png?raw=true)
 
1.	After certificate has been uploaded, you can download and install VPN client from the DASHBOARD page (AMD64 Client link for 64-bit machines, x86 Client link for 32-bit machines).

1.	After VPN client has been installed, you can see the VPN connection on your Windows network connection list. Click on the network to connect. When prompted by the VPN client, click Connect button to continue.

1.	[Optional] Now you can use ipconfig/all to verify if VPN connection has been successfully established  (look for PPP adapter).
1.	Log on to the virtual machine, create a new Share folder under c:\. Share the folder with the user you specified when you created the virtual machine. 
1.	On Management Portal, record the virtual machines private IP on its DASHBOARD page. In my case the IP is 10.0.1.4.
1.	Now you can use Explorer on your local machine and access the shared folder by \\10.0.1.4\Share.

--- 
Outlook Social connector for Gravatar
========
This connector allows you to see the Gravatar picture of your email participants directly in Outlook 2010/2013. Please be aware the Microsoft has stopped supporting the integration point this add-in uses as of Outlook 2016 and it therefore no longer works.

#### Install Instructions
 1. Extract a release ZIP into a folder of your liking
 2. Depending on whether you use 32 or 64 bit Outlook, execute the appropriate Install batch script
 3. When ever you move the files, you have to re-execute the install script

#### Build/Develop Instructions
 1. Find a machine with Outlook 2010 or 2013 and Visual Studio (e.g. 2017)
 2. Clone the repo
 3. Load the SocialProvider.reg and the GravatarOSC/Other Stuff/EnableDebugging.reg
 4. Open Visual Studio as Administrator and open solution from repo
 5. ***Note:*** if you are not using 32bit Outlook 2013, you will have to adjust the executable path.
 6. Start using the Debug profile

#### Release Instructions
 1. Copy install cmd's, the reg file and the dll's from the Release folder into a clean new folder
 2. ZIP it up and upload to GitHub

## .NET Library for Gravatar
Fork of http://gravatarnet.codeplex.com to incorporate Gravatar profile support.

 C# Provider Template for the Microsoft Outlook Social Connector (OSC)
 Copyright © 2009-2010 Microsoft Corporation
 Use this template to develop a provider for the OSC

 Instructions:
   1. Change the project name and namespace to your project name and namespace identifiers.
   2. Modify AssemblyInfo to use the correct assembly information.
   2. Implement the interface members marked as To-Do and add additional dependencies/references as required.
   3. Build the Project.
   4. The provider assembly ProgID must be listed as a key under
   HKEY_CURRENT_USER\Software\Microsoft\Office\Outlook\SocialConnector\SocialProviders
   5. To distribute the setup project, create a setup project in Visual Studio or the setup tool of your choice.
   6. Your setup project should COM register your assembly and also create the ProgID key listed in step 4.

 THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
 KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
 IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
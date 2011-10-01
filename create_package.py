import os
import zipfile

myZipFile = zipfile.ZipFile("gravatar-osc.zip", "w" )
myZipFile.write("Install32.cmd")
myZipFile.write("Install64.cmd")
myZipFile.write("GravatarOSC\\bin\\Release\\Gravatar.NET.dll", "Gravatar.NET.dll")
myZipFile.write("GravatarOSC\\bin\\Release\\GravatarOSC.dll", "GravatarOSC.dll")
myZipFile.write("GravatarOSC\\bin\\Release\\OutlookSocialProvider.dll", "OutlookSocialProvider.dll")

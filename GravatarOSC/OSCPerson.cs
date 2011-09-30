using System;
using System.Diagnostics;
using OutlookSocialProvider;

namespace GravatarOSC
{
    class OSCPerson : ISocialPerson 
    {
        public string GetActivities(DateTime startTime) {
            Debug.WriteLine("OSCPerson::GetActivities called");
            
            return string.Empty;
        }

        public string GetDetails() {
            Debug.WriteLine("OSCPerson::GetDetails called");
            
            return string.Empty;
        }

        public string GetFriendsAndColleagues() {
            Debug.WriteLine("OSCPerson::GetFriendsAndColleagues called");
            
            return string.Empty;
        }

        public string[] GetFriendsAndColleaguesIDs() {
            Debug.WriteLine("OSCPerson::GetFriendsAndColleaguesIDs called");
            
            string[] result = { "" };
            return result;
        }

        public byte[] GetPicture() {
            Debug.WriteLine("OSCPerson::GetPicture called");
            
            return null;
        }

        public string GetProfileUrl() {
            Debug.WriteLine("OSCPerson::GetProfileUrl called");
            
            return string.Empty;
        }

        /// <summary>
        /// Not supported in OSC version 1.0 and version 1.1
        /// </summary>
        public string GetStatus() {
            Debug.WriteLine("OSCPerson::GetStatus called");

            return string.Empty;
        }
    }
}

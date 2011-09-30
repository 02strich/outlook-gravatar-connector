using System;
using System.Diagnostics;
using OutlookSocialProvider;

namespace GravatarOSC
{
    class OSCProfile : OSCPerson, ISocialProfile  {
        public string GetActivitiesOfFriendsAndColleagues(DateTime startTime) {
            Debug.WriteLine("OSCProfile::GetActivitiesOfFriendsAndColleagues called");
            
            return string.Empty;
        }
        
        public new string GetActivities(DateTime startTime) {
            Debug.WriteLine("OSCProfile::GetActivities called");

            return string.Empty;
        }


        public new string GetDetails() {
            Debug.WriteLine("OSCProfile::GetDetails called");
            
            return string.Empty;
        }

        public new string GetFriendsAndColleagues() {
            Debug.WriteLine("OSCProfile::GetFriendsAndColleagues called");
            
            return string.Empty;
        }

        public new string[] GetFriendsAndColleaguesIDs() {
            Debug.WriteLine("OSCProfile::GetFriendsAndColleaguesIDs called");
            
            return null;
        }

        public new byte[] GetPicture() {
            Debug.WriteLine("OSCProfile::GetPicture called");
            
            return null;
        }

        public new string GetProfileUrl() {
            Debug.WriteLine("OSCProfile::GetProfileUrl called");
            
            return string.Empty;
        }

        /// <summary>
        /// Not supported in OSC version 1.0 and version 1.1
        /// </summary>
        public bool[] AreFriendsOrColleagues(string[] userIDs) {
            Debug.WriteLine("OSCProfile::AreFriendsOrColleagues called");

            return null;
        }

        /// <summary>
        /// Not supported in OSC version 1.0 and version 1.1
        /// </summary>
        public new string GetStatus() {
            Debug.WriteLine("OSCProfile::GetStatus called");
            
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not supported in OSC version 1.0 and version 1.1
        /// </summary>
        public void SetStatus(string status) {
            Debug.WriteLine("OSCProfile::SetStatus called");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using OutlookSocialProvider;

namespace GravatarOSC
{
    class OSCSession : ISocialSession, ISocialSession2 {
        private readonly OSCProvider _provider;
        private readonly string _loggedOnUser;

        public OSCSession(OSCProvider provider, bool autoConfigured = false) {
            _provider = provider;

            if(autoConfigured)
                _loggedOnUser = "Local User";
        }

        public string GetActivities(string[] emailAddresses, DateTime startTime) {
            Debug.WriteLine("ISocialSession::GetActivities called for:");
            foreach (var emailAddress in emailAddresses) {
                Debug.WriteLine("\t" + emailAddress);
            }

            var feed = new OSCSchema.activityFeedType { network = GetNetworkIdentifier() };
            return HelperMethods.SerializeObjectToString(feed);
        }

        /// <summary>
        /// Gravatar does not support activities, so we return an empty structure
        /// </summary>
        public string GetActivitiesEx(string[] hashedAddresses, DateTime startTime) {
            Debug.WriteLine("ISocialSession2::GetActivitiesEx called for:");
            foreach (var address in hashedAddresses)
                Debug.WriteLine("\t" + address);

            var feed = new OSCSchema.activityFeedType {network = GetNetworkIdentifier()};
            return HelperMethods.SerializeObjectToString(feed);
        }

        #region Person/People
        /// <summary>
        /// ???
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ISocialPerson GetPerson(string userId) {
            Debug.WriteLine("GetPerson called for " + userId);
            
            return new OSCPerson();
        }

        /// <summary>
        /// Should return a list of friends, for now nobody is our friend
        /// </summary>
        /// <param name="hashedAddresses"></param>
        /// <returns></returns>
        public string GetPeopleDetails(string hashedAddresses) {
            var addresses = HelperMethods.DeserializeStringToObject<OSCSchema2.hashedAddresses>(hashedAddresses);
            
            Debug.WriteLine("ISocialSession2::GetPeopleDetails called for personsAddresses");
            foreach (var line in addresses.personAddresses)
                Debug.WriteLine("\t" + line.index + ": " + String.Join(", ", line.hashedAddress));

            var resultList = new List<OSCSchema.personType>();
            foreach (var personAddress in addresses.personAddresses) {
                if (personAddress.hashedAddress.Length == 0) continue;
                
                // find gravatar for hashed addresses
                var response = _provider.GravatarService.Exists(personAddress.hashedAddress, true);
                var index = HelperMethods.GetTrueIndex(response.MultipleOperationResponse);
                
                // no gravatar found
                if(index == -1) continue;

                // get gravatar profile
                _provider.GravatarService.

                resultList.Add(new OSCSchema.personType {
                    userID = personAddress.hashedAddress[index],
                    pictureUrl = String.Format("http://www.gravatar.com/avatar/{0}", personAddress.hashedAddress[index]),
                    
                    index = personAddress.index,
                    indexSpecified = true
                });
            }


            var friends = new OSCSchema.friends {person = resultList.ToArray()};
            
            var result = HelperMethods.SerializeObjectToString(friends);
            Debug.WriteLine(result);
            return result;
        }
        
        /// <summary>
        /// ???
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public string FindPerson(string userID) {
            Debug.WriteLine("FindPerson called for " + userID);
            
            return string.Empty;
        }

        /// <summary>
        /// We do not follow people
        /// </summary>
        public void FollowPerson(string emailAddress) {
            Debug.WriteLine("FollowPerson called for " + emailAddress);
        }

        /// <summary>
        /// We do not follow people
        /// </summary>
        public void FollowPersonEx(string[] emailAddresses, string displayName) {
            Debug.WriteLine("ISocialSession2::FollowPersonEx called for displayName = " + displayName);
            foreach (var emailAddress in emailAddresses)
                Debug.WriteLine("\t" + emailAddress);
        }

        /// <summary>
        /// We do not follow, so we also do not unfollow
        /// </summary>
        /// <param name="userId"></param>
        public void UnFollowPerson(string userId) {
            Debug.WriteLine("UnFollowPerson called for " + userId);
        }
        #endregion

        public string GetNetworkIdentifier() {
            return OSCProvider.NetworkName;
        }

        public string SiteUrl {
            set { /* To-Do: Implement SiteUrl */ }
        }


        #region Logon
        /// <summary>
        /// We do fake auto configure and therefore never call this
        /// </summary>
        public void Logon(string username, string password) {
            Debug.WriteLine("Logon called with username: " + username + ", password: " + password);
        }

        /// <summary>
        /// We do fake auto configure and therefore never call this
        /// </summary>
        public void LogonWeb(string connectIn, out string connectOut) {
            if (!string.IsNullOrEmpty(connectIn))
                Debug.WriteLine("LogonWeb called with connectIn: " + connectIn);
            connectOut = string.Empty;
        }

        public void LogonCached(string connectIn, string userName, string password, out string connectOut) {
            if (!string.IsNullOrEmpty(connectIn))
                Debug.WriteLine("LogonCached called with connectIn: " + connectIn);
            
            connectOut = string.Empty;
        }
        
        public ISocialProfile GetLoggedOnUser() {
            return new OSCProfile();
        }

        public string LoggedOnUserID {
            get { return _loggedOnUser; }
        }

        public string LoggedOnUserName {
            get { return _loggedOnUser; }
        }

        public string GetLogonUrl() {
            return OSCProvider.NetworkUrl;
        }

        #endregion
    }
}

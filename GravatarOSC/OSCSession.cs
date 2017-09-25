using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Gravatar.NET;
using OutlookSocialProvider;

namespace GravatarOSC
{
    class OSCSession : ISocialSession, ISocialSession2 {
        private GravatarService _gravatarService = null;
        private readonly OSCProvider _provider;
        private string _loggedOnUser;

        public OSCSession(OSCProvider provider, bool autoConfigured = false) {
            _provider = provider;

            if (autoConfigured) {
                _loggedOnUser = "Local User";
            }
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
        /// not yet clear, what this is used for
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ISocialPerson GetPerson(string userId) {
            Debug.WriteLine("GetPerson called for " + userId);
            
            return new OSCPerson();
        }

        /// <summary>
        /// Should return information about the addresses
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
                try {
                    // find gravatar for hashed addresses
                    var response = _gravatarService.Exists(personAddress.hashedAddress, true);
                    if (response.IsError) {
                        Debug.WriteLine("Encountered error when calling gravatar: " + response.ErrorCode);
                        continue;
                    }
                    var index = HelperMethods.GetTrueIndex(response.MultipleOperationResponse);

                    // no gravatar found
                    if (index == -1) continue;

                    // get gravatar profile
                    var profile = GravatarService.GetGravatarProfile(personAddress.hashedAddress[index], true);
                    if (profile == null)
                        resultList.Add(new OSCSchema.personType {
                            userID = personAddress.hashedAddress[index],
                            pictureUrl = GravatarService.GetGravatarUrlForAddress(personAddress.hashedAddress[index], true),

                            index = personAddress.index,
                            indexSpecified = true
                        });
                    else
                        resultList.Add(new OSCSchema.personType {
                            userID = personAddress.hashedAddress[index],
                            pictureUrl = profile.ThumbnailUrl,
                            city = profile.CurrentLocation,
                            firstName = profile.GivenName,
                            lastName = profile.FamilyName,
                            nickname = profile.PreferredUsername,

                            index = personAddress.index,
                            indexSpecified = true
                        });
                } catch(Exception e) {
                    Debug.WriteLine(e.ToString());
                }
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
        /// We do cached logon and therefore never call this
        /// </summary>
        public void Logon(string username, string password) {
            Debug.WriteLine("Logon called with username: " + username + ", password: " + password);
        }

        /// <summary>
        /// We do cached windows logon and therefore never call this
        /// </summary>
        public void LogonWeb(string connectIn, out string connectOut) {
            if (!string.IsNullOrEmpty(connectIn))
                Debug.WriteLine("LogonWeb called with connectIn: " + connectIn);
            connectOut = string.Empty;
        }

        /// <summary>
        /// Logon to Gravatar service and safe Base64-encoded username/password combination in connectOut
        /// </summary>
        /// <param name="connectIn">If recurring login, information from last connectOut (should contain encoded password)</param>
        /// <param name="userName">The username supplied</param>
        /// <param name="password">If first login, the password supplied by the user</param>
        /// <param name="connectOut">Some value to cache, which allows us later to logon again (without the password)</param>
        public void LogonCached(string connectIn, string userName, string password, out string connectOut) {
            string realUsername = userName;
            string realPassword = password;

            if (!string.IsNullOrEmpty(connectIn)) {
                Debug.WriteLine("LogonCached called with connectIn: " + connectIn);
                var parts = connectIn.Split(':');
                realUsername = Encoding.UTF8.GetString(Convert.FromBase64String(parts[0]));
                realPassword = Encoding.UTF8.GetString(Convert.FromBase64String(parts[1]));
                connectOut = connectIn;
            } else {
                Debug.WriteLine("LogonCached called with username='" + userName + "' and password='" + password + "'");
                connectOut = String.Format("{0}:{1}", Convert.ToBase64String(Encoding.UTF8.GetBytes(userName)), Convert.ToBase64String(Encoding.UTF8.GetBytes(password)));
            }

            _gravatarService = new GravatarService(realUsername, realPassword);
            var response = _gravatarService.Test();
            if (response.IsError) {
                Debug.WriteLine("Failed to login to Gravatar");
                throw new Exception();
            }

            _loggedOnUser = realUsername;
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

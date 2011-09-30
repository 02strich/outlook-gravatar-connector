using System;
using System.Linq;
using Gravatar.NET.Data;
using Gravatar.NET.Exceptions;

namespace Gravatar.NET {
    /// <summary>
    /// Used to centerally populte the appropriate response properties of the <see cref="Gravatar.NET.GravatarServiceResponse"/>
    /// instance that is returned after an API method call.
    /// </summary>
    internal class GravatarResponseParser {
        internal static void ParseResponseForMethod(string method, GravatarServiceResponse response) {
            if (response == null) return;
            if (response.IsError) return;

            switch (method) {
                case GravatarConstants.METHOD_TEST:
                    SetTestMethodResponse(response);
                    break;

                case GravatarConstants.METHOD_EXISTS:
                    SetExistsMethodResponse(response);
                    break;

                case GravatarConstants.METHOD_ADDRESSES:
                    SetAddressesMethodResponse(response);
                    break;
                case GravatarConstants.METHOD_USE_USER_IMAGE:
                    SetUseUserImageMethodResponse(response);
                    break;
                case GravatarConstants.METHOD_USER_IMAGES:
                    SetUserImagesMethodResponse(response);
                    break;
                case GravatarConstants.METHOD_SAVE_DATA:
                case GravatarConstants.METHOD_SAVE_URL:
                    SetSaveMethodResponse(response);
                    break;
                case GravatarConstants.METHOD_DELETE_USER_IMAGE:
                    SetDeleteImageMethodResponse(response);
                    break;
                default:
                    throw new UnknownGravatarMethodException(method);
            }
        }

        private static void SetAddressesMethodResponse(GravatarServiceResponse response) {
            response.AddressesResponse = response.ResponseParameters
                .Select(parameter => new GravatarAddress {
                    Name = parameter.Name,
                    ImageId = parameter.StructValue.Parameters.Get("userimage").StringValue,
                    Image = new GravatarUserImage {
                        Rating = (GravatarImageRating)parameter.StructValue.Parameters.Get("rating").IntegerValue,
                        Url = parameter.StructValue.Parameters.Get("userimage_url").StringValue
                    }
                });
        }

        private static void SetDeleteImageMethodResponse(GravatarServiceResponse response) {
            var responsePar = response.ResponseParameters.First();
            if (responsePar.Type == GravatarParType.Bool)
                response.BooleanResponse = responsePar.BooleanValue;
        }

        private static void SetSaveMethodResponse(GravatarServiceResponse response) {
            var responsePar = response.ResponseParameters.First();
            response.SaveResponse = responsePar.Type == GravatarParType.String ? new GravatarSaveResponse { Success = true, SavedImageId = responsePar.StringValue } : new GravatarSaveResponse();
        }

        private static void SetUserImagesMethodResponse(GravatarServiceResponse response) {
            response.ImagesResponse = (
                from par in response.ResponseParameters
                where par.Type == GravatarParType.Array
                let arrPars = par.ArrayValue
                let rating = int.Parse(arrPars.First().StringValue)
                select new GravatarUserImage {
                    Name = par.Name,
                    Rating = (GravatarImageRating) rating,
                    Url = arrPars.Last().StringValue
                });
        }

        private static void SetUseUserImageMethodResponse(GravatarServiceResponse response) {
            response.MultipleOperationResponse = (
                from par in response.ResponseParameters 
                select par.BooleanValue
            ).ToArray();
        }

        private static void SetTestMethodResponse(GravatarServiceResponse response) {
            var responsePar = response.ResponseParameters.Last();
            response.IntegerResponse = responsePar.Type == GravatarParType.Integer ? responsePar.IntegerValue : 0;
        }

        private static void SetExistsMethodResponse(GravatarServiceResponse response) {
            response.MultipleOperationResponse = (
                from par in response.ResponseParameters
                select par.BooleanValue
            ).ToArray();
        }
    }
}

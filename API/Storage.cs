using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Genso.Astrology.Library;
using Google.Apis.Auth;

namespace API;

/// <summary>
/// Simple class encapsulate access to azure blob storage files
/// </summary>
public static class Storage
{

    private const string UserDataListXml = "UserDataList.xml";
    private const string BlobContainerName = "vedastro-site-data";


    /// <summary>
    /// Gets user data given google valid payload, if user does
    /// not exist makes a new one & returns that
    /// </summary>
    public static async Task<UserData> GetUserData(GoogleJsonWebSignature.Payload payload)
    {
        //get user data list file  (UserDataList.xml) Azure storage
        var userDataListXml = await APITools.GetXmlFileFromAzureStorage(UserDataListXml, BlobContainerName);

        //look for user with matching email
        var userEmail = payload.Email;
        var foundUserXml = userDataListXml.Root?.Elements()
            .Where(userDataXml => userDataXml.Element("Email")?.Value == userEmail)?
            .FirstOrDefault();

        //if user found, initialize xml and send that
        if (foundUserXml != null) { return UserData.FromXml(foundUserXml); }

        //if no user found, make new user and send that
        else
        {
            //create new user from google's data
            var newUser = new UserData(payload);

            //add new user xml to main list
            await APITools.AddXElementToXDocument(newUser.ToXml(), UserDataListXml, BlobContainerName);

            //return newly created user to caller
            return newUser;
        }

    }

    
}
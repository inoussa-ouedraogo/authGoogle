using Proyecto26;
using UnityEngine;

/// <summary>
/// Handles authentication calls to Firebase
/// </summary>

public static class FirebaseAuthHandler
{
    private const string ApiKey = "AIzaSyC2URgqyjWqQWdpjpFCl8yw9CaAztE8Tak"; //firebase authentification

    /// <summary>
    /// cote l'utilisateur sur la base de donnée avec son Token
    /// </summary>
    public static void SingInWithToken(string token, string providerId)
    {
        var payLoad =
            $"{{\"postBody\":\"id_token={token}&providerId={providerId}\",\"requestUri\":\"http://localhost\",\"returnIdpCredential\":true,\"returnSecureToken\":true}}";
        RestClient.Post($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithIdp?key={ApiKey}", payLoad).Then(
            response =>
            {
                // You now have the userId (localId) and the idToken of the user!
                Debug.Log(response.Text);
            }).Catch(Debug.Log);    
    }
}

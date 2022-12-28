using System;
using System.Collections.Generic;
using Proyecto26;
using UnityEngine;



public static class GoogleAuthenticator
{
    private const string ClientId = "710485781883-9pnur7v8i7h7gevp02r1cg07avcie0p5.apps.googleusercontent.com"; //ton clien google
    private const string ClientSecret = "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"; //le passwork

    private const int Port = 1234;//le port va ecouter les reponse de connection
    private static readonly string RedirectUri = $"http://localhost:{Port}";//ip ou sera rediriger les reponse de connection

    private static readonly HttpCodeListener codeListener = new HttpCodeListener(Port);

    
    public static void GetAuthCode()
    {
        Application.OpenURL($"https://accounts.google.com/o/oauth2/v2/auth?client_id={ClientId}&redirect_uri={RedirectUri}&response_type=code&scope=email");

        codeListener.StartListening(code =>
        {
            ExchangeAuthCodeWithIdToken(code, idToken =>
            {
                FirebaseAuthHandler.SingInWithToken(idToken, "google.com");
            });
            
            codeListener.StopListening();
        });
    }
    
    /// <summary>
    /// echanger le cod identification avec un token
  
    public static void ExchangeAuthCodeWithIdToken(string code, Action<string> callback)
    {
        try
        {
            RestClient.Request(new RequestHelper
            {
                Method = "POST",
                Uri = "https://oauth2.googleapis.com/token",
                Params = new Dictionary<string, string>
                {
                    {"code", code},
                    {"client_id", ClientId},
                    {"client_secret", ClientSecret},
                    {"redirect_uri", RedirectUri},
                    {"grant_type", "authorization_code"}
                }
            }).Then(
                response =>
                {
                    var data =
                        StringSerializationAPI.Deserialize(typeof(GoogleIdTokenResponse), response.Text) as
                            GoogleIdTokenResponse;
                    callback(data.id_token);
                }).Catch(Debug.Log);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}

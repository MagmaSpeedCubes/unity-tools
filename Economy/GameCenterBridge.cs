using UnityEngine;
//using GooglePlayGames;
using UnityEngine.SocialPlatforms;

namespace MagmaLabs{
    public class GameCenterBridge : MonoBehaviour

    {
        public static string playerID { get; private set; }
        public static string deviceID { get; private set; }
        public static string username { get; private set; }
        void Awake()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer: case RuntimePlatform.OSXPlayer: case RuntimePlatform.OSXEditor:
                    Social.localUser.Authenticate(success =>
                    {
                        if (success)
                        {
                            Debug.Log("Successfully authenticated with Apple Game Center.");
                            playerID = Social.localUser.id;
                            deviceID = SystemInfo.deviceUniqueIdentifier;
                            username = Social.localUser.userName;
                            
                        }
                        else
                        {
                            Debug.LogError("Failed to authenticate with Apple Game Center.");
                        }
                    });
                    break;
                // case RuntimePlatform.Android:
                //     PlayGamesPlatform.Instance.Authenticate((success) =>
                //     {
                //         if (success)
                //         {
                //             Debug.Log("Successfully authenticated with Google Play Games.");
                //             playerID = Social.localUser.id;
                //             username = Social.localUser.userName;
                //             deviceID = SystemInfo.deviceUniqueIdentifier;

                //         }
                //         else
                //         {
                //             Debug.LogError("Failed to authenticate with Google Play Games.");
                //         }
                //     });
                //     break;
                default:
                    Debug.LogError("Not on supported platform");
                    break;
            }
            // Authenticate Game Center

        }
    }

}

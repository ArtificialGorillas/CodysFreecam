using BepInEx;
using Gorilla_Tag_Mod__Graic__V2;
using GorillaNetworking;
using Photon.Pun;
using Photon.Realtime;
using Photon;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilla;

namespace CodysFreecam
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin("com.CodyLovesVR.gorillatag.RoomJoiner", "RoomJoiner", "1.0.0")]
    public class CodysFreecam : BaseUnityPlugin
    {
        bool inRoom;

        bool GUIEnabled = false;

        void Start()
        {
            //Gplay = GorillaLocomotion.Player.Instance;
        }

        void Update()
        {
            if (inRoom || !PhotonNetwork.InRoom)
            {
                if (Keyboard.current.tabKey.wasPressedThisFrame)
                {
                    GUIEnabled = !GUIEnabled;
                }
                WASD();
                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0.07f, 0f);
            }

        }

        /* This attribute tells Utilla to call this method when a modded room is joined */
        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            //If you wanna make it modded only, put the code in update in here

            inRoom = true;
        }

        /* This attribute tells Utilla to call this method when a modded room is left */
        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {


            inRoom = false;
        }

        private string roomName = "";

        private void OnGUI()
        {
            if (GUIEnabled)
            {
                GUI.Box(new Rect(10, 10, 150, 260), "Freecam");

                roomName = GUI.TextField(new Rect(15, 50, 140, 30), roomName, 25);

                if (GUI.Button(new Rect(15, 100, 140, 40), "Join Room"))
                {
                    if (!string.IsNullOrEmpty(roomName))
                    {
                        JoinRoom(roomName);
                    }
                    else
                    {
                        Debug.Log("Room name cannot be empty.");
                    }
                }

                if (GUI.Button(new Rect(15, 150, 140, 40), "Disconnect"))
                {
                    Disconnect();
                }

                if (GUI.Button(new Rect(15, 200, 140, 40), "Set To Modded"))
                {
                    GorillaComputer.instance.currentGameMode.Value = "MODDED_CASUAL";
                }
            }
        }

        public static float flySpeed = 5f;
        private static float X = 0;
        private static float Y = 0;


        public static void WASD()
        {
            GorillaLocomotion.Player playerInstance = GorillaLocomotion.Player.Instance;

            if (playerInstance == null)
            {
                Debug.LogError("Player instance is null!");
                return;
            }

            Rigidbody playerRigidbody = playerInstance.GetComponent<Rigidbody>();
            Vector3 movementDirection = Vector3.zero;

            if (Mouse.current.rightButton.isPressed)
            {
                Vector3 eulerAngles = GorillaLocomotion.Player.Instance.headCollider.transform.eulerAngles;
                if (X == 0f)
                {
                    X = Mouse.current.position.ReadValue().x;
                }

                float mouseXDelta = Mouse.current.position.ReadValue().x - X;
                eulerAngles.y += mouseXDelta * .5f;

                GorillaLocomotion.Player.Instance.headCollider.transform.eulerAngles = eulerAngles;
                X = Mouse.current.position.ReadValue().x;
            }
            else
            {
                X = 0f;
            }

            if (Keyboard.current.wKey.isPressed)
            {
                GorillaLocomotion.Player.Instance.headCollider.transform.position += GorillaLocomotion.Player.Instance.headCollider.transform.forward * .1f;
                Debug.Log("W");
            }
            if (Keyboard.current.sKey.isPressed)
            {
                GorillaLocomotion.Player.Instance.headCollider.transform.position -= GorillaLocomotion.Player.Instance.headCollider.transform.forward * .1f;
                Debug.Log("S");
            }
            if (Keyboard.current.aKey.isPressed)
            {
                GorillaLocomotion.Player.Instance.headCollider.transform.position -= GorillaLocomotion.Player.Instance.headCollider.transform.right * .1f;
                Debug.Log("A");
            }
            if (Keyboard.current.dKey.isPressed)
            {
                GorillaLocomotion.Player.Instance.headCollider.transform.position += GorillaLocomotion.Player.Instance.headCollider.transform.right * .1f;
                Debug.Log("D");
            }
            if (Keyboard.current.spaceKey.isPressed)
            {
                GorillaLocomotion.Player.Instance.headCollider.transform.position += GorillaLocomotion.Player.Instance.headCollider.transform.up * .1f;
                Debug.Log("Space");
            }
            if (Keyboard.current.ctrlKey.isPressed)
            {
                GorillaLocomotion.Player.Instance.headCollider.transform.position += GorillaLocomotion.Player.Instance.headCollider.transform.up * -.1f;
                Debug.Log("Ctrl");
            }
        }

        public static void JoinRoom(string roomName)
        {
            PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(roomName, JoinType.Solo);
        }
        
        public static void Disconnect()
        {
            PhotonNetwork.Disconnect();
        }
    }
}

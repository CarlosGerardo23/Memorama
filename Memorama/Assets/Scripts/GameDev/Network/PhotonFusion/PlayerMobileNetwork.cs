using Fusion;
using GameDev.Network.Photon.Mobile.Inputs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameDev.Network.Photon.Mobile
{
    public class PlayerMobileNetwork : NetworkBehaviour
    {
        //private NetworkCharacterControllerPrototype _cc;



        public override void FixedUpdateNetwork()
        {
            if (GetInput(out MobileNetworkInputData data))
            {
                print("Getting input");
                Vector3 worlPosition = UnityEngine.Camera.main.ScreenToWorldPoint(data.fingerStartPosition);
                RaycastHit2D hit = Physics2D.Raycast(worlPosition, Vector2.zero);
                if (hit.collider != null)
                {
                    if(hit.transform.TryGetComponent(out IInteractable interactable))
                    {
                        print("interact with object");
                        interactable.OnInteract();
                    }
                }
            }
        }
    }
}
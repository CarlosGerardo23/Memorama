using Fusion;
using GameDev.Behaviour2D.Controls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameDev.Network.Photon.Mobile.Inputs
{
    public struct MobileNetworkInputData : INetworkInput
    {
        public Vector3 fingerStartPosition;
        public Vector3 fingerEndPosition;
    }
}
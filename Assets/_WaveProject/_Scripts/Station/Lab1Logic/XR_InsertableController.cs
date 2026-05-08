using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveProject
{
    public class XR_InsertableController : ConnectableObject
    {
        [SerializeField] private bool loopInsert;
        [SerializeField] private CarriageStation station;

        public override void Connect()
        {
            base.Connect();
            station.loopInsert = loopInsert;
            if (connectorPoint.includeLayers == LayerMask.GetMask("Cross"))
                station.crossInsert = true;
            if (connectorPoint.includeLayers == LayerMask.GetMask("Long"))
                station.longInsert = true;
        }

        public override void Disconnect()
        {
            base.Disconnect();
            station.crossInsert = false;
            station.longInsert = false;
        }
    }
}

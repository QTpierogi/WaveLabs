using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace WaveProject
{
    public abstract class ConnectableObject : MonoBehaviour
    {
        public Collider connectorPoint;
        public ParentConstraint parentConstraint;

        public bool connected;
        private bool canBeConnected;

        public ConstraintSource newConstraint;

        private void Awake()
        {
            connected = false;
            canBeConnected = false;

            parentConstraint = GetComponent<ParentConstraint>();
            connectorPoint = null;
        }

        public void InvokeConnection()
        {
            if (connected)
            {
                Disconnect();
            }
            else if (canBeConnected)
            {
                Connect();
            }
        }

        public virtual void Connect()
        {
            newConstraint.sourceTransform = connectorPoint.gameObject.transform;
            parentConstraint.AddSource(newConstraint);
        }
        public virtual void Disconnect()
        {
            parentConstraint.RemoveSource(0);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.includeLayers == LayerMask.GetMask("Connectable") && other.isTrigger)
            {
                canBeConnected = true;
                connectorPoint = other;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.includeLayers == LayerMask.GetMask("Connectable") && other.isTrigger)
            {
                canBeConnected = false;
                connectorPoint = null;
            }
        }
    }
}

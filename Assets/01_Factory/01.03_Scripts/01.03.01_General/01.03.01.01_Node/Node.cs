using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RuntimeNodeEditor
{
    public abstract class Node : MonoBehaviour
    {
        public string               ID          { get; private set; }
        public Vector2              Position    { get => _panelRectTransform.anchoredPosition; }
        public RectTransform        PanelRect   { get => _panelRectTransform; }
        public string               LoadPath    { get; private set; }
        public List<SocketOutput>   Outputs     { get; private set; }
        public List<SocketInput>    Inputs      { get; private set; }

        public event Action<SocketInput, IOutput> OnConnectionEvent;
        public event Action<SocketInput, IOutput> OnDisconnectEvent;

        public TMP_Text                     headerText;
        public GameObject                   draggableBody;

        private int                         triggerCounter = 0;
        private NodeDraggablePanel          _dragPanel;
        private RectTransform               _panelRectTransform;
        private INodeEvents                 _nodeEvents;
        private ISocketEvents               _socketEvents;
        private Vector2                     gaphSize;

        public void Init(INodeEvents nodeEvents, ISocketEvents socketEvents, Vector2 pos, string id, string path)
        {
            ID                  = id;
            LoadPath            = path;
            Outputs             = new List<SocketOutput>();
            Inputs              = new List<SocketInput>();

            _nodeEvents         = nodeEvents;
            _socketEvents       = socketEvents;
            _panelRectTransform = gameObject.GetComponent<RectTransform>();
            _dragPanel          = draggableBody.AddComponent<NodeDraggablePanel>();
            _dragPanel.Init(this, _nodeEvents);
            SetPosition(pos);
        }
        public int getTriggerCounter() { return triggerCounter; }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            triggerCounter++;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            triggerCounter--;
        }

        public virtual void Setup() { }

        public virtual bool CanMove()
        {
            return true;
        }
        
        public void Register(SocketOutput output)
        {
            output.SetOwner(this, _socketEvents);
            Outputs.Add(output);
        }

        public void Register(SocketInput input)
        {
            input.SetOwner(this, _socketEvents);
            Inputs.Add(input);
        }

        public void Connect(SocketInput input, SocketOutput output)
        {
            OnConnectionEvent?.Invoke(input, output);
        }

        public void Disconnect(SocketInput input, SocketOutput output)
        {
            OnDisconnectEvent?.Invoke(input, output);
        }

        public virtual void OnSerialize(Serializer serializer)
        {

        }

        public virtual void OnDeserialize(Serializer serializer)
        {

        }

        public void SetHeader(string name)
        {
            headerText.SetText(name);
        }

        public void SetGraphSize(Vector2 size)
        {
            gaphSize = size;
        }

        public void SetPosition(Vector2 pos)
        {
            int multiplier = 20;
            Vector2 originalPos = pos;
            if (pos.x % multiplier != 0 && pos.y % multiplier != 0)
            {
                pos.x = (float)(Math.Round(originalPos.x / multiplier) * multiplier);
                pos.y = (float)(Math.Round(originalPos.y / multiplier) * multiplier);
            }
            float graphSize = gaphSize.x / 2;
            float nodeWidth = this.GetComponent<RectTransform>().rect.width;
            float nodeHeight = this.GetComponent<RectTransform>().rect.height;
            if (pos.x - nodeWidth / 2 < -graphSize)
            {
                pos.x = -graphSize + nodeWidth / 2;
            }
            else if (pos.x + nodeWidth / 2 > graphSize)
            {
                pos.x = graphSize - nodeWidth / 2;
            }
            if (pos.y - nodeHeight < -graphSize)
            {
                pos.y = -graphSize + nodeHeight;
            }
            else if (pos.y > graphSize)
            {
                pos.y = graphSize;
            }
            _panelRectTransform.localPosition = pos; 
        }

        public void SetAsLastSibling()
        {
            _panelRectTransform.SetAsLastSibling();
        }
        
        public void SetAsFirstSibling()
        {
            _panelRectTransform.SetAsFirstSibling();
        }
    }
}
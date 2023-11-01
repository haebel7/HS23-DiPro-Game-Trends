using System;
using RuntimeNodeEditor;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RuntimeNodeEditor.Examples
{
    public class ExampleNodeEditor : NodeEditor
    {
        private string _savePath;
        private Vector2 finalGraphSize = Vector2.one * 800f;

        public override void StartEditor(NodeGraph graph)
        {
            base.StartEditor(graph);
            _savePath = Application.dataPath + "/01_Factory/01.01_Prefabs/Resourcesgraph.json";
            
            Events.OnGraphPointerClickEvent           += OnGraphPointerClick;
            Events.OnNodePointerClickEvent            += OnNodePointerClick;
            Events.OnConnectionPointerClickEvent      += OnNodeConnectionPointerClick;
            Events.OnSocketConnect                    += OnConnect;

            graph.SetSize(finalGraphSize);
        }

        private void OnConnect(SocketInput arg1, SocketOutput arg2)
        {
            Graph.drawer.SetConnectionColor(arg2.connection.connId, Color.green);
        }

        private void OnGraphPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Right: 
                {
                    var ctx = new ContextMenuBuilder()
                    .Add("nodes/Ressource",     CreateRessourceNode)
                    .Add("nodes/Smelter",       CreateSmelterNode)
                    .Add("nodes/Assembling",    CreateAssemblingNode)
                    .Add("nodes/Inventory",     CreateInventoryNode)
                    .Add("graph/load",          ()=>LoadGraph(_savePath))
                    .Add("graph/save",          ()=>SaveGraph(_savePath))
                    .Build();

                    SetContextMenu(ctx);
                    DisplayContextMenu(); 
                }
                break;
                case PointerEventData.InputButton.Left: CloseContextMenu(); break;
            }
        }

        private void SaveGraph(string savePath)
        {
            CloseContextMenu();
            Graph.SaveFile(savePath);
        }

        private void LoadGraph(string savePath)
        {
            CloseContextMenu();
            Graph.Clear();
            Graph.LoadFile(savePath);
        }

        private void OnNodePointerClick(Node node, PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                var ctx = new ContextMenuBuilder()
                .Add("duplicate",            () => DuplicateNode(node))
                .Add("clear connections",    () => ClearConnections(node))
                .Add("delete",               () => DeleteNode(node))
                .Build();

                SetContextMenu(ctx);
                DisplayContextMenu();
            }
        }

        private void OnNodeConnectionPointerClick(string connId, PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                var ctx = new ContextMenuBuilder()
                .Add("clear connection", () => DisconnectConnection(connId))
                .Build();

                SetContextMenu(ctx);
                DisplayContextMenu();
            }
        }


        //  context item actions
        
        private void CreateRessourceNode()
        {
            Graph.Create("01.01.01_Nodes/RessourceNode");
            CloseContextMenu();
        }

        private void CreateSmelterNode()
        {
            Graph.Create("01.01.01_Nodes/SmelterNode");
            CloseContextMenu();
        }

        private void CreateAssemblingNode()
        {
            Graph.Create("01.01.01_Nodes/AssemblingNode");
            CloseContextMenu();
        }

        private void CreateInventoryNode()
        {
            Graph.Create("01.01.01_Nodes/InventoryNode");
            CloseContextMenu();
        }

        private void DeleteNode(Node node)
        {
            Graph.Delete(node);
            CloseContextMenu();
        }
        
        private void DuplicateNode(Node node)
        {
            Graph.Duplicate(node);
            CloseContextMenu();
        }

        private void DisconnectConnection(string line_id)
        {
            Graph.Disconnect(line_id);
            CloseContextMenu();
        }

        private void ClearConnections(Node node)
        {
            Graph.ClearConnectionsOf(node);
            CloseContextMenu();
        }

    }
}
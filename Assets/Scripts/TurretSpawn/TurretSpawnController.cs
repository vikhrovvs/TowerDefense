using Field;
using Runtime;
using Turret;
using UnityEngine;
using Grid = Field.Grid;

namespace TurretSpawn
{
    public class TurretSpawnController : IController
    {
        private Grid m_Grid;
        private TurretMarket m_Market;

        public TurretSpawnController(Grid grid, TurretMarket market)
        {
            m_Grid = grid;
            m_Market = market;
        }
        public void OnStart()
        {
        }

        public void OnStop()
        {
        }

        public void Tick()
        {
            if (m_Grid.HasSelectedNode() && Input.GetMouseButtonDown(0))
            {
                TurretAsset asset = m_Market.ChosenTurret;
                if (asset != null)
                {
                    Node selectedNode = m_Grid.GetSelectedNode();
                    TrySpawnTurret(asset, selectedNode);
                }
                else
                {
                    Debug.Log("Not enough money!");
                }
            }
        }

        private void TrySpawnTurret(TurretAsset asset, Node node)
        {
            bool wasOccupied = Game.Player.Grid.TryOccupyNode(node);
            if (wasOccupied)
            {
                m_Market.BuyTurret(asset);
                TurretView view = Object.Instantiate(asset.ViewPrefab);
                TurretData data = new TurretData(asset, node);
                data.AttachView(view);
                Game.Player.TurretSpawned(data);
            }
        }
    }
}
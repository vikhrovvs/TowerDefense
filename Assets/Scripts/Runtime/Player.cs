using System.Collections.Generic;
using Enemy;
using Field;
using UnityEngine;
using Grid = Field.Grid;

namespace Runtime
{
    public class Player
    {
        private List<EnemyData> m_EnemyDatas = new List<EnemyData>();

        public IReadOnlyList<EnemyData> EnemyDatas => m_EnemyDatas;

        public readonly GridHolder GridHolder;
        public readonly Grid Grid;

        public Player()
        {
            GridHolder = Object.FindObjectOfType<GridHolder>();
            GridHolder.CreateGrid();
            Grid = GridHolder.Grid;
        }

        public void EnemySpawned(EnemyData data)
        {
            m_EnemyDatas.Add(data);
        }
    }
}
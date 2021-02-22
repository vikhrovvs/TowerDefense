using UnityEngine;

namespace Field
{
    public class GridHolder: MonoBehaviour
    {
        [SerializeField]
        private int m_GridWidth;
        [SerializeField]
        private int m_GridHeight;

        [SerializeField]
        private Vector2Int m_TargetCoordinate;

        [SerializeField]
        private Vector2Int m_StartCoordinate;

        [SerializeField]
        private float m_NodeSize;
        
        private Grid m_Grid;

        private Camera m_Camera;

        private Vector3 m_Offset;

        public Vector2Int TargetCoordinate => m_TargetCoordinate;

        public Grid Grid => m_Grid;

        private void Awake() //вызывается в момент загрузки сцены первым делом, до старта
        {
            m_Camera = Camera.main; //в 90% случаев одна камера. Лучше сохранить, так как Camera.main медленный метод, идет по всем объектам в сцене
            
            // Default plane size is 10 by 10
            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;
            transform.localScale = new Vector3(width * 0.1f, 1f, height * 0.1f);

            m_Offset = transform.position - (new Vector3(width, 0f, height) * 0.5f);
            m_Grid = new Grid(m_GridWidth, m_GridHeight, m_Offset, m_NodeSize, m_StartCoordinate);

            //Несколько раз обращаться к трансформу неэффективно, но здесь это не важно - всего 2 обращения, причем в эвейке.
        }

        private void OnValidate()
        {
            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;
            transform.localScale = new Vector3(width * 0.1f, 1f, height * 0.1f);

            m_Offset = transform.position - (new Vector3(width, 0f, height) * 0.5f);
            m_Grid = new Grid(m_GridWidth, m_GridHeight, m_Offset, m_NodeSize, m_StartCoordinate);
        }

        private void Update()
        {
            if (m_Grid == null || m_Camera == null) //дорого т. к. у MonoBehaviour == null переопределено 
            {
                return; //отказоустойчивость на случай работы в команде или спустя время. Лучше всегда делать такие проверки
            }

            Vector3 mousePosition = Input.mousePosition;

            Ray ray = m_Camera.ScreenPointToRay(mousePosition); //исходит из камеры и есть направление

            if (Physics.Raycast(ray, out RaycastHit hit))
                //Raycast возвращает bool. Если попали, то возвращает структуру RaycastHit
                //Различие между классом и структурой: класс хранится ссылкой, структура лежит целиком в стеке
                //Структуры именно копируются в метод, и методы их не меняют
            {
                if (hit.transform != transform)
                {
                    return; //надо будет повесить коллайдер в юнити
                    //Если трансформ хита не совпадает с трансформом объекта, на который повесили скрипт
                }

                Vector3 hitPosition = hit.point;
                Vector3 difference = hitPosition - m_Offset;

                int x = (int)(difference.x / m_NodeSize); //каст вниз - это нам и нужно
                int y = (int)(difference.z / m_NodeSize);

                if (Input.GetMouseButtonDown(0))
                {
                    Node node = m_Grid.GetNode(x, y);
                    node.IsOccupied = !node.IsOccupied;
                    m_Grid.UpdatePathfinding();

                }
            }
        }

        private void OnDrawGizmos() //debug method
        {
            if (m_Grid == null)
            {
                return;
            }

            
            foreach (Node node in m_Grid.EnumerateAllNodes())
            {
                if (node.NextNode == null)
                {
                    continue;
                }
                
                if (node.IsOccupied)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(node.Position, 0.2f*m_NodeSize);
                    continue;
                }
                Gizmos.color = Color.red;
                Vector3 start = node.Position;
                Vector3 end = node.NextNode.Position;

                Vector3 direction = end - start;

                start -= direction * 0.25f;
                end -= direction * 0.75f;
                
                Gizmos.DrawLine(start, end);
                Gizmos.DrawSphere(end, 0.1f * m_NodeSize);
            }
        }
    }
}
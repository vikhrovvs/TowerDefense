using System;
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
        private float m_NodeSize;
        
        private Grid m_Grid;

        private Camera m_Camera;

        private Vector3 m_Offset;
        
        private void Awake() //вызывается в момент загрузки сцены первым делом, до старта
        {
            m_Grid = new Grid(m_GridWidth, m_GridHeight);
            m_Camera = Camera.main; //в 90% случаев одна камера. Лучше сохранить, так как Camera.main медленный метод, идет по всем объектам в сцене
            
            // Default plane size is 10 by 10
            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;
            transform.localScale = new Vector3(width * 0.1f, 1f, height * 0.1f);

            m_Offset = transform.position - (new Vector3(width, 0f, height) * 0.5f);
            //Несколько раз обращаться к трансформу неэффективно, но здесь это не важно - всего 2 обращения, причем в эвейке.
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
                
                Debug.Log(x + " " + y); //.ToString() убрали т. к. автоматический каст внутри
            }
        }

        private void OnDrawGizmos() //debug method
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(m_Offset, 0.1f);
        }
    }
}
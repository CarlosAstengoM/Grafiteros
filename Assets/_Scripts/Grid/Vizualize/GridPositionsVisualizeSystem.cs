using UnityEngine;
using System.Collections.Generic;

namespace TurnBasedStrategy.Grid
{
    public class GridPositionsVisualizeSystem : MonoBehaviour
    {
        public static GridPositionsVisualizeSystem Instance;

        [SerializeField] private GameObject _prefab;
        private IndividualGridPositionVisual[,] _gridPositionsDebug;

        private void Awake()
        {
            if(Instance != null)
            {
                UnityEngine.Debug.LogError("More than one Grid Position Debug System singleton detected");
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            int width = LevelGrid.Instance.GetWidth();
            int height = LevelGrid.Instance.GetHeight();

            _gridPositionsDebug = new IndividualGridPositionVisual[width,height];

            for (int x=0; x < width; x++)
            {
                for (int z=0; z < height; z++)
                {
                    GridPosition gridPosition = new GridPosition(x,z);
                    Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);

                    GameObject gridDebug = Instantiate(_prefab,worldPosition,Quaternion.identity);
                    _gridPositionsDebug[x,z] = gridDebug.GetComponent<IndividualGridPositionVisual>(); 
                }
            }
            ShowAllGridPositions();
        }

        public void ShowAllGridPositions()
        {
            foreach(IndividualGridPositionVisual gridPositionDebug in _gridPositionsDebug)
            {
                gridPositionDebug.Show();
            }
        }

        public void ShowListOfGridPositions(List<GridPosition> gridPositions)
        {
            foreach(GridPosition gridPosition in gridPositions)
            {
                _gridPositionsDebug[gridPosition.X, gridPosition.Z].Show();
            }
        }

        public void HideAllGridPositions()
        {
            foreach (IndividualGridPositionVisual gridPositionDebug in _gridPositionsDebug)
            {
                gridPositionDebug.Hide();
            }
        }
    }

}

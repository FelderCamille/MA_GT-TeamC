using System;
using Core;
using Objects;
using UnityEngine;

namespace UI.Tile
{
    public class BaseTile : MonoBehaviour
    {
        [SerializeField] private bool onGrid;
        [SerializeField] private bool isSafeArea;
        [SerializeField] private GameObject natureGridPlane;
        [SerializeField] private GameObject natureBorder;
        [SerializeField] private GameObject natureSafeAreaBorder;
        [SerializeField] private GameObject naturePlane;
        [SerializeField] private GameObject warGridPlane;
        [SerializeField] private GameObject warBorder;
        [SerializeField] private GameObject warSafeAreaBorder;
        [SerializeField] private GameObject warPlane;

        public void InitAsOnGrid()
        {
            onGrid = true;
            isSafeArea = false;
            Init();
        }
        
        public void Start()
        {
            Init();
        }

        private void Init()
        {
            switch (GameParametersManager.Instance.MapTheme)
            {
                case MapTheme.Nature:
                    if (onGrid)
                    {
                        natureGridPlane.SetActive(true);
                        naturePlane.SetActive(false);
                        warGridPlane.SetActive(false);
                        warPlane.SetActive(false);
                        natureBorder.SetActive(!isSafeArea);
                        natureSafeAreaBorder.SetActive(isSafeArea);
                        warBorder.SetActive(false);
                        warSafeAreaBorder.SetActive(false);
                    }
                    else
                    {
                        natureGridPlane.SetActive(false);
                        naturePlane.SetActive(true);
                        warGridPlane.SetActive(false);
                        warPlane.SetActive(false);
                        natureBorder.SetActive(false);
                        natureSafeAreaBorder.SetActive(false);
                        warBorder.SetActive(false);
                        warSafeAreaBorder.SetActive(false);
                    }
                    break;
                case MapTheme.War:
                    print($"-------------- WAR and onGrid: {onGrid}--------------");
                    if (onGrid)
                    {
                        natureGridPlane.SetActive(false);
                        naturePlane.SetActive(false);
                        warGridPlane.SetActive(true);
                        warPlane.SetActive(false);
                        natureBorder.SetActive(false);
                        natureSafeAreaBorder.SetActive(false);
                        warBorder.SetActive(!isSafeArea);
                        warSafeAreaBorder.SetActive(isSafeArea);
                    }
                    else
                    {
                        natureGridPlane.SetActive(false);
                        naturePlane.SetActive(false);
                        warGridPlane.SetActive(false);
                        warPlane.SetActive(true);
                        natureBorder.SetActive(false);
                        natureSafeAreaBorder.SetActive(false);
                        warBorder.SetActive(false);
                        warSafeAreaBorder.SetActive(false);
                    }
                    break;
                default:
                    throw new NotImplementedException("Map theme not implemented");
            }
        }
    }
}
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController_StreetCarMinigam2 : MonoBehaviour
{
    public GameObject streetCarObj;
    public GameObject peopleParent;
    public List<Cube_StreetCarMinigame2> listCube = new List<Cube_StreetCarMinigame2>();
    public const float SCALE = 24.35714f;
    public Canvas canvas;
    public List<WaypointCheck_StreetCarMinigame2> listWaypointCheck = new List<WaypointCheck_StreetCarMinigame2>();
    public List<Transform> listWaypointSpawn = new List<Transform>();
    public bool isBegin;
    public RaycastHit2D[] hit;
    public Vector2 mouseCurrentPos;
    public Camera mainCamera;
    public WaypointCheck_StreetCarMinigame2[,] arrayListWaypointCheck = new WaypointCheck_StreetCarMinigame2[5, 7]; // 2 chieu
    public Transform[,] arrayListWaypointSpawn = new Transform[5, 7];
    public List<Cube_StreetCarMinigame2> listWaypointSameColor = new List<Cube_StreetCarMinigame2>();
    public bool isFalling;
    public int level = 0;
    public int[][,] arrayListSpawnLevel = new int[5][,]; //rang cua + 2 chieu
    public int countCubeLevel;
    public Hammer_StreetCarMinigame2 hammerObj;
    private float f2;
    public int power;
    public TextMeshProUGUI txtLevel, txtPower;
    public bool isLose, isWin, isIntro;
    public GameObject wordUI;
    public bool isComplete, isTutorial;
    public GameObject tutorial;

    private void Awake()
    {
        isBegin = false;
        isLose = false;
        isWin = false;
        isIntro = true;
        isTutorial = true;
        isFalling = false;
    }

    private void Start()
    {
        SetSizeCamera();
        level = 1;
        power = 5;
        InputText();
        arrayListSpawnLevel[0] = new int[,] { { 4, 4, 2, 2, 2, 3, 3 }, { 4, 4, 4, 2, 3, 3, 1 }, { 4, 2, 2, 2, 3, 3, 1 }, { 2, 3, 3, 3, 3, 1, 1 }, { 2, 2, 2, 2, 1, 1, 1 } };
        arrayListSpawnLevel[1] = new int[,] { { 3, 3, 2, 2, 2, 4, 4 }, { 1, 3, 3, 2, 4, 4, 4 }, { 1, 3, 3, 2, 2, 2, 4 }, { 1, 1, 3, 3, 3, 3, 2 }, { 1, 1, 1, 2, 2, 2, 2 } };
        arrayListSpawnLevel[2] = new int[,] { { 3, 3, 2, 2, 2, 1, 1 }, { 3, 3, 3, 2, 2, 1, 1 }, { 2, 3, 3, 2, 2, 2, 1 }, { 2, 2, 3, 3, 3, 4, 4 }, { 2, 2, 2, 4, 4, 4, 4 } };
        arrayListSpawnLevel[3] = new int[,] { { 2, 2, 4, 4, 4, 3, 4 }, { 3, 2, 2, 4, 3, 3, 1 }, { 3, 1, 1, 2, 2, 1, 1 }, { 3, 1, 1, 1, 3, 3, 1 }, { 4, 4, 4, 1, 2, 3, 4 } };
        arrayListSpawnLevel[4] = new int[,] { { 2, 2, 4, 4, 2, 3, 4 }, { 3, 2, 2, 4, 3, 3, 1 }, { 3, 1, 1, 2, 2, 1, 1 }, { 3, 1, 4, 1, 3, 3, 1 }, { 4, 4, 4, 1, 2, 3, 4 } };
        isComplete = false;
        hammerObj.gameObject.SetActive(false);
        wordUI.SetActive(false);
        tutorial.SetActive(false);
        txtLevel.gameObject.SetActive(false);
        txtPower.gameObject.SetActive(false);
        txtPower.DOFade(0, 0.1f);
        txtLevel.DOFade(0, 0.1f);
        ShowIntro();
        SetWaypoint();
    }

    void SetSizeCamera()
    {
        float f1 = 16.0f / 9;
        f2 = Screen.width * 1.0f / Screen.height;

        mainCamera.orthographicSize *= f1 / f2;
    }

    void SetWaypoint()
    {
        int k = 0;
        int h = 0;
        for (int i = 0; i < arrayListWaypointSpawn.GetLength(0); i++)
        {
            for (int j = 0; j < arrayListWaypointSpawn.GetLength(1); j++)
            {
                arrayListWaypointSpawn[i, j] = listWaypointSpawn[k++];
            }
        }

        for (int i = 0; i < arrayListWaypointCheck.GetLength(0); i++)
        {
            for (int j = 0; j < arrayListWaypointCheck.GetLength(1); j++)
            {

                arrayListWaypointCheck[i, j] = listWaypointCheck[h++];
            }
        }
    }

    void SetUpLevel()
    {
        countCubeLevel = 35;
        isComplete = false;
        if (level == 4 || level == 5)
        {
            power = 9;
        }
        else
        {
            power = 5;
        }
        InputText();
        txtLevel.transform.DOPunchScale(new Vector3(1.2f,1.2f,1.2f), 1);
        txtPower.transform.DOPunchScale(new Vector3(1.2f, 1.2f, 1.2f), 1);
        
        for (int m = 0; m < arrayListWaypointSpawn.GetLength(0); m++)
        {
            for (int n = 0; n < arrayListWaypointSpawn.GetLength(1); n++)
            {
                Cube_StreetCarMinigame2 obj = Spawn(m, n, arrayListSpawnLevel[level - 1][m, n]);
                obj.transform.DOMoveY(arrayListWaypointCheck[m, n].transform.position.y, 2).SetEase(Ease.InQuart).OnComplete(() =>
                {
                    if (isIntro)
                    {
                        peopleParent.transform.GetChild(0).gameObject.SetActive(true);
                        for (int z = 0; z < 5; z++)
                        {
                             peopleParent.transform.GetChild(0).GetChild(z).transform.DOPunchScale(new Vector2(0.025f, 0.025f), 0.5f);                     
                        }
                    }                            
                    obj.transform.DOMoveY(obj.transform.position.y + 0.5f, 0.5f).OnComplete(() =>
                    {                       
                        obj.GetComponent<Rigidbody2D>().gravityScale = 1;
                        if (isIntro)
                        {
                            isIntro = false;
                            Destroy(peopleParent.transform.GetChild(0).gameObject);
                            peopleParent.transform.DOMoveX(peopleParent.transform.position.x + 30, 2f);
                            mainCamera.DOOrthoSize(mainCamera.orthographicSize * 0.714f, 1).SetEase(Ease.Linear).OnComplete(() =>
                            {
                                hammerObj.gameObject.SetActive(true);
                                wordUI.SetActive(true);
                                txtPower.gameObject.SetActive(true);
                                txtPower.DOFade(1, 2);
                                txtLevel.gameObject.SetActive(true);
                                txtLevel.DOFade(1, 2);
                                tutorial.transform.DOScale(1, 1).SetLoops(-1);
                                tutorial.SetActive(true);
                                isBegin = true;
                            });
                        }                     
                    });
                });
            }
        }
    }

    void ShowIntro()
    {
        peopleParent.transform.DOMove(new Vector2(6.59f, -3.98f), 3);
        Invoke(nameof(SetUpLevel), 2);   
    }


    Cube_StreetCarMinigame2 Spawn(int m, int n, int color)
    {
        Cube_StreetCarMinigame2 obj = Instantiate(listCube[color - 1]);
        obj.transform.position = arrayListWaypointSpawn[m, n].transform.position;
        obj.transform.parent = canvas.transform.GetChild(0);
        obj.transform.localScale = new Vector2(SCALE, SCALE);
        return obj;
    }

    void InputText()
    {
        txtPower.text = $"{power}";
        txtLevel.text = $"{level}" + "/5";
    }

    void CheckColor(Cube_StreetCarMinigame2 cube)
    {
        for (int m = 0; m < arrayListWaypointSpawn.GetLength(0); m++)
        {
            for (int n = 0; n < arrayListWaypointSpawn.GetLength(1); n++)
            {
                if (arrayListWaypointCheck[m, n].Equals(cube.currentWaypoint))
                {
                    if (n + 1 < 7)
                    {
                        if (arrayListWaypointCheck[m, n + 1].colorIndex == arrayListWaypointCheck[m, n].colorIndex)
                        {
                            Cube_StreetCarMinigame2 tmpCube1 = arrayListWaypointCheck[m, n + 1].currentCube;
                            if (!listWaypointSameColor.Contains(tmpCube1))
                            {
                                listWaypointSameColor.Add(tmpCube1);
                            }
                        }
                    }
                    if (m + 1 < 5)
                    {
                        if (arrayListWaypointCheck[m + 1, n].colorIndex == arrayListWaypointCheck[m, n].colorIndex)
                        {
                            Cube_StreetCarMinigame2 tmpCube2 = arrayListWaypointCheck[m + 1, n].currentCube;
                            if (!listWaypointSameColor.Contains(tmpCube2))
                            {
                                listWaypointSameColor.Add(tmpCube2);
                            }
                        }
                    }

                    if (n - 1 >= 0)
                    {
                        if (arrayListWaypointCheck[m, n - 1].colorIndex == arrayListWaypointCheck[m, n].colorIndex)
                        {
                            Cube_StreetCarMinigame2 tmpCube3 = arrayListWaypointCheck[m, n - 1].currentCube;
                            if (!listWaypointSameColor.Contains(tmpCube3))
                            {
                                listWaypointSameColor.Add(tmpCube3);
                            }
                        }
                    }
                    if (m - 1 >= 0)
                    {
                        if (arrayListWaypointCheck[m - 1, n].colorIndex == arrayListWaypointCheck[m, n].colorIndex)
                        {
                            Cube_StreetCarMinigame2 tmpCube4 = arrayListWaypointCheck[m - 1, n].currentCube;
                            if (!listWaypointSameColor.Contains(tmpCube4))
                            {
                                listWaypointSameColor.Add(tmpCube4);
                            }
                        }
                    }
                }
            }
        }
    }

    void AutoCheckColor()
    {
        if (listWaypointSameColor.Count > 1)
        {
            for (int i = 0; i < listWaypointSameColor.Count; i++)
            {
                CheckColor(listWaypointSameColor[i]);
            }
        }
    }

    //void CheckFalling()
    //{
    //    for (int m = arrayListWaypointSpawn.GetLength(0) - 1; m >= 0; m--)
    //    {
    //        for (int n = arrayListWaypointSpawn.GetLength(1) - 1; n >= 0; n--)
    //        {
    //            if (arrayListWaypointCheck[m, n].colorIndex == -1)
    //            {
    //                for (int i = m - 1; i >= 0; i--)
    //                {
    //                    if (arrayListWaypointCheck[i, n].currentCube != null)
    //                    {
    //                        arrayListWaypointCheck[i, n].currentCube.transform.DOMoveY(arrayListWaypointCheck[i + 1, n].transform.position.y, 0.5f);
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
    void DelayResetFallStage()
    {
        isFalling = false;
    }
    void DelayUnActivePeople()
    {
        peopleParent.SetActive(false);
        streetCarObj.transform.DOMoveX(streetCarObj.transform.position.x + 25, 3);
    }

    void DestroyCubeFX(Cube_StreetCarMinigame2 cubeTmp)
    {
        cubeTmp.GetComponent<BoxCollider2D>().enabled = false;
        cubeTmp.GetComponent<SpriteRenderer>().DOFade(0, 0.5f).OnComplete(() =>
        {
            Destroy(cubeTmp.gameObject);                       
            if (listWaypointSameColor.Count > 0)
            {
                listWaypointSameColor.Clear();
            }
        });
    }

    void DestroyCubeSameColor()
    {
        for (int i = 0; i < listWaypointSameColor.Count; i++)
        {
            DestroyCubeFX(listWaypointSameColor[i]);
            countCubeLevel--;
            if (countCubeLevel == 0 && level < 5)
            {
                countCubeLevel = 35;
                isComplete = true;
                level++;
                SetUpLevel();
            }
        }
        if (countCubeLevel == 0 && level == 5)
        {
            isComplete = true;
            isWin = true;
            Debug.Log("Win");
            hammerObj.gameObject.SetActive(false);
            wordUI.SetActive(false);
            tutorial.SetActive(false);
            txtLevel.gameObject.SetActive(false);
            txtPower.gameObject.SetActive(false);
            mainCamera.DOOrthoSize(mainCamera.orthographicSize * 1/0.714f, 1).SetEase(Ease.Linear);

            peopleParent.transform.DOMove(new Vector2(6.59f, -3.98f), 3).OnComplete(() => 
            {
                for (int i = 0; i < peopleParent.transform.childCount; i++)
                {
                    peopleParent.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
                    peopleParent.transform.GetChild(i).DOMove(streetCarObj.transform.position, i * 0.5f + 1).OnComplete(() =>
                    {
                        peopleParent.transform.GetChild(i).GetChild(0).transform.DOKill();
                    });
                    peopleParent.transform.GetChild(i).GetChild(0).transform.DOScale(0, 0.5f).OnComplete(() =>
                    {
                        peopleParent.transform.GetChild(i).GetChild(0).transform.DOScale(0.25f, 0.5f).OnComplete(() =>
                        {
                            peopleParent.transform.GetChild(i).GetChild(0).transform.DOScale(0, 0.5f).OnComplete(() =>
                            {
                                peopleParent.transform.GetChild(i).GetChild(0).transform.DOScale(0.25f, 0.5f);
                            });
                        });
                    });
                }
                Invoke(nameof(DelayUnActivePeople), 1.1f);
            });                
        }      
    }

    private void Update()
    {      
        if (isBegin && !isIntro)
        {           
            mouseCurrentPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseCurrentPos = new Vector2(Mathf.Clamp(mouseCurrentPos.x, -mainCamera.orthographicSize * f2 + 0.4f, mainCamera.orthographicSize * f2 - 1.5f), Mathf.Clamp(mouseCurrentPos.y, -mainCamera.orthographicSize + 0.2f, mainCamera.orthographicSize - 1.2f));
            //hammerObj.transform.DOMove(mouseCurrentPos, 0.1f);
            hammerObj.transform.position = mouseCurrentPos;
            if (Input.GetMouseButtonDown(0) && !isWin && !isLose && !isFalling)
            {                
                hit = Physics2D.RaycastAll(mouseCurrentPos, Vector2.zero);
                if (hit.Length != 0)
                {
                    for (int i = 0; i < hit.Length; i++)
                    {
                        if (hit[i].collider.gameObject.CompareTag("Box") || hit[i].collider.gameObject.CompareTag("People") || hit[i].collider.gameObject.CompareTag("Balloon") || hit[i].collider.gameObject.CompareTag("Tree"))
                        {
                            isFalling = true;
                            Invoke(nameof(DelayResetFallStage), 0.8f);
                            if (isTutorial)
                            {
                                if (tutorial.activeSelf)
                                {
                                    isTutorial = false;
                                    tutorial.SetActive(false);
                                    tutorial.transform.DOKill();
                                }
                            }                                               
                            if (!listWaypointSameColor.Contains(hit[i].collider.GetComponent<Cube_StreetCarMinigame2>()))
                            {                               
                                listWaypointSameColor.Add(hit[i].collider.GetComponent<Cube_StreetCarMinigame2>());
                            }
                            CheckColor(hit[i].collider.GetComponent<Cube_StreetCarMinigame2>());
                            AutoCheckColor();
                            power--;
                            InputText();
                            DestroyCubeSameColor();                           
                            if (power == 0 && !isComplete)
                            {
                                isLose = true;
                                Debug.Log("Lose");
                            }

                        }
                    }
                }
            }
        }
    }
}

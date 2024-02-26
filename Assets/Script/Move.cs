using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    public GameObject Box_Clon;
    public AudioSource ClickAudio;
    public AudioSource ClickAudio2;
    public AudioSource ClickAudio3;
    private Text score;
    public Text HighScoreText;


    private Transform Spawn_point_Z;
    private Transform Spawn_point_X;
    public Camera Cam;

    public Transform Last_Obj;
    private Transform Current_Obj;

    private float Speed = 1.5f;
    private float Speed2;

    private int Count = 0;

    private bool Turn_Z = true;
    private bool front = true;
    private bool back = false;

    private Vector3 terget;

    private int R;
    private int G;
    private int B;
    private int HighScore = 0;


    private bool first = false;
    private bool scoend = false;
    private bool third = false;
    private bool down = false;
    private bool gameOver = false;
    private int low;
    private int high;
    private readonly int ColorRange = 20;


    private void Awake()
    {
        //Debug.Log("Awake");
        gameOver = false;
        R = UnityEngine.Random.Range(1, 255);
        G = UnityEngine.Random.Range(1, 255);
        
        B = UnityEngine.Random.Range(1, 255);
        //Debug.Log(R + " " + G + " " + B);
    }
    // Start is called before the first frame update
    void Start()
    {

        HighScore = PlayerPrefs.GetInt("HighScorePrefs", 0);
        
        //Debug.Log("Start");
        if (R >= G && R >= B)
        {
            //Debug.Log("R is Big");
            //R is Big

            high = R;
            if(G < B)
            {
                low = G;
                //B is Middle Value
                // B ++ goto R
                third = true;
                down = true;
            }
            else
            {
                low = B;
                // G is Middle Value
                // G goto -- B
                scoend = true;
                down = true;

            }
            //low = G < B ? G : B;

        }
        else if(G >= R & G >= B)
        {
            high = G;
            if(R < B)
            {
                low = R;
                // B is Middle
                // B -- goto R
                third = true;
                down = false;


            }
            else
            {
                low = B;
                //R is middle 
                // R ++ goto G
                first = true;
                down = true;
            }
            //low = R < B ? R : B;
            
        }
        else
        {
            high = B;
            if(R < G)
            {
                low = R;
                // G is Middle
                // G ++ goto B
                scoend = true;
                down = false;
            }
            else
            {
                low = G;
                // R is Middle Value
                // R -- got G
                first = true;
                down = false;
            }
            //low = R < G ? R : G;
            //high = B;
        }
        Last_Obj.GetComponent<Renderer>().material.color = new Color(R / 255f, G / 255f, B / 255f);
        Speed2 = Speed;
        terget = Cam.transform.position;
        Spawn_point_Z = GameObject.Find("Z").transform;
        Spawn_point_X = GameObject.Find("X").transform;
        score = GameObject.Find("score").GetComponent<Text>();
        Create_z_Box();
    }




    private void Create_z_Box()
    {
        Count++;
        //Debug.Log(Count);
       
        Turn_Z = true;
        Speed = Speed2;
        GameObject Box_Obj = Instantiate(Box_Clon);
        Box_Obj.transform.localScale = Last_Obj.localScale;
        Box_Obj.transform.position = Spawn_point_Z.position;
        Box_Obj.GetComponent<Renderer>().material.color = CreateColor();
        Current_Obj = Box_Obj.transform;
    }
    private void Create_x_Box()
    {
        Count++;
        Turn_Z = false;
        Speed = Speed2;
        GameObject Box_Obj = Instantiate(Box_Clon);

        Box_Obj.transform.localScale = Last_Obj.localScale;
        Box_Obj.transform.position = Spawn_point_X.position;
        Box_Obj.GetComponent<Renderer>().material.color = CreateColor();
        Current_Obj = Box_Obj.transform;
    }


    private void Update()
    {

        if (Turn_Z == true)
        {

            Move_Z();
        }
        else
        {
            Move_X();
        }

        if (gameOver == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                
                if (Turn_Z)
                {
                    StopZ();
                }
                else
                {
                    StopX();
                }

                if (Count == 3)
                {
                    terget.y += 0.40f;

                }
                else if (Count >= 3)
                {
                    terget.y += 0.20f;

                }

            }
        }
        else if(gameOver == true)
        {
            
            if (Cam.orthographicSize <= Count * 0.317f)
            {
                Debug.Log("Hello");
                Cam.orthographicSize = Cam.orthographicSize + 3f * Time.deltaTime;
            }
            else
            {
                if(HighScore <= Count)
                {
                    PlayerPrefs.SetInt("HighScorePrefs", Count-1);
                }
                
                HighScoreText.text = PlayerPrefs.GetInt("HighScorePrefs").ToString();
                FindObjectOfType<GameManager>().Fnish();
            }
            


        }
        
        Cam.transform.position = Vector3.Lerp(Cam.transform.position, terget, 1f * Time.deltaTime);
    }

    private void StopZ()
    {
        Speed = 0f;
        float Distance = Current_Obj.position.z - Last_Obj.position.z;
        if (Mathf.Abs(Distance) <= 0.03f)
        {
            ClickAudio2.Play();
            Vector3 tergetPos = Last_Obj.position;
            tergetPos.y = Current_Obj.position.y;
            Current_Obj.position = Vector3.Lerp(Current_Obj.position, tergetPos, 1f);
            Spawn_point_X.position = new Vector3(Spawn_point_X.position.x, Current_Obj.position.y + 0.2f, Current_Obj.position.z);
            Last_Obj = Current_Obj;
            score.text = Count.ToString();
            Create_x_Box();


        }
        else if(Mathf.Abs(Distance) >= Last_Obj.localScale.z)
        {
            ClickAudio3.Play();
            gameOver = true;
        }
        else
        {
            ClickAudio.Play();
            float NewScale = Last_Obj.localScale.z - Mathf.Abs(Distance);
            float NewPos = Last_Obj.position.z + (Distance / 2);
            float fallBoxScale = Current_Obj.localScale.z - NewScale;
            Current_Obj.position = new Vector3(Current_Obj.position.x, Current_Obj.position.y, NewPos);
            Current_Obj.localScale = new Vector3(Current_Obj.localScale.x, Current_Obj.localScale.y, NewScale);


            GameObject Fall_Box = Instantiate(Box_Clon);
            Fall_Box.transform.localScale = new Vector3(Current_Obj.localScale.x, Current_Obj.localScale.y, fallBoxScale);

            float fallBool = Current_Obj.position.z > Last_Obj.position.z ? 1 : 0;
            float fallBoxPos;
            if(fallBool == 1)
            {
                fallBoxPos = Current_Obj.position.z + (NewScale / 2) + (fallBoxScale / 2);
                Fall_Box.transform.position = new Vector3(Current_Obj.position.x, Current_Obj.position.y, fallBoxPos);
                //Debug.Log("1");
            }
            else if(fallBool == 0)
            {
                //Debug.Log("0");
                fallBoxPos = Current_Obj.position.z - (NewScale / 2) - (fallBoxScale / 2);
                Fall_Box.transform.position = new Vector3(Current_Obj.position.x, Current_Obj.position.y, fallBoxPos);
            }
            Fall_Box.AddComponent<Rigidbody>();
            Fall_Box.GetComponent<Rigidbody>().mass = 0.1f;
            Fall_Box.GetComponent<Renderer>().material.color = Current_Obj.GetComponent<Renderer>().material.color;
            Spawn_point_X.position = new Vector3(Spawn_point_X.position.x, Current_Obj.position.y + 0.2f, Current_Obj.position.z);

            score.text = Count.ToString();
            Last_Obj = Current_Obj;
            Create_x_Box();

        }
       
    }
    private void StopX()
    {
        Speed = 0f;
        float Distance = Current_Obj.position.x - Last_Obj.position.x;
        if (Mathf.Abs(Distance) <= 0.03f)
        {
            ClickAudio2.Play();
            Vector3 tergetPos = Last_Obj.position;
            tergetPos.y = Current_Obj.position.y;
            Current_Obj.position = Vector3.Lerp(Current_Obj.position, tergetPos, 1f);
            Spawn_point_Z.position = new Vector3(Current_Obj.position.x, Current_Obj.position.y + 0.2f, Spawn_point_Z.position.z);
            score.text = Count.ToString();
            Last_Obj = Current_Obj;
            Create_z_Box();
            //Debug.Log("All");
        }
        else if(Mathf.Abs(Distance) >= Last_Obj.localScale.x)
        {
            ClickAudio3.Play();
            gameOver = true;
            //Debug.Log("GameOver");
        }
        else
        {
            ClickAudio.Play();
            float NewScale = Last_Obj.localScale.x - Mathf.Abs(Distance);
            float NewPos = Last_Obj.position.x + (Distance / 2);
            float fallBoxScale = Current_Obj.localScale.x - NewScale;

            Current_Obj.position = new Vector3(NewPos , Current_Obj.position.y, Current_Obj.position.z );
            Current_Obj.localScale = new Vector3(NewScale , Current_Obj.localScale.y, Current_Obj.localScale.z );

            GameObject Fall_Box = Instantiate(Box_Clon);
            Fall_Box.transform.localScale = new Vector3(fallBoxScale , Current_Obj.localScale.y, Current_Obj.localScale.z);
            float fallBoxPos = 0;
            float fallBool = Current_Obj.position.x > Last_Obj.position.x ? 1 : 0;
            if (fallBool == 1)
            {
                fallBoxPos = Current_Obj.position.x + (NewScale / 2) + (fallBoxScale / 2);
                Fall_Box.transform.position = new Vector3(fallBoxPos, Current_Obj.position.y, Current_Obj.position.z);
            }
            else if (fallBool == 0)
            {
                fallBoxPos = Current_Obj.position.x - (NewScale / 2) - (fallBoxScale / 2);
                Fall_Box.transform.position = new Vector3(fallBoxPos, Current_Obj.position.y, Current_Obj.position.z);
            }
            Fall_Box.AddComponent<Rigidbody>();
            Fall_Box.GetComponent<Rigidbody>().mass = 0.1f;
            Fall_Box.GetComponent<Renderer>().material.color = Current_Obj.GetComponent<Renderer>().material.color;
            Spawn_point_Z.position = new Vector3(Current_Obj.position.x, Current_Obj.position.y + 0.2f, Spawn_point_Z.position.z);
            score.text = Count.ToString();
            Last_Obj = Current_Obj;
            Create_z_Box();

        }

        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    private void Move_Z()
    {
        if (front)
        {
            Current_Obj.Translate(Vector3.forward * Time.deltaTime * Speed);
        }
        else if (back)
        {
            Current_Obj.Translate(Vector3.forward * Time.deltaTime * -Speed);
        }
        if(Current_Obj.position.z <= -2f)
        {
            front = true;
            back = false;
        }
        else if(Current_Obj.position.z >= 2f)
        {
            back = true;
            front = false;
        }
        

    }
    private void Move_X()
    {
        if (front)
        {
            Current_Obj.Translate(Vector3.right * Time.deltaTime * Speed);
        }
        else if (back)
        {
            Current_Obj.Translate(Vector3.right * Time.deltaTime * -Speed);
        }
        if(Current_Obj.position.x <= -2f)
        {
            front = true;
            back = false;
        }
        else if(Current_Obj.position.x >= 2f)
        {
            back = true;
            front = false;
        }
        //Current_Obj.Translate(Vector3.forward * Time.deltaTime * Speed);

    }

    private Color CreateColor()
    {
        if(first)
        {
            if(R <= high && down == true)
            {
                R += ColorRange;
            }
            else if( R >= low && down == false)
            {
                R -= ColorRange;
            }

            else
            {
                if (down)
                {
                    G -= ColorRange;
                }
                else
                {
                    G += ColorRange;
                }
                scoend = true;
                first = false;
            }
        }
        else if (scoend)
        {
            if( G >= low && down == true)
            {
                G -= ColorRange;
            }
            else if(G <= high && down == false)
            {
                G += ColorRange;
            }
            else
            {
                if (down)
                {
                    B += ColorRange;
                }
                else
                {
                    B -= ColorRange;
                }
                third = true;
                scoend = false;
            }
        }
        else if (third)
        {
            if(B <= high && down == true)
            {
                B += ColorRange;
            }
            else if(B >= low && down == false)
            {
                B -= ColorRange;
            }
            else
            {
                first = true;
                third = false;
                if (down)
                {
                    R -= ColorRange;
                    down = false;
                }
                else
                {
                    R += ColorRange;
                    down = true;
                }

            }
        }
        
        return new Color(R / 255f, G / 255f,B / 255f);
    }
   
}

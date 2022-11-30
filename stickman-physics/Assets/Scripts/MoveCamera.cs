using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    public float divX;
    public float divY;

    public static bool overrideCam = false;

    public static float x, y;

    private float speed = 0f;
    private float desiredSpeed;
    private float smooth = 0.4f;
    private float redSpeed = 0f;
    //public Camera camera;

    private static float desiredZoom;
    private float currentZoom;
    private static Vector3 desiredLocation;

    private Vector3 velocity;
    private float zoomVel;
    private static float smoothTime = 0.35f;

    private bool constantSpeed = false;

    public Vector3 offset;
    public float offsetY = 5f;
    private static float minZoom = 10f;
    private static float maxZoom = 25f;
    //private float zoomLimiter = 3f;
    public Camera cam;

    private void Start()
    {
        overrideCam = false;
        desiredSpeed = speed;
        currentZoom = 15f;
        desiredZoom = currentZoom;
    }

    private void FixedUpdate()
    {
        x = transform.position.x;
        y = transform.position.y;

        if (constantSpeed)
        {
            transform.Translate(Vector3.right * speed * Time.fixedDeltaTime);
            if (speed != desiredSpeed)
                speed = Mathf.SmoothDamp(speed, desiredSpeed, ref redSpeed, smooth);
        }
        else
        {
            // if players alive > 0
            Move();
            Zoom();
        }

    }

    public void SetSpeed(float f)
    {
        desiredSpeed = f;
    }

    public static void SetDesiredLocation(Vector3 pos, float zoom, float speed)
    {
        desiredLocation = pos;
        desiredZoom = zoom;
        smoothTime = speed;
    }


    Vector3 GetCenterPoint()
    {
        float averagePointX = 0;
        float averagePointY = 0;
        for (int i = 0; i < GameManager.instance.numPlayers; i++)
        {
            Transform t = GameManager.instance.torsos[i].transform;
            averagePointX += t.position.x;
            averagePointY += t.position.y;
        }
        if (averagePointY < 0f) averagePointY = 0f;
        return new Vector3(averagePointX / 2f, averagePointY / 2f, 0);
    }

    private void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    private void Zoom()
    {
        Vector2 greatestDist = GetGreatestDistance();

        float target = minZoom + (greatestDist.magnitude / 2f);

        //if (target == maxZoom) target = maxZoom;
        if (target > maxZoom) target = maxZoom;
        if (target < minZoom) target = minZoom;

        // want closer to 0 the shorter & taller it is

        float hor = greatestDist.x / divX;
        //Mathf.Clamp(hor, hor, 1f);
        float vert = greatestDist.y / divY;
        //if (greatestDist.y > 10f)
        //{
        //    vert = (greatestDist.y) / divY;
        //}
        //else
        //{
        //    vert = (greatestDist.y * 2) / divY;
        //}
        //Mathf.Clamp(vert, vert, 1f);
        vert = 1 - vert;
        Mathf.Clamp(vert, 0f, 1f);
        Mathf.Clamp(hor, 0f, 1f);

        offset.y = offsetY * hor * vert;

        //if (cam.orthographicSize > maxZoom - 0.1f) offset.y = 0f;
        //offset.y = offsetY/* + GetGreatestDistance() / 12f*/;
        //offset.y = maxZoom - transform.position.y - offsetY;
        //if (target < 15f) offset.y -= (15f - target) * 2f;
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, target, ref zoomVel, smoothTime);
    }

    Vector2 GetGreatestDistance()
    {
        Vector2 topRight = GameManager.instance.torsos[0].transform.position;
        Vector2 bottomLeft = GameManager.instance.torsos[0].transform.position;
        for (int i = 0; i < GameManager.instance.numPlayers; i++)
        {
            Transform t = GameManager.instance.torsos[i].transform;
            // if player isnt dead
            float xPos = t.position.x;
            if (xPos > topRight.x)
            {
                topRight.x = xPos;
            }
            if (xPos < bottomLeft.x)
            {
                bottomLeft.x = xPos;
            }
            float yPos = t.position.y;
            if (yPos > topRight.y)
            {
                topRight.y = yPos;
            }
            if (yPos < bottomLeft.y)
            {
                bottomLeft.y = yPos;
            }
        }
        if (bottomLeft.y < 0f) bottomLeft = new Vector2(bottomLeft.x, 0f);
        //Debug.Log("top right: " + topRight  + "    bot left: " + bottomLeft + "    magnitude: " + (topRight - bottomLeft).magnitude);
        return topRight - bottomLeft;

        //if (GameManager.numPlayers /*players alive*/ == 1) return 0;
        //float max = -1000f; 
        //float min = 1000;

        //for (int i = 0; i < GameManager.numPlayers; i++)
        //{
        //    Transform t = GameManager.torsos[i].transform;
        //    // if player isnt dead
        //    float x1 = t.position.x;
        //    if (x1 > max)
        //    {
        //        max = x1;
        //    }
        //    if (x1 < min)
        //    {
        //        min = x1;
        //    }
        //    float y1 = t.position.y;
        //    if (y1 > max)
        //    {
        //        max = y1;
        //    }
        //    if (y1 < min)
        //    {
        //        min = y1;
        //    }
        //}
        //Debug.Log("max: " + max + "   min: " + min);

        //return max - min;
    }

    public static void SetMinZoom(float f)
    {
        minZoom = f;
    }
}

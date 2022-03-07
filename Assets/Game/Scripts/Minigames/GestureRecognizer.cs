using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Core.Input;


using PDollarGestureRecognizer;

public class GestureRecognizer : MonoBehaviour
{
    public Camera viewCamera;
    public InputActionData Fire;
    public InputActionData Pointer;
    public RectTransform DrawArea;

    public Transform gestureOnScreenPrefab;

    private List<Gesture> trainingSet = new List<Gesture>();

    private List<Point> points = new List<Point>();
    private int strokeId = -1;

    private Vector3 virtualKeyPosition = Vector2.zero;
    private Rect drawArea;

    private RuntimePlatform platform;
    private int vertexCount = 0;

    private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer>();
    private LineRenderer currentGestureLineRenderer;

    //GUI
    private string message;
    private bool recognized;
    private string newGestureName = "";

    public event System.Func<GestureRecognizer, string, float, bool> OnRecognize;

    void Start()
    {
        platform = Application.platform;
        drawArea = DrawArea.rect;//RectTransformToScreenSpace(DrawArea);//new Rect(0, 0, Screen.width - Screen.width / 3, Screen.height);

        //Load pre-made gestures
        TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");
        foreach (TextAsset gestureXml in gesturesXml)
            trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));

        //Load user custom gestures
        string[] filePaths = Directory.GetFiles(Application.persistentDataPath, "*.xml");
        foreach (string filePath in filePaths)
            trainingSet.Add(GestureIO.ReadGestureFromFile(filePath));
    }

    bool inRect = false;
    void Update()
    {
        if (Fire.InputAction().WasReleasedThisFrame() && inRect)
        {
            Recognize();
            AudioHook.StopSFX(SFX.WRITING);
            inRect = false;
            return;
        }

        if (Fire.InputAction().IsPressed())
        {
            virtualKeyPosition = (Pointer.InputAction().ReadValue<Vector2>());

            if (RectTransformUtility.RectangleContainsScreenPoint(DrawArea, virtualKeyPosition, viewCamera))
            {
                if (inRect == false)
                {
                    inRect = true;
                    AudioHook.PlaySFX(SFX.WRITING);
                }
            }
            else if (inRect == true)
            {
                inRect = false;
                AudioHook.StopSFX(SFX.WRITING);
                Recognize();
                return;
            }
        }

        if (inRect)
        {
            if (Fire.InputAction().WasPressedThisFrame())
            {
                ++strokeId;

                Transform tmpGesture = Instantiate(gestureOnScreenPrefab, transform.position, transform.rotation, transform) as Transform;
                currentGestureLineRenderer = tmpGesture.GetComponent<LineRenderer>();

                gestureLinesRenderer.Add(currentGestureLineRenderer);

                vertexCount = 0;
            }

            if (Fire.InputAction().IsPressed())
            {
                points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));

                currentGestureLineRenderer.positionCount = ++vertexCount;
                currentGestureLineRenderer.SetPosition(vertexCount - 1, viewCamera.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10)));
            }
        }

    }


    public void ClearCurrent()
    {
        recognized = false;
        strokeId = -1;

        points.Clear();

        foreach (LineRenderer lineRenderer in gestureLinesRenderer)
        {

            currentGestureLineRenderer.positionCount = 0;
            Destroy(lineRenderer.gameObject);
        }

        gestureLinesRenderer.Clear();
    }

    public void Recognize()
    {
        recognized = true;

        Gesture candidate = new Gesture(points.ToArray());
        Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());

        message = gestureResult.GestureClass + " " + gestureResult.Score;

        if (OnRecognize?.Invoke(this, gestureResult.GestureClass, gestureResult.Score) == true)
        {
            recognized = false;
            strokeId = -1;
            points.Clear();
            currentGestureLineRenderer = null;
            gestureLinesRenderer.Clear();
        }
        else
            ClearCurrent();
    }

    public static Rect RectTransformToScreenSpace(RectTransform transform)
    {
        Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
        Rect rect = new Rect(transform.position.x, Screen.height - transform.position.y, size.x, size.y);
        rect.x -= (transform.pivot.x * size.x);
        rect.y -= ((1.0f - transform.pivot.y) * size.y);
        return rect;
    }
}

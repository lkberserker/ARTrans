using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;


public class FaceDector : MonoBehaviour
{
    WebCamTexture _webCamTexture;
    WebCamTexture _webCamTexture2;
    [SerializeField] public OVRPassthroughLayer passLayer;
    CascadeClassifier cascade;
    OpenCvSharp.Rect Myface;
    // Start is called before the first frame update
    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        _webCamTexture = new WebCamTexture(devices[0].name);
       // _webCamTexture2 = passLayer.
        _webCamTexture.Play();
        cascade = new CascadeClassifier(Application.dataPath + @"\OpenCV+Unity\Demo\Face_Detector\haarcascade_frontalface_default.xml");
        foreach (WebCamDevice d in devices)
        {
            Debug.Log("Device Name++++++++++++++++"+d.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
       // GetComponent<Renderer>().material.mainTexture = _webCamTexture;
        Mat frame = OpenCvSharp.Unity.TextureToMat(_webCamTexture);

        findNewFace(frame);
        display(frame);
    }

    void findNewFace(Mat frame)
    {
        var faces = cascade.DetectMultiScale(frame, 1.1, 2, HaarDetectionType.ScaleImage);
        if (faces.Length >= 1)
        {
            Debug.Log(faces[0].Location);
            Myface = faces[0];
        }
    }
    void display(Mat frame)
    {
        if (Myface != null)
        {
            frame.Rectangle(Myface, new Scalar(250, 0, 0), 2);
        }
        Texture newtexture = OpenCvSharp.Unity.MatToTexture(frame);
        GetComponent<Renderer>().material.mainTexture = newtexture;
    }
}

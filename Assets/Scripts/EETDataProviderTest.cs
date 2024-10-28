using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.WebCam;

public class EETDataProviderTest : MonoBehaviour
{
    [SerializeField]
    private GameObject LeftGazeObject;
    [SerializeField]
    private GameObject RightGazeObject;
    [SerializeField]
    private GameObject CombinedGazeObject;
    [SerializeField]
    private GameObject CameraRelativeCombinedGazeObject;
    [SerializeField]
    private ExtendedEyeGazeDataProvider extendedEyeGazeDataProvider;

    private DateTime timestamp;
    private ExtendedEyeGazeDataProvider.GazeReading gazeReading;
    private StreamWriter gazeDataWriter;

    private VideoCapture m_VideoCapture = null;

    private void Awake()
    {
        // Define the file path for storing gaze data
        var gazeDataPath = Path.Combine(Application.persistentDataPath, "gaze_data.csv");

        // Create the StreamWriter and set it to auto-flush
        gazeDataWriter = new StreamWriter(gazeDataPath);
        gazeDataWriter.AutoFlush = true;

        // Write the header row
        gazeDataWriter.WriteLine("Timestamp,GazeType,EyePositionX,EyePositionY,EyePositionZ,GazeDirectionX,GazeDirectionY,GazeDirectionZ");

        // Start video capture
        StartVideoCapture();
    }

    private void Update()
    {
        timestamp = DateTime.Now;

        // Get and write the gaze data for left eye
        gazeReading = extendedEyeGazeDataProvider.GetWorldSpaceGazeReading(ExtendedEyeGazeDataProvider.GazeType.Left, timestamp);
        ProcessGazeReading(gazeReading, LeftGazeObject, "Left");

        // Get and write the gaze data for right eye
        gazeReading = extendedEyeGazeDataProvider.GetWorldSpaceGazeReading(ExtendedEyeGazeDataProvider.GazeType.Right, timestamp);
        ProcessGazeReading(gazeReading, RightGazeObject, "Right");

        // Get and write the gaze data for combined gaze
        gazeReading = extendedEyeGazeDataProvider.GetWorldSpaceGazeReading(ExtendedEyeGazeDataProvider.GazeType.Combined, timestamp);
        ProcessGazeReading(gazeReading, CombinedGazeObject, "Combined");

        // Get and write the gaze data for combined gaze in camera space
        gazeReading = extendedEyeGazeDataProvider.GetCameraSpaceGazeReading(ExtendedEyeGazeDataProvider.GazeType.Combined, timestamp);
        ProcessGazeReading(gazeReading, CameraRelativeCombinedGazeObject, "CameraRelativeCombined", isCameraSpace: true);
    }

    private void ProcessGazeReading(ExtendedEyeGazeDataProvider.GazeReading gazeReading, GameObject gazeObject, string gazeType, bool isCameraSpace = false)
    {
        if (gazeReading.IsValid)
        {
            Vector3 position = gazeReading.EyePosition + 1.5f * gazeReading.GazeDirection;
            if (isCameraSpace)
            {
                gazeObject.transform.localPosition = position;
            }
            else
            {
                gazeObject.transform.position = position;
            }

            gazeObject.SetActive(true);

            // Write the gaze data to the CSV file
            gazeDataWriter.WriteLine(FormattableString.Invariant(
                $"{timestamp:O},{gazeType},{gazeReading.EyePosition.x},{gazeReading.EyePosition.y},{gazeReading.EyePosition.z},{gazeReading.GazeDirection.x},{gazeReading.GazeDirection.y},{gazeReading.GazeDirection.z}"));
        }
        else
        {
            gazeObject.SetActive(false);
        }
    }

    private void StartVideoCapture()
    {
        Resolution cameraResolution = VideoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        float cameraFramerate = VideoCapture.GetSupportedFrameRatesForResolution(cameraResolution).OrderByDescending((fps) => fps).First();

        VideoCapture.CreateAsync(false, videoCapture =>
        {
            if (videoCapture != null)
            {
                m_VideoCapture = videoCapture;
                Debug.Log("Created VideoCapture Instance!");

                CameraParameters cameraParameters = new CameraParameters
                {
                    hologramOpacity = 0.0f,
                    frameRate = cameraFramerate,
                    cameraResolutionWidth = cameraResolution.width,
                    cameraResolutionHeight = cameraResolution.height,
                    pixelFormat = CapturePixelFormat.BGRA32
                };

                m_VideoCapture.StartVideoModeAsync(cameraParameters, VideoCapture.AudioState.ApplicationAndMicAudio, OnStartedVideoCaptureMode);
            }
            else
            {
                Debug.LogError("Failed to create VideoCapture Instance!");
            }
        });
    }

    private void OnStartedVideoCaptureMode(VideoCapture.VideoCaptureResult result)
    {
        Debug.Log("Started Video Capture Mode!");
        string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        string filename = $"GazeVideo_{timeStamp}.mp4";
        string filepath = Path.Combine(Application.persistentDataPath, filename);
        m_VideoCapture.StartRecordingAsync(filepath, OnStartedRecordingVideo);
    }

    private void OnStartedRecordingVideo(VideoCapture.VideoCaptureResult result)
    {
        if (result.success)
        {
            Debug.Log("Started Recording Video!");
        }
        else
        {
            Debug.LogError("Failed to start video recording.");
        }
    }

    private void OnStoppedRecordingVideo(VideoCapture.VideoCaptureResult result)
    {
        Debug.Log("Stopped Recording Video!");
        m_VideoCapture.StopVideoModeAsync(OnStoppedVideoCaptureMode);
    }

    private void OnStoppedVideoCaptureMode(VideoCapture.VideoCaptureResult result)
    {
        Debug.Log("Stopped Video Capture Mode!");
        m_VideoCapture.Dispose();
        m_VideoCapture = null;
    }

    private void OnDestroy()
    {
        // Stop recording video when the application is closed
        if (m_VideoCapture != null && m_VideoCapture.IsRecording)
        {
            m_VideoCapture.StopRecordingAsync(OnStoppedRecordingVideo);
        }

        // Close the StreamWriter when the object is destroyed
        if (gazeDataWriter != null)
        {
            gazeDataWriter.Close();
        }
    }
}

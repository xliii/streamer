using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using UnityEngine;
using UnityEngine.UI;
using Image = System.Drawing.Image;

public class GIF : MonoBehaviour
{
	private const string basePath = "Assets/Sprites/GIF/";

	public string pictureName;
	[Range(0, 2)]
	public float speed = 1;

	public bool playOnce;

	int frameCountStart;
	RawImage rawImage;
	List<Texture2D> gifFrames = new List<Texture2D>();

	void Awake()
	{
		rawImage = GetComponent<RawImage>();
		if (rawImage == null) return;

		var gifImage = Image.FromFile(basePath + pictureName + ".gif");
		var dimension = new FrameDimension(gifImage.FrameDimensionsList[0]);
		int frameCount = gifImage.GetFrameCount(dimension);
		for (int i = 0; i < frameCount; i++)
		{
			gifImage.SelectActiveFrame(dimension, i);
			var frame = new Bitmap(gifImage.Width, gifImage.Height);
			System.Drawing.Graphics.FromImage(frame).DrawImage(gifImage, Point.Empty);
			var frameTexture = new Texture2D(frame.Width, frame.Height);
			for (int x = 0; x < frame.Width; x++)
				for (int y = 0; y < frame.Height; y++)
				{
					System.Drawing.Color sourceColor = frame.GetPixel(x, y);
					frameTexture.SetPixel(frame.Width - 1 - x, frame.Height - 1 - y, new Color32(sourceColor.R, sourceColor.G, sourceColor.B, sourceColor.A)); // for some reason, x is flipped
				}
			frameTexture.Apply();
			gifFrames.Add(frameTexture);
		}
	}

	void OnEnable()
	{
		rawImage = GetComponent<RawImage>();
		frameCountStart = Time.frameCount;
	}

	void Update()
	{
		if (rawImage == null) return;

		int index = (int) ((Time.frameCount - frameCountStart)*speed) % gifFrames.Count;
		Texture2D currentFrame = gifFrames[index];
		rawImage.texture = currentFrame;

		if (index == gifFrames.Count - 1 && playOnce)
		{
			rawImage = null;
		}
	}	
}
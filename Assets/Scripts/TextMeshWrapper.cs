using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class TextMeshWrapper : MonoBehaviour
{
	private TextMesh textMesh;
	public int charsPerLine = 80;

	public AnimationType animationType = AnimationType.None;
	private AnimationProcessor animationProcessor;

	void Awake ()
	{
		textMesh = GetComponent<TextMesh>();
		animationProcessor = animationType.AddProcessor(gameObject);
	}

	public string text
	{
		get { return textMesh.text; }
		set { SetText(ProcessEmoticons(value)); }
	}

	void SetText(string message)
	{
		if (textMesh == null) return;

		animationProcessor.Animate(textMesh, message);
	}

	private string ProcessEmoticons(string text)
	{
		if (textMesh == null) return "";

		if (string.IsNullOrEmpty(text))
		{
			return "";
		}

		string result = "";
		int length = 0;
		foreach (string word in text.Split(' '))
		{
			if (length > charsPerLine)
			{
				result += "\n";
				length = 0;
			}
			if (!TwitchEmotes.IsEmote(word))
			{
				result += word + " ";
				length += word.Length + 1;
			}
			else
			{
				Vector2 pos = TwitchEmotes.GetPos(word);
				const float offset = 0.1428f;
				result += string.Format("<quad material=1 x={0} y={1} width={2} height={3} size=80/> ", offset * pos.x, offset * pos.y, offset, offset);
				length += 4;
			}
		}

		if (textMesh.GetComponent<MeshRenderer>().materials.Length > 1)
		{
			result += string.Format("<quad material=1 x={0} y={1} width={2} height={3} size=80/> ", 0, 0, 0.00001f, 0.00001f);
		}

		return result;
	}
}

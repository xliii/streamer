using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System;

public class LanguageUtils {

	public static string RemoveDiacritics(string text)
	{
		var normalizedString = text.Normalize(NormalizationForm.FormD);
		var stringBuilder = new StringBuilder();

		foreach (var c in normalizedString)
		{
			var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
			if (unicodeCategory != UnicodeCategory.NonSpacingMark)
			{
				stringBuilder.Append(c);
			}
		}

		return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
	}

	public static string RegexFix(string text)
	{
		return Regex.Replace(text, @"[^0-9a-zA-Z, \-]+", "");
	}

	public static int Levenshtein(string a, string b)
	{
		if (string.IsNullOrEmpty(a))
		{
			if (!string.IsNullOrEmpty(b))
			{
				return b.Length;
			}
			return 0;
		}

		if (string.IsNullOrEmpty(b))
		{
			if (!string.IsNullOrEmpty(a))
			{
				return a.Length;
			}
			return 0;
		}

		int cost;
		int[,] d = new int[a.Length + 1, b.Length + 1];
		int min1;
		int min2;
		int min3;

		for (int i = 0; i <= d.GetUpperBound(0); i += 1)
		{
			d[i, 0] = i;
		}

		for (int i = 0; i <= d.GetUpperBound(1); i += 1)
		{
			d[0, i] = i;
		}

		for (int i = 1; i <= d.GetUpperBound(0); i += 1)
		{
			for (int j = 1; j <= d.GetUpperBound(1); j += 1)
			{
				cost = Convert.ToInt32(!(a[i - 1] == b[j - 1]));

				min1 = d[i - 1, j] + 1;
				min2 = d[i, j - 1] + 1;
				min3 = d[i - 1, j - 1] + cost;
				d[i, j] = Math.Min(Math.Min(min1, min2), min3);
			}
		}

		return d[d.GetUpperBound(0), d.GetUpperBound(1)];

	}
}

using UnityEngine;
using DG.Tweening;

public class EyesFader : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer eyesRenderer;

	[SerializeField]
	private SpriteRenderer pupilsRenderer;

	[SerializeField]
	private SpriteRenderer backEyesRenderer;

	private Tween fadeTween;
	private Tween pupilsFadeTween;
	private Tween backEyesFadeTween;

	private void Reset()
	{
		eyesRenderer = GetComponent<SpriteRenderer>();
	}

	public void FadeIn(float duration)
	{
		fadeTween?.Kill();
		fadeTween = eyesRenderer.DOFade(1f, duration);

		if (pupilsRenderer != null)
		{
			pupilsFadeTween?.Kill();
			pupilsFadeTween = pupilsRenderer.DOFade(1f, duration);
		}

		if (backEyesRenderer != null)
		{
			backEyesFadeTween?.Kill();
			backEyesFadeTween = backEyesRenderer.DOFade(0f, duration);
		}
	}

	public void FadeOut(float duration)
	{
		fadeTween?.Kill();
		fadeTween = eyesRenderer.DOFade(0f, duration);

		if (pupilsRenderer != null)
		{
			pupilsFadeTween?.Kill();
			pupilsFadeTween = pupilsRenderer.DOFade(0f, duration);
		}

		if (backEyesRenderer != null)
		{
			backEyesFadeTween?.Kill();
			backEyesFadeTween = backEyesRenderer.DOFade(1f, duration);
		}
	}

	public void FadeOutTeleport(float duration)
	{
		fadeTween?.Kill();
		fadeTween = eyesRenderer.DOFade(0f, duration);

		if (pupilsRenderer != null)
		{
			pupilsFadeTween?.Kill();
			pupilsFadeTween = pupilsRenderer.DOFade(0f, duration);
		}
	}

	public void SetAlphaInstant(float alpha)
	{
		fadeTween?.Kill();
		Color c = eyesRenderer.color;
		c.a = alpha;
		eyesRenderer.color = c;
	}
}

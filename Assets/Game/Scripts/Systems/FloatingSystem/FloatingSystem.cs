using DG.Tweening;
using UnityEngine;

namespace Game.Systems.FloatingSystem
{
	public class FloatingSystem
	{
		private Floating3DText.Factory floating3DTextFactory;
		private Floating3DTextWithIcon.Factory floating3DTextWithIconFactory;

		private CameraSystem.CameraSystem cameraSystem;

		public FloatingSystem(CameraSystem.CameraSystem cameraSystem,
			Floating3DText.Factory floating3DTextFactory,
			Floating3DTextWithIcon.Factory floating3DTextWithIconFactory)
		{
			this.cameraSystem = cameraSystem;
			this.floating3DTextFactory = floating3DTextFactory;
			this.floating3DTextWithIconFactory = floating3DTextWithIconFactory;
		}

		public void CreateText(Vector3 position, string text, Color? color = null, AnimationType type = AnimationType.Add)
		{
			var obj = Create();

			ApplyAnimation(obj, type);

			Floating3DText Create()
			{
				var item = floating3DTextFactory.Create();

				item.SetFade(1f);
				item.Text.color = color ?? Color.white;
				item.Text.text = text;
				item.transform.position = position;
				item.transform.rotation = cameraSystem.Rotation;

				return item;
			}
		}

		public void CreateText(Vector3 position, string text, Sprite icon, Color? color = null, AnimationType type = AnimationType.Add)
		{
			var obj = Create();

			ApplyAnimation(obj, type);

			Floating3DTextWithIcon Create()
			{
				var item = floating3DTextWithIconFactory.Create();

				item.SetFade(1f);
				item.Text.color = color ?? Color.white;
				item.Text.text = text;
				item.Icon.sprite = icon;
				item.transform.position = position;
				item.transform.rotation = cameraSystem.Rotation;

				return item;
			}
		}

		private void ApplyAnimation(FloatingPoolableObject floatingObject, AnimationType type)
		{
			Vector3 position = floatingObject.transform.position;

			switch (type)
			{
				case AnimationType.Add:
				{
					Sequence sequence = DOTween.Sequence();
					position.y += 2;

					sequence
						.Append(floatingObject.transform.DOMove(position, 0.5f).SetEase(Ease.OutQuint))
						.Append(floatingObject.Fade(0f, 0.7f))
						.AppendCallback(floatingObject.DespawnIt);
					break;
				}
				//case AnimationType.AdvanceDamage:
				//{
				//	floatingObject.transform.localScale = Vector3.zero;

				//	float offsetX = Random.Range(-1.5f, 1.5f);
				//	if (Mathf.Abs(offsetX) <= 0.2f) offsetX += Mathf.Sign(offsetX) * 0.1f;

				//	floatingObject.transform.position += Vector3.up;

				//	Vector3 endPosition = floatingObject.transform.position + (Vector3.right * offsetX) + (Vector3.up * Random.Range(0f, 1f));

				//	Sequence sequence = DOTween.Sequence();

				//	sequence
				//		.Append(floatingObject.transform.DOJump(endPosition, Random.Range(0.5f, 1.5f), 1, 1f))
				//		.Join(floatingObject.transform.DOScale(1f, 0.5f))
				//		.Append(floatingObject.Fade(0, 0.5f))
				//		.AppendCallback(floatingObject.DespawnIt);
				//	break;
				//}
				//case AnimationType.AddGold:
				//{
				//	Sequence sequence = DOTween.Sequence();
				//	position.y -= Random.Range(50, 125);
				//	sequence
				//		.Append(floatingObject.transform.DOMove(position, 0.5f).SetEase(Ease.OutQuint))
				//		.Append(floatingObject.Fade(0f, 0.7f))
				//		.AppendCallback(floatingObject.DespawnIt);
				//	break;
				//}
				default:
				{
					Sequence sequence = DOTween.Sequence();
					sequence
						.AppendInterval(1f)
						.AppendCallback(floatingObject.DespawnIt);
					break;
				}
			}
		}
	}

	public enum AnimationType
	{
		None,
		Add,
	}
}
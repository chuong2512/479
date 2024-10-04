using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Brick : MonoBehaviour
{
	private sealed class _Shake_d__9 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int __1__state;

		private object __2__current;

		public Brick __4__this;

		private Vector2 _randomVec_5__2;

		object IEnumerator<object>.Current
		{
			get
			{
				return this.__2__current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this.__2__current;
			}
		}

		public _Shake_d__9(int __1__state)
		{
			this.__1__state = __1__state;
		}

		void IDisposable.Dispose()
		{
		}

		bool IEnumerator.MoveNext()
		{
			int num = this.__1__state;
			Brick brick = this.__4__this;
			if (num == 0)
			{
				this.__1__state = -1;
				this._randomVec_5__2 = new Vector2(UnityEngine.Random.Range(-0.01f, 0.01f), UnityEngine.Random.Range(-0.01f, 0.01f));
				brick.transform.position += (Vector3)this._randomVec_5__2;
				this.__2__current = new WaitForSeconds(0.1f);
				this.__1__state = 1;
				return true;
			}
			if (num != 1)
			{
				return false;
			}
			this.__1__state = -1;
			brick.transform.position -= (Vector3)this._randomVec_5__2;
			return false;
		}

		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	public int strength = 1;

	public GameObject solid;

	public GameObject[] cracks;

	public GameObject shatterAnim;

	private int hp;

	private void Start()
	{
		base.transform.localScale = new Vector3(0.66f, 0.66f, 0.66f);
		this.hp = this.strength;
		Game.NextMove += new Action(this.MoveUp);
		Game.OnContinue += new Action(this.MoveDown);
		Game.OnRestart += new Action(this.RemoveMe);
		if (this.shatterAnim != null)
		{
			this.shatterAnim.transform.Rotate(new Vector3(0f, (float)UnityEngine.Random.Range(0, 180), 0f));
		}
	}

	private void RemoveMe()
	{
		Game.NextMove -= new Action(this.MoveUp);
		Game.OnRestart -= new Action(this.RemoveMe);
		Game.OnContinue -= new Action(this.MoveDown);
		this.shatterAnim.gameObject.SetActive(true);
		this.shatterAnim.transform.parent = null;
		UnityEngine.Object.Destroy(base.gameObject);
		UnityEngine.Object.Destroy(this.shatterAnim, 3f);
	}

	private void Update()
	{
		Vector3 eulers = Vector3.forward * Time.deltaTime * -10f;
		if (this.solid != null)
		{
			this.solid.transform.Rotate(eulers);
		}
		GameObject[] array = this.cracks;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].transform.Rotate(eulers);
		}
		if (Game.TopLineY - base.transform.position.y <= this.GetMoveHeight())
		{
			base.StartCoroutine(this.Shake());
		}
	}

	private float GetMoveHeight()
	{
		return base.transform.localScale.y * 1.5f;
	}

//	[IteratorStateMachine(typeof(Brick._003CShake_003Ed__9))]
	private IEnumerator Shake()
	{
		//int num = 0;
		Vector2 v = Vector2.zero;
		//while (num == 0)
		{
			v = new Vector2(UnityEngine.Random.Range(-0.01f, 0.01f), UnityEngine.Random.Range(-0.01f, 0.01f));
			base.transform.position += (Vector3)v;
			yield return new WaitForSeconds(0.1f);
		}
		//if (num != 1)
		{
			//yield break;
		}
		base.transform.position -= (Vector3)v;
		yield break;
	}

	public void MoveUp()
	{
		int num = (int)((Game.TopLineY - base.transform.position.y) / this.GetMoveHeight());
		base.transform.DOMoveY(base.transform.position.y + this.GetMoveHeight(), 0.5f, false).SetEase(Ease.InOutSine).SetDelay(0.05f * (float)num).OnComplete(delegate
		{
			if (base.transform.position.y >= Game.TopLineY)
			{
				this.RemoveMe();
				Game.Instance.GameOverUI();
			}
		});
	}

	private void MoveDown()
	{
		if (base.transform != null)
		{
			float num = 3f;
			int num2 = (int)((Game.TopLineY - base.transform.position.y) / this.GetMoveHeight());
			base.transform.DOMoveY(base.transform.position.y - this.GetMoveHeight() * num, 0.5f, false).SetEase(Ease.InOutSine).SetDelay(0.05f * (float)num2);
		}
	}

	private void CrackTheBall()
	{
		base.StartCoroutine(this.Shake());
		Vector3 eulers = new Vector3((float)UnityEngine.Random.Range(10, 180), (float)UnityEngine.Random.Range(10, 180), (float)UnityEngine.Random.Range(10, 180));
		GameObject[] array = this.cracks;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = array[i];
			this.solid.SetActive(false);
			gameObject.SetActive(false);
			if (this.hp == this.strength - 1)
			{
				gameObject.transform.Rotate(eulers);
			}
		}
		if (this.strength >= 2 && this.hp == 1)
		{
			this.cracks[0].SetActive(true);
		}
		if (this.strength >= 3 && this.hp == 2)
		{
			this.cracks[1].SetActive(true);
		}
		if (this.strength >= 4 && this.hp == 3)
		{
			this.cracks[2].SetActive(true);
		}
		if (this.strength >= 5 && this.hp == 4)
		{
			this.cracks[3].SetActive(true);
		}
	}

	private void OnCollisionEnter(Collision col)
	{
		this.hp--;
		if (this.strength > 1)
		{
			this.CrackTheBall();
		}
		if ((float)this.hp <= 0.05f)
		{
			this.RemoveMe();
		}
	}

	private void OnTriggerEnter(Collider col)
	{
		if (col.name == "Line")
		{
			base.StartCoroutine(this.Shake());
		}
	}
}

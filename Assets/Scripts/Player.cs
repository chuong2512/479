using System;
using UnityEngine;

public class Player : MonoBehaviour
{
	[HideInInspector]
	public Rigidbody rb;

	[HideInInspector]
	public TrailRenderer tr;

	private void Start()
	{
		this.tr = base.GetComponent<TrailRenderer>();
		this.rb = base.GetComponent<Rigidbody>();
		Game.Instance.endOfTurn = true;
	}

	private void Update()
	{
		if (base.transform.position.y < Game.BottomLineY)
		{
			Game.Instance.Finish();
			base.gameObject.SetActive(false);
		}
		if (base.transform.position.y < Game.TopLineY && base.transform.position.y > Game.BottomLineY)
		{
			this.tr.emitting = true;
		}
		else
		{
			this.tr.emitting = false;
		}
		if (this.rb.IsSleeping())
		{
			this.rb.AddForce(Vector3.up * 0.05f);
		}
	}

	private void OnCollisionEnter(Collision col)
	{
		Vector3 a = base.transform.position - col.collider.transform.position;
		a.Normalize();
		float num = 0.001f;
		if (col.collider.name == "Wall")
		{
			num *= 10f;
		}
		this.rb.AddForce(a * num);
	}
}

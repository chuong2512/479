using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
	private sealed class __c__DisplayClass50_0
	{
		public Vector2 r;

		internal bool _SpawnSphereTier_b__0(Vector2 v)
		{
			return v == this.r;
		}
	}

	private sealed class _SpawnBall_d__52 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int __1__state;

		private object __2__current;

		public int i;

		public Game __4__this;

		public Vector3 spawnPos;

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

		public _SpawnBall_d__52(int __1__state)
		{
			this.__1__state = __1__state;
		}

		void IDisposable.Dispose()
		{
		}

		bool IEnumerator.MoveNext()
		{
			int num = this.__1__state;
			Game game = this.__4__this;
			if (num == 0)
			{
				this.__1__state = -1;
				this.__2__current = new WaitForSeconds(0.3f * (float)this.i);
				this.__1__state = 1;
				return true;
			}
			if (num != 1)
			{
				return false;
			}
			this.__1__state = -1;
			Player expr_58 = game.balls[this.i].GetComponent<Player>();
			expr_58.gameObject.SetActive(true);
			expr_58.rb.velocity = Vector3.zero;
			expr_58.rb.angularVelocity = Vector3.zero;
			expr_58.gameObject.transform.position = new Vector3(this.spawnPos.x, 7f, 0f);
			return false;
		}

		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[Serializable]
	private sealed class __c
	{
		public static readonly Game.__c __9 = new Game.__c();

		public static Func<GameObject, bool> __9__53_0;

		internal bool _CheckAllBallsUnderScreen_b__53_0(GameObject b)
		{
			return b.transform.position.y < Game.BottomLineY;
		}
	}

	private const string saveName = "build10";

	public static Game Instance;







	public static float TopLineY = 4.665f;

	public static float BottomLineY = -7f;

	public static float MinimumX = -2.4f;

	public static float MaximumX = 2.4f;

	public GameObject menu;

	public GameObject continueBtn;

	public Text txt;

	public GameObject[] balls;

	public MeshRenderer bg;

	public GameObject[] particles;

	public MeshRenderer wall1;

	public MeshRenderer wall2;

	public Material[] bgList;

	public Material[] wallList;

	public GameObject line;

	public InputField levelInput;

	public int level = 1;

	public bool endOfTurn;

	private int randomSeed = 1;

	private int moveNumber;

	public static event Action NextMove;

	public static event Action OnRestart;

	public static event Action OnContinue;

	public int NumBalls
	{
		get
		{
			if (this.level < 10)
			{
				return 1;
			}
			if (this.level < 20)
			{
				return 2;
			}
			if (this.level < 100)
			{
				return 3;
			}
			if (this.level < 200)
			{
				return 4;
			}
			if (this.level < 300)
			{
				return 5;
			}
			if (this.level < 400)
			{
				return 6;
			}
			if (this.level < 500)
			{
				return 7;
			}
			if (this.level < 600)
			{
				return 8;
			}
			if (this.level < 700)
			{
				return 9;
			}
			return 10;
		}
	}

	public int MoveNumber
	{
		get
		{
			return this.moveNumber;
		}
		set
		{
			Game.NextMove();
			this.moveNumber = value;
		}
	}

	private void Awake()
	{
		Game.Instance = this;
		Time.timeScale = 2f;
		this.level = PlayerPrefs.GetInt("build10", 1);
		this.LoadLevel(this.level);
	}

	private void Start()
	{
        //PluginManager.StartPluginManager();
        //PluginManager.PluginManagerIsReadyEvent += new Action(this.CacheVideo);
        //PluginManager.VideoWatchedWithSuccessEvent += new Action<int>(this.ContinueVideoWatchedSuccess);
        Application.targetFrameRate = 60;
	}

	private void CacheVideo()
	{
		//PluginManager.IsAnyVideoAdAvaileable("VIDEO");
	}

	private void ShowContinueButtonIfVideoAvailable()
	{
        /*
		if (PluginManager.IsAnyVideoAdAvaileable("VIDEO"))
		{
			this.continueBtn.SetActive(true);
			return;
		}
		this.continueBtn.SetActive(false);
		*/      
	}

	public void ContinueForVideo()
	{
		//PluginManager.ShowVideoForCoin("VIDEO");
	}

	private void ContinueVideoWatchedSuccess(int points)
	{
		this.ContinueCallback();
	}

	internal void GameOverUI()
	{
		this.txt.text = "GAME OVER!";
		this.menu.SetActive(true);
		this.ShowContinueButtonIfVideoAvailable();
        AdsControl.Instance.showAds();
	}

	public void Restart()
	{
		if (this.levelInput.text.Length > 0)
		{
			int.TryParse(this.levelInput.text, out this.level);
		}
		if (Game.OnRestart != null)
		{
			Game.OnRestart();
		}
		this.txt.text = "Level " + this.level;
		this.GenerateNewLevel(this.level);
	}

	public void Continue()
	{
		this.ContinueForVideo();
	}

	public void ContinueCallback()
	{
		this.txt.text = "Level " + this.level;
		if (Game.OnContinue != null)
		{
			Game.OnContinue();
		}
		if (this.NoBricksLeft())
		{
			this.EndOfLevel();
		}
	}

	private void LoadLevel(int num)
	{
		this.GenerateNewLevel(this.level);
		this.txt.text = "Level " + this.level;
	}

	internal void EndOfLevel()
	{
		this.level++;
		this.LoadLevel(this.level);
		PlayerPrefs.SetInt("build10", this.level);
	}

	private Vector2 GetRandomPoint(Vector2 min, Vector2 max)
	{
		double arg_28_0 = (double)UnityEngine.Random.Range(min.x, max.x);
		float num = UnityEngine.Random.Range(min.y, max.y);
		double arg_31_0 = arg_28_0 * (double)1.5f;
		num *= 1.5f;
		float arg_48_0 = (float)Math.Round(arg_31_0, 0);
		num = (float)Math.Round((double)num, 0);
		float arg_52_0 = arg_48_0 / 1.5f;
		num /= 1.5f;
		return new Vector2(arg_52_0, num);
	}

	private void GenerateNewLevel(int _level)
	{
		int num = (_level < 1000) ? _level : 1000;
		UnityEngine.Random.InitState(num);
		List<Vector2> listOfPoints = new List<Vector2>();
		int num2 = (num < 20) ? 0 : ((int)Math.Round(Math.Log((double)num - 18.0)));
		int glasses = (num < 10) ? 0 : ((int)Math.Round(Math.Log((double)num - 8.0) * 2.0));
		int glasses2 = (num < 5) ? 0 : ((int)Math.Round(Math.Log((double)num - 3.0) * 3.0));
		int glasses3 = (num < 3) ? 0 : ((int)Math.Round(Math.Log((double)num - 1.0) * 4.0));
		int num3 = (num < 10) ? 5 : ((int)Math.Round(Math.Log((double)num + 1.0) * 5.0));
		if (num < 10 && num >= 5)
		{
			glasses2 = 1;
		}
		if (num < 10 && num >= 3)
		{
			glasses3 = 3;
		}
		if (num >= 100)
		{
			num2 = 5 + (int)Math.Round((double)num / 50.0);
			num3 -= num2;
		}
		Vector2 max = new Vector2(Game.MaximumX, Game.TopLineY - 1f);
		Vector2 min = new Vector2(Game.MinimumX, Game.BottomLineY);
		this.SpawnSphereTier(listOfPoints, max, min, num3, "Glass");
		max = new Vector2(Game.MaximumX, Game.TopLineY - 2f);
		this.SpawnSphereTier(listOfPoints, max, min, glasses3, "Yellow");
		max = new Vector2(Game.MaximumX, Game.TopLineY - 3f);
		this.SpawnSphereTier(listOfPoints, max, min, glasses2, "Green");
		max = new Vector2(Game.MaximumX, Game.TopLineY - 4f);
		this.SpawnSphereTier(listOfPoints, max, min, glasses, "Blue");
		max = new Vector2(Game.MaximumX, Game.TopLineY - 5f);
		this.SpawnSphereTier(listOfPoints, max, min, num2, "Red");
		this.SetBackground();
	}

	private void SpawnSphereTier(List<Vector2> listOfPoints, Vector2 max, Vector2 min, int glasses, string prefabName)
	{
		for (int i = 0; i < glasses; i++)
		{
			Vector2 r = this.GetRandomPoint(min, max);
			while (listOfPoints.Exists((Vector2 v) => v == r))
			{
				r = this.GetRandomPoint(min, max);
			}
			listOfPoints.Add(r);
			UnityEngine.Object.Instantiate(Resources.Load(prefabName), r, Quaternion.Euler(90f, 0f, 0f));
		}
	}

	private void Update()
	{
		Vector3 mousePosition = UnityEngine.Input.mousePosition;
		Vector3 vector = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.transform.position.z * -1f));
		if (vector.x < Game.MinimumX)
		{
			vector = new Vector3(Game.MinimumX - 0.3f, vector.y, vector.z);
		}
		if (vector.x > Game.MaximumX)
		{
			vector = new Vector3(Game.MaximumX + 0.3f, vector.y, vector.z);
		}
		if (Input.GetMouseButtonDown(0) && !this.menu.activeSelf)
		{
			if (this.endOfTurn)
			{
				for (int i = 0; i < this.NumBalls; i++)
				{
                   
					base.StartCoroutine(this.SpawnBall(vector, i));
				}
			}
			this.endOfTurn = false;
		}
	}

//	[IteratorStateMachine(typeof(Game._003CSpawnBall_003Ed__52))]
	private IEnumerator SpawnBall(Vector3 spawnPos, int i)
	{
		int num = 0;
		//while (num == 0)
		{
			yield return new WaitForSeconds(0.3f * (float)i);
		}
		//if (num != 1)
		{
		//yield break;
		}
		Player expr_58 = this.balls[i].GetComponent<Player>();
		expr_58.gameObject.SetActive(true);
		expr_58.rb.velocity = Vector3.zero;
		expr_58.rb.angularVelocity = Vector3.zero;
		expr_58.gameObject.transform.position = new Vector3(spawnPos.x, 7f, 0f);
		yield break;
	}

	public bool CheckAllBallsUnderScreen()
	{
		IEnumerable<GameObject> arg_25_0 = this.balls;
		Func<GameObject, bool> arg_25_1;
		if ((arg_25_1 = Game.__c.__9__53_0) == null)
		{
			arg_25_1 = (Game.__c.__9__53_0 = new Func<GameObject, bool>(Game.__c.__9._CheckAllBallsUnderScreen_b__53_0));
		}
		return arg_25_0.All(arg_25_1);
	}

	private bool NoBricksLeft()
	{
		return GameObject.FindGameObjectsWithTag("Brick").Length == 0;
	}

	public void Finish()
	{
		if (this.CheckAllBallsUnderScreen() && !this.endOfTurn)
		{
			this.endOfTurn = true;
			if (this.NoBricksLeft())
			{
				this.EndOfLevel();
				return;
			}
			int num = this.MoveNumber;
			this.MoveNumber = num + 1;
		}
	}

	private void SetBackground()
	{
		int num;
		if (this.level < 50)
		{
			num = (int)Math.Floor(this.level / 10m);
			this.bg.material = this.bgList[num];
			this.wall1.material = this.wallList[num];
			this.wall2.material = this.wallList[num];
		}
		else
		{
			num = (this.level - 50 * (this.level / 50)) / 10;
			this.bg.material = this.bgList[num];
			this.wall1.material = this.wallList[num];
			this.wall2.material = this.wallList[num];
		}
		GameObject[] array = this.particles;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
		if (num == 4)
		{
			num = 1;
		}
		this.particles[num].SetActive(true);
	}
}

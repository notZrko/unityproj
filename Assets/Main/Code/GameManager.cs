using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // the most scuffed game loop
    // will never look back at this ever again

    public GameObject player;

    public GameObject[] spawn_points;
    GameObject current_point;
    int index;

    public int kills = 0;

    public int wave_index = 0;
    public int enemies_in_wave;
    private bool wave_in_progress = false;
    private int wave_countdown = 5; // time inbetween waves

    public Wave[] wave;

    public TextMeshProUGUI  wave_label;
    private Animator text_animator;
    public TextMeshProUGUI boss_label;
    public TextMeshProUGUI kills_label;


    void Start()
    {
        text_animator = wave_label.GetComponent<Animator>();

        StartCoroutine(StartWave());
    }

    private void Update()
    {
        kills_label.text = kills.ToString();

        if (wave_in_progress == true && enemies_in_wave <= 0)
        {
            wave_in_progress = false;
            StartCoroutine(StartWave());
        }

        if (wave[wave_index].is_boss == true)
        {
            var boss = wave[wave_index].enemies[0].GetComponent<Enemy>();
            boss_label.text = "BOSS // " + boss.health.ToString() + "/" + boss.max_health.ToString();
        }
        else
        {
            boss_label.text = " ";
        }
    }

    public void NewWave()
    {
        print("new wave");

        for (int i = 0; i < wave[wave_index].enemies.Length; i++)
        {
            // random spawn pos
            spawn_points = GameObject.FindGameObjectsWithTag("point");
            index = Random.Range(0, spawn_points.Length);
            current_point = spawn_points[index];

            var new_enemy = (GameObject) Instantiate(wave[wave_index].enemies[i], new Vector2(current_point.transform.position.x, current_point.transform.position.y), Quaternion.identity);

            var component = new_enemy.GetComponent<Enemy>();
            component.target = player;

            wave_in_progress = true;
            print("round started with" + enemies_in_wave.ToString() + " enemies");
        }
    }

    public IEnumerator StartWave()
    {
        wave_index += 1;
        wave_label.text = "Wave #" + wave_index.ToString();
        text_animator.SetBool("Start", true);

        yield return new WaitForSeconds(wave_countdown);
        text_animator.SetBool("Start", false);
        NewWave();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
    }
}

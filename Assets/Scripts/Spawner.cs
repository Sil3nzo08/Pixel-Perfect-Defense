using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Diagnostics.Tracing;

public class Spawner : MonoBehaviour
{
    // Round related
    public static bool roundEnded = true;
    public float timeBetweenRounds = 5.0f;
    public float currentTimeDifference;
    public int waveNum = 0;
    public GameObject spawnPoint;
    public TMP_Text cashText;
    public TMP_Text waveText;
    public static bool finalWaveEnded = false;

    private void Start()
    {
        UIManager.instance.startChallenge += SpawnEndless;
    }

    private void Update()
    {
        if (currentTimeDifference >= timeBetweenRounds)
        {
            waveNum++;
            roundEnded = false;
            currentTimeDifference = 0f;
            UpdateUI(false);
            SpawnWave(waveNum);
        }

        if (roundEnded == true && waveNum != 40)
        {
            currentTimeDifference += Time.deltaTime;
            UpdateUI(true);
        }
        else if (roundEnded == true && finalWaveEnded == true && waveNum == 40)
        {
            UIManager.instance.DisplayCongratulations();
            roundEnded = false;
        }

        // AdminSpawn(); // Admin commands
    }

    private void SpawnEndless()
    {
        waveNum++;
        UpdateUI(true);

        StartCoroutine(SpawnWave41());
    }

    private void SpawnWave(int waveNum)
    {
        switch (waveNum)
        {
            case 1: StartCoroutine(SpawnWave1()); break;
            case 2: StartCoroutine(SpawnWave2()); break;
            case 3: StartCoroutine(SpawnWave3()); break;
            case 4: StartCoroutine(SpawnWave4()); break;
            case 5: StartCoroutine(SpawnWave5()); break;
            case 6: StartCoroutine(SpawnWave6()); break;
            case 7: StartCoroutine(SpawnWave7()); break;
            case 8: StartCoroutine(SpawnWave8()); break;
            case 9: StartCoroutine(SpawnWave9()); break;
            case 10: StartCoroutine(SpawnWave10()); break;
            case 11: StartCoroutine(SpawnWave11()); break;
            case 12: StartCoroutine(SpawnWave12()); break;
            case 13: StartCoroutine(SpawnWave13()); break;
            case 14: StartCoroutine(SpawnWave14()); break;
            case 15: StartCoroutine(SpawnWave15()); break;
            case 16: StartCoroutine(SpawnWave16()); break;
            case 17: StartCoroutine(SpawnWave17()); break;
            case 18: StartCoroutine(SpawnWave18()); break;
            case 19: StartCoroutine(SpawnWave19()); break;
            case 20: StartCoroutine(SpawnWave20()); break;
            case 21: StartCoroutine(SpawnWave21()); break;
            case 22: StartCoroutine(SpawnWave22()); break;
            case 23: StartCoroutine(SpawnWave23()); break;
            case 24: StartCoroutine(SpawnWave24()); break;
            case 25: StartCoroutine(SpawnWave25()); break;
            case 26: StartCoroutine(SpawnWave26()); break;
            case 27: StartCoroutine(SpawnWave27()); break;
            case 28: StartCoroutine(SpawnWave28()); break;
            case 29: StartCoroutine(SpawnWave29()); break;
            case 30: StartCoroutine(SpawnWave30()); break;
            case 31: StartCoroutine(SpawnWave31()); break;
            case 32: StartCoroutine(SpawnWave32()); break;
            case 33: StartCoroutine(SpawnWave33()); break;
            case 34: StartCoroutine(SpawnWave34()); break;
            case 35: StartCoroutine(SpawnWave35()); break;
            case 36: StartCoroutine(SpawnWave36()); break;
            case 37: StartCoroutine(SpawnWave37()); break;
            case 38: StartCoroutine(SpawnWave38()); break;
            case 39: StartCoroutine(SpawnWave39()); break;
            case 40: StartCoroutine(SpawnWave40()); break;
            default:
                StartCoroutine(SpawnWave1());
                break;
        }
    }

    private void SpawnEnemy(int enemyID)
   {
        GameObject newEnemy = Instantiate(EnemyDatabase.instance.enemies[enemyID]);
        newEnemy.transform.position = spawnPoint.transform.position;
   }

   private void UpdateUI(bool inIntermission)
   {
        if (inIntermission)
        {
            int timeTillNextRound = Mathf.CeilToInt(timeBetweenRounds - currentTimeDifference);
            waveText.text = "Next Round in " + timeTillNextRound;
        }
        else
        {
            waveText.text = "Wave " + waveNum;
        }
   }

   private IEnumerator RepeatSpawn(int enemyID, float timeBetweenSpawns, int totalAmount, bool lastSection)
   {
        for (int index = 1; index <= totalAmount; index++)
        {
            SpawnEnemy(enemyID);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        if (lastSection == true)
        {
            roundEnded = true;
        }
   }

    // ================= Waves =================
    /* == Enemy indexes ==
        1. Basic
        2. Fast
        3. Armoured
        4. Boss1
        5. Anchor
        6. Sprinter
        7. Randomizer
        8. Ninja
        9. Enforcer
        10. Devotee
        11. DeathBringer
        12. Overseer
        13. Dark Champion
        14. Signal Disrupter
    */ 
    private IEnumerator SpawnWave1()
    {
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(RepeatSpawn(1, 2f, 10, true));
    }

    private IEnumerator SpawnWave2()
    {
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(RepeatSpawn(1, 1.5f, 5, false));
        yield return new WaitForSeconds(7.5f);
        StartCoroutine(RepeatSpawn(2, 1, 5, true));
    }

    private IEnumerator SpawnWave3()
    {
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(RepeatSpawn(1, 2f, 20, false));
        yield return new WaitForSeconds(16f);
        StartCoroutine(RepeatSpawn(2, 1.5f, 20, true));
    }

    private IEnumerator SpawnWave4()
    {
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(RepeatSpawn(3, 2f, 10, true));
    }

    private IEnumerator SpawnWave5()
    {
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(RepeatSpawn(3, 0.7f, 4, false));
        yield return new WaitForSeconds(2.8f);
        StartCoroutine(RepeatSpawn(1, 0.5f, 10, false));
        yield return new WaitForSeconds(2.2f);
        StartCoroutine(RepeatSpawn(3, 0.7f, 4, true));
    }

    private IEnumerator SpawnWave6()
    {
        StartCoroutine(RepeatSpawn(1, 0.7f, 30, false));
        yield return new WaitForSeconds(21f);
        StartCoroutine(RepeatSpawn(2, 0.7f, 30, false));
        yield return new WaitForSeconds(21f);
        StartCoroutine(RepeatSpawn(3, 1f, 10, true));
    }

    private IEnumerator SpawnWave7()
    {
        StartCoroutine(RepeatSpawn(3, 1f, 20, false));
        yield return new WaitForSeconds(3f);
        StartCoroutine(RepeatSpawn(2, 1f, 50, true));
    }
    private IEnumerator SpawnWave8()
    {
        StartCoroutine(RepeatSpawn(3, 2f, 10, false));
        yield return new WaitForSeconds(23f);
        StartCoroutine(RepeatSpawn(4, 5f, 1, true));

    }

    private IEnumerator SpawnWave9()
    {
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(RepeatSpawn(6, 2f, 5, false));
        StartCoroutine(RepeatSpawn(3, 1f, 10, false));
        StartCoroutine(RepeatSpawn(2, 0.3f, 10, true));
    }

    private IEnumerator SpawnWave10()
    {
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(RepeatSpawn(9, 1.4f, 15, true));
    }

    private IEnumerator SpawnWave11()
    {
        StartCoroutine(RepeatSpawn(4, 10f, 4, false));
        yield return new WaitForSeconds(1f);
        StartCoroutine(RepeatSpawn(6, 0.8f, 40, true));
    }

    private IEnumerator SpawnWave12()
    {
        StartCoroutine(RepeatSpawn(9, 0.3f, 20, false));
        yield return new WaitForSeconds(10f);
        StartCoroutine(RepeatSpawn(5, 0.3f, 1, false));
        yield return new WaitForSeconds(4f);
        StartCoroutine(RepeatSpawn(9, 0.3f, 20, true));
    }

    private IEnumerator SpawnWave13()
    {
        StartCoroutine(RepeatSpawn(3, 0.5f, 50, false));
        yield return new WaitForSeconds(28f);
        StartCoroutine(RepeatSpawn(4, 5f, 10, false));
        yield return new WaitForSeconds(15.5f);
        StartCoroutine(RepeatSpawn(9, 2f, 20, false));
        StartCoroutine(RepeatSpawn(6, 1f, 20, true));
    }

    private IEnumerator SpawnWave14()
    {
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(RepeatSpawn(5, 5f, 40, true));
    }

    private IEnumerator SpawnWave15()
    {
        StartCoroutine(RepeatSpawn(5, 1f, 1, false));
        yield return new WaitForSeconds(1f);
        StartCoroutine(RepeatSpawn(4, 2f, 20, true));
        yield return new WaitForSeconds(20f);
        StartCoroutine(RepeatSpawn(6, 2f, 10, false));
    }

    private IEnumerator SpawnWave16()
    {
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(RepeatSpawn(8, 1.5f, 20, true));
    }

    private IEnumerator SpawnWave17()
    {
        StartCoroutine(RepeatSpawn(5, 1.5f, 1, false));
        yield return new WaitForSeconds(3f);
        StartCoroutine(RepeatSpawn(7, 2f, 20, true));
    }

    private IEnumerator SpawnWave18()
    {
        StartCoroutine(RepeatSpawn(4, 2f, 2, false));
        yield return new WaitForSeconds(4f);
        StartCoroutine(RepeatSpawn(8, 3f, 10, false));
        yield return new WaitForSeconds(30f);
        StartCoroutine(RepeatSpawn(5, 3f, 10, false));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(RepeatSpawn(9, 1.5f, 30, false));
        yield return new WaitForSeconds(48f);
        StartCoroutine(RepeatSpawn(7, 1f, 10, true));
    }

    private IEnumerator SpawnWave19()
    {
        StartCoroutine(RepeatSpawn(6, 1f, 20, false));
        StartCoroutine(RepeatSpawn(8, 1.2f, 20, false));
        yield return new WaitForSeconds(14f);
        StartCoroutine(RepeatSpawn(5, 0.5f, 2, false));
        StartCoroutine(RepeatSpawn(7, 1f, 10, true));
    }

    private IEnumerator SpawnWave20()
    {
        StartCoroutine(RepeatSpawn(9, 2f, 25, true));
        yield return new WaitForSeconds(21f);
        StartCoroutine(RepeatSpawn(14, 0.5f, 1, false));
        StartCoroutine(RepeatSpawn(5, 0.5f, 1, false));
    }

    private IEnumerator SpawnWave21()
    {
        StartCoroutine(RepeatSpawn(5, 2.5f, 10, false));
        yield return new WaitForSeconds(21f);
        StartCoroutine(RepeatSpawn(7, 1f, 50, false));
        yield return new WaitForSeconds(25f);
        StartCoroutine(RepeatSpawn(14, 0.5f, 1, false));
        StartCoroutine(RepeatSpawn(8, 0.5f, 10, true));
    }

    private IEnumerator SpawnWave22()
    {
        StartCoroutine(RepeatSpawn(7, 0.5f, 20, false));
        yield return new WaitForSeconds(4f);
        StartCoroutine(RepeatSpawn(9, 0.3f, 10, false));
        yield return new WaitForSeconds(3f);
        StartCoroutine(RepeatSpawn(6, 0.9f, 25, false));
        StartCoroutine(RepeatSpawn(8, 0.8f, 20, false));
        yield return new WaitForSeconds(3f);
        StartCoroutine(RepeatSpawn(9, 0.3f, 10, false));
        yield return new WaitForSeconds(6f);
        StartCoroutine(RepeatSpawn(9, 0.3f, 10, true));
    }

    private IEnumerator SpawnWave23()
    {
        StartCoroutine(RepeatSpawn(5, 0.3f, 5, false));
        yield return new WaitForSeconds(2f);
        StartCoroutine(RepeatSpawn(9, 0.5f, 10, false));
        yield return new WaitForSeconds(8f);
        StartCoroutine(RepeatSpawn(10, 0.5f, 1, false));
        yield return new WaitForSeconds(3f);
        StartCoroutine(RepeatSpawn(14, 2f, 2, true));
    }

    private IEnumerator SpawnWave24()
    {
        StartCoroutine(RepeatSpawn(14, 5f, 20, false));
        yield return new WaitForSeconds(3f);
        StartCoroutine(RepeatSpawn(6, 0.3f, 20, false));
        StartCoroutine(RepeatSpawn(5, 0.5f, 5, false));
        yield return new WaitForSeconds(7f);
        StartCoroutine(RepeatSpawn(5, 0.5f, 5, false));
        yield return new WaitForSeconds(3f);
        StartCoroutine(RepeatSpawn(6, 0.3f, 20, false));
        yield return new WaitForSeconds(4f);
        StartCoroutine(RepeatSpawn(5, 0.5f, 5, false));
        yield return new WaitForSeconds(6f);
        StartCoroutine(RepeatSpawn(6, 0.3f, 20, false));
        yield return new WaitForSeconds(1f);
        StartCoroutine(RepeatSpawn(5, 0.5f, 5, false));
        yield return new WaitForSeconds(7f);
        StartCoroutine(RepeatSpawn(5, 0.5f, 5, false));
        yield return new WaitForSeconds(2f);
        StartCoroutine(RepeatSpawn(6, 0.3f, 20, false));
        yield return new WaitForSeconds(10f);
        StartCoroutine(RepeatSpawn(6, 0.3f, 20, false));
        yield return new WaitForSeconds(10f);
        StartCoroutine(RepeatSpawn(6, 0.3f, 20, false));
        yield return new WaitForSeconds(10f);
        StartCoroutine(RepeatSpawn(6, 0.3f, 20, false));
        yield return new WaitForSeconds(10f);
        StartCoroutine(RepeatSpawn(6, 0.3f, 20, false));
        yield return new WaitForSeconds(10f);
        StartCoroutine(RepeatSpawn(6, 0.3f, 20, false));
        yield return new WaitForSeconds(10f);
        StartCoroutine(RepeatSpawn(6, 0.3f, 20, false));
        yield return new WaitForSeconds(10f);
        StartCoroutine(RepeatSpawn(6, 0.3f, 20, true));
    }

    private IEnumerator SpawnWave25()
    {
        StartCoroutine(RepeatSpawn(8, 0.8f, 30, false));
        yield return new WaitForSeconds(27f);
        StartCoroutine(RepeatSpawn(10, 2f, 5, false));
        yield return new WaitForSeconds(4.5f);
        StartCoroutine(RepeatSpawn(14, 4f, 5, true));
    }

    private IEnumerator SpawnWave26()
    {
        StartCoroutine(RepeatSpawn(4, 0.4f, 10, false));
        yield return new WaitForSeconds(4.5f);
        StartCoroutine(RepeatSpawn(5, 1f, 20, false));
        yield return new WaitForSeconds(28f);
        StartCoroutine(RepeatSpawn(10, 4f, 5, false));
        yield return new WaitForSeconds(24f);
        StartCoroutine(RepeatSpawn(4, 0.4f, 5, true));
    }

    private IEnumerator SpawnWave27()
    {
        StartCoroutine(RepeatSpawn(5, 1f, 20, false));
        yield return new WaitForSeconds(5f);
        StartCoroutine(RepeatSpawn(12, 1f, 1, false));
        yield return new WaitForSeconds(4f);
        StartCoroutine(RepeatSpawn(8, 0.8f, 20, true));
    }

    private IEnumerator SpawnWave28()
    {
        StartCoroutine(RepeatSpawn(8, 1f, 50, false));
        yield return new WaitForSeconds(4f);
        StartCoroutine(RepeatSpawn(14, 6f, 2, false));
        yield return new WaitForSeconds(14f);
        StartCoroutine(RepeatSpawn(12, 0.8f, 1, false));
        yield return new WaitForSeconds(2f);
        StartCoroutine(RepeatSpawn(10, 3f, 2, true));
    }

    private IEnumerator SpawnWave29()
    {
        StartCoroutine(RepeatSpawn(10, 3.5f, 20, false));
        yield return new WaitForSeconds(19f);
        StartCoroutine(RepeatSpawn(12, 1f, 1, false));
        yield return new WaitForSeconds(1f);
        StartCoroutine(RepeatSpawn(14, 4f, 5, true));
    }

    private IEnumerator SpawnWave30()
    {
        StartCoroutine(RepeatSpawn(13, 3.5f, 1, false));
        yield return new WaitForSeconds(4f);
        StartCoroutine(RepeatSpawn(6, 1f, 20, true));
    }

    private IEnumerator SpawnWave31()
    {
        StartCoroutine(RepeatSpawn(5, 0.7f, 30, false));
        yield return new WaitForSeconds(18f);
        StartCoroutine(RepeatSpawn(12, 3f, 3, true));
    }

    private IEnumerator SpawnWave32()
    {
        StartCoroutine(RepeatSpawn(6, 0.5f, 25, false));
        StartCoroutine(RepeatSpawn(8, 0.6f, 20, false));
        yield return new WaitForSeconds(11f);
        StartCoroutine(RepeatSpawn(14, 3f, 1, false));
        StartCoroutine(RepeatSpawn(12, 3f, 1, false));
        yield return new WaitForSeconds(7f);
        StartCoroutine(RepeatSpawn(6, 0.5f, 25, false));
        StartCoroutine(RepeatSpawn(8, 0.6f, 20, false));
        yield return new WaitForSeconds(12f);
        StartCoroutine(RepeatSpawn(6, 0.5f, 25, false));
        StartCoroutine(RepeatSpawn(8, 0.6f, 20, false));
        yield return new WaitForSeconds(7f);
        StartCoroutine(RepeatSpawn(14, 3f, 1, false));
        StartCoroutine(RepeatSpawn(12, 3f, 1, false));
        yield return new WaitForSeconds(11f);
        StartCoroutine(RepeatSpawn(6, 0.5f, 25, false));
        StartCoroutine(RepeatSpawn(8, 0.6f, 20, false));
        yield return new WaitForSeconds(9f);
        StartCoroutine(RepeatSpawn(14, 3f, 1, false));
        StartCoroutine(RepeatSpawn(12, 3f, 1, false));
        yield return new WaitForSeconds(9f);
        StartCoroutine(RepeatSpawn(6, 0.5f, 25, false));
        StartCoroutine(RepeatSpawn(8, 0.6f, 20, false));
        yield return new WaitForSeconds(11f);
        StartCoroutine(RepeatSpawn(14, 3f, 1, false));
        StartCoroutine(RepeatSpawn(12, 3f, 1, false));
        yield return new WaitForSeconds(7f);
        StartCoroutine(RepeatSpawn(6, 0.5f, 25, false));
        StartCoroutine(RepeatSpawn(8, 0.6f, 20, false));
        yield return new WaitForSeconds(12f);
        StartCoroutine(RepeatSpawn(6, 0.5f, 25, false));
        StartCoroutine(RepeatSpawn(8, 0.6f, 20, false));
        yield return new WaitForSeconds(8f);
        StartCoroutine(RepeatSpawn(14, 3f, 1, false));
        StartCoroutine(RepeatSpawn(12, 3f, 1, false));
        yield return new WaitForSeconds(10f);
        StartCoroutine(RepeatSpawn(6, 0.5f, 25, true));
        StartCoroutine(RepeatSpawn(8, 0.6f, 20, false));
    }

    private IEnumerator SpawnWave33()
    {
        StartCoroutine(RepeatSpawn(13, 0.7f, 1, false));
        yield return new WaitForSeconds(3f);
        StartCoroutine(RepeatSpawn(10, 2f, 10, false));
        yield return new WaitForSeconds(2f);
        StartCoroutine(RepeatSpawn(14, 10f, 2, false));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(RepeatSpawn(12, 10f, 2, true));
    }

    private IEnumerator SpawnWave34()
    {
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(RepeatSpawn(12, 3f, 10, true));
    }

    private IEnumerator SpawnWave35()
    {
        StartCoroutine(RepeatSpawn(13, 8f, 3, false));
        yield return new WaitForSeconds(2f);
        StartCoroutine(RepeatSpawn(8, 1.5f, 50, true));
    }

    private IEnumerator SpawnWave36()
    {
        StartCoroutine(RepeatSpawn(13, 1f, 1, false));
        yield return new WaitForSeconds(3f);
        StartCoroutine(RepeatSpawn(5, 1.5f, 100, false));
        yield return new WaitForSeconds(31f);
        StartCoroutine(RepeatSpawn(10, 2.5f, 20, false));
        yield return new WaitForSeconds(2f);
        StartCoroutine(RepeatSpawn(14, 7f, 3, false));
        StartCoroutine(RepeatSpawn(12, 7f, 3, true));
    }

    private IEnumerator SpawnWave37()
    {
        StartCoroutine(RepeatSpawn(10, 2f, 40, false));
        yield return new WaitForSeconds(3f);
        StartCoroutine(RepeatSpawn(14, 6f, 10, false));
        yield return new WaitForSeconds(3f);
        StartCoroutine(RepeatSpawn(12, 7f, 10, false));
        yield return new WaitForSeconds(42f);
        StartCoroutine(RepeatSpawn(13, 3f, 2, true));
    }

    private IEnumerator SpawnWave38()
    {
        StartCoroutine(RepeatSpawn(10, 1f, 30, false));
        yield return new WaitForSeconds(5f);
        StartCoroutine(RepeatSpawn(14, 3f, 2, false));
        StartCoroutine(RepeatSpawn(12, 3f, 2, false));
        yield return new WaitForSeconds(30f);
        StartCoroutine(RepeatSpawn(10, 1f, 30, false));
        yield return new WaitForSeconds(5f);
        StartCoroutine(RepeatSpawn(14, 3f, 2, false));
        StartCoroutine(RepeatSpawn(12, 3f, 2, false));
        yield return new WaitForSeconds(30f);
        StartCoroutine(RepeatSpawn(10, 1f, 30, false));
        yield return new WaitForSeconds(5f);
        StartCoroutine(RepeatSpawn(14, 3f, 2, false));
        StartCoroutine(RepeatSpawn(12, 3f, 2, false));
        yield return new WaitForSeconds(30f);
        StartCoroutine(RepeatSpawn(10, 1f, 30, false));
        yield return new WaitForSeconds(5f);
        StartCoroutine(RepeatSpawn(14, 3f, 2, false));
        StartCoroutine(RepeatSpawn(12, 3f, 2, false));
        yield return new WaitForSeconds(30f);
        StartCoroutine(RepeatSpawn(10, 1f, 30, true));
        yield return new WaitForSeconds(5f);
        StartCoroutine(RepeatSpawn(14, 3f, 2, false));
        StartCoroutine(RepeatSpawn(12, 3f, 2, false));
    }

    private IEnumerator SpawnWave39()
    {
        StartCoroutine(RepeatSpawn(13, 10f, 10, false));
        yield return new WaitForSeconds(2f);
        StartCoroutine(RepeatSpawn(14, 10f, 10, false));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(RepeatSpawn(12, 20f, 5, false));
        yield return new WaitForSeconds(105f);
        StartCoroutine(RepeatSpawn(13, 1f, 3, true));
    }

    private IEnumerator SpawnWave40()
    {
        StartCoroutine(RepeatSpawn(5, 0.8f, 50, false));
        yield return new WaitForSeconds(9f);
        StartCoroutine(RepeatSpawn(14, 1f, 10, false));
        yield return new WaitForSeconds(1f);
        StartCoroutine(RepeatSpawn(12, 5f, 2, false));
        yield return new WaitForSeconds(10f);
        StartCoroutine(RepeatSpawn(13, 4f, 3, false));
        StartCoroutine(RepeatSpawn(10, 2.8f, 75, true));
        yield return new WaitForSeconds(60f);
        StartCoroutine(RepeatSpawn(14, 4f, 5, false));
        yield return new WaitForSeconds(15f);
        StartCoroutine(RepeatSpawn(11, 1f, 1, false));
        yield return new WaitForSeconds(6f);
        StartCoroutine(RepeatSpawn(7, 1.5f, 40, false));
        yield return new WaitForSeconds(65f);
        StartCoroutine(RepeatSpawn(12, 3f, 3, false));
    }

    private IEnumerator SpawnWave40Test()
    {
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(RepeatSpawn(12, 3f, 3, true));
    }

    /*  1. Basic
        2. Fast
        3. Armoured
        4. Boss1
        5. Anchor
        6. Sprinter
        7. Randomizer
        8. Ninja
        9. Enforcer
        10. Devotee
        11. DeathBringer
        12. Overseer
        13. Dark Champion
        14. Signal Disrupter
    */

    private IEnumerator SpawnWave41()
    {
        while (true)
        {
            StartCoroutine(RepeatSpawn(11, 0.8f, 1, false));
            yield return new WaitForSeconds(1f);
            StartCoroutine(RepeatSpawn(13, 1f, 5, false));
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(RepeatSpawn(12, 1f, 5, false));
            StartCoroutine(RepeatSpawn(12, 1f, 5, false));

            yield return new WaitForSeconds(30f);
        }
    }

    private IEnumerator SpawnDefaultWave()
    {
        StartCoroutine(RepeatSpawn(4, 10f, 2, false));
        yield return new WaitForSeconds(15f);
        StartCoroutine(RepeatSpawn(5, 20f, 10, false));
        StartCoroutine(RepeatSpawn(7, 2f, 20, false));
        yield return new WaitForSeconds(40f);
        StartCoroutine(RepeatSpawn(9, 1f, 8, false));
        yield return new WaitForSeconds(8f);
        StartCoroutine(RepeatSpawn(6, 0.6f, 20, true));
    }


    // ========================= ADMIN COMMANDS =========================
    private void AdminSpawn() 
    {
        int enemyID = -1;

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            //StartCoroutine(SpawnWave41());
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            enemyID = 11;
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            enemyID = 12;
        } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            enemyID = 13;
        } else if (Input.GetKeyDown(KeyCode.Alpha5)) {
            enemyID = 14;
        } else if (Input.GetKeyDown(KeyCode.Alpha6)) {
            enemyID = 6;
        } else if (Input.GetKeyDown(KeyCode.Alpha7)) {
            enemyID = 7;
        } else if (Input.GetKeyDown(KeyCode.Alpha8)) {
            enemyID = 8;
        } else if (Input.GetKeyDown(KeyCode.Alpha9)) {
            enemyID = 9;
        }

        if (enemyID != -1) {
            SpawnEnemy(enemyID);
        }
    } 
}
/* 
Things to work on:
== Priority ==
1. Add the plasma radiator, so you can have some sort of splash damage in the game.
2. Maybe a button/key to deselect the hover tower you have.

== Everything else to do ==
2. Add more towers, and make you as the player more OP.
3. Death screen.
4. Cash bonus.
5. More scenary + sound effects - From jackie.
6. Add a non-black border to EVERYTHING: Dark grey - Zoe
*/

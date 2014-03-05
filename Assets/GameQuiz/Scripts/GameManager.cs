using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;

public class GameManager : MonoBehaviour {
    public GameObject goodEffect, badEffect, soulEffect, happyEffect;
    public Animator friendAnimator, enemyAnimator;
    public HpManager friendHpMan, enemyHpMan;

    Vector3 friendPos, enemyPos, friendHpPos, enemyHpPos, shieldPos;
    Transform friendHpGroup, enemyHpGroup, shieldGroup;

    Transform questionTf;
    Transform[] answerTfs;
    UILabel questionLabel;
    UILabel[] answerLabels;

    List<QuizData> quizList;
    int quizTotal;
    int quizIndex = 0;

    [HideInInspector]
    public int quizLength = 0;

    bool quizOn = true;

    void Awake()
    {
        Screen.SetResolution(480, 800, false); 
    }
    void Start()
    {
        InitGame();
        HideGame();
        //StartGame();
    }

    void HideGame()
    {
        ClearQuiz();
        Vector3 pos = friendPos;
        friendAnimator.transform.localPosition = new Vector3(pos.x * 3f, pos.y, pos.z);
        pos = enemyPos;
        enemyAnimator.transform.localPosition = new Vector3(pos.x * 3f, pos.y, pos.z);
        pos = friendHpPos;
        friendHpGroup.localPosition = new Vector3(pos.x * 3f, pos.y, pos.z);
        pos = enemyHpPos;
        enemyHpGroup.localPosition = new Vector3(pos.x * 3f, pos.y, pos.z);
        shieldGroup.localScale = new Vector3(2f, 2f, 1f);
        pos = shieldPos;
        shieldGroup.localPosition = new Vector3(pos.x, 0f, pos.z);
    }

    void StartGame()
    {
        IntroGame();
        DrawQuiz();
    }

    void DrawQuiz()
    {
        HideQuiz();
        StartCoroutine(DelayActoin(1f, () =>
        {
            SetQuiz();
            ShowQuiz();
        }));
    }

    void QuizInit()
    {
        quizList = new List<QuizData>();
        List<string> champs = new List<string>();
        string[,] dic = LolSkillData.dic;
        for (int i = 0; i < dic.GetLength(0); i++)
        {
            string champ = dic[i,2];
            if (!champs.Contains(champ)) champs.Add(champ);
        }
        for (int i = 0; i < dic.GetLength(0); i++)
        {
            string idx = dic[i, 0];
            string skill = dic[i, 1];
            string champ = dic[i, 2];
            QuizData quiz = new QuizData();

            int t = champs.IndexOf(champ);
            List<int> ansIdList = new List<int>();
            Hashtable ansValList = new Hashtable();
            ansValList[0] = champs[t];
            ansValList[1] = champs[(t+1)%champs.Count];
            ansValList[2] = champs[(t+2)%champs.Count];
            ansValList[3] = champs[(t+3)%champs.Count];
            for (int j = 0; j < 4; j++) ansIdList.Add(j);
            ansIdList.Shuffle();
            for (int j = 0; j < 4; j++)
                if (ansIdList[j] == 0) quiz.correct = j;
            quiz.answer1 = "1. " + ansValList[ansIdList[0]] as string;
            quiz.answer2 = "2. " + ansValList[ansIdList[1]] as string;
            quiz.answer3 = "3. " + ansValList[ansIdList[2]] as string;
            quiz.answer4 = "4. " + ansValList[ansIdList[3]] as string;
            quiz.question = skill + "?";
            quiz.id = int.Parse(idx);
            quizList.Add(quiz);
        }
        quizTotal = quizList.Count;
    }

    void InitGame()
    {
        friendHpMan.InitHp();
        enemyHpMan.InitHp();
        questionTf = GameObject.Find("Question").transform;
        questionLabel = questionTf.GetComponentInChildren<UILabel>();
        answerLabels = new UILabel[4];
        answerTfs = new Transform[4];
        int i = 0;
        foreach (Transform tf in GameObject.Find("Answers").transform)
        {
            answerTfs[i] = tf;
            answerLabels[i] = tf.GetComponentInChildren<UILabel>();
            i++;
        }
        QuizInit();

        shieldGroup = GameObject.Find("ShieldGroup").transform;
        shieldPos = shieldGroup.localPosition;
        friendPos = friendAnimator.transform.localPosition;
        enemyPos = enemyAnimator.transform.localPosition;
        friendHpGroup = friendHpMan.hpBar.transform.parent;
        enemyHpGroup = enemyHpMan.hpBar.transform.parent;
        friendHpPos = friendHpGroup.localPosition;
        enemyHpPos = enemyHpGroup.localPosition;
    }

    void IntroGame()
    {
        friendAnimator.CrossFade("Walk", 0.2f);
        enemyAnimator.CrossFade("Walk", 0.2f);
        Vector3 pos = friendPos;
        friendAnimator.transform.localPosition = new Vector3(pos.x * 3f, pos.y, pos.z);
        TweenParms parms = new TweenParms().Prop("localPosition", friendPos).Ease(EaseType.Linear).OnComplete(OnFriendStop);
        HOTween.To(friendAnimator.transform, 2f, parms);
        
        pos = enemyPos;
        enemyAnimator.transform.localPosition = new Vector3(pos.x * 3f, pos.y, pos.z);
        parms = new TweenParms().Prop("localPosition", pos).Ease(EaseType.Linear).OnComplete(OnEnemyStop);
        HOTween.To(enemyAnimator.transform, 2f, parms);

        pos = shieldPos;
        shieldGroup.localPosition = new Vector3(pos.x, 0f, pos.z);
        parms = new TweenParms().Prop("localPosition", pos).Delay(1f);
        HOTween.To(shieldGroup, 1f, parms);

        shieldGroup.localScale = new Vector3(2f, 2f, 1f);
        parms = new TweenParms().Prop("localScale", new Vector3(0.8f, 0.8f, 1f));
        HOTween.To(shieldGroup, 1f, parms);

        pos = friendHpPos;
        friendHpGroup.localPosition = new Vector3(pos.x * 3f, pos.y, pos.z);
        parms = new TweenParms().Prop("localPosition", pos).Delay(0.5f);
        HOTween.To(friendHpGroup, 1f, parms);

        pos = enemyHpPos;
        enemyHpGroup.localPosition = new Vector3(pos.x * 3f, pos.y, pos.z);
        parms = new TweenParms().Prop("localPosition", pos).Delay(0.5f);
        HOTween.To(enemyHpGroup, 1f, parms);
    }

    void OnFriendStop()
    {
        friendAnimator.CrossFade("Idle", 0.2f);
    }
    void OnEnemyStop()
    {
        enemyAnimator.CrossFade("Idle", 0.2f);
    }

    void ClearQuiz()
    {
        questionTf.localScale = new Vector3(0f, 1f, 1f);
        int i = -1;
        foreach (Transform tf in answerTfs)
        {
            tf.localPosition = new Vector3(600f * i, tf.localPosition.y, tf.localPosition.z);
            i *= -1;
        }
    }

    void HideQuiz()
    {
        TweenParms parms = new TweenParms().Prop("localScale", new Vector3(0f, 1f, 1f));
        HOTween.To(questionTf, 0.5f, parms);
        int i = -1;
        foreach (Transform tf in answerTfs)
        {
            parms = new TweenParms().Prop("localPosition", new Vector3(600f * i, tf.localPosition.y, tf.localPosition.z));
            HOTween.To(tf, 0.5f, parms);
            i *= -1;
        }
    }

    void TypeQuiz()
    {
        questionLabel.text = quizList[quizIndex].question.Substring(0, quizLength);
    }

    void ShowQuiz()
    {
        TweenParms parms = new TweenParms().Prop("localScale", new Vector3(1f, 1f, 1f));
        HOTween.To(questionTf, 0.5f, parms);
        int i = 1;
        foreach (Transform tf in answerTfs)
        {
            parms = new TweenParms().Prop("localPosition", new Vector3(0f, tf.localPosition.y, tf.localPosition.z)).Delay(0.3f * i++);
            HOTween.To(tf, 0.5f, parms);
        }
        quizOn = true;

        quizLength = 0;
        parms = new TweenParms().Prop("quizLength", quizList[quizIndex].question.Length).Ease(EaseType.Linear).OnUpdate(TypeQuiz);
        HOTween.To(this, 1f, parms);
    }

    string QuizMakeString(string str) 
    {
        return (str.Length > 41) ? str.Substring(0, 40) : str;
    }

    void SetQuiz()
    {
        quizIndex = Random.Range(0, quizTotal) % quizTotal;
        QuizData item = quizList[quizIndex];
        answerLabels[0].text = QuizMakeString(item.answer1);
        answerLabels[1].text = QuizMakeString(item.answer2);
        answerLabels[2].text = QuizMakeString(item.answer3);
        answerLabels[3].text = QuizMakeString(item.answer4);
        questionLabel.text = item.question;
    }
    
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        if (Input.GetButton("Fire1"))
        {
            //DrawQuiz();
        }
	}

    void ClickAnswer(int no)
    {
        if (!quizOn) return;
        quizOn = false;
        QuizData item = quizList[quizIndex];
        if (item.correct == no) 
        {
            Instantiate(goodEffect);
            GameObject go = Instantiate(soulEffect) as GameObject;
            go.GetComponent<SoulEffect>().posX = -1f;
            StartCoroutine(DelayActoin(0.6f, () =>
            {
                go = Instantiate(happyEffect, new Vector3(-0.7f, 1f, 0f), Quaternion.identity) as GameObject;
                enemyHpMan.DoDamageHp(10);
            }));
            friendAnimator.CrossFade("Good", 0.2f);
            enemyAnimator.CrossFade("Bad", 0.2f);
        }
        else
        {
            Instantiate(badEffect);
            GameObject go = Instantiate(soulEffect) as GameObject;
            go.GetComponent<SoulEffect>().posX = 1f;
            StartCoroutine(DelayActoin(0.6f, () =>
            {
                go = Instantiate(happyEffect, new Vector3(0.7f, 1f, 0f), Quaternion.identity) as GameObject;
                friendHpMan.DoDamageHp(10);
            }));
            friendAnimator.CrossFade("Bad", 0.2f);
            enemyAnimator.CrossFade("Good", 0.2f);
        }


        StartCoroutine(DelayActoin(3f, () =>
        {
            DrawQuiz();
        }));
    }

    public void OnClickAnswer1()
    {
        ClickAnswer(0);
    }
    public void OnClickAnswer2()
    {
        ClickAnswer(1);
    }
    public void OnClickAnswer3()
    {
        ClickAnswer(2);
    }
    public void OnClickAnswer4()
    {
        ClickAnswer(3);
    }

    public IEnumerator DelayActoin(float dtime, System.Action callback)
    {
        yield return new WaitForSeconds(dtime);
        callback();
    }
}

using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class csvReader : MonoBehaviour
{
    public string csvFileName = "quiz";
    public Dictionary<string, Quiz> quizDic = new Dictionary<string, Quiz>(); //퀴즈(문제 번호, 문제, 답)
    
    [System.Serializable]
    public class Quiz
    {
        public int quizIndex;
        public string question;
        public string answer;
    }

    private void Start()
    {
        ReadCSV();
    }

    private void ReadCSV()
    {
        bool isFinish = false;
        string path = "Quiz.csv";
        List<Quiz> menuList = new List<Quiz>();
        StreamReader reader = new StreamReader(Application.dataPath + "/" + path);

        while(isFinish == false)
        {
            string data = reader.ReadLine();
            if(data == null)
            {
                isFinish = true;
                break;
            }
            var splitData = data.Split(',');
            Quiz quiz = new Quiz();
            quiz.quizIndex = int.Parse(splitData[0]);
            quiz.question = splitData[1];
            quiz.answer = splitData[2];
            quizDic.Add(quiz.quizIndex.ToString(), quiz);
            Debug.Log(quizDic.Count);
        }
    }
}